using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class ShiftControllerScript : GrabbableObject
    {
        bool teleportSetIndoors;
        Vector3 savedLocation;
        GameObject startScreen;
        GameObject shiftScreen;
        GameObject glitchScreen;
        GameObject cScreenPoor;
        GameObject cScreenAverage;
        GameObject cScreenGreat;
        AudioSource setCoordAudio;
        AudioSource teleportAudio;
        AudioSource glitchAudio;
        bool itemGlitching = false;
        bool itemTeleporting = false;
        float teleportBatteryCost;
        int Stage = 0;

        private int GreatRange;
        private int PoorRange;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;
        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            AudioSource[] Sounds = this.transform.Find("ShiftControllerSounds").gameObject.GetComponents<AudioSource>();
            setCoordAudio = Sounds[0];
            teleportAudio = Sounds[1];
            glitchAudio = Sounds[2];
            startScreen = this.transform.Find("ShiftControllerModel").gameObject.transform.Find("StartScreen").gameObject;
            shiftScreen = this.transform.Find("ShiftControllerModel").gameObject.transform.Find("TeleportScreen").gameObject;
            glitchScreen = this.transform.Find("ShiftControllerModel").gameObject.transform.Find("GlitchScreen").gameObject;
            cScreenPoor = this.transform.Find("ShiftControllerModel").gameObject.transform.Find("CScreenPoor").gameObject;
            cScreenAverage = this.transform.Find("ShiftControllerModel").gameObject.transform.Find("CScreenAverage").gameObject;
            cScreenGreat = this.transform.Find("ShiftControllerModel").gameObject.transform.Find("CScreenGreat").gameObject;
            this.insertedBattery = new Battery(false, 1f);
            teleportBatteryCost = this.insertedBattery.charge / 5 ;
            GreatRange = (BoundConfig.ShiftControllerGreatRange.Value); 
            PoorRange = (BoundConfig.ShiftControllerPoorRange.Value);
            if (GreatRange >= (PoorRange - 5) || PoorRange <= (GreatRange + 5))
            {
                print("US - The Shift Controller's great/first connection range cannot be too close, higher than, or equal to the poor/last connection range! setting value to default.");
                print("US - The Shift Controller's poor/last connection range cannot be too close to, lower than, or equal to the great/first connection range! setting value to default.");
                GreatRange = 50;
                PoorRange = 175;
            }
        }
        public override void ChargeBatteries()
        {
            if (playerHeldBy != null && !itemGlitching)
            {
                GlitchServerRpc(3);
                this.insertedBattery.empty = false;
                this.insertedBattery.charge = 1;
            }
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            //print($"{GreatRange}");
            //print($"{PoorRange}");
            if (buttonDown && !itemGlitching && !itemTeleporting)
            {
                if (Stage == 0)
                {
                    SetStageServerRpc(1);
                }
                else if (Stage == 1)
                {

                    SetStageServerRpc(2);
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void ToggleCoroutineServerRpc(bool toggle)
        {
            ToggleCoroutine(toggle);
        }
        public void ToggleCoroutine(bool toggle)
        {
            if (toggle == true)
            {
                StartCoroutine(CheckDistance());
            }
            else if (toggle == false)
            {
                StopAllCoroutines();
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void SetStageServerRpc(int stage)
        {
            SetStageClientRpc(stage);
        }
        [ClientRpc]
        public void SetStageClientRpc(int stage)
        {
            SetStage(stage);
        }
        public void SetStage(int setStage)
        {
            if (StartOfRound.Instance.inShipPhase && !this.insertedBattery.empty && !itemGlitching || !StartOfRound.Instance.shipHasLanded && !this.insertedBattery.empty && !itemGlitching)
            {
                GlitchServerRpc(3);
                print(" US - WE ARE LEAVING OR IN ORBIT, CONTROLLER DISABLED!");
                return;
            }
            if (setStage == 1 && !this.insertedBattery.empty && !itemGlitching)
            {
                Stage = 1;

                setCoordAudio.PlayOneShot(setCoordAudio.clip);
                SwitchScreenServerRpc("Start");

                this.isBeingUsed = true;

                savedLocation = playerHeldBy.transform.position;

                if (playerHeldBy.isInsideFactory)
                {
                    teleportSetIndoors = true;
                }
                else
                {
                    teleportSetIndoors = false;
                }

                ToggleCoroutineServerRpc(true);
            }
            else if (setStage == 2 && !this.insertedBattery.empty && !itemGlitching)
            {
                Stage = 0;

                SwitchScreenServerRpc("Shift");

                TeleportDelay();
            }
        }

        public async void TeleportDelay()
        {
            itemTeleporting = true;
            ToggleCoroutineServerRpc(false);
            await Task.Delay(TimeSpan.FromSeconds(.5f));
            if (this.heldByPlayerOnServer)
            {
                this.isBeingUsed = false;
                teleportAudio.PlayOneShot(teleportAudio.clip);
                if (teleportSetIndoors)
                {
                    Stage = 0;
                    TeleportServerRpc("Indoors", savedLocation);
                    print("US - Shift Controller teleporting indoors!");
                }
                else
                {
                    Stage = 0;
                    TeleportServerRpc("Outdoors", savedLocation);
                    print("US - Shift Controller teleporting outdoors!");
                }

                if (this.insertedBattery.charge > teleportBatteryCost)
                {
                    this.insertedBattery.charge = this.insertedBattery.charge - teleportBatteryCost;
                }
                else if (this.insertedBattery.charge < teleportBatteryCost)
                {
                    this.isBeingUsed = false;
                    this.insertedBattery.empty = true;
                    this.insertedBattery.charge = 0;
                    ToggleCoroutineServerRpc(false);
                    SwitchScreenServerRpc("Off");
                    Stage = 0;
                }
                if (this.insertedBattery.charge > 0)
                {
                    SwitchScreenServerRpc("Start");
                }
            }
            else
            {
                GlitchServerRpc(6);
            }
            itemTeleporting = false;
        }

        [ServerRpc(RequireOwnership = false)]
        public void GlitchServerRpc(int duration)
        {
            ToggleCoroutine(false);
            GlitchClientRpc(duration);
        }
        [ClientRpc]
        public void GlitchClientRpc(int duration)
        {
            Glitch(duration);
        }
        public async void Glitch(int duration)
        {
            itemGlitching = true;
            this.isBeingUsed = false;
            Stage = 0;

            SwitchScreenServerRpc("Glitch");
            glitchAudio.Play();

            await Task.Delay(TimeSpan.FromSeconds(duration));

            SwitchScreenServerRpc("Start");
            glitchAudio.Stop();
            itemGlitching = false;
        }
        [ServerRpc(RequireOwnership = false)]
        public void SwitchScreenServerRpc(string Screen)
        {
            SwitchScreenClientRpc(Screen);
        }
        [ClientRpc]
        public void SwitchScreenClientRpc(string Screen)
        {
            SwitchScreen(Screen);
        }

        private System.Collections.IEnumerator CheckDistance()
        {
            SwitchScreenServerRpc("Great");
            int glitchChance = 0;
            int averageGlitchCuchion = 10;
            while (!itemGlitching && Stage == 1)
            {
                yield return new WaitForSeconds(1);
                if (!heldByPlayerOnServer)
                {
                    yield return new WaitUntil(() => heldByPlayerOnServer);
                }
                float distance = Vector3.Distance(playerHeldBy.transform.position, savedLocation);
                if (distance <= GreatRange)
                {
                    SwitchScreenServerRpc("Great");
                    glitchChance = 0;
                    //print("Player is close to me");
                }
                else if (distance > GreatRange && distance < PoorRange)
                {
                    SwitchScreenServerRpc("Average");
                    glitchChance = 0;
                    //print("Player is a ways away from me");
                }
                else if (distance >= PoorRange)
                {
                    SwitchScreenServerRpc("Poor");
                    glitchChance = 25;
                    //print("Player is far from me");
                }
                if (glitchChance == 25)
                {
                    if (averageGlitchCuchion > 0)
                    {
                        averageGlitchCuchion--;
                    }
                    if (averageGlitchCuchion <= 0)
                    {
                        int diceRoll = new System.Random().Next(0, 5);
                        //print($"{diceRoll}");
                        if (diceRoll is 1)
                        {
                            GlitchServerRpc(6);
                            averageGlitchCuchion = 10;
                        }
                    }
                }
                glitchChance = 0;
            }
        }

        public void SwitchScreen(string Screen)
        {
            if (Screen == "Off")
            {
                glitchScreen.SetActive(false);
                startScreen.SetActive(false);
                shiftScreen.SetActive(false);
                cScreenPoor.SetActive(false);
                cScreenAverage.SetActive(false);
                cScreenGreat.SetActive(false);

            }
            else if (Screen == "Start")
            {
                glitchScreen.SetActive(false);
                startScreen.SetActive(true);
                shiftScreen.SetActive(false);
                cScreenPoor.SetActive(false);
                cScreenAverage.SetActive(false);
                cScreenGreat.SetActive(false);
            }
            else if (Screen == "Shift")
            {
                glitchScreen.SetActive(false);
                startScreen.SetActive(false);
                shiftScreen.SetActive(true);
                cScreenPoor.SetActive(false);
                cScreenAverage.SetActive(false);
                cScreenGreat.SetActive(false);
            }
            else if (Screen == "Poor")
            {
                glitchScreen.SetActive(false);
                startScreen.SetActive(false);
                shiftScreen.SetActive(false);
                cScreenPoor.SetActive(true);
                cScreenAverage.SetActive(false);
                cScreenGreat.SetActive(false);
            }
            else if (Screen == "Average")
            {
                glitchScreen.SetActive(false);
                startScreen.SetActive(false);
                shiftScreen.SetActive(false);
                cScreenPoor.SetActive(false);
                cScreenAverage.SetActive(true);
                cScreenGreat.SetActive(false);
            }
            else if (Screen == "Great")
            {
                glitchScreen.SetActive(false);
                startScreen.SetActive(false);
                shiftScreen.SetActive(false);
                cScreenPoor.SetActive(false);
                cScreenAverage.SetActive(false);
                cScreenGreat.SetActive(true);
            }
            else if (Screen == "Glitch")
            {
                glitchScreen.SetActive(true);
                startScreen.SetActive(false);
                shiftScreen.SetActive(false);
                cScreenPoor.SetActive(false);
                cScreenAverage.SetActive(false);
                cScreenGreat.SetActive(false);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void TeleportServerRpc(string Area, Vector3 Location)
        {
            if (Area == null || Location == null)
            {
                print("US - Shift Controller Teleport location returned null. Resetting..");
                return;
            }
            if(IsOwner)
            {
                Teleport(Area, Location);
            }
            else
            {
                TeleportClientRpc(Area, Location);
            }
        }
        [ClientRpc]
        public void TeleportClientRpc(string Area, Vector3 Location)
        {
            Teleport(Area, Location);
        }
        public void Teleport(string Area, Vector3 Location)
        {
            if (Area == "Indoors" && this.playerHeldBy != null)
            {
                playerHeldBy.TeleportPlayer(Location, false, 0f, false, true);
                this.playerHeldBy.averageVelocity = 0f;
                this.playerHeldBy.velocityLastFrame = Vector3.zero;
                this.playerHeldBy.isInElevator = false;
                this.playerHeldBy.isInHangarShipRoom = false;
                this.playerHeldBy.isInsideFactory = true;
            }
            else if (Area == "Outdoors" && this.playerHeldBy != null)
            {
                playerHeldBy.TeleportPlayer(Location, false, 0f, false, true);
                this.playerHeldBy.averageVelocity = 0f;
                this.playerHeldBy.velocityLastFrame = Vector3.zero;
                this.playerHeldBy.isInElevator = true;
                this.playerHeldBy.isInHangarShipRoom = true;
                this.playerHeldBy.isInsideFactory = false;
            }
        }
    }
}
