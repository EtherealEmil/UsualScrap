using GameNetcodeStuff;
using System.Threading.Tasks;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace UsualScrap.Behaviors
{
    internal class GloomyCapsuleScript : GrabbableObject
    {
        ParticleSystem teleportParticles;
        ParticleSystem createdTeleportParticles;
        ParticleSystem createdPocketedParticles;
        ParticleSystem chargingParticles;
        ParticleSystem PocketedParticles;
        ParticleSystem AmbientParticles;
        System.Random shipTeleporterSeed;
        Light light;
        bool activateCoroutineRunning = false;
        bool teleportCoroutineRunning = false;
        bool chargingParticlePlaying = false;
        bool pocketedParticlesPlaying = false;
        int teleportChanceRoll;
        int charge = 0;
        bool disabledInShip;
        bool ambientParticlesPlaying = true;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            teleportParticles = this.transform.Find("TeleportParticles").GetComponent<ParticleSystem>();
            chargingParticles = this.transform.Find("ChargingParticles").GetComponent<ParticleSystem>();
            PocketedParticles = this.transform.Find("PocketedParticles").GetComponent<ParticleSystem>();
            AmbientParticles = this.transform.Find("AmbientParticles").GetComponent<ParticleSystem>();
            light = this.transform.Find("Point Light").GetComponent<Light>();
            if (!StartOfRound.Instance.inShipPhase)
            {
                this.shipTeleporterSeed = new System.Random(StartOfRound.Instance.randomMapSeed + 17 + (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
            }
            disabledInShip = (BoundConfig.CapsulesDisabledOnTheShip.Value);
        }
        public override void PocketItem()
        {
            base.PocketItem();
            if (ambientParticlesPlaying)
            {
                AmbientParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ambientParticlesPlaying = false;
            }
            if (light.enabled)
            {
                light.enabled = false;
            }
        }
        public override void EquipItem()
        {
            base.EquipItem();
            if (!ambientParticlesPlaying)
            {
                AmbientParticles.Play();
                ambientParticlesPlaying = true;
            }
            if (!light.enabled)
            {
                light.enabled = true;
            }
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            if (!ambientParticlesPlaying)
            {
                AmbientParticles.Play();
                ambientParticlesPlaying = true;
            }
            if (!light.enabled)
            {
                light.enabled = true;
            }
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
                chargingParticlePlaying = false;
                pocketedParticlesPlaying = false;
                charge = 0;
                PocketedParticles.Stop();
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
                    chargingParticlePlaying = false;
                    yield return new WaitUntil(() => !isInFactory);
                }
                if (isPocketed)
                {
                    createdPocketedParticles = Instantiate(PocketedParticles, playerHeldBy.transform.position, Quaternion.identity, playerHeldBy.transform);
                    createdPocketedParticles.Play();
                    pocketedParticlesPlaying = true;
                    chargingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    chargingParticlePlaying = false;
                }
                if (charge is < 30 && chargingParticlePlaying == false && !isPocketed)
                {
                    chargingParticles.Play();
                    chargingParticlePlaying = true;
                }
                if (charge >= 30 && !teleportCoroutineRunning)
                {
                    chargingParticles.Stop();
                    chargingParticlePlaying = false;
                    StartCoroutine(WaitToTeleport());
                }
            }
            activateCoroutineRunning = false;
        }
        private System.Collections.IEnumerator WaitToTeleport()
        {
            teleportCoroutineRunning = true;
            while (charge >= 20)
            {
                yield return new WaitForSeconds(1);
                teleportChanceRoll = new System.Random().Next(1, 6);
                if (teleportChanceRoll == 1)
                {
                    if (!heldByPlayerOnServer)
                    {
                        yield return new WaitUntil(() => heldByPlayerOnServer);
                    }
                    if (playerHeldBy.isInsideFactory)
                    {
                        TeleportPlayerServerRpc(true);
                    }
                    else if (!playerHeldBy.isInsideFactory)
                    {
                        TeleportPlayerServerRpc(false);
                    }
                    charge = 0;
                    break;
                }
                teleportCoroutineRunning = false;
            }
            teleportCoroutineRunning = false;
            if (!activateCoroutineRunning && !teleportCoroutineRunning)
            {
                StartCoroutine(WaitToActivate());
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void TeleportPlayerServerRpc(bool indoors)
        {
            TeleportPlayerClientRpc(indoors);
        }
        [ClientRpc]
        public void TeleportPlayerClientRpc(bool isIndoors)
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
        public async void RandomlyTeleportPlayer(Vector3 TeleportVector, bool teleportedIndoors)
        {
            int playerID = (int)playerHeldBy.playerClientId;
            PlayerControllerB player = RoundManager.Instance.playersManager.allPlayerScripts[playerID];
            createdTeleportParticles = Instantiate(teleportParticles, player.gameObject.transform.position + new Vector3(0, 1f, 0), Quaternion.identity, player.transform);
            createdTeleportParticles.Play();
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (player != null)
            {
                player.averageVelocity = 0f;
                player.velocityLastFrame = Vector3.zero;
                player.DropAllHeldItems(true, false);
                player.TeleportPlayer(TeleportVector, false, 0f, false, true);
                if (UnityEngine.Object.FindObjectOfType<AudioReverbPresets>())
                {
                    UnityEngine.Object.FindObjectOfType<AudioReverbPresets>().audioPresets[2].ChangeAudioReverbForPlayer(player);
                }
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
                charge = 0;
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
                chargingParticlePlaying = false;
                charge = 0;
                chargingParticles.Stop();
            }
        }
    }
}
