using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    public class MedicalKitScript : GrabbableObject
    {
        int Healthpool = 150;
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
            if (!replenishCoroutineRunning && !healCoroutineRunning && Healthpool < 150)
            {
                replenishCoroutine = StartCoroutine(ReplenishKitRoutine());
            }
            if (healCoroutineRunning && replenishCoroutineRunning)
            {
                StopCoroutine(replenishCoroutine);
                replenishCoroutineRunning = false;
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
                print("US - Medical Kit playerheldby returned null.");
                return;
            }
            if (buttonDown)
            {
                HealServerRpc();
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
        public void HealServerRpc()
        {
            if (Physics.SphereCast(this.playerHeldBy.gameplayCamera.transform.position, .5f, this.playerHeldBy.gameplayCamera.transform.forward, out RaycastHit hit, 3f, LayerMask.GetMask("Player")))
            {
                PlayerControllerB g = hit.transform.gameObject.GetComponent<PlayerControllerB>();
                int f = (int)g.playerClientId;
                healCoroutine = StartCoroutine(HealPlayer(f, .15f));
            }
            else
            {
                healCoroutine = StartCoroutine(HealPlayer((int)playerHeldBy.playerClientId, .2f));
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
                    float num = Vector3.Distance(player.transform.position, playerHeldBy.transform.position);
                    if (num > 3)
                    {
                        //print("US - Medical Kit out of range.");
                        break;
                    }
                }
                HealClientRpc(playerID);
                SubtractHealpoolServerRpc();
            }
            healCoroutineRunning = false;
        }
        [ServerRpc(RequireOwnership = false)]
        public void SubtractHealpoolServerRpc()
        {
            SubtractHealpoolClientRpc();
        }
        [ClientRpc]
        public void SubtractHealpoolClientRpc()
        {
            Healthpool--;
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
                Effects(playerID);
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
                    if (GameNetworkManager.Instance.localPlayerController == player)
                    {
                        HUDManager.Instance.UpdateHealthUI(player.health, false);
                    }
                }
            }
        }
        public void Effects(int playerID)
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
            ParticleSystem Healparticle = Instantiate(particle, player2.transform.position, Quaternion.identity, player2.transform);
            Healparticle.Play();
        }
        private System.Collections.IEnumerator ReplenishKitRoutine()
        {
            replenishCoroutineRunning = true;
            while (Healthpool < 150)
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
                Healthpool = 150;
            }
            if (Healthpool < 150)
            {
                Healthpool++;
            }
            //print($"{Healthpool}");
        }
    }

}

