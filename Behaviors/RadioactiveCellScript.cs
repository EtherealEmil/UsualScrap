using System.Threading.Tasks;
using System;
using UnityEngine;
using Unity.Netcode;

namespace UsualScrap.Behaviors
{
    internal class RadioactiveCellScript : GrabbableObject
    {
        Light light;
        ParticleSystem particle;
        GameObject Crystal;
        bool coroutinerunning = false;
        Coroutine coroutine;
        int timesDamaged = 0;
        UnityEngine.Color setColor;

        public void Awake()
        {
            light = GetComponentInChildren<Light>();
            particle = GetComponentInChildren<ParticleSystem>();
            Crystal = this.transform.Find("RadioactiveCellModel").gameObject.transform.Find("Crystal").gameObject;
            
        }
        public override void Start()
        {
            base.Start();

            SetColorServerRPC();
        }
        [ServerRpc(RequireOwnership = false)]
        public void SetColorServerRPC()
        {
            byte ColorRRoll = (byte)new System.Random().Next(1, 160);
            byte ColorBRoll = (byte)new System.Random().Next(1, 180);

            //byte r = (byte)(.6f / ColorRRoll);
            //byte b = (byte)(.5f / ColorBRoll);

            setColor = new UnityEngine.Color32(ColorRRoll, 255, ColorBRoll, 255);

            SetColorClientRPC(setColor);
        }
        [ClientRpc]
        public void SetColorClientRPC(UnityEngine.Color color)
        {
            var CrystalMesh = Crystal.GetComponent<MeshRenderer>().material;
            CrystalMesh.color = color;
            CrystalMesh.SetColor("_EmissionColor", color);

            light.color = color;
        }

        public void ToggleLights(bool toggle)
        {
            light.enabled = toggle;
        }
        public override void PocketItem()
        {
            base.PocketItem();
            ToggleLights(false);
            particle.Stop();
            particle.Clear();
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            if (coroutinerunning)
            {
                StopCoroutine(coroutine);
                timesDamaged = 0;
            }
            ToggleLights(true);
            particle.Play();
        }
        public override void EquipItem()
        {
            base.EquipItem();
            ToggleLights(true);
            particle.Play();
        }
        public async override void GrabItem()
        {
            base.GrabItem();
            if (isHeld)
            {
                await Task.Delay(TimeSpan.FromSeconds(2.5f));
                if (isHeld)
                {
                    playerHeldBy.DamagePlayer(5);
                    coroutine = StartCoroutine(DamageHolder());
                }
            }
        }
        private System.Collections.IEnumerator DamageHolder()
        {
            coroutinerunning = true;
            while (isHeld)
            {
                yield return new WaitForSeconds(5f);
                if (!isHeld)
                {
                    yield break;
                }
                if (timesDamaged < 1)
                {
                    playerHeldBy.DamagePlayer(5);
                }
                else if (timesDamaged == 1)
                {
                    playerHeldBy.DamagePlayer(10);
                }
                else if (timesDamaged == 2 || timesDamaged == 3)
                {
                    playerHeldBy.DamagePlayer(15);
                }
                else if (timesDamaged > 3)
                {
                    playerHeldBy.DamagePlayer(20);
                }
                timesDamaged++;
            }
            coroutinerunning = false;
        }
    }
}
