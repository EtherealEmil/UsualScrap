using GameNetcodeStuff;
using UnityEngine;

namespace UsualScrap.Behaviors.Effects
{
    internal class RandomCandyEffect : MonoBehaviour
    {
        int randomRoll = new System.Random().Next(1, 3);
        int Duration = 10;
        PlayerControllerB playerUsedBy;
        float speedStrength = 2f;
        float jumpStrength = 6f;
        public void Awake()
        {
            playerUsedBy = GetComponent<PlayerControllerB>();
            if (randomRoll == 1)
            {
                StartCoroutine(SpeedEffect());
            }
            else if (randomRoll == 2)
            {
                StartCoroutine(JumpEffect());
            }

        }
        private System.Collections.IEnumerator SpeedEffect()
        {
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed + speedStrength;
            while (Duration > 0)
            {
                yield return new WaitForSeconds(1);
                Duration--;
            }
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed - speedStrength;
            Destroy(this);
        }
        private System.Collections.IEnumerator JumpEffect()
        {
            playerUsedBy.jumpForce = playerUsedBy.jumpForce + jumpStrength;
            while (Duration > 0)
            {
                yield return new WaitForSeconds(1);
                Duration--;
            }
            playerUsedBy.jumpForce = playerUsedBy.jumpForce - jumpStrength;
            Destroy(this);
        }
    }
}
