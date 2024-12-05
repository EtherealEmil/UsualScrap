using BepInEx;
using HarmonyLib;
using LethalLib.Modules;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UsualScrap.Behaviors;



namespace UsualScrap
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "Emil.UsualScrap";
        public const string PLUGIN_NAME = "UsualScrap";
        public const string PLUGIN_VERSION = "1.7.3";


        public static Plugin instance;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        public void Registerstoreitem(Item name, string description, int price)
        {
            TerminalNode ItemNode = ScriptableObject.CreateInstance<TerminalNode>();
            ItemNode.clearPreviousText = true;
            ItemNode.displayText = $"{description}\n\n";
            Items.RegisterShopItem(name, null, null, ItemNode, price);
        }
         
        public void ScriptSetUp(GrabbableObject script, bool grabbable, bool grabbableToEnemies, bool isInFactory, Item itemProperties)
        {
            script.grabbable = grabbable;
            script.grabbableToEnemies = grabbableToEnemies;
            script.isInFactory = isInFactory;
            script.itemProperties = itemProperties;
        }

        public void Awake()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            Plugin.instance = this;

            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "usualscrapassetbundle"));

            Item US_UnstableFuelCylinder = assetBundle.LoadAsset<Item>("Assets/Items/UnstableFuelCylinderAssets/UnstableFuelCylinder_Item.asset");
            Item US_Handlamp = assetBundle.LoadAsset<Item>("Assets/Items/HandlampAssets/Handlamp_Item.asset");
            Item US_Bandages = assetBundle.LoadAsset<Item>("Assets/Items/BandagesAssets/Bandages_Item.asset");
            Item US_MedicalKit = assetBundle.LoadAsset<Item>("Assets/Items/MedicalKitAssets/MedicalKit_Item.asset");
            Item US_Defibrillator = assetBundle.LoadAsset<Item>("Assets/Items/DefibrillatorAssets/Defibrillator_Item.asset");
            Item US_Toolbox = assetBundle.LoadAsset<Item>("Assets/Items/ToolboxAssets/Toolbox_Item.asset");
            Item US_WalkingCane = assetBundle.LoadAsset<Item>("Assets/Items/WalkingCaneAssets/WalkingCane_Item.asset");
            Item US_RadioactiveCell = assetBundle.LoadAsset<Item>("Assets/Items/RadioactiveCellAssets/RadioactiveCell_Item.asset");
            Item US_Scissors = assetBundle.LoadAsset<Item>("Assets/Items/ScissorsAssets/Scissors_Item.asset");
            Item US_Rose = assetBundle.LoadAsset<Item>("Assets/Items/RoseAssets/Rose_Item.asset");
            Item US_GoldenTicket = assetBundle.LoadAsset<Item>("Assets/Items/GoldenTicketAssets/GoldenTicket_Item.asset");
            Item US_Ticket = assetBundle.LoadAsset<Item>("Assets/Items/TicketAssets/Ticket_Item.asset");
            Item US_Padlock = assetBundle.LoadAsset<Item>("Assets/Items/PadlockAssets/Padlock_Item.asset");
            Item US_PocketWatch = assetBundle.LoadAsset<Item>("Assets/Items/PocketWatchAssets/PocketWatch_Item.asset");
            Item US_Crowbar = assetBundle.LoadAsset<Item>("Assets/Items/CrowbarAssets/Crowbar_Item.asset");
            Item US_CandyDispenser = assetBundle.LoadAsset<Item>("Assets/Items/CandyDispenserAssets/CandyDispenser_Item.asset");
            Item US_Lollipop = assetBundle.LoadAsset<Item>("Assets/Items/CandyAssets/Lollipop_Item.asset");
            Item US_Peppermint = assetBundle.LoadAsset<Item>("Assets/Items/CandyAssets/Peppermint_Item.asset");
            Item US_Caramel = assetBundle.LoadAsset<Item>("Assets/Items/CandyAssets/Caramel_Item.asset");
            Item US_EmergencyInjector = assetBundle.LoadAsset<Item>("Assets/Items/EmergencyInjectorAssets/EmergencyInjector_Item.asset");
            Item US_ShiftController = assetBundle.LoadAsset<Item>("Assets/Items/ShiftControllerAssets/ShiftController_Item.asset");
            Item US_GloomyCapsule = assetBundle.LoadAsset<Item>("Assets/Items/GloomyCapsuleAssets/GloomyCapsule_Item.asset");
            Item US_FrigidCapsule = assetBundle.LoadAsset<Item>("Assets/Items/FrigidCapsuleAssets/FrigidCapsule_Item.asset");
            Item US_SanguineCapsule = assetBundle.LoadAsset<Item>("Assets/Items/SanguineCapsuleAssets/SanguineCapsule_Item.asset");


            List<Item> ItemsList = new List<Item>() {
                US_UnstableFuelCylinder,
                US_Handlamp,
                US_Bandages,
                US_MedicalKit,
                US_Defibrillator,
                US_EmergencyInjector,
                US_ShiftController,
                US_Toolbox,
                US_RadioactiveCell,
                US_GloomyCapsule,
                US_FrigidCapsule,
                US_SanguineCapsule,
                US_Padlock,
                US_Crowbar,
                US_WalkingCane,
                US_Scissors,
                US_Rose,
                US_GoldenTicket,
                US_Ticket,
                US_CandyDispenser,
                US_Caramel,
                US_Lollipop,
                US_Caramel };

            foreach (Item item in ItemsList)
            {
                if (item != null)
                {
                    LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(item.spawnPrefab);
                    Utilities.FixMixerGroups(item.spawnPrefab);
                }
            }

            HandlampScript handlampScript = US_Handlamp.spawnPrefab.AddComponent<HandlampScript>();
            ScriptSetUp(handlampScript, true, true, false, US_Handlamp);

            ToolboxScript toolboxScript = US_Toolbox.spawnPrefab.AddComponent<ToolboxScript>();
            ScriptSetUp(toolboxScript, true, true, false, US_Toolbox);
            toolboxScript.useCooldown = .1f;

            PadlockScript padlockScript = US_Padlock.spawnPrefab.AddComponent<PadlockScript>();
            ScriptSetUp(padlockScript, true, true, true, US_Padlock);

            CrowbarScript crowbarScript = US_Crowbar.spawnPrefab.AddComponent<CrowbarScript>();
            ScriptSetUp(crowbarScript, true, true, true, US_Crowbar);
            Items.RegisterItem(US_Crowbar);

            UnstableFuelCylinderScript unstableFuelCylinderScript = US_UnstableFuelCylinder.spawnPrefab.AddComponent<UnstableFuelCylinderScript>();
            ScriptSetUp(unstableFuelCylinderScript, true, true, true, US_UnstableFuelCylinder);

            RadioactiveCellScript radioactiveMineralCellScript = US_RadioactiveCell.spawnPrefab.AddComponent<RadioactiveCellScript>();
            ScriptSetUp(radioactiveMineralCellScript, true, true, true, US_RadioactiveCell);

            GloomyCapsuleScript gloomyCapsuleScript = US_GloomyCapsule.spawnPrefab.AddComponent<GloomyCapsuleScript>();
            ScriptSetUp(gloomyCapsuleScript, true, true, true, US_GloomyCapsule);

            FrigidCapsuleScript frigidCapsuleScript = US_FrigidCapsule.spawnPrefab.AddComponent<FrigidCapsuleScript>();
            ScriptSetUp(frigidCapsuleScript, true, true, true, US_FrigidCapsule);

            SanguineCapsuleScript sanguineCapsuleScript = US_SanguineCapsule.spawnPrefab.AddComponent<SanguineCapsuleScript>();
            ScriptSetUp(sanguineCapsuleScript, true, true, true, US_SanguineCapsule);

            BandagesScript bandageScript = US_Bandages.spawnPrefab.AddComponent<BandagesScript>();
            ScriptSetUp(bandageScript, true, true, false, US_Bandages);
            bandageScript.useCooldown = .5f;

            MedicalKitScript medkitScript = US_MedicalKit.spawnPrefab.AddComponent<MedicalKitScript>();
            ScriptSetUp(medkitScript, true, true, false, US_MedicalKit);

            DefibrillatorScript defibrillatorScript = US_Defibrillator.spawnPrefab.AddComponent<DefibrillatorScript>();
            ScriptSetUp(defibrillatorScript, true, true, false, US_Defibrillator);

            EmergencyInjectorScript emergencyInjectorScript = US_EmergencyInjector.spawnPrefab.AddComponent<EmergencyInjectorScript>();
            ScriptSetUp(emergencyInjectorScript, true, true, false, US_EmergencyInjector);

            ShiftControllerScript shiftControllerScript = US_ShiftController.spawnPrefab.AddComponent<ShiftControllerScript>();
            ScriptSetUp(shiftControllerScript, true, true, false, US_ShiftController);

            CaneScript caneScript = US_WalkingCane.spawnPrefab.AddComponent<CaneScript>();
            ScriptSetUp(caneScript, true, true, true, US_WalkingCane);

            ScissorsScript scissorsScript = US_Scissors.spawnPrefab.AddComponent<ScissorsScript>();
            ScriptSetUp(scissorsScript, true, true, true, US_Scissors);

            RoseScript roseScript = US_Rose.spawnPrefab.AddComponent<RoseScript>();
            ScriptSetUp(roseScript, true, true, true, US_Rose);

            GoldenTicketScript goldenTicketScript = US_GoldenTicket.spawnPrefab.AddComponent<GoldenTicketScript>();
            ScriptSetUp(goldenTicketScript, true, true, true, US_GoldenTicket);

            TicketScript ticketScript = US_Ticket.spawnPrefab.AddComponent<TicketScript>();
            ScriptSetUp(ticketScript, true, true, true, US_Ticket);

            CandyDispenserScript candyDispenserScript = US_CandyDispenser.spawnPrefab.AddComponent<CandyDispenserScript>();
            ScriptSetUp(candyDispenserScript, true, true, true, US_CandyDispenser);

            CandyScript lollipopScript = US_Lollipop.spawnPrefab.AddComponent<CandyScript>();
            ScriptSetUp(lollipopScript, true, true, true, US_Lollipop);

            CandyScript caramelScript = US_Caramel.spawnPrefab.AddComponent<CandyScript>();
            ScriptSetUp(caramelScript, true, true, true, US_Caramel);

            CandyScript peppermintScript = US_Peppermint.spawnPrefab.AddComponent<CandyScript>();
            ScriptSetUp(peppermintScript, true, true, true, US_Peppermint);

            var HardMoons = (Levels.LevelTypes.TitanLevel | Levels.LevelTypes.DineLevel | Levels.LevelTypes.RendLevel | Levels.LevelTypes.ArtificeLevel | Levels.LevelTypes.EmbrionLevel | Levels.LevelTypes.Modded);

            BoundConfig = new UsualScrapConfigs(base.Config);

            if (BoundConfig.UnstableFuelCylinderLoaded.Value)
            {
                if (BoundConfig.UnstableFuelCylinderSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_UnstableFuelCylinder, BoundConfig.UnstableFuelCylinderSpawnWeight.Value, Levels.LevelTypes.All);
                }
                else
                {
                    Items.RegisterItem(US_UnstableFuelCylinder);
                }
            }

            if (BoundConfig.RadioactiveCellLoaded.Value)
            {
                if (BoundConfig.RadioactiveCellSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_RadioactiveCell, BoundConfig.RadioactiveCellSpawnWeight.Value, Levels.LevelTypes.All);
                }
                else
                {
                    Items.RegisterItem(US_RadioactiveCell);
                }
            }

            if (BoundConfig.CrowbarLoaded.Value)
            {
                if (BoundConfig.CrowbarSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_Crowbar, BoundConfig.CrowbarSpawnWeight.Value, Levels.LevelTypes.All);
                }
                else
                {
                    Items.RegisterItem(US_Crowbar);
                }
            }

            if (BoundConfig.PadlockLoaded.Value)
            {
                if (BoundConfig.PadlockSpawnsAsScrap.Value == true)
                {
                    Items.RegisterScrap(US_Padlock, BoundConfig.PadlockSpawnWeight.Value, Levels.LevelTypes.All);
                }
                if (BoundConfig.PadlockIsStoreItem.Value == true)
                {
                    Registerstoreitem(US_Padlock, "This Padlock can be used to lock doors on your enemies..or coworkers.", BoundConfig.PadlockStorePrice.Value);
                }
                if (!BoundConfig.PadlockSpawnsAsScrap.Value && !BoundConfig.PadlockIsStoreItem.Value)
                {
                    Items.RegisterItem(US_Padlock);
                }
            }

            if (BoundConfig.WalkingCaneLoaded.Value)
            {
                if (BoundConfig.WalkingCaneSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_WalkingCane, BoundConfig.WalkingCaneSpawnWeight.Value, HardMoons);
                }
                else
                {
                    Items.RegisterItem(US_WalkingCane);
                }
            }

            if (BoundConfig.ScissorsLoaded.Value)
            {
                if (BoundConfig.ScissorsSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_Scissors, BoundConfig.ScissorsSpawnWeight.Value, HardMoons);
                }
                else
                {
                    Items.RegisterItem(US_Scissors);
                }
            }

            if (BoundConfig.RoseLoaded.Value)
            {
                if (BoundConfig.RoseSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_Rose, BoundConfig.RoseSpawnWeight.Value, HardMoons);
                }
                else
                {
                    Items.RegisterItem(US_Rose);
                }
            }

            if (BoundConfig.TicketLoaded.Value)
            {
                if (BoundConfig.TicketSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_Ticket, BoundConfig.TicketSpawnWeight.Value, HardMoons);
                }
                else
                {
                    Items.RegisterItem(US_Ticket);
                }
            }

            if (BoundConfig.GoldenTicketLoaded.Value)
            {
                if (BoundConfig.GoldenTicketSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_GoldenTicket, BoundConfig.GoldenTicketSpawnWeight.Value, HardMoons);
                }
                else
                {
                    Items.RegisterItem(US_GoldenTicket);
                }
            }

            if (BoundConfig.CandyDispenserLoaded.Value)
            {
                if (BoundConfig.CandyDispenserSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_CandyDispenser, BoundConfig.CandyDispenserSpawnWeight.Value, HardMoons);
                    Items.RegisterItem(US_Caramel);
                    Items.RegisterItem(US_Lollipop);
                    Items.RegisterItem(US_Peppermint);
                }
                else
                {
                    Items.RegisterItem(US_CandyDispenser);
                    Items.RegisterItem(US_Caramel);
                    Items.RegisterItem(US_Lollipop);
                    Items.RegisterItem(US_Peppermint);
                }
            }

            if (BoundConfig.GloomyCapsuleLoaded.Value)
            {
                if (BoundConfig.GloomyCapsuleSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_GloomyCapsule, BoundConfig.GloomyCapsuleSpawnWeight.Value, Levels.LevelTypes.All);
                }
                else
                {
                    Items.RegisterItem(US_GloomyCapsule);
                }
            }

            if (BoundConfig.FrigidCapsuleLoaded.Value)
            {
                if (BoundConfig.FrigidCapsuleSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_FrigidCapsule, BoundConfig.FrigidCapsuleSpawnWeight.Value, Levels.LevelTypes.All);
                }
                else
                {
                    Items.RegisterItem(US_FrigidCapsule);
                }
            }

            if (BoundConfig.PocketWatchLoaded.Value)
            {
                if (BoundConfig.PocketWatchSpawnsAsScrap.Value)
                {
                    Items.RegisterScrap(US_PocketWatch, BoundConfig.PocketWatchSpawnWeight.Value, HardMoons);
                }
                else
                {
                    Items.RegisterItem(US_PocketWatch);
                }
            }

            if (BoundConfig.HandlampLoaded.Value)
            {
                if (BoundConfig.HandlampIsStoreItem.Value)
                {
                    Registerstoreitem(US_Handlamp, "The Handlamp is perfect for illuminating your surroundings..and possibly blinding you.", BoundConfig.HandlampStorePrice.Value);
                }
                else
                {
                    Items.RegisterItem(US_Handlamp);
                }
            }

            if (BoundConfig.BandagesLoaded.Value)
            {
                if (BoundConfig.BandagesIsStoreItem.Value)
                {
                    Registerstoreitem(US_Bandages, "The Bandages will get you up and going in no time at all!", BoundConfig.BandagesStorePrice.Value);
                }
                else
                {
                    Items.RegisterItem(US_Bandages);
                }
            }

            if (BoundConfig.MedicalKitLoaded.Value)
            {
                if (BoundConfig.MedicalKitIsStoreItem.Value)
                {
                    Registerstoreitem(US_MedicalKit, "The Medical Kit can heal anything if given time!", BoundConfig.MedicalKitStorePrice.Value);
                }
                else
                {
                    Items.RegisterItem(US_MedicalKit);
                }
            }

            if (BoundConfig.ShiftControllerLoaded.Value)
            {
                if (BoundConfig.ShiftControllerIsStoreItem.Value)
                {
                    Registerstoreitem(US_ShiftController, "The Shift Controller is a portable teleporter that allows you to set the teleport position.", BoundConfig.ShiftControllerStorePrice.Value);
                }
                else
                {
                    Items.RegisterItem(US_ShiftController);
                }
            }

            if (BoundConfig.EmergencyInjectorLoaded.Value)
            {
                if (BoundConfig.EmergencyInjectorIsStoreItem.Value)
                {
                    Registerstoreitem(US_EmergencyInjector, "The Emergency Injector boosts your movement capabilities for a while and gives a small amount of health, don't take too much!", BoundConfig.EmergencyInjectorStorePrice.Value);
                }
                else
                {
                    Items.RegisterItem(US_EmergencyInjector);
                }
            }

            if (BoundConfig.DefibrillatorLoaded.Value)
            {
                if (BoundConfig.DefibrillatorIsStoreItem.Value)
                {
                    Registerstoreitem(US_Defibrillator, "The Defibrillator can revive those employees lost to stupidity.", BoundConfig.DefibrillatorStorePrice.Value);
                }
                else
                {
                    Items.RegisterItem(US_Defibrillator);
                }
            }

            if (BoundConfig.ToolboxLoaded.Value)
            {
                if (BoundConfig.ToolboxIsStoreItem.Value)
                {
                    Registerstoreitem(US_Toolbox, "The Toolbox can deconstruct some traps for a fine reward.", BoundConfig.ToolboxStorePrice.Value);
                }
                else
                {
                    Items.RegisterItem(US_Toolbox);
                }
            }

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        }
    }
}
