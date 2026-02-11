using GameNetcodeStuff;
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
        bool activateCoroutineRunning = false;
        bool chargingParticlePlaying = false;
        bool chargedParticlesPlaying = false;
        int charge = 0;
        EnemyType maskedPlayer;
        Vector3 navMeshPosition;
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
            }
            disabledInShip = (BoundConfig.CapsulesDisabledOnTheShip.Value);
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

        public override void Update()
        {
            base.Update();
            if (StartOfRound.Instance.inShipPhase && activateCoroutineRunning || disabledInShip == true && this.isInShipRoom && activateCoroutineRunning)
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
            else if (StartOfRound.Instance.shipHasLanded && !activateCoroutineRunning && TimeOfDay.Instance.currentLevel.planetHasTime)
            {
                StartCoroutine(AbsorbBlood());
            }
        }
        private System.Collections.IEnumerator AbsorbBlood()
        {
            activateCoroutineRunning = true;
            if (!StartOfRound.Instance.shipHasLanded || !TimeOfDay.Instance.currentLevel.planetHasTime)
            {
                yield return new WaitUntil(() => StartOfRound.Instance.shipHasLanded && TimeOfDay.Instance.currentLevel.planetHasTime);
            }
            while (StartOfRound.Instance.shipHasLanded && TimeOfDay.Instance.currentLevel.planetHasTime)
            {
                yield return new WaitForSeconds(3);
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
                            if (charge is > 0 and < 12 && chargingParticlePlaying == false && !isPocketed)
                            {
                                chargingParticles.Play();
                                chargingParticlePlaying = true;
                            }
                            if (charge >= 12)
                            {
                                if (chargingParticlePlaying)
                                {
                                    chargingParticles.Stop();
                                    chargingParticlePlaying = false;
                                }
                                chargedParticles.Play();
                                chargedParticlesPlaying = true;
                                charge = 0;
                                yield return new WaitUntil(() => !isHeld);
                                //SpawnMaskedServerRpc();
                                chargedParticles.Stop();
                                chargedParticlesPlaying = false;

                            }
                        }
                    }
                }
                else
                {
                    chargingParticles.Stop();
                    chargingParticlePlaying = false;
                }
                if (isPocketed)
                {
                    chargingParticles.Stop();
                    chargingParticlePlaying = false;
                }
                
            }
            activateCoroutineRunning = false;
        }

        [ServerRpc]
        public void SpawnMaskedServerRpc()
        {
            SelectableLevel[] levels = StartOfRound.Instance.levels;

            foreach (SelectableLevel val in levels)
            {
                foreach (SpawnableEnemyWithRarity enemy in val.Enemies)
                {
                    if (((UnityEngine.Object)enemy.enemyType).name.Contains("MaskedPlayerEnemy"))
                    {
                        maskedPlayer = enemy.enemyType;
                        navMeshPosition = RoundManager.Instance.GetNavMeshPosition(this.transform.position, default(NavMeshHit), 10f, -1);
                        break;
                    }
                }
            }
            RoundManager.Instance.SpawnEnemyGameObject(navMeshPosition, 0f, 0, maskedPlayer);
        }
    }
}

