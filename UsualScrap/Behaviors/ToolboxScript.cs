using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class ToolboxScript : GrabbableObject
    {
        private static Item _laserPointerItem;
        private static Item _bigBoltItem;
        private Object viewedTrapObject;
        private Turret viewedTurret;
        private Landmine viewedLandmine;
        private Coroutine coroutine;
        private int timeLapsed = 0;
        private int timeToSuccess;
        bool coroutinerunning = false;
        Vector3 trap;

        AudioSource[] toolboxAudio;
        AudioSource toolboxSound;
        AudioClip Smash;
        ParticleSystem sparkEffect;

        public void Awake()
        {
            try
            {
                toolboxSound = this.transform.Find("ToolboxSounds").gameObject.GetComponent<AudioSource>();
                toolboxAudio = this.transform.Find("ToolboxSounds").gameObject.GetComponents<AudioSource>();
                Smash = toolboxAudio[0].clip;
                sparkEffect = this.transform.Find("SparkEffect").GetComponent<ParticleSystem>();
            }
            catch
            {
                if (toolboxAudio == null)
                {
                    print("ERROR: US_Toolbox audio not found.");
                }
                if (sparkEffect == null)
                {
                    print("ERROR: US_Toolbox effect not found.");
                }
            }
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            if (coroutinerunning)
            {
                StopCoroutine(coroutine);
                coroutinerunning = false;
            }
            viewedTrapObject = null;
            viewedTurret = null;
            viewedLandmine = null;
            timeLapsed = 0;
        }
        public override void PocketItem()
        {
            base.PocketItem();
            if (coroutinerunning)
            {
                StopCoroutine(coroutine);
                coroutinerunning = false;
            }
            viewedTrapObject = null;
            viewedTurret = null;
            viewedLandmine = null;
            timeLapsed = 0;
        } 
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown) {
                print("Starting");
                Collider[] detectedtrapsArray = Physics.OverlapSphere(this.transform.position, 3, LayerMask.GetMask("MapHazards"), QueryTriggerInteraction.Collide);
                if (detectedtrapsArray.Length > 0)
                {
                    foreach (Collider collider in detectedtrapsArray)
                    {
                        if (collider.transform.GetComponent<Turret>() != null || collider.transform.GetComponent<Landmine>() != null || collider.transform.gameObject.GetComponent<SpikeRoofTrap>() != null)
                        {
                            //print("US - Toolbox initializing trap!");
                            viewedTrapObject = collider.transform.gameObject.transform.parent.gameObject;
                            viewedTurret = collider.transform.GetComponent<Turret>();
                            viewedLandmine = collider.transform.GetComponent<Landmine>();
                            trap = collider.transform.position;
                            if (viewedTurret != null && viewedTrapObject != null && !coroutinerunning)
                            {
                                coroutine = StartCoroutine(DismantleTrap("Turret"));
                            }
                            else if (viewedLandmine != null && viewedTrapObject != null && !coroutinerunning)
                            {
                                coroutine = StartCoroutine(DismantleTrap("Landmine"));
                            }
                        }
                    }
                }
            }
            if (!buttonDown) {
                if (coroutinerunning)
                {
                    StopCoroutine(coroutine);
                    coroutinerunning = false;
                }
                viewedTrapObject = null;
                viewedTurret = null;
                viewedLandmine = null;
                timeLapsed = 0;
            }
        }
        private System.Collections.IEnumerator DismantleTrap(string trapType)
        {
            coroutinerunning = true;
            timeLapsed = 0;
            Vector3 playerPosition = playerHeldBy.transform.position;
            if (viewedTurret != null)
            {
                timeToSuccess = 10;
                //print("Turret detected");
            }
            else if (viewedLandmine != null)
            {
                timeToSuccess = 5;
                //print("Landmine detected");
            }
            //print(timeToSuccess);
            while (viewedTrapObject != null)
            { 
                yield return new WaitForSeconds(.8f);
                sparkEffect = Instantiate(sparkEffect, trap, Quaternion.identity);
                sparkEffect.Play();
                toolboxSound.pitch = Random.Range(.4f, .6f);
                toolboxSound.PlayOneShot(Smash);
                Vector3 newPlayerPosition = playerHeldBy.transform.position;
                if (newPlayerPosition != playerPosition)
                {
                    viewedTrapObject = null;
                    viewedTurret = null;
                    viewedLandmine = null;
                    timeLapsed = 0;
                    break;
                }
                else if (timeLapsed == timeToSuccess)
                {
                    //print("US - Starting dismantle function!");
                    DismantleFunctionServerRPC(trapType);
                    viewedTrapObject = null;
                    viewedTurret = null;
                    viewedLandmine = null;
                    timeLapsed = 0;
                    break;
                }
                else
                {
                    timeLapsed++;
                    print(timeLapsed);
                }
            }
            coroutinerunning = false;
        }

        [ServerRpc(RequireOwnership = false)]
        public void DismantleFunctionServerRPC(string trapType)
        {
            print("US - Running spawn dismantled scrap method!");
            Item LaserPointerItem = PullLaserPointer();
            Item BigBoltItem = PullBigBolt();
            GameObject LootSpawn;
            Vector3 spawnVector;
            Collider[] detectedtrapsArray = Physics.OverlapSphere(this.transform.position, 3, LayerMask.GetMask("MapHazards"), QueryTriggerInteraction.Collide);
            if (detectedtrapsArray.Length > 0)
            {
                foreach (Collider collider in detectedtrapsArray)
                {
                    viewedTrapObject = collider.transform.gameObject.transform.parent.gameObject;
                    if (trapType == "Turret" && collider.transform.GetComponent<Turret>() != null)
                    {
                        viewedTurret = collider.transform.GetComponent<Turret>();
                        for (int i = new System.Random().Next(1, 3); i > 0; i--)
                        {
                            float TurretLootRoll = new System.Random().Next(1, 11);
                            spawnVector = viewedTurret.transform.position + Vector3.up * 0.25f;
                            if (TurretLootRoll is 1)
                            {
                                LootSpawn = Object.Instantiate<GameObject>(LaserPointerItem.spawnPrefab, spawnVector, Quaternion.identity);
                                GrabbableObject component = LootSpawn.GetComponent<GrabbableObject>();
                                component.startFallingPosition = spawnVector;
                                StartCoroutine(SetObjectToHitGroundSFX(component));
                                component.targetFloorPosition = component.GetItemFloorPosition(viewedTurret.transform.position); ;
                                component.SetScrapValue(new System.Random().Next(35, 50));
                                component.NetworkObject.Spawn(false);
                            }
                            else if (TurretLootRoll is > 1 and <= 10)
                            {
                                LootSpawn = Object.Instantiate<GameObject>(BigBoltItem.spawnPrefab, spawnVector, Quaternion.identity);
                                GrabbableObject component = LootSpawn.GetComponent<GrabbableObject>();
                                component.startFallingPosition = spawnVector;
                                StartCoroutine(SetObjectToHitGroundSFX(component));
                                component.targetFloorPosition = component.GetItemFloorPosition(viewedTurret.transform.position); ;
                                component.SetScrapValue(new System.Random().Next(15, 25));
                                component.NetworkObject.Spawn(false);
                            }
                        }
                    }
                    else if (trapType == "Landmine" && collider.transform.GetComponent<Landmine>() != null)
                    {
                        viewedLandmine = collider.transform.GetComponent<Landmine>();
                        spawnVector = viewedLandmine.transform.position;
                        LootSpawn = Object.Instantiate<GameObject>(BigBoltItem.spawnPrefab, spawnVector, Quaternion.identity);
                        GrabbableObject component = LootSpawn.GetComponent<GrabbableObject>();
                        component.startFallingPosition = spawnVector;
                        StartCoroutine(SetObjectToHitGroundSFX(component));
                        component.targetFloorPosition = component.GetItemFloorPosition(viewedLandmine.transform.position); ;
                        component.SetScrapValue(new System.Random().Next(15, 25));
                        component.NetworkObject.Spawn(false);
                    }
                }
            }
            DismantleFunctionClientRPC(trapType);
        }
        [ClientRpc]
        public void DismantleFunctionClientRPC(string trapType)
        {
            DismantleFunction(trapType);
        }
        public void DismantleFunction(string trapType)
        {
            print("US - Running destroy trap method!");
            Collider[] detectedtrapsArray = Physics.OverlapSphere(this.transform.position, 3, LayerMask.GetMask("MapHazards"), QueryTriggerInteraction.Collide);
            if (detectedtrapsArray.Length > 0)
            {
                foreach (Collider collider in detectedtrapsArray)
                {
                    viewedTrapObject = collider.transform.gameObject.transform.parent.gameObject;
                    if (trapType == "Turret" && collider.transform.GetComponent<Turret>() != null)
                    {
                        Object.Destroy(viewedTrapObject);
                    }
                    else if (trapType == "Landmine" && collider.transform.GetComponent<Landmine>() != null)
                    {
                        Object.Destroy(viewedTrapObject);
                    }
                }
            }
        }
        private System.Collections.IEnumerator SetObjectToHitGroundSFX(GrabbableObject lootObject)
        {
            yield return new WaitForEndOfFrame();
            lootObject.reachedFloorTarget = false;
            lootObject.hasHitGround = false;
            lootObject.fallTime = 0f;
            yield break;
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