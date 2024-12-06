using GameNetcodeStuff;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class ToolboxScript : GrabbableObject
    {
        private static Item _metalSheetItem;
        private static Item _laserPointerItem;
        private static Item _bigBoltItem;
        private PlayerControllerB previousPlayerHeldBy;
        private Object viewedTrap;
        private Turret viewedTurret;
        private Landmine viewedLandmine;
        private Coroutine coroutine;
        private int timeLapsed = 0;
        AudioSource audioSource;
        bool coroutinerunning = false;
        RaycastHit raycastHit;

        public void Awake()
        {
            GameObject audioChild = this.transform.Find("DismantleSound").gameObject;
            audioSource = audioChild.GetComponent<AudioSource>();
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            if (coroutinerunning)
            {
                audioSource.Stop();
                StopCoroutine(coroutine);
            }
        }
        public override void PocketItem()
        {
            base.PocketItem();
            if (coroutinerunning)
            {
                audioSource.Stop();
                StopCoroutine(coroutine);
            }
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown) {
                if (Physics.Raycast(playerHeldBy.transform.position + new Vector3(0, 0.5f , 0), playerHeldBy.transform.forward, out raycastHit , 5f , LayerMask.GetMask("MapHazards")))
                {
                    viewedTrap = raycastHit.transform.gameObject.transform.parent.gameObject;
                    viewedTurret = raycastHit.transform.GetComponent<Turret>();
                    viewedLandmine = raycastHit.transform.GetComponent<Landmine>();
                    previousPlayerHeldBy = playerHeldBy;
                    if (viewedTrap != null && viewedTurret != null && previousPlayerHeldBy != null|| viewedTrap != null && viewedLandmine != null && previousPlayerHeldBy != null)
                    {
                        coroutine = StartCoroutine(DismantleTrap());
                    }
                }
            }
            if (!buttonDown) {
                StopCoroutine(coroutine);
                audioSource.Stop();
                viewedTrap = null;
                viewedTurret = null;
                viewedLandmine = null;
                timeLapsed = 0;
            }
        }
        private System.Collections.IEnumerator DismantleTrap()
        {
            audioSource.Play();
            coroutinerunning = true;
            while (viewedTrap != null)
            {
                yield return new WaitForSeconds(1);
                if (Physics.Raycast(new Ray(this.playerHeldBy.gameplayCamera.transform.position, this.playerHeldBy.gameplayCamera.transform.forward), out RaycastHit raycastHit, 3f, LayerMask.GetMask("MapHazards")))
                {
                    viewedTurret = raycastHit.transform.GetComponent<Turret>();
                    viewedLandmine = raycastHit.transform.GetComponent<Landmine>();
                    timeLapsed++;

                    if (viewedTurret != null && timeLapsed == 12)
                    {
                        DismantleFunctionServerRPC();
                    }
                    else if (viewedLandmine != null && timeLapsed == 6)
                    {
                        DismantleFunctionServerRPC();
                    }
                }
                else
                {
                    audioSource.Stop();
                    viewedTrap = null;
                    viewedTurret = null;
                    viewedLandmine = null;
                    timeLapsed = 0;
                    break;
                }
            }
            audioSource.Stop();
            viewedTrap = null;
            viewedTurret = null;
            viewedLandmine = null;
            timeLapsed = 0;
            StopCoroutine(coroutine);
        }

        [ServerRpc(RequireOwnership = false)]
        public void DismantleFunctionServerRPC()
        {
            DismantleFunction();
        }
        public void DismantleFunction()
        {
            audioSource.Stop();
            Item MetalSheetItem = PullMetalSheet();
            Item LaserPointerItem = PullLaserPointer();
            Item BigBoltItem = PullBigBolt();
            GameObject LootSpawn;
            if (viewedTrap != null && playerHeldBy != null && viewedTurret)
            {
                int i;
                for (i = 0; i < 2; i++)
                {
                    float TurretLootRoll = new System.Random().Next(1, 11); 
                    if (TurretLootRoll is 1 or 2)
                    {
                        LootSpawn = Object.Instantiate<GameObject>(LaserPointerItem.spawnPrefab, viewedTurret.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity);
                        GrabbableObject component = LootSpawn.GetComponent<GrabbableObject>();
                        component.SetScrapValue(new System.Random().Next(40, 45));
                        component.NetworkObject.Spawn(false);
                    }
                    else if (TurretLootRoll is 3 or 4 or 5 or 6)
                    {
                        LootSpawn = Object.Instantiate<GameObject>(MetalSheetItem.spawnPrefab, viewedTurret.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity);
                        GrabbableObject component = LootSpawn.GetComponent<GrabbableObject>();
                        component.SetScrapValue(new System.Random().Next(20, 25));
                        component.NetworkObject.Spawn(false);
                    }
                    else if (TurretLootRoll is 7 or 8 or 9 or 10)
                    {
                        LootSpawn = Object.Instantiate<GameObject>(BigBoltItem.spawnPrefab, viewedTurret.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity);
                        GrabbableObject component = LootSpawn.GetComponent<GrabbableObject>();
                        component.SetScrapValue(new System.Random().Next(20, 25));
                        component.NetworkObject.Spawn(false);
                    }
                }
            }
            else if (viewedTrap != null && playerHeldBy != null && viewedLandmine)
            {
                float LandmineLootRoll = new System.Random().Next(1, 5);
                if (LandmineLootRoll is 1 or 2)
                {
                    LootSpawn = Object.Instantiate<GameObject>(BigBoltItem.spawnPrefab, viewedLandmine.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity);
                    GrabbableObject component = LootSpawn.GetComponent<GrabbableObject>();
                    component.SetScrapValue(new System.Random().Next(20, 25));
                    component.NetworkObject.Spawn(false);
                }
                else if (LandmineLootRoll is 3 or 4)
                {
                    LootSpawn = Object.Instantiate<GameObject>(MetalSheetItem.spawnPrefab, viewedLandmine.transform.position + new Vector3(0f, 2.5f, 0f), Quaternion.identity);
                    GrabbableObject component = LootSpawn.GetComponent<GrabbableObject>();
                    component.SetScrapValue(new System.Random().Next(20, 25));
                    component.NetworkObject.Spawn(false);
                }
            }
            Object.Destroy(viewedTrap);
        }
        public static Item PullMetalSheet()
        {
            if ((Object)(object)_metalSheetItem == (Object)null)
            {
                _metalSheetItem = StartOfRound.Instance.allItemsList.itemsList.First((Item i) => ((Object)i).name == "MetalSheet");
            }
            return _metalSheetItem;
        }
        public static Item PullLaserPointer()
        {
            if ((Object)(object)_laserPointerItem == (Object)null)
            {
                _laserPointerItem = StartOfRound.Instance.allItemsList.itemsList.First((Item i) => ((Object)i).name == "FlashLaserPointer");
            }
            return _laserPointerItem;

        }
        public static Item PullBigBolt()
        {
            if ((Object)(object)_bigBoltItem == (Object)null)
            {
                _bigBoltItem = StartOfRound.Instance.allItemsList.itemsList.First((Item i) => ((Object)i).name == "BigBolt");
            }
            return _bigBoltItem;
        }
    }
}