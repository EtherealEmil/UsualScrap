using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using Color = UnityEngine.Color;

namespace UsualScrap.Behaviors.OvertimeEffects
{
    internal class EmergencyInjectorEffect : MonoBehaviour
    {
        int buffDuration = 60;
        int debuffDuration = 30;
        readonly float speedStrength = 1.5f;
        readonly float slowStrength = 1f;
        bool speedBoostActive = false;
        bool speedDebuffActive = false;
        PlayerControllerB playerUsedBy;

        public void Awake()
        {
            playerUsedBy = GetComponent<PlayerControllerB>();
            StartCoroutine(AdrenalineRush());
        }
        private System.Collections.IEnumerator AdrenalineRush()
        {
            speedBoostActive = true;
            playerUsedBy.sprintTime = playerUsedBy.sprintTime + 5;
            playerUsedBy.sprintMeter = 1;
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed + speedStrength;
            while (buffDuration > 0)
            { 
                yield return new WaitForSeconds(1f);
                buffDuration--;
            }
            playerUsedBy.sprintTime = playerUsedBy.sprintTime - 5;
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed - speedStrength;
            speedBoostActive = false;
            StartCoroutine(AdrenalineCrash());
        }
        private System.Collections.IEnumerator AdrenalineCrash()
        {
            speedDebuffActive = true;
            playerUsedBy.sprintTime = playerUsedBy.sprintTime - 2;
            playerUsedBy.sprintMeter = 0;
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed - slowStrength;
            while (debuffDuration > 0)
            {
                yield return new WaitForSeconds(1f);
                debuffDuration--;
            }
            playerUsedBy.movementSpeed = playerUsedBy.movementSpeed + slowStrength;
            playerUsedBy.sprintTime = playerUsedBy.sprintTime + 2;
            speedDebuffActive = false;
            Destroy(this);
        }
    }
}
