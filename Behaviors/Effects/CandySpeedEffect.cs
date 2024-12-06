using GameNetcodeStuff;
using UnityEngine;

namespace UsualScrap.Behaviors.Effects
{
    internal class CandySpeedEffect : MonoBehaviour
    {
        int speedDuration = 30;
        PlayerControllerB playerUsedBy;
        float speedStrength = .5f;
        public void Awake()
        {
            playerUsedBy = GetComponent<PlayerControllerB>();
            if (playerUsedBy.health < 90)
            {
                playerUsedBy.health = playerUsedBy.health + 10;
            }
            else if (playerUsedBy.health >= 90 && playerUsedBy.health < 100)
            {
                playerUsedBy.health = playerUsedBy.health + (100 - playerUsedBy.health);
            }
            StartCoroutine(SpeedEffect());
        }
        private System.Collections.IEnumerator SpeedEffect()
        {
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed + speedStrength;
            while (speedDuration > 0)
            {
                yield return new WaitForSeconds(1);
                speedDuration--;
            }
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed - speedStrength;
            Destroy(this);
        }
    }
}
