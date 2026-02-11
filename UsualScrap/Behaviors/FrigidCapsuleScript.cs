using GameNetcodeStuff;
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
        ParticleSystem chargedSnowParticles;
        Coroutine activateCoroutine;
        Coroutine stormCoroutine;
        AudioSource[] Sounds;
        AudioSource snowstormAudio;
        bool activateCoroutineRunning = false;
        bool snowstormCoroutineRunning = false;
        bool chargingParticlesPlaying = false;
        bool chargedParticlesPlaying = false;
        bool chargedParticlesPreviouslyPlaying = false;
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
            if (!snowstormCoroutineRunning && !passiveSnowParticlesPlaying)
            {
                StartCoroutine(PlayRandomSnowParticle());
            }
            if (!StartOfRound.Instance.shipHasLanded && activateCoroutineRunning || !StartOfRound.Instance.shipHasLanded && snowstormCoroutineRunning)
            {
                if (activateCoroutineRunning)
                {
                    StopCoroutine(activateCoroutine);
                }
                if (snowstormCoroutineRunning)
                {
                    StopCoroutine(stormCoroutine);
                }
                activateCoroutineRunning = false;
                snowstormCoroutineRunning = false;

                chargingSnowflakeParticles.Stop();
                chargingParticlesPlaying = false;
                charge = 0;

                chargedParticlesPlaying = false;
                chargedParticlesPreviouslyPlaying = false;
                chargedSnowParticles.Stop();
                snowstormAudio.Stop();

                if (!snowstormCoroutineRunning && !passiveSnowParticlesPlaying)
                {
                    StartCoroutine(PlayRandomSnowParticle());
                }
            }
            else if (disabledInShip == true && this.isInShipRoom)
            {
                return;
            }
            else if (StartOfRound.Instance.shipHasLanded && !activateCoroutineRunning && !snowstormCoroutineRunning && TimeOfDay.Instance.currentLevel.planetHasTime)
            {
                activateCoroutine = StartCoroutine(WaitToActivate());
            }
        }
        private System.Collections.IEnumerator PlayRandomSnowParticle()
        {
            passiveSnowParticlesPlaying = true;
            int randomSnowParticleDelay = new System.Random().Next(10, 21);
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
            passiveSnowParticlesPlaying = false;
        }

        private System.Collections.IEnumerator WaitToActivate()
        {
            activateCoroutineRunning = true;
            if (TimeOfDay.Instance.dayMode != DayMode.Midnight || TimeOfDay.Instance.dayMode != DayMode.Sundown)
            {
                yield return new WaitUntil(() => TimeOfDay.Instance.dayMode == DayMode.Midnight || TimeOfDay.Instance.dayMode == DayMode.Sundown);
            }
            while (TimeOfDay.Instance.dayMode == DayMode.Midnight && !snowstormCoroutineRunning || TimeOfDay.Instance.dayMode == DayMode.Sundown && !snowstormCoroutineRunning)
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
                if (charge is < 30 && chargingParticlesPlaying == false && !isPocketed)
                {
                    chargingSnowflakeParticles.Play();
                    ambientSnowflakeParticles.Stop();
                    chargingParticlesPlaying = true;
                }
                if (charge >= 30 && !snowstormCoroutineRunning)
                {
                    SnowstormCoroutineServerRpc();
                    activateCoroutineRunning = false;
                    yield break;
                }
            }
            activateCoroutineRunning = false;
        }
        [ServerRpc(RequireOwnership = false)]
        public void SnowstormCoroutineServerRpc()
        {
            SnowstormCoroutineClientRpc();
        }
        [ClientRpc]
        public void SnowstormCoroutineClientRpc()
        {
            stormCoroutine = StartCoroutine(Snowstorm());
        }
        private System.Collections.IEnumerator Snowstorm()
        {
            snowstormCoroutineRunning = true;
            int stormDuration = 10;
            if (!chargedParticlesPlaying && !isPocketed)
            {
                chargedSnowParticles.Play();
                chargedParticlesPlaying = true;
            }
            else
            {
                chargedParticlesPreviouslyPlaying = true;
            }
            snowstormAudio.Play();
            while (stormDuration > 0)
            {
                yield return new WaitForSeconds(1f);
                snowstormParticles.Play();
                stormDuration--;
                //print("US - Applying frost stack");
                CheckForPlayerAndCallServerRpc();
            }
            charge = 0;
            if (chargedParticlesPlaying == true)
            {
                chargedParticlesPlaying = false;
                chargedSnowParticles.Stop();
                chargedParticlesPreviouslyPlaying = false;
            }
            if (!activateCoroutineRunning)
            {
                activateCoroutine = StartCoroutine(WaitToActivate());
            }
            snowstormCoroutineRunning = false;
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
        public override void OnBroughtToShip()
        {
            base.OnBroughtToShip();
            if (disabledInShip == true)
            {
                if (activateCoroutineRunning)
                {
                    StopCoroutine(activateCoroutine);
                }
                if (snowstormCoroutineRunning)
                {
                    StopCoroutine(stormCoroutine);
                }
                activateCoroutineRunning = false;
                snowstormCoroutineRunning = false;

                chargingSnowflakeParticles.Stop();
                chargingParticlesPlaying = false;
                charge = 0;

                chargedParticlesPlaying = false;
                chargedParticlesPreviouslyPlaying = false;
                chargedSnowParticles.Stop();
                snowstormAudio.Stop();

                if (!snowstormCoroutineRunning && !passiveSnowParticlesPlaying)
                {
                    StartCoroutine(PlayRandomSnowParticle());
                }
            }
        }
    }
}

