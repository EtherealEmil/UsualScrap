using GameNetcodeStuff;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class HandlampScript : GrabbableObject
    {
        Light[] lights;
        Light gh;
        Light g;
        bool lightstate = false;
        bool previouslightstate = false;
        GameObject Bulb;
        AudioSource LightOnSound;
        AudioSource LightOffSound;
        PlayerControllerB previousPlayerHeldBy;


        public void Awake()
        {
            Bulb = this.transform.Find("HandlampModel").gameObject.transform.Find("Bulb").gameObject;
            AudioSource[] Sounds = this.transform.Find("HandlampSounds").gameObject.GetComponents<AudioSource>();
            LightOnSound = Sounds[0];
            LightOffSound = Sounds[1];
            this.insertedBattery = new Battery(false, 1f);
            lights = GetComponentsInChildren<Light>();
            gh = lights[1];
        }
        public override void GrabItem()
        {
            base.GrabItem();
            previousPlayerHeldBy = playerHeldBy;
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
            previousPlayerHeldBy = null;
            if (g != null)
            {
                Destroy(g.gameObject); 
            }
        }
        public void ToggleLights(bool toggle)
        {
            lightstate = toggle;
            foreach (Light light in lights)
            {
                light.enabled = toggle;
            }
            if (toggle == true)  
            {
                LightOnSound.PlayOneShot(LightOnSound.clip);
                Bulb.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.white);
            }
            if (toggle == false)
            {
                LightOffSound.PlayOneShot(LightOffSound.clip);
                Bulb.GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.black);
            }
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (lightstate == false && this.insertedBattery.charge > 0)
            {
                this.ToggleLights(true);
                this.isBeingUsed = true;
                previouslightstate = true;
            }
            else if (this.insertedBattery.charge > 0)
            {
                this.ToggleLights(false);
                this.isBeingUsed = false;
                previouslightstate = false;
            }
        }
        public override void UseUpBatteries() 
        {
            base.UseUpBatteries();
            this.ToggleLights(false);
            this.isBeingUsed = false;
            previouslightstate = false;
        }
        public override void PocketItem()
        {
            base.PocketItem();
            if (lightstate == true)
            {
                this.ToggleLights(false);
                if (g == null)
                {
                    g = Instantiate(gh, previousPlayerHeldBy.gameplayCamera.transform);
                    g.enabled = true;
                }
                else
                {
                    g.enabled = true;
                }
            }
        }
        public override void EquipItem()
        {
            base.EquipItem();
            if (previouslightstate == true && lightstate != true && this.insertedBattery.charge > 0)
            {
                this.ToggleLights(true);
                if (g != null)
                {
                    g.enabled = false;
                }
            }
        }
    }
}
