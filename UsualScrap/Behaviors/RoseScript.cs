using UsualScrap.Behaviors.Effects;

namespace UsualScrap.Behaviors
{
    internal class RoseScript : GrabbableObject
    {
        public override void EquipItem()
        {
            base.EquipItem();
            playerHeldBy.DamagePlayer(5);
            playerHeldBy.gameObject.AddComponent<RoseSlowEffect>();
        }
        public override void PocketItem()
        {
            base.PocketItem();
            playerHeldBy.DamagePlayer(5);
            playerHeldBy.gameObject.AddComponent<RoseSlowEffect>();
        }
    }
}
