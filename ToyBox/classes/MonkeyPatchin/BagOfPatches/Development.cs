﻿using HarmonyLib;
using Kingmaker.Utility;
using ModKit;
using Owlcat.Runtime.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Logger = UnityModManagerNet.UnityModManager.Logger;

namespace ToyBox.classes.MonkeyPatchin.BagOfPatches {
    internal class Development {
        public static Settings settings = Main.settings;

        [HarmonyPatch(typeof(BuildModeUtility), "IsDevelopment", MethodType.Getter)]
        private static class BuildModeUtility_IsDevelopment_Patch {
            private static void Postfix(ref bool __result) {
                if (settings.toggleDevopmentMode) {
                    __result = true;
                }
            }
        }
        [HarmonyPatch(typeof(SmartConsole), "WriteLine")]
        private static class SmartConsole_WriteLine_Patch {
            private static void Postfix(string message) {
                if (settings.toggleDevopmentMode) {
                    Mod.Log(message);
                    var timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    UberLoggerAppWindow.Instance.Log(new LogInfo(null, LogChannel.Default, timestamp, LogSeverity.Message, new List<LogStackFrame>(), false, message, Array.Empty<object>())
                        );
                }
            }
        }
        [HarmonyPatch(typeof(SmartConsole), "Initialise")]
        private static class SmartConsole_Initialise_Patch {
            private static void Postfix() {
                if (settings.toggleDevopmentMode) {
                    SmartConsoleCommands.Register();
                }
            }
        }


        [HarmonyPatch(typeof(Owlcat.Runtime.Core.Logging.Logger), "ForwardToUnity")]
        private static class UberLoggerLogger_ForwardToUnity_Patch {
            private static void Prefix(ref object message) {
                if (settings.toggleUberLoggerForwardPrefix) {
                    var message1 = "[UberLogger] " + message;
                    message = message1;
                }
            }
        }
        // This patch if for you @ArcaneTrixter and @Vek17
        [HarmonyPatch(typeof(Logger), "Write")]
        private static class Logger_Logger_Patch {
            private static bool Prefix(string str, bool onlyNative = false) {
                if (str == null)
                    return false;
                var stripHTMLNative = settings.stripHtmlTagsFromNativeConsole;
                var sriptHTMLHistory = settings.stripHtmlTagsFromUMMLogsTab;
                Console.WriteLine(stripHTMLNative ? str.StripHTML() : str);

                if (onlyNative)
                    return false;
                if (sriptHTMLHistory) str = str.StripHTML();
                Logger.buffer.Add(str);
                Logger.history.Add(str);

                if (Logger.history.Count >= Logger.historyCapacity * 2) {
                    var result = Logger.history.Skip(Logger.historyCapacity);
                    Logger.history.Clear();
                    Logger.history.AddRange(result);
                }
                return false;
            }
        }
    }
}
