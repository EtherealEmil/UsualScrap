using GameNetcodeStuff;
using UnityEngine;

namespace UsualScrap.Behaviors.Effects
{
    class StackingSlowEffect : MonoBehaviour
    {
        PlayerControllerB player;
        public ParticleSystem frostedParticles;
        ParticleSystem particle;
        float slowDebuff = .3f;
        float effectDuration = 20;
        public void Awake()
        {
            player = GetComponent<PlayerControllerB>();
            StartCoroutine(SlowEffect());
        }
        private System.Collections.IEnumerator SlowEffect()
        {
            yield return new WaitUntil(() => frostedParticles != null);
            player = this.GetComponent<PlayerControllerB>();
            CheckAndApplySlow(true);
            particle = Instantiate(frostedParticles, player.transform.position, Quaternion.identity, player.transform);
            particle.Play();
            while (effectDuration > 0)
            {
                effectDuration--;
                yield return new WaitForSeconds(1);
                if (StartOfRound.Instance.inShipPhase || !StartOfRound.Instance.shipHasLanded || player.enteringSpecialAnimation)
                {
                    effectDuration = 0;
                }
            }
            CheckAndApplySlow(false);
            Destroy(this);
        }
        public void CheckAndApplySlow(bool apply)
        {
            if (apply)
            {
                player.movementSpeed -= slowDebuff;
                player.jumpForce -= slowDebuff * 3;
                player.playerBodyAnimator.speed -= .05f;
            }
            else if (!apply)
            {
                player.movementSpeed += slowDebuff;
                player.jumpForce += slowDebuff * 3;
                player.playerBodyAnimator.speed += .05f;
            }
        }
    }
}
