// ============================================================
//  JupiterX | Mods/Presets.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using JupiterX.Menu;
using JupiterX.Notifications;
using System.IO;

namespace JupiterX.Mods
{
    public static class Presets
    {
        public static void SaveCustomPreset(int id)
        {
            if (!Directory.Exists($"{Utility.MainPath}/SavedPresets"))
                Directory.CreateDirectory($"{Utility.MainPath}/SavedPresets");

            File.WriteAllText($"{Utility.MainPath}/SavedPresets/Preset_" + id + ".txt", Utility.SaveSettings());
        }

        public static void LoadCustomPreset(int id)
        {
            if (Directory.Exists($"{Utility.MainPath}/SavedPresets"))
            {
                string text = File.ReadAllText($"{Utility.MainPath}/SavedPresets/Preset_" + id + ".txt");
                Utility.LoadSettings(text);
            }
        }


        public static void NovaPreset()
        {
            string[] presetMods =
            {
                "Freeze Player In Menu",
                "Menu Trail",
                "See Others Menus",
                "Menu Outline",
                "Custom Boards",
                "Version Text",
                "Stump Text",
                "FPS Text",
                "Anti AFK",
                "Turning",
                "Excel Fly",
                "Long Arms",
                "No Tag Freeze",
                "Name Tags",
                "FPS Overlay",
                "Ping Overlay"
            };

            Utility.PageType = -1;
            Utility.currentTheme = 8;
            Utility.MainDropType = -1;
            Movement.FlySpeedAmount = 2;
            Movement.ArmSizeAmount = -1;
            Utility.currentFontStyleChoice = 0;

            Utility.ChangePageType();
            Utility.ChangeMenuTheme();
            Utility.ChangeDropType();
            Movement.ChangeFlySpeed();
            Movement.ChangeArmLength();
            Utility.ChangeFontStyle();

            Utility.Panic();

            foreach (string mod in presetMods)
                Main.Toggle(mod);

            NotificationManager.SendNotification("<color=grey>[</color><color=purple>PRESET</color><color=grey>]</color> Nova preset enabled successfully.");
        }
    }
}