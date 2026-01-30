using GameNetcodeStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class CandyDispenserScript : GrabbableObject
    {
        RaycastHit[] objectsHitByMeleeWeapon;
        List<RaycastHit> objectsHitByMeleeWeaponList = new List<RaycastHit>();

        bool isHoldingButton;
        bool reelingUp;
        Coroutine reelingUpCoroutine;
        PlayerControllerB previousPlayerHeldBy;

        private static Item _candyItem;
        GameObject dispenserBase;
        MeshRenderer dispenserMesh;

        int meleeWeaponMask = 1084754248;
        int meleeWeaponHitForce = 1;
        AudioSource dispenserAudio;
        AudioClip swing;
        AudioClip Dispense;
        AudioClip[] hitSFX;
        Color setColor;

        public void Awake()
        {
            dispenserAudio = this.transform.Find("CandyDispenserSounds").gameObject.GetComponent<AudioSource>();
            AudioSource[] audioSourceSounds = this.transform.Find("CandyDispenserSounds").gameObject.GetComponents<AudioSource>();
            swing = audioSourceSounds[1].clip;
            Dispense = audioSourceSounds[2].clip;
            hitSFX = new AudioClip[1] { dispenserAudio.clip };
        }
        public override void DiscardItem()
        {
            if (this.playerHeldBy != null)
            {
                this.playerHeldBy.activatingItem = false;
            }
            base.DiscardItem();
        }
        public override void Start()
        {
            base.Start();
            SetColorServerRPC();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetColorServerRPC()
        {
            //float ColorRRoll = new System.Random().Next(2, 11);
            //float ColorGRoll = new System.Random().Next(2, 11);
            //float ColorBRoll = new System.Random().Next(2, 11);

            //float r = .9f / ColorRRoll;
            //float g = .9f / ColorGRoll;
            //float b = .9f / ColorBRoll;

            //setColor = new Color(r, g, b, 1f);

            byte ColorRRoll = (byte)new System.Random().Next(1, 256);
            byte ColorGRoll = (byte)new System.Random().Next(1, 256);
            byte ColorBRoll = (byte)new System.Random().Next(1, 256);


            setColor = new Color32(ColorRRoll, ColorGRoll, ColorBRoll, 255);

            SetColorClientRPC(setColor);
        }
        [ClientRpc]
        public void SetColorClientRPC(Color color)
        {
            dispenserBase = this.transform.Find("CandyDispenserModel").gameObject.transform.Find("DispenserBase").gameObject;
            dispenserMesh = dispenserBase.GetComponent<MeshRenderer>();
            dispenserMesh.material.color = color;
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            if (playerHeldBy == null)
            {
                return;
            }
            isHoldingButton = buttonDown;
            if (this.reelingUp)
            {
                return;
            }
            if (!reelingUp && buttonDown)
            {
                reelingUp = true;
                previousPlayerHeldBy = playerHeldBy;
                if (reelingUpCoroutine != null)
                {
                    StopCoroutine(reelingUpCoroutine);
                }
                reelingUpCoroutine = StartCoroutine(reelUpMeleeWeapon());
            }
        }
        private IEnumerator reelUpMeleeWeapon()
        {
            this.playerHeldBy.activatingItem = true;
            this.playerHeldBy.twoHanded = true;
            this.playerHeldBy.playerBodyAnimator.ResetTrigger("shovelHit");
            this.playerHeldBy.playerBodyAnimator.SetBool("reelingUp", true);
            yield return new WaitForSeconds(0.35f);
            yield return new WaitUntil(() => !this.isHoldingButton || !this.isHeld);
            this.SwingMeleeWeapon(!this.isHeld);
            yield return new WaitForSeconds(0.13f);
            yield return new WaitForEndOfFrame();
            this.HitMeleeWeapon(!this.isHeld);
            yield return new WaitForSeconds(0.3f);
            this.reelingUp = false;
            this.reelingUpCoroutine = null;
            yield break;
        }
        public void SwingMeleeWeapon(bool cancel = false)
        {
            previousPlayerHeldBy.playerBodyAnimator.SetBool("reelingUp", false);
            if (!cancel)
            {
                dispenserAudio.PlayOneShot(swing);
                previousPlayerHeldBy.UpdateSpecialAnimationValue(true, (short)previousPlayerHeldBy.transform.localEulerAngles.y, 0.4f, false);
            }
        }
        public void HitMeleeWeapon(bool cancel = false)
        {
            if (previousPlayerHeldBy == null)
            {
                return;
            }
            previousPlayerHeldBy.activatingItem = false;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            int num = -1;
            if (!cancel)
            {
                previousPlayerHeldBy.twoHanded = false;
                objectsHitByMeleeWeapon = Physics.SphereCastAll(previousPlayerHeldBy.gameplayCamera.transform.position + previousPlayerHeldBy.gameplayCamera.transform.right * -0.35f, 0.8f, previousPlayerHeldBy.gameplayCamera.transform.forward, 1.5f, meleeWeaponMask, QueryTriggerInteraction.Collide);
                objectsHitByMeleeWeaponList = (from x in objectsHitByMeleeWeapon orderby x.distance select x).ToList<RaycastHit>();
                List<EnemyAI> list = new List<EnemyAI>();
                for (int i = 0; i < objectsHitByMeleeWeaponList.Count; i++)
                {
                    IHittable hittable;
                    RaycastHit raycastHit;
                    if (objectsHitByMeleeWeaponList[i].transform.gameObject.layer == 8 || objectsHitByMeleeWeaponList[i].transform.gameObject.layer == 11)
                    {
                        if (!objectsHitByMeleeWeaponList[i].collider.isTrigger)
                        {
                            flag = true;
                            string tag = objectsHitByMeleeWeaponList[i].collider.gameObject.tag;
                            for (int j = 0; j < StartOfRound.Instance.footstepSurfaces.Length; j++)
                            {
                                if (StartOfRound.Instance.footstepSurfaces[j].surfaceTag == tag)
                                {
                                    num = j;
                                    dispenserAudio.PlayOneShot(StartOfRound.Instance.footstepSurfaces[j].hitSurfaceSFX);
                                    break;
                                }
                            }
                        }
                    }
                    else if (objectsHitByMeleeWeaponList[i].transform.TryGetComponent<IHittable>(out hittable) && !(objectsHitByMeleeWeaponList[i].transform == previousPlayerHeldBy.transform) && (objectsHitByMeleeWeaponList[i].point == Vector3.zero || !Physics.Linecast(previousPlayerHeldBy.gameplayCamera.transform.position, objectsHitByMeleeWeaponList[i].point, out raycastHit, StartOfRound.Instance.collidersAndRoomMaskAndDefault, QueryTriggerInteraction.Ignore)))
                    {
                        flag = true;
                        Vector3 forward = previousPlayerHeldBy.gameplayCamera.transform.forward;
                        try
                        {
                            EnemyAICollisionDetect component = this.objectsHitByMeleeWeaponList[i].transform.GetComponent<EnemyAICollisionDetect>();
                            if (component != null)
                            {
                                if (component.mainScript == null || list.Contains(component.mainScript))
                                {
                                    goto IL_361;
                                }
                            }
                            else if (this.objectsHitByMeleeWeaponList[i].transform.GetComponent<PlayerControllerB>() != null)
                            {
                                if (flag3)
                                {
                                    goto IL_361;
                                }
                                flag3 = true;
                            }
                            bool HitCheck = hittable.Hit(meleeWeaponHitForce, forward, previousPlayerHeldBy, true, 1);
                            if (HitCheck && component != null)
                            {
                                list.Add(component.mainScript);
                            }
                            if (!flag2)
                            {
                                flag2 = HitCheck;
                            }
                        }
                        catch (Exception miss)
                        {
                            print($"Error hitting object with Candy Dispenser {miss}, caused by playerInital: {previousPlayerHeldBy.playerClientId}");
                        }
                    }
                IL_361:;
                }
                float RandomRoll = new System.Random().Next(1, 25);
                if (RandomRoll == 1)
                {
                    if (StartOfRound.Instance.inShipPhase || !StartOfRound.Instance.shipHasLanded || !TimeOfDay.Instance.currentLevel.planetHasTime)
                    {
                        print("US - Candy spawning disabled here, land at a moon with a time cycle!");
                        return;
                    }
                    dispenserAudio.PlayOneShot(Dispense);
                    SpawnScrapServerRpc();
                }

            }
            if (flag)
            {
                var sound = RoundManager.PlayRandomClip(this.dispenserAudio, this.hitSFX);
                UnityEngine.Object.FindObjectOfType<RoundManager>().PlayAudibleNoise(base.transform.position, 17f, 0.8f, 0, false, 0);
                if (num != -1)
                {
                    dispenserAudio.PlayOneShot(hitSFX[sound]);
                    WalkieTalkie.TransmitOneShotAudio(dispenserAudio, hitSFX[sound]);
                }
                playerHeldBy.playerBodyAnimator.SetTrigger("shovelHit");
                HitMeleeWeaponServerRpc(sound);
            }
        }
        [ServerRpc]
        public void HitMeleeWeaponServerRpc(int sound)
        {
            HitMeleeWeaponClientRpc(sound);
        }
        [ClientRpc]
        public void HitMeleeWeaponClientRpc(int sound)
        {
            HitSurfaceWithMeleeWeapon(sound);
        }
        private void HitSurfaceWithMeleeWeapon(int sound)
        {
            if (!IsOwner)
            {
                dispenserAudio.PlayOneShot(hitSFX[sound]);
            }
            WalkieTalkie.TransmitOneShotAudio(dispenserAudio, hitSFX[sound]);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void SpawnScrapServerRpc()
        {
            SpawnCandy();
        }
        public void SpawnCandy()
        {
            Item candyItem = PullCandyItem();
            GameObject LootSpawn;

            LootSpawn = UnityEngine.Object.Instantiate<GameObject>(candyItem.spawnPrefab, playerHeldBy.transform.position + Vector3.up, Quaternion.identity);
            GrabbableObject PieceofCandyObject = LootSpawn.GetComponent<GrabbableObject>();
            PieceofCandyObject.fallTime = 0f;
            PieceofCandyObject.NetworkObject.Spawn(false);
            PieceofCandyObject.SetScrapValue(6);

            if (this.previousPlayerHeldBy != null && this.previousPlayerHeldBy.isInHangarShipRoom)
            {
                this.previousPlayerHeldBy.SetItemInElevator(true, true, LootSpawn.GetComponent<GrabbableObject>());
            }

        }

        public static Item PullCandyItem()
        {
            if ((UnityEngine.Object)(object)_candyItem == (UnityEngine.Object)null)
            {
                _candyItem = StartOfRound.Instance.allItemsList.itemsList.First((Item i) => ((UnityEngine.Object)i).name == "US_PieceofCandyItem");
            }
            return _candyItem;
        }
    }
}

