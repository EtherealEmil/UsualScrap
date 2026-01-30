using GameNetcodeStuff;

namespace UsualScrap.Behaviors
{
    internal class RoseScript : GrabbableObject
    {
        PlayerControllerB player;
        public override void EquipItem()
        {
            base.EquipItem();
            player = playerHeldBy;
            player.DamagePlayer(5);
        }
        public override void PocketItem()
        {
            base.PocketItem();
            player.DamagePlayer(5);
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            player = null;
        }
    }
}
