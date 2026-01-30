using BepInEx;
using Dawn;
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
        public const string PLUGIN_VERSION = "1.9.1";

        public static Plugin instance;

        internal static UsualScrapConfigs BoundConfig { get; private set; } = null!;

        public static class ScrapPackKeys
        {
            public static NamespacedKey<DawnItemInfo> US_PocketWatch = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Pocket_Watch");

            public static NamespacedKey<DawnItemInfo> US_ServantBell = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Servant_Bell");

            public static NamespacedKey<DawnItemInfo> US_TrafficCone = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Traffic_Cone");

            public static NamespacedKey<DawnItemInfo> US_LargeGear = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Large_Gear");
        }

        public static class ScrapItemKeys
        {
            public static NamespacedKey<DawnItemInfo> US_Ticket = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Ticket");

            public static NamespacedKey<DawnItemInfo> US_GoldenTicket = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Golden_Ticket");

            public static NamespacedKey<DawnItemInfo> US_Rose = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Rose");

            public static NamespacedKey<DawnItemInfo> US_Scissors = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Scissors");

            public static NamespacedKey<DawnItemInfo> US_WalkingCane = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Walking_Cane");

            public static NamespacedKey<DawnItemInfo> US_CandyDispenser = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Candy_Dispenser");

            public static NamespacedKey<DawnItemInfo> US_PieceofCandy = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Piece_of_Candy");

            public static NamespacedKey<DawnItemInfo> US_FuelCylinder = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Fuel_Cylinder");

            public static NamespacedKey<DawnItemInfo> US_RadioactiveCell = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Radioactive_Cell");

            public static NamespacedKey<DawnItemInfo> US_GloomyCapsule = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Gloomy_Capsule");

            public static NamespacedKey<DawnItemInfo> US_FrigidCapsule = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Frigid_Capsule");

            public static NamespacedKey<DawnItemInfo> US_NoxiousCapsule = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Noxious_Capsule");

            public static NamespacedKey<DawnItemInfo> US_BloodyCapsule = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Bloody_Capsule");

            public static NamespacedKey<DawnItemInfo> US_Padlock = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Padlock");

            public static NamespacedKey<DawnItemInfo> US_SalvagedPadlock = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Salvaged_Padlock");

            public static NamespacedKey<DawnItemInfo> US_Crowbar = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Crowbar");

            public static NamespacedKey<DawnItemInfo> US_SalvagedCrowbar = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Salvaged_Crowbar");

            //public static NamespacedKey<DawnItemInfo> US_DoomsayerBell = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Doomsayer_Bell");
        }

        public static class StoreItemKeys
        {
            public static NamespacedKey<DawnItemInfo> US_HandLamp = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Hand_Lamp");

            public static NamespacedKey<DawnItemInfo> US_Bandages = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Bandages");

            public static NamespacedKey<DawnItemInfo> US_MedicalKit = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Medical_Kit");

            public static NamespacedKey<DawnItemInfo> US_Defibrillator = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Defibrillator");

            public static NamespacedKey<DawnItemInfo> US_ProductivityAutoinjector = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Productivity_Autoinjector");

            public static NamespacedKey<DawnItemInfo> US_ShiftController = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Shift_Controller");

            public static NamespacedKey<DawnItemInfo> US_Toolbox = NamespacedKey<DawnItemInfo>.From("Usual_Scrap", "US_Toolbox");
        }

        public void InitializeItem(GrabbableObject itemScript, bool grabbable, bool grabbableToEnemies, Item itemProperties)
        {
            DawnLib.RegisterNetworkPrefab(itemProperties.spawnPrefab);
            Utilities.FixMixerGroups(itemProperties.spawnPrefab);
            itemScript.grabbable = grabbable;
            itemScript.grabbableToEnemies = grabbableToEnemies;
            itemScript.itemProperties = itemProperties;
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

            List<Item> itemList = new List<Item>();

            Plugin.instance = this;

            BoundConfig = new UsualScrapConfigs(base.Config);

            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "usualscrapassetbundle"));

            //========== SCRAP PACKS ==========

            if (BoundConfig.MansionScrapPackEnabled.Value)
            {
                try
                {
                    Item US_PocketWatch = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/ScrapPackItems/PocketWatchAssets/US_PocketWatchItem.asset");
                    itemList.Add(US_PocketWatch);
                    PhysicsProp pocketWatchScript = US_PocketWatch.spawnPrefab.AddComponent<PhysicsProp>();
                    InitializeItem(pocketWatchScript, true, true, US_PocketWatch);
                    DawnLib.DefineItem(ScrapPackKeys.US_PocketWatch, US_PocketWatch, builder => builder.DefineScrap(scrapBuilder => scrapBuilder.SetWeights(weightBuilder => weightBuilder.SetGlobalWeight(20))));
                }
                catch {Debug.LogError("USUAL SCRAP - Pocket Watch scrap pack item experienced an error while loading. Skipping...");}
                try
                {
                    Item US_ServantBell = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/ScrapPackItems/ServantBellAssets/US_ServantBellItem.asset");
                    itemList.Add(US_ServantBell);
                    NoiseMakerItemScript servantBellScript = US_ServantBell.spawnPrefab.AddComponent<NoiseMakerItemScript>();
                    InitializeItem(servantBellScript, true, true, US_ServantBell);
                    DawnLib.DefineItem(ScrapPackKeys.US_ServantBell, US_ServantBell, builder => builder.DefineScrap(scrapBuilder => scrapBuilder.SetWeights(weightBuilder => weightBuilder.SetGlobalWeight(20))));
                }
                catch {Debug.LogError("USUAL SCRAP - Servant Bell scrap pack item experienced an error while loading. Skipping..."); }
                
            }

            if (BoundConfig.FacilityScrapPackEnabled.Value)
            {
                try
                {
                    Item US_TrafficCone = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/ScrapPackItems/TrafficConeAssets/US_TrafficConeItem.asset");
                    itemList.Add(US_TrafficCone);
                    PhysicsProp trafficConeScript = US_TrafficCone.spawnPrefab.AddComponent<PhysicsProp>();
                    InitializeItem(trafficConeScript, true, true, US_TrafficCone);
                    DawnLib.DefineItem(ScrapPackKeys.US_TrafficCone, US_TrafficCone, builder => builder.DefineScrap(scrapBuilder => scrapBuilder.SetWeights(weightBuilder => weightBuilder.SetGlobalWeight(20))));
                }
                catch { Debug.LogError("USUAL SCRAP - Traffic Cone scrap pack item experienced an error while loading. Skipping..."); }
                try
                {
                    Item US_LargeGear = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/ScrapPackItems/LargeGearAssets/US_LargeGearItem.asset");
                    itemList.Add(US_LargeGear);
                    PhysicsProp largeGearScript = US_LargeGear.spawnPrefab.AddComponent<PhysicsProp>();
                    InitializeItem(largeGearScript, true, true, US_LargeGear);
                    DawnLib.DefineItem(ScrapPackKeys.US_LargeGear, US_LargeGear, builder => builder.DefineScrap(scrapBuilder => scrapBuilder.SetWeights(weightBuilder => weightBuilder.SetGlobalWeight(30))));
                }
                catch { Debug.LogError("USUAL SCRAP - Large Gear scrap pack item experienced an error while loading. Skipping..."); }
            }

            //========== SCRAP ITEMS ==========

            try
            {
                if (BoundConfig.TicketLoaded.Value)
                {
                    Item US_Ticket = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/TicketAssets/US_TicketItem.asset");
                    itemList.Add(US_Ticket);
                    TicketScript ticketScript = US_Ticket.spawnPrefab.AddComponent<TicketScript>();
                    InitializeItem(ticketScript, true, true, US_Ticket);
                    US_Ticket.itemIcon = null;
                    if (BoundConfig.TicketSpawnsAsScrap.Value)
                    {
                        string[] ticketMoonStrings = BoundConfig.TicketMoonSpawnWeights.Value.Split(',');

                                DawnLib.DefineItem(ScrapItemKeys.US_Ticket, US_Ticket, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, ticketMoonStrings, BoundConfig.TicketGlobalSpawnWeight.Value, US_Ticket); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_Ticket);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Ticket experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.GoldenTicketLoaded.Value)
                {
                    Item US_GoldenTicket = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/GoldenTicketAssets/US_GoldenTicketItem.asset");
                    itemList.Add(US_GoldenTicket);
                    GoldenTicketScript goldenTicketScript = US_GoldenTicket.spawnPrefab.AddComponent<GoldenTicketScript>();
                    InitializeItem(goldenTicketScript, true, true, US_GoldenTicket);
                    if (BoundConfig.GoldenTicketSpawnsAsScrap.Value)
                    {
                        string[] goldenTicketMoonStrings = BoundConfig.GoldenTicketMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_GoldenTicket, US_GoldenTicket, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, goldenTicketMoonStrings, BoundConfig.GoldenTicketGlobalSpawnWeight.Value, US_GoldenTicket); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_GoldenTicket);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Golden Ticket experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.RoseLoaded.Value)
                {
                    Item US_Rose = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/RoseAssets/US_RoseItem.asset");
                    itemList.Add(US_Rose);
                    RoseScript roseScript = US_Rose.spawnPrefab.AddComponent<RoseScript>();
                    InitializeItem(roseScript, true, true, US_Rose);
                    if (BoundConfig.RoseSpawnsAsScrap.Value)
                    {
                        string[] roseMoonStrings = BoundConfig.RoseMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_Rose, US_Rose, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, roseMoonStrings, BoundConfig.RoseGlobalSpawnWeight.Value, US_Rose); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_Rose);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Rose experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.ScissorsLoaded.Value)
                {
                    Item US_Scissors = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/ScissorsAssets/US_ScissorsItem.asset");
                    itemList.Add(US_Scissors);
                    ScissorsScript scissorsScript = US_Scissors.spawnPrefab.AddComponent<ScissorsScript>();
                    InitializeItem(scissorsScript, true, true, US_Scissors);
                    if (BoundConfig.ScissorsSpawnsAsScrap.Value)
                    {
                        string[] scissorsMoonStrings = BoundConfig.ScissorsMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_Scissors, US_Scissors, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, scissorsMoonStrings, BoundConfig.ScissorsGlobalSpawnWeight.Value, US_Scissors); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_Scissors);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Scissors experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.WalkingCaneLoaded.Value)
                {
                    Item US_WalkingCane = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/WalkingCaneAssets/US_WalkingCaneItem.asset");
                    itemList.Add(US_WalkingCane);
                    WalkingCaneScript walkingCaneScript = US_WalkingCane.spawnPrefab.AddComponent<WalkingCaneScript>();
                    InitializeItem(walkingCaneScript, true, true, US_WalkingCane);
                    if (BoundConfig.WalkingCaneSpawnsAsScrap.Value)
                    {
                        string[] walkingCaneMoonStrings = BoundConfig.WalkingCaneMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_WalkingCane, US_WalkingCane, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, walkingCaneMoonStrings, BoundConfig.WalkingCaneGlobalSpawnWeight.Value, US_WalkingCane); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_WalkingCane);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Walking Cane experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.CandyDispenserLoaded.Value)
                {
                    Item US_CandyDispenser = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/CandyDispenserAssets/US_CandyDispenserItem.asset");
                    itemList.Add(US_CandyDispenser);
                    CandyDispenserScript candyDispenserScript = US_CandyDispenser.spawnPrefab.AddComponent<CandyDispenserScript>();
                    InitializeItem(candyDispenserScript, true, true, US_CandyDispenser);
                    Item US_PieceofCandy = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/PieceofCandyAssets/US_PieceofCandyItem.asset");
                    itemList.Add(US_PieceofCandy);
                    PieceofCandyScript pieceofCandyScript = US_PieceofCandy.spawnPrefab.AddComponent<PieceofCandyScript>();
                    InitializeItem(pieceofCandyScript, true, true, US_PieceofCandy);
                    if (BoundConfig.CandyDispenserSpawnsAsScrap.Value)
                    {
                        string[] candyDispenserMoonStrings = BoundConfig.CandyDispenserMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_CandyDispenser, US_CandyDispenser, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, candyDispenserMoonStrings, BoundConfig.CandyDispenserGlobalSpawnWeight.Value, US_CandyDispenser); }); }));
                        DawnLib.DefineItem(ScrapItemKeys.US_PieceofCandy, US_PieceofCandy, builder => builder.DefineScrap(scrapBuilder => scrapBuilder.SetWeights(weightBuilder => weightBuilder.SetGlobalWeight(0))));
                    }
                    else
                    {
                        Items.RegisterItem(US_CandyDispenser);
                        Items.RegisterItem(US_PieceofCandy);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Candy Dispenser experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.FuelCylinderLoaded.Value)
                {
                    Item US_FuelCylinder = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/FuelCylinderAssets/US_FuelCylinderItem.asset");
                    itemList.Add(US_FuelCylinder);
                    FuelCylinderScript fuelCylinderScript = US_FuelCylinder.spawnPrefab.AddComponent<FuelCylinderScript>();
                    InitializeItem(fuelCylinderScript, true, true, US_FuelCylinder);
                    if (BoundConfig.FuelCylinderSpawnsAsScrap.Value)
                    {
                        string[] fuelCylinderMoonStrings = BoundConfig.FuelCylinderMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_FuelCylinder, US_FuelCylinder, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, fuelCylinderMoonStrings, BoundConfig.FuelCylinderGlobalSpawnWeight.Value, US_FuelCylinder); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_FuelCylinder);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Fuel Cylinder experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.RadioactiveCellLoaded.Value)
                {
                    Item US_RadioactiveCell = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/RadioactiveCellAssets/US_RadioactiveCellItem.asset");
                    itemList.Add(US_RadioactiveCell);
                    RadioactiveCellScript radioactiveCellScript = US_RadioactiveCell.spawnPrefab.AddComponent<RadioactiveCellScript>();
                    InitializeItem(radioactiveCellScript, true, true, US_RadioactiveCell);
                    if (BoundConfig.RadioactiveCellSpawnsAsScrap.Value)
                    {
                        string[] radioactiveCellMoonStrings = BoundConfig.RadioactiveCellMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_RadioactiveCell, US_RadioactiveCell, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, radioactiveCellMoonStrings, BoundConfig.RadioactiveCellGlobalSpawnWeight.Value, US_RadioactiveCell); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_RadioactiveCell);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Radioactive Cell experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.GloomyCapsuleLoaded.Value)
                {
                    Item US_GloomyCapsule = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/GloomyCapsuleAssets/US_GloomyCapsuleItem.asset");
                    itemList.Add(US_GloomyCapsule);
                    GloomyCapsuleScript gloomyCapsuleScript = US_GloomyCapsule.spawnPrefab.AddComponent<GloomyCapsuleScript>();
                    InitializeItem(gloomyCapsuleScript, true, true, US_GloomyCapsule);
                    if (BoundConfig.GloomyCapsuleSpawnsAsScrap.Value)
                    {
                        string[] gloomyCapsuleMoonStrings = BoundConfig.GloomyCapsuleMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_GloomyCapsule, US_GloomyCapsule, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, gloomyCapsuleMoonStrings, BoundConfig.GloomyCapsuleGlobalSpawnWeight.Value, US_GloomyCapsule); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_GloomyCapsule);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Gloomy Capsule experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.FrigidCapsuleLoaded.Value)
                {
                    Item US_FrigidCapsule = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/FrigidCapsuleAssets/US_FrigidCapsuleItem.asset");
                    itemList.Add(US_FrigidCapsule);
                    FrigidCapsuleScript frigidCapsuleScript = US_FrigidCapsule.spawnPrefab.AddComponent<FrigidCapsuleScript>();
                    InitializeItem(frigidCapsuleScript, true, true, US_FrigidCapsule);
                    if (BoundConfig.FrigidCapsuleSpawnsAsScrap.Value)
                    {
                        string[] frigidCapsuleMoonStrings = BoundConfig.FrigidCapsuleMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_FrigidCapsule, US_FrigidCapsule, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, frigidCapsuleMoonStrings, BoundConfig.FrigidCapsuleGlobalSpawnWeight.Value, US_FrigidCapsule); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_FrigidCapsule);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Frigid Capsule experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.NoxiousCapsuleLoaded.Value)
                {
                    Item US_NoxiousCapsule = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/NoxiousCapsuleAssets/US_NoxiousCapsuleItem.asset");
                    itemList.Add(US_NoxiousCapsule);
                    NoxiousCapsuleScript noxiousCapsuleScript = US_NoxiousCapsule.spawnPrefab.AddComponent<NoxiousCapsuleScript>();
                    InitializeItem(noxiousCapsuleScript, true, true, US_NoxiousCapsule);
                    if (BoundConfig.NoxiousCapsuleSpawnsAsScrap.Value)
                    {
                        string[] noxiousCapsuleMoonStrings = BoundConfig.NoxiousCapsuleMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_NoxiousCapsule, US_NoxiousCapsule, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, noxiousCapsuleMoonStrings, BoundConfig.NoxiousCapsuleGlobalSpawnWeight.Value, US_NoxiousCapsule); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_NoxiousCapsule);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Noxious Capsule experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.BloodyCapsuleLoaded.Value)
                {
                    Item US_BloodyCapsule = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/BloodyCapsuleAssets/US_BloodyCapsuleItem.asset");
                    itemList.Add(US_BloodyCapsule);
                    BloodyCapsuleScript bloodyCapsuleScript = US_BloodyCapsule.spawnPrefab.AddComponent<BloodyCapsuleScript>();
                    InitializeItem(bloodyCapsuleScript, true, true, US_BloodyCapsule);
                    if (BoundConfig.BloodyCapsuleSpawnsAsScrap.Value)
                    {
                        string[] bloodyCapsuleMoonStrings = BoundConfig.BloodyCapsuleMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_BloodyCapsule, US_BloodyCapsule, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, bloodyCapsuleMoonStrings, BoundConfig.BloodyCapsuleGlobalSpawnWeight.Value, US_BloodyCapsule); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_BloodyCapsule);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Bloody Capsule experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.PadlockLoaded.Value)
                {
                    Item US_Padlock = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/PadlockAssets/US_PadlockItem.asset");
                    itemList.Add(US_Padlock);
                    PadlockScript padlockScript = US_Padlock.spawnPrefab.AddComponent<PadlockScript>();
                    InitializeItem(padlockScript, true, true, US_Padlock);
                    if (BoundConfig.PadlockSpawnsAsScrap.Value)
                    {
                        string[] padlockMoonStrings = BoundConfig.PadlockMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_Padlock, US_Padlock, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, padlockMoonStrings, BoundConfig.PadlockGlobalSpawnWeight.Value, US_Padlock); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_Padlock);
                    }
                    if (BoundConfig.PadlockIsStoreItem.Value && BoundConfig.PadlockStorePrice.Value >= 0)
                    {
                        Item US_SalvagedPadlock = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/PadlockAssets/US_SalvagedPadlockItem.asset");
                        itemList.Add(US_SalvagedPadlock);
                        TerminalNode US_PadlockStoreNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/PadlockAssets/US_SalvagedPadlockNode.asset");
                        ScanNodeProperties salvagedPadlockNode = US_SalvagedPadlock.spawnPrefab.GetComponentInChildren<ScanNodeProperties>();
                        salvagedPadlockNode.headerText = "Salvaged US_Padlock";
                        salvagedPadlockNode.nodeType = 3;

                        PadlockScript salvagedPadlockScript = US_SalvagedPadlock.spawnPrefab.AddComponent<PadlockScript>();
                        InitializeItem(salvagedPadlockScript, true, true, US_SalvagedPadlock);

                        DawnLib.DefineItem(ScrapItemKeys.US_SalvagedPadlock, US_SalvagedPadlock, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.PadlockStorePrice.Value).OverrideInfoNode(US_PadlockStoreNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Padlock experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.CrowbarLoaded.Value)
                {
                    Item US_Crowbar = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/CrowbarAssets/US_CrowbarItem.asset");
                    itemList.Add(US_Crowbar);
                    CrowbarScript crowbarScript = US_Crowbar.spawnPrefab.AddComponent<CrowbarScript>();
                    InitializeItem(crowbarScript, true, true, US_Crowbar);
                    if (BoundConfig.CrowbarSpawnsAsScrap.Value)
                    {
                        string[] crowbarMoonStrings = BoundConfig.CrowbarMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_Crowbar, US_Crowbar, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, crowbarMoonStrings, BoundConfig.CrowbarGlobalSpawnWeight.Value, US_Crowbar); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_Crowbar);
                    }
                    if (BoundConfig.CrowbarIsStoreItem.Value && BoundConfig.CrowbarStorePrice.Value >= 0)
                    {
                        Item US_SalvagedCrowbar = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/CrowbarAssets/US_SalvagedCrowbarItem.asset");
                        itemList.Add(US_SalvagedCrowbar);
                        TerminalNode US_CrowbarStoreNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/CrowbarAssets/US_SalvagedCrowbarNode.asset");
                        ScanNodeProperties salvagedCrowbarNode = US_SalvagedCrowbar.spawnPrefab.GetComponentInChildren<ScanNodeProperties>();
                        salvagedCrowbarNode.headerText = "Salvaged US_Crowbar";
                        salvagedCrowbarNode.nodeType = 3;

                        CrowbarScript salvagedCrowbarScript = US_SalvagedCrowbar.spawnPrefab.AddComponent<CrowbarScript>();
                        InitializeItem(salvagedCrowbarScript, true, true, US_SalvagedCrowbar);

                        DawnLib.DefineItem(ScrapItemKeys.US_SalvagedCrowbar, US_SalvagedCrowbar, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.CrowbarStorePrice.Value).OverrideInfoNode(US_CrowbarStoreNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Crowbar experienced an error while loading. Skipping...");
            }
            /*
            try
            {
                if (BoundConfig.DoomsayerBellLoaded.Value)
                {
                    Item US_DoomsayerBell = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/DoomsayerBellAssets/US_DoomsayerBellItem.asset");
                    itemList.Add(US_DoomsayerBell);
                    DoomsayerBellScript doomsayerBellScript = US_DoomsayerBell.spawnPrefab.AddComponent<DoomsayerBellScript>();
                    InitializeItem(doomsayerBellScript, true, true, US_DoomsayerBell);
                    if (BoundConfig.DoomsayerBellSpawnsAsScrap.Value)
                    {
                        string[] doomsayerBellMoonStrings = BoundConfig.DoomsayerBellMoonSpawnWeights.Value.Split(',');

                        DawnLib.DefineItem(ScrapItemKeys.US_DoomsayerBell, US_DoomsayerBell, builder => builder.DefineScrap(scrapBuilder => { scrapBuilder.SetWeights(weightBuilder => { buildweights(weightBuilder, doomsayerBellMoonStrings, BoundConfig.DoomsayerBellGlobalSpawnWeight.Value, US_DoomsayerBell); }); }));
                    }
                    else
                    {
                        Items.RegisterItem(US_DoomsayerBell);
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Doomsayer Bell experienced an error while loading. Skipping...");
            }
            */
            //========== STORE ITEMS ==========

            try
            {
                if (BoundConfig.HandlampLoaded.Value)
                {
                    Item US_Handlamp = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/HandlampAssets/US_HandlampItem.asset");
                    itemList.Add(US_Handlamp);
                    TerminalNode US_HandlampNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/HandlampAssets/US_HandlampNode.asset");
                    HandlampScript handlampScript = US_Handlamp.spawnPrefab.AddComponent<HandlampScript>();
                    InitializeItem(handlampScript, true, true, US_Handlamp);
                    if (BoundConfig.HandlampIsStoreItem.Value && BoundConfig.HandlampStorePrice.Value >= 0)
                    {
                        DawnLib.DefineItem(StoreItemKeys.US_HandLamp, US_Handlamp, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.HandlampStorePrice.Value).OverrideInfoNode(US_HandlampNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Handlamp experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.BandagesLoaded.Value)
                {
                    Item US_Bandages = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/BandagesAssets/US_BandagesItem.asset");
                    itemList.Add(US_Bandages);
                    TerminalNode US_BandagesNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/BandagesAssets/US_BandagesNode.asset");
                    BandagesScript bandagesScript = US_Bandages.spawnPrefab.AddComponent<BandagesScript>();
                    InitializeItem(bandagesScript, true, true, US_Bandages);
                    if (BoundConfig.BandagesIsStoreItem.Value && BoundConfig.BandagesStorePrice.Value >= 0)
                    {
                        DawnLib.DefineItem(StoreItemKeys.US_Bandages, US_Bandages, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.BandagesStorePrice.Value).OverrideInfoNode(US_BandagesNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Bandages experienced an error while loading. Skipping...");
            }

            //==========
            
            try
            {
                if (BoundConfig.MedicalKitLoaded.Value)
                {
                    Item US_MedicalKit = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/MedicalKitAssets/US_MedicalKitItem.asset");
                    itemList.Add(US_MedicalKit);
                    TerminalNode US_MedicalKitNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/MedicalKitAssets/US_MedicalKitNode.asset");
                    MedicalKitScript medicalKitScript = US_MedicalKit.spawnPrefab.AddComponent<MedicalKitScript>();
                    InitializeItem(medicalKitScript, true, true, US_MedicalKit);
                    if (BoundConfig.MedicalKitIsStoreItem.Value && BoundConfig.MedicalKitStorePrice.Value >= 0)
                    {
                        DawnLib.DefineItem(StoreItemKeys.US_MedicalKit, US_MedicalKit, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.MedicalKitStorePrice.Value).OverrideInfoNode(US_MedicalKitNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Medical Kit experienced an error while loading. Skipping...");
            }
            
            //==========

            try
            {
                if (BoundConfig.DefibrillatorLoaded.Value)
                {
                    Item US_Defibrillator = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/DefibrillatorAssets/US_DefibrillatorItem.asset");
                    itemList.Add(US_Defibrillator);
                    TerminalNode US_DefibrillatorNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/DefibrillatorAssets/US_DefibrillatorNode.asset");
                    DefibrillatorScript defibrillatorScript = US_Defibrillator.spawnPrefab.AddComponent<DefibrillatorScript>();
                    InitializeItem(defibrillatorScript, true, true, US_Defibrillator);
                    if (BoundConfig.DefibrillatorIsStoreItem.Value && BoundConfig.DefibrillatorStorePrice.Value >= 0)
                    {
                        DawnLib.DefineItem(StoreItemKeys.US_Defibrillator, US_Defibrillator, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.DefibrillatorStorePrice.Value).OverrideInfoNode(US_DefibrillatorNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Defibrillator experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.ProductivityAutoinjectorLoaded.Value)
                {
                    Item US_ProductivityAutoinjector = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/ProductivityAutoinjectorAssets/US_ProductivityAutoinjectorItem.asset");
                    itemList.Add(US_ProductivityAutoinjector);
                    TerminalNode US_ProductivityAutoinjectorNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/ProductivityAutoinjectorAssets/US_ProductivityAutoinjectorNode.asset");
                    ProductivityAutoinjectorScript productivityAutoinjectorScript = US_ProductivityAutoinjector.spawnPrefab.AddComponent<ProductivityAutoinjectorScript>();
                    InitializeItem(productivityAutoinjectorScript, true, true, US_ProductivityAutoinjector);
                    if (BoundConfig.ProductivityAutoinjectorIsStoreItem.Value && BoundConfig.ProductivityAutoinjectorStorePrice.Value >= 0)
                    {
                        DawnLib.DefineItem(StoreItemKeys.US_ProductivityAutoinjector, US_ProductivityAutoinjector, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.ProductivityAutoinjectorStorePrice.Value).OverrideInfoNode(US_ProductivityAutoinjectorNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Productivity Autoinjector experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.ToolboxLoaded.Value)
                {
                    Item US_Toolbox = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/ToolboxAssets/US_ToolboxItem.asset");
                    itemList.Add(US_Toolbox);
                    TerminalNode US_ToolboxNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/ToolboxAssets/US_ToolboxNode.asset");
                    ToolboxScript toolboxScript = US_Toolbox.spawnPrefab.AddComponent<ToolboxScript>();
                    InitializeItem(toolboxScript, true, true, US_Toolbox);
                    if (BoundConfig.ToolboxIsStoreItem.Value && BoundConfig.ToolboxStorePrice.Value >= 0)
                    {
                        DawnLib.DefineItem(StoreItemKeys.US_Toolbox, US_Toolbox, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.ToolboxStorePrice.Value).OverrideInfoNode(US_ToolboxNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Toolbox experienced an error while loading. Skipping...");
            }

            //==========

            try
            {
                if (BoundConfig.ShiftControllerLoaded.Value)
                {
                    Item US_ShiftController = assetBundle.LoadAsset<Item>("Assets/UsualScrapContent/Items/ShiftControllerAssets/US_ShiftControllerItem.asset");
                    itemList.Add(US_ShiftController);
                    TerminalNode US_ShiftControllerNode = assetBundle.LoadAsset<TerminalNode>("Assets/Items/ShiftControllerAssets/US_ShiftControllerNode.asset");
                    ShiftControllerScript ShiftControllerScript = US_ShiftController.spawnPrefab.AddComponent<ShiftControllerScript>();
                    InitializeItem(ShiftControllerScript, true, true, US_ShiftController);
                    if (BoundConfig.ShiftControllerIsStoreItem.Value && BoundConfig.ShiftControllerStorePrice.Value >= 0)
                    {
                        DawnLib.DefineItem(StoreItemKeys.US_ShiftController, US_ShiftController, builder => builder.DefineShop(shopBuilder => shopBuilder.OverrideCost(BoundConfig.ShiftControllerStorePrice.Value).OverrideInfoNode(US_ShiftControllerNode)));
                    }
                }
            }
            catch
            {
                Debug.LogError("USUAL SCRAP - Shift Controller experienced an error while loading. Skipping...");
            }

            //==========

            void buildweights(WeightTableBuilder<DawnMoonInfo, SpawnWeightContext> builder, string[] pulledString, int globalWeight, Item item)
            {
                if (globalWeight >= 0)
                {
                    if (BoundConfig.DebugLogging.Value)
                    {
                        print($"SETTING GLOBAL SPAWN WEIGHT FOR [{item}] TO [{globalWeight}]");
                    }
                    builder.SetGlobalWeight(globalWeight);
                }
                else if (BoundConfig.DebugLogging.Value)
                {
                    print($"GLOBAL SPAWN WEIGHT DISABLED FOR [{item}]");
                }
                foreach (var moonString in pulledString)
                {
                    try
                    {
                        string[] splitMoonString = moonString.Split(':');
                        string stringMoon = splitMoonString[0].Trim().ToLowerInvariant();
                        string stringWeight = splitMoonString[1].Trim();
                        int moonWeight = int.Parse(stringWeight);

                        if (moonWeight <= -1)
                        {
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"{stringMoon} weight skipped for {item}");
                            }
                        }
                        else if (stringMoon == "experimentation")
                        {
                            builder.AddWeight(MoonKeys.Experimentation, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for experimentation");
                            }
                        }
                        else if (stringMoon == "vow")
                        {
                            builder.AddWeight(MoonKeys.Vow, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for vow");
                            }
                        }
                        else if (stringMoon == "march")
                        {
                            builder.AddWeight(MoonKeys.March, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for march");
                            }
                        }
                        else if (stringMoon == "assurance")
                        {
                            builder.AddWeight(MoonKeys.Assurance, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for assurance");
                            }
                        }
                        else if (stringMoon == "offense")
                        {
                            builder.AddWeight(MoonKeys.Offense, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for offense");
                            }
                        }
                        else if (stringMoon == "rend")
                        {
                            builder.AddWeight(MoonKeys.Rend, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for rend");
                            }
                        }
                        else if (stringMoon == "dine")
                        {
                            builder.AddWeight(MoonKeys.Dine, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for dine");
                            }
                        }
                        else if (stringMoon == "titan")
                        {
                            builder.AddWeight(MoonKeys.Titan, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for titan");
                            }
                        }
                        else if (stringMoon == "adamance")
                        {
                            builder.AddWeight(MoonKeys.Adamance, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for adamance");
                            }
                        }
                        else if (stringMoon == "artifice")
                        {
                            builder.AddWeight(MoonKeys.Artifice, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for artifice");
                            }
                        }
                        else if (stringMoon == "embrion")
                        {
                            builder.AddWeight(MoonKeys.Embrion, moonWeight);
                            if (BoundConfig.DebugLogging.Value)
                            {
                                print($"Setting {item}'s moonWeight to {moonWeight} for embrion");
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Config [{stringMoon}] from [{item}] returned a readable format error, please check that the moon you entered is an available option and that it is spelled correctly.");
                        }
                    }
                    catch 
                    {
                        Debug.LogWarning($"Moon weight config [{moonString}] from [{item}] is unreadable and returned an error. Check your formatting or report this to the modmaker if you need support, thanks.");
                    }
                }
            }
            bool DisableItemIcons = (BoundConfig.DisableItemIcons.Value);
            if (DisableItemIcons == true)
            {
                foreach (Item item in itemList)
                {
                    item.itemIcon = null;
                }
            }

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PLUGIN_GUID);
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");
        }
    }
}
