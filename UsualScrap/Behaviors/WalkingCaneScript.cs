using GameNetcodeStuff;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class WalkingCaneScript : GrabbableObject
    {
        private PlayerControllerB Player;
        readonly float speedBuff = 1.5f;
        bool speedCoroutineRunning = false;

        public override void EquipItem()
        {
            base.EquipItem();
            //print("equipped");
            if (speedCoroutineRunning == false)
            {
                //print("starting routine");
                this.isHeld = true;
                StartCoroutine(ApplySpeedBuff());
            }
        }
        private System.Collections.IEnumerator ApplySpeedBuff()
        {
            speedCoroutineRunning = true;
            //print("routine started");
            Player = playerHeldBy;
            Player.movementSpeed = Player.movementSpeed + speedBuff;
            while (this.isHeld)
            {
                yield return new WaitForSeconds(.5f);
            }
            //print("routine ended");
            Player.movementSpeed = Player.movementSpeed - speedBuff;
            speedCoroutineRunning = false;
        }
    }
}
