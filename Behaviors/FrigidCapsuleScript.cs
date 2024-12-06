using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UsualScrap.Behaviors.Effects;
using static UnityEngine.ParticleSystem;

namespace UsualScrap.Behaviors
{
    internal class FrigidCapsuleScript : GrabbableObject
    {
        ParticleSystem chargingSnowflakeParticles;
        ParticleSystem ambientSnowflakeParticles;
        ParticleSystem snowstormParticles;
        ParticleSystem frostedParticles;
        ParticleSystem ambientSnowParticles;
        ParticleSystem chargedSnowParticles;
        //ParticleSystem frozenParticles;
        AudioSource[] Sounds;
        AudioSource snowstormAudio;
        bool snowstormParticlesRunning = false;
        bool activateCoroutineRunning = false;
        bool snowstormCoroutineRunning = false;
        bool chargingParticlesPlaying = false;
        bool chargedParticlesPlaying = false;
        bool chargedParticlesPreviouslyPlaying = false;
        bool randomSnowParticlesPlaying = false;
        Light light;
        int charge = 0;
        int randomSnowParticleDelay;
        bool disabledInShip;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;
        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            ambientSnowflakeParticles = this.transform.Find("AmbientSnowflakeParticles").GetComponent<ParticleSystem>();
            chargingSnowflakeParticles = this.transform.Find("ActiveSnowflakeParticles").GetComponent<ParticleSystem>();
            //frozenParticles = this.transform.Find("FrozenParticles").GetComponent<ParticleSystem>();
            snowstormParticles = this.transform.Find("StormParticles").GetComponent<ParticleSystem>();
            frostedParticles = this.transform.Find("FrostedParticles").GetComponent<ParticleSystem>();
            ambientSnowParticles = this.transform.Find("AmbientSnowParticles").GetComponent<ParticleSystem>();
            chargedSnowParticles = this.transform.Find("ActiveSnowParticles").GetComponent<ParticleSystem>();
            light = this.transform.Find("Point Light").GetComponent<Light>();
            Sounds = this.transform.Find("FrigidCapsuleSounds").gameObject.GetComponents<AudioSource>();
            snowstormAudio = Sounds[0];
            disabledInShip = (BoundConfig.CapsulesDisabledOnTheShip.Value);
        }
        public override void PocketItem()
        {
            base.PocketItem();
            chargingSnowflakeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ambientSnowflakeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            if (chargedParticlesPlaying)
            {
                chargedSnowParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                chargedParticlesPlaying = false;
                chargedParticlesPreviouslyPlaying = true;
            }
            if (light.enabled)
            {
                light.enabled = false;
            }
        }
        public override void EquipItem()
        {
            base.EquipItem();
            if (chargingParticlesPlaying)
            {
                chargingSnowflakeParticles.Play();
            }
            else
            {
                ambientSnowflakeParticles.Play();
            }
            if (chargedParticlesPreviouslyPlaying && !chargedParticlesPlaying)
            {
                chargedSnowParticles.Play();
                chargedParticlesPlaying = true;
                chargedParticlesPreviouslyPlaying = false;
            }
            if (!light.enabled)
            {
                light.enabled = true;
            }
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            if (chargingParticlesPlaying)
            {
                chargingSnowflakeParticles.Play();
            }
            else
            {
                ambientSnowflakeParticles.Play();
            }
            if (chargedParticlesPreviouslyPlaying && !chargedParticlesPlaying)
            {
                chargedSnowParticles.Play();
                chargedParticlesPlaying = true;
                chargedParticlesPreviouslyPlaying = false;
            }
            if (!light.enabled)
            {
                light.enabled = true;
            }
        }
        public override void Update()
        {
            base.Update();
            if (!snowstormCoroutineRunning && !randomSnowParticlesPlaying)
            {
                StartCoroutine(PlayRandomSnowParticle());
            }
            if (!StartOfRound.Instance.shipHasLanded && activateCoroutineRunning || !StartOfRound.Instance.shipHasLanded && snowstormCoroutineRunning)
            {
                StopAllCoroutines();
                activateCoroutineRunning = false;
                snowstormCoroutineRunning = false;

                chargingSnowflakeParticles.Stop();
                chargingParticlesPlaying = false;
                charge = 0;

                snowstormParticlesRunning = false;
                chargedParticlesPlaying = false;
                chargedParticlesPreviouslyPlaying = false;
                chargedSnowParticles.Stop();
                snowstormParticles.Stop();
                snowstormAudio.Stop();

                ambientSnowflakeParticles.Play();
            }
            else if (disabledInShip == true && this.isInShipRoom)
            {
                return;
            }
            else if (StartOfRound.Instance.shipHasLanded && !activateCoroutineRunning && !snowstormCoroutineRunning && TimeOfDay.Instance.currentLevel.planetHasTime)
            {
                StartCoroutine(WaitToActivate());
            }
        }
        private System.Collections.IEnumerator PlayRandomSnowParticle()
        {
            randomSnowParticlesPlaying = true;
            randomSnowParticleDelay = new System.Random().Next(3, 9);
            while (!snowstormCoroutineRunning)
            {
                yield return new WaitForSeconds(1);
                if (isPocketed)
                {
                    yield return new WaitUntil(() => !isPocketed);
                }
                randomSnowParticleDelay--;
                if (randomSnowParticleDelay <= 0)
                {
                    ambientSnowParticles.Play();
                    randomSnowParticleDelay = new System.Random().Next(3, 9);
                }
            }
            randomSnowParticlesPlaying = false;
        }

        private System.Collections.IEnumerator WaitToActivate()
        {
            activateCoroutineRunning = true;
            if (TimeOfDay.Instance.dayMode != DayMode.Noon || TimeOfDay.Instance.dayMode != DayMode.Dawn)
            {
                yield return new WaitUntil(() => TimeOfDay.Instance.dayMode == DayMode.Noon || TimeOfDay.Instance.dayMode == DayMode.Dawn);
            }
            while (TimeOfDay.Instance.dayMode == DayMode.Noon && !snowstormCoroutineRunning || TimeOfDay.Instance.dayMode == DayMode.Dawn && !snowstormCoroutineRunning)
            {
                yield return new WaitForSeconds(1);
                charge++;
                //print($"{charge}");
                if (isInFactory)
                {
                    chargingSnowflakeParticles.Stop();
                    ambientSnowflakeParticles.Play();
                    chargingParticlesPlaying = false;
                    yield return new WaitUntil(() => !isInFactory);
                }
                if (charge is > 0 and < 15 && chargingParticlesPlaying == false && !isPocketed)
                {
                    chargingSnowflakeParticles.Play();
                    ambientSnowflakeParticles.Stop();
                    chargingParticlesPlaying = true;
                }
                if (charge >= 20 && !snowstormCoroutineRunning)
                {
                    SnowstormCoroutineServerRPC();
                }
            }
            activateCoroutineRunning = false;
            if (!activateCoroutineRunning && !snowstormCoroutineRunning)
            {
                StartCoroutine(WaitToActivate());
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void SnowstormCoroutineServerRPC()
        {
            SnowstormCoroutineClientRPC();
        }
        [ClientRpc]
        public void SnowstormCoroutineClientRPC()
        {
            StartCoroutine(Snowstorm());
        }
        private System.Collections.IEnumerator Snowstorm()
        {
            snowstormCoroutineRunning = true;
            int stormDuration = 20;
            if (!chargedParticlesPlaying && !isPocketed)
            {
                chargedSnowParticles.Play();
                chargedParticlesPlaying = true;
            }
            else
            {
                chargedParticlesPreviouslyPlaying = true;
            }
            if (!snowstormParticlesRunning)
            {
                snowstormParticlesRunning = true;
                snowstormParticles.Play();
            }
            snowstormAudio.Play();
            while (stormDuration > 0)
            {
                yield return new WaitForSeconds(1.5f);
                stormDuration--;
                CheckAndCallFrostStacksServerRPC();
            }
            charge = 0;
            snowstormCoroutineRunning = false;
            if (snowstormParticlesRunning == true)
            {
                snowstormParticlesRunning = false;
                chargedParticlesPlaying = false;
                chargedSnowParticles.Stop();
                snowstormParticles.Stop();
                snowstormAudio.Stop();
                chargedParticlesPreviouslyPlaying = false;
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void CheckAndCallFrostStacksServerRPC()
        {
            CheckAndCallFrostStacksClientRPC();
        }
        [ClientRpc]
        public void CheckAndCallFrostStacksClientRPC()
        {
            CheckAndCallFrostStacks();
        }

        public void CheckAndCallFrostStacks()
        {
            Collider[] playerArray = Physics.OverlapSphere(this.transform.position, 3, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
            HashSet<Collider> Affected = new HashSet<Collider>();
            foreach (Collider playerCollider in playerArray)
            {
                if (!Affected.Contains(playerCollider))
                {
                    PlayerControllerB playerControllerB = playerCollider.gameObject.GetComponent<PlayerControllerB>();
                    if (playerControllerB == null)
                    {
                        return;
                    }
                    var ID = (int)playerControllerB.playerClientId;
                    if (playerControllerB.isPlayerDead == true)
                    {
                        return;
                    }
                    Component[] frostStacks = playerControllerB.gameObject.GetComponents<SnowstormStackingSlowEffect>();
                    if (frostStacks.Length < 15) 
                    {
                        ApplyFrostStackServerRPC(ID, frostStacks.Length);
                    }
                    Affected.Add(playerCollider);
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void ApplyFrostStackServerRPC(int playerID, int stackNumber)
        {
            ApplyFrostStackClientRPC(playerID, stackNumber);
        }
        [ClientRpc]
        public void ApplyFrostStackClientRPC(int playerID, int stackNumber)
        {
            ApplyFrostStack(playerID, stackNumber);
        }
        public void ApplyFrostStack(int playerID, int stackNumber)
        {
            PlayerControllerB PlayerScript = RoundManager.Instance.playersManager.allPlayerScripts[playerID];
            SnowstormStackingSlowEffect effect = PlayerScript.gameObject.AddComponent<SnowstormStackingSlowEffect>();
            effect.frostedParticles = frostedParticles;
            effect.stackNumber = stackNumber;
        }
        public override void OnBroughtToShip()
        {
            base.OnBroughtToShip();
            if (disabledInShip == true)
            {
                StopAllCoroutines();
                activateCoroutineRunning = false;
                snowstormCoroutineRunning = false;

                chargingSnowflakeParticles.Stop();
                chargingParticlesPlaying = false;
                charge = 0;

                snowstormParticlesRunning = false;
                chargedParticlesPlaying = false;
                chargedParticlesPreviouslyPlaying = false;
                chargedSnowParticles.Stop();
                snowstormParticles.Stop();
                snowstormAudio.Stop();

                ambientSnowflakeParticles.Play();
            }
        }
    }
}
