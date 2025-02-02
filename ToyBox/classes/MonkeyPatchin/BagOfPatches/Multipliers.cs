﻿// borrowed shamelessly and enhanced from Bag of Tricks https://www.nexusmods.com/pathfinderkingmaker/mods/26, which is under the MIT License

using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.Items;
using Kingmaker.Settings;
using Kingmaker.Settings.Difficulty;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.View;
using ModKit;
using System;
using System.Linq;
using UnityEngine;

namespace ToyBox.BagOfPatches {
    internal static class Multipliers {
        public static Settings settings = Main.settings;
        public static Player player = Game.Instance.Player;

        [HarmonyPatch(typeof(EncumbranceHelper), "GetHeavy")]
        private static class EncumbranceHelper_GetHeavy_Patch {
            private static void Postfix(ref int __result) => __result = Mathf.RoundToInt(__result * settings.encumberanceMultiplier);
        }

        [HarmonyPatch(typeof(UnitPartWeariness), "GetFatigueHoursModifier")]
        private static class EncumbranceHelper_GetFatigueHoursModifier_Patch {
            private static void Postfix(ref float __result) => __result *= (float)Math.Round(settings.fatigueHoursModifierMultiplier, 1);
        }

        [HarmonyPatch(typeof(Player), "GainPartyExperience")]
        public static class Player_GainPartyExperience_Patch {
            [HarmonyPrefix]
            public static bool Prefix(Player __instance, ref int gained) {
                gained = Mathf.RoundToInt(gained * (float)Math.Round(settings.experienceMultiplier, 1));
                return true;
            }
        }

        [HarmonyPatch(typeof(Player), "GainMoney")]
        public static class Player_GainMoney_Patch {
            [HarmonyPrefix]
            public static bool Prefix(Player __instance, ref long amount) {
                amount = Mathf.RoundToInt(amount * (float)Math.Round(settings.moneyMultiplier, 1));
                return true;
            }
        }

        [HarmonyPatch(typeof(Player), "GetCustomCompanionCost")]
        public static class Player_GetCustomCompanionCost_Patch {
            public static bool Prefix(ref bool __state) => !__state;    // FIXME - why did Bag of Tricks do this?

            public static void Postfix(ref int __result) => __result = Mathf.RoundToInt(__result * settings.companionCostMultiplier);
        }

        /**
        public Buff AddBuff(
          BlueprintBuff blueprint,
          UnitEntityData caster,
          TimeSpan? duration,
          [CanBeNull] AbilityParams abilityParams = null) {
            MechanicsContext context = new MechanicsContext(caster, this.Owner, (SimpleBlueprint)blueprint);
            if (abilityParams != null)
                context.SetParams(abilityParams);
            return this.Manager.Add<Buff>(new Buff(blueprint, context, duration));
        }
        */
#if false
        [HarmonyPatch(typeof(Buff), "AddBuff")]
        [HarmonyPatch(new Type[] { typeof(BlueprintBuff), typeof(UnitEntityData), typeof(TimeSpan?), typeof(AbilityParams) })]
        public static class Buff_AddBuff_patch {
            public static void Prefix(BlueprintBuff blueprint, UnitEntityData caster, ref TimeSpan? duration, [CanBeNull] AbilityParams abilityParams = null) {
                try {
                    if (!caster.IsPlayersEnemy) {
                        if (duration != null) {
                            duration = TimeSpan.FromTicks(Convert.ToInt64(duration.Value.Ticks * settings.buffDurationMultiplierValue));
                        }
                    }
                }
                catch (Exception e) {
                    Mod.Error(e);
                }

                Mod.Debug("Initiator: " + caster.CharacterName + "\nBlueprintBuff: " + blueprint.Name + "\nDuration: " + duration.ToString());
            }
        }
#endif


        private static readonly string[] badBuffs = new string[]
             {
                 "24cf3deb078d3df4d92ba24b176bda97", //Prone
                 "e6f2fc5d73d88064583cb828801212f4" //Fatigued
             };

        private static bool isGoodBuff(BlueprintBuff blueprint) => !blueprint.Harmful && !badBuffs.Contains(blueprint.AssetGuidThreadSafe);

        [HarmonyPatch(typeof(VendorLogic), "GetItemSellPrice", new Type[] { typeof(ItemEntity) })]
        private static class VendorLogic_GetItemSellPrice_Patch {
            private static void Postfix(ref long __result) => __result = (long)(__result * settings.vendorSellPriceMultiplier);
        }
        [HarmonyPatch(typeof(VendorLogic), "GetItemSellPrice", new Type[] { typeof(BlueprintItem) })]
        private static class VendorLogic_GetItemSellPrice_Patch2 {
            private static void Postfix(ref long __result) => __result = (long)(__result * settings.vendorSellPriceMultiplier);
        }

        [HarmonyPatch(typeof(VendorLogic), "GetItemBuyPrice", new Type[] { typeof(ItemEntity) })]
        private static class VendorLogic_GetItemBuyPrice_Patch {
            private static void Postfix(ref long __result) => __result = (long)(__result * settings.vendorBuyPriceMultiplier);
        }
        [HarmonyPatch(typeof(VendorLogic), "GetItemBuyPrice", new Type[] { typeof(BlueprintItem) })]
        private static class VendorLogic_GetItemBuyPrice_Patc2h {
            private static void Postfix(ref long __result) => __result = (long)(__result * settings.vendorBuyPriceMultiplier);
        }

        [HarmonyPatch(typeof(CameraZoom), "TickZoom")]
        private static class CameraZoom_TickZoom {
            private static bool firstCall = true;
            private static readonly float BaseFovMin = 17.5f;
            private static readonly float BaseFovMax = 30;
            public static bool Prefix(CameraZoom __instance) {
                if (settings.fovMultiplier == 1) return true;
                if (firstCall) {
                    //Main.Log($"baseMin/Max: {__instance.FovMin} {__instance.FovMax}");
                    if (__instance.FovMin != BaseFovMin) {
                        Mod.Warn($"Warning: game has changed FovMin to {__instance.FovMin} vs {BaseFovMin}. Toy Box should be updated to avoid stability issues when enabling and disabling the mod repeatedly".orange().bold());
                        //BaseFovMin = __instance.FovMin;
                    }
                    if (__instance.FovMax != BaseFovMax) {
                        Mod.Warn($"Warning: game has changed FovMax to {__instance.FovMax} vs {BaseFovMax}. Toy Box should be updated to avoid stability issues when enabling and disabling the mod repeatedly".orange().bold());
                        //BaseFovMax = __instance.FovMax;
                    }
                    firstCall = false;
                }
                __instance.FovMax = BaseFovMax * settings.fovMultiplier;
                __instance.FovMin = BaseFovMin / settings.fovMultiplier;
                if (__instance.m_ZoomRoutine != null)
                    return true;
                if (!__instance.IsScrollBusy && Game.Instance.IsControllerMouse)
                    __instance.m_PlayerScrollPosition += __instance.IsOutOfScreen ? 0.0f : Input.GetAxis("Mouse ScrollWheel");
                __instance.m_ScrollPosition = __instance.m_PlayerScrollPosition;
                __instance.m_ScrollPosition = Mathf.Clamp(__instance.m_ScrollPosition, 0.0f, __instance.m_ZoomLenght);
                __instance.m_SmoothScrollPosition = Mathf.Lerp(__instance.m_SmoothScrollPosition, __instance.m_ScrollPosition, Time.unscaledDeltaTime * __instance.m_Smooth);
                __instance.m_Camera.fieldOfView = Mathf.Lerp(__instance.FovMax, __instance.FovMin, __instance.CurrentNormalizePosition);
                __instance.m_PlayerScrollPosition = __instance.m_ScrollPosition;
                return true;
            }
        }

        [HarmonyPatch(typeof(CameraRig), "SetMode")]
        private static class CameraRig_SetMode_Apply {
            public static void Postfix(CameraRig __instance, CameraMode mode) {
                if (settings.fovMultiplierCutScenes == 1 && settings.fovMultiplier == 1) return;
                if (mode == CameraMode.Default && Game.Instance.CurrentMode == GameModeType.Cutscene) {
                    __instance.Camera.fieldOfView = __instance.CameraZoom.FovMax * settings.fovMultiplierCutScenes / settings.fovMultiplier;
                }
            }
        }

#if false
        [HarmonyPatch(typeof(BlueprintArea), nameof(BlueprintArea.CameraMode), MethodType.Getter)]
        static class BlueprintArea_CameraMode_Patch {
            public static void Postfix(BlueprintArea __instance, CameraMode __result) {
                Main.Log("hi");
                __result = CameraMode.Default;
            }
        }
#endif
    }
}
