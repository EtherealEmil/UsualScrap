using GameNetcodeStuff;
using System.Threading.Tasks;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace UsualScrap.Behaviors
{
    public class GloomyCapsuleScript : GrabbableObject
    {
        ParticleSystem teleportParticles;

        ParticleSystem createdTeleportParticles;

        ParticleSystem createdPocketedChargingParticles;

        ParticleSystem chargingParticles;
        bool chargingParticlePlaying = false;

        ParticleSystem pocketedChargingParticles;
        bool pocketedChargingParticlesPlaying = false;

        ParticleSystem chargedParticles;
        bool chargedParticlesPlaying = false;

        Coroutine detectCoroutine = null;

        System.Random shipTeleporterSeed;
        Light light;           
        bool teleportingCoroutineRunning = false;

        int teleportChanceRoll;
        int charge = 0;
        bool disabledInShip;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            teleportParticles = this.transform.Find("TeleportParticles").GetComponent<ParticleSystem>();
            chargingParticles = this.transform.Find("ChargingParticles").GetComponent<ParticleSystem>();
            pocketedChargingParticles = this.transform.Find("PocketedChargingParticles").GetComponent<ParticleSystem>();
            chargedParticles = this.transform.Find("ChargedParticles").GetComponent<ParticleSystem>();
            light = this.transform.Find("Point Light").GetComponent<Light>();
            if (!StartOfRound.Instance.inShipPhase)
            {
                this.shipTeleporterSeed = new System.Random(StartOfRound.Instance.randomMapSeed + 17 + (int)GameNetworkManager.Instance.localPlayerController.playerClientId);
            }
            disabledInShip = (BoundConfig.CapsulesDisabledOnTheShip.Value);

            StartCoroutine(Glow());
        }

        private System.Collections.IEnumerator Glow()
        {
            float maxintensity = light.intensity * 1.5f;
            float minintensity = light.intensity * .5f;
            while (true)
            {
                if (!light.enabled)
                {
                    yield return new WaitUntil(() => light.enabled);
                }
                float t = (MathF.Sin(Time.time * .5f) + 1f) / 2f;
                light.intensity = minintensity + (maxintensity - minintensity) * t;
                yield return null;
            }
        }

        public override void PocketItem()
        {
            base.PocketItem();
            if (light.enabled)
            {
                light.enabled = false;
            }
        }
        public override void EquipItem()
        {
            base.EquipItem();
            if (!light.enabled)
            {
                light.enabled = true;
            }
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
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

        public override void GrabItem()
        {
            base.GrabItem();
            if (!StartOfRound.Instance.inShipPhase && detectCoroutine == null)
            {
                detectCoroutine = StartCoroutine(Detecting());
            }
        }

        private System.Collections.IEnumerator Detecting()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (chargingParticlePlaying == true)
                {
                    chargingParticles.Stop();
                    chargingParticlePlaying = false;
                }
                if (!StartOfRound.Instance.inShipPhase && StartOfRound.Instance.shipHasLanded && TimeOfDay.Instance.currentLevel.planetHasTime)
                {
                    if (disabledInShip == true && this.isInShipRoom)
                    {
                        yield return null;
                        continue;
                    }
                    else
                    {
                        while (TimeOfDay.Instance.dayMode == DayMode.Midnight && !this.isInFactory && !teleportingCoroutineRunning|| TimeOfDay.Instance.dayMode == DayMode.Sundown && !this.isInFactory && !teleportingCoroutineRunning)
                        {
                            yield return new WaitForSeconds(1);
                            if (StartOfRound.Instance.inShipPhase || disabledInShip == true && this.isInShipRoom)
                            {
                                charge = 0;
                                break;
                            }

                            charge++;

                            if (isPocketed)
                            {
                                chargingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                                chargingParticlePlaying = false;

                                createdPocketedChargingParticles = Instantiate(pocketedChargingParticles, playerHeldBy.transform.position, Quaternion.identity, playerHeldBy.transform);
                                createdPocketedChargingParticles.Play();
                            }
                            else if (chargingParticlePlaying == false)
                            {
                                chargingParticles.Play();
                                chargingParticlePlaying = true;
                            }
                            if (charge >= 40 && !teleportingCoroutineRunning)
                            {
                                chargingParticles.Stop();
                                chargingParticlePlaying = false;

                                chargedParticles.Play();
                                chargedParticlesPlaying = true;

                                teleportingCoroutineRunning = true;
                                StartCoroutine(WaitToTeleport());
                                yield return new WaitUntil(() => teleportingCoroutineRunning == false);

                                chargedParticles.Stop();
                                chargedParticlesPlaying = false;

                                charge = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private System.Collections.IEnumerator WaitToTeleport()
        {
            teleportingCoroutineRunning = true;
            bool playerTeleported = false;
            while (playerTeleported == false)
            {
                yield return new WaitForSeconds(1);
                if (StartOfRound.Instance.inShipPhase || disabledInShip == true && this.isInShipRoom)
                {
                    break;
                }
                teleportChanceRoll = new System.Random().Next(1, 6);
                if (teleportChanceRoll == 1)
                {
                    if (heldByPlayerOnServer)
                    {
                        if (playerHeldBy.isInsideFactory)
                        {
                            createdTeleportParticles = Instantiate(teleportParticles, playerHeldBy.gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.identity, playerHeldBy.transform);
                            createdTeleportParticles.Play();
                            TeleportPlayerServerRpc(true);
                            playerTeleported = true;
                            break;
                        }
                        else if (!playerHeldBy.isInsideFactory)
                        {
                            createdTeleportParticles = Instantiate(teleportParticles, playerHeldBy.gameObject.transform.position + new Vector3(0, 0, 0), Quaternion.identity, playerHeldBy.transform);
                            createdTeleportParticles.Play();
                            TeleportPlayerServerRpc(false);
                            playerTeleported = true;
                            break;
                        }
                    }
                    else
                    {
                        yield return null;
                        continue;
                    }
                }
            }
            teleportingCoroutineRunning = false;
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
            }
        }
    }
}
