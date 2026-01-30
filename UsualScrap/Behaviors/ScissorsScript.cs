using System.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine;
using GameNetcodeStuff;
using System.Collections.Generic;
using System.Collections;
using Unity.Netcode;

namespace UsualScrap.Behaviors
{
    internal class ScissorsScript : GrabbableObject
    {
        private Coroutine coroutine;

        PlayerControllerB previousPlayerHeldBy;

        RaycastHit[] objectsHitByMeleeWeapon;
        List<RaycastHit> objectsHitByMeleeWeaponList = new List<RaycastHit>();

        AudioSource scissorAudio;
        AudioClip swing;
        AudioClip Hit;
        AudioClip snip;

        public int meleeWeaponHitForce = 2;
        bool isHoldingButton;
        bool reelingUp;
        Coroutine reelingUpCoroutine;
        public int meleeWeaponMask = 1084754248;
        public void Awake()
        {
            scissorAudio = this.transform.Find("ScissorsSounds").gameObject.GetComponent<AudioSource>();
            AudioSource[] audioSourceSounds = this.transform.Find("ScissorsSounds").gameObject.GetComponents<AudioSource>();
            snip = audioSourceSounds[0].clip;
            Hit = audioSourceSounds[1].clip;
            swing = audioSourceSounds[2].clip;
        }
        public override void GrabItem()
        {
            base.GrabItem();
            coroutine = StartCoroutine(WaitForRoll());
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            StopCoroutine(coroutine);
            if (this.playerHeldBy != null)
            {
                this.playerHeldBy.activatingItem = false;
            }
        }
        System.Collections.IEnumerator WaitForRoll()
        {
            for (; ; )
            {
                if (playerHeldBy.isSprinting)
                {
                    RollForDamage();
                }
                yield return new WaitForSeconds(1);
            }
        }
        public async void RollForDamage()
        {
            int damageroll = new System.Random().Next(1, 4);
            //print($"{damageroll}");
            if (damageroll == 1)
            {
                await Task.Delay(TimeSpan.FromSeconds(.2));
                if (isHeld)
                {
                    PlaySound();
                    playerHeldBy.DamagePlayer(15, true, true, CauseOfDeath.Snipped, 7, false, default(Vector3));
                }
                await Task.Delay(TimeSpan.FromSeconds(.2));
                if (isHeld)
                {
                    PlaySound();
                    playerHeldBy.DamagePlayer(15, true, true, CauseOfDeath.Snipped, 7, false, default(Vector3));
                }
            }
        }

        public void PlaySound()
        {
            scissorAudio.pitch = UnityEngine.Random.Range(.8f, 1);
            scissorAudio.PlayOneShot(snip);
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
                scissorAudio.PlayOneShot(swing);
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
                //previousPlayerHeldBy.twoHanded = false;
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
                                    scissorAudio.PlayOneShot(StartOfRound.Instance.footstepSurfaces[j].hitSurfaceSFX);
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
                            print($"Error hitting object with scissors {miss}, caused by playerInital: {previousPlayerHeldBy.playerClientId}");
                        }
                    }
                IL_361:;
                }
            }
            if (flag)
            {
                UnityEngine.Object.FindObjectOfType<RoundManager>().PlayAudibleNoise(base.transform.position, 17f, 0.8f, 0, false, 0);
                if (num != -1)
                {
                    scissorAudio.PlayOneShot(Hit);
                    WalkieTalkie.TransmitOneShotAudio(scissorAudio, Hit);
                }
                playerHeldBy.playerBodyAnimator.SetTrigger("shovelHit");
                HitMeleeWeaponServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void HitMeleeWeaponServerRpc()
        {
            HitMeleeWeaponClientRpc();
        }
        [ClientRpc]
        public void HitMeleeWeaponClientRpc()
        {
            HitSurfaceWithMeleeWeapon();
        }
        private void HitSurfaceWithMeleeWeapon()
        {
            if (!IsOwner)
            {
                scissorAudio.PlayOneShot(Hit);
            }
        }
    }
}
