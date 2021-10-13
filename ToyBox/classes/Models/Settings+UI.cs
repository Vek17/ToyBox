﻿using ModKit;
using System.Globalization;
using System.Threading;

namespace ToyBox {
    public partial class SettingsUI {
        public static CultureInfo uiCulture = Thread.CurrentThread.CurrentUICulture;
        public static string cultureSearchText = "";
        public static void OnGUI() {
            UI.HStack("Settings", 1,
                () => {
                    UI.Toggle("Enable Game Development Mode", ref Main.settings.toggleDevopmentMode);
                    UI.Space(25);
                    UI.Label("This turns on the developer console which lets you access cheat commands, shows a FPS window (hife with F11), etc".green());
                },
                () => UI.Label(""),
                () => UI.EnumGrid("Log Level", ref Main.settings.loggingLevel, UI.AutoWidth()),
                () => UI.Label(""),
                () => UI.Toggle("Strip HTML (colors) from Native Console", ref Main.settings.stripHtmlTagsFromNativeConsole),
#if DEBUG
                () => UI.Toggle("Strip HTML (colors) from Logs Tab in Unity Mod Manager", ref Main.settings.stripHtmlTagsFromUMMLogsTab),
#endif
              () => { }
            );
#if DEBUG
            UI.Div(0, 25);
            UI.HStack("Localizaton", 1,
                () => {
                    var cultureInfo = Thread.CurrentThread.CurrentUICulture;
                    var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(ci => ci.DisplayName).ToList();
                    using (UI.VerticalScope()) {
                        using (UI.HorizontalScope()) {
                            UI.Label("Current Cultrue".cyan(), UI.Width(275));
                            UI.Space(25);
                            UI.Label($"{cultureInfo.DisplayName}({cultureInfo.Name})".orange());

                        }
                        if (UI.GridPicker<CultureInfo>("Culture", ref uiCulture, cultures, null, ci => ci.DisplayName, ref cultureSearchText, 8, UI.rarityButtonStyle, UI.Width(UI.ummWidth - 350))) {
                            // can we set it?
                        }
                    }
                },
                () => { }
            );
#endif
        }
    }
}
