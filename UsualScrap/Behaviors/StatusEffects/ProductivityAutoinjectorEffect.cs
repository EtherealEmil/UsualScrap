using GameNetcodeStuff;
using UnityEngine;

namespace UsualScrap.Behaviors.Effects
{
    internal class ProductivityAutoinjectorEffect : MonoBehaviour
    {
        int buffDuration = 180;
        float speedStrength = 1f;
        PlayerControllerB playerUsedBy;

        public void Awake()
        {
            playerUsedBy = GetComponent<PlayerControllerB>();
            StartCoroutine(Boost());
        }
        private System.Collections.IEnumerator Boost()
        {
            playerUsedBy.carryWeight = 1f;
            playerUsedBy.jumpForce = playerUsedBy.jumpForce + speedStrength / 2;
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed + speedStrength;
            while (buffDuration > 0)
            {
                yield return new WaitForSeconds(1f);
                playerUsedBy.carryWeight = 1f;
                buffDuration--;
                if (playerUsedBy.isPlayerDead)
                {
                    buffDuration = 0;
                }
            }
            playerUsedBy.carryWeight = 1f;
            playerUsedBy.jumpForce = playerUsedBy.jumpForce - speedStrength / 2;
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed - speedStrength;
            float setWeight = 1f;
            foreach (GrabbableObject fl in playerUsedBy.ItemSlots)
            {
                if (fl != null)
                {
                    print(fl.itemProperties.weight);
                    print((fl.itemProperties.weight - 1) * 100);
                    setWeight = setWeight + (fl.itemProperties.weight - 1);
                }
            }
            playerUsedBy.carryWeight = setWeight;
            Destroy(this);
        }
    }
}
