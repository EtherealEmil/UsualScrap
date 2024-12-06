using GameNetcodeStuff;
using System.Threading.Tasks;
using System;
using Unity.Netcode;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    internal class DefibrillatorScript : GrabbableObject
    {
        Coroutine coroutine;
        bool ReadyingDefib = false;
        Renderer[] displayRenderers;
        GameObject ChargeDisplayOne;
        GameObject ChargeDisplayTwo;
        GameObject ChargeDisplayThree;
        GameObject ChargeDisplayFour;
        GameObject ChargeDisplayFive;
        Renderer DisplayOneRenderer;
        Renderer DisplayTwoRenderer;
        Renderer DisplayThreeRenderer;
        Renderer DisplayFourRenderer;
        Renderer DisplayFiveRenderer;
        AudioSource ChargedAudio;
        AudioSource ShockAudio;
        int timeToHold;
        float batteryCost;


        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        bool UsesLimited;
        int UseLimit;

        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            this.insertedBattery = new Battery(false, 1f);
            batteryCost = this.insertedBattery.charge / 2;
            AudioSource[] Sounds = this.transform.Find("DefibrillatorSounds").gameObject.GetComponents<AudioSource>();
            ChargedAudio = Sounds[0];
            ShockAudio = Sounds[1];
            ChargeDisplayOne = this.transform.Find("DefibrillatorModel").gameObject.transform.Find("ChargeDisplayOne").gameObject;
            ChargeDisplayTwo = this.transform.Find("DefibrillatorModel").gameObject.transform.Find("ChargeDisplayTwo").gameObject;
            ChargeDisplayThree = this.transform.Find("DefibrillatorModel").gameObject.transform.Find("ChargeDisplayThree").gameObject;
            ChargeDisplayFour = this.transform.Find("DefibrillatorModel").gameObject.transform.Find("ChargeDisplayFour").gameObject;
            ChargeDisplayFive = this.transform.Find("DefibrillatorModel").gameObject.transform.Find("ChargeDisplayFive").gameObject;
            DisplayOneRenderer = ChargeDisplayOne.GetComponent<Renderer>();
            DisplayTwoRenderer = ChargeDisplayTwo.GetComponent<Renderer>();
            DisplayThreeRenderer = ChargeDisplayThree.GetComponent<Renderer>();
            DisplayFourRenderer = ChargeDisplayFour.GetComponent<Renderer>();
            DisplayFiveRenderer = ChargeDisplayFive.GetComponent<Renderer>();
            displayRenderers = [DisplayOneRenderer, DisplayTwoRenderer, DisplayThreeRenderer, DisplayFourRenderer, DisplayFiveRenderer];
            UsesLimited = (BoundConfig.DefibrillatorUsesLimited.Value);
            UseLimit = (BoundConfig.DefibrillatorUseLimit.Value);
            if (UsesLimited == true && UseLimit < 1)
            {
                print("Defibrillator uses enabled but number of uses less than the minimum of 1! setting uses to 1.");
                UseLimit = 1;
            }
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            if (buttonDown && !this.insertedBattery.empty)
            {
                //print($"Uses remaining: {UseLimit}");
                foreach (Renderer display in displayRenderers)
                {
                    display.material.SetColor("_EmissiveColor", Color.black);
                }
                if (UsesLimited == true && UseLimit <= 0)
                {
                    foreach (Renderer display in displayRenderers)
                    {
                        display.material.SetColor("_EmissiveColor", Color.red);
                    }
                    return;
                }
                ReadyingDefib = true;
                coroutine = StartCoroutine(ReadyDefib());
            }
            if (!buttonDown)
            {
                ReadyingDefib = false;
            }
        }
        private System.Collections.IEnumerator ReadyDefib()
        {
            timeToHold = 25;
            isBeingUsed = true;
            while (ReadyingDefib == true)
            {
                yield return new WaitForSeconds(.1f);
                if (!isHeld || isPocketed)
                {
                    ReadyingDefib = false;
                    foreach (Renderer display in displayRenderers)
                    {
                        display.material.SetColor("_EmissiveColor", Color.black);
                    }
                    isBeingUsed = false;
                    yield break;
                }
                if (timeToHold > 0)
                {
                    timeToHold--;
                }
                if (timeToHold is < 25 and > 20)
                {
                    DisplayOneRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold is < 20 and > 15)
                {
                    DisplayTwoRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold is < 15 and > 10)
                {
                    DisplayThreeRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold is < 10 and > 5)
                {
                    DisplayFourRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold < 5)
                {
                    DisplayFiveRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold == 0 && ReadyingDefib)
                {
                    ChargedAudio.PlayOneShot(ChargedAudio.clip);
                    yield return new WaitUntil(() => !ReadyingDefib || !isHeld || isPocketed);
                    ShockAudio.PlayOneShot(ShockAudio.clip);
                    HUDManager.Instance.ShakeCamera(ScreenShakeType.Small);
                    if (this.insertedBattery.charge > batteryCost)
                    {
                        this.insertedBattery.charge = this.insertedBattery.charge - batteryCost;
                    }
                    else if (this.insertedBattery.charge < batteryCost)
                    {
                        this.insertedBattery.empty = true;
                        this.insertedBattery.charge = 0;
                        this.isBeingUsed = false;
                    }
                    if (!isHeld || isPocketed)
                    {
                        ReadyingDefib = false;
                        isBeingUsed = false;
                        foreach (Renderer display in displayRenderers)
                        {
                            display.material.SetColor("_EmissiveColor", Color.black);
                        }
                        yield break;
                    }
                    ReadyingDefib = false;
                    break;
                }
            }
            foreach (Renderer display in displayRenderers)
            {
                display.material.SetColor("_EmissiveColor", Color.black);
            }
            isBeingUsed = false;
            if (isHeld && timeToHold == 0)
            {
                FindBody();
            }
        }
        public async void FindBody()
        {
            RaycastHit[] hits;
            hits = Physics.SphereCastAll(this.playerHeldBy.gameplayCamera.transform.position, 3f , this.playerHeldBy.gameplayCamera.transform.forward, 5F, LayerMask.GetMask("PlayerRagdoll"));
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if(hit.transform.TryGetComponent<DeadBodyInfo>(out DeadBodyInfo deadBodyInfo))
                {
                    var ID = deadBodyInfo.playerObjectId;
                    PlayerControllerB playerScript = RoundManager.Instance.playersManager.allPlayerScripts[ID];
                    Vector3 HitLocation = hit.transform.position + Vector3.up * .25f;
                    if (deadBodyInfo.causeOfDeath == CauseOfDeath.Snipped || deadBodyInfo.detachedHead == true)
                    {
                        print("This corpse can't be saved. What a mess.");
                        foreach (Renderer display in displayRenderers)
                        {
                            display.material.SetColor("_EmissiveColor", Color.red);
                        }
                        await Task.Delay(TimeSpan.FromSeconds(.3f));
                        foreach (Renderer display in displayRenderers)
                        {
                            display.material.SetColor("_EmissiveColor", Color.black);
                        }
                        return;
                    }
                    if (playerScript != null && playerScript.isPlayerDead && !playerScript.currentlyHeldObject)
                    {
                        RevivePlayerServerRpc(ID, HitLocation);
                        if (IsHost)
                        {
                            Destroy(hit.transform.gameObject);
                        }
                        else if (IsClient)
                        {
                            DestroyBodyServerRpc();
                        }
                        i = hits.Length;
                    }
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void DestroyBodyServerRpc()
        {
            DestroyBodyClientRpc();
        }
        [ClientRpc]
        public void DestroyBodyClientRpc()
        {
            RaycastHit[] hits = Physics.SphereCastAll(this.playerHeldBy.gameplayCamera.transform.position, 1f, this.playerHeldBy.gameplayCamera.transform.forward, 5F, LayerMask.GetMask("PlayerRagdoll"));
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Destroy(hit.transform.gameObject);
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void RevivePlayerServerRpc(int ID, Vector3 SpawnPosition)
        {
            RevivePlayerClientRpc(ID, SpawnPosition);
        }
        [ClientRpc]
        public void RevivePlayerClientRpc(int ID, Vector3 SpawnPosition)
        {
            RevivePlayer(ID, SpawnPosition);
        }
        public void RevivePlayer(int PlayerID, Vector3 SpawnPosition)
        {
            if (PlayerID < 0)
            {
                print("No playerInital id? returning.");
                return;
            }
            PlayerControllerB PlayerScript = RoundManager.Instance.playersManager.allPlayerScripts[PlayerID];
            if (!PlayerScript.isPlayerDead)
            {
                return;
            }

            if (playerHeldBy.isInsideFactory)
            {
                PlayerScript.isInsideFactory = true;
                PlayerScript.isInElevator = false;
                PlayerScript.isInHangarShipRoom = false;
            }
            else
            {
                PlayerScript.isInsideFactory = false;
                PlayerScript.isInElevator = true;
                PlayerScript.isInHangarShipRoom = true;
            }
            
            PlayerScript.ResetPlayerBloodObjects(PlayerScript.isPlayerDead);
            PlayerScript.health = 5;
            PlayerScript.isClimbingLadder = false;
            PlayerScript.clampLooking = false;
            PlayerScript.inVehicleAnimation = false;
            PlayerScript.disableMoveInput = false;
            PlayerScript.disableLookInput = false;
            PlayerScript.disableInteract = false;
            PlayerScript.ResetZAndXRotation();
            PlayerScript.thisController.enabled = true;
            if (PlayerScript.isPlayerDead)
            {
                print("playerInital is dead, reviving them.");
                PlayerScript.thisController.enabled = true;
                PlayerScript.isPlayerDead = false;
                PlayerScript.isPlayerControlled = true;
                PlayerScript.health = 5;
                PlayerScript.hasBeenCriticallyInjured = false;
                PlayerScript.criticallyInjured = false;
                PlayerScript.playerBodyAnimator.SetBool("Limp", value: false);
                PlayerScript.TeleportPlayer(SpawnPosition, false, 0f, false, true);
                PlayerScript.parentedToElevatorLastFrame = false;
                PlayerScript.overrideGameOverSpectatePivot = null;
                StartOfRound.Instance.SetPlayerObjectExtrapolate(enable: false);
                PlayerScript.setPositionOfDeadPlayer = false;
                PlayerScript.DisablePlayerModel(PlayerScript.gameObject, enable: true, disableLocalArms: true);
                PlayerScript.helmetLight.enabled = false;
                PlayerScript.Crouch(crouch: false);
                if (PlayerScript.playerBodyAnimator != null)
                {
                    PlayerScript.playerBodyAnimator.SetBool("Limp", value: false);
                }
                PlayerScript.bleedingHeavily = true;
                PlayerScript.deadBody = null;
                PlayerScript.activatingItem = false;
                PlayerScript.twoHanded = false;
                PlayerScript.inShockingMinigame = false;
                PlayerScript.inSpecialInteractAnimation = false;
                PlayerScript.freeRotationInInteractAnimation = false;
                PlayerScript.disableSyncInAnimation = false;
                PlayerScript.inAnimationWithEnemy = null;
                PlayerScript.holdingWalkieTalkie = false;
                PlayerScript.speakingToWalkieTalkie = false;
                PlayerScript.isSinking = false;
                PlayerScript.isUnderwater = false;
                PlayerScript.sinkingValue = 0f;
                PlayerScript.statusEffectAudio.Stop();
                PlayerScript.DisableJetpackControlsLocally();
                PlayerScript.mapRadarDotAnimator.SetBool("dead", value: false);
                PlayerScript.hasBegunSpectating = false;
                PlayerScript.externalForceAutoFade = Vector3.zero;
                PlayerScript.hinderedMultiplier = 1f;
                PlayerScript.isMovementHindered = 0;
                PlayerScript.sourcesCausingSinking = 0;
                PlayerScript.reverbPreset = StartOfRound.Instance.shipReverb;

                SoundManager.Instance.earsRingingTimer = 0f;
                PlayerScript.voiceMuffledByEnemy = false;
                SoundManager.Instance.playerVoicePitchTargets[PlayerID] = 1f;
                SoundManager.Instance.SetPlayerPitch(1f, PlayerID);

                if (PlayerScript.currentVoiceChatIngameSettings == null)
                {
                    StartOfRound.Instance.RefreshPlayerVoicePlaybackObjects();
                }

                if (PlayerScript.currentVoiceChatIngameSettings != null)
                {
                    if (PlayerScript.currentVoiceChatIngameSettings.voiceAudio == null)
                    {
                        PlayerScript.currentVoiceChatIngameSettings.InitializeComponents();
                    }

                    if (PlayerScript.currentVoiceChatIngameSettings.voiceAudio == null)
                    {
                        return;
                    }

                    PlayerScript.currentVoiceChatIngameSettings.voiceAudio.GetComponent<OccludeAudio>().overridingLowPass = false;
                }
            }
            PlayerControllerB localplayercontroller = GameNetworkManager.Instance.localPlayerController;
            if (localplayercontroller.playerClientId == PlayerScript.playerClientId)
            {
               localplayercontroller.bleedingHeavily = false;
               localplayercontroller.criticallyInjured = false;
               localplayercontroller.health = 5;
               HUDManager.Instance.UpdateHealthUI(5, hurtPlayer: true);
               localplayercontroller.playerBodyAnimator.SetBool("Limp", false);
               localplayercontroller.spectatedPlayerScript = null;
               StartOfRound.Instance.SetSpectateCameraToGameOverMode(false, localplayercontroller);
               StartOfRound.Instance.SetPlayerObjectExtrapolate(false);
               HUDManager.Instance.audioListenerLowPass.enabled = false;
               HUDManager.Instance.gasHelmetAnimator.SetBool("gasEmitting", false);
               HUDManager.Instance.RemoveSpectateUI();
               HUDManager.Instance.gameOverAnimator.SetTrigger("revive");
            }

            StartOfRound.Instance.allPlayersDead = false;
            StartOfRound.Instance.livingPlayers++;
            StartOfRound.Instance.UpdatePlayerVoiceEffects();

            if (UsesLimited == true && UseLimit > 0)
            {
                UseLimit--;
                if (UseLimit <= 0)
                {
                    foreach (Renderer display in displayRenderers)
                    {
                        display.material.SetColor("_EmissiveColor", Color.red);
                    }
                }
            }
        }
    }
}
