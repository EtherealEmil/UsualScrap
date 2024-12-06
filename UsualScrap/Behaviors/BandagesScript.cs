using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class BandagesScript : GrabbableObject
    {
        int uses = 3;
        ParticleSystem particle;

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown && playerHeldBy.health < 100)
            {
                HealServerRpc();
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void HealServerRpc()
        {
            HealClientRpc();
        }
        [ClientRpc]
        public void HealClientRpc()
        {
            Heal();
        }
        public void Heal()
        {
            particle = GetComponentInChildren<ParticleSystem>();
            ParticleSystem Healparticle = Instantiate(particle, playerHeldBy.transform.position, Quaternion.identity, playerHeldBy.transform);
            Healparticle.Play();
            if (uses > 1 && playerHeldBy.health < 80)
            {
                playerHeldBy.health = playerHeldBy.health + 20;
                uses--;
            }
            else if (uses > 1 && playerHeldBy.health >= 80 && playerHeldBy.health < 100)
            {
                playerHeldBy.health = playerHeldBy.health + (100 - playerHeldBy.health);
                uses--;
            }
            else if (uses <= 1 && playerHeldBy.health < 80)
            {
                playerHeldBy.health = playerHeldBy.health + 20;
                if (this.isHeld)
                {
                    playerHeldBy.DespawnHeldObject();
                }
            }
            else if (uses <= 1 && playerHeldBy.health >= 80 && playerHeldBy.health < 100)
            {
                playerHeldBy.health = playerHeldBy.health + (100 - playerHeldBy.health);
                if (this.isHeld)
                {
                    playerHeldBy.DespawnHeldObject();
                }
            }
            if (playerHeldBy.health > 20)
            {
                if (playerHeldBy.criticallyInjured || playerHeldBy.bleedingHeavily)
                {
                    playerHeldBy.criticallyInjured = false;
                    playerHeldBy.bleedingHeavily = false;
                }
                if (playerHeldBy.playerBodyAnimator != null)
                {
                    playerHeldBy.playerBodyAnimator.SetBool("Limp", false);
                }
                HUDManager.Instance.UpdateHealthUI(playerHeldBy.health, false);
            }
        }
    }
}
