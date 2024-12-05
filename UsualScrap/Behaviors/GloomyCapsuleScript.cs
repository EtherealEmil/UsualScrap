using GameNetcodeStuff;
using LethalLib.Modules;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace UsualScrap.Behaviors
{
    internal class GloomyCapsuleScript : GrabbableObject
    {
        ParticleSystem teleportParticles;
        ParticleSystem createdTeleportParticles;
        ParticleSystem chargedParticles;
        ParticleSystem chargingParticles;
        System.Random shipTeleporterSeed;
        bool activateCoroutineRunning = false;
        bool teleportCoroutineRunning = false;
        bool chargeParticlePlaying = false;
        bool chargedParticlesPlaying = false;
        int teleportChanceRoll;
        int charge = 0;
        bool disabledInShip;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            teleportParticles = this.transform.Find("TeleportParticles").GetComponent<ParticleSystem>();
            chargedParticles = this.transform.Find("ChargedParticles").GetComponent<ParticleSystem>();
            chargingParticles = this.transform.Find("ChargingParticles").GetComponent<ParticleSystem>();
            if (!StartOfRound.Instance.inShipPhase)
            {
                this.shipTeleporterSeed = new System.Random(StartOfRound.Instance.randomMapSeed + 17 + (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
            }
            disabledInShip = (BoundConfig.CapsulesDisabledOnTheShip.Value);
        }
        private void OnEnable()
        {
            StartOfRound.Instance.StartNewRoundEvent.AddListener(new UnityAction(SetTeleporterSeed));
        }

        private void SetTeleporterSeed()
        {
            this.shipTeleporterSeed = new System.Random(StartOfRound.Instance.randomMapSeed + 17 + (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
        }

        public override void Update()
        {
            base.Update();
            if (StartOfRound.Instance.inShipPhase && activateCoroutineRunning || StartOfRound.Instance.inShipPhase && teleportCoroutineRunning)
            {
                StopAllCoroutines();
                activateCoroutineRunning = false;
                teleportCoroutineRunning = false;
                chargeParticlePlaying = false;
                chargedParticlesPlaying = false;
                charge = 0;
                chargedParticles.Stop();
                chargingParticles.Stop();
            }
            else if (disabledInShip == true && this.isInShipRoom)
            {
                return;
            }
            else if (!StartOfRound.Instance.inShipPhase && !activateCoroutineRunning && !teleportCoroutineRunning && TimeOfDay.Instance.currentLevel.planetHasTime)
            {
                StartCoroutine(WaitToActivate());
            }
        }

        private System.Collections.IEnumerator WaitToActivate()
        {
            activateCoroutineRunning = true;
            if (TimeOfDay.Instance.dayMode != DayMode.Midnight || TimeOfDay.Instance.dayMode != DayMode.Sundown)
            {
                yield return new WaitUntil(() => TimeOfDay.Instance.dayMode == DayMode.Midnight || TimeOfDay.Instance.dayMode == DayMode.Sundown);
            }
            while (TimeOfDay.Instance.dayMode == DayMode.Midnight && !teleportCoroutineRunning|| TimeOfDay.Instance.dayMode == DayMode.Sundown && !teleportCoroutineRunning)
            {
                yield return new WaitForSeconds(1);
                charge++;
                //print($"{charge}");
                if (isInFactory)
                {
                    chargingParticles.Stop();
                    chargeParticlePlaying = false;
                    yield return new WaitUntil(() => !isInFactory);
                }
                if (isPocketed)
                {
                    chargingParticles.Stop();
                    chargeParticlePlaying = false;
                }
                if (charge is > 0 and < 20 && chargeParticlePlaying == false && !isPocketed)
                {
                    chargingParticles.Play();
                    chargeParticlePlaying = true;
                }
                if (charge >= 20 && !teleportCoroutineRunning)
                {
                    if (chargeParticlePlaying)
                    {
                        chargingParticles.Stop();
                        chargeParticlePlaying = false;
                    }
                    chargedParticles.Play();
                    chargedParticlesPlaying = true;
                    StartCoroutine(WaitToTeleport());
                }
            }
            activateCoroutineRunning = false;
            if (!activateCoroutineRunning && !teleportCoroutineRunning)
            {
                StartCoroutine(WaitToActivate());
            }
        }
        private System.Collections.IEnumerator WaitToTeleport()
        {
            teleportCoroutineRunning = true;
            while (charge >= 20)
            {
                if (!heldByPlayerOnServer)
                {
                    yield return new WaitUntil(() => heldByPlayerOnServer);
                }
                yield return new WaitForSeconds(1);
                if (isPocketed)
                {
                    chargedParticles.Stop();
                    chargedParticlesPlaying = false;
                }
                if (!isPocketed)
                {
                    chargedParticles.Play();
                    chargedParticlesPlaying = true;
                } 
                teleportChanceRoll = new System.Random().Next(1, 6);
                //print($"{teleportRoll}");
                if (teleportChanceRoll == 1 && heldByPlayerOnServer)
                {
                    if (playerHeldBy.isInsideFactory)
                    {
                        TeleportPlayerServerRPC(true);
                    }
                    else if (!playerHeldBy.isInsideFactory)
                    {
                        TeleportPlayerServerRPC(false);
                    }
                }
            }
            teleportCoroutineRunning = false;
            if (!activateCoroutineRunning && !teleportCoroutineRunning)
            {
                StartCoroutine(WaitToActivate());
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void TeleportPlayerServerRPC(bool indoors)
        {
            TeleportPlayerClientRPC(indoors);
        }
        [ClientRpc]
        public void TeleportPlayerClientRPC(bool isIndoors)
        {
            if (RoundManager.Instance.insideAINodes.Length != 0 && isIndoors == false)
            {
                Vector3 insideLocations = RoundManager.Instance.insideAINodes[shipTeleporterSeed.Next(0, RoundManager.Instance.insideAINodes.Length)].transform.position;
                RandomlyTeleportPlayer(insideLocations, true); 
            }
            else if (RoundManager.Instance.outsideAINodes.Length != 0 && isIndoors == true)
            {
                Vector3 outsideLocations = RoundManager.Instance.outsideAINodes[shipTeleporterSeed.Next(0, RoundManager.Instance.outsideAINodes.Length)].transform.position;
                RandomlyTeleportPlayer(outsideLocations, false);
            }
        }
        public void RandomlyTeleportPlayer(Vector3 TeleportVector, bool teleportedIndoors)
        {
            PlayerControllerB player = playerHeldBy;
            if (player != null && isHeld)
            {
                createdTeleportParticles = Instantiate(teleportParticles, player.gameObject.transform.position, Quaternion.identity);
                createdTeleportParticles.Play();
                player.averageVelocity = 0f;
                player.velocityLastFrame = Vector3.zero;
                player.DropAllHeldItems(true, false);
                player.TeleportPlayer(TeleportVector, false, 0f, false, true);
                if (teleportedIndoors)
                {
                    player.isInsideFactory = true;
                    player.isInHangarShipRoom = false;
                    player.isInElevator = false;
                }
                else
                {
                    player.isInsideFactory = false;
                    player.isInHangarShipRoom = true;
                    player.isInElevator = true;
                }
                chargedParticles.Stop();
                charge = 0;
                createdTeleportParticles = Instantiate(teleportParticles, player.gameObject.transform.position, Quaternion.identity);
                createdTeleportParticles.Play();
            }
        }

        public override void OnBroughtToShip()
        {
            base.OnBroughtToShip();
            if (disabledInShip == true)
            {
                StopAllCoroutines();
                activateCoroutineRunning = false;
                teleportCoroutineRunning = false;
                chargeParticlePlaying = false;
                chargedParticlesPlaying = false;
                charge = 0;
                chargedParticles.Stop();
                chargingParticles.Stop();
            }
        }
    }
}
