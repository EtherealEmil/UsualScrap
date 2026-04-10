using GameNetcodeStuff;
using System;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;


namespace UsualScrap.Behaviors
{
    public class FuelCylinderScript : GrabbableObject, IHittable
    {
        bool activated = false;
        bool hasBeenGrabbed = false;
        bool coroutinerunning = false;
        float explosionTimer = new System.Random().Next(120, 180);
        int activeDropsRemaining = 3;
        int inactiveDropsRemaining = 3;
        Coroutine coroutine;
        bool pitchcoroutinerunning;
        ParticleSystem FireEffect;
        ParticleSystem WildFireEffect;
        ParticleSystem SmokeEffect;
        AudioSource FireSound;


        public void Awake()
        {
            FireEffect = this.transform.Find("FireEffect").GetComponentInChildren<ParticleSystem>();
            WildFireEffect = this.transform.Find("WildFireEffect").GetComponentInChildren<ParticleSystem>();
            SmokeEffect = this.transform.Find("SmokeEffect").GetComponentInChildren<ParticleSystem>();
            AudioSource[] Sounds = this.transform.Find("FuelCylinderSounds").gameObject.GetComponents<AudioSource>();
            FireSound = Sounds[0];
        }
        public override void GrabItem()
        {
            if (hasBeenGrabbed == false && !isInShipRoom && !activated)
            {
                int chanceToActivate = new System.Random().Next(1, 9);
                SmokeEffect.Play();
                if (chanceToActivate is 1)
                {
                    return;
                }
                else
                {
                    ToggleStateServerRpc(true);
                }
            }
        }
        private System.Collections.IEnumerator Countdown(bool State, float Timer)
        {
            coroutinerunning = true;
            float coroutineExplosionTimer;
            if (Timer == 0)
            {
                coroutineExplosionTimer = new System.Random().Next(120, 180);
            }
            else
            {
                coroutineExplosionTimer = Timer;
            }
            while (coroutineExplosionTimer > 0 && State == true)
            {
                yield return new WaitForSeconds(1f);
                if (this.isInShipRoom || heldByPlayerOnServer && playerHeldBy.isInHangarShipRoom)
                {
                    ToggleStateServerRpc(false);
                }
                coroutineExplosionTimer--;
                explosionTimer = coroutineExplosionTimer;
                print($"{coroutineExplosionTimer}");
            }
            if (coroutineExplosionTimer <= 0)
            {
                ExplodeServerRpc();
            }
            coroutinerunning = false;
        }
        public bool Hit(int force, Vector3 hitDirection, PlayerControllerB playerWhoHit = null, bool playHitSFX = false, int hitID = -1)
        {
            ExplodeServerRpc();
            return true;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ExplodeServerRpc()
        {
            ExplodeClientRpc();
        }
        [ClientRpc]
        public void ExplodeClientRpc()
        {
            Landmine.SpawnExplosion(transform.position, true, 5.7f, 6.4f);
            if (heldByPlayerOnServer)
            {
                playerHeldBy.DiscardHeldObject();
                if (this.radarIcon != null)
                {
                    UnityEngine.Object.Destroy(this.radarIcon.gameObject);
                }
                Destroy(this.transform.gameObject);
            }
            else
            {
                if (this.radarIcon != null)
                {
                    UnityEngine.Object.Destroy(this.radarIcon.gameObject);
                }
                Destroy(this.transform.gameObject);
            }
            print("Spawning Explosion.");
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            DropsToExplodeServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        public void DropsToExplodeServerRpc()
        {
            DropsToExplodeClientRpc();
        }
        [ClientRpc]
        public void DropsToExplodeClientRpc()
        {
            DropsToExplode();
        }
        public async void DropsToExplode()
        {
            if (!isInShipRoom)
            {
                if (activated)
                {
                    activeDropsRemaining--;
                    if (activeDropsRemaining <= 0)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                        ExplodeServerRpc();
                    }
                    else if (activeDropsRemaining > 0)
                    {
                        Math.Floor(explosionTimer = explosionTimer * .8f);
                        UpdateTimerServerRpc(explosionTimer);
                    }
                }
                else if (!activated)
                {
                    inactiveDropsRemaining--;
                    if (inactiveDropsRemaining <= 0)
                    {
                        ToggleStateServerRpc(true);
                        activeDropsRemaining = 3;
                        inactiveDropsRemaining = 3;
                    }
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void UpdateTimerServerRpc(float timer)
        {
            activated = true;
            if (coroutinerunning)
            {
                StopCoroutine(coroutine);
                coroutinerunning = false;
            }
            if (!coroutinerunning)
            {
                coroutine = StartCoroutine(Countdown(true, timer));
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void ToggleStateServerRpc(bool State)
        {
            activated = State;
            if (State == true)
            {
                if (!coroutinerunning)
                {
                    coroutine = StartCoroutine(Countdown(State, 0));
                }
            }
            else if (State == false)
            {
                if (coroutinerunning)
                {
                    StopCoroutine(coroutine);
                    coroutinerunning = false;
                }
            }

            ToggleStateClientRpc(State);
            StartCoroutine(Pitch());
        }
        [ClientRpc]
        public void ToggleStateClientRpc(bool State)
        {
            activated = State;
            if (State == true)
            {
                hasBeenGrabbed = true;
                ActivateEffects();
            }
            else if (State == false)
            {
                DeactivateEffects();
            }
        }
        public async void ActivateEffects()
        {
            await Task.Delay(TimeSpan.FromSeconds(.4f));
            FireEffect.Play();
            FireSound.Play();
        }
        public void DeactivateEffects()
        {
            FireEffect.Stop();
            WildFireEffect.Stop();
            FireSound.Stop();
            SmokeEffect.Play();
        }
        private System.Collections.IEnumerator Pitch()
        {
            pitchcoroutinerunning = true;
            float i = explosionTimer;
            while (explosionTimer >= 3)
            {
                float e = 1 - (explosionTimer / i);
                FireSound.pitch = Mathf.Lerp(1f, 1.5f, e);
                FireSound.volume = Mathf.Lerp(.3f, .5f, e);

                var fireEffectMain = FireEffect.main;
                fireEffectMain.simulationSpeed = Mathf.Lerp(2f, 4f, e);

                //print($"{explosionTimer} is the changing time");

                yield return null;
            }
            //print("IT'S THE FINAL COUNTDOWN");
            FireEffect.Stop();
            while (explosionTimer is > 0 and < 3)
            {
                float e = 1 - (explosionTimer / i);
                FireSound.pitch = Mathf.Lerp(1.5f, 2f, e) + Random.Range(-0.05f, 0.05f);
                WildFireEffect.Play();
                FireEffect.Stop();

                yield return null;
            }
            pitchcoroutinerunning = false;
        }
    }
}


