using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class DoomsayerBellScript : GrabbableObject
    {
        AudioSource[] audioSource;
        AudioClip clip;
        public void Awake()
        {
            audioSource = this.transform.Find("DoomsayerBellSounds").gameObject.GetComponents<AudioSource>();
            clip = audioSource[0].clip;

            this.useCooldown = 2;

        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            ringBell();
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            ringBell();
        }

        public void ringBell()
        {
            this.GetComponent<AudioSource>().pitch = Random.Range(0.50f, 1f);
            this.GetComponent<AudioSource>().PlayOneShot(clip);
            RoundManager.Instance.PlayAudibleNoise(base.transform.position, 16f, 0.9f, 0, false, 0);
            if (!this.isInShipRoom && StartOfRound.Instance.shipHasLanded && TimeOfDay.Instance.currentLevel.planetHasTime)
            {
                int roll = new System.Random().Next(1, 5);
                if (roll == 1)
                {
                    TimeOfDay.Instance.globalTime = TimeOfDay.Instance.globalTime + 60;
                }
            }
        }
    }
}
