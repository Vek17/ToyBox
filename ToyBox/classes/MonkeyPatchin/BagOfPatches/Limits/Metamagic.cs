﻿// borrowed shamelessly and enhanced from Bag of Tricks https://www.nexusmods.com/pathfinderkingmaker/mods/26, which is under the MIT License
using HarmonyLib;
using Kingmaker;
//using Kingmaker.Controllers.GlobalMap;
using Kingmaker.RuleSystem.Rules;
//using Kingmaker.UI._ConsoleUI.Models;
//using Kingmaker.UI.RestCamp;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using ModKit;
//using Kingmaker.UI._ConsoleUI.GroupChanger;

namespace ToyBox.BagOfPatches {
    internal static class MetamagicPatches {
        public static Settings settings = Main.settings;
        public static Player player = Game.Instance.Player;

        [HarmonyPatch(typeof(MetamagicHelper), "DefaultCost")]
        public static class MetamagicHelper_DefaultCost_Patch {
            public static void Postfix(ref int __result) {
                if (settings.toggleMetamagicIsFree) {
                    __result = 0;
                }
            }
        }

        [HarmonyPatch(typeof(RuleCollectMetamagic), "AddMetamagic")]
        public static class RuleCollectMetamagic_AddMetamagic_Patch {
            public static bool Prefix() => !settings.toggleMetamagicIsFree;
            public static void Postfix(ref RuleCollectMetamagic __instance, int ___m_SpellLevel, Feature metamagicFeature) {
                if (settings.toggleMetamagicIsFree) {
                    var component = metamagicFeature.GetComponent<AddMetamagicFeat>();
                    if (component == null) {
                        Mod.Trace(string.Format("Trying to add metamagic feature without metamagic component: {0}", metamagicFeature));
                    }
                    else {
                        __instance.KnownMetamagics.Add(metamagicFeature);
                        var metamagic = component.Metamagic;
                        if (___m_SpellLevel < 0 || ___m_SpellLevel >= 10 || ___m_SpellLevel + component.Metamagic.DefaultCost() > 10 || __instance.SpellMetamagics.Contains(metamagicFeature) || (__instance.Spell.AvailableMetamagic & metamagic) != metamagic)
                            return;
                        __instance.SpellMetamagics.Add(metamagicFeature);
                    }
                }
            }
        }
    }
}