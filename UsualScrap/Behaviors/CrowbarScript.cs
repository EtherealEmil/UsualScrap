using GameNetcodeStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class CrowbarScript : GrabbableObject
    {
        PlayerControllerB previousPlayerHeldBy;
        DoorLock viewedDoorLock;
        int hitsToOpen = new System.Random().Next(2, 3);

        AudioSource crowbarAudio;
        AudioClip swing;
        AudioClip[] hitSFX;

        public int meleeWeaponHitForce = 1;
        bool isHoldingButton;
        bool reelingUp;
        Coroutine reelingUpCoroutine;
        RaycastHit[] objectsHitByMeleeWeapon;
        List<RaycastHit> objectsHitByMeleeWeaponList = new List<RaycastHit>();
        public int meleeWeaponMask = 1084754248;

        public void Awake()
        {
            crowbarAudio = this.transform.Find("CrowbarSounds").gameObject.GetComponent<AudioSource>();
            AudioSource[] audioSourceSounds = this.transform.Find("CrowbarSounds").gameObject.GetComponents<AudioSource>();
            swing = audioSourceSounds[1].clip;
            hitSFX = new AudioClip[1] {crowbarAudio.clip};
        }
        public override void DiscardItem()
        {
            if (this.playerHeldBy != null)
            {
                this.playerHeldBy.activatingItem = false;
            }
            base.DiscardItem();
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            if (this.playerHeldBy == null)
            {
                return;
            }
            this.isHoldingButton = buttonDown;
            if (this.reelingUp)
            {
                return;
            }
            if (buttonDown)
            {
                this.reelingUp = true;
                this.previousPlayerHeldBy = this.playerHeldBy;
                if (this.reelingUpCoroutine != null)
                {
                    base.StopCoroutine(this.reelingUpCoroutine);
                }
                this.reelingUpCoroutine = base.StartCoroutine(this.reelUpMeleeWeapon());
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
                crowbarAudio.PlayOneShot(swing);
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
                                    crowbarAudio.PlayOneShot(StartOfRound.Instance.footstepSurfaces[j].hitSurfaceSFX);
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
                            print($"Error hitting object with crowbar {miss}, caused by playerInital: {previousPlayerHeldBy.playerClientId}");
                        }
                    }
                IL_361:;
                }
                if (Physics.Raycast(new Ray(this.playerHeldBy.gameplayCamera.transform.position, this.playerHeldBy.gameplayCamera.transform.forward), out RaycastHit doorRay, 3f, LayerMask.GetMask("InteractableObject")))
                {
                    viewedDoorLock = doorRay.transform.GetComponent<DoorLock>();
                    if (viewedDoorLock != null && viewedDoorLock.isLocked && hitsToOpen > 0)
                    {
                        hitsToOpen--;
                    }
                    else if (viewedDoorLock != null && viewedDoorLock.isLocked && hitsToOpen == 0)
                    {
                        UnlockDoorServerRpc();
                        OpenDoorServerRpc();
                        hitsToOpen = new System.Random().Next(2, 3);
                    }
                    if (viewedDoorLock != null && !viewedDoorLock.isLocked)
                    {
                        OpenDoorServerRpc();
                    }
                }
            }
            if (flag)
            {
                var sound  = RoundManager.PlayRandomClip(this.crowbarAudio, this.hitSFX);
                UnityEngine.Object.FindObjectOfType<RoundManager>().PlayAudibleNoise(base.transform.position, 17f, 0.8f, 0, false, 0);
                if (num != -1)
                {
                    crowbarAudio.PlayOneShot(hitSFX[sound]);
                    WalkieTalkie.TransmitOneShotAudio(crowbarAudio, hitSFX[sound]);
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
                if (sound != -1)
                {
                    crowbarAudio.PlayOneShot(hitSFX[sound]);
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void OpenDoorServerRpc()
        {
            OpenDoorClientRpc();
        }
        [ClientRpc]
        public void OpenDoorClientRpc()
        {
            try
            {
                viewedDoorLock.OpenOrCloseDoor(playerHeldBy);
                viewedDoorLock.SetDoorAsOpen(true);
            }
            catch (Exception)
            {
                //...
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void UnlockDoorServerRpc()
        {
            UnlockDoorClientRpc();
        }
        [ClientRpc]
        public void UnlockDoorClientRpc()
        {
            viewedDoorLock.UnlockDoor();
        }
    }
}

