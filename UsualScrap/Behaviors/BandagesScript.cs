using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class BandagesScript : GrabbableObject
    {
        public NetworkVariable<int> savedUses = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
        int uses = 3;
        ParticleSystem particle;
        
        
        public override int GetItemDataToSave()
        {
            base.GetItemDataToSave();
            return savedUses.Value;
        }

        public override void LoadItemSaveData(int saveData)
        {
            base.LoadItemSaveData(saveData);
            savedUses.Value = saveData;
        }
        

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
            if (uses > 1 && playerHeldBy.health < 70)
            {
                playerHeldBy.health = playerHeldBy.health + 30;
                uses--;
            }
            else if (uses > 1 && playerHeldBy.health >= 70 && playerHeldBy.health < 100)
            {
                playerHeldBy.health = playerHeldBy.health + (100 - playerHeldBy.health);
                uses--;
            }
            else if (uses <= 1 && playerHeldBy.health < 70)
            {
                playerHeldBy.health = playerHeldBy.health + 30;
                if (this.isHeld)
                {
                    playerHeldBy.DespawnHeldObject();
                }
            }
            else if (uses <= 1 && playerHeldBy.health >= 70 && playerHeldBy.health < 100)
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
                if (GameNetworkManager.Instance.localPlayerController == playerHeldBy)
                {
                    HUDManager.Instance.UpdateHealthUI(playerHeldBy.health, false);
                }
            }
        }

    }
}
