// ============================================================
//  JupiterX | Utility.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using Console;
using ExitGames.Client.Photon;
using GorillaNetworking;
using Il2CppSystem.Net;
using JupiterX.Classes.Menu;
using JupiterX.Classes.Mods;
using JupiterX.Menu;
using JupiterX.Mods;
using JupiterX.Notifications;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static JupiterX.Menu.Main;
using static JupiterX.Settings;

namespace JupiterX
{
    public enum JupxLogType
    {
        Info, Warning, Error
    }

    public class Utility
    {
        public static void Log(string msg, JupxLogType type)
        {
            string prefix = "[*}";
            switch (type)
            {
                case JupxLogType.Info: prefix = "[INFO]"; break;
                case JupxLogType.Warning: prefix = "[WARNING]"; break;
                case JupxLogType.Error: prefix = "[ERROR]"; break;
            }
            string full = $"{prefix} > {msg}";
            MelonLoader.MelonLogger.Msg(full);
        }

        public static void StopCurrentPrompt() =>
            prompts.RemoveAt(0);

        public static void MoveStumpTextGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.Pointer;

                if (GetGunInput(true))
                {
                    Plugin.StumpText.transform.position = NewPointer.transform.position + new Vector3(0, 0.7f, 0);
                }
            }
        }

        public static void PingOverlay()
        {
            NotificationManager.information["Ping"] = PhotonNetwork.GetPing() + "ms";
        }

        public static void NearbyTaggerOverlay()
        {
            float closest = float.MaxValue;
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null && vrrig != myVRRig() && vrrig.IsTagged())
                {
                    float dist = Vector3.Distance(GorillaTagger.Instance.headCollider.transform.position, vrrig.headMesh.transform.position);
                    if (dist < closest)
                        closest = dist;
                }
            }
            if (!Mathf.Approximately(closest, float.MaxValue))
                NotificationManager.information["Nearby"] = $"{closest:F1}m";
            else
                NotificationManager.information.Remove("Nearby");
        }

        private static Color HexToColor(string hex)
        {
            hex = hex.Replace("#", "");
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = 255;
            if (hex.Length == 8)
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, a);
        }

        static Hashtable jupiterxProp = new Hashtable();
        public static void ThisGuyIsUsingJupiter()
        {
            if (!jupiterxProp.ContainsKey("jupiterx2026revive"))
                jupiterxProp.Add("jupiterx2026revive", "jupiterx2026revive");
            if (PhotonNetwork.InRoom)
            {
                if (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("jupiterx2026revive"))
                    PhotonNetwork.LocalPlayer.SetCustomProperties(jupiterxProp);
                if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("jupiterx2026revive"))
                {
                    string name = "[JUPITERX] " + PhotonNetwork.LocalPlayer.nickName;
                    myVRRig().playerText.text = name;
                    myVRRig().playerText.color = Color.cyan;
                    if (offlineVRRig().playerText.text != myVRRig().playerText.text)
                    {
                        offlineVRRig().playerText.text = myVRRig().playerText.text;
                        offlineVRRig().playerText.color = myVRRig().playerText.color;
                    }
                }
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != myVRRig())
                    {
                        string nickname = rig.photonView.Owner.NickName;
                        if (rig.photonView.Owner.CustomProperties.ContainsKey("jupiterx2026revive"))
                        {
                            rig.playerText.text = "[JUPITERX] " + nickname;
                            rig.playerText.color = Color.cyan;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("jupiterxusersosigma"))
                        {
                            rig.playerText.text = "[JUPITERX OLD] " + nickname;
                            rig.playerText.color = Color.yellow;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("solaaaaaaaaaaaa"))
                        {
                            rig.playerText.text = "[SOLAR] " + nickname;
                            rig.playerText.color = Color.grey;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("solarnovapleasestopdoingdumbshityoudotsallthetimrimgettingpissed"))
                        {
                            rig.playerText.text = "[SOLAR - OLD] " + nickname;
                            rig.playerText.color = Color.grey;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("zyph"))
                        {
                            rig.playerText.text = "[ZYPH] " + nickname;
                            rig.playerText.color = HexToColor("#6600CC");
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("bunny"))
                        {
                            rig.playerText.text = "[BUNNY.LOL] " + nickname;
                            rig.playerText.color = HexToColor("#ED7014");
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("titled"))
                        {
                            rig.playerText.text = "[TITLED] " + nickname;
                            rig.playerText.color = HexToColor("#333333");
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("genesis"))
                        {
                            rig.playerText.text = "[GENESIS] " + nickname;
                            rig.playerText.color = Color.grey;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("terrormenussohot"))
                        {
                            rig.playerText.text = "[TERROR] " + nickname;
                            rig.playerText.color = Color.red;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("qolossal"))
                        {
                            rig.playerText.text = "[QCM] " + nickname;
                            rig.playerText.color = Color.magenta;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("stupid"))
                        {
                            rig.playerText.text = "[STUPID] " + nickname;
                            rig.playerText.color = HexToColor("#ffa200");
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("toomanyplayers"))
                        {
                            rig.playerText.text = "[TOOMANYPLAYERS] " + nickname;
                            rig.playerText.color = Color.red;
                        }
                        else if (rig.photonView.Owner.CustomProperties.ContainsKey("console"))
                        {
                            rig.playerText.text = "[CONSOLE] " + nickname;
                            rig.playerText.color = Color.grey;
                        }
                    }
                }
            }
        }

        public static void DestroyAllPhotonViews()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                SetMaster(MyPlayer());
                PhotonView objectView = obj.GetComponent<PhotonView>();
                if (objectView != null)
                    PhotonNetwork.Destroy(objectView);
            }
        }

        public static Photon.Realtime.Player MyPlayer() =>
            PhotonNetwork.LocalPlayer;

        public static void BanAll()
        {
            Plugin.StartCoroutine(BetaBanAllWithDelay());
            if (RightTrigger)
                Toggle("Ban All");
        }

        static System.Collections.IEnumerator BetaBanAllWithDelay()
        {
            yield return new WaitForSeconds(2);
            foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
            {
                BetaBanAll(plr.UserId);
            }
        }

        public static string CleanPlayerName(string input, int length = 12)
        {
            input = NoRichtextTags(input);
            if (input.Length > length)
                input = input[..(length - 1)];
            return input;
        }
        public static void BetaBanAll(string userid)
        {
            string url = "https://novax.lol/banusingcloudscript";
            var payload = new
            {
                titleId = PlayFabSettings.TitleId,
                playerId = userid
            };
            string json = JsonConvert.SerializeObject(payload);
            WebWrapper.Post(url, json, responseString =>
            {
                if (string.IsNullOrEmpty(responseString) || responseString.StartsWith("ERROR:", StringComparison.Ordinal))
                {
                    NotificationManager.SendNotification($"<color=red>[INFO]</color> Ban request failed: {responseString}", 6f);
                    return;
                }
                NotificationManager.SendNotification($"<color=cyan>[INFO]</color> Success {responseString}", 6f);
            });
        }
        public static void BetaTPToSling()
        {
            Slingshot slingshot = offlineVRRig().slingshot;
            if (slingshot != null)
            {
                SlingshotProjectile slingproj = slingshot.projectilePrefab.GetComponent<SlingshotProjectile>();
                if (slingproj != null)
                {
                    GorillaLocomotion.Player.Instance.transform.position = slingproj.transform.position;
                }
            }
        }
        public static string FindVRRigFromPlayerId(VRRig who)
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != myVRRig())
                    {
                        return rig.photonView.Owner.UserId;
                    }
                }
            }
            return null;
        }

        public static void BetaCrashAllV2(VRRig target)
        {
            if (PhotonNetwork.InRoom)
            {
                MakeMeMaster();
                if (target != null && target != myVRRig())
                {
                    PhotonNetwork.Destroy(target.photonView);
                    PhotonNetwork.DestroyPlayerObjects(target.photonView.Owner);
                    PhotonNetwork.DestroyPlayerObjects(target.photonView.Controller);
                    PhotonNetwork.SendDestroyOfPlayer(target.photonView.Owner.ActorNumber);
                    PhotonNetwork.SendDestroyOfPlayer(target.photonView.Controller.ActorNumber);
                }
            }
        }

        private static bool lastfreezegarbadge;
        private static float Meorw = 0f;
        public static void PacketStresser()
        {
            if (Time.time > Meorw)
            {
                lastfreezegarbadge = !lastfreezegarbadge;
                foreach (GorillaPlayerScoreboardLine line in GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>())
                {
                    line.PressButton(lastfreezegarbadge, GorillaPlayerLineButton.ButtonType.Mute);
                }
                Meorw = Time.time + 0.1f;
            }
        }

        private static readonly System.Random _random = new System.Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public static string Generate(int length)
        {
            char[] buffer = new char[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = _chars[_random.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        public static void FixGhostRig()
        {
            if (PhotonNetwork.InRoom)
            {
                if (myVRRig().enabled == false)
                    myVRRig().enabled = true;
            }
            else
            {
                if (offlineVRRig().enabled == false)
                    offlineVRRig().enabled = true;
            }
            GhostView(false);
        }

        public static int currentFontStyleChoice = 0;
        private static string[] fontstylestring = new string[] { "Default", "Bold", "Italic", "Bold & Italic" };
        public static FontStyle currentFontStyle = FontStyle.Normal;
        public static void ChangeFontStyle(bool increment = true)
        {
            if (increment)
                currentFontStyleChoice = (currentFontStyleChoice + 1) % fontstylestring.Length;
            else
                currentFontStyleChoice = (currentFontStyleChoice - 1 + fontstylestring.Length) % fontstylestring.Length;
            switch (currentFontStyleChoice)
            {
                case 0:
                    currentFontStyle = FontStyle.Normal;
                    break;
                case 1:
                    currentFontStyle = FontStyle.Bold;
                    break;
                case 2:
                    currentFontStyle = FontStyle.Italic;
                    break;
                case 3:
                    currentFontStyle = FontStyle.BoldAndItalic;
                    break;
            }
            Buttons.GetIndex("Change Font Style").overlapText = "Change Font Style <color=grey>[<color=cyan>" + fontstylestring[currentFontStyleChoice] + "</color>]</color>";
        }

        static Vector3 closePosition;
        public static void FreezePlayerInMenu()
        {
            if (menu != null)
            {
                if (closePosition == Vector3.zero)
                    closePosition = GorillaTagger.Instance.GetComponent<Rigidbody>().transform.position;
                else
                    GorillaTagger.Instance.GetComponent<Rigidbody>().transform.position = closePosition;
                GorillaTagger.Instance.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            }
            else
                closePosition = Vector3.zero;
        }
        public static void GhostInMenu()
        {
            if (PhotonNetwork.InRoom)
            {
                if (menu != null)
                    myVRRig().enabled = false;
                else
                    myVRRig().enabled = true;
            }
            else
            {
                if (menu != null)
                    offlineVRRig().enabled = false;
                else
                    offlineVRRig().enabled = true;
            }
        }
        public static void InvisInMeun()
        {
            if (PhotonNetwork.InRoom)
            {
                if (menu != null)
                {
                    myVRRig().enabled = false;
                    myVRRig().transform.position = new Vector3(4543f, 34532f, 453);
                }
                else
                    myVRRig().enabled = true;
            }
            else
            {
                if (menu != null)
                {
                    offlineVRRig().enabled = false;
                    offlineVRRig().transform.position = new Vector3(4543f, 34532f, 453);
                }
                else
                    offlineVRRig().enabled = true;
            }
        }

        public static bool hasTriggeredOnceL = false;
        public static bool hasTriggeredOnceR = false;
        public static string[] PageTypes = { "Side", "Bottom", "Triggers" };
        public static int PageType = 0;
        public static bool isTriggers = false;
        public static Vector3 PageObjectPosRight = new Vector3(0.56f, 0.65f, 0);
        public static Vector3 PageObjectPosLeft = new Vector3(0.56f, -0.65f, 0);
        public static Vector3 PageTextPosRight = new Vector3(0.064f, -0.195f, 0f);
        public static Vector3 PageTextPosLeft = new Vector3(0.064f, 0.195f, 0f);
        public static Vector3 PageObjScale = new Vector3(0.09f, 0.2f, 0.9f);
        public static void ChangePageType(bool increment = true)
        {
            if (increment)
            {
                PageType = (PageType + 1) % PageTypes.Length;
            }
            else
            {
                PageType = (PageType - 1 + PageTypes.Length) % PageTypes.Length;
            }
            switch (PageType)
            {
                case 0:
                    isTriggers = false;
                    PageObjectPosRight = new Vector3(0.56f, 0.65f, 0);
                    PageObjectPosLeft = new Vector3(0.56f, -0.65f, 0);
                    PageObjScale = new Vector3(0.09f, 0.2f, 0.9f);
                    PageTextPosLeft = new Vector3(0.064f, 0.195f, 0f);
                    PageTextPosRight = new Vector3(0.064f, -0.195f, 0f);
                    break;
                case 1:
                    isTriggers = false;
                    PageObjectPosRight = new Vector3(0.56f, 0.44f, -0.6f);
                    PageObjectPosLeft = new Vector3(0.56f, -0.44f, -0.6f);
                    PageTextPosLeft = new Vector3(0.062f, 0.132f, -0.23f);
                    PageTextPosRight = new Vector3(0.062f, -0.130f, -0.23f);
                    PageObjScale = new Vector3(0.1f, 0.2f, 0.1f);
                    break;
                case 2:
                    isTriggers = true;
                    PageObjectPosRight = new Vector3(0f, -0f, -0f);
                    PageObjectPosLeft = new Vector3(0f, 0f, -0f);
                    PageObjScale = new Vector3(0, 0, 0);
                    PageTextPosLeft = new Vector3(222222f, -22222222f, -222222222f);
                    PageTextPosRight = new Vector3(222222f, -22222222f, -222222222f);
                    break;
            }
            Buttons.GetIndex("Change Page Type").overlapText = "Change Page Type <color=grey>[<color=cyan>" + PageTypes[PageType] + "</color>]</color>";
        }

        public static int MainDropType = 0;
        private static int dropType = 0;
        private static string[] dropTypes = new string[] { "Destroy", "Drop", "No Gravity", "Throw" };
        public static void ChangeDropType(bool increment = true)
        {
            dropType = increment ? (dropType + 1) % dropTypes.Length : (dropType - 1 + dropTypes.Length) % dropTypes.Length;
            switch (dropType)
            {
                case 0:
                    MainDropType = 0;
                    break;
                case 1:
                    MainDropType = 1;
                    break;
                case 2:
                    MainDropType = 2;
                    break;
                case 3:
                    MainDropType = 3;
                    break;
            }
            Buttons.GetIndex("Change Drop Type").overlapText = $"Change Drop Type <color=grey>[<color=cyan>{dropTypes[dropType]}</color>]</color>";
        }

        private static string[] MenuThemes = new string[]
{
    "Default", "Blue", "Rainbow", "Red", "Transparent", "Pastel",
    "Rig Color", "Yellow", "Green", "Fading Grey", "Fading Red", "Fading Blue",
    "Fading Yellow", "Fading Magenta", "White", "Black",
    "Cyan", "Orange", "Purple", "Pink", "Lime", "Crimson",
    "Fading Cyan", "Fading Green", "Fading Orange", "Fading White",
    "Ocean", "Sunset",
    "Teal", "Indigo", "Gold", "Silver", "Maroon", "Navy",
    "Olive", "Turquoise", "Lavender", "Salmon", "Mint", "Coral",
    "Violet", "Amber", "Emerald", "Ruby", "Sapphire", "Rose",
    "Slate", "Bronze", "Hot Pink", "Neon Blue", "Neon Green",
    "Neon Pink", "Neon Purple",
    "Fading Purple", "Fading Pink", "Fading Lime", "Fading Teal",
    "Fading Gold", "Fading Black", "Fading Crimson", "Fading Indigo",
    "Forest", "Blood Moon", "Midnight", "Aurora", "Fire", "Ice",
    "Grape", "Peach", "Toxic", "Galaxy", "Inferno", "Frost",
    "Jade", "Molten"
};
        public static int currentTheme = 0;
        public static void ChangeMenuTheme(bool increment = true)
        {
            currentTheme = increment ? (currentTheme + 1) % MenuThemes.Length : (currentTheme - 1 + MenuThemes.Length) % MenuThemes.Length;
            textColors[0] = Color.white;
            textColors[1] = Color.white;
            switch (currentTheme)
            {
                case 0:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    break;
                case 1:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.blue) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    break;
                case 2:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black), rainbow = true };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    break;
                case 3:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 4:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black), transparent = true };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 5:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black), pastelRainbow = true };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 6:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black), copyRigColor = true };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 7:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.yellow) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 8:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.green) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 9:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.gray) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    break;
                case 10:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.red) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 11:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.blue) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 12:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.yellow) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 13:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.magenta) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 14:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.white) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.white) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.white) };
                    textColors[0] = Color.black;
                    textColors[1] = Color.red;
                    break;
                case 15:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    textColors[0] = Color.white;
                    textColors[1] = Color.red;
                    break;
                case 16:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.cyan) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 17:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(1f, 0.55f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 18:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.6f, 0.2f, 0.8f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 19:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(1f, 0.4f, 0.7f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 20:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.6f, 1f, 0.2f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 21:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.6f, 0f, 0.1f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 22:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.cyan) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 23:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.green) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 24:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(1f, 0.55f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 25:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.white) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 26:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.blue, Color.cyan) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 27:
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.red, Color.yellow) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 28: // Teal
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0f, 0.5f, 0.5f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 29: // Indigo
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.29f, 0f, 0.51f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 30: // Gold
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(1f, 0.84f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 31: // Silver
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.75f, 0.75f, 0.75f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    textColors[0] = Color.black;
                    textColors[1] = Color.red;
                    break;
                case 32: // Maroon
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.5f, 0f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 33: // Navy
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0f, 0f, 0.5f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 34: // Olive
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.5f, 0.5f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 35: // Turquoise
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.25f, 0.88f, 0.82f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 36: // Lavender
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.8f, 0.7f, 1f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 37: // Salmon
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.98f, 0.5f, 0.45f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 38: // Mint
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.6f, 1f, 0.8f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    textColors[0] = Color.black;
                    textColors[1] = Color.red;
                    break;
                case 39: // Coral
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(1f, 0.5f, 0.31f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 40: // Violet
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.56f, 0f, 1f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 41: // Amber
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(1f, 0.75f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 42: // Emerald
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.31f, 0.78f, 0.47f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 43: // Ruby
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.88f, 0.07f, 0.37f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 44: // Sapphire
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.06f, 0.32f, 0.73f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 45: // Rose
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(1f, 0.0f, 0.5f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 46: // Slate
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.44f, 0.5f, 0.56f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    break;
                case 47: // Bronze
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.8f, 0.5f, 0.2f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 48: // Hot Pink
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(1f, 0.08f, 0.58f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 49: // Neon Blue
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.1f, 0.6f, 1f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 50: // Neon Green
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.22f, 1f, 0.08f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 51: // Neon Pink
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(1f, 0.11f, 0.81f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 52: // Neon Purple
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(new Color(0.75f, 0.2f, 1f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 53: // Fading Purple
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(0.6f, 0.2f, 0.8f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 54: // Fading Pink
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(1f, 0.4f, 0.7f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 55: // Fading Lime
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(0.6f, 1f, 0.2f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 56: // Fading Teal
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(0f, 0.5f, 0.5f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 57: // Fading Gold
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(1f, 0.84f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 58: // Fading Black
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(0.15f, 0.15f, 0.15f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    break;
                case 59: // Fading Crimson
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(0.86f, 0.08f, 0.24f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 60: // Fading Indigo
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(0.29f, 0f, 0.51f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 61: // Forest
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(0.05f, 0.2f, 0.05f), new Color(0.13f, 0.55f, 0.13f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 62: // Blood Moon
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(0.1f, 0f, 0f), new Color(0.7f, 0f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.red) };
                    break;
                case 63: // Midnight
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(0f, 0f, 0.15f), new Color(0f, 0f, 0.5f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 64: // Aurora
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(0.13f, 0.55f, 0.13f), Color.cyan) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 65: // Fire
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.red, new Color(1f, 0.55f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 66: // Ice
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.cyan, Color.white) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 67: // Grape
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(0.4f, 0f, 0.6f), Color.magenta) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 68: // Peach
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(1f, 0.55f, 0f), new Color(1f, 0.4f, 0.7f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 69: // Toxic
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(0.6f, 1f, 0.2f), Color.green) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 70: // Galaxy
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(0.29f, 0f, 0.51f), Color.blue) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 71: // Inferno
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(new Color(0.86f, 0.08f, 0.24f), Color.yellow) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 72: // Frost
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.blue, Color.white) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 73: // Jade
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.green, Color.cyan) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
                case 74: // Molten
                    backgroundColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, new Color(1f, 0.3f, 0f)) };
                    buttonColors[0] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) };
                    buttonColors[1] = new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.grey) };
                    break;
            }
            Buttons.GetIndex("Change Menu Theme").overlapText = $"Change Menu Theme <color=grey>[<color=cyan>{MenuThemes[currentTheme]}</color>]</color>";
        }

        public static void BetaEmojiName(int emoji) =>
            MyPlayer().NickName = "\n\n<size=4532><sprite=" + emoji + "></size>";
        public static void BetaSpawnPrefab(string prefabName, Vector3 Position, Quaternion Roation) =>
            PhotonNetwork.Instantiate(prefabName, Position, Roation, 0, null);
        public static void SetMaster(Photon.Realtime.Player newMaster) =>
            PhotonNetwork.SetMasterClient(newMaster);
        public static void MakeMeMaster() =>
            SetMaster(MyPlayer());

        static GameObject sphereeR = null;
        static GameObject sphereeL = null;
        public static void GhostView(bool enabled)
        {
            if (disableGhostview)
                return;
            if (enabled)
            {
                if (legacyGhostview)
                {
                    if (sphereeL == null)
                    {
                        sphereeL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphereeL.GetComponent<Renderer>().material.shader = GUIShader();
                        sphereeL.transform.SetParent(LeftHandTransform(), false);
                        sphereeL.transform.localRotation = Quaternion.identity;
                        sphereeL.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        sphereeL.GetComponent<Renderer>().material.color = Color.grey;
                        GameObject.Destroy(sphereeL.GetComponent<Collider>());
                    }
                    if (sphereeR == null)
                    {
                        sphereeR = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        sphereeR.GetComponent<Renderer>().material.shader = GUIShader();
                        sphereeR.transform.SetParent(RightHandTransform(), false);
                        sphereeR.transform.localRotation = Quaternion.identity;
                        sphereeR.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                        sphereeR.GetComponent<Renderer>().material.color = Color.grey;
                        GameObject.Destroy(sphereeR.GetComponent<Collider>());
                    }
                }
                else
                {
                    if (!offlineVRRig().mainSkin.enabled)
                        offlineVRRig().mainSkin.enabled = true;
                    Color color = backgroundColor.GetCurrentColor();
                    offlineVRRig().mainSkin.material.color = new Color(color.r, color.g, color.b, 0.4f);
                }
            }
            else
            {
                if (legacyGhostview)
                {
                    if (sphereeL != null)
                    {
                        GameObject.Destroy(sphereeL);
                        sphereeL = null;
                    }
                    if (sphereeR != null)
                    {
                        GameObject.Destroy(sphereeR);
                        sphereeR = null;
                    }
                }
                else
                {
                    if (PhotonNetwork.InRoom)
                    {
                        if (offlineVRRig().mainSkin.enabled)
                            offlineVRRig().mainSkin.enabled = false;
                    }
                }
            }
        }

        public static bool IsMaster() =>
            MyPlayer().IsMasterClient;

        public static void BetaDestroyPlayers(Photon.Realtime.Player who)
        {
            MakeMeMaster();
            PhotonNetwork.DestroyPlayerObjects(who);
            PhotonNetwork.DestroyPlayerObjects(who);
            PhotonNetwork.SendDestroyOfPlayer(who.ActorNumber);
        }

        static List<GameObject> Prefabs = new List<GameObject>();
        public static void BetaDoPrefab(string prefabName)
        {
            GameObject prefab = PhotonNetwork.Instantiate(prefabName, Vector3.zero, Quaternion.identity);
            Prefabs.Add(prefab);
            foreach (GameObject gameObject in Prefabs)
            {
                if (gameObject != null)
                    GameObject.Destroy(gameObject);
            }
        }

        static string[] RPCNames = { "SetTaggedTime", "UpdatePlayerCosmetic", "RequestCosmetics", "ReportTagRPC" };
        static string[] prefabNames = { "gorillaprefabs/gorillaenemy", "Network Player", "STICKABLE TARGET", "bulletPrefab" };

        public static void SlowPlayer(Photon.Realtime.Player who)
        {
            MakeMeMaster();
            myVRRig().photonView.RPC("SetTaggedTime", who, null);
        }

        public static void TagPlayer(Photon.Realtime.Player who)
        {
            MakeMeMaster();
            GorillaGameManager.instance.GetComponent<PhotonView>().RPC("ReportTagRPC", RpcTarget.MasterClient, new Il2CppSystem.Object[] { who });
        }

        public static void InstaCrashPlayer(Photon.Realtime.Player who)
        {
            for (int i = 0; i < 150; i++)
            {
                PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { TargetActors = new int[] { who.ActorNumber } }, SendOptions.SendUnreliable);
                PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { TargetActors = new int[] { who.ActorNumber } }, SendOptions.SendUnreliable);
            }
            PhotonNetwork.SendAllOutgoingCommands();
        }

        public static void CrashPlayerForPlayerTab(Photon.Realtime.Player plr)
        {
            MakeMeMaster();
            PhotonNetwork.DestroyPlayerObjects(plr);
            PhotonNetwork.SendDestroyOfPlayer(plr.ActorNumber);
            BetaDestroyPlayers(plr);
        }

        public static void BetaCrashPlayer(Photon.Realtime.Player crash)
        {
            MakeMeMaster();
            BetaDestroyPlayers(crash);
            BetaDestroyPlayers(crash);
            BetaDestroyPlayers(crash);
            myVRRig().photonView.RPC(RPCNames[0], crash, null);
            myVRRig().photonView.RPC(RPCNames[0], crash, null);
            myVRRig().photonView.RPC(RPCNames[1], crash, null);
            myVRRig().photonView.RPC(RPCNames[1], crash, null);
            myVRRig().photonView.RPC(RPCNames[2], crash, null);
            myVRRig().photonView.RPC(RPCNames[2], crash, null);
            myVRRig().photonView.RPC(RPCNames[3], crash, null);
            myVRRig().photonView.RPC(RPCNames[3], crash, null);
            myVRRig().photonView.RPC(RPCNames[4], crash, null);
            myVRRig().photonView.RPC(RPCNames[4], crash, null);
            BetaDestroyPlayers(crash);
            BetaDoPrefab(prefabNames[0]);
            BetaDoPrefab(prefabNames[0]);
            BetaDoPrefab(prefabNames[1]);
            BetaDoPrefab(prefabNames[2]);
            BetaDoPrefab(prefabNames[3]);
        }

        public static void ChangeName(string name)
        {
            MyPlayer().NickName = name;
            GorillaComputer.instance.currentName = name;
            PlayerPrefs.SetString("playerName", name);
            PlayerPrefs.Save();
        }

        public static void BetaSetIndex(int matIndex, VRRig who)
        {
            if (PhotonNetwork.InRoom)
            {
                MakeMeMaster();
                VRRig[] rigs = GorillaParent.instance.vrrigs.ToArray();
                GorillaTagManager[] tagman = GameObject.FindObjectsOfType<GorillaTagManager>();
                for (int i = 0; i < rigs.Length; i++)
                {
                    if (rigs[i] != null && !rigs[i].photonView.IsMine && !rigs[i].isMyPlayer)
                    {
                        foreach (var tag in tagman)
                        {
                            tag.SetisCurrentlyTag(true);
                            bool isTagged = who.mainSkin.material.name.Contains("fected");
                            tag.MyMatIndex(who.photonView.Owner);
                            who.setMatIndex = isTagged ? matIndex : matIndex + 1;
                            tag.EndInfectionGame();
                            tag.UpdateTagState();
                        }
                    }
                }
            }
        }

        public static void FlushRPCS()
        {
            GorillaNot.instance.rpcCallLimit = int.MaxValue;
            GorillaNot.instance.OnPlayerLeftRoom(MyPlayer());
            PhotonNetwork.OpCleanRpcBuffer(myVRRig().photonView);
            PhotonNetwork.SendAllOutgoingCommands();
        }

        public static Shader StandardShader()
        {
            if (Shader.Find("GorillaTag/UberShader") == null)
                return Shader.Find("Standard");
            else
                return Shader.Find("GorillaTag/UberShader");
        }

        public static Shader UnlitShader() =>
            Shader.Find("Unlit/Color");

        public static Shader GUIShader() =>
            Shader.Find("GUI/Text Shader");

        public static Vector3 ThrowMenu(easyInputs.EasyHand hand) =>
            easyInputs.EasyInputs.GetDeviceVelocity(hand);

        public static void GetTagFreeze(bool enabled) =>
            GorillaLocomotion.Player.Instance.disableMovement = !enabled;

        public static void TeleportPlayer(Vector3 pos) =>
            MainTransform().transform.position = pos;

        public static Transform MainTransform() =>
            GorillaTagger.Instance.transform;

        public static Transform RightHandTransform() =>
            GorillaTagger.Instance.rightHandTransform;

        public static Transform LeftHandTransform() =>
            GorillaTagger.Instance.leftHandTransform;

        public static Transform Head() =>
            GorillaTagger.Instance.headCollider.transform;

        public static Transform BodyTransform() =>
            GorillaTagger.Instance.bodyCollider.transform;

        public static Rigidbody RigidbodyTransform() =>
            GorillaTagger.Instance.bodyCollider.attachedRigidbody;

        public static void UnlockAll()
        {
            foreach (CosmeticItem item in GetAllCosmetics())
            {
                UnlockItem(item.displayName);
                UpdateWardrobeModelsAndButtons();
            }
        }

        public struct CosmeticItem
        {
            public string itemName;
            public string itemSlot;
            public Sprite itemPicture;
            public string displayName;
            public int cost;
            public string[] bundledItems;
            public bool canTryOn;
        }

        private static List<CosmeticItem> allCosmetics = new();
        private static object GetCosmeticsControllerInstance()
        {
            Type controllerType = Type.GetType("CosmeticsController, Assembly-CSharp") ?? Type.GetType("GorillaNetworking.CosmeticsController, Assembly-CSharp");
            if (controllerType == null)
            {
                NotificationManager.SendNotification("Can't find CosmeticsController type");
                return null;
            }
            FieldInfo instanceField = controllerType.GetField("instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (instanceField != null)
            {
                object instance = instanceField.GetValue(null);
                if (instance != null)
                    return instance;
            }
            PropertyInfo instanceProperty = controllerType.GetProperty("instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (instanceProperty != null)
            {
                object instance = instanceProperty.GetValue(null);
                if (instance != null)
                    return instance;
                NotificationManager.SendNotification("instance property returned null");
                return null;
            }
            NotificationManager.SendNotification("Can't find controller instance");
            return null;
        }

        public static List<CosmeticItem> GetAllCosmetics()
        {
            Type controllerType = Type.GetType("CosmeticsController, Assembly-CSharp") ?? Type.GetType("GorillaNetworking.CosmeticsController, Assembly-CSharp");
            if (controllerType == null)
                return new List<CosmeticItem>();
            object controllerInstance = GetCosmeticsControllerInstance();
            if (controllerInstance == null)
                return new List<CosmeticItem>();
            PropertyInfo cosmeticsProperty = controllerType.GetProperty("allCosmetics", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (cosmeticsProperty == null)
            {
                NotificationManager.SendNotification("Can't find allCosmetics property");
                return new List<CosmeticItem>();
            }
            object cosmetics = cosmeticsProperty.GetValue(controllerInstance);
            if (cosmetics == null)
            {
                NotificationManager.SendNotification("allCosmetics is null");
                return new List<CosmeticItem>();
            }
            return (List<CosmeticItem>)cosmetics;
        }

        public static void UpdateWardrobeModelsAndButtons()
        {
            Type controllerType = Type.GetType("CosmeticsController, Assembly-CSharp") ?? Type.GetType("GorillaNetworking.CosmeticsController, Assembly-CSharp");
            if (controllerType == null)
            {
                NotificationManager.SendNotification("Can't find CosmeticsController type");
                return;
            }
            object controllerInstance = GetCosmeticsControllerInstance();
            if (controllerInstance == null)
                return;
            MethodInfo method = controllerType.GetMethod("UpdateWardrobeModelsAndButtons", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
            {
                NotificationManager.SendNotification("Can't find UpdateWardrobeModelsAndButtons");
                return;
            }
            method.Invoke(controllerInstance, null);
        }

        public static void UnlockItem(string itemId)
        {
            Type controllerType = Type.GetType("CosmeticsController, Assembly-CSharp") ?? Type.GetType("GorillaNetworking.CosmeticsController, Assembly-CSharp");
            if (controllerType == null)
            {
                NotificationManager.SendNotification("Can't find CosmeticsController type");
                return;
            }
            object controllerInstance = GetCosmeticsControllerInstance();
            if (controllerInstance == null)
                return;
            MethodInfo method = controllerType.GetMethod("UnlockItem", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
            {
                NotificationManager.SendNotification("Can't find UnlockItem method");
                return;
            }
            method.Invoke(controllerInstance, new object[] { itemId });
        }

        public static void DickSpawn()
        {
            if (RightGrip)
            {
                BetaSpawnPrefab("STICKABLE TARGET", RightHandTransform().position, RightHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", RightHandTransform().position + Vector3.up * 0.3f, RightHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", RightHandTransform().position + Vector3.up * 0.6f, RightHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", RightHandTransform().position + Vector3.up * 0.9f, RightHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", RightHandTransform().position + Vector3.left * 0.3f, RightHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", RightHandTransform().position + Vector3.right * 0.3f, RightHandTransform().rotation);
            }

            if (LeftGrip)
            {
                BetaSpawnPrefab("STICKABLE TARGET", LeftHandTransform().position, LeftHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", LeftHandTransform().position + Vector3.up * 0.3f, LeftHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", LeftHandTransform().position + Vector3.up * 0.6f, LeftHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", LeftHandTransform().position + Vector3.up * 0.9f, LeftHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", LeftHandTransform().position + Vector3.left * 0.3f, LeftHandTransform().rotation);
                BetaSpawnPrefab("STICKABLE TARGET", LeftHandTransform().position + Vector3.right * 0.3f, LeftHandTransform().rotation);
            }
        }

        public static void TpSelfToPlayer(Photon.Realtime.Player plr)
        {
            MainTransform().transform.position = RigManager.GetVRRigFromPlayer(plr).headMesh.transform.position;
        }

        public static VRRig GetAllVRRigsWithoutMe(VRRig who)
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != myVRRig())
                        return rig;
                }
            }
            return null;
        }

        public static VRRig myVRRig() =>
            GorillaTagger.Instance.myVRRig;
        public static VRRig offlineVRRig() =>
            GorillaTagger.Instance.offlineVRRig;
        public static VRRig ActualRig() =>
            PhotonNetwork.InRoom ? myVRRig() : offlineVRRig();

        public static bool RightPrimary;
        public static bool LeftPrimary;
        public static bool RightSecondary;
        public static bool LeftSecondary;
        public static bool RightGrip;
        public static bool LeftGrip;
        public static bool RightTrigger;
        public static bool LeftTrigger;
        public static float RightTriggerFloat;
        public static float LeftTriggerFloat;
        public static bool RightJoystick;
        public static bool LeftJoystick;
        public static Vector2 RightJoystickAxis;
        public static Vector2 LeftJoystickAxis;

        public static string fps = "0";
        public static void UpdateFPS()
        {
            fps = (1f / Time.unscaledDeltaTime).ToString("F0");
        }

        public static GameObject platR = null;
        public static GameObject platL = null;
        public static void CreatePlatform(bool triggerplats, Transform handR, Transform handL, Quaternion rot, Quaternion rott, Vector3 scale, Color color, bool invis = false)
        {
            if (triggerplats ? RightTrigger : RightGrip)
            {
                if (platR == null)
                {
                    platR = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platR.transform.position = handR.position;
                    platR.transform.rotation = rot;
                    platR.transform.localScale = scale;
                    var rendererR = platR.GetComponent<Renderer>();
                    if (rendererR != null)
                    {
                        if (invis)
                            rendererR.enabled = false;
                        else
                            rendererR.material.color = color;
                    }
                }
            }
            else
            {
                if (platR != null)
                {
                    GameObject.Destroy(platR);
                    platR = null;
                }
            }
            if (triggerplats ? LeftTrigger : LeftGrip)
            {
                if (platL == null)
                {
                    platL = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platL.transform.position = handL.position;
                    platL.transform.rotation = rott;
                    platL.transform.localScale = scale;
                    var rendererL = platL.GetComponent<Renderer>();
                    if (rendererL != null)
                    {
                        if (invis)
                            rendererL.enabled = false;
                        else
                            rendererL.material.color = color;
                    }
                }
            }
            else
            {
                if (platL != null)
                {
                    GameObject.Destroy(platL);
                    platL = null;
                }
            }
        }

        public static bool BetaNotificiation = false;
        public static bool UsedBeforeNotificaiton = false;

        public static string name = "JupiterX";
        public static string author = "Nova";

        public static bool canusemenu = true;
        public static void LockCheck()
        {
            WebWrapper.Get("https://novax.lol/jupiterx/locks/lock1", s =>
            {
                if (s.Contains("true"))
                {
                    canusemenu = false;
                    NotificationManager.SendNotification("<color=red>[LOCKDOWN]</color> Menu has been locked down!", 50f);
                }
            });

            WebWrapper.Get("https://novax.lol/jupiterx/locks/mainlock", s =>
            {
                if (s.Contains("true"))
                {
                    canusemenu = false;
                    NotificationManager.SendNotification("<color=red>[LOCKDOWN]</color> Menu has been locked down!", 50f);
                }
            });
        }

        public const string version = "3.0.0";
        public static string serverversion;
        public static string minversion;
        public static string discord = "https://novax.lol/d";
        public static string[] blacklistedids;

        public static bool isBetaRelease = false;
        public static bool updateneeded = false;
        public static bool blacklisted = false;
        public static bool extremeupdateneeded = false;
        public static string motdtemplate = $"THANK YOU FOR USING JUPITERX, THE BEST FREE CHEAT MENU FOR GORILLA TAG COPYS. YOU ARE USING VERSION {version}, IF YOU HAVE PAID FOR THIS MENU YOU HAVE BEEN <color=red>SCAMMED</color>, JOIN THE DISCORD https://novax.lol/d";

        public static string MainPath = Path.Combine(Application.persistentDataPath, "JupiterX");
        public static string PreferencesPath = Path.Combine(MainPath, "Preferences.json");
        public static string HasUsedMenuBefore = Path.Combine(MainPath, "UsedBefore.txt");

        public static Text motdText;
        public static Text motd;
        public static Text cocText;
        public static Text codeOfConduct;

        public static string lastDeltaTime;
        public static bool FirstLaunch;
        public static VRRig GetVRRigFromPhotonView(PhotonView who)
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig.photonView == who)
                {
                    return rig;
                }
            }
            return null;
        }
        public static bool toOpen;
        public static void CreateFilesOnStart()
        {
            if (!Directory.Exists(MainPath))
                Directory.CreateDirectory(MainPath);
            if (!File.Exists(PreferencesPath))
                File.Create(PreferencesPath);
        }
        public static void DetectOtherUsers()
        {
            if (!networkedmenu)
                return;
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig == null || rig.photonView == null || rig.photonView.Owner == null || rig == myVRRig())
                    continue;
                var props = rig.photonView.Owner.CustomProperties;
                if (props == null || !props.ContainsKey("jupiterx2026revive"))
                    continue;
                Transform menuTransform = rig.transform.Find("jupiterxMenu");
                if (rig.leftThumb.calcT > 0.2f)
                {
                    if (menuTransform == null)
                    {
                        GameObject menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        menu.transform.SetParent(rig.transform);
                        menu.name = "jupiterxMenu";
                        UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
                        UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
                        UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
                        menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);
                        GameObject menuObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        menuObj.transform.SetParent(menu.transform);
                        UnityEngine.Object.Destroy(menuObj.GetComponent<Rigidbody>());
                        UnityEngine.Object.Destroy(menuObj.GetComponent<BoxCollider>());
                        menuObj.GetComponent<Renderer>().material.color = Settings.backgroundColor.GetCurrentColor();
                        menuObj.transform.localPosition = new Vector3(-0.05f, 0f, 0f);
                        menuObj.transform.localRotation = Quaternion.identity;
                        menuObj.transform.localScale = new Vector3(0.1f, 1f, 1f);
                    }
                    else
                    {
                        menuTransform.position = rig.leftHandTransform.position;
                        menuTransform.rotation = rig.leftHandTransform.rotation;
                    }
                }
                else
                {
                    if (menuTransform != null)
                        GameObject.Destroy(menuTransform.gameObject);
                }
            }
        }
        public static void Panic()
        {
            foreach (ButtonInfo[] btn in Buttons.buttons)
            {
                foreach (ButtonInfo button in btn)
                {
                    if (button.enabled)
                        Toggle(button.buttonText);
                }
            }
        }

        private static Material linermat;
        public static (GameObject lineholder, LineRenderer line) CreateLine(Transform pos1, Transform pos2, Color color)
        {
            GameObject lineholder = new GameObject();
            LineRenderer line = lineholder.AddComponent<LineRenderer>();
            line.positionCount = 2;
            if (linermat == null)
            {
                linermat = new Material(GUIShader());
            }
            line.material = linermat;
            line.useWorldSpace = true;
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            line.startColor = color;
            line.endColor = color;
            line.SetPosition(0, pos1.position);
            line.SetPosition(1, pos2.position);
            GameObject.Destroy(lineholder, Time.deltaTime);
            return (lineholder, line);
        }
        public static void BetaAntiCosmetic(string cosmeticId)
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != myVRRig())
                    {
                        if (rig.concatStringOfCosmeticsAllowed.Contains(cosmeticId))
                        {
                            PhotonNetwork.Disconnect();
                            NotificationManager.SendNotification("Someone with " + cosmeticId + " joined.", 5f);
                        }
                    }
                }
            }
        }
        public static void BetaAntiReport(bool Crash, bool Disconnect)
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (GorillaPlayerScoreboardLine lines in GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>())
                {
                    if (lines.linePlayer.UserId == MyPlayer().UserId)
                    {
                        Transform reportBtn = lines.reportButton.gameObject.transform;
                        foreach (VRRig rig in GorillaParent.instance.vrrigs)
                        {
                            if (rig != null && rig != myVRRig())
                            {
                                float disR = Vector3.Distance(reportBtn.transform.position, rig.rightHandTransform.position);
                                float disL = Vector3.Distance(reportBtn.transform.position, rig.leftHandTransform.position);

                                if (disR < 0.50f || disL < 0.50f)
                                {
                                    if (Crash)
                                    {
                                        MakeMeMaster();
                                        PhotonNetwork.DestroyPlayerObjects(rig.photonView.Owner);
                                    }
                                    if (Disconnect)
                                    {
                                        PhotonNetwork.Disconnect();
                                        PhotonNetwork.ConnectUsingSettings();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void FindObjects()
        {
            motd = GameObject.Find("motd")?.GetComponent<Text>();
            motdText = GameObject.Find("motdtext")?.GetComponent<Text>();
            motdText.text = motdtemplate;
            codeOfConduct = GameObject.Find("CodeOfConduct")?.GetComponent<Text>();
            cocText = GameObject.Find("COC Text")?.GetComponent<Text>();
        }

        public static void CreateCustomBoards(Text top, Text bottom, string title, string text)
        {
            if (top != null)
                top.text = title;
            if (bottom != null)
                bottom.text = text;
            if (top == null && bottom == null)
                FindObjects();
        }

        public static string ogmotd;
        public static string ogmotdtext;
        public static string ogcoc;
        public static string ogcoctext;

        public static AudioClip buttonClickSound = null;
        public static AudioClip menuOpenSound = null;
        public static AudioClip menuCloseSound = null;
        public static AudioClip achievementSound = null;

        public static void CacheSounds()
        {
            buttonClickSound = GetAudioClip("JupiterX.Resources.steal.wav");
            menuOpenSound = GetAudioClip("JupiterX.Resources.menuopen.wav");
            menuCloseSound = GetAudioClip("JupiterX.Resources.menuclose.wav");
            achievementSound = GetAudioClip("JupiterX.Resources.achievement.wav");
        }

        public static List<AudioSource> cachedSources = new List<AudioSource>();

        private static readonly Dictionary<string, AudioClip> CachedClips = new Dictionary<string, AudioClip>();
        public static AudioClip GetAudioClip(string resourceName)
        {
            if (CachedClips.TryGetValue(resourceName, out AudioClip cachedClip))
                return cachedClip;
            byte[] soundBytes = LoadEmbeddedSounds(resourceName);
            if (soundBytes == null)
                return null;
            AudioClip clip = WavToAudioClip(soundBytes);
            if (clip == null)
                return null;
            CachedClips[resourceName] = clip;
            return clip;
        }

        public static void PlaySound(AudioClip clip, float volume = 0.5f)
        {
            if (clip == null)
                return;
            AudioSource source = offlineVRRig().gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.loop = false;
            source.Play();
            UnityEngine.Object.Destroy(source, clip.length + 0.1f);
        }

        private static byte[] LoadEmbeddedSounds(string resourceName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;

                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        public static Assembly LoadEmbeddedDll(string resourceName)
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                    return null;
                byte[] assemblyData = new byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        }

        private static AudioClip WavToAudioClip(byte[] fileBytes)
        {
            const int headerSize = 44;
            if (fileBytes.Length < headerSize)
                return null;
            int sampleRate = BitConverter.ToInt32(fileBytes, 24);
            int channels = BitConverter.ToInt16(fileBytes, 22);
            int dataSize = fileBytes.Length - headerSize;
            int sampleCount = dataSize / 2;
            float[] samples = new float[sampleCount];
            for (int i = 0; i < sampleCount; i++)
            {
                short sample = BitConverter.ToInt16(fileBytes, headerSize + (i * 2));
                samples[i] = sample / 32768f;
            }
            AudioClip clip = AudioClip.Create("sound", sampleCount / channels, channels, sampleRate, false);
            clip.SetData(samples, 0);
            return clip;
        }

        public static PhotonNetworkController photonNetworkController
        {
            get
            {
                return GameObject.FindObjectsOfType<PhotonNetworkController>().FirstOrDefault();
            }
        }

        public static PlayFabAuthenticator playFabAuthenticator
        {
            get
            {
                return GameObject.FindObjectsOfType<PlayFabAuthenticator>().FirstOrDefault();
            }
        }

        public class SavedSettings
        {
            public int currentTheme { get; set; }
            public int currentFontStyleChoice { get; set; }
            public int PageType { get; set; }
            public int dropType { get; set; }
            public int inputTextColorInt { get; set; }
            public int gunVariation { get; set; }
            public int GunDirection { get; set; }
            public int menuScaleIndex { get; set; }
            public int FlySpeedAmount { get; set; }
            public int ArmSizeAmount { get; set; }
            public List<string> enabledMods { get; set; }
            public List<string> favorites { get; set; }
            public List<string> quickactions { get; set; }
        }

        public static string SaveSettings()
        {
            SavedSettings settings = new SavedSettings
            {
                currentTheme = currentTheme,
                currentFontStyleChoice = currentFontStyleChoice,
                PageType = PageType,
                dropType = dropType,
                inputTextColorInt = inputTextColorInt,
                gunVariation = gunVariation,
                GunDirection = GunDirection,
                menuScaleIndex = menuScaleIndex,
                FlySpeedAmount = Movement.FlySpeedAmount,
                ArmSizeAmount = Movement.ArmSizeAmount,
                enabledMods = Buttons.buttons.SelectMany(x => x).Where(x => x.enabled).Select(x => x.buttonText).ToList(),
                favorites = favorites,
                quickactions = quickActions
            };
            return JsonConvert.SerializeObject(settings, Formatting.Indented);
        }

        public static void LoadSettings(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;
            try
            {
                SavedSettings settings = JsonConvert.DeserializeObject<SavedSettings>(json);
                if (settings == null)
                    return;

                currentTheme = settings.currentTheme - 1; ChangeMenuTheme();
                currentFontStyleChoice = settings.currentFontStyleChoice - 1; ChangeFontStyle();
                PageType = settings.PageType - 1; ChangePageType();
                dropType = settings.dropType - 1; ChangeDropType();
                inputTextColorInt = settings.inputTextColorInt - 1; ChangeInputTextColor();
                gunVariation = settings.gunVariation - 1; ChangeGunVariation();
                GunDirection = settings.GunDirection - 1; ChangeGunDirection();
                menuScaleIndex = settings.menuScaleIndex - 1; ChangeMenuScale();
                Movement.FlySpeedAmount = settings.FlySpeedAmount - 1; Movement.ChangeFlySpeed();
                Movement.ArmSizeAmount = settings.ArmSizeAmount - 1; Movement.ChangeArmLength();

                HashSet<string> enabled = settings.enabledMods.ToHashSet();
                foreach (ButtonInfo button in Buttons.buttons.SelectMany(x => x))
                    if (button.enabled != enabled.Contains(button.buttonText))
                        Toggle(button.buttonText);

                favorites.Clear();
                foreach (var fav in settings.favorites) favorites.Add(fav);

                quickActions.Clear();
                foreach (var quick in settings.quickactions) quickActions.Add(quick);
            }
            catch (Exception ex) { Log($"Unable to load settings {ex}", JupxLogType.Error); }
        }

        public static void SavePreferences()
        {
            Directory.CreateDirectory(MainPath);
            File.WriteAllText(PreferencesPath, SaveSettings());
        }

        public static void LoadPreferences()
        {
            if (File.Exists(PreferencesPath))
                LoadSettings(File.ReadAllText(PreferencesPath));
        }
    }
}