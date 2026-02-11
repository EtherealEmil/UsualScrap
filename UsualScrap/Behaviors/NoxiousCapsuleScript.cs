using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UsualScrap.Behaviors.Effects;

namespace UsualScrap.Behaviors
{
    public class NoxiousCapsuleScript : GrabbableObject
    {
        ParticleSystem ambientSporeParticles;
        ParticleSystem noxiousCloudParticles;
        ParticleSystem gasParticles;
        Coroutine activatingCoroutine;
        Coroutine noxCloudCoroutine;
        //AudioSource[] Sounds;
        //AudioSource snowstormAudio;
        bool activatingCoroutineRunning = false;
        bool NoxCloudCoroutineRunning = false;
        bool gasParticlesPlaying = false;
        bool ambientParticlesPlaying = true;
        bool gasParticlesPreviouslyPlaying = false;
        Light light;
        int charge = 0;
        bool disabledInShip;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;
        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            ambientSporeParticles = this.transform.Find("AmbientSporeParticles").GetComponent<ParticleSystem>();
            noxiousCloudParticles = this.transform.Find("NoxiousCloudParticles").GetComponent<ParticleSystem>();
            gasParticles = this.transform.Find("ActiveGasParticles").GetComponent<ParticleSystem>();
            light = this.transform.Find("Point Light").GetComponent<Light>();
            //Sounds = this.transform.Find("NoxSounds").gameObject.GetComponents<AudioSource>();
            //Audio = Sounds[0];
            disabledInShip = (BoundConfig.CapsulesDisabledOnTheShip.Value);
        }
        public override void PocketItem()
        {
            base.PocketItem();
            if (ambientParticlesPlaying)
            {
                ambientSporeParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                ambientParticlesPlaying = false;
            }
            if (gasParticlesPlaying)
            {
                gasParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                gasParticlesPlaying = false;
                gasParticlesPreviouslyPlaying = true;
            }
            if (light.enabled)
            {
                light.enabled = false;
            }
        }
        public override void EquipItem()
        {
            base.EquipItem();
            if(!ambientParticlesPlaying)
            {
                ambientSporeParticles.Play();
                ambientParticlesPlaying = true;
            }
            if (gasParticlesPreviouslyPlaying && !gasParticlesPlaying)
            {
                gasParticles.Play();
                gasParticlesPlaying = true;
                gasParticlesPreviouslyPlaying = false;
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
                ambientSporeParticles.Play();
                ambientParticlesPlaying = true;
            }
            if (gasParticlesPreviouslyPlaying && !gasParticlesPlaying)
            {
                gasParticles.Play();
                gasParticlesPlaying = true;
                gasParticlesPreviouslyPlaying = false;
            }
            if (!light.enabled)
            {
                light.enabled = true;
            }
        }
        public override void Update()
        {
            base.Update();
            if (!StartOfRound.Instance.shipHasLanded && activatingCoroutineRunning || !StartOfRound.Instance.shipHasLanded && NoxCloudCoroutineRunning)
            {
                if (activatingCoroutineRunning)
                {
                    StopCoroutine(activatingCoroutine);
                }
                if (NoxCloudCoroutineRunning)
                {
                    StopCoroutine(noxCloudCoroutine);
                }
                activatingCoroutineRunning = false;
                NoxCloudCoroutineRunning = false;

                charge = 0;

                gasParticlesPlaying = false;
                gasParticlesPreviouslyPlaying = false;
                gasParticles.Stop();
                //Audio.Stop();

            }
            else if (disabledInShip == true && this.isInShipRoom)
            {
                return;
            }
            else if (StartOfRound.Instance.shipHasLanded && !activatingCoroutineRunning && !NoxCloudCoroutineRunning && TimeOfDay.Instance.currentLevel.planetHasTime)
            {
                activatingCoroutine = StartCoroutine(Activating());
            }
        }

        private System.Collections.IEnumerator Activating()
        {
            activatingCoroutineRunning = true;
            if (TimeOfDay.Instance.dayMode != DayMode.Dawn || TimeOfDay.Instance.dayMode != DayMode.Noon)
            {
                yield return new WaitUntil(() => TimeOfDay.Instance.dayMode == DayMode.Dawn || TimeOfDay.Instance.dayMode == DayMode.Noon);
            }
            while (TimeOfDay.Instance.dayMode == DayMode.Dawn && !NoxCloudCoroutineRunning || TimeOfDay.Instance.dayMode == DayMode.Noon && !NoxCloudCoroutineRunning)
            {
                yield return new WaitForSeconds(1);
                charge++;
                //print($"{charge}");
                if (isInFactory)
                {
                    ambientSporeParticles.Play();
                    yield return new WaitUntil(() => !isInFactory);
                }
                if (charge >= 20 && !NoxCloudCoroutineRunning)
                {
                    NoxCloudCoroutineServerRpc();
                    activatingCoroutineRunning = false;
                    yield break;
                }
            }
            activatingCoroutineRunning = false;
        }
        [ServerRpc(RequireOwnership = false)]
        public void NoxCloudCoroutineServerRpc()
        {
            NoxCloudCoroutineClientRpc();
        }
        [ClientRpc]
        public void NoxCloudCoroutineClientRpc()
        {
            noxCloudCoroutine = StartCoroutine(NoxCloud());
        }
        private System.Collections.IEnumerator NoxCloud()
        {
            NoxCloudCoroutineRunning = true;
            int noxDuration = 20;
            if (!gasParticlesPlaying && !isPocketed)
            {
                gasParticles.Play();
                gasParticlesPlaying = true;
            }
            else
            {
                gasParticlesPreviouslyPlaying = true;
            }
            //Audio.Play();
            while (noxDuration > 0)
            {
                yield return new WaitForSeconds(1f);
                noxiousCloudParticles.Play();
                noxDuration--;
                Collider[] playerArray = Physics.OverlapSphere(this.transform.position, 3, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide);
                HashSet<PlayerControllerB> Affected = new HashSet<PlayerControllerB>();
                foreach (Collider playerCollider in playerArray)
                {
                    PlayerControllerB playerControllerB = playerCollider.gameObject.GetComponent<PlayerControllerB>();
                    playerControllerB.DamagePlayer(5);
                }
            }
            charge = 0;
            if (gasParticlesPlaying == true)
            {
                gasParticlesPlaying = false;
                gasParticles.Stop();
                gasParticlesPreviouslyPlaying = false;
            }
            if (!activatingCoroutineRunning)
            {
                activatingCoroutine = StartCoroutine(Activating());
            }
            NoxCloudCoroutineRunning = false;
        }
        public override void OnBroughtToShip()
        {
            base.OnBroughtToShip();
            if (disabledInShip == true)
            {
                if (activatingCoroutineRunning)
                {
                    StopCoroutine(activatingCoroutine);
                }
                if (NoxCloudCoroutineRunning)
                {
                    StopCoroutine(noxCloudCoroutine);
                }
                activatingCoroutineRunning = false;
                NoxCloudCoroutineRunning = false;

                charge = 0;

                gasParticlesPlaying = false;
                gasParticlesPreviouslyPlaying = false;
                gasParticles.Stop();
                //Audio.Stop();

            }
        }
    }
}
