using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;

namespace UsualScrap
{
    class UsualScrapConfigs
    {
        public readonly ConfigEntry<bool> UnstableFuelCylinderLoaded;
        public readonly ConfigEntry<bool> UnstableFuelCylinderSpawnsAsScrap;
        public readonly ConfigEntry<int> UnstableFuelCylinderSpawnWeight;

        public readonly ConfigEntry<bool> RadioactiveCellLoaded;
        public readonly ConfigEntry<bool> RadioactiveCellSpawnsAsScrap;
        public readonly ConfigEntry<int> RadioactiveCellSpawnWeight;

        public readonly ConfigEntry<bool> CrowbarLoaded;
        public readonly ConfigEntry<bool> CrowbarSpawnsAsScrap;
        public readonly ConfigEntry<int> CrowbarSpawnWeight;

        public readonly ConfigEntry<bool> PadlockLoaded;
        public readonly ConfigEntry<bool> PadlockSpawnsAsScrap;
        public readonly ConfigEntry<int> PadlockSpawnWeight;
        public readonly ConfigEntry<bool> PadlockIsStoreItem;
        public readonly ConfigEntry<int> PadlockStorePrice;

        public readonly ConfigEntry<bool> WalkingCaneLoaded;
        public readonly ConfigEntry<bool> WalkingCaneSpawnsAsScrap;
        public readonly ConfigEntry<int> WalkingCaneSpawnWeight;

        public readonly ConfigEntry<bool> ScissorsLoaded;
        public readonly ConfigEntry<bool> ScissorsSpawnsAsScrap;
        public readonly ConfigEntry<int> ScissorsSpawnWeight;

        public readonly ConfigEntry<bool> RoseLoaded;
        public readonly ConfigEntry<bool> RoseSpawnsAsScrap;
        public readonly ConfigEntry<int> RoseSpawnWeight;

        public readonly ConfigEntry<bool> TicketLoaded;
        public readonly ConfigEntry<bool> TicketSpawnsAsScrap;
        public readonly ConfigEntry<int> TicketSpawnWeight;

        public readonly ConfigEntry<bool> GoldenTicketLoaded;
        public readonly ConfigEntry<bool> GoldenTicketSpawnsAsScrap;
        public readonly ConfigEntry<int> GoldenTicketSpawnWeight;

        public readonly ConfigEntry<bool> CandyDispenserLoaded;
        public readonly ConfigEntry<bool> CandyDispenserSpawnsAsScrap;
        public readonly ConfigEntry<int> CandyDispenserSpawnWeight;

        public readonly ConfigEntry<bool> GloomyCapsuleLoaded;
        public readonly ConfigEntry<bool> GloomyCapsuleSpawnsAsScrap;
        public readonly ConfigEntry<int> GloomyCapsuleSpawnWeight;

        public readonly ConfigEntry<bool> FrigidCapsuleLoaded;
        public readonly ConfigEntry<bool> FrigidCapsuleSpawnsAsScrap;
        public readonly ConfigEntry<int> FrigidCapsuleSpawnWeight;

        public readonly ConfigEntry<bool> PocketWatchLoaded;
        public readonly ConfigEntry<bool> PocketWatchSpawnsAsScrap;
        public readonly ConfigEntry<int> PocketWatchSpawnWeight;

        public readonly ConfigEntry<bool> HandlampLoaded;
        public readonly ConfigEntry<bool> HandlampIsStoreItem;
        public readonly ConfigEntry<int> HandlampStorePrice;

        public readonly ConfigEntry<bool> BandagesLoaded;
        public readonly ConfigEntry<bool> BandagesIsStoreItem;
        public readonly ConfigEntry<int> BandagesStorePrice;

        public readonly ConfigEntry<bool> MedicalKitLoaded;
        public readonly ConfigEntry<bool> MedicalKitIsStoreItem;
        public readonly ConfigEntry<int> MedicalKitStorePrice;

        public readonly ConfigEntry<bool> DefibrillatorLoaded;
        public readonly ConfigEntry<bool> DefibrillatorIsStoreItem;
        public readonly ConfigEntry<int> DefibrillatorStorePrice;
        public readonly ConfigEntry<bool> DefibrillatorUsesLimited;
        public readonly ConfigEntry<int> DefibrillatorUseLimit;

        public readonly ConfigEntry<bool> ShiftControllerLoaded;
        public readonly ConfigEntry<bool> ShiftControllerIsStoreItem;
        public readonly ConfigEntry<int> ShiftControllerStorePrice;

        public readonly ConfigEntry<bool> EmergencyInjectorLoaded;
        public readonly ConfigEntry<bool> EmergencyInjectorIsStoreItem;
        public readonly ConfigEntry<int> EmergencyInjectorStorePrice;

        public readonly ConfigEntry<bool> ToolboxLoaded;
        public readonly ConfigEntry<bool> ToolboxIsStoreItem;
        public readonly ConfigEntry<int> ToolboxStorePrice;

        public readonly ConfigEntry<int> ShiftControllerGreatRange;
        public readonly ConfigEntry<int> ShiftControllerPoorRange;

        public readonly ConfigEntry<bool> CapsulesDisabledOnTheShip;

        public UsualScrapConfigs(ConfigFile cfg)
        {

            cfg.SaveOnConfigSet = false;

            //==================== SCRAP ITEMS ====================

            UnstableFuelCylinderLoaded = cfg.Bind(
                "Scrap Item - Unstable Fuel Cylinder",
                "Unstable Fuel Cylinder is Loaded",
                true,
                "Whether or not the Unstable Fuel Cylinder is loaded."
            );

            UnstableFuelCylinderSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Unstable Fuel Cylinder",
                "Unstable Fuel Cylinder is a scrap item",
                true,
                "Whether or not the Unstable Fuel Cylinder spawns as scrap."
            );

            UnstableFuelCylinderSpawnWeight = cfg.Bind(
                "Scrap Item - Unstable Fuel Cylinder",
                "Unstable Fuel Cylinder spawn weight",
                25,
                "How often the Unstable Fuel Cylinder spawns as scrap."
            );

            //========================================

            RadioactiveCellLoaded = cfg.Bind(
                "Scrap Item - Radioactive Cell",
                "Radioactive Cell is Loaded",
                true,
                "Whether or not the Radioactive Cell is loaded."
            );

            RadioactiveCellSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Radioactive Cell",
                "Radioactive Cell is a scrap item",
                true,
                "Whether or not the Radioactive Cell spawns as scrap."
            );

            RadioactiveCellSpawnWeight = cfg.Bind(
                "Scrap Item - Radioactive Cell",
                "Radioactive Cell spawn weight",
                15,
                "How often the Radioactive Cell spawns as scrap."
            );

            //========================================

            CrowbarLoaded = cfg.Bind(
                "Scrap Item - Crowbar",
                "Crowbar is loaded",
                true,
                "Whether or not the Crowbar is loaded."
            );

            CrowbarSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Crowbar",
                "Crowbar is a scrap item",
                true,
                "Whether or not the Crowbar spawns as scrap."
            );

            CrowbarSpawnWeight = cfg.Bind(
                "Scrap Item - Crowbar",
                "Crowbar spawn weight",
                30,
                "How often the Crowbar spawns as scrap."
            );

            //========================================

            PadlockLoaded = cfg.Bind(
                "Scrap Item - Padlock",
                "Padlock is loaded",
                true,
                "Whether or not the Padlock is loaded."
            );

            PadlockSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Padlock",
                "Padlock is a scrap item",
                true,
                "Whether or not the Padlock spawns as scrap."
            );

            PadlockSpawnWeight = cfg.Bind(
                "Scrap Item - Padlock",
                "Padlock spawn weight",
                30,
                "How often the Padlock spawns as scrap."
            );

            PadlockIsStoreItem = cfg.Bind(
                "Scrap Item - Padlock",
                "Padlock is a store item",
                false,
                "Whether or not the Padlock is a store item."
            );

            PadlockStorePrice = cfg.Bind(
                "Scrap Item - Padlock",
                "Padlock store price",
                15,
                "The store price of the Padlock."
            );

            //========================================

            WalkingCaneLoaded = cfg.Bind(
                "Scrap Item - Walking Cane",
                "Walking Cane is loaded",
                true,
                "Whether or not the Walking Cane is loaded."
            );

            WalkingCaneSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Walking Cane",
                "Walking Cane is a scrap item",
                true,
                "Whether or not the Walking Cane spawns as scrap."
            );

            WalkingCaneSpawnWeight = cfg.Bind(
                "Scrap Item - Walking Cane",
                "Walking Cane spawn weight",
                20,
                "How often the Walking Cane spawns as scrap."
            );

            //========================================

            ScissorsLoaded = cfg.Bind(
                "Scrap Item - Scissors",
                "Scissors are loaded",
                true,
                "Whether or not the Scissors are loaded."
            );

            ScissorsSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Scissors",
                "Scissors are a scrap item",
                true,
                "Whether or not the Scissors spawn as scrap."
            );

            ScissorsSpawnWeight = cfg.Bind(
                "Scrap Item - Scissors",
                "Scissors spawn weight",
                30,
                "How often the Scissors spawn as scrap."
            );

            //========================================

            RoseLoaded = cfg.Bind(
                "Scrap Item - Rose",
                "Rose is loaded",
                true,
                "Whether or not the Rose is loaded."
            );

            RoseSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Rose",
                "Rose is scrap item",
                true,
                "Whether or not the Rose spawns as scrap."
            );

            RoseSpawnWeight = cfg.Bind(
                "Scrap Item - Rose",
                "Rose spawn weight",
                30,
                "How often the Rose spawns as scrap."
            );

            //========================================

            TicketLoaded = cfg.Bind(
                "Scrap Item - Ticket",
                "Ticket is loaded",
                true,
                "Whether or not the Ticket is loaded."
            );

            TicketSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Ticket",
                "Ticket is a scrap item",
                true,
                "Whether or not the Ticket spawns as scrap."
            );

            TicketSpawnWeight = cfg.Bind(
                "Scrap Item - Ticket",
                "Ticket spawn weight",
                40,
                "How often the Ticket spawns as scrap."
            );

            //========================================

            GoldenTicketLoaded = cfg.Bind(
                "Scrap Item - Golden Ticket",
                "Golden Ticket is loaded",
                true,
                "Whether or not the Golden Ticket is loaded."
            );

            GoldenTicketSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Golden Ticket",
                "Golden Ticket is a scrap item",
                true,
                "Whether or not the Golden Ticket spawns as scrap."
            );

            GoldenTicketSpawnWeight = cfg.Bind(
                "Scrap Item - Golden Ticket",
                "Golden Ticket spawn weight",
                10,
                "How often the Golden Ticket spawns as scrap."
            );

            //========================================

            CandyDispenserLoaded = cfg.Bind(
                "Scrap Item - Candy Dispenser",
                "Candy Dispenser is loaded",
                true,
                "Whether or not the Candy Dispenser and its drops are loaded."
            );

            CandyDispenserSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Candy Dispenser",
                "Candy Dispenser is a scrap Item",
                true,
                "Whether or not the Candy Dispenser spawns as scrap."
            );

            CandyDispenserSpawnWeight = cfg.Bind(
                "Scrap Item - Candy Dispenser",
                "Candy Dispenser spawn weight",
                30,
                "How often the Candy Dispenser spawns as scrap."
            );

            //========================================

            GloomyCapsuleLoaded = cfg.Bind(
                "Scrap Item - Gloomy Capsule",
                "Gloomy Capsule is loaded",
                true,
                "Whether or not the Gloomy Capsule is loaded."
            );

            GloomyCapsuleSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Gloomy Capsule",
                "Gloomy Capsule is a scrap item",
                true,
                "Whether or not the Gloomy Capsule spawns as scrap."
            );

            GloomyCapsuleSpawnWeight = cfg.Bind(
                "Scrap Item - Gloomy Capsule",
                "Gloomy Capsule spawn weight",
                25,
                "How often the Gloomy Capsule spawns as scrap."
            );

            //========================================

            FrigidCapsuleLoaded = cfg.Bind(
                "Scrap Item - Frigid Capsule",
                "Frigid Capsule is loaded",
                true,
                "Whether or not the Frigid Capsule is loaded."
            );

            FrigidCapsuleSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Frigid Capsule",
                "Frigid Capsule is a scrap item",
                true,
                "Whether or not the Frigid Capsule spawns as scrap."
            );

            FrigidCapsuleSpawnWeight = cfg.Bind(
                "Scrap Item - Frigid Capsule",
                "Frigid Capsule spawn weight",
                25,
                "How often the Frigid Capsule spawns as scrap."
            );

            //==================== STORE ITEMS ====================

            HandlampLoaded = cfg.Bind(
                "Store Item - Handlamp",
                "Handlamp is loaded",
                true,
                "Whether or not the Handlamp is loaded."
            );

            HandlampIsStoreItem = cfg.Bind(
                "Store Item - Handlamp",
                "Handlamp is a store item",
                true,
                "Whether or not the Handlamp is a store item."
            );

            HandlampStorePrice = cfg.Bind(
                "Store Item - Handlamp",
                "Handlamp store price",
                25,
                "The store price of the Handlamp."
            );

            //========================================

            BandagesLoaded = cfg.Bind(
                "Store Item - Bandages",
                "Bandages are loaded",
                true,
                "Whether or not the Bandages are loaded."
            );

            BandagesIsStoreItem = cfg.Bind(
                "Store Item - Bandages",
                "Bandages are a store item",
                true,
                "Whether or not the Bandages are a store item."
            );

            BandagesStorePrice = cfg.Bind(
                "Store Item - Bandages",
                "Bandages store price",
                20,
                "The store price of the Bandages."
            );

            //========================================

            MedicalKitLoaded = cfg.Bind(
                "Store Item - Medical Kit",
                "Medical Kit is loaded",
                true,
                "Whether or not the Medical Kit is loaded."
            );

            MedicalKitIsStoreItem = cfg.Bind(
                "Store Item - Medical Kit",
                "Medical Kit is a store item",
                true,
                "Whether or not the Medical Kit is a store item."
            );

            MedicalKitStorePrice = cfg.Bind(
                "Store Item - Medical Kit",
                "Medical Kit store price",
                120,
                "The store price of the Medical Kit."
            );

            //========================================

            DefibrillatorLoaded = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator is loaded",
                true,
                "Whether or not the Defibrillator is loaded."
            );

            DefibrillatorIsStoreItem = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator is a store item",
                true,
                "Whether or not the Defibrillator is a store item."
            );

            DefibrillatorStorePrice = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator store price",
                375,
                "The store price of the Defibrillator."
            );

            DefibrillatorUsesLimited = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillators uses are limited",
                true,
                "Whether or not Defibrillators have limited uses."
            );

            DefibrillatorUseLimit = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillators uses",
                4,
                "The number of uses each Defibrillator has (Uses have a minimum of 1, uses are based on the amount of players the defib has successfully revived)."
            );

            //========================================

            ShiftControllerLoaded = cfg.Bind(
                "Store Item - Shift Controller",
                "Shift Controller is loaded",
                true,
                "Whether or not the Shift Controller is loaded."
            );

            ShiftControllerIsStoreItem = cfg.Bind(
                "Store Item - Shift Controller",
                "Shift Controller is a store item",
                true,
                "Whether or not the Shift Controller is a store item."
            );

            ShiftControllerStorePrice = cfg.Bind(
                "Store Item - Shift Controller",
                "Shift Controller store price",
                225,
                "The store price of the Shift Controller."
            );

            ShiftControllerGreatRange = cfg.Bind(
                "Store Item - Shift Controller",
                "Shift Controllers great/first connection range",
                50,
                $"The max range of the Shift Controller's great/first connection range. (The great/first connection range starts at 0 and stops at this value, The average/middle connection range starts at this value)"
            );

            ShiftControllerPoorRange = cfg.Bind(
                "Store Item - Shift Controller",
                "Shift Controllers poor/last connection range",
                175,
                $"The range where the Shift Controller enters it's poor/last connection range. (The average/middle connection range stops at this value, The poor/last connection range starts at this value)"
            );

            //========================================

            EmergencyInjectorLoaded = cfg.Bind(
                "Store Item - Emergency Injector",
                "Emergency Injector is loaded",
                true,
                "Whether or not the Emergency Injector is loaded."
            );

            EmergencyInjectorIsStoreItem = cfg.Bind(
                "Store Item - Emergency Injector",
                "Emergency Injector is a store item",
                true,
                "Whether or not the Emergency Injector is a store item."
            );

            EmergencyInjectorStorePrice = cfg.Bind(
                "Store Item - Emergency Injector",
                "Emergency Injector store price",
                60,
                "The store price of the Emergency Injector."
            );

            //========================================

            ToolboxLoaded = cfg.Bind(
                "Store Item - Toolbox",
                "Toolbox is loaded",
                true,
                "Whether or not the Toolbox is loaded."
            );

            ToolboxIsStoreItem = cfg.Bind(
                "Store Item - Toolbox",
                "Toolbox is a store item",
                true,
                "Whether or not the Toolbox is a store item."
            );

            ToolboxStorePrice = cfg.Bind(
                "Store Item - Toolbox",
                "Toolbox store price",
                115,
                "The store price of the Toolbox."
            );

            //========================================

            PocketWatchLoaded = cfg.Bind(
                "General Items",
                "PocketWatch is loaded",
                true,
                "Whether or not the PocketWatch is loaded."
            );

            PocketWatchSpawnsAsScrap = cfg.Bind(
                "General Items",
                "PocketWatch is a scrap item",
                true,
                "Whether or not the PocketWatch spawns as scrap."
            );

            PocketWatchSpawnWeight = cfg.Bind(
                "General Items",
                "PocketWatch spawn weight",
                20,
                "How often the PocketWatch spawns as scrap."
            );

            //========================================

            CapsulesDisabledOnTheShip = cfg.Bind(
                "General Mechanic Changes",
                "Capsules Disabled On The Ship",
                true,
                "Whether or not capsules are disabled on the ship when landed regardless of their individual mechanics (Capsules are disabled in orbit always)."
            );



            ClearOrphanedEntries(cfg);
            cfg.Save();
            cfg.SaveOnConfigSet = true;
        }

        static void ClearOrphanedEntries(ConfigFile cfg)
        {
            PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg);
            orphanedEntries.Clear();
        }
    }
}
