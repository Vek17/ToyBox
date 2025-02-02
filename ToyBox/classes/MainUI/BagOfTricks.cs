// Copyright < 2021 > Narria (github user Cabarius) - License: MIT

using Kingmaker;
using Kingmaker.Cheats;
using Kingmaker.Kingdom;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using ModKit;
using System;
using System.Linq;
using UnityModManagerNet;
using static ModKit.UI;

namespace ToyBox {
    public static class BagOfTricks {
        public static Settings settings => Main.settings;

        // cheats combat
        private const string RestAll = "Rest All";
        private const string Empowered = "Empowered";
        private const string FullBuffPlease = "Common Buffs";
        private const string GoddesBuffs = "Buff Like A Godess";
        private const string RemoveBuffs = "Remove Buffs";
        private const string RemoveDeathsDoor = "Remove Deaths Door";
        private const string KillAllEnemies = "Kill All Enemies";
        //private const string SummonZoo = "Summon Zoo"
        private const string LobotomizeAllEnemies = "Lobotomize Enemies";

        // cheats common
        private const string TeleportPartyToYou = "Teleport Party To You";
        private const string GoToGlobalMap = "Go To Global Map";
        private const string RerollPerception = "Reroll Perception";
        private const string RerollInteractionSkillChecks = "Reroll Interaction Skill Checks";
        private const string ChangeParty = "Change Party";

        // other
        private const string TimeScaleMultToggle = "Main/Alt Timescale";
        private const string PreviewDialogResults = "Preview Results";

        public static void OnLoad() {
            // Combat
            KeyBindings.RegisterAction(RestAll, () => CheatsCombat.RestAll());
            KeyBindings.RegisterAction(Empowered, () => CheatsCombat.Empowered(""));
            KeyBindings.RegisterAction(FullBuffPlease, () => CheatsCombat.FullBuffPlease(""));
            KeyBindings.RegisterAction(GoddesBuffs, () => CheatsCombat.Iddqd(""));
            KeyBindings.RegisterAction(RemoveBuffs, () => Actions.RemoveAllBuffs());
            KeyBindings.RegisterAction(RemoveDeathsDoor, () => CheatsCombat.DetachDebuff());
            KeyBindings.RegisterAction(KillAllEnemies, () => CheatsCombat.KillAll());
            //KeyBindings.RegisterAction(SummonZoo, () => CheatsCombat.SpawnInspectedEnemiesUnderCursor(""));
            KeyBindings.RegisterAction(LobotomizeAllEnemies, () => Actions.LobotomizeAllEnemies());
            // Common
            KeyBindings.RegisterAction(TeleportPartyToYou, () => Teleport.TeleportPartyToPlayer());
            KeyBindings.RegisterAction(GoToGlobalMap, () => Teleport.TeleportToGlobalMap());
            KeyBindings.RegisterAction(RerollPerception, () => Actions.RunPerceptionTriggers());
            KeyBindings.RegisterAction(RerollInteractionSkillChecks, () => Actions.RerollInteractionSkillChecks());
            KeyBindings.RegisterAction(ChangeParty, () => { Actions.ChangeParty(); });
            // Other
            KeyBindings.RegisterAction(TimeScaleMultToggle, () => {
                settings.useAlternateTimeScaleMultiplier = !settings.useAlternateTimeScaleMultiplier;
                Actions.ApplyTimeScale();
            });
            KeyBindings.RegisterAction(PreviewDialogResults, () => {
                settings.previewDialogResults = !settings.previewDialogResults;
                var dialogController = Game.Instance.DialogController;
            });
        }
        public static void ResetGUI() { }
        public static void OnGUI() {
#if BUILD_CRUI
            ActionButton("Demo crUI", () => ModKit.crUI.Demo());
#endif
            if (Main.IsInGame) {
                BeginHorizontal();
                Space(25);
                Label("increment".cyan(), AutoWidth());
                var increment = IntTextField(ref settings.increment, null, Width(150));
                EndHorizontal();
                var mainChar = Game.Instance.Player.MainCharacter.Value;
                var kingdom = KingdomState.Instance;
                HStack("Resources", 1,
                    () => {
                        var money = Game.Instance.Player.Money;
                        Label("Gold".cyan(), Width(150));
                        Label(money.ToString().orange().bold(), Width(200));
                        ActionButton($"Gain {increment}", () => Game.Instance.Player.GainMoney(increment), AutoWidth());
                        ActionButton($"Lose {increment}", () => {
                            var loss = Math.Min(money, increment);
                            Game.Instance.Player.GainMoney(-loss);
                        }, AutoWidth());
                    },
                    () => {
                        var exp = mainChar.Progression.Experience;
                        Label("Experience".cyan(), Width(150));
                        Label(exp.ToString().orange().bold(), Width(200));
                        ActionButton($"Gain {increment}", () => {
                            Game.Instance.Player.GainPartyExperience(increment);
                        }, AutoWidth());
                    });
            }
            Div(0, 25);
            HStack("Combat", 2,
                () => BindableActionButton(RestAll),
                () => BindableActionButton(FullBuffPlease),
                () => BindableActionButton(Empowered),
                () => BindableActionButton(GoddesBuffs),
                () => BindableActionButton(RemoveBuffs),
                () => BindableActionButton(RemoveDeathsDoor),
                () => BindableActionButton(KillAllEnemies),
                //() => UI.BindableActionButton(SummonZoo),
                () => BindableActionButton(LobotomizeAllEnemies),
                () => { }
                );
            Div(0, 25);
            HStack("Teleport", 2,
                () => BindableActionButton(TeleportPartyToYou),
                () => {
                    Toggle("Enable Teleport Keys", ref settings.toggleTeleportKeysEnabled);
                    Space(100);
                    if (settings.toggleTeleportKeysEnabled) {
                        using (VerticalScope()) {
                            KeyBindPicker("TeleportMain", "Main Character", 0, 200);
                            KeyBindPicker("TeleportSelected", "Selected Chars", 0, 200);
                            KeyBindPicker("TeleportParty", "Whole Party", 0, 200);
                        }
                    }
                    UI.Space(25);
                    UI.Label("You can enable hot keys to teleport members of your party to your mouse cursor on Area or the Global Map".green());
                });
            Div(0, 25);
            HStack("Common", 2,
                () => BindableActionButton(GoToGlobalMap),
                () => BindableActionButton(RerollPerception),
                () => BindableActionButton(ChangeParty),
                () => {
                    NonBindableActionButton("Set Perception to 40", () => {
                        CheatsCommon.StatPerception();
                        Actions.RunPerceptionTriggers();
                    });
                },
                () => NonBindableActionButton("Change Weather", () => CheatsCommon.ChangeWeather("")),
                () => NonBindableActionButton("Give All Items", () => CheatsUnlock.CreateAllItems("")),
                () => NonBindableActionButton("Identify All", () => Actions.IdentifyAll()),
                () => { }
                );
            Div(0, 25);
            HStack("Preview", 0, () => {
                Toggle("Dialog Results", ref settings.previewDialogResults);
                Space(25);
                Toggle("Dialog Alignment", ref settings.previewAlignmentRestrictedDialog);
                Space(25);
                Toggle("Random Encounters", ref settings.previewRandomEncounters);
                Space(25);
                Toggle("Events", ref settings.previewEventResults);
                Space(25);
                BindableActionButton(PreviewDialogResults);
            });
            Div(0, 25);
            HStack("Dialog", 1,
                () => {
                    Toggle("Expand Dialog To Include Remote Companions".bold(), ref settings.toggleRemoteCompanionDialog);
                    Space(100);
                    Label("Experimental".orange() + " Allow remote companions to make comments on dialog you are having.".green());
                },
                () => {
                    if (settings.toggleRemoteCompanionDialog) {
                        Space(50);
                        Toggle("Include Former Companions", ref settings.toggleExCompanionDialog);
                        Space(25);
                    }
                },
                () => { }
            );
            Div(0, 25);
            HStack("Quality of Life", 1,
                () => {
                    Toggle("Allow Achievements While Using Mods", ref settings.toggleAllowAchievementsDuringModdedGame);
                    UI.Space(25);
                    Label("This is intended for you to be able to enjoy the game while using mods that enhance your quality of life.  Please be mindful of the player community and avoid using this mod to trivialize earning prestige achievements like Sadistic Gamer. The author is in discussion with Owlcat about reducing the scope of achievement blocking to just these. Let's show them that we as players can mod and cheat responsibly.".orange());
                },
                () => Toggle("Object Highlight Toggle Mode", ref settings.highlightObjectsToggle),
                () => Toggle("Highlight Copyable Scrolls", ref settings.toggleHighlightCopyableScrolls),
                () => {
                    Toggle("Icky Stuff Begone!!!", ref settings.toggleReplaceModelMenu, UI.AutoWidth());
                    if (settings.toggleReplaceModelMenu) {
                        Space(25);
                        using (VerticalScope(UI.Width(250))) {
                            Toggle("Spiders Begone!", ref settings.toggleSpiderBegone);
                            Toggle("Vescavors Begone!", ref settings.toggleVescavorsBegone);
                            Toggle("Retrievers Begone!", ref settings.toggleRetrieversBegone);
                        }
                    }
                    UI.Space(25);
                    UI.Label("Some players find spiders and other swarms icky. This replaces them with something more pleasent".green());
                },
                () => Toggle("Make Tutorials Not Appear If Disabled In Settings", ref settings.toggleForceTutorialsToHonorSettings),
                () => Toggle("Refill consumables in belt slots if in inventory", ref settings.togglAutoEquipConsumables),
                () => Toggle("Allow Shift Click To Use Items In Inventory", ref settings.toggleShiftClickToUseInventorySlot),
                () => Toggle("Allow Shift Click To Transfer Entire Stack", ref settings.toggleShiftClickToFastTransfer),
                () => ActionButton("Fix Incorrect Main Character", () => {
                    var probablyPlayer = Game.Instance.Player?.Party?
                        .Where(x => !x.IsCustomCompanion())
                        .Where(x => !x.IsStoryCompanion()).ToList();
                    if (probablyPlayer is { Count: 1 }) {
                        var newMainCharacter = probablyPlayer.First();
                        Mod.Warn($"Promoting {newMainCharacter.CharacterName} to main character!");
                        if (Game.Instance != null) Game.Instance.Player.MainCharacter = newMainCharacter;
                    }
                }, AutoWidth()),
                () => {
                    using (VerticalScope()) {
                        Div(0, 25, 1280);
                        var useAlt = settings.useAlternateTimeScaleMultiplier;
                        var mainTimeScaleTitle = "Game Time Scale";
                        if (useAlt) mainTimeScaleTitle = mainTimeScaleTitle.grey();
                        var altTimeScaleTitle = "Alternate Time Scale";
                        if (!useAlt) altTimeScaleTitle = altTimeScaleTitle.grey();
                        using (HorizontalScope()) {
                            LogSlider(mainTimeScaleTitle, ref settings.timeScaleMultiplier, 0f, 20, 1, 1, "", Width(450));
                            Space(25);
                            Label("Speeds up or slows down the entire game (movement, animation, everything)".green());
                        }
                        using (HorizontalScope()) {
                            LogSlider(altTimeScaleTitle, ref settings.alternateTimeScaleMultiplier, 0f, 20, 5, 1, "", Width(450));
                        }
                        using (HorizontalScope()) {
                            BindableActionButton(TimeScaleMultToggle);
                            Space(-95);
                            Label("Bindable hot key to swap between main and alternate time scale multipliers".green());
                        }
                        Div(0, 25, 1280);
                    }
                },
            () => { }
            );
            Div(0, 25);
            HStack("Alignment", 1,
                () => { Toggle("Fix Alignment Shifts", ref settings.toggleAlignmentFix); Space(119); Label("Makes alignment shifts towards pure good/evil/lawful/chaotic only shift on those axes".green()); },
                () => { Toggle("Prevent Alignment Changes", ref settings.togglePreventAlignmentChanges); Space(25); Label("See Party Editor for more fine grained alignment locking per character".green()); },
                () => { }
                );
            Div(0, 25);
            HStack("Cheats", 1,
                () => {
                    using (HorizontalScope()) {
                        ToggleCallback("Highlight Hidden Objects", ref settings.highlightHiddenObjects, Actions.UpdateHighlights);
                        if (settings.highlightHiddenObjects) {
                            Space(100);
                            ToggleCallback("In Fog Of War ", ref settings.highlightHiddenObjectsInFog, Actions.UpdateHighlights);
                        }
                    }
                },
                () => Toggle("Infinite Abilities", ref settings.toggleInfiniteAbilities),
                () => Toggle("Infinite Spell Casts", ref settings.toggleInfiniteSpellCasts),
                () => Toggle("No Material Components", ref settings.toggleMaterialComponent),
                () => Toggle("Disable Party Negative Levels", ref settings.togglePartyNegativeLevelImmunity),
                () => Toggle("Disable Party Ability Damage", ref settings.togglePartyAbilityDamageImmunity),

                () => Toggle("Unlimited Actions During Turn", ref settings.toggleUnlimitedActionsPerTurn),
                () => Toggle("Infinite Charges On Items", ref settings.toggleInfiniteItems),

                () => Toggle("Instant Cooldown", ref settings.toggleInstantCooldown),

                () => Toggle("Spontaneous Caster Scroll Copy", ref settings.toggleSpontaneousCopyScrolls),

                () => Toggle("Disable Equipment Restrictions", ref settings.toggleEquipmentRestrictions),
                () => Toggle("Disable Armor Max Dexterity", ref settings.toggleIgnoreMaxDexterity),
                () => Toggle("Disable Armor Speed Reduction", ref settings.toggleIgnoreSpeedReduction),
                () => Toggle("Disable Armor & Shield Arcane Spell Failure", ref settings.toggleIgnoreSpellFailure),
                () => Toggle("Disable Armor & Shield Checks Penalty", ref settings.toggleIgnoreArmorChecksPenalty),

                () => Toggle("Disable Dialog Restrictions (Alignment)", ref settings.toggleDialogRestrictions),
                () => Toggle("Disable Dialog Restrictions (Mythic Path)", ref settings.toggleDialogRestrictionsMythic),
#if DEBUG
                () => Toggle("Disable Dialog Restrictions (Everything, Experimental)", ref settings.toggleDialogRestrictionsEverything),
#endif
                () => Toggle("No Friendly Fire On AOEs", ref settings.toggleNoFriendlyFireForAOE),
                () => Toggle("Free Meta-Magic", ref settings.toggleMetamagicIsFree),

                () => Toggle("No Fog Of War", ref settings.toggleNoFogOfWar),
                () => Toggle("Restore Spells & Skills After Combat", ref settings.toggleRestoreSpellsAbilitiesAfterCombat),
                //() => UI.Toggle("Recharge Items After Combat", ref settings.toggleRechargeItemsAfterCombat),
                //() => UI.Toggle("Access Remote Characters", ref settings.toggleAccessRemoteCharacters,0),
                //() => UI.Toggle("Show Pet Portraits", ref settings.toggleShowAllPartyPortraits,0),
                () => Toggle("Instant Rest After Combat", ref settings.toggleInstantRestAfterCombat),
                () => Toggle("Disallow Companions Leaving Party (experimental; only enable while needed)", ref settings.toggleBlockUnrecruit),
                () => Toggle("Disable Romance IsLocked Flag (experimental)", ref settings.toggleMultipleRomance),
                () => Toggle("Instant change party members", ref settings.toggleInstantChangeParty),
                () => ToggleCallback("Equipment No Weight", ref settings.toggleEquipmentNoWeight, BagOfPatches.Tweaks.NoWeight_Patch1.Refresh),
                () => Toggle("Allow Item Use From Inventory During Combat", ref settings.toggleUseItemsDuringCombat),
                () => Toggle("Ignore Alignment Requirements for Abilities", ref settings.toggleIgnoreAbilityAlignmentRestriction),
                () => Toggle("Remove Level 20 Caster Level Cap", ref settings.toggleUncappedCasterLevel),
                () => { }
                );
            Div(153, 25);
            HStack("", 1,
                () => EnumGrid("Disable Attacks Of Opportunity", ref settings.noAttacksOfOpportunitySelection, AutoWidth()),
                    () => EnumGrid("Can Move Through", ref settings.allowMovementThroughSelection, AutoWidth()),
                    () => {
                        Space(328); Label("This allows characters you control to move through the selected category of units during combat".green(), AutoWidth());
                    }
#if false
                () => { UI.Slider("Collision Radius Multiplier", ref settings.collisionRadiusMultiplier, 0f, 2f, 1f, 1, "", UI.AutoWidth()); },
#endif
                );
            Div(0, 25);
            HStack("Class Specific", 1,
                () => Slider("Kineticist: Burn Reduction", ref settings.kineticistBurnReduction, 0, 30, 1, "", AutoWidth()),
                        () => {
                            Space(25);
                            Label("Please rest after adjusting to recalculate your spell slots.".green());
                        },
                        () => Toggle("Witch/Shaman: Cackling/Shanting Extends Hexes By 10 Min (Out Of Combat)", ref settings.toggleExtendHexes),
                        () => Toggle("Allow Simultaneous Activatable Abilities (Like Judgements)", ref settings.toggleAllowAllActivatable),
                        () => Toggle("Kineticist: Allow Gather Power Without Hands", ref settings.toggleKineticistGatherPower),
                        () => Toggle("Barbarian: Auto Start Rage When Entering Combat", ref settings.toggleEnterCombatAutoRage),
                        () => Toggle("Demon: Auto Start Rage When Entering Combat", ref settings.toggleEnterCombatAutoRageDemon),
                        () => Toggle("Magus: Always Allow Spell Combat", ref settings.toggleAlwaysAllowSpellCombat),
                        () => { }
                        );
            Div(0, 25);
            HStack("Multipliers", 1,
                () => LogSlider("Experience", ref settings.experienceMultiplier, 0f, 20, 1, 1, "", AutoWidth()),
                () => LogSlider("Money Earned", ref settings.moneyMultiplier, 0f, 20, 1, 1, "", AutoWidth()),
                () => LogSlider("Vendor Sell Price", ref settings.vendorSellPriceMultiplier, 0f, 20, 1, 1, "", AutoWidth()),
                () => LogSlider("Vendor Buy Price", ref settings.vendorBuyPriceMultiplier, 0f, 20, 1, 1, "", AutoWidth()),
                () => Slider("Increase Carry Capacity", ref settings.encumberanceMultiplier, 1, 100, 1, "", AutoWidth()),
                () => {
                    LogSlider("Movement Speed", ref settings.partyMovementSpeedMultiplier, 0f, 20, 1, 1, "", Width(600));
                    Space(25);
                    Toggle("Whole Team Moves Same Speed", ref settings.toggleMoveSpeedAsOne);
                    Space(25);
                    Label("Adjusts the movement speed of your party in area maps".green());
                },
                () => {
                    LogSlider("Travel Speed", ref settings.travelSpeedMultiplier, 0f, 20, 1, 1, "", Width(600));
                    Space(25);
                    Label("Adjusts the movement speed of your party on world maps".green());
                },
                () => {
                    LogSlider("Companion Cost", ref settings.companionCostMultiplier, 0, 20, 1, 1, "", Width(600));
                    Space(25);
                    Label("Adjusts costs of hiring mercenaries at the Pathfinder vendor".green());

                },
                () => LogSlider("Enemy HP Multiplier", ref settings.enemyBaseHitPointsMultiplier, 0.1f, 20, 1, 1, "", AutoWidth()),
                () => LogSlider("Field Of View", ref settings.fovMultiplier, 0.4f, 5.0f, 1, 2, "", AutoWidth()),
                () => LogSlider("FoV (Cut Scenes)", ref settings.fovMultiplierCutScenes, 0.4f, 5.0f, 1, 2, "", AutoWidth()),
                () => { }
                );
            Actions.ApplyTimeScale();
            Div(0, 25);
            HStack("Dice Rolls", 1,
                () => EnumGrid("All Attacks Hit", ref settings.allAttacksHit, AutoWidth()),
                () => EnumGrid("All Hits Critical", ref settings.allHitsCritical, AutoWidth()),
                () => EnumGrid("Roll With Avantage", ref settings.rollWithAdvantage, AutoWidth()),
                () => EnumGrid("Roll With Disavantage", ref settings.rollWithDisadvantage, AutoWidth()),
                () => EnumGrid("Always Roll 20", ref settings.alwaysRoll20, AutoWidth()),
                () => EnumGrid("Always Roll 1", ref settings.alwaysRoll1, AutoWidth()),
                () => EnumGrid("Never Roll 20", ref settings.neverRoll20, AutoWidth()),
                () => EnumGrid("Never Roll 1", ref settings.neverRoll1, AutoWidth()),
                () => EnumGrid("Always Roll 20 Initiative ", ref settings.roll20Initiative, AutoWidth()),
                () => EnumGrid("Always Roll 1 Initiative", ref settings.roll1Initiative, AutoWidth()),
                () => EnumGrid("Always Roll 20 Out Of Combat", ref settings.alwaysRoll20OutOfCombat, AutoWidth()),
                () => EnumGrid("Take 10 Out of Combat (Always)", ref settings.take10always, AutoWidth()),
                () => EnumGrid("Take 10 Out of Combat (Minimum)", ref settings.take10minimum, AutoWidth()),
                () => { }
                );
            Div(0, 25);
            HStack("Summons", 1,
                () => Toggle("Make Controllable", ref settings.toggleMakeSummmonsControllable),
                () => {
                    using (VerticalScope()) {
                        Div(0, 25);
                        using (HorizontalScope()) {
                            Label("Primary".orange(), AutoWidth()); Space(215); Label("good for party".green());
                        }
                        Space(25);
                        EnumGrid("Modify Summons For", ref settings.summonTweakTarget1, AutoWidth());
                        LogSlider("Duration Multiplier", ref settings.summonDurationMultiplier1, 0f, 20, 1, 2, "", AutoWidth());
                        Slider("Level Increase/Decrease", ref settings.summonLevelModifier1, -20f, +20f, 0f, 0, "", AutoWidth());
                        Div(0, 25);
                        using (HorizontalScope()) {
                            Label("Secondary".orange(), AutoWidth()); Space(215); Label("good for larger group or to reduce enemies".green());
                        }
                        Space(25);
                        EnumGrid("Modify Summons For", ref settings.summonTweakTarget2, AutoWidth());
                        LogSlider("Duration Multiplier", ref settings.summonDurationMultiplier2, 0f, 20, 1, 2, "", AutoWidth());
                        Slider("Level Increase/Decrease", ref settings.summonLevelModifier2, -20f, +20f, 0f, 0, "", AutoWidth());
                    }
                },
                () => { }
             );
        }
    }
}
