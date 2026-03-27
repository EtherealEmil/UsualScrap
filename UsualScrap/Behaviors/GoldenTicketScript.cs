using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UsualScrap.Behaviors
{
    public class GoldenTicketScript : GrabbableObject
    {
        ParticleSystem idleSparkle;
        bool particlePlaying = true;
        public static Item _giftBoxItem;
        GameObject WrappedPresent;
        ScanNodeProperties scanNodeProperties;
        GiftBoxItem component;

        bool worksOnCheapItemsConfig;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        public void Awake()
        {
            idleSparkle = GetComponentInChildren<ParticleSystem>();
            BoundConfig = Plugin.BoundConfig;
            worksOnCheapItemsConfig = (BoundConfig.TicketsFunctionOnCheapItems.Value);
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
            if (!particlePlaying)
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
                CheckForItems();
            }
        }
        public void CheckForItems()
        {
            bool presentsCreated = false;
            int numberOfPresents = 0;
            Collider[] itemArray = Physics.OverlapSphere(this.transform.position, 5, LayerMask.GetMask("Props"));
            foreach (Collider itemCollider in itemArray)
            {
                if (numberOfPresents == 5)
                {
                    break;
                }
                try
                {
                    Object viewedGameObject = itemCollider.transform.gameObject;
                    NetworkObject viewedNetworkObject = itemCollider.transform.gameObject.GetComponent<NetworkObject>();
                    Vector3 viewVector = itemCollider.transform.gameObject.transform.position;
                    GrabbableObject viewedGrabbableObject = itemCollider.transform.gameObject.GetComponentInChildren<GrabbableObject>();

                    if (viewedNetworkObject.GetComponentInChildren<VehicleController>() != null || viewedNetworkObject.GetComponent<VehicleController>() != null || viewedNetworkObject.GetComponent<GiftBoxItem>() != null || !worksOnCheapItemsConfig && viewedGrabbableObject.scrapValue <= 5)
                    {
                        continue;
                    }
                    if (viewedGameObject != null && viewedNetworkObject != null && playerHeldBy != null && viewedGrabbableObject != null)
                    {
                        RemoveTargetRadarIconsServerRpc(viewedNetworkObject.NetworkObjectId);

                        DespawnItemServerRpc(viewedNetworkObject.NetworkObjectId);

                        SpawnGiftBoxServerRpc(viewVector);

                        presentsCreated = true;
                        numberOfPresents++;

                    }
                }
                catch (Exception)
                {
                    //...
                }
            }
             if (presentsCreated == true)
            {
                if (particlePlaying)
                {
                    particlePlaying = false;
                    idleSparkle.Stop();
                }
                RemoveThisRadarIconsServerRpc();
                playerHeldBy.DespawnHeldObject();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnGiftBoxServerRpc(Vector3 vector)
        {
            SpawnGiftBox(vector);
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
        public void RemoveThisRadarIconsServerRpc()
        {
            RemoveThisRadarIconsClientRpc();
        }

        [ClientRpc]
        public void RemoveThisRadarIconsClientRpc()
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

        [ServerRpc]
        public void RemoveTargetRadarIconsServerRpc(ulong ID)
        {
            RemoveTargetRadarIconsClientRpc(ID);
        }

        [ClientRpc]
        public void RemoveTargetRadarIconsClientRpc(ulong ID)
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
    }
}

