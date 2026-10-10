// ============================================================
//  JupiterX | Managers/PluginManager.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using MelonLoader;
using JupiterX.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using JupiterX.Classes.Menu;
using JupiterX.Notifications;

namespace JupiterX.Managers
{
    public class PluginManager
    {
        public class Plugin
        {
            public string FileName;
            public bool Enabled;
            public string Name;
            public string Description;
            public Assembly Assembly;
            public List<MelonMod> Instances = new List<MelonMod>();
        }
        public interface IMenuExtension
        {
            ButtonInfo[] GetButtons();
        }
        public static readonly List<Plugin> Plugins = new List<Plugin>();
        private static string PluginsPath => Path.Combine(Application.persistentDataPath, "JupiterX/Plugins");
        public static void ReloadPlugins()
        {
            NotificationManager.SendNotification("Reloading plugins...", 500);
            Utility.SaveSettings();
            try
            {
                LoadPlugins();
                Utility.Log("Reloaded plugins.", JupxLogType.Info);
            }
            catch (Exception e)
            {
                Utility.Log("Reload failed: " + e, JupxLogType.Error);
            }
            Utility.LoadPreferences();
            Buttons.CurrentCategoryName = "Main";
        }
        public static void LoadPlugins()
        {
            if (!Directory.Exists(PluginsPath))
                Directory.CreateDirectory(PluginsPath);
            foreach (var p in Plugins)
            {
                foreach (var mod in p.Instances)
                {
                    try {  mod.GetType().GetMethod("OnUnload")?.Invoke(mod, null); } catch { }
                }
                p.Instances.Clear();
            }
            Plugins.Clear();
            int category = Buttons.GetCategory("Plugin Settings");
            Buttons.buttons[category] = new ButtonInfo[]
            {
                new ButtonInfo
                {
                    buttonText = "Exit Plugin Settings",
                    method = () => Buttons.CurrentCategoryName = "Settings",
                    isTogglable = false,
                    toolTip = "Return to settings"
                }
            };
            string[] files = Directory.GetFiles(PluginsPath, "*.dll");
            foreach (string file in files)
            {
                try
                {
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(file));
                    var modTypes = assembly.GetTypes().Where(t => typeof(MelonMod).IsAssignableFrom(t) && !t.IsAbstract);
                    Plugin plugin = new Plugin()
                    {
                        FileName = Path.GetFileName(file),
                        Assembly = assembly,
                        Enabled = true
                    };
                    foreach (var type in modTypes)
                    {
                        MelonMod mod = (MelonMod)Activator.CreateInstance(type);

                        mod.OnInitializeMelon();
                        plugin.Instances.Add(mod);

                        var attr = type.Assembly.GetCustomAttribute<MelonInfoAttribute>();
                        plugin.Name = attr?.Name ?? type.Name;
                        plugin.Description = attr?.Author ?? "No description";
                        if (mod is IMenuExtension ext)
                        {
                            foreach (var btn in ext.GetButtons())
                                Buttons.AddButton(category, btn);
                        }
                    }
                    Plugins.Add(plugin);
                }
                catch (Exception e)
                {
                    Utility.Log($"Failed loading {file}: {e}", JupxLogType.Error);
                }
            }
            foreach (var plugin in Plugins)
            {
                Buttons.AddButton(category, new ButtonInfo
                {
                    buttonText = plugin.FileName,
                    overlapText = GetStatus(plugin),
                    method = () => TogglePlugin(plugin),
                    isTogglable = false,
                    toolTip = plugin.Description
                });
            }
            Buttons.AddButton(category, new ButtonInfo
            {
                buttonText = "Reload Plugins",
                method = ReloadPlugins,
                isTogglable = false,
                toolTip = "Reload all plugins"
            });
        }
        private static string GetStatus(Plugin p)
        {
            return (p.Enabled ? "<color=grey>[</color><color=cyan>ON</color><color=grey>]</color> " : "<color=grey>[</color><color=red>OFF</color><color=grey>]</color> ") + p.Name;
        }
        public static void TogglePlugin(Plugin plugin)
        {
            plugin.Enabled = !plugin.Enabled;
            foreach (var mod in plugin.Instances)
            {
                try
                {
                    if (plugin.Enabled)
                        mod.OnInitializeMelon();
                    else
                        mod.GetType().GetMethod("OnUnload")?.Invoke(mod, null);
                }
                catch (Exception e)
                {
                    Utility.Log("Toggle error: " + e, JupxLogType.Error);
                }
            }
            var btn = Buttons.GetIndex(plugin.FileName);
            if (btn != null)
                btn.overlapText = GetStatus(plugin);
        }
        public static void ExecuteUpdate()
        {
            foreach (var plugin in Plugins.Where(p => p.Enabled))
            {
                foreach (var mod in plugin.Instances)
                {
                    try { mod.OnUpdate(); } catch { }
                }
            }
        }
        public static void ExecuteOnGUI()
        {
            foreach (var plugin in Plugins.Where(p => p.Enabled))
            {
                foreach (var mod in plugin.Instances)
                {
                    try { mod.OnGUI(); } catch { }
                }
            }
        }
    }
}