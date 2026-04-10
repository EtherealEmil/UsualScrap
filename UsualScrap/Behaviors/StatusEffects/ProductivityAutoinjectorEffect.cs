using GameNetcodeStuff;
using System;
using UnityEngine;

namespace UsualScrap.Behaviors.Effects
{
    public class ProductivityAutoinjectorEffect : MonoBehaviour
    {
        int buffDuration = 10;
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
            StartOfRound.Instance.SendChangedWeightEvent();
            playerUsedBy.jumpForce = playerUsedBy.jumpForce + speedStrength / 2;
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed + speedStrength;
            while (buffDuration > 0)
            {
                yield return new WaitForSeconds(.5f);
                //print(playerUsedBy.carryWeight);
                playerUsedBy.carryWeight = 1f;
                StartOfRound.Instance.SendChangedWeightEvent();
                buffDuration--;
                if (playerUsedBy.isPlayerDead)
                {
                    buffDuration = 0;
                }
            }
            playerUsedBy.jumpForce = playerUsedBy.jumpForce - speedStrength / 2;
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed - speedStrength;
            foreach (GrabbableObject fl in playerUsedBy.ItemSlots)
            {
                if (fl != null)
                {
                    playerUsedBy.carryWeight =  Math.Clamp(playerUsedBy.carryWeight + (fl.itemProperties.weight - 1), 1,10);
                    StartOfRound.Instance.SendChangedWeightEvent();
                    //print($"{fl.itemProperties.weight} is the item carry weight");
                }
            }
            Destroy(this);
        }
    }
}
