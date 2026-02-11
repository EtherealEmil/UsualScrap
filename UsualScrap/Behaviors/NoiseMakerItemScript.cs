using UnityEngine;

namespace UsualScrap.Behaviors
{
    public class NoiseMakerItemScript : GrabbableObject
    {
        AudioSource[] sources;
        AudioClip clip;
        public void Awake()
        {
            this.useCooldown = 2;
            sources = GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in sources)
            {
                if (source.clip != null)
                {
                    clip = source.clip;
                }
            }
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            this.GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1.10f);
            this.GetComponent<AudioSource>().PlayOneShot(clip);
            RoundManager.Instance.PlayAudibleNoise(base.transform.position, 16f, 0.9f, 0, false, 0);
        }
    }
}

