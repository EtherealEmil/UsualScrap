using System.Threading.Tasks;
using System;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class ScissorsScript : GrabbableObject
    {
        private Coroutine coroutine;
        private AudioSource audioSource;

        public void Awake()
        {
            GameObject audioChild = this.transform.Find("ScissorsSnip").gameObject;
            audioSource = audioChild.GetComponent<AudioSource>();
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
            audioSource.pitch = UnityEngine.Random.Range(1, 1.2f);
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
