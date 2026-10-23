// ============================================================
//  JupiterX | Mods/Settings.cs
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
using JupiterX.Classes.Menu;
using JupiterX.Menu;
using JupiterX.Notifications;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using static JupiterX.Menu.Main;

namespace JupiterX
{
    public class Settings
    {
        public static ExtGradient backgroundColor = new ExtGradient{rainbow = false};
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient{colors = GetSolidGradient(Color.black) }, // Disabled
            new ExtGradient{colors = GetSolidGradient(Color.red)} // Enabled
        };
        public static Color[] textColors = new Color[]
        {
            Color.white, // Disabled
            Color.white // Enabled
        };

        public static Font currentFont = (Resources.GetBuiltinResource<Font>("Arial.ttf") as Font);

        public static float menuScale = 1f;

        public static bool VersionText = true;
        public static bool showfps = true;
        public static bool CustomBoards = true;
        public static bool DisconnectButton = false;
        public static bool MenuTitle = true;
        public static bool CustomMenuTitle = false;
        public static bool homeButton = false;
        public static bool RightHanded = false;
        public static bool bothHands = false;
        public static bool StumpText = true;
        public static bool Notifications = true;
        public static bool Rounding = false;
        public static bool menuoutline = false;
        public static bool lowqualttext = false;
        public static bool lowercaseMode = false;
        public static bool uppercaseMode = false;
        public static bool NoAutoSizeText = false;
        public static bool flipMenu = false;
        public static bool menuTrail = false;
        public static bool pointerTrail = false;
        public static bool DisableButtonSounds = false;
        public static bool DisableMenuSounds = false;
        public static bool DisablePageNumber = false;
        public static bool hidepointer = false;
        public static bool networkedmenu = true;
        public static bool showEnabledModsVR = true;
        public static bool advancedArraylist;
        public static bool flipArraylist;
        public static bool disableGhostview;
        public static bool legacyGhostview;
        public static bool dynamicAnimations;

        public static Material _trailMat;

        public static Vector3 smoothTargetPosition = Vector3.zero;
        public static Quaternion smoothTargetRotation = Quaternion.identity;

        public static bool smoothMenuPosition;
        public static bool smoothMenuRotation;
        public static bool serversidedButtonSounds;
        public static bool disableFavoritingKeybind;
        public static bool disablepanicprompt;

        public static KeyCode keyboardButton = KeyCode.Q;

        public static int buttonsPerPage = 8;

        public static int inputTextColorInt = 3;
        public static void ChangeInputTextColor(bool positive = true)
        {
            string[] textColors = {
                "Red",
                "Orange",
                "Yellow",
                "Green",
                "Blue",
                "Cyan",
                "Purple",
                "Pink",
                "White",
                "Grey",
                "Black",
                "Rose"
            };
            string[] realinputcolor = {
                "red",
                "#ff8000",
                "yellow",
                "green",
                "blue",
                "cyan",
                "purple",
                "#FF00FF",
                "white",
                "grey",
                "black",
                "#ff005d"
            };

            if (positive)
                inputTextColorInt++;
            else
                inputTextColorInt--;

            inputTextColorInt %= realinputcolor.Length;
            if (inputTextColorInt < 0)
                inputTextColorInt = realinputcolor.Length - 1;

            inputTextColor = realinputcolor[inputTextColorInt];
            Buttons.GetIndex("Change Input Text Color").overlapText = $"Change Input Text Color <color=grey>[</color><color=cyan>{textColors[inputTextColorInt]}</color><color=grey>]</color>";
        }

        public static void ChangeGunVariation(bool positive = true)
        {
            string[] VariationNames = {
                "Default",
                "Lightning",
                "Wavy",
                "Blocky",
                "Zigzag",
                "Spring",
                "Bouncy",
                "Bezier"
            };

            if (positive)
                gunVariation++;
            else
                gunVariation--;

            gunVariation %= VariationNames.Length;
            if (gunVariation < 0)
                gunVariation = VariationNames.Length - 1;

            Buttons.GetIndex("Change Gun Variation").overlapText = $"Change Gun Variation <color=grey>[</color><color=cyan>" + VariationNames[gunVariation] + "</color><color=grey>]</color>";
        }

        public static void ChangeGunDirection(bool positive = true)
        {
            string[] VariationNames = {
                "Default",
                "Legacy",
                "Forward",
                "Inverted"
            };

            if (positive)
                GunDirection++;
            else
                GunDirection--;

            GunDirection %= VariationNames.Length;
            if (GunDirection < 0)
                GunDirection = VariationNames.Length - 1;

            Buttons.GetIndex("Change Gun Direction").overlapText = $"Change Gun Direction <color=grey>[</color><color=cyan>" + VariationNames[GunDirection] + "</color><color=grey>]</color>";
        }

        public static int menuScaleIndex = 10;
        public static void ChangeMenuScale(bool positive = true)
        {
            if (positive)
                menuScaleIndex++;
            else
                menuScaleIndex--;

            if (menuScaleIndex > 30)
                menuScaleIndex = 2;
            if (menuScaleIndex < 2)
                menuScaleIndex = 30;

            menuScale = menuScaleIndex / 10f;

            Buttons.GetIndex("Change Menu Scale").overlapText = "Change Menu Scale <color=grey>[</color><color=cyan>" + menuScale + "</color><color=grey>]</color>";
        }

        public static void CategorySettings()
        {
            List<ButtonInfo> buttons = new List<ButtonInfo> { new ButtonInfo { buttonText = "Exit Menu Settings", method = () => { Buttons.CurrentCategoryName = "Settings"; Buttons.buttons[Buttons.GetCategory("Temporary Category")] = Array.Empty<ButtonInfo>(); }, isTogglable = false, toolTip = "Returns you back to the settings menu." } };

            foreach (var button in Buttons.buttons[Buttons.GetCategory("Main")])
            {
                buttons.Add(new ButtonInfo
                {
                    buttonText = $"Category{button.buttonText.GetHashCode()}",
                    overlapText = button.buttonText,
                    enabled = !skipButtons.Contains(button.buttonText),
                    enableMethod = () => skipButtons.Remove(button.buttonText),
                    disableMethod = () => skipButtons.Add(button.buttonText),
                    toolTip = "Toggles the visibility of the category " + button.buttonText + ".",
                    hideFromArraylist = true
                });
            }

            Buttons.buttons[Buttons.GetCategory("Temporary Category")] = buttons.ToArray();
            Buttons.CurrentCategoryName = "Temporary Category";
        }

        public static void GlobalReturn()
        {
            NotificationManager.ClearAllNotifications();
            Toggle(Buttons.buttons[Buttons.CurrentCategoryIndex][Buttons.GetCategory("Main")].buttonText, true);

            if (prompts.Count > 0)
                StopCurrentPrompt();
        }
        public static void StopCurrentPrompt() =>
            prompts.RemoveAt(0);

        public static void Players()
        {
            Buttons.CurrentCategoryName = "Players";

            List<ButtonInfo> buttons = new List<ButtonInfo> {
                new ButtonInfo {
                    buttonText = "Exit Players",
                    method =() => Buttons.CurrentCategoryName = "Main",
                    isTogglable = false,
                    toolTip = "Returns you back to the main page."
                }
            };


            if (!PhotonNetwork.InRoom)
                buttons.Add(new ButtonInfo { buttonText = "Not in a Room", label = true, isTogglable = false });
            else
            {
                for (int i = 0; i < PhotonNetwork.PlayerListOthers.Length; i++)
                {
                    Photon.Realtime.Player player = PhotonNetwork.PlayerListOthers[i];
                    buttons.Add(new ButtonInfo
                    {
                        buttonText = $"PlayerButton{i}",
                        overlapText = $"<color=cyan>" + player.NickName.ToUpper() + "</color>",
                        method = () => NavigatePlayer(player),
                        isTogglable = false,
                        toolTip = $"See information on the player {player.NickName}."
                    });
                }
            }

            Buttons.buttons[Buttons.GetCategory("Players")] = buttons.ToArray();
        }

        static void NavigatePlayer(Photon.Realtime.Player plr)
        {
            string TargetName = plr.NickName.ToUpper();
            string UserId = plr.UserId;

            List<ButtonInfo> buttons = new List<ButtonInfo> {
                new ButtonInfo {
                    buttonText = "Exit PlayerInspect",
                    overlapText = $"Exit {TargetName}",
                    method =() => Players(),
                    isTogglable = false,
                    toolTip = "Returns you back to the players tab."
                },

                /*new ButtonInfo {
                    buttonText = "Placeholder",
                    overlapText = $"Does placeholder to {TargetName}",
                    method =() => Utility.BetaCrashPlayer(plr),
                    isTogglable = false,
                },*/
                new ButtonInfo {
                    buttonText = "Crash Player",
                    overlapText = $"Crash {TargetName}",
                    method =() => Utility.CrashPlayerForPlayerTab(plr),
                    isTogglable = true,
                },
                new ButtonInfo {
                    buttonText = "Insta Crash Player",
                    overlapText = $"Insta Crash {TargetName}",
                    method =() => Utility.InstaCrashPlayer(plr),
                    isTogglable = true,
                },
                new ButtonInfo {
                    buttonText = "Tag Player",
                    overlapText = $"Tag {TargetName}",
                    method =() => Utility.TagPlayer(plr),
                    isTogglable = false,
                },
                new ButtonInfo {
                    buttonText = "Slow Player",
                    overlapText = $"Slow {TargetName}",
                    method =() => Utility.SlowPlayer(plr),
                    isTogglable = false,
                },
                new ButtonInfo {
                    buttonText = "TP Self To",
                    overlapText = $"TP Self To {TargetName}",
                    method =() => Utility.TpSelfToPlayer(plr),
                    isTogglable = false,
                },
            };

            if (ServerDataJupiterX.Administrators.ContainsKey(PhotonNetwork.LocalPlayer.UserId))
            {
                buttons.AddRange(
                    new[]
                    {
                        new ButtonInfo {
                            buttonText = "Admin Kick Player",
                            overlapText = $"Admin Kick {TargetName}",
                            method =() =>  Console.ConsoleJupiterX.ExecuteCommand($"{UserId}\n\nkickgun"),
                            isTogglable = false,
                            toolTip = $"Kicks {TargetName} if they're using the menu."
                        },
                        new ButtonInfo {
                            buttonText = "Admin Quit Player",
                            overlapText = $"Admin Quit {TargetName}",
                            method =() =>  Console.ConsoleJupiterX.ExecuteCommand($"{UserId}\n\nquitgun"),
                            isTogglable = false,
                            toolTip = $"Quits {TargetName} if they're using the menu."
                        },
                        new ButtonInfo {
                            buttonText = "Admin Ghost Player",
                            overlapText = $"Admin Ghost {TargetName}",
                            method =() =>  Console.ConsoleJupiterX.ExecuteCommand($"{UserId}\n\nghostgun"),
                            isTogglable = false,
                            toolTip = $"Ghosts {TargetName} if they're using the menu."
                        },
                        new ButtonInfo {
                            buttonText = "Admin Unghost Player",
                            overlapText = $"Admin Unghost {TargetName}",
                            method =() =>  Console.ConsoleJupiterX.ExecuteCommand($"{UserId}\n\nunghostgun"),
                            isTogglable = false,
                            toolTip = $"Unghosts {TargetName} if they're using the menu."
                        },
                        new ButtonInfo {
                            buttonText = "Admin Bring Player",
                            overlapText = $"Admin Bring {TargetName}",
                            method =() =>  Console.ConsoleJupiterX.ExecuteCommand($"{UserId}\n\ngotouser"),
                            isTogglable = false,
                            toolTip = $"Brings {TargetName} if they're using the menu."
                        },
                        new ButtonInfo {
                            buttonText = "Admin Fling Player",
                            overlapText = $"Admin Fling {TargetName}",
                            method =() =>  Console.ConsoleJupiterX.ExecuteCommand($"{UserId}\n\nadminflinggun"),
                            isTogglable = false,
                            toolTip = $"Flings {TargetName} if they're using the menu."
                        },
                    }
                );
            }

            Buttons.buttons[Buttons.GetCategory("Temporary Category")] = buttons.ToArray();
            Buttons.CurrentCategoryName = "Temporary Category";
        }
    }
}