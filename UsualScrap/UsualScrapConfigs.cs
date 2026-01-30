using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;

namespace UsualScrap
{
    class UsualScrapConfigs
    {
        public readonly ConfigEntry<bool> MansionScrapPackEnabled;
        //public readonly ConfigEntry<string> MansionScrapPackMoonSpawns;
        public readonly ConfigEntry<bool> FacilityScrapPackEnabled;
        //public readonly ConfigEntry<string> FacilityScrapPackMoonSpawns;

        public readonly ConfigEntry<bool> TicketLoaded;
        public readonly ConfigEntry<bool> TicketSpawnsAsScrap;
        public readonly ConfigEntry<string> TicketMoonSpawnWeights;
        public readonly ConfigEntry<int> TicketGlobalSpawnWeight;

        public readonly ConfigEntry<bool> GoldenTicketLoaded;
        public readonly ConfigEntry<bool> GoldenTicketSpawnsAsScrap;
        public readonly ConfigEntry<string> GoldenTicketMoonSpawnWeights;
        public readonly ConfigEntry<int> GoldenTicketGlobalSpawnWeight;

        public readonly ConfigEntry<bool> RoseLoaded;
        public readonly ConfigEntry<bool> RoseSpawnsAsScrap;
        public readonly ConfigEntry<string> RoseMoonSpawnWeights;
        public readonly ConfigEntry<int> RoseGlobalSpawnWeight;

        public readonly ConfigEntry<bool> ScissorsLoaded;
        public readonly ConfigEntry<bool> ScissorsSpawnsAsScrap;
        public readonly ConfigEntry<string> ScissorsMoonSpawnWeights;
        public readonly ConfigEntry<int> ScissorsGlobalSpawnWeight;

        public readonly ConfigEntry<bool> WalkingCaneLoaded;
        public readonly ConfigEntry<bool> WalkingCaneSpawnsAsScrap;
        public readonly ConfigEntry<string> WalkingCaneMoonSpawnWeights;
        public readonly ConfigEntry<int> WalkingCaneGlobalSpawnWeight;

        public readonly ConfigEntry<bool> CandyDispenserLoaded;
        public readonly ConfigEntry<bool> CandyDispenserSpawnsAsScrap;
        public readonly ConfigEntry<string> CandyDispenserMoonSpawnWeights;
        public readonly ConfigEntry<int> CandyDispenserGlobalSpawnWeight;

        public readonly ConfigEntry<bool> FuelCylinderLoaded;
        public readonly ConfigEntry<bool> FuelCylinderSpawnsAsScrap;
        public readonly ConfigEntry<string> FuelCylinderMoonSpawnWeights;
        public readonly ConfigEntry<int> FuelCylinderGlobalSpawnWeight;

        public readonly ConfigEntry<bool> RadioactiveCellLoaded;
        public readonly ConfigEntry<bool> RadioactiveCellSpawnsAsScrap;
        public readonly ConfigEntry<string> RadioactiveCellMoonSpawnWeights;
        public readonly ConfigEntry<int> RadioactiveCellGlobalSpawnWeight;

        public readonly ConfigEntry<bool> GloomyCapsuleLoaded;
        public readonly ConfigEntry<bool> GloomyCapsuleSpawnsAsScrap;
        public readonly ConfigEntry<string> GloomyCapsuleMoonSpawnWeights;
        public readonly ConfigEntry<int> GloomyCapsuleGlobalSpawnWeight;

        public readonly ConfigEntry<bool> FrigidCapsuleLoaded;
        public readonly ConfigEntry<bool> FrigidCapsuleSpawnsAsScrap;
        public readonly ConfigEntry<string> FrigidCapsuleMoonSpawnWeights;
        public readonly ConfigEntry<int> FrigidCapsuleGlobalSpawnWeight;

        public readonly ConfigEntry<bool> NoxiousCapsuleLoaded;
        public readonly ConfigEntry<bool> NoxiousCapsuleSpawnsAsScrap;
        public readonly ConfigEntry<string> NoxiousCapsuleMoonSpawnWeights;
        public readonly ConfigEntry<int> NoxiousCapsuleGlobalSpawnWeight;

        public readonly ConfigEntry<bool> BloodyCapsuleLoaded;
        public readonly ConfigEntry<bool> BloodyCapsuleSpawnsAsScrap;
        public readonly ConfigEntry<string> BloodyCapsuleMoonSpawnWeights;
        public readonly ConfigEntry<int> BloodyCapsuleGlobalSpawnWeight;

        public readonly ConfigEntry<bool> PadlockLoaded;
        public readonly ConfigEntry<bool> PadlockSpawnsAsScrap;
        public readonly ConfigEntry<string> PadlockMoonSpawnWeights;
        public readonly ConfigEntry<int> PadlockGlobalSpawnWeight;
        public readonly ConfigEntry<bool> PadlockIsStoreItem;
        public readonly ConfigEntry<int> PadlockStorePrice;

        public readonly ConfigEntry<bool> CrowbarLoaded;
        public readonly ConfigEntry<bool> CrowbarSpawnsAsScrap;
        public readonly ConfigEntry<string> CrowbarMoonSpawnWeights;
        public readonly ConfigEntry<int> CrowbarGlobalSpawnWeight;
        public readonly ConfigEntry<bool> CrowbarIsStoreItem;
        public readonly ConfigEntry<int> CrowbarStorePrice;

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
        public readonly ConfigEntry<bool> DefibrillatorRequiresBattery;
        public readonly ConfigEntry<bool> DefibrillatorUsesLimited;
        public readonly ConfigEntry<int> DefibrillatorUseLimit;
        public readonly ConfigEntry<bool> DefibrillatorPermaDeathRule;
        public readonly ConfigEntry<bool> DefibrillatorRefillsOnLanding;

        public readonly ConfigEntry<bool> ProductivityAutoinjectorLoaded;
        public readonly ConfigEntry<bool> ProductivityAutoinjectorIsStoreItem;
        public readonly ConfigEntry<int> ProductivityAutoinjectorStorePrice;

        public readonly ConfigEntry<bool> ToolboxLoaded;
        public readonly ConfigEntry<bool> ToolboxIsStoreItem;
        public readonly ConfigEntry<int> ToolboxStorePrice;

        public readonly ConfigEntry<bool> ShiftControllerLoaded;
        public readonly ConfigEntry<bool> ShiftControllerIsStoreItem;
        public readonly ConfigEntry<int> ShiftControllerStorePrice;
        public readonly ConfigEntry<int> ShiftControllerGreatRange;
        public readonly ConfigEntry<int> ShiftControllerPoorRange;

        public readonly ConfigEntry<bool> CapsulesDisabledOnTheShip;
        public readonly ConfigEntry<bool> TicketsFunctionOnCheapItems;
        public readonly ConfigEntry<bool> DisableItemIcons;

        public readonly ConfigEntry<bool> DebugLogging;

        public UsualScrapConfigs(ConfigFile cfg)
        {

            cfg.SaveOnConfigSet = false;

            //========================================

            DebugLogging = cfg.Bind(
                "1. Debugging",
                "Log actions for debugging purposes",
                false,
                "Whether or not to log actions for debugging purposes"
            );

            //========================================

            CapsulesDisabledOnTheShip = cfg.Bind(
                "2. General Changes",
                "Capsules Disabled On The Ship",
                true,
                "Whether or not capsules are disabled inside the ship when landed (Capsules are disabled in orbit always)"
            );

            TicketsFunctionOnCheapItems = cfg.Bind(
                "2. General Changes",
                "Tickets function on cheap items",
                true,
                "Whether or not tickets function when used on items with a value below 5 (Meaning tickets will no longer work on most equipment whose value is normally 0)"
            );

            DisableItemIcons = cfg.Bind(
                "2. General Changes",
                "Disable Item Icons",
                false,
                "Whether or not this mod's items icons are applied (For use with runtime icons and similar mods)"
            );

            //========================================

            MansionScrapPackEnabled = cfg.Bind(
                "3. Mansion Scrap Pack",
                "Mansion Scrap Pack Enabled",
                false,
                "Whether or not the Mansion Scrap Pack is enabled"
            );

            /*
            MansionScrapPackMoonSpawns = cfg.Bind(
                "2 Mansion Scrap Pack",
                "Mansion Scrap Pack Spawning",
                "experimentation:True, vow:True, march:True, assurance:True, offense:True, rend:True, dine:True, titan:true, adamance:True, artifice:true, embrion:true",
                "Where Mansion Scrap Pack items will spawn."
            );
            */

            //==================== Scrap Packs ====================

            FacilityScrapPackEnabled = cfg.Bind(
                "3. Facility Scrap Pack",
                "Facility Scrap Pack Enabled",
                false,
                "Whether or not the Facility Scrap Pack is enabled"
            );

            /*
            FacilityScrapPackMoonSpawns = cfg.Bind(
                "2 Facility Scrap Pack",
                "Facility Scrap Pack Spawning",
                "experimentation:True, vow:True, march:True, assurance:True, offense:True, rend:True, dine:True, titan:true, adamance:True, artifice:true, embrion:true",
                "Where Facility Scrap Pack items will spawn."
            );
            */

            //==================== Scrap Items ====================

            TicketLoaded = cfg.Bind(
                "Scrap Item - Ticket",
                "US_Ticket is loaded",
                true,
                "Whether or not the US_Ticket is loaded"
            );

            TicketSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Ticket",
                "US_Ticket is a scrap item",
                true,
                "Whether or not the US_Ticket spawns as scrap"
            );

            TicketMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Ticket",
                "US_Ticket Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the US_Ticket will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            TicketGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Ticket",
                "US_Ticket Global Spawning",
                40,
                "How often the US_Ticket will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            GoldenTicketLoaded = cfg.Bind(
                "Scrap Item - Golden Ticket",
                "Golden Ticket is loaded",
                true,
                "Whether or not the Golden Ticket is loaded"
            );

            GoldenTicketSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Golden Ticket",
                "Golden Ticket is a scrap item",
                true,
                "Whether or not the Golden Ticket spawns as scrap"
            );

            GoldenTicketMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Golden Ticket",
                "Golden Ticket Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Golden Ticket will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            GoldenTicketGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Golden Ticket",
                "Golden Ticket Global Spawning",
                5,
                "How often the Golden Ticket will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            RoseLoaded = cfg.Bind(
                "Scrap Item - Rose",
                "Rose is loaded",
                true,
                "Whether or not the Rose is loaded"
            );

            RoseSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Rose",
                "Rose is a scrap item",
                true,
                "Whether or not the Rose spawns as scrap"
            );

            RoseMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Rose",
                "Rose Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Rose will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            RoseGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Rose",
                "Rose Global Spawning",
                30,
                "How often the Rose will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            ScissorsLoaded = cfg.Bind(
                "Scrap Item - Scissors",
                "Scissors is loaded",
                true,
                "Whether or not the Scissors is loaded"
            );

            ScissorsSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Scissors",
                "Scissors is a scrap item",
                true,
                "Whether or not the Scissors spawns as scrap"
            );

            ScissorsMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Scissors",
                "Scissors Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Scissors will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            ScissorsGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Scissors",
                "Scissors Global Spawning",
                30,
                "How often the Scissors will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            WalkingCaneLoaded = cfg.Bind(
                "Scrap Item - Walking Cane",
                "Walking Cane is loaded",
                true,
                "Whether or not the Walking Cane is loaded"
            );

            WalkingCaneSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Walking Cane",
                "Walking Cane is a scrap item",
                true,
                "Whether or not the Walking Cane spawns as scrap"
            );

            WalkingCaneMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Walking Cane",
                "Walking Cane Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Walking Cane will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            WalkingCaneGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Walking Cane",
                "Walking Cane Global Spawning",
                15,
                "How often the Walking Cane will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            CandyDispenserLoaded = cfg.Bind(
                "Scrap Item - Candy Dispenser",
                "Candy Dispenser is loaded",
                true,
                "Whether or not the Candy Dispenser is loaded"
            );

            CandyDispenserSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Candy Dispenser",
                "Candy Dispenser is a scrap item",
                true,
                "Whether or not the Candy Dispenser spawns as scrap"
            );

            CandyDispenserMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Candy Dispenser",
                "Candy Dispenser Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Candy Dispenser will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            CandyDispenserGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Candy Dispenser",
                "Candy Dispenser Global Spawning",
                30,
                "How often the Candy Dispenser will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            FuelCylinderLoaded = cfg.Bind(
                "Scrap Item - Fuel Cylinder",
                "Fuel Cylinder is loaded",
                true,
                "Whether or not the Fuel Cylinder is loaded"
            );

            FuelCylinderSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Fuel Cylinder",
                "Fuel Cylinder is a scrap item",
                true,
                "Whether or not the Fuel Cylinder spawns as scrap"
            );

            FuelCylinderMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Fuel Cylinder",
                "Fuel Cylinder Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Fuel Cylinder will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            FuelCylinderGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Fuel Cylinder",
                "Fuel Cylinder Global Spawning",
                20,
                "How often the Fuel Cylinder will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            RadioactiveCellLoaded = cfg.Bind(
                "Scrap Item - Radioactive Cell",
                "Radioactive Cell is loaded",
                true,
                "Whether or not the Radioactive Cell is loaded"
            );

            RadioactiveCellSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Radioactive Cell",
                "Radioactive Cell is a scrap item",
                true,
                "Whether or not the Radioactive Cell spawns as scrap"
            );

            RadioactiveCellMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Radioactive Cell",
                "Radioactive Cell Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Radioactive Cell will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            RadioactiveCellGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Radioactive Cell",
                "Radioactive Cell Global Spawning",
                20,
                "How often the Radioactive Cell will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            GloomyCapsuleLoaded = cfg.Bind(
                "Scrap Item - Gloomy Capsule",
                "Gloomy Capsule is loaded",
                true,
                "Whether or not the Gloomy Capsule is loaded"
            );

            GloomyCapsuleSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Gloomy Capsule",
                "Gloomy Capsule is a scrap item",
                true,
                "Whether or not the Gloomy Capsule spawns as scrap"
            );

            GloomyCapsuleMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Gloomy Capsule",
                "Gloomy Capsule Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Gloomy Capsule will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            GloomyCapsuleGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Gloomy Capsule",
                "Gloomy Capsule Global Spawning",
                15,
                "How often the Gloomy Capsule will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //-----

            FrigidCapsuleLoaded = cfg.Bind(
                "Scrap Item - Frigid Capsule",
                "Frigid Capsule is loaded",
                true,
                "Whether or not the Frigid Capsule is loaded"
            );

            FrigidCapsuleSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Frigid Capsule",
                "Frigid Capsule is a scrap item",
                true,
                "Whether or not the Frigid Capsule spawns as scrap"
            );

            FrigidCapsuleMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Frigid Capsule",
                "Frigid Capsule Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Frigid Capsule will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            FrigidCapsuleGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Frigid Capsule",
                "Frigid Capsule Global Spawning",
                15,
                "How often the Frigid Capsule will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            NoxiousCapsuleLoaded = cfg.Bind(
                "Scrap Item - Noxious Capsule",
                "Noxious Capsule is loaded",
                true,
                "Whether or not the Noxious Capsule is loaded"
            );

            NoxiousCapsuleSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Noxious Capsule",
                "Noxious Capsule is a scrap item",
                true,
                "Whether or not the Noxious Capsule spawns as scrap"
            );

            NoxiousCapsuleMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Noxious Capsule",
                "Noxious Capsule Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Noxious Capsule will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            NoxiousCapsuleGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Noxious Capsule",
                "Noxious Capsule Global Spawning",
                15,
                "How often the Noxious Capsule will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            BloodyCapsuleLoaded = cfg.Bind(
                "Scrap Item - Bloody Capsule",
                "Bloody Capsule is loaded",
                true,
                "Whether or not the Bloody Capsule is loaded"
            );

            BloodyCapsuleSpawnsAsScrap = cfg.Bind(
                "Scrap Item - Bloody Capsule",
                "Bloody Capsule is a scrap item",
                true,
                "Whether or not the Bloody Capsule spawns as scrap"
            );

            BloodyCapsuleMoonSpawnWeights = cfg.Bind(
                "Scrap Item - Bloody Capsule",
                "Bloody Capsule Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Bloody Capsule will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            BloodyCapsuleGlobalSpawnWeight = cfg.Bind(
                "Scrap Item - Bloody Capsule",
                "Bloody Capsule Global Spawning",
                15,
                "How often the Bloody Capsule will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            //========================================

            PadlockLoaded = cfg.Bind(
                "Item - Padlock",
                "Padlock is loaded",
                true,
                "Whether or not the Padlock is loaded"
            );

            PadlockSpawnsAsScrap = cfg.Bind(
                "Item - Padlock",
                "Padlock is a scrap item",
                true,
                "Whether or not the Padlock spawns as scrap"
            );

            PadlockMoonSpawnWeights = cfg.Bind(
                "Item - Padlock",
                "Padlock Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the Padlock will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            PadlockGlobalSpawnWeight = cfg.Bind(
                "Item - Padlock",
                "Padlock Global Spawning",
                30,
                "How often the Padlock will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            PadlockIsStoreItem = cfg.Bind(
                "Item - Padlock",
                "Padlock is a store item",
                false,
                "Whether or not the Padlock is a store item"
            );

            PadlockStorePrice = cfg.Bind(
                "Item - Padlock",
                "Padlock store price",
                15,
                "The store price of the Padlock (Cannot be a negative number)"
            );

            //========================================

            CrowbarLoaded = cfg.Bind(
                "Item - Crowbar",
                "Crowbar is loaded",
                true,
                "Whether or not the US_Crowbar is loaded"
            );

            CrowbarSpawnsAsScrap = cfg.Bind(
                "Item - Crowbar",
                "Crowbar is a scrap item",
                true,
                "Whether or not the US_Crowbar spawns as scrap"
            );

            CrowbarMoonSpawnWeights = cfg.Bind(
                "Item - Crowbar",
                "Crowbar Moon Spawning",
                "experimentation:-1, vow:-1, march:-1, assurance:-1, offense:-1, rend:-1, dine:-1, titan:-1, adamance:-1, artifice:-1, embrion:-1",
                "Where and how often the US_Crowbar will spawn (Moon spawn weights will individually overwrite global spawn weights, set to -1 or leave blank to disable. Correct spelling and formatting is critical, shown, and a log will appear pointing out unreadable/disabled configs"
            );

            CrowbarGlobalSpawnWeight = cfg.Bind(
                "Item - Crowbar",
                "Crowbar Global Spawning",
                30,
                "How often the US_Crowbar will spawn globally (Will affect all moons including modded moons unless overwritten by moon spawn weights, -1 to disable)"
            );

            CrowbarIsStoreItem = cfg.Bind(
                "Item - Crowbar",
                "Crowbar is a store item",
                false,
                "Whether or not the US_Crowbar is a store item"
            );

            CrowbarStorePrice = cfg.Bind(
                "Item - Crowbar",
                "Crowbar store price",
                60,
                "The store price of the US_Crowbar (Cannot be a negative number)"
            );

            //==================== Store ====================

            HandlampLoaded = cfg.Bind(
                "Store Item - Handlamp",
                "Handlamp is loaded",
                true,
                "Whether or not the Handlamp is loaded"
            );

            HandlampIsStoreItem = cfg.Bind(
                "Store Item - Handlamp",
                "Handlamp is a store item",
                true,
                "Whether or not the Handlamp is a store item"
            );

            HandlampStorePrice = cfg.Bind(
                "Store Item - Handlamp",
                "Handlamp store price",
                25,
                "The store price of the Handlamp (Cannot be a negative number)"
            );

            //========================================

            BandagesLoaded = cfg.Bind(
                "Store Item - Bandages",
                "Bandages are loaded",
                true,
                "Whether or not the Bandages are loaded"
            );

            BandagesIsStoreItem = cfg.Bind(
                "Store Item - Bandages",
                "Bandages are a store item",
                true,
                "Whether or not the Bandages are a store item"
            );

            BandagesStorePrice = cfg.Bind(
                "Store Item - Bandages",
                "Bandages store price",
                20,
                "The store price of the Bandages (Cannot be a negative number)"
            );

            //========================================

            MedicalKitLoaded = cfg.Bind(
                "Store Item - Medical Kit",
                "Medical Kit is loaded",
                true,
                "Whether or not the Medical Kit is loaded"
            );

            MedicalKitIsStoreItem = cfg.Bind(
                "Store Item - Medical Kit",
                "Medical Kit is a store item",
                true,
                "Whether or not the Medical Kit is a store item"
            );

            MedicalKitStorePrice = cfg.Bind(
                "Store Item - Medical Kit",
                "Medical Kit store price",
                120,
                "The store price of the Medical Kit (Cannot be a negative number)"
            );

            //========================================

            DefibrillatorLoaded = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator is loaded",
                true,
                "Whether or not the Defibrillator is loaded"
            );

            DefibrillatorIsStoreItem = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator is a store item",
                true,
                "Whether or not the Defibrillator is a store item"
            );

            DefibrillatorStorePrice = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator store price",
                375,
                "The store price of the Defibrillator (Cannot be a negative number)"
            );

            DefibrillatorRequiresBattery = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator requires battery",
                true,
                "Whether or not the Defibrillator requires battery"
            );

            DefibrillatorUsesLimited = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator uses are limited",
                true,
                "Whether or not Defibrillators have limited uses"
            );

            DefibrillatorUseLimit = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillators use limit",
                3,
                "The number of uses each Defibrillator has (Uses have a minimum of 1, uses are used up on players successfully revived only)"
            );
            DefibrillatorRefillsOnLanding = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator refills every moon landing",
                false,
                "Whether or not existing Defibrillators will refill their uses upon landing on a moon"
            );
            DefibrillatorPermaDeathRule = cfg.Bind(
                "Store Item - Defibrillator",
                "Defibrillator follows perma-death rules",
                true,
                "Whether or not specific causes of death will prevent the defibrillator from reviving players (Ex - Corpses decapitated by CoilHeads, Corpses snipped in half by a Barber, Corpses head popped by the Ghost Girl)"
            );

            //========================================

            ProductivityAutoinjectorLoaded = cfg.Bind(
                "Store Item - Productivity Autoinjector",
                "Productivity Autoinjector is loaded",
                true,
                "Whether or not the Productivity Autoinjector is loaded"
            );

            ProductivityAutoinjectorIsStoreItem = cfg.Bind(
                "Store Item - Productivity Autoinjector",
                "Productivity Autoinjector is a store item",
                true,
                "Whether or not the Productivity Autoinjector is a store item"
            );

            ProductivityAutoinjectorStorePrice = cfg.Bind(
                "Store Item - Productivity Autoinjector",
                "Productivity Autoinjector store price",
                90 ,
                "The store price of the Productivity Autoinjector"
            );

            //========================================

            ToolboxLoaded = cfg.Bind(
                "Store Item - Toolbox",
                "Toolbox is loaded",
                true,
                "Whether or not the Toolbox is loaded"
            );

            ToolboxIsStoreItem = cfg.Bind(
                "Store Item - Toolbox",
                "Toolbox is a store item",
                true,
                "Whether or not the Toolbox is a store item"
            );

            ToolboxStorePrice = cfg.Bind(
                "Store Item - Toolbox",
                "Toolbox store price",
                115,
                "The store price of the Toolbox"
            );

            //========================================

            ShiftControllerLoaded = cfg.Bind(
                "Store Item - Shift Controller",
                "Shift Controller is loaded",
                true,
                "Whether or not the Shift Controller is loaded"
            );

            ShiftControllerIsStoreItem = cfg.Bind(
                "Store Item - Shift Controller",
                "Shift Controller is a store item",
                true,
                "Whether or not the Shift Controller is a store item"
            );

            ShiftControllerStorePrice = cfg.Bind(
                "Store Item - Shift Controller",
                "Shift Controller store price",
                225,
                "The store price of the Shift Controller"
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
