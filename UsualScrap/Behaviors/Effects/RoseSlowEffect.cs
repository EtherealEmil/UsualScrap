using GameNetcodeStuff;
using UnityEngine;

namespace UsualScrap.Behaviors.Effects
{
    internal class RoseSlowEffect : MonoBehaviour
    {
        float slowDuration = 5;
        PlayerControllerB playerUsedBy;
        float slowStrength = .5f;
        public void Awake()
        {
            playerUsedBy = GetComponent<PlayerControllerB>();
            StartCoroutine(SlowEffect());
        }
        private System.Collections.IEnumerator SlowEffect()
        {
            if (playerUsedBy.movementSpeed <= slowStrength)
            {
                Destroy(this);
            }
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed - slowStrength;
            while (slowDuration > 0)
            {
                yield return new WaitForSeconds(1);
                slowDuration--;
            }
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed + slowStrength;
            Destroy(this);
        }
    }
}
