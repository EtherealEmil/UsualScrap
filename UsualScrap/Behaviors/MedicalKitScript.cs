using GameNetcodeStuff;
using LethalLib.Modules;
using System.Numerics;
using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class MedicalKitScript : GrabbableObject
    {
        int Healthpool = 100;
        private Coroutine healCoroutine;
        private Coroutine replenishCoroutine;
        private ParticleSystem particle;
        bool healCoroutineRunning = false;
        bool replenishCoroutineRunning = false;

        public void Awake()
        {
            particle = GetComponentInChildren<ParticleSystem>();
        } 
        public override void LateUpdate()
        {
            base.LateUpdate();
            if (!replenishCoroutineRunning && !healCoroutineRunning && Healthpool < 100)
            {
                replenishCoroutine = StartCoroutine(ReplenishKitRoutine());
            }
            if (healCoroutineRunning && replenishCoroutineRunning)
            {
                StopCoroutine(replenishCoroutine);
            }
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            StopHealRoutineServerRpc();
        }
        public override void PocketItem()
        {
            base.PocketItem();
            StopHealRoutineServerRpc();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (this.playerHeldBy == null)
            {
                print("Medical Kit Playerheldby == null but is being used??");
                return;
            }
            if (buttonDown)
            {
                StartHealRoutineServerRpc();
            }
            if (!buttonDown)
            {
                StopHealRoutineServerRpc();
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void StopHealRoutineServerRpc()
        {
            StopHealRoutineClientRpc();
        }
        [ClientRpc]
        public void StopHealRoutineClientRpc()
        {
            if (healCoroutineRunning)
            {
                StopCoroutine(healCoroutine);
                healCoroutineRunning = false;
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void StartHealRoutineServerRpc()
        {
            if (Physics.SphereCast(this.playerHeldBy.gameplayCamera.transform.position, .5f ,this.playerHeldBy.gameplayCamera.transform.forward, out RaycastHit hit, 3f, LayerMask.GetMask("Player")))
            {
                PlayerControllerB g = hit.transform.gameObject.GetComponent<PlayerControllerB>();
                int f = (int)g.playerClientId;
                healCoroutine = StartCoroutine(HealPlayer(f, .6f));
            }
            else
            {
                healCoroutine = StartCoroutine(HealPlayer((int)playerHeldBy.playerClientId, .3f)); 
            }
        }
        private System.Collections.IEnumerator HealPlayer(int playerID, float rateOfHealing)
        {
            healCoroutineRunning = true;
            PlayerControllerB player;
            if (RoundManager.Instance.playersManager.allPlayerScripts[playerID] != playerHeldBy)
            {
                player = RoundManager.Instance.playersManager.allPlayerScripts[playerID];
            }
            else
            {
                player = playerHeldBy;
            }
            while (player.health < 100 && Healthpool > 0)
            {
                yield return new WaitForSeconds(rateOfHealing);
                if (player != playerHeldBy)
                {
                    float num = UnityEngine.Vector3.Distance(player.transform.position, playerHeldBy.transform.position);
                    if (num > 3)
                    {
                        print("Out of range");
                        break;
                    }
                }
                HealServerRpc(playerID);
            }
            healCoroutineRunning = false;
        }
        [ServerRpc(RequireOwnership = false)]
        public void HealServerRpc(int playerID)
        {
            HealClientRpc(playerID);
        }
        [ClientRpc]
        public void HealClientRpc(int playerID)
        {
            Heal(playerID);
        }
        public void Heal(int playerID)
        {
            PlayerControllerB player;
            if (RoundManager.Instance.playersManager.allPlayerScripts[playerID] != playerHeldBy)
            {
                player = RoundManager.Instance.playersManager.allPlayerScripts[playerID];
            }
            else
            {
                player = playerHeldBy;
            }
            if (isHeld && player.health < 100 && Healthpool > 0)
            {
                EffectsServerRpc(playerID);
                player.health = player.health + 1;
                if (player.health > 20)
                {
                    if (player.criticallyInjured || player.bleedingHeavily)
                    {
                        player.criticallyInjured = false;
                        player.bleedingHeavily = false;
                    }
                    if (player.playerBodyAnimator != null)
                    {
                        player.playerBodyAnimator.SetBool("Limp", false);
                    }
                    HUDManager.Instance.UpdateHealthUI(player.health, false);
                    //^^^ Eventually update health only for player being healed if at all possible. 
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void EffectsServerRpc(int playerID)
        {
            EffectsClientRpc(playerID);
        }
        [ClientRpc]
        public void EffectsClientRpc(int playerID)
        {
            PlayerControllerB player2;
            if (RoundManager.Instance.playersManager.allPlayerScripts[playerID] != playerHeldBy)
            {
                player2 = RoundManager.Instance.playersManager.allPlayerScripts[playerID];
            }
            else
            {
                player2 = playerHeldBy;
            }
            ParticleSystem Healparticle = Instantiate(particle, player2.transform.position, UnityEngine.Quaternion.identity);
            Healparticle.Play();
            Healthpool--;
        }
        private System.Collections.IEnumerator ReplenishKitRoutine()
        {
            replenishCoroutineRunning = true;
            while (Healthpool < 100)
            {
                yield return new WaitForSeconds(1.5f);
                ReplenishKitServerRpc();
                if (healCoroutineRunning)
                {
                    break;
                }
            }
            replenishCoroutineRunning = false;
        }
        [ServerRpc(RequireOwnership = false)]
        public void ReplenishKitServerRpc()
        {
            ReplenishKitClientRpc();
        }
        [ClientRpc]
        public void ReplenishKitClientRpc()
        {
            ReplenishKit();
        }

        public void ReplenishKit()
        {
            if (StartOfRound.Instance.inShipPhase)
            {
                Healthpool = 100;
            }
            if (Healthpool < 100)
            {
                Healthpool++;
            }
            //print($"{Healthpool}");
        }
    }
}
