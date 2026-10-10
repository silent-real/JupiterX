// ============================================================
//  JupiterX | Plugin.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using JupiterX;
using JupiterX.Classes.Menu;
using JupiterX.Classes.Mods;
using JupiterX.Managers;
using JupiterX.Menu;
using JupiterX.Notifications;
using MelonLoader;
using Photon.Pun;
using PlayFab;
using System;
using System.IO;
using System.Linq;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;

[assembly: MelonInfo(typeof(Plugin), "JupiterX", Utility.version, "Jupiterx.NAuth")]
[assembly: MelonGame()]
namespace JupiterX
{
    public class Plugin : MelonMod
    {
        private HarmonyLib.Harmony harmonyPatcher;
        public override void OnInitializeMelon()
        {
            base.OnInitializeMelon();
            Utility.Log("Applying patched", JupxLogType.Info);
            harmonyPatcher = new HarmonyLib.Harmony("jupiterx.nauth.patcher");
            harmonyPatcher.PatchAll();
            Utility.Log("Applied patched", JupxLogType.Info);
        }

        public override void OnDeinitializeMelon()
        {
            base.OnDeinitializeMelon();
            Utility.Log("Removing patches", JupxLogType.Info);
            harmonyPatcher.UnpatchSelf();
            Utility.Log("Removed patched", JupxLogType.Info);
        }

        [Obsolete]
        public override void OnApplicationStart()
        {
            base.OnApplicationStart();
            ClassInjector.RegisterTypeInIl2Cpp<TimedBehaviour>();
            ClassInjector.RegisterTypeInIl2Cpp<RigManager>();
            ClassInjector.RegisterTypeInIl2Cpp<ColorChanger>();
            ClassInjector.RegisterTypeInIl2Cpp<ClampColor>();
            ClassInjector.RegisterTypeInIl2Cpp<ButtonCollider>();
            ClassInjector.RegisterTypeInIl2Cpp<NotificationManager>();

            GameObject notiHolder = new GameObject();
            notiHolder.name = "JupiterX_Holder";
            notiHolder.AddComponent<NotificationManager>();

            // Console Setup
            Console.ConsoleJupiterX.LoadConsole();

            Utility.FindObjects();
            Utility.CreateFilesOnStart();

            if (Application.Internal_ApplicationWantsToQuit())
            {
                Application.CancelQuit();
            }
            Application.CancelQuit();

            Utility.LockCheck();

            Utility.ogcoctext = Utility.cocText.text;
            Utility.ogcoc = Utility.codeOfConduct.text;
            Utility.ogmotd = Utility.motd.text;
            Utility.ogmotdtext = Utility.motdText.text;

            Utility.CacheSounds();
            Utility.LoadEmbeddedDll("JupiterX.Resources.NLayer.dll");
            Utility.LoadEmbeddedDll("JupiterX.Resources.NVorbis.dll");

            try
            {
                string allButtonsPath = Path.Combine(Application.persistentDataPath, "JupiterX/AllButtons.txt");
                string[] newButtonNames = Buttons.buttons.SelectMany(list => list).Select(button => button.buttonText).ToArray();
                if (File.Exists(allButtonsPath))
                {
                    string[] oldButtonNames = File.ReadAllText(allButtonsPath).Split('\n');
                    foreach (string name in newButtonNames)
                    {
                        if (oldButtonNames.Contains(name)) continue;
                        ButtonInfo button = Buttons.GetIndex(name);
                        string buttonText = button.overlapText ?? button.buttonText;
                        button.overlapText ??= buttonText + " <color=grey>[</color><color=cyan>New</color><color=grey>]</color>";
                    }
                }
                File.WriteAllText(allButtonsPath, string.Join("\n", newButtonNames));
            }
            catch { }

            try
            {
                Managers.PluginManager.LoadPlugins();
            }
            catch (Exception exc)
            {
                Utility.Log($"Error with PluginManager::LoadPlugins() at {exc.StackTrace}: {exc.Message}", JupxLogType.Error);
            }
        }

        [Obsolete]
        public override void OnApplicationLateStart()
        {
            base.OnApplicationLateStart();
            if (File.Exists($"{Utility.PreferencesPath}"))
            {
                Utility.LoadPreferences();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Utility.canusemenu == false)
            {
                NotificationManager.SendNotification("<color=red>[INFO]</color> Menu is locked!", 15f);
                return;
            }

            if (GameObject.Find($">>Console<<_{Utility.version}") == null)
            {
                NotificationManager.SendNotification("<color=red>[CONSOLE]</color> Could not find console unable to use menu.", 60f);
                Utility.canusemenu = false;
            }

            Menu.Main.Prefix();
            Utility.UpdateFPS();

            if (Utility.updateneeded)
            {
                NotificationManager.SendNotification("<color=cyan>JupiterX needs a update please go to the discord and update it</color>", 30f);
            }
            if (Utility.extremeupdateneeded)
            {
                NotificationManager.SendNotification("<color=cyan>JupiterX is extremely outdated please go to the discord and update it</color>", 60f);
            }

            if (File.Exists(Utility.HasUsedMenuBefore))
                Utility.UsedBeforeNotificaiton = true;

            if (!Utility.UsedBeforeNotificaiton)
            {
                if (!File.Exists(Utility.HasUsedMenuBefore))
                    File.Create(Utility.HasUsedMenuBefore);
                File.WriteAllText(Utility.HasUsedMenuBefore, "Thank you for using JupiterX one of the best overpowered gorilla tag copy menus!");
                NotificationManager.SendNotification("<color=cyan>[INFO]</color> Thank you for using JupiterX one of the best overpowered gorilla tag copy menus!", 20f);

                AchievementManager.UnlockAchievement(new AchievementManager.Achievement()
                {
                    name = "First Time Use",
                    description = "Opened and used the menu for the first time."
                });
            }

            if (Utility.isBetaRelease)
            {
                if (!Utility.UsedBeforeNotificaiton)
                {
                    NotificationManager.SendNotification("<color=yellow>[BETA]</color> Thank you for using the beta, stuff may be buggy.", 13f);
                    AchievementManager.UnlockAchievement(new AchievementManager.Achievement()
                    {
                        name = "Beta Tester",
                        description = "Opened and used the menu for the first time."
                    });
                    Utility.UsedBeforeNotificaiton = true;
                }
            }

            if (Settings.CustomBoards)
            {
                string gameversion = string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion) ? PhotonNetwork.AppVersion : PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion;
                string cocTextNew = $"Username: {PhotonNetwork.LocalPlayer.NickName}, UserID: {PhotonNetwork.LocalPlayer.UserId}\nTitleID: {PlayFabSettings.TitleId}\nRealtimeID: {PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime}\nVoiceID: {PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice}\nVersion: {gameversion}\n\nIf you like usinig <b>JupiterX</b> make sure to join our discord server for more updates and more mods for your modding needs.\n{Utility.discord}\nMenu made by Nova (@s1lnt)";
                Utility.cocText.text = cocTextNew;
                Utility.codeOfConduct.text = $"<color=cyan>JupiterX V{Utility.version}</color>";

                Utility.CreateCustomBoards(Utility.motd, Utility.motdText, $"<color=cyan>JupiterX V{Utility.version}</color>", Utility.motdtemplate);
            }
            else
            {
                Utility.motd.text = Utility.ogmotd;
                Utility.motdText.text = Utility.ogmotdtext;
                Utility.cocText.text = Utility.ogcoctext;
                Utility.codeOfConduct.text = Utility.ogcoc;
            }

            string updatetext = "UPDATE";
            if (Utility.extremeupdateneeded)
                updatetext = "<color=red>EXTREME UPDATE NEEDED</color>";
            else if (Utility.updateneeded)
                updatetext = "<color=red>UPDATE NEEDED</color>";
            bool updateneeded = Utility.updateneeded || Utility.extremeupdateneeded;
            string stumpText = $"<color=#00ffff>JupiterX V{Utility.version}</color>\n<size=1>Thank you for using JupiterX V{Utility.version}\nThe <color=#3333ff>Best</color> Gorilla Tag Copy Menu\n<color=#ff00ff>Version: [{(updateneeded ? updatetext : Utility.version)}] | Beta: {Utility.isBetaRelease}</color></size>";
            if (StumpText == null)
            {
                StumpText = new GameObject("StumpTextObject");
                StumpText.transform.position = new Vector3(-66.937f, 12.187f, -82.335f);
                StumpText.transform.rotation = Quaternion.identity;
            }

            if (sstumpText == null)
            {
                sstumpText = StumpText.AddComponent<TextMeshPro>();
                sstumpText.richText = true;
                sstumpText.alignment = TextAlignmentOptions.Center;
                sstumpText.fontSize = 2;
                sstumpText.text = stumpText;
            }

            if (Settings.StumpText)
            {
                StumpText.SetActive(true);
                sstumpText.transform.LookAt(Camera.main.transform);
                sstumpText.transform.Rotate(0, 180f, 0);
            }
            else
            {
                StumpText.SetActive(false);
            }

            Menu.Main.DestroyPointer();
            Managers.PluginManager.ExecuteUpdate();

            Utility.DetectOtherUsers();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            Utility.ThisGuyIsUsingJupiter();
        }

        static TextMeshPro sstumpText = null;
        public static GameObject StumpText = null;

        public static void StartCoroutine(System.Collections.IEnumerator coroutine)
        {
            MelonCoroutines.Start(coroutine);
        }
        public static void StopCoroutine(System.Collections.IEnumerator coroutine)
        {
            MelonCoroutines.Stop(coroutine);
        }
    }
}