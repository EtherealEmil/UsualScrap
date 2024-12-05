using Unity.Netcode;
using UsualScrap.Behaviors.Effects;

namespace UsualScrap.Behaviors
{
    internal class CandyScript : GrabbableObject
    {
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown)
            {
                playerHeldBy.gameObject.AddComponent<CandySpeedEffect>();
                this.playerHeldBy.DespawnHeldObject();
            }
        }
        [ServerRpc]
        public void DestroyRadarIconServerRpc()
        {
            DestroyRadarIconClientRpc();
        }
        [ClientRpc]
        public void DestroyRadarIconClientRpc()
        {
            Destroy(this.radarIcon.gameObject);
        }
    }
}
