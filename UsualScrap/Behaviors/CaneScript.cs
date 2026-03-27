namespace UsualScrap.Behaviors
{
    public class CaneScript : GrabbableObject
    {
        float movementbuff = 1f;

        public override void EquipItem()
        {
            base.EquipItem();
            playerHeldBy.movementSpeed = playerHeldBy.movementSpeed + movementbuff;
        }
        public override void PocketItem()
        {
            base.PocketItem();
            playerHeldBy.movementSpeed = playerHeldBy.movementSpeed - movementbuff;
        }
        public override void DiscardItem()
        {
            playerHeldBy.movementSpeed = playerHeldBy.movementSpeed - movementbuff;
            base.DiscardItem();
        }
    }
}

