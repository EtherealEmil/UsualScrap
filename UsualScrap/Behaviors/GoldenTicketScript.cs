using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UsualScrap.Behaviors
{
    internal class GoldenTicketScript : GrabbableObject
    {
        ParticleSystem idleSparkle;
        bool particlePlaying = true;
        public static Item _giftBoxItem;
        Object viewedGameObject;
        GameObject WrappedPresent;
        GrabbableObject viewedGrabbableObject;
        ScanNodeProperties scanNodeProperties;
        NetworkObject viewedNetworkObject;
        GiftBoxItem component;
        RaycastHit hit;
        int uses = 5;

        public void Awake()
        {
            idleSparkle = GetComponentInChildren<ParticleSystem>();
        }
        public override void PocketItem()
        {
            base.PocketItem();
            if (particlePlaying)
            {
                particlePlaying = false;
                idleSparkle.Stop();
            }
        }
        public override void EquipItem()
        {
            base.EquipItem();
            if(!particlePlaying)
            {
                particlePlaying = true;
                idleSparkle.Play();
            }
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            if (!particlePlaying)
            {
                particlePlaying = true;
                idleSparkle.Play();
            }
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown)
            {
                if (Physics.Raycast(new Ray(this.playerHeldBy.gameplayCamera.transform.position, this.playerHeldBy.gameplayCamera.transform.forward), out hit, 5f, LayerMask.GetMask("Props")))
                {
                    viewedGameObject = hit.transform.gameObject;
                    viewedNetworkObject = hit.transform.gameObject.GetComponent<NetworkObject>();
                    Vector3 vector = hit.transform.gameObject.transform.position;

                    try
                    {
                        viewedGrabbableObject = hit.transform.gameObject.GetComponentInChildren<GrabbableObject>();

                        if (viewedNetworkObject.GetComponentInChildren<VehicleController>() != null || viewedNetworkObject.GetComponent<VehicleController>() != null)
                        {
                            return;
                        }

                        if (viewedGameObject != null && viewedNetworkObject != null && playerHeldBy != null && viewedGrabbableObject != null)
                        {
                            SpawnGiftBoxServerRpc(vector);

                            DestroyRadarIconsServerRpc(viewedNetworkObject.NetworkObjectId);

                            DespawnItemServerRpc(viewedNetworkObject.NetworkObjectId);

                            if (uses == 0)
                            {
                                playerHeldBy.DespawnHeldObject();
                            }
                        }
                    }
                    catch
                    {
                        return;
                    }

                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnGiftBoxServerRpc(Vector3 vector)
        {
            SpawnGiftBox(vector);
            SpawnGiftBoxClientRpc();
        }
        [ClientRpc]
        public void SpawnGiftBoxClientRpc()
        {
            uses--;
        }
        public void SpawnGiftBox(Vector3 vector)
        {
            Item GiftBoxItem = PullGiftBox();
            WrappedPresent = Object.Instantiate(GiftBoxItem.spawnPrefab, vector + Vector3.up * .25f, Quaternion.identity);
            component = WrappedPresent.GetComponent<GiftBoxItem>();
            component.NetworkObject.Spawn();
            component.SetScrapValue(5);
        }

        public static Item PullGiftBox()
        {
            if ((Object)(object)_giftBoxItem == (Object)null)
            {
                _giftBoxItem = StartOfRound.Instance.allItemsList.itemsList.First((Item i) => ((Object)i).name == "GiftBox");
            }
            return _giftBoxItem;
        }

        [ServerRpc]
        public void DespawnItemServerRpc(ulong ID)
        {
            var netObject = NetworkManager.SpawnManager.SpawnedObjects[ID];
            netObject.Despawn(true);
        }
        [ServerRpc]
        public void DestroyRadarIconsServerRpc(ulong ID)
        {
            DestroyRadarIconsClientRpc(ID);
        }
        [ClientRpc]
        public void DestroyRadarIconsClientRpc(ulong ID)
        {
            idleSparkle.Stop();
            try
            {
                NetworkObject netObject = NetworkManager.SpawnManager.SpawnedObjects[ID];
                var ra = netObject.GetComponentInParent<GrabbableObject>();
                if (ra.radarIcon.gameObject != null)
                {
                    Destroy(ra.radarIcon.gameObject);
                }
                if (this.radarIcon.gameObject != null)
                {
                    Destroy(this.radarIcon.gameObject);
                }
            }
            catch (Exception miss)
            {
                print($"{miss} Radar icons already missing. Skipping..");
            }
        }
    }
}
