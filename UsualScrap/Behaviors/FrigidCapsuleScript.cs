using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UsualScrap.Behaviors.Effects;

namespace UsualScrap.Behaviors
{
    public class FrigidCapsuleScript : GrabbableObject
    {
        ParticleSystem chargingSnowflakeParticles;
        ParticleSystem ambientSnowflakeParticles;
        ParticleSystem snowstormParticles;
        ParticleSystem frostedParticles;
        ParticleSystem ambientSnowParticles;
        ParticleSystem activeSnowParticles;
        Coroutine detectCoroutine = null;
        AudioSource[] Sounds;
        AudioSource snowstormAudio;
        bool chargingSnowflakeParticlesPlaying = false;
        bool ambientSnowflakeParticlesPlaying = false;
        bool snowstormCoroutineRunning = false;
        bool activeSnowParticlesPlaying = false;
        bool passiveSnowParticlesPlaying = false;
        Light light;
        int charge = 0;
        bool disabledInShip;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;
        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            ambientSnowflakeParticles = this.transform.Find("AmbientSnowflakeParticles").GetComponent<ParticleSystem>();
            chargingSnowflakeParticles = this.transform.Find("ActiveSnowflakeParticles").GetComponent<ParticleSystem>();
            snowstormParticles = this.transform.Find("StormParticles").GetComponent<ParticleSystem>();
            frostedParticles = this.transform.Find("FrostedParticles").GetComponent<ParticleSystem>();
            ambientSnowParticles = this.transform.Find("AmbientSnowParticles").GetComponent<ParticleSystem>();
            activeSnowParticles = this.transform.Find("ActiveSnowParticles").GetComponent<ParticleSystem>();
            light = this.transform.Find("Point Light").GetComponent<Light>();
            Sounds = this.transform.Find("FrigidCapsuleSounds").gameObject.GetComponents<AudioSource>();
            snowstormAudio = Sounds[0];
            disabledInShip = (BoundConfig.CapsulesDisabledOnTheShip.Value);

            ambientSnowflakeParticles.Play();
            ambientSnowflakeParticlesPlaying = true;

            StartCoroutine(Glow());
        }

        private System.Collections.IEnumerator Glow()
        {
            float maxintensity = light.intensity * .5f;
            float minintensity = light.intensity * 1.5f;
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

        public override void GrabItem()
        {
            base.GrabItem();
            if (!StartOfRound.Instance.inShipPhase && detectCoroutine == null)
            {
                StartCoroutine(Detecting());
            }
        }

        private System.Collections.IEnumerator Detecting()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (ambientSnowflakeParticlesPlaying == false && !isPocketed)
                {
                    ambientSnowflakeParticles.Play();
                    ambientSnowflakeParticlesPlaying = true;

                    if (chargingSnowflakeParticlesPlaying)
                    {
                        chargingSnowflakeParticles.Stop();
                        chargingSnowflakeParticlesPlaying = false;
                    }
                }
                else if (isPocketed && ambientSnowflakeParticlesPlaying == true)
                {
                    ambientSnowflakeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ambientSnowflakeParticlesPlaying = false;

                    if (chargingSnowflakeParticlesPlaying)
                    {
                        chargingSnowflakeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                        chargingSnowflakeParticlesPlaying = false;
                    }
                }
                if (!passiveSnowParticlesPlaying)
                {
                    StartCoroutine(PassiveSnowParticles());
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
                        while (!this.isInFactory && !snowstormCoroutineRunning)
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
                                if (chargingSnowflakeParticlesPlaying)
                                {
                                    chargingSnowflakeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                                    chargingSnowflakeParticlesPlaying = false;
                                }
                                if (ambientSnowflakeParticlesPlaying)
                                {
                                    ambientSnowflakeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                                    ambientSnowflakeParticlesPlaying = false;
                                }
                            }
                            else if (chargingSnowflakeParticlesPlaying == false)
                            {
                                if (ambientSnowflakeParticlesPlaying == true)
                                {
                                    ambientSnowflakeParticles.Stop();
                                    ambientSnowflakeParticlesPlaying = false;
                                }
                                chargingSnowflakeParticles.Play();
                                chargingSnowflakeParticlesPlaying = true;
                            }
                            if (charge >= 45 && !snowstormCoroutineRunning)
                            {

                                SnowstormCoroutineServerRpc();

                                charge = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SnowstormCoroutineServerRpc()
        {
            SnowstormCoroutineClientRpc();
        }
        [ClientRpc]
        public void SnowstormCoroutineClientRpc()
        {
            StartCoroutine(Snowstorm());
        }
        private System.Collections.IEnumerator Snowstorm()
        {
            snowstormCoroutineRunning = true;
            int stormDuration = 15;
            int particlespacer = 0;
            snowstormAudio.Play();
            if (!activeSnowParticlesPlaying)
            {
                activeSnowParticles.Play();
                activeSnowParticlesPlaying = true;
            }
            while (stormDuration > 0)
            {
                yield return new WaitForSeconds(1f);
                if (particlespacer == 0)
                {
                    snowstormParticles.Play();
                    particlespacer++;
                }
                else if (particlespacer == 1)
                {
                    particlespacer = 0;
                }
                else
                {
                    particlespacer++;
                }
                stormDuration--;
                //print("US - Applying frost stack");
                CheckForPlayerAndCallServerRpc();
            }
            if (activeSnowParticlesPlaying)
            {
                activeSnowParticles.Stop();
                activeSnowParticlesPlaying = false;
            }
            snowstormCoroutineRunning = false;
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

        private System.Collections.IEnumerator PassiveSnowParticles()
        {
            passiveSnowParticlesPlaying = true;
            int randomSnowParticleDelay = new System.Random().Next(10, 15);
            while (!snowstormCoroutineRunning)
            {
                yield return new WaitForSeconds(1);
                if (!isPocketed)
                {
                    randomSnowParticleDelay--;
                    if (randomSnowParticleDelay <= 0)
                    {
                        ambientSnowParticles.Play();
                        randomSnowParticleDelay = new System.Random().Next(10, 15);
                    }
                }
            }
            passiveSnowParticlesPlaying = false;
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void CheckForPlayerAndCallServerRpc()
        {
            CheckForPlayerAndCallClientRpc();
        }
        [ClientRpc]
        public void CheckForPlayerAndCallClientRpc()
        {
            CheckForPlayerAndCall();
        }

        public void CheckForPlayerAndCall()
        {
            Collider[] playerArray = Physics.OverlapSphere(this.transform.position, 3, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
            HashSet<PlayerControllerB> Affected = new HashSet<PlayerControllerB>();
            foreach (Collider playerCollider in playerArray)
            {
                PlayerControllerB playerControllerB = playerCollider.gameObject.GetComponent<PlayerControllerB>();
                if (!Affected.Contains(playerControllerB))
                {
                    if (playerControllerB == null || playerControllerB.isPlayerDead == true)
                    {
                        return;
                    }
                    var ID = (int)playerControllerB.playerClientId;
                    CheckAndApplyFrostStacksServerRpc(ID);
                    Affected.Add(playerControllerB);
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void CheckAndApplyFrostStacksServerRpc(int playerID)
        {
            CheckAndApplyFrostStacksClientRpc(playerID);
        }
        [ClientRpc]
        public void CheckAndApplyFrostStacksClientRpc(int playerID)
        {
            CheckAndApplyFrostStacks(playerID);
        }
        public void CheckAndApplyFrostStacks(int playerID)
        {
            PlayerControllerB PlayerScript = RoundManager.Instance.playersManager.allPlayerScripts[playerID];
            PlayerControllerB localplayercontroller = GameNetworkManager.Instance.localPlayerController;
            StackingSlowEffect[] frostStacks = PlayerScript.gameObject.GetComponents<StackingSlowEffect>();
            if (frostStacks.Length < 5)
            {
                if (localplayercontroller == PlayerScript)
                {
                    StackingSlowEffect effect = PlayerScript.gameObject.AddComponent<StackingSlowEffect>();
                    effect.frostedParticles = frostedParticles;
                }
            }
        }
    }
}
