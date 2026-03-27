using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace UsualScrap.Behaviors
{
    public class SanguineCapsuleScript : GrabbableObject
    {
        ParticleSystem chargedParticles;
        ParticleSystem chargingParticles;
        ParticleSystem drainingParticles;
        bool activateCoroutineRunning = false;
        bool chargingParticlePlaying = false;
        bool chargedParticlesPlaying = false;
        int charge = 0;

        public void Awake()
        {
            chargedParticles = this.transform.Find("ChargedParticles").GetComponent<ParticleSystem>();
            chargingParticles = this.transform.Find("ChargingParticles").GetComponent<ParticleSystem>();
            drainingParticles = this.transform.Find("DrainingParticles").GetComponent<ParticleSystem>();
        }

        public override void Update()
        {
            base.Update();
            if (StartOfRound.Instance.inShipPhase && activateCoroutineRunning)
            {
                StopAllCoroutines();
                activateCoroutineRunning = false;
                chargingParticlePlaying = false;
                chargedParticlesPlaying = false;
                charge = 0;
                chargedParticles.Stop();
                chargingParticles.Stop();
                drainingParticles.Stop();
            }
        }
        public override void GrabItem()
        {
            base.GrabItem();
            StartCoroutine(ThirstForBlood());
        }

        private System.Collections.IEnumerator ThirstForBlood()
        {
            activateCoroutineRunning = true;
            while (isHeld)
            {
                yield return new WaitForSeconds(5);
                Collider[] playerArray = Physics.OverlapSphere(this.transform.position, 1, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
                if (playerArray.Length > 0)
                {
                    for (int i = 0; i < playerArray.Length; i++)
                    {
                        PlayerControllerB playerControllerB = playerArray[i].gameObject.GetComponent<PlayerControllerB>();
                        if (playerControllerB.health < 100)
                        {
                            print("Draining playerInital's health");
                            if (playerControllerB.health > 40)
                            {
                                playerControllerB.health -= 5;
                                ParticleSystem DrainingParticle = Instantiate(drainingParticles, playerControllerB.transform.position, Quaternion.identity);
                                DrainingParticle.Play();
                            }
                            charge++;
                            if (charge is > 0 and < 25 && chargingParticlePlaying == false && !isPocketed)
                            {
                                chargingParticles.Play();
                                chargingParticlePlaying = true;
                            }
                            if (charge >= 25)
                            {
                                if (chargingParticlePlaying)
                                {
                                    chargingParticles.Stop();
                                    chargingParticlePlaying = false;
                                }
                                chargedParticles.Play();
                                chargedParticlesPlaying = true;
                                //BloodForLife();
                            }
                        }
                    }
                }
                if (isPocketed)
                {
                    chargingParticles.Stop();
                    chargingParticlePlaying = false;
                }
            }
            activateCoroutineRunning = false;
        }
        private void BloodForLife()
        {
        }
    }
}

