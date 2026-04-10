using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace UsualScrap.Behaviors
{
    public class BloodyCapsuleScript : GrabbableObject
    {
        ParticleSystem chargedParticles;
        ParticleSystem chargingParticles;
        ParticleSystem drainingParticles;
        AudioSource[] sounds;
        AudioClip pulse;
        Light light;
        bool chargingParticlePlaying = false;

        bool chargedParticlesPlaying = false;

        Coroutine detectCoroutine = null;

        int charge = 0;
        //EnemyType maskedPlayer;
        //Vector3 navMeshPosition;
        bool disabledInShip;
        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            try
            {
                chargedParticles = this.transform.Find("ChargedParticles").GetComponent<ParticleSystem>();
                chargingParticles = this.transform.Find("ChargingParticles").GetComponent<ParticleSystem>();
                drainingParticles = this.transform.Find("DrainingParticles").GetComponent<ParticleSystem>();
                sounds = this.transform.Find("BloodyCapsuleSounds").gameObject.GetComponents<AudioSource>();
                light = this.transform.Find("Point Light").GetComponent<Light>();
                pulse = sounds[0].clip;
            }
            catch { 
                if (chargedParticles == null)
                {
                    print("Bloody Capsule missing charged particles! - USUAL SCRAP");
                }
                if (chargingParticles == null)
                {
                    print("Bloody Capsule missing charging particles! - USUAL SCRAP");
                }
                if (drainingParticles == null)
                {
                    print("Bloody Capsule missing draining particles! - USUAL SCRAP");
                }
                if (sounds == null)
                {
                    print("Bloody Capsule missing sounds! - USUAL SCRAP");
                }
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
                float t = (MathF.Sin(Time.time * 1f) + 1f) / 2f;
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
                        while (TimeOfDay.Instance.dayMode == DayMode.Midnight && !this.isInFactory|| TimeOfDay.Instance.dayMode == DayMode.Sundown && !this.isInFactory)
                        {
                            yield return new WaitForSeconds(3);
                            if (StartOfRound.Instance.inShipPhase || disabledInShip == true && this.isInShipRoom)
                            {
                                charge = 0;
                                break;
                            }

                            if (isPocketed)
                            {
                                chargingParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                                chargingParticlePlaying = false;
                            }
                            else if (chargingParticlePlaying == false)
                            {
                                chargingParticles.Play();
                                chargingParticlePlaying = true;
                            }

                            Collider[] playerColliderArray = Physics.OverlapSphere(this.transform.position, 4, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
                            HashSet<PlayerControllerB> drainedPlayers = new HashSet<PlayerControllerB>();
                            if (playerColliderArray.Length > 0)
                            {
                                foreach (Collider collider in playerColliderArray)
                                {
                                    PlayerControllerB targetPlayer = collider.gameObject.GetComponent<PlayerControllerB>();

                                    if (targetPlayer != null && !drainedPlayers.Contains(targetPlayer))
                                    {
                                        targetPlayer.DamagePlayer(10);
                                        drainedPlayers.Add(targetPlayer);
                                        ParticleSystem DrainingParticle = Instantiate(drainingParticles, collider.transform.position, Quaternion.identity, targetPlayer.transform);
                                        DrainingParticle.Play();
                                        sounds[0].PlayOneShot(pulse);
                                        charge++;
                                    }
                                }
                            }

                            if (charge >= 20)
                            {
                                chargingParticles.Stop();
                                chargingParticlePlaying = false;

                                chargedParticles.Play();
                                chargedParticlesPlaying = true;

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
    }
}
