using GameNetcodeStuff;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace UsualScrap.Behaviors.Effects
{
    class SnowstormStackingSlowEffect : MonoBehaviour
    {
        public ParticleSystem frostedParticles;
        ParticleSystem particle;
        Component[] frostStacks;
        public int stackNumber;
        float effectDuration = 15;
        PlayerControllerB playerUsedBy;
        float slowStrength;
        float burdenStrength;
        float animationSlowStrength;
        public void Awake()
        {
            playerUsedBy = GetComponent<PlayerControllerB>();
            slowStrength = playerUsedBy.movementSpeed / 8;
            burdenStrength = playerUsedBy.jumpForce / 10;
            animationSlowStrength = playerUsedBy.playerBodyAnimator.speed / 10;
            if (playerUsedBy.movementSpeed <= slowStrength || playerUsedBy.jumpForce <= burdenStrength || playerUsedBy.playerBodyAnimator.speed <= animationSlowStrength)
            {
                Destroy(this);
            }
            StartCoroutine(SlowEffect());
        }
        private System.Collections.IEnumerator SlowEffect()
        {
            yield return new WaitUntil(() => frostedParticles != null);
            if (stackNumber == 1)
            {
                particle = Instantiate(frostedParticles, playerUsedBy.transform.position, Quaternion.identity, playerUsedBy.transform);
                particle.Play();
            }
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed - slowStrength;
            playerUsedBy.jumpForce = playerUsedBy.jumpForce - burdenStrength;
            playerUsedBy.playerBodyAnimator.speed = playerUsedBy.playerBodyAnimator.speed - animationSlowStrength;
            while (effectDuration > 0)
            {
                yield return new WaitForSeconds(1);
                effectDuration--;
                if (playerUsedBy.isPlayerDead)
                {
                    effectDuration = 0;
                }
            }
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed + slowStrength;
            playerUsedBy.jumpForce = playerUsedBy.jumpForce + burdenStrength;
            playerUsedBy.playerBodyAnimator.speed = playerUsedBy.playerBodyAnimator.speed + animationSlowStrength;

            if (stackNumber == 1)
            {
                frostStacks = playerUsedBy.gameObject.GetComponents<SnowstormStackingSlowEffect>();
                while (frostStacks.Length > 1)
                {
                    yield return new WaitForSeconds(1);
                    frostStacks = playerUsedBy.gameObject.GetComponents<SnowstormStackingSlowEffect>();
                }
                particle.Stop();
            }
            Destroy(this);
        }
    }
}
