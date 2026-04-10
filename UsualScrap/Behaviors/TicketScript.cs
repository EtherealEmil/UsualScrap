using System;
using System.Data;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UsualScrap.Behaviors
{
    public class TicketScript : GrabbableObject
    {
        public static Item _giftBoxItem;
        Object viewedGameObject;
        GameObject WrappedPresent;
        NetworkObject viewedNetworkObject;
        GrabbableObject viewedGrabbableObject;
        GiftBoxItem component;
        RaycastHit hit;
        ParticleSystem wrapEffect;
        AudioSource[] sounds;

        bool worksOnCheapItemsConfig;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            worksOnCheapItemsConfig = (BoundConfig.TicketsFunctionOnCheapItems.Value);
            wrapEffect = GetComponentInChildren<ParticleSystem>();
            sounds = this.transform.Find("TicketSounds").gameObject.GetComponents<AudioSource>();
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
                    Vector3 vector = hit.transform.gameObject.transform.position + Vector3.up * .5f;
                    try
                    {
                        viewedGrabbableObject = hit.transform.gameObject.GetComponentInChildren<GrabbableObject>();

                        if (viewedNetworkObject.GetComponentInChildren<VehicleController>() != null || viewedNetworkObject.GetComponent<VehicleController>() != null)
                        {
                            return;
                        }
                        if (!worksOnCheapItemsConfig && viewedGrabbableObject.scrapValue <= 15)
                        {
                            return;
                        }

                        if (viewedGameObject != null && viewedNetworkObject != null && playerHeldBy != null && viewedGrabbableObject != null)
                        {
                            DestroyTargetRadarIconsServerRpc(viewedNetworkObject.NetworkObjectId);

                            DespawnItemServerRpc(viewedNetworkObject.NetworkObjectId);

                            SpawnGiftBoxServerRpc(vector);

                            DestroyThisRadarIconsServerRpc();

                            playerHeldBy.DespawnHeldObject();
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
        }

        public void SpawnGiftBox(Vector3 vector)
        {
            Item GiftBoxItem = PullGiftBox();
            WrappedPresent = Object.Instantiate(GiftBoxItem.spawnPrefab, vector, Quaternion.identity);
            GrabbableObject g = WrappedPresent.GetComponent<GrabbableObject>();
            g.startFallingPosition = vector;
            StartCoroutine(this.SetObjectToHitGroundSFX(component));
            g.targetFloorPosition = g.GetItemFloorPosition(g.transform.position);
            component = WrappedPresent.GetComponent<GiftBoxItem>();
            component.NetworkObject.Spawn();
            component.SetScrapValue(5);
            wrapEffect = Instantiate(wrapEffect, WrappedPresent.transform.position, Quaternion.identity);
            wrapEffect.Play();
            AudioSource.PlayClipAtPoint(sounds[0].clip, WrappedPresent.transform.position);
        }

        public System.Collections.IEnumerator SetObjectToHitGroundSFX(GrabbableObject Item)
        {
            yield return new WaitForEndOfFrame();
            Item.reachedFloorTarget = false;
            Item.hasHitGround = false;
            Item.fallTime = 0f;
            yield break;
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
        public void DestroyTargetRadarIconsServerRpc(ulong ID)
        {
            DestroyTargetRadarIconsClientRpc(ID);
        }
        [ClientRpc]
        public void DestroyTargetRadarIconsClientRpc(ulong ID)
        {
            try
            {
                NetworkObject netObject = NetworkManager.SpawnManager.SpawnedObjects[ID];
                var ra = netObject.GetComponentInParent<GrabbableObject>();
                if (ra.radarIcon.gameObject != null)
                {
                    Destroy(ra.radarIcon.gameObject);
                }
            }
            catch (Exception miss)
            {
                print($"US - {miss} Radar icons already missing. Skipping..");
            }
        }
        [ServerRpc]
        public void DestroyThisRadarIconsServerRpc()
        {
            DestroyThisRadarIconsClientRpc();
        }
        [ClientRpc]
        public void DestroyThisRadarIconsClientRpc()
        {
            try
            {
                if (this.radarIcon.gameObject != null)
                {
                    Destroy(this.radarIcon.gameObject);
                }
            }
            catch (Exception miss)
            {
                print($"US - {miss} Radar icons already missing. Skipping..");
            }
        }
    }
}
