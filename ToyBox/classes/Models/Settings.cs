// Copyright < 2021 > Narria (github user Cabarius) - License: MIT
using ModKit;
using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;

namespace ToyBox {

    public class Settings : UnityModManager.ModSettings {
        // Main
        public int selectedTab = 0;

        // Cheap Tricks
        public int increment = 10000;
        public int kineticistBurnReduction = 0;

        // Tweaks
        public bool highlightObjectsToggle = false;
        public bool toggleHighlightCopyableScrolls = false;
        public bool toggleSpontaneousCopyScrolls = false;
        public bool toggleInstantEvent = false;
        public bool toggleInfiniteAbilities = false;
        public bool toggleInfiniteSpellCasts = false;
        public bool toggleInfiniteSkillpoints = false;
        public bool toggleInstantCooldown = false;
        public bool toggleUnlimitedActionsPerTurn = false;
        public bool toggleEquipmentRestrictions = false;
        public bool toggleIgnoreMaxDexterity = false;
        public bool toggleIgnoreArmorChecksPenalty = false;
        public bool toggleIgnoreSpeedReduction = false;
        public bool toggleIgnoreSpellFailure = false;
        public bool togglePartyNegativeLevelImmunity = false;
        public bool togglePartyAbilityDamageImmunity = false;
        public bool toggleDialogRestrictions = false;
        public bool toggleDialogRestrictionsMythic = false;
        public bool toggleDialogRestrictionsEverything = false;
        public bool toggleNoFriendlyFireForAOE = false;
        public bool toggleSettlementRestrictions = false;
        public bool toggleMoveSpeedAsOne = false;
        public bool toggleNoFogOfWar = false;
        public bool toggleRestoreSpellsAbilitiesAfterCombat = false;
        public bool toggleRechargeItemsAfterCombat = false;
        public bool toggleInstantRestAfterCombat = false;
        public bool toggleShowAllPartyPortraits = false;    // TODO - port this
        public bool toggleAccessRemoteCharacters = false;
        public bool toggleInfiniteItems = false;
        public bool toggleMetamagicIsFree = false;
        public bool toggleMaterialComponent = false;
        public bool toggleBlockUnrecruit = false;
        public bool toggleAllowAchievementsDuringModdedGame = false;
        public bool toggleForceTutorialsToHonorSettings = false;
        public bool toggleMultipleRomance = false;
        public bool toggleReplaceModelMenu = false;
        public bool toggleSpiderBegone = false;
        public bool toggleVescavorsBegone = false;
        public bool toggleRetrieversBegone = false;
        public bool togglAutoEquipConsumables = false;
        public bool toggleInstantChangeParty = false;
        public bool toggleExtendHexes = false;
        public bool toggleAllowAllActivatable = false;
        public bool toggleKineticistGatherPower = false;
        public bool toggleAlwaysAllowSpellCombat = false;
        public bool toggleEnterCombatAutoRage = false;
        public bool toggleEnterCombatAutoRageDemon = false;
        public bool toggleEquipmentNoWeight = false;
        public bool toggleUseItemsDuringCombat = false;
        public bool toggleTeleportKeysEnabled = false;
        public bool toggleShiftClickToUseInventorySlot = false;
        public bool toggleShiftClickToFastTransfer = false;
        public bool toggleAlignmentFix = false;
        public bool togglePreventAlignmentChanges = false;
        public bool toggleBrutalUnfair = false;
        public bool toggleRemoteCompanionDialog = false;
        public bool toggleExCompanionDialog = false;
        public bool highlightHiddenObjects = false;
        public bool highlightHiddenObjectsInFog = false;

        // Loot 
        public bool toggleColorLootByRarity = false;
        public RarityType lootFilterIgnore = RarityType.None;
        public RarityType lootFilterAutoSell = RarityType.None;
        public bool toggleMassLootEverything = false;
        public bool toggleLootChecklistFilterFriendlies = false;
        public bool toggleLootChecklistFilterBlueprint = false;
        public bool toggleLootChecklistFilterDescription = false;
        public RarityType lootChecklistFilterRarity = RarityType.None;


        //Crusade
        public bool toggleInfiniteArmyRerolls = false;
        public bool toggleLargeArmies = false;
        public bool toggleCrusadeFlagsStayGreen = false;
        public float postBattleSummonMultiplier = 1;
        public float recruitmentCost = 1;
        public float recruitmentMultiplier = 1;
        public float armyExperienceMultiplier = 1;
        public float playerLeaderPowerMultiplier = 1;
        public float enemyLeaderPowerMultiplier = 1;
        public float kingdomTaskResolutionLengthMultiplier = 0;

        // selectors
        public UnitSelectType noAttacksOfOpportunitySelection = UnitSelectType.Off;
        public UnitSelectType allowMovementThroughSelection = UnitSelectType.Off;
        public float collisionRadiusMultiplier = 1;

        // char creation
        public bool toggleAllRaceCustomizations = false;
        public bool toggleIgnoreGenderRestrictions = false;

        // level up
        public bool toggleNoLevelUpRestrictions = false;
        public bool toggleFullHitdiceEachLevel = false;
        public bool toggleIgnoreClassAndFeatRestrictions = false;
        public bool toggleAllowCompanionsToBecomeMythic = false;
        public bool toggleIgnorePrerequisites = false;
        public bool toggleIgnoreCasterTypeSpellLevel = false;
        public bool toggleIgnoreForbiddenArchetype = false;
        public bool toggleIgnorePrerequisiteStatValue = false;
        public bool toggleIgnoreAlignmentWhenChoosingClass = false;
        public bool toggleIgnoreFeaturePrerequisitesWhenChoosingClass = false;  // TODO - implement
        public bool toggleIgnoreForbiddenFeatures = false;
        public bool toggleIgnoreFeaturePrerequisites = false;
        public bool toggleIgnoreFeatureListPrerequisites = false;
        public bool toggleFeaturesIgnorePrerequisites = false;
        public bool toggleSkipSpellSelection = false;
        public bool toggleNextWhenNoAvailableFeatSelections = true;
        public bool toggleOptionalFeatSelection = false;
        public bool toggleUniversalSpellbookd = false;
        public bool toggleUncappedCasterLevel = false;

        // Multipliers
        public int encumberanceMultiplier = 1;
        public float experienceMultiplier = 1;
        public float moneyMultiplier = 1;
        public float vendorSellPriceMultiplier = 1;
        public float vendorBuyPriceMultiplier = 1;
        public float fatigueHoursModifierMultiplier = 1;
        public float partyMovementSpeedMultiplier = 1;
        public float travelSpeedMultiplier = 1;
        public int characterCreationAbilityPointsMax = 18;
        public int characterCreationAbilityPointsMin = 7;
        public int characterCreationAbilityPointsPlayer = 25;
        public int characterCreationAbilityPointsMerc = 20;
        public float companionCostMultiplier = 1;
        public float kingdomBuildingTimeModifier = 0;
        public float enemyBaseHitPointsMultiplier = 1;
        public float fovMultiplier = 1;
        public float fovMultiplierCutScenes = 1;
        public float fovMultiplierMax = 1.25f;
        public float timeScaleMultiplier = 1;
        public float alternateTimeScaleMultiplier = 3;
        public bool useAlternateTimeScaleMultiplier = false;

        // Dice Rolls
        public UnitSelectType allAttacksHit = UnitSelectType.Off;
        public UnitSelectType allHitsCritical = UnitSelectType.Off;
        public UnitSelectType rollWithAdvantage = UnitSelectType.Off;
        public UnitSelectType rollWithDisadvantage = UnitSelectType.Off;
        public UnitSelectType alwaysRoll20 = UnitSelectType.Off;
        public UnitSelectType alwaysRoll20OutOfCombat = UnitSelectType.Off;
        public UnitSelectType neverRoll20 = UnitSelectType.Off;
        public UnitSelectType alwaysRoll1 = UnitSelectType.Off;
        public UnitSelectType neverRoll1 = UnitSelectType.Off;
        public UnitSelectType roll20Initiative = UnitSelectType.Off;
        public UnitSelectType roll1Initiative = UnitSelectType.Off;
        public UnitSelectType take10always = UnitSelectType.Off;
        public UnitSelectType take10minimum = UnitSelectType.Off;

        // Summons
        public bool toggleMakeSummmonsControllable = false;
        public UnitSelectType summonTweakTarget1 = UnitSelectType.Off;
        public float summonDurationMultiplier1 = 1;
        public float summonLevelModifier1 = 0;
        public UnitSelectType summonTweakTarget2 = UnitSelectType.Off;
        public float summonDurationMultiplier2 = 1;
        public float summonLevelModifier2 = 0;

        // Key Bindings
        //public SerializableDictionary<string, KeyCode>

        public KeyCode teleportMainHotKey = KeyCode.Comma;
        public KeyCode teleportSelectedHotKey = KeyCode.Period;
        public KeyCode teleportPartyHotKey = KeyCode.Slash;

        // Party Editor
        public int selectedPartyFilter = 0;

        // Blueprint Browser
        public int searchLimit = 100;
        public int selectedBPTypeFilter = 1;
        public string searchText = "";
        public bool searchDescriptions = false; // keeping around for a bit to not break saves 
        public bool searchesDescriptions = true;
        public bool showAttributes = false;
        public bool showAssetIDs = false;
        public bool showComponents = false;
        public bool showElements = false;
        public bool showDivisions = true;
        public bool showFromAllSpellbooks = false;

        public string searchTextEnchantments = "";

        // Previews (Dialogs, Events ,etc)

        public bool previewEventResults = false;
        public bool previewDialogResults = false;
        public bool previewAlignmentRestrictedDialog = false;
        public bool previewRandomEncounters = false;

        // Quests
        public bool hideCompleted = true;

        // these are useful and belong in Char Generation
        public bool toggleIgnoreAlignmentRestrictionCharGen = false;    // was called toggleIgnoreAlignmentRestriction in Multi-Class mod
        public bool toggleIgnoreAttributeCap = false;
        public bool toggleIgnoreAttributePointsRemainingChargen = false;

        // these are useful and belong in LevelUp
        public bool toggleLockCharacterLevel = false;
        public bool toggleIgnoreAttributePointsRemaining = false;
        public bool toggleIgnoreSkillCap = false;
        public bool toggleIgnoreSkillPointsRemaining = false;

        // Some of these look redundant.  It might be nice to add the fine grain configuration but part of the philosphy of ToyBox is to avoid too much kitchen sink options.  I would like to focus and simplify this.  Maybe see if there is a way to unify these into some broader groupings like I did in Cheap Tricks for patches that adopted CheckUnitEntityData (Off, You, Party, Enemies, etc)
        // public bool toggleIgnoreClassAndFeatRestrictions = false; 
        public HashSet<string> ignoredPrerequisiteSet = new(); // adding this granularity might be nice

        public bool toggleIgnoreAbilityAlignmentRestriction = false;
        public bool toggleIgnoreAolityCasterCheckers = false;
        public HashSet<string> ignoredAbilityCasterCheckerSet = new();
        public bool toggleIgnoreActivatableAbilityRestrictions = false;
        public HashSet<string> ignoredActivatableAbilityRestrictionSet = new();
        public bool toggleIgnoreEquipmentRestrictions = false;
        public HashSet<string> ignoredEquipmentRestrictionSet = new();
        public bool toggleIgnoreBuildingRestrictions = false;
        public HashSet<string> ignoredBuildingRestrictionSet = new();

        // Development
        public LogLevel loggingLevel = LogLevel.Info;
        public bool stripHtmlTagsFromUMMLogsTab = false;
        public bool stripHtmlTagsFromNativeConsole = true;
        public bool toggleShowDebugInfo = true;
        public bool toggleDevopmentMode = false;
        public bool toggleUberLoggerForwardPrefix = false;

        // Localization
        public string uiCultureCode = "en";

        // Deprecated
        public bool toggleNoLevelUpRestirctions = false;    // deprecated
        internal bool toggleSpellbookAbilityAlignmentChecks = false;

        // Save
        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);

    }
}

