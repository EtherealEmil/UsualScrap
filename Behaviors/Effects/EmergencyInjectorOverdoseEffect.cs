using GameNetcodeStuff;
using UnityEngine;

namespace UsualScrap.Behaviors.OvertimeEffects
{
    internal class EmergencyInjectorOverdoseEffect : MonoBehaviour
    {
        int duration = 5;
        PlayerControllerB playerUsedBy;
        
        public void Awake()
        {
            playerUsedBy = GetComponent<PlayerControllerB>();
            StartCoroutine(EmergencyInjectorOverdose());
        }
        private System.Collections.IEnumerator EmergencyInjectorOverdose()
        {
            while (duration > 0)
            {
                yield return new WaitForSeconds(1f);
                playerUsedBy.DamagePlayer(5,false, true ,CauseOfDeath.Suffocation);
                duration--;
            }
            Destroy(this);
        }
    }
}
