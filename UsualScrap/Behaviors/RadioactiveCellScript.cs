using UnityEngine;
using Unity.Netcode;

namespace UsualScrap.Behaviors
{
    public class RadioactiveCellScript : GrabbableObject
    {
        Light light;
        ParticleSystem particle;
        GameObject Crystal;
        bool coroutinerunning = false;
        Coroutine coroutine;
        int damageRamp;
        Color setColor;

        public void Awake()
        {
            light = GetComponentInChildren<Light>();
            particle = GetComponentInChildren<ParticleSystem>();
            Crystal = this.transform.Find("RadioactiveCellModel").gameObject.transform.Find("Crystal").gameObject;
        }
        public override void Start()
        {
            base.Start();

            SetColorServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        public void SetColorServerRpc()
        {
            byte ColorRRoll = (byte)new System.Random().Next(1, 160);
            byte ColorBRoll = (byte)new System.Random().Next(1, 180);

            setColor = new Color32(ColorRRoll, 255, ColorBRoll, 255);

            SetColorClientRpc(setColor);
        }
        [ClientRpc]
        public void SetColorClientRpc(Color color)
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
                coroutinerunning = false;
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
        public override void GrabItem()
        {
            base.GrabItem();
            if (coroutinerunning == false)
            {
                coroutine = StartCoroutine(DamageHolder());
            }
        }
        private System.Collections.IEnumerator DamageHolder()
        {
            coroutinerunning = true;
            damageRamp = 5;
            yield return new WaitForSeconds(2.5f);
            playerHeldBy.DamagePlayer(damageRamp);
            while (playerHeldBy != null)
            {
                yield return new WaitForSeconds(5f);
                if (playerHeldBy == null)
                {
                    yield break;
                }
                playerHeldBy.DamagePlayer(damageRamp);
                damageRamp += 5;
            }
            coroutinerunning = false;
        }
    }
}

