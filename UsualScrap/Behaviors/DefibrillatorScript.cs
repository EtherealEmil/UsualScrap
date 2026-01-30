using GameNetcodeStuff;
using System.Threading.Tasks;
using System;
using Unity.Netcode;
using UnityEngine;
using System.Linq;

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

        bool RequiresBattery;
        bool UsesLimited;
        bool PermaDeathRule;
        bool DefibrillatorRefillsOnLanding;
        int useLimit;
        //public NetworkVariable<int> useLimit = new NetworkVariable<int>(3, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        public void Awake()
        {
            BoundConfig = Plugin.BoundConfig;
            RequiresBattery = (BoundConfig.DefibrillatorRequiresBattery.Value);
            if (RequiresBattery)
            {
                this.insertedBattery = new Battery(false, 1f);
                batteryCost = this.insertedBattery.charge / 2;
            }
            else
            {
                this.itemProperties.requiresBattery = false;
            }
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
            useLimit = (BoundConfig.DefibrillatorUseLimit.Value);
            PermaDeathRule = (BoundConfig.DefibrillatorPermaDeathRule.Value);
            DefibrillatorRefillsOnLanding = (BoundConfig.DefibrillatorRefillsOnLanding.Value);
            if (UsesLimited == true && useLimit < 1)
            {
                print("US - US_Defibrillator savedUses enabled but number of savedUses less than the minimum of 1! setting savedUses to 1.");
                useLimit = 1;
            }
        }
        /*
        public override int GetItemDataToSave()
        {
            base.GetItemDataToSave();
            return useLimit.Value;
        }

        public override void LoadItemSaveData(int saveData)
        {
            base.LoadItemSaveData(saveData);
            useLimit.Value = saveData;
        }
        */
        public override void Update()
        {
            base.Update();
            if (DefibrillatorRefillsOnLanding && StartOfRound.Instance.inShipPhase && useLimit != BoundConfig.DefibrillatorUseLimit.Value)
            {
                useLimit = BoundConfig.DefibrillatorUseLimit.Value;
            }
        }
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            if (buttonDown)
            {
                if (RequiresBattery && !this.insertedBattery.empty)
                {
                    foreach (Renderer display in displayRenderers)
                    {
                        display.material.SetColor("_EmissiveColor", Color.black);
                    }
                    if (UsesLimited == true && useLimit <= 0)
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
                else if (!RequiresBattery)
                {
                    foreach (Renderer display in displayRenderers)
                    {
                        display.material.SetColor("_EmissiveColor", Color.black);
                    }
                    if (UsesLimited == true && useLimit <= 0)
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
            }
            if (!buttonDown)
            {
                ReadyingDefib = false;
            }
        }
        private System.Collections.IEnumerator ReadyDefib()
        {
            timeToHold = 20;
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
                if (timeToHold > 15)
                {
                    DisplayOneRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold is < 15 and > 10)
                {
                    DisplayTwoRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold is < 10 and > 5)
                {
                    DisplayThreeRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold is < 5 and > 0)
                {
                    DisplayFourRenderer.material.SetColor("_EmissiveColor", Color.green);
                }
                if (timeToHold == 0 && ReadyingDefib)
                {
                    DisplayFiveRenderer.material.SetColor("_EmissiveColor", Color.green);
                    ChargedAudio.PlayOneShot(ChargedAudio.clip);
                    yield return new WaitUntil(() => !ReadyingDefib || !isHeld || isPocketed);
                    ShockAudio.PlayOneShot(ShockAudio.clip);
                    HUDManager.Instance.ShakeCamera(ScreenShakeType.Small);
                    if (RequiresBattery)
                    {
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
        public void FindBody()
        {
            RaycastHit[] hits = Physics.SphereCastAll(this.playerHeldBy.gameplayCamera.transform.position, 3 , this.playerHeldBy.gameplayCamera.transform.forward, 5, LayerMask.GetMask("PlayerRagdoll"));
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if(hit.transform.TryGetComponent<DeadBodyInfo>(out DeadBodyInfo deadBodyInfo))
                {
                    var ID = deadBodyInfo.playerObjectId;
                    PlayerControllerB playerScript = RoundManager.Instance.playersManager.allPlayerScripts[ID];
                    Vector3 HitLocation = hit.transform.position + Vector3.up * .25f;
                    if (PermaDeathRule)
                    {
                        if (deadBodyInfo.causeOfDeath == CauseOfDeath.Snipped || deadBodyInfo.detachedHead == true)
                        {
                            print("US - This corpse can't be saved. What a mess.");
                            return;
                        }
                    }
                    if (playerScript != null && playerScript.isPlayerDead && !playerScript.currentlyHeldObject)
                    {
                        RevivePlayerServerRpc(ID, HitLocation);
                        if (IsHost)
                        {
                            DeadBodyInfo[] array1 = UnityEngine.Object.FindObjectsOfType<DeadBodyInfo>(true);
                            foreach (DeadBodyInfo dead in array1)
                            {
                                if (dead.playerObjectId == ID)
                                {
                                    RagdollGrabbableObject[] array2 = UnityEngine.Object.FindObjectsOfType<RagdollGrabbableObject>(true);
                                    foreach (RagdollGrabbableObject ragdoll in array2)
                                    {
                                        if (ragdoll.ragdoll.playerObjectId == dead.playerObjectId)
                                        {
                                            Destroy(ragdoll.transform.gameObject);
                                        }
                                    }
                                }
                            }
                        }
                        else if (IsClient)
                        {
                            DestroyBodyServerRpc(ID);
                        }
                        break;
                    }
                }
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void DestroyBodyServerRpc(int ID)
        {
            DeadBodyInfo[] array1 = UnityEngine.Object.FindObjectsOfType<DeadBodyInfo>(true);
            foreach (DeadBodyInfo dead in array1)
            {
                if (dead.playerObjectId == ID)
                {
                    RagdollGrabbableObject[] array2 = UnityEngine.Object.FindObjectsOfType<RagdollGrabbableObject>(true);
                    foreach (RagdollGrabbableObject ragdoll in array2)
                    {
                        if (ragdoll.ragdoll.playerObjectId == dead.playerObjectId)
                        {
                            Destroy(ragdoll.transform.gameObject);
                        }
                    }
                }
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
            if (GameNetworkManager.Instance.localPlayerController.isPlayerDead)
            {
                HUDManager.Instance.UpdateBoxesSpectateUI();
                //HUDManager.Instance.UpdateSpectateBoxSpeakerIcons();
                //Look into accessing ^ tooooo tirrrred!!!
            }

        }
        public void RevivePlayer(int PlayerID, Vector3 SpawnPosition)
        {
            if (PlayerID < 0)
            {
                print("US - No player inital id? returning..");
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
                print("US - Player inital is dead, reviving them!");
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

            if (UsesLimited == true && useLimit > 0)
            {
                useLimit--;
                if (useLimit <= 0)
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
