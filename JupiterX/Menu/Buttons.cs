// ============================================================
//  JupiterX | Menu/Buttons.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using JupiterX.Classes.Menu;
using JupiterX.Managers;
using JupiterX.Mods;
using JupiterX.Notifications;
using JupiterX.Patches;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static JupiterX.Menu.Main;
using static JupiterX.Settings;

namespace JupiterX.Menu
{
    public class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods
                new ButtonInfo { buttonText = "Join Discord", method = Important.JoinDiscord, isTogglable = false, toolTip = "Invites you to join the <b>Qolossal</b> Discord server."},

                new ButtonInfo { buttonText = "Settings", method =() => CurrentCategoryName = "Settings", isTogglable = false, toolTip = "Opens the main settings page for the menu."},
                new ButtonInfo { buttonText = "Players", method =() => Players(), isTogglable = false, toolTip = "Opens the player mods page for the menu."},

                new ButtonInfo { buttonText = "Favorite", method =() => CurrentCategoryName = "Favorite", isTogglable = false, toolTip = "Opens the favorite mods page for the menu."},
                new ButtonInfo { buttonText = "Enabled", method =() => CurrentCategoryName = "Enabled", isTogglable = false, toolTip = "Opens the enabled mods page for the menu."},

                new ButtonInfo { buttonText = "Important", method =() => CurrentCategoryName = "Important", isTogglable = false, toolTip = "Opens the important mods page for the menu."},
                new ButtonInfo { buttonText = "Safety", method =() => CurrentCategoryName = "Safety", isTogglable = false, toolTip = "Opens the safety mods page for the menu."},
                new ButtonInfo { buttonText = "Computer", method =() => CurrentCategoryName = "Computer", isTogglable = false, toolTip = "Opens the computer mods page for the menu."},
                new ButtonInfo { buttonText = "Movement", method =() => CurrentCategoryName = "Movement", isTogglable = false, toolTip = "Opens the movement mods page for the menu."},
                new ButtonInfo { buttonText = "Advantage", method =() => CurrentCategoryName = "Advantage", isTogglable = false, toolTip = "Opens the advantage mods page for the menu."},
                new ButtonInfo { buttonText = "VRRig", method =() => CurrentCategoryName = "VRRig", isTogglable = false, toolTip = "Opens the vrrig mods page for the menu."},
                new ButtonInfo { buttonText = "Visual", method =() => CurrentCategoryName = "Visual", isTogglable = false, toolTip = "Opens the visual mods page for the menu."},
                new ButtonInfo { buttonText = "Fun", method =() => CurrentCategoryName = "Fun", isTogglable = false, toolTip = "Opens the fun mods page for the menu."},
                new ButtonInfo { buttonText = "Name", method =() => CurrentCategoryName = "Name", isTogglable = false, toolTip = "Opens the name mods page for the menu."},
                new ButtonInfo { buttonText = "Prefabs", method =() => CurrentCategoryName = "Prefabs", isTogglable = false, toolTip = "Opens the prefab mods page for the menu."},
                new ButtonInfo { buttonText = "Overpowered", method =() => CurrentCategoryName = "Overpowered", isTogglable = false, toolTip = "Opens the overpowered page for the menu."},
                new ButtonInfo { buttonText = "Experimental", method =() => CurrentCategoryName = "Experimental", isTogglable = false, toolTip = "Opens the experimental mods page for the menu."},
                new ButtonInfo { buttonText = "Master", method =() => CurrentCategoryName = "Master", isTogglable = false, toolTip = "Opens the master mods page for the menu."},
                new ButtonInfo { buttonText = "Gorilla Tag Horror", method =() => CurrentCategoryName = "Gorilla Tag Horror", isTogglable = false, toolTip = "Opens the gorilla tag horror mods page for the menu."},

                new ButtonInfo { buttonText = "Photon Bots", method =() => CurrentCategoryName = "Photon Bots", isTogglable = false, toolTip = "Opens the photon bots page for the menu."},

                new ButtonInfo { buttonText = "Projectiles", method =() => CurrentCategoryName = "Projectiles", isTogglable = false, toolTip = "Opens the projectiles mods page for the menu."},
                new ButtonInfo { buttonText = "Sound Spammers", method =() => CurrentCategoryName = "Sound Spammers", isTogglable = false, toolTip = "Opens the sound spammers mods page for the menu."},

                new ButtonInfo { buttonText = "Soundboard", method =() => Soundboard.LoadSoundboard(), isTogglable = false, toolTip = "Opens the soundboard page for the menu."},

                new ButtonInfo { buttonText = "Achievements", method = AchievementManager.EnterAchievementTab, isTogglable = false, toolTip = "Opens the achievements page." },
                new ButtonInfo { buttonText = "Credits", method =() => CurrentCategoryName = "Credits", isTogglable = false, toolTip = "Opens the credits page." }
            },

            new ButtonInfo[] { // Settings
                new ButtonInfo { buttonText = "Exit Settings", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Menu Settings", method =() => CurrentCategoryName = "Menu Settings", isTogglable = false, toolTip = "Opens the menu settings page for the menu." },
                new ButtonInfo { buttonText = "Category Settings", method = CategorySettings, isTogglable = false, toolTip = "Opens the settings for the categories." },

                new ButtonInfo { buttonText = "Movement Settings", method =() => CurrentCategoryName = "Movement Settings", isTogglable = false, toolTip = "Opens the movement settings page for the menu." },
                new ButtonInfo { buttonText = "Gun Settings", method =() => CurrentCategoryName = "Gun Settings", isTogglable = false, toolTip = "Opens the gun settings page for the menu." },

                new ButtonInfo { buttonText = "Plugin Settings", method =() => CurrentCategoryName = "Plugin Settings", isTogglable = false, toolTip = "Opens the settings for the plugins."},
            },

            new ButtonInfo[] { // Menu Settings
                new ButtonInfo { buttonText = "Exit Menu Settings", method =() => CurrentCategoryName = "Settings", isTogglable = false, toolTip = "Returns to the settings page of the menu." },

                new ButtonInfo { buttonText = "Right Hand", enableMethod =() => RightHanded = true, disableMethod =() => RightHanded = false, toolTip = "Puts the menu on your right hand."},
                new ButtonInfo { buttonText = "Both Hands", enableMethod =() => bothHands = true, disableMethod =() => bothHands = false, toolTip = "Puts the menu on your both of your hands."},

                new ButtonInfo { buttonText = "Freeze Player In Menu", method =() => Utility.FreezePlayerInMenu(), isTogglable = true, toolTip = "Lets you float while the menu is open."},
                new ButtonInfo { buttonText = "Ghost In Menu", method =() => Utility.GhostInMenu(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Makes you have ghost monke when menu is open."},
                new ButtonInfo { buttonText = "Invis In Menu", method =() => Utility.InvisInMeun(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Makes you have invis monke when menu is open."},
                new ButtonInfo { buttonText = "Round Menu", enableMethod =() => Rounding = true, disableMethod =() => Rounding = false, isTogglable = true, toolTip = "Toggle the menu rounding [<color=red>CAN CAUSE LAG</color>]."},

                new ButtonInfo { buttonText = "Flip Menu", enableMethod =() => flipMenu = true, disableMethod =() => flipMenu = false, toolTip = "Flips the menu to the back of your hand."},
                new ButtonInfo { buttonText = "Menu Trail", enableMethod =() => menuTrail = true, disableMethod =() => menuTrail = false, toolTip = "Gives the menu a trail when you drop."},
                new ButtonInfo { buttonText = "Pointer Trail", enableMethod =() => pointerTrail = true, disableMethod =() => pointerTrail = false, toolTip = "Gives the pointer a trail when you drop."},
                new ButtonInfo { buttonText = "Hide Pointer", enableMethod =() => hidepointer = true, disableMethod =() => hidepointer = false, toolTip = "Hides the menu pointer."},
                new ButtonInfo { buttonText = "See Others Menus", enableMethod =() => networkedmenu = true, disableMethod =() => networkedmenu = false, toolTip = "Lets you see other menu users menus."},

                new ButtonInfo { buttonText = "Disable Menu Sounds", enableMethod =() => DisableMenuSounds = true, disableMethod =() => DisableMenuSounds = false, toolTip = "Disables the menu open and close sounds."},
                new ButtonInfo { buttonText = "Disable Button Sounds", enableMethod =() => DisableButtonSounds = true, disableMethod =() => DisableButtonSounds = false, toolTip = "Disables the button sounds."},
                new ButtonInfo { buttonText = "Disable Disconnect Button", enableMethod =() => DisconnectButton = true, disableMethod =() => DisconnectButton = false, enabled = DisconnectButton, toolTip = "Toggles the disconnect button."},
                new ButtonInfo { buttonText = "Disable Incremental Buttons", enableMethod =() => incrementalButtons = false, disableMethod =() => incrementalButtons = true, toolTip = "Disables the buttons with the increment and decrement buttons next to it."},
                new ButtonInfo { buttonText = "Disable Menu Title", enableMethod =() => MenuTitle = false, disableMethod =() => MenuTitle = true, toolTip = "Toggles the menu title."},
                new ButtonInfo { buttonText = "Disable Page Number", enableMethod =() => DisablePageNumber = true, disableMethod =() => DisablePageNumber = false, toolTip = "Disables the page number on the title."},
                new ButtonInfo { buttonText = "Custom Menu Title", enableMethod =() => CustomMenuTitle = true, disableMethod =() => CustomMenuTitle = false, toolTip = "Gives the menu a custom title you choose inside a txt."},
                new ButtonInfo { buttonText = "Change Page Type", method =() => Utility.ChangePageType(), enableMethod =() => Utility.ChangePageType(), disableMethod =() => Utility.ChangePageType(false), incremental = true, overlapText = "Change Page Type <color=grey>[<color=cyan>Side</color>]</color>", isTogglable = false, toolTip = "Changes the page type." },
                new ButtonInfo { buttonText = "Change Font Style", method =() => Utility.ChangeFontStyle(), enableMethod =() => Utility.ChangeFontStyle(), disableMethod =() => Utility.ChangeFontStyle(false), incremental = true, overlapText = "Change Font Style <color=grey>[<color=cyan>Default</color>]</color>", isTogglable = false, toolTip = "Changes the font style of the menu." },

                new ButtonInfo { buttonText = "Change Input Text Color", overlapText = "Change Input Text Color <color=grey>[</color><color=cyan>Cyan</color><color=grey>]</color>", method =() => Settings.ChangeInputTextColor(), enableMethod =() => Settings.ChangeInputTextColor(), disableMethod =() => Settings.ChangeInputTextColor(false), incremental = true, isTogglable = false, toolTip = "Changes the color of the input indicator next to the buttons."},

                new ButtonInfo { buttonText = "Disable Favorite Keybind", enableMethod =() => disableFavoritingKeybind = true, disableMethod =() => disableFavoritingKeybind = false, toolTip = "Makes it so toggling a button while holding left grip does nothing." },

                new ButtonInfo { buttonText = "Smooth Menu Position", enableMethod =() => smoothMenuPosition = true, disableMethod =() => smoothMenuPosition = false, toolTip = "Smoothes the menu's position."},
                new ButtonInfo { buttonText = "Smooth Menu Rotation", enableMethod =() => smoothMenuRotation = true, disableMethod =() => smoothMenuRotation = false, toolTip = "Smoothes the menu's rotation."},

                new ButtonInfo { buttonText = "Change Menu Scale", overlapText = "Change Menu Scale <color=grey>[</color><color=cyan>1</color><color=grey>]</color>", method =() => Settings.ChangeMenuScale(), enableMethod =() => Settings.ChangeMenuScale(), disableMethod =() => Settings.ChangeMenuScale(false), incremental = true, isTogglable = false, toolTip = "Changes the scale of the menu."},

                new ButtonInfo { buttonText = "Serversided Button Sounds", enableMethod =() => serversidedButtonSounds = true, disableMethod =() => serversidedButtonSounds = false, toolTip = "Lets everyone in the the room hear the buttons."},

                //new ButtonInfo { buttonText = "Disable Search Button", enableMethod =() => disableSearchButton = true, disableMethod =() => disableSearchButton = false, toolTip = "Disables the search button at the bottom of the menu."},
                new ButtonInfo { buttonText = "Disable Return Button", enableMethod =() => disableReturnButton = true, disableMethod =() => disableReturnButton = false, toolTip = "Disables the return button at the bottom of the menu."},

                new ButtonInfo { buttonText = "Change Menu Theme", method =() => Utility.ChangeMenuTheme(), enableMethod =() => Utility.ChangeMenuTheme(), disableMethod =() => Utility.ChangeMenuTheme(false), incremental = true, overlapText = "Change Menu Theme <color=grey>[<color=cyan>Default</color>]</color>", isTogglable = false, toolTip = "Changes the menu theme." },
                new ButtonInfo { buttonText = "Menu Outline", overlapText = "Outline Menu", enableMethod =() => menuoutline = true, disableMethod =() => menuoutline = false, isTogglable = true, toolTip = "Gives the menu a outline." },
                new ButtonInfo { buttonText = "Change Drop Type", method =() => Utility.ChangeDropType(), enableMethod =() => Utility.ChangeDropType(), disableMethod =() => Utility.ChangeDropType(false), incremental = true, overlapText = "Change Drop Type <color=grey>[<color=cyan>Destroy</color>]</color>", isTogglable = false, toolTip = "Changes the drop type for the menu." },

                new ButtonInfo { buttonText = "Advanced Arraylist", enableMethod =() => Settings.advancedArraylist = true, disableMethod =() => Settings.advancedArraylist = false, toolTip = "Updates the FPS Counter less, making it easier to read."},
                new ButtonInfo { buttonText = "Flip Arraylist", enableMethod =() => Settings.flipArraylist = true, disableMethod =() => Settings.flipArraylist = false, toolTip = "Flips the arraylist at the top of the screen."},

                new ButtonInfo { buttonText = "Disable Arraylist GUI", enableMethod =() => Settings.showEnabledModsVR = false, disableMethod =() => Settings.showEnabledModsVR = true, toolTip = "Disables the GUI that shows the enabled mods."},
                new ButtonInfo { buttonText = "Disable Notifications", enableMethod =() => { Settings.Notifications = false; NotificationManager.ClearAllNotifications(); }, disableMethod =() => Settings.Notifications = true, toolTip = "Toggles the Notifcations."},
                new ButtonInfo { buttonText = "Disable Master Client Notifications", enableMethod =() => disableMasterClientNotifications = true, disableMethod =() => disableMasterClientNotifications = false, toolTip = "Disables all notifications regarding master client."},
                new ButtonInfo { buttonText = "Disable Room Notifications", enableMethod =() => disableRoomNotifications = true, disableMethod =() => disableRoomNotifications = false, toolTip = "Disables all notifications regarding the room."},
                new ButtonInfo { buttonText = "Disable Player Notifications", enableMethod =() => disablePlayerNotifications = true, disableMethod =() => disablePlayerNotifications = false, toolTip = "Disables all notifications regarding players."},
                new ButtonInfo { buttonText = "Clear Notifications on Disconnect", enableMethod =() => clearNotificationsOnDisconnect = true, disableMethod =() => clearNotificationsOnDisconnect = false, toolTip = "Clears all notifications on disconnect."},

                new ButtonInfo { buttonText = "Low Quality Text", enableMethod =() => lowqualttext = true, disableMethod =() => lowqualttext = false, toolTip = "Makes the menu text low quality."},
                new ButtonInfo { buttonText = "Lowercase Mode", enableMethod =() => lowercaseMode = true, disableMethod =() => lowercaseMode = false, toolTip = "Makes the entire menu's text lowercase."},
                new ButtonInfo { buttonText = "Uppercase Mode", enableMethod =() => uppercaseMode = true, disableMethod =() => uppercaseMode = false, toolTip = "Makes the entire menu's text uppercase."},
                new ButtonInfo { buttonText = "Overflow Mode", enableMethod =() => NoAutoSizeText = true, disableMethod =() => NoAutoSizeText = false, toolTip = "Makes the entire menu's text overflow."},

                new ButtonInfo { buttonText = "Dynamic Animations", enableMethod =() => dynamicAnimations = true, disableMethod =() => dynamicAnimations = false, toolTip = "Adds more animations to the menu, giving you a better sense of control." },
                new ButtonInfo { buttonText = "Slow Dynamic Animations", enableMethod =() => slowDynamicAnimations = true, disableMethod =() => slowDynamicAnimations = false, toolTip = "Makes Dynamic Animations slower." },

                new ButtonInfo { buttonText = "Incremental Boost", enableMethod =() => incrementalBoost = true, disableMethod =() => incrementalBoost = false, toolTip = "Allows you to increment faster by holding down your <color=cyan>grip</color>." },

                new ButtonInfo { buttonText = "Custom Boards", enableMethod =() => CustomBoards = true, disableMethod =() => CustomBoards = false, enabled = CustomBoards, isTogglable = true, toolTip = "Enables the custom boards in stump." },
                new ButtonInfo { buttonText = "Move Stump Text Gun", method =() => Utility.MoveStumpTextGun(), isTogglable = true, toolTip = "Lets you move the stump text with a gun." },
                new ButtonInfo { buttonText = "Version Text", enableMethod =() => VersionText = true, disableMethod =() => VersionText = false, enabled = VersionText, toolTip = "Toggles the Version Text on the menu."},
                new ButtonInfo { buttonText = "FPS Text", enableMethod =() => showfps = true, disableMethod =() => showfps = false, enabled = showfps, toolTip = "Toggles the FPS Text on the menu."},
                new ButtonInfo { buttonText = "Stump Text", enableMethod =() => StumpText = true, disableMethod =() => StumpText = false, enabled = StumpText, toolTip = "Toggles the stump text."},

                new ButtonInfo { buttonText = "Disable Ghostview", enableMethod =() => disableGhostview = true, disableMethod =() => disableGhostview = false, toolTip = "Disables the transparent rig when you're in ghost."},
                new ButtonInfo { buttonText = "Legacy Ghostview", enableMethod =() => legacyGhostview = true, disableMethod =() => legacyGhostview = false, toolTip = "Reverts the transparent rig to the two balls when you're in ghost."},

                new ButtonInfo { buttonText = "Menu Presets", method =() => CurrentCategoryName = "Menu Presets", isTogglable = false, toolTip = "Opens the page of presets."},
                new ButtonInfo { buttonText = "Save Preferences", method =() => Utility.SaveSettings(), isTogglable = false, toolTip = "Saves your enabled mods to file." },
                new ButtonInfo { buttonText = "Load Preferences", method =() => Utility.LoadSettings(), isTogglable = false, toolTip = "Loads your saved mods from a file." },
            },

            new ButtonInfo[] { // Movement Settings
                new ButtonInfo { buttonText = "Exit Movement Settings", method =() => CurrentCategoryName = "Settings", isTogglable = false, toolTip = "Returns to the settings page of the menu." },

                new ButtonInfo { buttonText = "Change Fly Speed", method =() => Movement.ChangeFlySpeed(), enableMethod =() => Movement.ChangeFlySpeed(), disableMethod =() => Movement.ChangeFlySpeed(false), incremental = true, overlapText = "Change Fly Speed <color=grey>[<color=cyan>Very Slow</color>]</color>", isTogglable = false, toolTip = "Changes the current fly speed." },
                new ButtonInfo { buttonText = "Change Arm Length", method =() => Movement.ChangeArmLength(), enableMethod =() => Movement.ChangeArmLength(), disableMethod =() => Movement.ChangeArmLength(false), incremental = true, overlapText = "Change Arm Length <color=grey>[<color=cyan>Steam</color>]</color>", isTogglable = false, toolTip = "Changes your arm length." },
            },

            new ButtonInfo[] { // Gun Settings
                new ButtonInfo { buttonText = "Exit Gun Settings", method =() => CurrentCategoryName = "Settings", isTogglable = false, toolTip = "Returns to the settings page of the menu." },

                new ButtonInfo { buttonText = "Disable Gun Pointer", enableMethod =() => disableGunPointer = true, disableMethod =() => disableGunPointer = false, isTogglable = true, toolTip = "Disables the gun pointer." },
                new ButtonInfo { buttonText = "Small Gun Pointer", enableMethod =() => smallGunPointer = true, disableMethod =() => smallGunPointer = false, isTogglable = true, toolTip = "Makes the gun pointer smaller." },
                new ButtonInfo { buttonText = "Disable Gun Line", enableMethod =() => disableGunLine = true, disableMethod =() => disableGunLine = false, isTogglable = true, toolTip = "Disables the gun line." },
                new ButtonInfo { buttonText = "Swap Gun Hand", enableMethod =() => SwapGunHand = true, disableMethod =() => SwapGunHand = false, isTogglable = true, toolTip = "Swaps the hand of the gun is on." },
                new ButtonInfo { buttonText = "Gripless Guns", enableMethod =() => GriplessGuns = true, disableMethod =() => GriplessGuns = false, isTogglable = true, toolTip = "Makes the gun work without holding grip." },
                new ButtonInfo { buttonText = "Triggerless Guns", enableMethod =() => TriggerlessGuns = true, disableMethod =() => TriggerlessGuns = false, isTogglable = true, toolTip = "Makes the gun shoot without holding trigger." },

                new ButtonInfo { buttonText = "Change Gun Variation", overlapText = "Change Gun Variation <color=grey>[</color><color=cyan>Default</color><color=grey>]</color>", method =() => ChangeGunVariation(), enableMethod =() => ChangeGunVariation(), disableMethod =() => ChangeGunVariation(false), incremental = true, isTogglable = false, toolTip = "Changes the look of the gun."},
                new ButtonInfo { buttonText = "Change Gun Direction", overlapText = "Change Gun Direction <color=grey>[</color><color=cyan>Default</color><color=grey>]</color>", method =() => ChangeGunDirection(), enableMethod =() => ChangeGunDirection(), disableMethod =() => ChangeGunDirection(false), incremental = true, isTogglable = false, toolTip = "Changes the direction of the gun."},
            },

            new ButtonInfo[] { // Important
                new ButtonInfo { buttonText = "Exit Important", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Quit Game", method =() => Important.QuitGame(), isTogglable = false, toolTip = "Quits your game." },

                new ButtonInfo { buttonText = "Clear Notifcations", method =() => NotificationManager.ClearAllNotifications(), isTogglable = false, toolTip = "Clears all the notifications." },

                new ButtonInfo { buttonText = "Anti AFK", method =() => Important.AntiAFK(), isTogglable = true, toolTip = "Disables the afk kick you get." },
                new ButtonInfo { buttonText = "Disable Network Triggers", enableMethod =() => Important.DisableNetworkTriggers(true), disableMethod =() => Important.DisableNetworkTriggers(false), isTogglable = true, toolTip = "Disables the network triggers, so you can change maps without disconnecting." },
                new ButtonInfo { buttonText = "Disable Quit Box", enableMethod =() => QuitBoxPatch.enabled = true, disableMethod =() => QuitBoxPatch.enabled = false, isTogglable = true, toolTip = "Disables the box under the map that closes your game." },
                new ButtonInfo { buttonText = "Stump Quit Box", enableMethod =() => QuitBoxPatch.teleportToStump = true, disableMethod =() => QuitBoxPatch.teleportToStump = false, toolTip = "Disables the box under the map that closes your game."},

                new ButtonInfo { buttonText = "90 FPS", method =() => Important.CapFPS(90), toolTip = "Caps your FPS at 90 frames per second."},
                new ButtonInfo { buttonText = "72 FPS", method =() => Important.CapFPS(72), toolTip = "Caps your FPS at 72 frames per second."},
                new ButtonInfo { buttonText = "60 FPS", method =() => Important.CapFPS(60), toolTip = "Caps your FPS at 60 frames per second."},
                new ButtonInfo { buttonText = "45 FPS", method =() => Important.CapFPS(45), toolTip = "Caps your FPS at 45 frames per second."},
                new ButtonInfo { buttonText = "30 FPS", method =() => Important.CapFPS(30), toolTip = "Caps your FPS at 30 frames per second."},
                new ButtonInfo { buttonText = "15 FPS", method =() => Important.CapFPS(15), toolTip = "Caps your FPS at 15 frames per second."},
                new ButtonInfo { buttonText = "Unlock FPS", method = Important.UncapFPS, disableMethod =() => Application.targetFrameRate = 144, toolTip = "Unlocks your FPS."},

                new ButtonInfo { buttonText = "Connect to US", method =() => Important.ConnectToRegion("us"), toolTip = "Connects you to the United States servers."},
                new ButtonInfo { buttonText = "Connect to US West", method =() => Important.ConnectToRegion("usw"), toolTip = "Connects you to the western United States servers."},
                new ButtonInfo { buttonText = "Connect to EU", method =() => Important.ConnectToRegion("eu"), toolTip = "Connects you to the Europe servers."},

                new ButtonInfo { buttonText = "Turning", method =() => Important.Turning(), isTogglable = true, toolTip = "Lets you turn while moving your right joystick." },
            },

            new ButtonInfo[] { // Safety
                new ButtonInfo { buttonText = "Exit Safety", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Panic", method =() => Utility.Panic(), isTogglable = false, toolTip = "Disables every mod you have enabled." },

                new ButtonInfo { buttonText = "Flush RPCs", method = Utility.FlushRPCS, isTogglable = false, toolTip = "Flushes all RPC calls, good after you stop spamming." },

                new ButtonInfo { buttonText = "Anti Report <color=grey>[</color><color=cyan>DISCONNECT</color><color=grey>]</color>", method =() => Utility.BetaAntiReport(false, true), isTogglable = true, toolTip = "Disconnects you when someone is close to the report button." },
                new ButtonInfo { buttonText = "Anti Report <color=grey>[</color><color=cyan>CRASH</color><color=grey>]</color>", method =() => Utility.BetaAntiReport(true, false), isTogglable = true, toolTip = "Crashes the person who tries to report you when someone is close to the report button." },
                new ButtonInfo { buttonText = "Anti Moderator", method =() => Utility.BetaAntiCosmetic("LBAAK."), isTogglable = true, toolTip = "Disconnects you when someone has the moderator stick." },
                new ButtonInfo { buttonText = "Anti Admin", method =() => Utility.BetaAntiCosmetic("LBAAD."), isTogglable = true, toolTip = "Disconnects you when someone has the admin badge." },
                new ButtonInfo { buttonText = "Anti Finger Painter", method =() => Utility.BetaAntiCosmetic("LBADE."), isTogglable = true, toolTip = "Disconnects you when someone has the finger painter." },

                new ButtonInfo { buttonText = "Show Anti Cheat Reports <color=grey>[</color><color=cyan>Self</color><color=grey>]</color>", enableMethod =() => AntiCheatPatches.AntiCheatSelf = true, disableMethod =() => AntiCheatPatches.AntiCheatSelf = false, toolTip = "Gives you a notification every time you have been reported by the anti cheat."},
                new ButtonInfo { buttonText = "Show Anti Cheat Reports <color=grey>[</color><color=cyan>All</color><color=grey>]</color>", enableMethod =() => AntiCheatPatches.AntiCheatAll = true, disableMethod =() => AntiCheatPatches.AntiCheatAll = false, toolTip = "Gives you a notification every time anyone has been reported by the anti cheat."},
            },

            new ButtonInfo[] { // Computer
                new ButtonInfo { buttonText = "Exit Computer", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Disconnect", method =() => Important.Leave(), isTogglable = false, toolTip = "Disconnects you from the lobby." },
                new ButtonInfo { buttonText = "Reconnect", method =() => Important.Reconnect(), isTogglable = false, toolTip = "Reconnects you to the current lobby." },
                new ButtonInfo { buttonText = "Lobby Hop", method =() => Important.LobbyHop(), isTogglable = false, toolTip = "Lets you lobby hop." },
                new ButtonInfo { buttonText = "Join Random", method =() => Important.Jrr(), isTogglable = false, toolTip = "Lets you join a random room." },

                new ButtonInfo { buttonText = "Create Room", method =() => Important.CreateRoom(), isTogglable = false, toolTip = "Creates a custom room." },

                new ButtonInfo { buttonText = "Join Code '1'", method =() => Important.JoinCode("1"), isTogglable = false, toolTip = "Lets you join the code \"1\"." },
                new ButtonInfo { buttonText = "Join Code 'JUPITERX'", method =() => Important.JoinCode("_@-JupiterX-@_"), isTogglable = false, toolTip = "Lets you join the code \"JupiterX\"." },
                new ButtonInfo { buttonText = "Join Code 'MODS'", method =() => Important.JoinCode("MODS"), isTogglable = false, toolTip = "Lets you join the code \"MODS\"." },
                new ButtonInfo { buttonText = "Join Code 'MOD'", method =() => Important.JoinCode("MOD"), isTogglable = false, toolTip = "Lets you join the code \"MOD\"." },
                new ButtonInfo { buttonText = "Join Code 'PBBV'", method =() => Important.JoinCode("PBBV"), isTogglable = false, toolTip = "Lets you join the code \"PBBV\"." },
                new ButtonInfo { buttonText = "Join Code 'DAISY'", method =() => Important.JoinCode("DAISY"), isTogglable = false, toolTip = "Lets you join the code \"DAISY\"." },
            },

            new ButtonInfo[] { // Movement
                new ButtonInfo { buttonText = "Exit Movement", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Platforms <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Movement.Platforms(), isTogglable = true, toolTip = "Lets you walk on air while holding grip." },
                new ButtonInfo { buttonText = "Trigger Platforms <color=grey>[</color><color=cyan>T</color><color=grey>]</color>", method =() => Movement.TriggerPlatforms(), isTogglable = true, toolTip = "Lets you walk on air while holding trigger." },
                new ButtonInfo { buttonText = "Invisable Platforms <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Movement.InvisablePlatforms(), isTogglable = true, toolTip = "Lets you walk on air while holding grip." },
                new ButtonInfo { buttonText = "Frozone <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Movement.Frozone(), isTogglable = true, toolTip = "Spawns slippery blocks under your hands using <color=cyan>grip</color>." },
                new ButtonInfo { buttonText = "Platform Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method = Movement.PlatformSpam, toolTip = "Spawns legacy platforms rapidly at your hand for those who have networked platforms."},
                new ButtonInfo { buttonText = "Platform Gun", method = Movement.PlatformGun, toolTip = "Spawns legacy platforms rapidly wherever your hand desires for those who have networked platforms."},

                new ButtonInfo { buttonText = "Fly <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", method =() => Movement.Fly(), isTogglable = true, toolTip = "Lets you fly while holding your right primary." },
                new ButtonInfo { buttonText = "Trigger Fly <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Movement.TriggerFly(), isTogglable = true, toolTip = "Lets you fly while holding your right trigger." },
                new ButtonInfo { buttonText = "Noclip Fly <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", method =() => Movement.NoClipFly(), isTogglable = true, toolTip = "Lets you fly while holding your right trigger." },
                new ButtonInfo { buttonText = "Bark Fly <color=grey>[</color><color=cyan>J</color><color=grey>]</color>", method = Movement.BarkFly, toolTip = "Acts like the fly that Bark has. Credits to KyleTheScientist."},
                new ButtonInfo { buttonText = "Excel Fly", method =() => Movement.ExcelFly(), isTogglable = true, toolTip = "Lets you fly like iron man."  },
                new ButtonInfo { buttonText = "Slingshot Fly <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", method =() => Movement.SlingShotFly(), isTogglable = true, toolTip = "Lets you fly like a slingshot while hold your right primary." },
                new ButtonInfo { buttonText = "Fly Towards Gun", method = Movement.FlyTowardsGun, toolTip = "Sends your character towards whoever your hand desires."},

                new ButtonInfo { buttonText = "Dash <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", method = Movement.Dash, toolTip = "Flings your character forwards when pressing <color=cyan>A</color>."},
                new ButtonInfo { buttonText = "Reverse Velocity <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", method = Movement.ReverseVelocity, toolTip = "Reverses your current velocity when you press <color=cyan>A</color>."},
                new ButtonInfo { buttonText = "Bird Fly", method = Movement.BirdFly, toolTip = "Makes you fly like a bird when you flap your wings."},

                new ButtonInfo { buttonText = "Drive <color=grey>[</color><color=cyan>J</color><color=grey>]</color>", method = Movement.Drive, toolTip = "Lets you drive around in your invisible car. Use the <color=cyan>joystick</color> to move."},
                new ButtonInfo { buttonText = "Hard Drive <color=grey>[</color><color=cyan>J</color><color=grey>]</color>", overlapText = "Sticky Drive <color=grey>[</color><color=cyan>J</color><color=grey>]</color>", method = Movement.HardDrive, toolTip = "Similar to drive, but locks you to the ground."},

                new ButtonInfo { buttonText = "Long Arms", method =() => Movement.LongArms(false), disableMethod =() => Movement.LongArms(true), isTogglable = true, toolTip = "Gives you long arms." },

                new ButtonInfo { buttonText = "No Tag Freeze", method =() => Movement.NoTagFreeze(0), isTogglable = true, toolTip = "Removes the tag freeze you get when you get tagged." },
                new ButtonInfo { buttonText = "Force Tag Freeze", method =() => Movement.NoTagFreeze(1), disableMethod =() => Movement.NoTagFreeze(0),  isTogglable = true, toolTip = "Lets you act like you have tag freeze." },

                new ButtonInfo { buttonText = "NoClip <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Movement.NoClip(Utility.RightTrigger), isTogglable = true, toolTip = "Removes object colliders when you hold right trigger." },
                new ButtonInfo { buttonText = "Up And Down", method = Movement.UpAndDown, toolTip = "Makes you go up when holding your <color=cyan>trigger</color>, and down when holding your <color=cyan>grip</color>."},
                new ButtonInfo { buttonText = "Left And Right", method = Movement.LeftAndRight, toolTip = "Makes you go left when holding your <color=cyan>trigger</color>, and right when holding your <color=cyan>grip</color>."},
                new ButtonInfo { buttonText = "Forwards And Backwards", method = Movement.ForwardsAndBackwards, toolTip = "Makes you go forwards when holding your <color=cyan>trigger</color>, and backwards when holding your <color=cyan>grip</color>."},

                new ButtonInfo { buttonText = "Long Jump <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", overlapText = "Playspace Abuse <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", method = Movement.PlayspaceAbuse, toolTip = "Makes you look like you're legitimately long jumping when holding <color=cyan>A</color>."},

                new ButtonInfo { buttonText = "SpeedBoost", method =() => Movement.Speedboost(), isTogglable = true, toolTip = "Gives you a speed boost." },
                new ButtonInfo { buttonText = "Mosa Boost", method =() => Movement.Mosaboost(), isTogglable = true, toolTip = "Gives you a slight speed boost." },

                new ButtonInfo { buttonText = "TP Gun", method =() => Movement.TPGun(), isTogglable = true, toolTip = "Lets you teleport with a gun."  },
                new ButtonInfo { buttonText = "Car Monke <color=grey>[</color><color=cyan>T</color><color=grey>]</color>", method =() => Movement.CarMonke(), isTogglable = true, toolTip = "Drive around while holding your triggers." },
                new ButtonInfo { buttonText = "Follow Player Gun", method =() => Movement.FollowPlayerGun(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Lets you follow someone with a gun." },

                new ButtonInfo { buttonText = "Low Gravity", method = Movement.LowGravity, toolTip = "Makes gravity lower on your character."},
                new ButtonInfo { buttonText = "Zero Gravity", method = Movement.ZeroGravity, toolTip = "Disables gravity on your character."},
                new ButtonInfo { buttonText = "High Gravity", method = Movement.HighGravity, toolTip = "Makes gravity higher on your character."},
                new ButtonInfo { buttonText = "Reverse Gravity", method = Movement.ReverseGravity, disableMethod = Movement.UnflipCharacter, toolTip = "Reverses gravity on your character."},

                new ButtonInfo { buttonText = "Wall Walk <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method = Movement.WallWalk, toolTip = "Makes you get brought towards any wall you touch when holding <color=cyan>grip</color>."},

                new ButtonInfo { buttonText = "Checkpoint <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Movement.Checkpoint(), isTogglable = true, toolTip = "Lets you a place a checkpoint to go back to later." },
                new ButtonInfo { buttonText = "C4 <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Movement.C4(), isTogglable = true, toolTip = "Lets you a place a checkpoint to go back to later." },

                new ButtonInfo { buttonText = "Disable Body Collider", method =() => GorillaTagger.Instance.bodyCollider.enabled = false, disableMethod =() => GorillaTagger.Instance.bodyCollider.enabled = true, toolTip = "Disables your body's collider."},
                new ButtonInfo { buttonText = "Disable Head Collider", method =() => GorillaTagger.Instance.headCollider.enabled = false, disableMethod =() => GorillaTagger.Instance.headCollider.enabled = true, toolTip = "Disables your head's collider."},
            },

            new ButtonInfo[] { // Advantage
                new ButtonInfo { buttonText = "Exit Advantage", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Tag All", method =() => Advantage.TagAll(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Lets you tag everyone in the lobby." },
                new ButtonInfo { buttonText = "Tag Aura", method =() => Advantage.TagAura(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Lets you tag someone when they come close to you." },

                new ButtonInfo { buttonText = "Tag Gun", method =() => Advantage.TagGun(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Lets you tag someone with a gun." },
                new ButtonInfo { buttonText = "Flick Tag Gun", method =() => Advantage.FlickTagGun(), isTogglable = true, toolTip = "Lets you flick tag someone with a gun." },
            },

            new ButtonInfo[] { // VRRig
                new ButtonInfo { buttonText = "Exit VRRig", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Ghost Monke <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", method =() => vRRig.GhostMonke(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Lets you become a ghost." },
                new ButtonInfo { buttonText = "Invis Monke <color=grey>[</color><color=cyan>B</color><color=grey>]</color>", method =() => vRRig.InvisMonke(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Lets you become invisable." },
                new ButtonInfo { buttonText = "Grab Rig <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => vRRig.GrabRig(), disableMethod = Utility.FixGhostRig, isTogglable = true, toolTip = "Lets you grab your rig while holding right grip." },
                new ButtonInfo { buttonText = "Spaz Rig", method =() => vRRig.SpazRig(), isTogglable = true, toolTip = "Makes your rig spazz out." },
                new ButtonInfo { buttonText = "Strobe", method =() => vRRig.Strobe(), isTogglable = true, toolTip = "Makes your color go crazy." },
                new ButtonInfo { buttonText = "Move Rig Gun", method =() => vRRig.MoveRigGun(), isTogglable = true, toolTip = "Moves your rig to the gun point." },
                new ButtonInfo { buttonText = "Bees", method =() => vRRig.Bees(), disableMethod =() => Utility.FixGhostRig(), isTogglable = true, toolTip = "Makes your rig teleport to other players." },

                new ButtonInfo { buttonText = "Laggy Rig", method = Movement.LaggyRig, disableMethod = Utility.FixGhostRig, toolTip = "Makes your rig laggy."},
                new ButtonInfo { buttonText = "Smooth Rig", method =() => PhotonNetwork.SerializationRate = 30, disableMethod =() => PhotonNetwork.SerializationRate = 10, toolTip = "Makes your rig really smooth."},
                new ButtonInfo { buttonText = "Update Rig <color=grey>[</color><color=cyan>A</color><color=grey>]</color>", method = Movement.UpdateRig, disableMethod = Utility.FixGhostRig, toolTip = "Freezes your rig in place. Whenever you click <color=cyan>A</color>, your rig will update."},

                new ButtonInfo { buttonText = "Decapitation", enableMethod =() => VRRigTorsoPatch.enable = true, method = vRRig.DecapitateRig, disableMethod =() => VRRigTorsoPatch.enable = false, toolTip = "Desyncs your head and body rotations."},

                new ButtonInfo { buttonText = "Paralyze Rig", method = vRRig.ParalyzeRig, disableMethod =() => Utility.ActualRig().enabled = true, toolTip = "Removes your arms from your rig."},
                new ButtonInfo { buttonText = "Chicken Rig", method = vRRig.ChickenRig, disableMethod =() => Utility.ActualRig().enabled = true, toolTip = "Makes your rig look like a chicken."},
                new ButtonInfo { buttonText = "Amputate Rig", method = vRRig.AmputateRig, disableMethod =() => Utility.ActualRig().enabled = true, toolTip = "Removes all of your limbs from your rig."},
            },

            new ButtonInfo[] { // Visual
                new ButtonInfo { buttonText = "Exit Visual", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Chams", method =() => Visual.Chams(true), disableMethod =() => Visual.Chams(false), isTogglable = true, toolTip = "Lets you see players through walls." },

                new ButtonInfo { buttonText = "Tracers", method =() => Visual.Tracers(), disableMethod =() => Visual.CleanUpTracers(), isTogglable = true, toolTip = "Points lines at other players." },
                new ButtonInfo { buttonText = "Box ESP", method =() => Visual.BoxESP(), disableMethod =() => Visual.DisableBoxESP(), isTogglable = true, toolTip = "Lets you see players through walls." },
                new ButtonInfo { buttonText = "Capsule ESP", method =() => Visual.CapsuleESP(), disableMethod =() => Visual.DisableCapsuleESP(), isTogglable = true, toolTip = "Lets you see players through walls." },
                new ButtonInfo { buttonText = "Sphere ESP", method =() => Visual.SphereESP(), disableMethod =() => Visual.DisableSphereESP(), isTogglable = true, toolTip = "Lets you see players through walls." },

                new ButtonInfo { buttonText = "Name Tags", method =() => Visual.NameTags(), disableMethod =() => Visual.DisableNameTags(), isTogglable = true, toolTip = "Gives players name tags above their heads that show their nickname." },
                new ButtonInfo { buttonText = "ID Name Tags", method =() => Visual.IDNameTags(), disableMethod =() => Visual.DisableIDNameTags(), isTogglable = true, toolTip = "Gives players name tags above their heads that show their ID." },
                new ButtonInfo { buttonText = "Platform Name Tags", method =() => Visual.PlatformTags(), disableMethod =() => Visual.DisablePlatformNameTags(), isTogglable = true, toolTip = "Gives players name tags above their heads that show their Platform." },
                new ButtonInfo { buttonText = "Master Name Tags", method =() => Visual.MasterTags(), disableMethod =() => Visual.DisableMasterNameTags(), isTogglable = true, toolTip = "Gives players name tags above their heads that show if their Master Client." },
                new ButtonInfo { buttonText = "Tagged Name Tags", method =() => Visual.TaggedTags(), disableMethod =() => Visual.DisableTaggedNameTags(), isTogglable = true, toolTip = "Gives players name tags above their heads that show their Tagged." },

                new ButtonInfo { buttonText = "Velocity Label", method = Visual.VelocityLabel, disableMethod =() => Visual.RemoveLabel(0), toolTip = "Puts text on your right hand, showing your velocity."},
                new ButtonInfo { buttonText = "Nearby Label", method = Visual.NearbyTaggerLabel, disableMethod =() => Visual.RemoveLabel(1), toolTip = "Puts text on your left hand, showing you the distance of the nearest tagger."},
                new ButtonInfo { buttonText = "Last Label", method = Visual.LastLabel, disableMethod =() => Visual.RemoveLabel(2), toolTip = "Puts text on your left hand, showing you how many untagged people are left."},
                new ButtonInfo { buttonText = "Time Label", method = Visual.TimeLabel, disableMethod =() => Visual.RemoveLabel(3), toolTip = "Puts text on your right hand, showing how long you've been playing for without getting tagged."},

                new ButtonInfo { buttonText = "FPS Overlay", method =() => NotificationManager.information["FPS"] = Utility.lastDeltaTime.ToString(), disableMethod =() => NotificationManager.information.Remove("FPS"), toolTip = "Displays your FPS on your screen."},
                new ButtonInfo { buttonText = "Ping Overlay", method = Utility.PingOverlay, disableMethod =() => NotificationManager.information.Remove("Ping"), toolTip = "Displays the server's ping on your screen."},
                new ButtonInfo { buttonText = "Time Overlay", method =() => NotificationManager.information["Time"] = DateTime.Now.ToString("hh:mm tt"), disableMethod =() => NotificationManager.information.Remove("Time"), toolTip = "Displays your current time on your screen."},
                new ButtonInfo { buttonText = "Velocity Overlay", method =() => NotificationManager.information["Velocity"] = $"{GorillaTagger.Instance.bodyCollider.attachedRigidbody.velocity.magnitude:F1}m/s", disableMethod =() => NotificationManager.information.Remove("Velocity"), toolTip = "Displays your velocity on your screen."},
                new ButtonInfo { buttonText = "Nearby Overlay", method = Utility.NearbyTaggerOverlay, disableMethod =() => NotificationManager.information.Remove("Nearby"), toolTip = "Displays the distance to the nearest tagger/target on your screen."},
                new ButtonInfo { buttonText = "Room Information Overlay", method =() => { if (PhotonNetwork.InRoom) { NotificationManager.information["Room Code"] = PhotonNetwork.CurrentRoom.Name; NotificationManager.information["Players"] = PhotonNetwork.PlayerList.Length.ToString(); } else { NotificationManager.information.Remove("Room Code"); NotificationManager.information.Remove("Players"); } }, disableMethod =() => { NotificationManager.information.Remove("Room Code"); NotificationManager.information.Remove("Players"); }, toolTip = "Displays information about the room on your screen."},

                new ButtonInfo { buttonText = "Draw Gun", method = Visual.DrawGun, disableMethod = Visual.DisableDrawGun, toolTip = "Lets you draw on whatever your hand desires." },

                new ButtonInfo { buttonText = "FPS Boost", enableMethod =() => QualitySettings.masterTextureLimit = int.MaxValue, disableMethod =() => QualitySettings.masterTextureLimit = 1, toolTip = "Makes everything low quality in an attempt to boost your FPS."},
                new ButtonInfo { buttonText = "Full Bright", method =() => Visual.FullBright(), disableMethod =() => Visual.DisableFullBright(), isTogglable = true, toolTip = "Lets you see in the dark." },

                new ButtonInfo { buttonText = "Disable Rig Lerping", method = Visual.NoSmoothRigs, disableMethod = Visual.ReSmoothRigs, toolTip = "Disable the smoothing on the other player's rigs."},

                new ButtonInfo { buttonText = "X-Ray <color=grey>[</color><color=cyan>T</color><color=grey>]</color>", method = Visual.Xray, toolTip = "Lets you see through objects when holding <color=cyan>trigger</color>."},
            },

            new ButtonInfo[] { // Fun
                new ButtonInfo { buttonText = "Exit Fun", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Spin Head X", method =() => Fun.SpinHead("x"), disableMethod = Fun.FixHead, toolTip = "Spins your head on the X axis."},
                new ButtonInfo { buttonText = "Spin Head Y", method =() => Fun.SpinHead("y"), disableMethod = Fun.FixHead, toolTip = "Spins your head on the Y axis."},
                new ButtonInfo { buttonText = "Spin Head Z", method =() => Fun.SpinHead("z"), disableMethod = Fun.FixHead, toolTip = "Spins your head on the Z axis."},

                new ButtonInfo { buttonText = "Upside Down Head", method = Fun.UpsideDownHead, disableMethod = Fun.FixHead, toolTip = "Flips your head upside down on the Z axis."},
                new ButtonInfo { buttonText = "Backwards Head", method = Fun.BackwardsHead, disableMethod = Fun.FixHead, toolTip = "Rotates your head 180 degrees on the Y axis."},
                new ButtonInfo { buttonText = "Sideways Head", method = Fun.SidewaysHead, disableMethod = Fun.FixHead, toolTip = "Rotates your head 90 degrees on the Y axis."},

                new ButtonInfo { buttonText = "Broken Neck", method = Fun.BrokenNeck, disableMethod = Fun.FixHead, toolTip = "Rotates your head 90 degrees on the Z axis."},

                new ButtonInfo { buttonText = "No Tap Cooldown", method =() => Fun.NoTapCooldown(), disableMethod =() => Fun.ResetTapcooldown(), isTogglable = true, toolTip = "Removes the hand tap cooldown." },
                new ButtonInfo { buttonText = "Loud Hand Taps", method =() => Fun.LoadHandTaps(), disableMethod =() => Fun.FixHandTaps(), isTogglable = true, toolTip = "Makes the hand tap sound very loud." },
                new ButtonInfo { buttonText = "Silent Hand Taps", method =() => Fun.SilentHandTaps(), disableMethod =() => Fun.FixHandTaps(), isTogglable = true, toolTip = "Removes the hand tap sound." },

                new ButtonInfo { buttonText = "Auto Clicker <color=grey>[</color><color=cyan>T</color><color=grey>]</color>", method =() => Fun.AutoClicker(), isTogglable = true, toolTip = "Automatically presses trigger for you when holding <color=cyan>trigger</color>." },

                new ButtonInfo { buttonText = "Mute Gun", method =() => Fun.MuteGun(), isTogglable = true, toolTip = "Lets you mute the player you shoot the gun at." },
                new ButtonInfo { buttonText = "Mute All", method =() => Fun.MuteAll(), isTogglable = false, toolTip = "Lets you mute everyone in the current room." },

                new ButtonInfo { buttonText = "UnMute Gun", method =() => Fun.UnMuteGun(), isTogglable = true, toolTip = "Lets you unmute the player you shoot the gun at." },
                new ButtonInfo { buttonText = "UnMute All", method =() => Fun.UnMuteAll(), isTogglable = false, toolTip = "Lets you unmute everyone in the current room." },

                new ButtonInfo { buttonText = "Report Gun", method =() => Fun.ReportGun(), isTogglable = false, toolTip = "Reports the person who you shoot at for cheating." },
                new ButtonInfo { buttonText = "Report All", method =() => Fun.ReportAll(), isTogglable = false, toolTip = "Reports everyone in the lobby for cheating." },

                new ButtonInfo { buttonText = "Fix Mic", method =() => Fun.FixMic(), isTogglable = false, toolTip = "Fixes your mic."  },
                new ButtonInfo { buttonText = "Low Quality Mic", method =() => Fun.LowQualityMic(), isTogglable = false, toolTip = "Makes your micrphone low quality." },
                new ButtonInfo { buttonText = "High Quality Mic", method =() => Fun.HighQualityMic(), isTogglable = false, toolTip = "Makes your micrphone high quality." },

                new ButtonInfo { buttonText = "Bass Boosted Mic", method =() => Fun.BassBoostMic(), isTogglable = false, toolTip = "Makes your micrphone bass boosted." },
                new ButtonInfo { buttonText = "Extreme Bass Boosted Mic", method =() => Fun.BassBoostMicExtreme(), isTogglable = false, toolTip = "Makes your micrphone hextremely bass boosted." },

                new ButtonInfo { buttonText = "Get ID Self", method =() => Fun.GetIdSelf(), isTogglable = false, toolTip = "Gets your own userid and writes it to a file." },
                new ButtonInfo { buttonText = "Get ID Gun", method =() => Fun.GetIdGun(), isTogglable = true, toolTip = "Gets the person you shoot userid and writes it to a file." },
                new ButtonInfo { buttonText = "Get ID All", method =() => Fun.GetIdAll(), isTogglable = false, toolTip = "Gets everyone in the room userid and writes it to a file." },

                new ButtonInfo { buttonText = "Grab Player Info", method =() => Fun.GrabRoomInfo(), isTogglable = false, toolTip = "Gets the room info and writes it to a file." },

                new ButtonInfo { buttonText = "Open Basement Door", method =() => Fun.ChangeDoorState(GTDoor.DoorState.Opening), isTogglable = false, toolTip = "Gets the room info and writes it to a file." },
                new ButtonInfo { buttonText = "Close Basement Door", method =() => Fun.ChangeDoorState(GTDoor.DoorState.Closed), isTogglable = false, toolTip = "Gets the room info and writes it to a file." },
            },

            new ButtonInfo[] { // Name
                new ButtonInfo { buttonText = "Exit Name", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Menu Name Tag", method =() => Name.MenuNameTag(), isTogglable = true, toolTip = "Sets your name to the menu name." },
                new ButtonInfo { buttonText = "Rainbow Owner Name", method =() => Name.ChangeNameSpaz("\nOwner", new string[] { "red", "yellow", "cyan", "blue", "magenta", "lime", "green" }), isTogglable = true, toolTip = "Sets your name as Owner with rainbow text." },
                new ButtonInfo { buttonText = "Owner Name", method =() => Name.ChangeName("\nOwner", "white"), isTogglable = true, toolTip = "Sets your name as Owner." },
                new ButtonInfo { buttonText = "Moderator Name", method =() => Name.ChangeName("\nModerator", "white"), isTogglable = true, toolTip = "Sets your name as Moderator." },
                new ButtonInfo { buttonText = "BSU Skids Name", method =() => Name.ChangeName("BSU Menu is skidded\nBSU Skids", "blue"), isTogglable = true, toolTip = "Sets your name as BSU Skids." },
                new ButtonInfo { buttonText = "PBBV Name", method =() => Name.ChangeName("\nPBBV", "white"), isTogglable = true, toolTip = "Sets your name as PBBV." },
                new ButtonInfo { buttonText = "ECHO Name", method =() => Name.ChangeName("\nECHO", "white"), isTogglable = true, toolTip = "Sets your name as ECHO." },
                new ButtonInfo { buttonText = "DAISY09 Name", method =() => Name.ChangeName("\nDAISY09", "white"), isTogglable = true, toolTip = "Sets your name as DAISY09." },
                new ButtonInfo { buttonText = "Custom Name", method =() => Name.CustomName(), isTogglable = true, toolTip = "Makes your name be the custom name you set in a file." },

                new ButtonInfo { buttonText = "No Name", method =() => Name.ChangeName("\n\n", "white"), isTogglable = true, toolTip = "Makes you have no name."  },
                new ButtonInfo { buttonText = "Emoji Name (1)", method =() => Utility.BetaEmojiName(0), isTogglable = true, toolTip = "Sets your name as a emoji."  },
                new ButtonInfo { buttonText = "Emoji Name (2)", method =() => Utility.BetaEmojiName(1), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (3)", method =() => Utility.BetaEmojiName(2), isTogglable = true , toolTip = "Sets your name as a emoji."},
                new ButtonInfo { buttonText = "Emoji Name (4)", method =() => Utility.BetaEmojiName(3), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (5)", method =() => Utility.BetaEmojiName(4), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (6)", method =() => Utility.BetaEmojiName(5), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (7)", method =() => Utility.BetaEmojiName(6), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (8)", method =() => Utility.BetaEmojiName(7), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (9)", method =() => Utility.BetaEmojiName(8), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (10)", method =() => Utility.BetaEmojiName(9), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (11)", method =() => Utility.BetaEmojiName(10), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (12)", method =() => Utility.BetaEmojiName(11), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (13)", method =() => Utility.BetaEmojiName(12), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (14)", method =() => Utility.BetaEmojiName(13), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (15)", method =() => Utility.BetaEmojiName(14), isTogglable = true, toolTip = "Sets your name as a emoji." },
                new ButtonInfo { buttonText = "Emoji Name (16)", method =() => Utility.BetaEmojiName(15), isTogglable = true, toolTip = "Sets your name as a emoji." },
            },

            new ButtonInfo[] { // Prefabs
                new ButtonInfo { buttonText = "Exit Prefabs", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Get Fucked Spawn <color=grey>[</color><color=cyan>FOREST</color><color=grey>]</color>", method =() => Experimental.GetFucked(), isTogglable = false, toolTip = "Spawns the word 'Get Fucked' using stickable targets in forest." },
                new ButtonInfo { buttonText = "Clear Prefabs", method =() => Prefabs.ClearPrefabs(), isTogglable = false, toolTip = "Clears every prefab with a photon view." },

                new ButtonInfo { buttonText = "Cube All <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Fun.CubeAll(), isTogglable = true, toolTip = "Lets you spawn cubes at others while holding right trigger." },

                new ButtonInfo { buttonText = "Cube Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Prefabs.CubeSpam(), isTogglable = true, toolTip = "Lets you spam cubes while holding grip." },
                new ButtonInfo { buttonText = "Give Cube Spam Gun", method =() => Prefabs.GiveSpamGun(0), isTogglable = true, toolTip = "Lets you give someone cube spam when they hold grip." },
                new ButtonInfo { buttonText = "Target Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Prefabs.TargetSpam(), isTogglable = true, toolTip = "Lets you spam stickable targets while holding grip."  },
                new ButtonInfo { buttonText = "Give Target Spam Gun", method =() => Prefabs.GiveSpamGun(1), isTogglable = true, toolTip = "Lets you give someone stickable target spam when they hold grip."  },
                new ButtonInfo { buttonText = "Network Player Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Prefabs.NetworkPlayerSpam(), isTogglable = true, toolTip = "Lets you spam network players when holding grip." },
                new ButtonInfo { buttonText = "Give Network Player Spam Gun", method =() => Prefabs.GiveSpamGun(2), isTogglable = true, toolTip = "Lets you give someone network player spam when they are holding grip." },
                new ButtonInfo { buttonText = "Enemy Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Prefabs.EnemySpam(), isTogglable = true, toolTip = "Lets you spam enemys when holding grip." },
                new ButtonInfo { buttonText = "Give Enemy Spam Gun", method =() => Prefabs.GiveSpamGun(3), isTogglable = true, toolTip = "Lets you give someone enemy spam when they are holding grip." },
                new ButtonInfo { buttonText = "Scoreboard Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Prefabs.SpamScoreboard(), isTogglable = true, toolTip = "Lets you spam enemys when holding grip." },

                new ButtonInfo { buttonText = "Cube Gun", method =() => Prefabs.CubeGun(), isTogglable = true, toolTip = "Lets you shoot cubes with a gun." },
                new ButtonInfo { buttonText = "Target Gun", method =() => Prefabs.TargetGun(), isTogglable = true, toolTip = "Lets you shoot targets with a gun." },
                new ButtonInfo { buttonText = "Network Player Gun", method =() => Prefabs.NetworkPlayerGun(), isTogglable = true, toolTip = "Lets you shoot network players with a gun." },
                new ButtonInfo { buttonText = "Enemy Gun", method =() => Prefabs.EnemyGun(), isTogglable = true, toolTip = "Lets you shoot enemys with a gun." },
                new ButtonInfo { buttonText = "Scoreboard Gun", method =() => Prefabs.ScoreboardGun(), isTogglable = true, toolTip = "Lets you shoot scoreboard with a gun." },

                new ButtonInfo { buttonText = "Cube Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Prefabs.PrefabLuancher("bulletPrefab"), isTogglable = true, toolTip = "Lets you launch cubes while holding grips." },
                new ButtonInfo { buttonText = "Target Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Prefabs.PrefabLuancher("STICKABLE TARGET"), isTogglable = true, toolTip = "Lets you launch targets while holding grips." },
                new ButtonInfo { buttonText = "Target Dick Spawner <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Utility.DickSpawn(), isTogglable = true, toolTip = "Lets you spawn a dick out of targets." },
            },

            new ButtonInfo[] { // Overpowered
                new ButtonInfo { buttonText = "Exit Overpowered", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "MasterLabel", label = true },

                new ButtonInfo { buttonText = "Cosmetix", method =() => Utility.UnlockAll(), isTogglable = false, toolTip = "Gives you every cosmetic in the game." },

                new ButtonInfo { buttonText = "Unban Self", method =() => Experimental.Unban(), isTogglable = false, toolTip = "Attempts to login you into a new playfab account if your main gets banned." },
                new ButtonInfo { buttonText = "Set Master", method =() => Utility.MakeMeMaster(), isTogglable = true, toolTip = "Sets you as master client." },
                new ButtonInfo { buttonText = "Always Set Master", method =() => Overpowered.AlawysMaster(), isTogglable = true, toolTip = "Always sets you as master no matter who tries to take it." },

                new ButtonInfo { buttonText = "Rig Spam <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Overpowered.RigSpam(), isTogglable = true, toolTip = "Lets you spam rigs." },

                new ButtonInfo { buttonText = "Material Spam All", method =() => Overpowered.MatSpamAll(), isTogglable = true, toolTip = "Lets you spaz out the infection material on others." },
                new ButtonInfo { buttonText = "Material Spam Gun", method =() => Overpowered.MatSpamGun(), isTogglable = true, toolTip = "Lets you spaz out the infection material on who you shoot." },

                new ButtonInfo { buttonText = "Kick Gun <color=grey>[</color><color=cyan>Stump</color><color=grey>]</color>", method =() => Overpowered.KickGun(), isTogglable = true, toolTip = "Lets you kick someone with a gun in stump." },
                new ButtonInfo { buttonText = "Kick All <color=grey>[</color><color=cyan>Stump</color><color=grey>]</color>", method =() => Overpowered.KickAll(), isTogglable = true, toolTip = "Lets you kick everyone in stump." },

                new ButtonInfo { buttonText = "Disable Network Triggers <color=grey>[</color><color=cyan>SS</color><color=grey>]</color>", method =() => Overpowered.DisableNetworkTriggersSS(), isTogglable = false, toolTip = "Disables the network triggers, allowing people to join every map without getting disconnected." },
                new ButtonInfo { buttonText = "Enable Network Triggers <color=grey>[</color><color=cyan>SS</color><color=grey>]</color>", method =() => Overpowered.EnableNetworkTriggersSS(), isTogglable = false, toolTip = "Enables the network triggers, making people get disconnected when joining new maps." },

                new ButtonInfo { buttonText = "Create Symbol Name Public", method =() => Overpowered.CreatePublic("<>{][]()@.,/?!"), isTogglable = true, toolTip = "Creates a public room with a symbols as the name." },
                new ButtonInfo { buttonText = "Create Dot Name Public", method =() => Overpowered.CreatePublic("."), isTogglable = true, toolTip = "Creates a public room with a dot name." },
                new ButtonInfo { buttonText = "Create Short Name Public", method =() => Overpowered.CreatePublic("1"), isTogglable = true, toolTip = "Creates a public room with a short name." },
                new ButtonInfo { buttonText = "Create Long Name Public", method =() => Overpowered.LongNamePub(), isTogglable = true, toolTip = "Creates a public room with a long name." },
                new ButtonInfo { buttonText = "Create Modded Public", method =() => Overpowered.CreatePublic("\n\n\n\nMODDED\n\n\n\n"), isTogglable = true, toolTip = "Creates a public room that is modded." },
                new ButtonInfo { buttonText = "Create JupiterX Public", method =() => Overpowered.CreatePublic("\n\nJupiterX on top\n\n\n"), isTogglable = true, toolTip = "Creates a public room that says jupiterx on top." },

                new ButtonInfo { buttonText = "Create 255 Player Room", method =() => Overpowered.CreatePublic(255), isTogglable = true, toolTip = "Creates a public room that has max players allowed." },
                new ButtonInfo { buttonText = "Create 1 Player Room", method =() => Overpowered.CreatePublic(1), isTogglable = true, toolTip = "Creates a public room that has 1 players allowed." },
                new ButtonInfo { buttonText = "Create 5 Player Room", method =() => Overpowered.CreatePublic(5), isTogglable = true, toolTip = "Creates a public room that has 5 players allowed." },
                new ButtonInfo { buttonText = "Create 50 Player Room", method =() => Overpowered.CreatePublic(50), isTogglable = true, toolTip = "Creates a public room that has 50 players allowed." },
                new ButtonInfo { buttonText = "Create 100 Player Room", method =() => Overpowered.CreatePublic(100), isTogglable = true, toolTip = "Creates a public room that has 100 players allowed." },

                new ButtonInfo { buttonText = "Crash Gun", method =() => Overpowered.CrashGun(), isTogglable = true, toolTip = "Lets you crash someone you shoot at." },
                new ButtonInfo { buttonText = "Crash Gun V2", method =() => Overpowered.CrashGunV2(), isTogglable = true, toolTip = "Lets you crash someone you shoot at." },
                new ButtonInfo { buttonText = "Crash Gun V3", overlapText = "Insta Crash Gun", method =() => Overpowered.CrashGunV3(), isTogglable = true, toolTip = "Lets you crash someone you shoot at." },
                new ButtonInfo { buttonText = "Crash Gun V4", method =() => Overpowered.CrashGunV4(), isTogglable = true, toolTip = "Lets you crash someone you shoot at." },
                new ButtonInfo { buttonText = "Crash Gun V5", method =() => Overpowered.CrashGunV5(), isTogglable = true, toolTip = "Lets you crash someone you shoot at." },
                new ButtonInfo { buttonText = "Crash Gun V6", method =() => Overpowered.ActualInstaCrashGun(), isTogglable = true, toolTip = "Lets you crash someone you shoot at." },

                new ButtonInfo { buttonText = "Crash All <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Overpowered.CrashAll(), isTogglable = true, toolTip = "Lets you crash all while holding right trigger." },
                new ButtonInfo { buttonText = "Crash All V2 <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Overpowered.CrashAllV2(), isTogglable = true, toolTip = "Lets you crash all while holding right trigger." },
                new ButtonInfo { buttonText = "Crash All V3 <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Overpowered.CrashAllV3(), isTogglable = true, toolTip = "Lets you crash all while holding right trigger." },
                new ButtonInfo { buttonText = "Crash All V4 <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Overpowered.CrashAllV4(), isTogglable = true, toolTip = "Lets you crash all while holding right trigger." },
                new ButtonInfo { buttonText = "Crash All V5 <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Overpowered.CrashAllV5(), isTogglable = true, toolTip = "Lets you crash all while holding right trigger." },
                new ButtonInfo { buttonText = "Crash All V6 <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Overpowered.CrashAllV5(), isTogglable = true, toolTip = "Lets you crash all while holding right trigger." },

                new ButtonInfo { buttonText = "Lag on Touch", method =() => Overpowered.LagOnTouch(), isTogglable = true, toolTip = "Lags who ever you touch." },
                new ButtonInfo { buttonText = "Crash on Touch", method =() => Overpowered.CrashOnTouch(), isTogglable = true, toolTip = "Crashes who ever you touch." },

                new ButtonInfo { buttonText = "Ban All", method =() => Utility.BanAll(), isTogglable = true, toolTip = "Lets you ban everyone in the current room." },
                new ButtonInfo { buttonText = "Ban Gun", method =() => Overpowered.BanGun(), isTogglable = true, toolTip = "Lets you ban someone you shoot at." },

                new ButtonInfo { buttonText = "Set GameMode <color=grey>[</color><color=cyan>CASUAL</color><color=grey>]</color>", method =() => Experimental.SetGameMode("CASUAL"), isTogglable = false, toolTip = "Sets the game mode to casual."  },
                new ButtonInfo { buttonText = "Set GameMode <color=grey>[</color><color=cyan>INFECTION</color><color=grey>]</color>", method =() => Experimental.SetGameMode("INFECTION"), isTogglable = false, toolTip = "Sets the game mode to infection." },
                new ButtonInfo { buttonText = "Set GameMode <color=grey>[</color><color=cyan>HUNT</color><color=grey>]</color>", method =() => Experimental.SetGameMode("HUNT"), isTogglable = false, toolTip = "Sets the game mode to hunt." },
                new ButtonInfo { buttonText = "Set GameMode <color=grey>[</color><color=cyan>PAINTBRAWL</color><color=grey>]</color>", method =() => Experimental.SetGameMode("PAINTBRAWL"), isTogglable = false, toolTip = "Sets the game mode to battle." },
                new ButtonInfo { buttonText = "Set GameMode <color=grey>[</color><color=cyan>ERROR</color><color=grey>]</color>", method =() => Experimental.SetGameMode("ERROR"), isTogglable = false, toolTip = "Sets the game mode to battle." },
            },

            new ButtonInfo[] { // Experimental
                new ButtonInfo { buttonText = "Exit Experimental", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Show Full Mod Amount", method =() => NotificationManager.SendNotification($"Full amount: {buttons.SelectMany(list => list).ToArray().Length}"), isTogglable = false, toolTip = "Shows the amount of mods on the menu." },

                new ButtonInfo { buttonText = "Grab RPC Data", method =() => Experimental.GrabRPCData(), isTogglable = false, toolTip = "Grabs all the games rpcs and writes them to a file." },
                new ButtonInfo { buttonText = "Grab Game Info", method =() => Experimental.GrabGameInfo(), isTogglable = false, toolTip = "Grabs the games info and writes them to a file." },

                new ButtonInfo { buttonText = "Better FPS Boost", enableMethod = Experimental.BetterFPSBoost, disableMethod = Experimental.DisableBetterFPSBoost, toolTip = "Makes everything one color, boosting your FPS."},

                new ButtonInfo { buttonText = "Spaz Forest Targets", method =() => Experimental.SpazForestTargets(), isTogglable = true, toolTip = "Spazzes out the targets that are in forest." },

                new ButtonInfo { buttonText = "Spam Pop & Unpop Balloon <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => Experimental.BalloonSpam(), isTogglable = true, toolTip = "Spams everyones balloon while holding right trigger." },

                new ButtonInfo { buttonText = "Freeze All", method =() => Utility.PacketStresser(), isTogglable = true, toolTip = "Attempts to freeze everyone in the lobby." },
            },

            new ButtonInfo[] { // Master
                new ButtonInfo { buttonText = "Exit Master", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Set Master Gun", method =() => Overpowered.SetMasterGun(), isTogglable = true, toolTip = "Lets you set someone as master client." },

                new ButtonInfo { buttonText = "Slow Gun", method =() => Overpowered.SlowGun(), isTogglable = true, toolTip = "Lets you slow someone with a gun." },
                new ButtonInfo { buttonText = "Slow All", method =() => Overpowered.SlowAll(), isTogglable = true, toolTip = "Lets you slow everyone in the lobby." },

                new ButtonInfo { buttonText = "Spawn Blue Lucy", method =() => Overpowered.SpawnBlueLucy(), isTogglable = false, toolTip = "Spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Spawn Red Lucy", method =() => Overpowered.SpawnRedLucy(), isTogglable = false, toolTip = "Spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Instant Spawn Blue Lucy", method =() => Overpowered.InstantSpawnBlueLucy(), isTogglable = false, toolTip = "Instantly spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Instant Spawn Red Lucy", method =() => Overpowered.InstantSpawnRedLucy(), isTogglable = false, toolTip = "Instantly spawns the ghost Lucy in forest." },

                new ButtonInfo { buttonText = "Spaz Lucy", method =() => Overpowered.SpazLucy(), isTogglable = true, toolTip = "Spazes the ghost Lucy in forest." },

                new ButtonInfo { buttonText = "Despawn Lucy", method =() => Overpowered.DepawnLucy(), isTogglable = true, toolTip = "Despawns the ghost Lucy in forest." },

                new ButtonInfo { buttonText = "Move Lucy Gun", method =() => Overpowered.MoveLucyGun(), isTogglable = true, toolTip = "Moves the ghost Lucy in forest to the gun." },
                new ButtonInfo { buttonText = "Grab Lucy <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Overpowered.GrabLucy(), isTogglable = true, toolTip = "Lets you grab the ghost Lucy in forest while holding grips." },

                new ButtonInfo { buttonText = "Lucy Chase Self", method =() => Overpowered.LucyChaseSelf(), isTogglable = true, toolTip = "Makes the ghost Lucy chase you." },
                new ButtonInfo { buttonText = "Lucy Chase Gun", method =() => Overpowered.LucyChaseGun(), isTogglable = true, toolTip = "Makes the ghost Lucy chase the person you shoot." },

                new ButtonInfo { buttonText = "Lucy Attack Self", method =() => Overpowered.LucyAttackSelf(), isTogglable = true, toolTip = "Makes the ghost Lucy attack you." },
                new ButtonInfo { buttonText = "Lucy Attack Gun", method =() => Overpowered.LucyAttackGun(), isTogglable = true, toolTip = "Makes the ghost Lucy attack the person you shoot." },

                new ButtonInfo { buttonText = "Slow Lucy", method =() => Overpowered.SlowLucy(), isTogglable = true, toolTip = "Makes the ghost Lucy slow." },
                new ButtonInfo { buttonText = "Fast Lucy", method =() => Overpowered.FastLucy(), isTogglable = true, toolTip = "Makes the ghost Lucy fast." },

                new ButtonInfo { buttonText = "Lucy Orbit Self", method =() => Overpowered.LucyOrbitSelf(), isTogglable = true, toolTip = "Makes the ghost Lucy orbit around you." },
                new ButtonInfo { buttonText = "Lucy Orbit Gun", method =() => Overpowered.LucyOrbitGun(), isTogglable = true, toolTip = "Makes the ghost Lucy orbit around who you shoot." },

                new ButtonInfo { buttonText = "Lucy Float Self", method =() => Overpowered.LucyFloatSelf(), isTogglable = true, toolTip = "Makes the ghost Lucy float you." },
                new ButtonInfo { buttonText = "Lucy Float Gun", method =() => Overpowered.LucyFloatGun(), isTogglable = true, toolTip = "Makes the ghost Lucy float who you shoot." },

                new ButtonInfo { buttonText = "Lucy Chase Spaz", method =() => Overpowered.LucyChaseSpaz(), isTogglable = true, toolTip = "Makes the ghost Lucy spaz who they chase." },

                new ButtonInfo { buttonText = "Spawn Yellow Lucy", method =() => Overpowered.SpawnColouredLucy(Color.yellow), isTogglable = true, toolTip = "Spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Spawn Dark Blue Lucy", method =() => Overpowered.SpawnColouredLucy(Color.blue), isTogglable = true, toolTip = "Spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Spawn Magenta Lucy", method =() => Overpowered.SpawnColouredLucy(Color.magenta), isTogglable = true, toolTip = "Spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Spawn Light Red Lucy", method =() => Overpowered.SpawnColouredLucy(Color.red), isTogglable = true, toolTip = "Spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Spawn Black Lucy", method =() => Overpowered.SpawnColouredLucy(Color.black), isTogglable = true, toolTip = "Spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Spawn White Lucy", method =() => Overpowered.SpawnColouredLucy(Color.white), isTogglable = true, toolTip = "Spawns the ghost Lucy in forest." },
                new ButtonInfo { buttonText = "Spawn Green Lucy", method =() => Overpowered.SpawnColouredLucy(Color.green), isTogglable = true, toolTip = "Spawns the ghost Lucy in forest." },
            },

            new ButtonInfo[] { // Projectiles
                new ButtonInfo { buttonText = "Exit Projectiles", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Snowball Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.SnowballSpam(), isTogglable = true, toolTip = "Lets you spam snowballs when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Slingshot Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.SlingshotSpam(), isTogglable = true, toolTip = "Lets you spam slingshots when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Cloud Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.CloudSpam(), isTogglable = true, toolTip = "Lets you spam clouds when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Cupid Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.CupidSpam(), isTogglable = true, toolTip = "Lets you spam cupid when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Ice Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.IceSpam(), isTogglable = true, toolTip = "Lets you spam ice when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Deadshot Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.DeadshotSpam(), isTogglable = true, toolTip = "Lets you spam deadshot when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Elf Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.ElfSpam(), isTogglable = true, toolTip = "Lets you spam elf when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },

                new ButtonInfo { buttonText = "Snowball Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.SnowballLauncher(), isTogglable = true, toolTip = "Lets you launch snowballs when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Slingshot Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.SlingshotLauncher(), isTogglable = true, toolTip = "Lets you launch slingshots when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Cloud Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.CloudLauncher(), isTogglable = true, toolTip = "Lets you launch clouds when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Cupid Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.CupidLauncher(), isTogglable = true, toolTip = "Lets you launch cupid when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Ice Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.IceLauncher(), isTogglable = true, toolTip = "Lets you launch ice when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Deadshot Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.DeadshotLauncher(), isTogglable = true, toolTip = "Lets you launch deadshot when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Elf Launcher <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.ElfLauncher(), isTogglable = true, toolTip = "Lets you launch elf when holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },

                new ButtonInfo { buttonText = "Snowball Gun <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.SnowballGun(), disableMethod =() => Utility.FixGhostRig(), isTogglable = true, toolTip = "Lets you shoot snowballs with a gun." },
                new ButtonInfo { buttonText = "Slingshot Gun <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.SlingshotGun(), disableMethod =() => Utility.FixGhostRig(), isTogglable = true, toolTip = "Lets you shoot slingshots with a gun." },
                new ButtonInfo { buttonText = "Cloud Gun <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.CloudGun(), disableMethod =() => Utility.FixGhostRig(), isTogglable = true, toolTip = "Lets you shoot clouds with a gun." },
                new ButtonInfo { buttonText = "Cupid Gun <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.CupidGun(), disableMethod =() => Utility.FixGhostRig(), isTogglable = true, toolTip = "Lets you shoot cupid with a gun." },
                new ButtonInfo { buttonText = "Ice Gun <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.IceGun(), disableMethod =() => Utility.FixGhostRig(), isTogglable = true, toolTip = "Lets you shoot ice with a gun." },
                new ButtonInfo { buttonText = "Deadshot Gun <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.DeadshotGun(), disableMethod =() => Utility.FixGhostRig(), isTogglable = true, toolTip = "Lets you shoot deadshots with a gun." },
                new ButtonInfo { buttonText = "Elf Gun <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Projectiles.ElfGun(), disableMethod =() => Utility.FixGhostRig(), isTogglable = true, toolTip = "Lets you shoot elfs with a gun." },
            },

            new ButtonInfo[] { // Spammers
                new ButtonInfo { buttonText = "Exit Spammers", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Hand Tap Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Spammers.HandTapSpam(), isTogglable = true, toolTip = "Lets you spam a sound while holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Bark Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Spammers.BarkSpam(), isTogglable = true, toolTip = "Lets you spam a sound while holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Crystal Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Spammers.CrystalSpam(), isTogglable = true, toolTip = "Lets you spam a sound while holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Metal Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Spammers.MetalSpam(), isTogglable = true, toolTip = "Lets you spam a sound while holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Glass Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Spammers.GlassSpam(), isTogglable = true, toolTip = "Lets you spam a sound while holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Snow Step Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Spammers.SnowStepSpam(), isTogglable = true, toolTip = "Lets you spam a sound while holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Leaf Crunch Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Spammers.LeafCrunchSpam(), isTogglable = true, toolTip = "Lets you spam a sound while holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },
                new ButtonInfo { buttonText = "Random Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => Spammers.RandomSpam(), isTogglable = true, toolTip = "Lets you spam a sound while holding <color=grey>[</color><color=cyan>G</color><color=grey>]</color>." },

                new ButtonInfo { buttonText = "Custom Sound ID", method = Spammers.IncreaseSoundID, enableMethod = Spammers.IncreaseSoundID, disableMethod = Spammers.DecreaseSoundID, incremental = true, isTogglable = false, toolTip = "Changes the Sound ID of the Custom Sound Spam." },
                new ButtonInfo { buttonText = "Custom Sound Spam", overlapText = "Custom Sound Spam <color=grey>[</color><color=cyan>0</color><color=grey>]</color>", method = Spammers.CustomSoundSpam, toolTip = "Plays the selected sound when holding <color=cyan>G</color>." },
            },

            new ButtonInfo[] { // Soundboard
                new ButtonInfo { buttonText = "Exit Soundboard", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },
            },

            new ButtonInfo[] { // Players
                new ButtonInfo { buttonText = "Exit Players", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },
            },

            new ButtonInfo[] { // GTH Mods
                new ButtonInfo { buttonText = "Exit Gorilla Tag Horror", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the settings page of the menu." },

                new ButtonInfo { buttonText = "Spawn Timmy", method =() => GTH.SpawnTimmy(), toolTip = "Spawns a timmy above your head.", isTogglable = true },
                new ButtonInfo { buttonText = "Spawn Stalker", method =() => GTH.SpawnStalker(), toolTip = "Spawns a stalker above your head.", isTogglable = true },
                new ButtonInfo { buttonText = "Timmy Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => GTH.TimmySpam(), toolTip = "Lets you spam timmys while holding your grips.", isTogglable = true },
                new ButtonInfo { buttonText = "Stalker Spam <color=grey>[</color><color=cyan>G</color><color=grey>]</color>", method =() => GTH.StalkerSpam(), toolTip = "Lets you spam stalkers while holding your grips.", isTogglable = true },
                new ButtonInfo { buttonText = "Timmy Gun", method =() => GTH.TimmyGun(), toolTip = "Lets you spawn timmys with a gun.", isTogglable = true },
                new ButtonInfo { buttonText = "Stalker Gun", method =() => GTH.StalkerGun(), toolTip = "Lets you spawn stalkers with a gun.", isTogglable = true },

                new ButtonInfo { buttonText = "Timmy ESP", method =() => GTH.TimmyESP(false), disableMethod =() => GTH.TimmyESP(true), toolTip = "Lets you find timmys through walls.", isTogglable = true },
                new ButtonInfo { buttonText = "Timmy Tracers", method =() => GTH.TimmyTracers(), toolTip = "Puts tracers on the timmys.", isTogglable = true },
                new ButtonInfo { buttonText = "Stalker ESP", method =() => GTH.StalkerESP(false), disableMethod =() => GTH.StalkerESP(true), toolTip = "Lets you find stalkers through walls.", isTogglable = true },
                new ButtonInfo { buttonText = "Stalker Tracers", method =() => GTH.StalkerTracers(), toolTip = "Puts tracers on the stalkers.", isTogglable = true },

                new ButtonInfo { buttonText = "Kill Gun", method =() => GTH.KillGun(), toolTip = "Lets you kill the person you shoot at with a gun.", isTogglable = true },
                new ButtonInfo { buttonText = "Kill All", method =() => GTH.KillAll(), toolTip = "Lets you kill everyone in the current room.", isTogglable = true },

                new ButtonInfo { buttonText = "Timmy Rape Gun", method =() => GTH.TimmyRapeGun(), toolTip = "Lets you rape someone using the timmys.", isTogglable = true },
                new ButtonInfo { buttonText = "Stalker Rape Gun", method =() => GTH.StalkerRapeGun(), toolTip = "Lets you rape someone using the stalkers.", isTogglable = true },

                new ButtonInfo { buttonText = "Fling Timmy Gun", method =() => GTH.FlingGun("timmy"), toolTip = "Lets you fling the timmy you shoot at.", isTogglable = true },
                new ButtonInfo { buttonText = "Fling Stalker Gun", method =() => GTH.FlingGun("stalker"), toolTip = "Lets you fling the stalker you shoot at.", isTogglable = true },
                new ButtonInfo { buttonText = "Fling Monter Gun", method =() => GTH.FlingGunComponent("EnemyController"), toolTip = "Lets you fling all the monsters you shoot at.", isTogglable = true },

                new ButtonInfo { buttonText = "Bring All Monsters", method =() => GTH.BringAllMonsters(), toolTip = "Lets you bring all the monsters to you.", isTogglable = false },
                new ButtonInfo { buttonText = "Bring Monsters Gun", method =() => GTH.BringMonstersGun(), toolTip = "Lets you bring all the monsters to the gun.", isTogglable = true },

                new ButtonInfo { buttonText = "Kill Timmy Gun", method =() => GTH.KillTimmyGun(), toolTip = "Lets you kill the timmy you shoot at.", isTogglable = true },
                new ButtonInfo { buttonText = "Kill Stalker Gun", method =() => GTH.KillStalkerGun(), toolTip = "Lets you kill the stalker you shoot at.", isTogglable = true },
                new ButtonInfo { buttonText = "Kill Monster Gun", method =() => GTH.KillMonsterGun(), toolTip = "Lets you kill the monster you shoot at.", isTogglable = true },

                new ButtonInfo { buttonText = "Kill All Timmys", method =() => GTH.KillAllTimmys(), toolTip = "Lets you kill all the timmys.", isTogglable = true },
                new ButtonInfo { buttonText = "Kill All Stalkers", method =() => GTH.KillAllStalkers(), toolTip = "Lets you kill all the stalkers.", isTogglable = true },
                new ButtonInfo { buttonText = "Kill All Monsters", method =() => GTH.KillAllMonsters(), toolTip = "Lets you kill all the monsters.", isTogglable = true },

                new ButtonInfo { buttonText = "Explode Timmy Gun", method =() => GTH.ExplodeTimmyGun(), toolTip = "Spawns timmys at the gun and explodes them.", isTogglable = true },
                new ButtonInfo { buttonText = "Explode Stalker Gun", method =() => GTH.ExplodeStalkerGun(), toolTip = "Spawns stalkers at the gun and explodes them.", isTogglable = true },
                new ButtonInfo { buttonText = "Explode Monsters", method =() => GTH.ExplodeMonsters(), toolTip = "Explodes all the monsters.", isTogglable = true },

                new ButtonInfo { buttonText = "Become Timmy", method =() => GTH.BecomeTimmy(), toolTip = "Lets you become the timmy in forest.", isTogglable = true },
                new ButtonInfo { buttonText = "Become Stalker", method =() => GTH.BecomeStalker(), toolTip = "Lets you become the stalkers.", isTogglable = true },

                new ButtonInfo { buttonText = "Fast Timmys", method =() => GTH.FastTimmys(), disableMethod =() => GTH.ResetTimmy(), toolTip = "Makes the timmys faster.", isTogglable = true },
                new ButtonInfo { buttonText = "Slow Timmys", method =() => GTH.SlowTimmys(), disableMethod =() => GTH.ResetTimmy(), toolTip = "Makes the timmys slower.", isTogglable = true },

                new ButtonInfo { buttonText = "Spaz Timmys", method =() => GTH.SpazTimmys(), toolTip = "Spazes the timmys in forest", isTogglable = true },
                new ButtonInfo { buttonText = "Spaz Stalkers", method =() => GTH.SpazStalkers(), toolTip = "Spazes all the stalkers", isTogglable = true },

                new ButtonInfo { buttonText = "Place Trap <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => GTH.PlaceTrap(), toolTip = "Places down a trap when you press right trigger.", isTogglable = true },
                new ButtonInfo { buttonText = "Destroy Trap", method =() => GTH.DestroyTrap(), toolTip = "Destroys the trap.", isTogglable = false },
                new ButtonInfo { buttonText = "Timmys To Trap", method =() => GTH.TimmysToTrap(), toolTip = "Makes all the timmys go to the trap.", isTogglable = true },
                new ButtonInfo { buttonText = "Stalkers To Trap", method =() => GTH.StalkersToTrap(), toolTip = "Makes all the stalkers go to the trap.", isTogglable = true },
                new ButtonInfo { buttonText = "Monsters To Trap", method =() => GTH.MonstersToTrap(), toolTip = "Makes all the monsters go to the trap.", isTogglable = true },

                new ButtonInfo { buttonText = "Timmy Work <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => GTH.TimmyWork(), toolTip = "Makes a firework out of timmys.", isTogglable = true },
                new ButtonInfo { buttonText = "Stalker Work <color=grey>[</color><color=cyan>RT</color><color=grey>]</color>", method =() => GTH.StalkerWork(), toolTip = "Makes a firework out of stalkers.", isTogglable = true },

                new ButtonInfo { buttonText = "Joystick Control Timmys <color=grey>[</color><color=cyan>RJ</color><color=grey>]</color>", method =() => GTH.JoystickControlTimmys(), toolTip = "Lets you control the timmys movement with your right joystick.", isTogglable = true },
                new ButtonInfo { buttonText = "Joystick Control Stalkers <color=grey>[</color><color=cyan>RJ</color><color=grey>]</color>", method =() => GTH.JoystickControlStalkers(), toolTip = "Lets you control the stalkers movement with your right joystick.", isTogglable = true },
                new ButtonInfo { buttonText = "Joystick Control Monsters <color=grey>[</color><color=cyan>RJ</color><color=grey>]</color>", method =() => GTH.JoystickControlMonters(), toolTip = "Lets you control the monsters movement with your right joystick.", isTogglable = true },
            },

            new ButtonInfo[] { // Photon Bots | mocas stinky -nova
                new ButtonInfo { buttonText = "Exit Photon Bots", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },

                new ButtonInfo { buttonText = "Spawn Bots", method =() => PhotonBots.SpawnBots(), isTogglable = false, toolTip = "Spawns photon bots in the current room." },
                new ButtonInfo { buttonText = "Stop Bots", method =() => PhotonBots.StopBots(), isTogglable = false, toolTip = "Stops and clears all photon bots." },

                new ButtonInfo { buttonText = "Bot Count", overlapText = "Bot Count <color=grey>[</color><color=cyan>10</color><color=grey>]</color>", method =() => PhotonBots.IncreaseBotCount(), enableMethod =() => PhotonBots.IncreaseBotCount(), disableMethod =() => PhotonBots.DecreaseBotCount(), incremental = true, isTogglable = false, toolTip = "Changes how many bots to spawn (1-50)." },
                new ButtonInfo { buttonText = "Bot Name", overlapText = "Bot Name <color=grey>[</color><color=cyan>JUPITERX</color><color=grey>]</color>", method =() => PhotonBots.NextBotName(), enableMethod =() => PhotonBots.NextBotName(), disableMethod =() => PhotonBots.PrevBotName(), incremental = true, isTogglable = false, toolTip = "Changes the name prefix for spawned bots." },

                new ButtonInfo { buttonText = "Bring Bots", method =() => PhotonBots.BringBots(), isTogglable = false, toolTip = "Brings all bots to your position." },
                new ButtonInfo { buttonText = "Bots To Gun", method =() => PhotonBots.BotsToGun(), isTogglable = true, toolTip = "Moves bots to wherever your gun points." },

                new ButtonInfo { buttonText = "Freeze Bots", enableMethod =() => PhotonBots.freezeBots = true, disableMethod =() => PhotonBots.freezeBots = false, isTogglable = true, toolTip = "Freezes all bots in place." },

                new ButtonInfo { buttonText = "Orbit Me", method =() => PhotonBots.OrbitMe(), isTogglable = true, toolTip = "Makes bots orbit around you." },
                new ButtonInfo { buttonText = "Orbit Gun", method =() => PhotonBots.OrbitGun(), isTogglable = true, toolTip = "Makes bots orbit around whoever you shoot." },
                new ButtonInfo { buttonText = "Stack Bots", method =() => PhotonBots.StackBots(), isTogglable = true, toolTip = "Stacks bots on top of each other." },
                new ButtonInfo { buttonText = "Line Bots", method =() => PhotonBots.LineBots(), isTogglable = true, toolTip = "Arranges bots in a line." },
                new ButtonInfo { buttonText = "Spaz Bots", method =() => PhotonBots.SpazBots(), isTogglable = true, toolTip = "Makes bots spaz out randomly." },
                new ButtonInfo { buttonText = "Stop Movement", method =() => PhotonBots.StopMovement(), isTogglable = false, toolTip = "Stops all bot movement." },

                new ButtonInfo { buttonText = "Orbit Radius", overlapText = "Orbit Radius <color=grey>[</color><color=cyan>3.5</color><color=grey>]</color>", method =() => PhotonBots.RadiusUp(), enableMethod =() => PhotonBots.RadiusUp(), disableMethod =() => PhotonBots.RadiusDown(), incremental = true, isTogglable = false, toolTip = "Changes the orbit radius (1-15)." },
                new ButtonInfo { buttonText = "Orbit Speed", overlapText = "Orbit Speed <color=grey>[</color><color=cyan>60</color><color=grey>]</color>", method =() => PhotonBots.SpeedUp(), enableMethod =() => PhotonBots.SpeedUp(), disableMethod =() => PhotonBots.SpeedDown(), incremental = true, isTogglable = false, toolTip = "Changes the orbit speed (0-180)." },

                new ButtonInfo { buttonText = "Mirror Me", method =() => PhotonBots.MirrorMe(), isTogglable = true, toolTip = "Bots copy your movements." },
                new ButtonInfo { buttonText = "Mirror Gun", method =() => PhotonBots.MirrorGun(), isTogglable = true, toolTip = "Bots copy the movements of whoever you shoot." },

                new ButtonInfo { buttonText = "Bot Taps", enableMethod =() => PhotonBots.botTaps = true, disableMethod =() => PhotonBots.botTaps = false, isTogglable = true, toolTip = "Makes bots spam hand taps." },
                new ButtonInfo { buttonText = "Tap Sound", overlapText = "Tap Sound <color=grey>[</color><color=cyan>0</color><color=grey>]</color>", method =() => PhotonBots.NextTapSound(), enableMethod =() => PhotonBots.NextTapSound(), disableMethod =() => PhotonBots.PrevTapSound(), incremental = true, isTogglable = false, toolTip = "Changes the tap sound ID (0-61)." },
                new ButtonInfo { buttonText = "Bot Colors", enableMethod =() => PhotonBots.botColors = true, disableMethod =() => PhotonBots.botColors = false, isTogglable = true, toolTip = "Makes bots cycle through rainbow colors." },

                new ButtonInfo { buttonText = "Bot Event Spam", enableMethod =() => PhotonBots.botEventSpam = true, disableMethod =() => PhotonBots.botEventSpam = false, isTogglable = true, toolTip = "Makes bots spam junk events." },
                new ButtonInfo { buttonText = "Bot Prefab Spam", enableMethod =() => PhotonBots.botPrefabSpam = true, disableMethod =() => PhotonBots.botPrefabSpam = false, isTogglable = true, toolTip = "Makes bots spam prefabs." },
                new ButtonInfo { buttonText = "Bot Crash", enableMethod =() => PhotonBots.botCrash = true, disableMethod =() => PhotonBots.botCrash = false, isTogglable = true, toolTip = "Makes bots send crash events." },
            },

            new ButtonInfo[] { }, // Temporary Category

            new ButtonInfo[] { // Enabled
                new ButtonInfo { buttonText = "Exit Enabled", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },
            },

            new ButtonInfo[] { // Favorites
                new ButtonInfo { buttonText = "Exit Favorite", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns to the main page of the menu." },
            },

            new[] { // Menu Presets
                new ButtonInfo { buttonText = "Exit Menu Presets", method =() => CurrentCategoryName = "Menu Settings", isTogglable = false, toolTip = "Returns to the settings for the menu."},

                new ButtonInfo { buttonText = "Nova Preset", method =() => Presets.NovaPreset(), isTogglable = false, toolTip = "Saves a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 1", method =() => Presets.SaveCustomPreset(1), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 1", method =() => Presets.LoadCustomPreset(1), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 2", method =() => Presets.SaveCustomPreset(2), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 2", method =() => Presets.LoadCustomPreset(2), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 3", method =() => Presets.SaveCustomPreset(3), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 3", method =() => Presets.LoadCustomPreset(3), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 4", method =() => Presets.SaveCustomPreset(4), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 4", method =() => Presets.LoadCustomPreset(4), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 5", method =() => Presets.SaveCustomPreset(5), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 5", method =() => Presets.LoadCustomPreset(5), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 6", method =() => Presets.SaveCustomPreset(6), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 6", method =() => Presets.LoadCustomPreset(6), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 7", method =() => Presets.SaveCustomPreset(7), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 7", method =() => Presets.LoadCustomPreset(7), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 8", method =() => Presets.SaveCustomPreset(8), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 8", method =() => Presets.LoadCustomPreset(8), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 9", method =() => Presets.SaveCustomPreset(9), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 9", method =() => Presets.LoadCustomPreset(9), isTogglable = false, toolTip = "Loads a custom preset."},

                new ButtonInfo { buttonText = "Save Custom Preset 10", method =() => Presets.SaveCustomPreset(10), isTogglable = false, toolTip = "Saves a custom preset."},
                new ButtonInfo { buttonText = "Load Custom Preset 10", method =() => Presets.LoadCustomPreset(10), isTogglable = false, toolTip = "Loads a custom preset."},
            },

            new[] { // Admin
                new ButtonInfo { buttonText = "Exit Admin", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Opens the visual page for the menu" },

                new ButtonInfo { buttonText = "Get Console Users", method =() => Experimental.GetMenuUsers(), isTogglable = false, toolTip = "Gets all users using console" },
                new ButtonInfo { buttonText = "Console Users NameTag", enableMethod =() => Console.ServerDataJupiterX.instance.adminnametags = true, disableMethod =() => Console.ServerDataJupiterX.instance.adminnametags = false, isTogglable = true, toolTip = "Enables the console nametags" },
                new ButtonInfo { buttonText = "Admin Quit All", method =() => Experimental.ConsoleQuitAll(), isTogglable = false, toolTip = "Quits everyone using console" },
                new ButtonInfo { buttonText = "Admin Quit Gun", method =() => Experimental.ConsoleQuitGun(), isTogglable = true, toolTip = "Quits who ever you shoot using console" },
                new ButtonInfo { buttonText = "Admin Discord Open All", method =() => Experimental.ConsoleDiscordAll(), isTogglable = false, toolTip = "Opens the qolossal discord on the browser of everyone using console" },
                new ButtonInfo { buttonText = "Admin Discord Open Gun", method =() => Experimental.ConsoleOpenDiscordGun(), isTogglable = true, toolTip = "Opens the qolossal discord on the browser of who ever you shoot using console" },
                new ButtonInfo { buttonText = "Admin Crash All", method =() => Experimental.ConsoleCrashAll(), isTogglable = false, toolTip = "Crashes everyone who is using console" },
                new ButtonInfo { buttonText = "Admin Crash Gun", method =() => Experimental.ConsoleCrashGun(), isTogglable = true, toolTip = "Crashes the person you shoot who is using console" },
                new ButtonInfo { buttonText = "Admin Kick All", method =() => Experimental.ConsoleKickAll(), isTogglable = false, toolTip = "Kicks everyone using console"},
                new ButtonInfo { buttonText = "Admin Kick Gun", method =() => Experimental.ConsoleKickGun(), isTogglable = true, toolTip = "Kicks who ever you shoot using console" },
                new ButtonInfo { buttonText = "Admin Fling All", method =() => Experimental.ConsoleFlingAll(), isTogglable = false, toolTip = "Flings everyone using console" },
                new ButtonInfo { buttonText = "Admin Fling Gun", method =() => Experimental.ConsoleFlingGun(), isTogglable = true, toolTip = "Flings who ever you shoot using console" },
                new ButtonInfo { buttonText = "Admin Bring All", method =() => Experimental.ConsoleBringAll(), isTogglable = false, toolTip = "Brings everyone using console to you" },
                new ButtonInfo { buttonText = "Admin Bring Gun", method =() => Experimental.ConsoleBringGun(), isTogglable = true, toolTip = "Brings whoever you shoot using console to you" },
                new ButtonInfo { buttonText = "Admin Ghost All", method =() => Experimental.ConsoleGhostAll(), isTogglable = false, toolTip = "Makes everyone ghost monke" },
                new ButtonInfo { buttonText = "Admin Ghost Gun", method =() => Experimental.ConsoleGhostGun(), isTogglable = true, toolTip = "Makes who ever you shoot ghost monke" },
                new ButtonInfo { buttonText = "Admin UnGhost All", method =() => Experimental.ConsoleUnGhostAll(), isTogglable = false, toolTip = "Fixes everyones rig" },
                new ButtonInfo { buttonText = "Admin UnGhost Gun", method =() => Experimental.ConsoleUnGhostGun(), isTogglable = true, toolTip = "Fixes who you shoot rig" },
                new ButtonInfo { buttonText = "Admin Disable Movement All", method =() => Experimental.ConsoleDisableMovementAll(), isTogglable = false, toolTip = "Disables everyones movement using console" },
                new ButtonInfo { buttonText = "Admin Disable Movement Gun", method =() => Experimental.ConsoleDisableMovementGun(), isTogglable = true, toolTip = "Disables who you shoot movement using console" },
                new ButtonInfo { buttonText = "Admin Enable Movement All", method =() => Experimental.ConsoleEnableMovementAll(), isTogglable = false, toolTip = "Reanbles everyones movement using console" },
                new ButtonInfo { buttonText = "Admin Enable Movement Gun", method =() => Experimental.ConsoleEnableMovementGun(), isTogglable = true, toolTip = "Reanbles who you shoot movement using console" },
                new ButtonInfo { buttonText = "Admin Mute All", method =() => Experimental.ConsoleMuteAll(), isTogglable = false, toolTip = "Mutes everyone using console" },
                new ButtonInfo { buttonText = "Admin Mute Gun", method =() => Experimental.ConsoleMuteGun(), isTogglable = true, toolTip = "Mutes who you shoot using console" },
                new ButtonInfo { buttonText = "Admin UnMute All", method =() => Experimental.ConsoleUnMuteAll(), isTogglable = false, toolTip = "UnMutes everyone using console"},
                new ButtonInfo { buttonText = "Admin UnMute Gun", method =() => Experimental.ConsoleUnMuteGun(), isTogglable = true, toolTip = "UnMutes who you shoot using console" },
                new ButtonInfo { buttonText = "Admin Network Player All", method =() => Experimental.ConsoleNetworkPlayerAll(), isTogglable = false, toolTip = "Spawns a network player at people using console" },
                new ButtonInfo { buttonText = "Admin Network Player Gun", method =() => Experimental.ConsoleNetworkPlayerGun(), isTogglable = true, toolTip = "Spawns a network player at who you shoot using console" },
                new ButtonInfo { buttonText = "Admin Target All", method =() => Experimental.ConsoleTargetPlayerAll(), isTogglable = false, toolTip = "Spawns a stickable target at everyone using console" },
                new ButtonInfo { buttonText = "Admin Target Gun", method =() => Experimental.ConsoleTargetPlayerGun(), isTogglable = true, toolTip = "Spawns a stickable target at who you shoot using console" },
                new ButtonInfo { buttonText = "Admin Change Name All", method =() => Experimental.ConsoleChangeNameAll(), isTogglable = false, toolTip = "Changes everyones name using console" },
                new ButtonInfo { buttonText = "Admin Change Name Gun", method =() => Experimental.ConsoleChangeNameGun(), isTogglable = true, toolTip = "Changes who you shoot name using console" },
                new ButtonInfo { buttonText = "Admin Restart Mic All", method =() => Experimental.ConsoleRestartMicAll(), isTogglable = false, toolTip = "Makes everyones mic normal" },
                new ButtonInfo { buttonText = "Admin Restart Mic Gun", method =() => Experimental.ConsoleRestartMicGun(), isTogglable = true, toolTip = "Makes who you shoot mic normal" },
                new ButtonInfo { buttonText = "Admin Disable Mods All", method =() => Experimental.ConsolePanicAll(), isTogglable = false, toolTip = "Disables everyones mods who are using console" },
                new ButtonInfo { buttonText = "Admin Disable Mods Gun", method =() => Experimental.ConsolePanicGUn(), isTogglable = true, toolTip = "Makes the person you shoot mods disable" },
                new ButtonInfo { buttonText = "Admin Stop Photon Bots All", method =() => Experimental.ConsoleStopPhotonBotsAll(), isTogglable = false, toolTip = "Disables everyones photon bots who are using console or jupiterx" },
                new ButtonInfo { buttonText = "Admin Stop Photon Bots Gun", method =() => Experimental.StopPhotonBotsGun(), isTogglable = true, toolTip = "Disables who you shoot photon bots who are using console or jupiterx" },
            },

            new[] { // Plugin Settings
                new ButtonInfo { buttonText = "Exit Plugin Settings", method =() => CurrentCategoryName = "Settings", isTogglable = false, toolTip = "Returns you back to the settings menu." },

                new ButtonInfo { buttonText = "Reload Plugins", method = PluginManager.ReloadPlugins, isTogglable = false, toolTip = "Reloads all of your plugins." }
            },

            new[] { // Credits
                new ButtonInfo { buttonText = "Exit Credits", method =() => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns you back to the main page." },

                new ButtonInfo { buttonText = "Nova", method =() => Application.OpenURL("https://github.com/real-Silent"), isTogglable = false, toolTip = "Nova is the main developer of <b>JupiterX</b> she makes and ports most of the mods on the menu." },
                new ButtonInfo { buttonText = "Silent", method =() => Application.OpenURL("https://github.com/real-Silent"), isTogglable = false, toolTip = "Silent was the original owner of <b>JupiterX</b> until she changed account to Nova she is just Nova." },

                new ButtonInfo { buttonText = "Z0", method =() => Application.OpenURL("https://github.com/kralsei"), isTogglable = false, toolTip = "Z0 is a helper of <b>JupiterX</b> and has made some of the crash mods and helped in general." },
                new ButtonInfo { buttonText = "Dom", method =() => Application.OpenURL("https://github.com/xfi0"), isTogglable = false, toolTip = "Dom made the original soundboard code for his mod menu <b>Titled</b> that then Nova changed how it loads and added it to <b>JupiterX</b>." },
                new ButtonInfo { buttonText = "Saturn", method =() => Application.OpenURL("https://github.com/saturnlamoooooooooooooooo"), isTogglable = false, toolTip = "Helped a lil with <b>JupiterX</b>." },
            },

            new[] // Achievements
            {
                new ButtonInfo { buttonText = "Exit Achievements", method = () => CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns you back to the main page." }
            },

            new[] { // Sound Library
                new ButtonInfo { buttonText = "Exit Sound Library", method =() => Soundboard.LoadSoundboard(), isTogglable = false, toolTip = "Returns you back to the soundboard." }
            },

            new [] // Internal
            {
                //new ButtonInfo { buttonText = "Search", method = KeyboardManager.Search, isTogglable = false, toolTip = "Lets you search for specific mods."},
                new ButtonInfo { buttonText = "Global Return", method = Settings.GlobalReturn, isTogglable = false, toolTip = "Returns you to the previous category." },
                new ButtonInfo { buttonText = "Accept Prompt", method =() => { NotificationManager.ClearAllNotifications(); CurrentPrompt.AcceptAction?.Invoke(); Utility.StopCurrentPrompt(); }, isTogglable = false },
                new ButtonInfo { buttonText = "Decline Prompt", method =() => { NotificationManager.ClearAllNotifications(); CurrentPrompt.DeclineAction?.Invoke(); Utility.StopCurrentPrompt(); }, isTogglable = false },
            }
        };

        public static string[] categoryNames =
        {
            "Main",
            "Settings",
            "Menu Settings",
            "Movement Settings",
            "Gun Settings",

            "Important",
            "Safety",
            "Computer",
            "Movement",
            "Advantage",
            "VRRig",
            "Visual",
            "Fun",
            "Name",
            "Prefabs",
            "Overpowered",
            "Experimental",
            "Master",

            "Projectiles",
            "Sound Spammers",

            "Soundboard",
            "Players",
            "Gorilla Tag Horror",
            "Photon Bots",

            "Temporary Category",

            "Enabled",
            "Favorite",
            "Menu Presets",
            "Admin",
            "Plugin Settings",
            "Credits",
            "Achievements",
            "Sound Library",
            "Internal",
        };

        public static int _currentCategoryIndex;
        public static event Action OnCategoryChanged;

        public static int CurrentCategoryIndex
        {
            get => _currentCategoryIndex;
            set
            {
                _currentCategoryIndex = value;
                pageNumber = 0;

                OnCategoryChanged?.Invoke();
            }
        }

        public static string CurrentCategoryName
        {
            get => Buttons.categoryNames[CurrentCategoryIndex];
            set =>
                CurrentCategoryIndex = Buttons.GetCategory(value);
        }

        private static readonly Dictionary<string, (int Category, int Index)> cacheGetIndex = new Dictionary<string, (int Category, int Index)>(); // Looping through 800 elements is not a light task :/

        public static ButtonInfo GetIndex(string buttonText)
        {
            if (buttonText == null)
                return null;

            if (cacheGetIndex.TryGetValue(buttonText, out var cacheData))
            {
                try
                {
                    if (buttons[cacheData.Category][cacheData.Index].buttonText == buttonText)
                        return buttons[cacheData.Category][cacheData.Index];
                }
                catch { cacheGetIndex.Remove(buttonText); }
            }

            int categoryIndex = 0;
            foreach (ButtonInfo[] buttons in buttons)
            {
                int buttonIndex = 0;
                foreach (ButtonInfo button in buttons)
                {
                    if (button.buttonText == buttonText)
                    {
                        try
                        {
                            cacheGetIndex.Add(buttonText, (categoryIndex, buttonIndex));
                        }
                        catch
                        {
                            cacheGetIndex.Remove(buttonText);
                        }

                        return button;
                    }
                    buttonIndex++;
                }
                categoryIndex++;
            }

            return null;
        }
        public static int GetCategory(string categoryName) =>
            categoryNames.ToList().IndexOf(categoryName);

        public static int AddCategory(string categoryName)
        {
            List<ButtonInfo[]> buttonInfoList = buttons.ToList();
            buttonInfoList.Add(new ButtonInfo[] { });
            buttons = buttonInfoList.ToArray();

            List<string> categoryList = categoryNames.ToList();
            categoryList.Add(categoryName);
            categoryNames = categoryList.ToArray();

            return buttons.Length - 1;
        }
        public static void RemoveCategory(string categoryName)
        {
            List<ButtonInfo[]> buttonInfoList = buttons.ToList();
            buttonInfoList.RemoveAt(GetCategory(categoryName));
            buttons = buttonInfoList.ToArray();

            List<string> categoryList = categoryNames.ToList();
            categoryList.Remove(categoryName);
            categoryNames = categoryList.ToArray();
        }
        public static void AddButton(int category, ButtonInfo button, int index = -1)
        {
            List<ButtonInfo> buttonInfoList = buttons[category].ToList();
            if (index > 0)
                buttonInfoList.Insert(index, button);
            else
                buttonInfoList.Add(button);

            buttons[category] = buttonInfoList.ToArray();
        }
        public static void AddButtons(int category, ButtonInfo[] buttons, int index = -1)
        {
            List<ButtonInfo> buttonInfoList = Buttons.buttons[category].ToList();
            if (index > 0)
            {
                for (int i = 0; i < buttons.Length; i++)
                    buttonInfoList.Insert(index + i, buttons[i]);
            }
            else
                buttonInfoList.AddRange(buttons);

            Buttons.buttons[category] = buttonInfoList.ToArray();
        }
        public static void RemoveButton(int category, string name, int index = -1)
        {
            List<ButtonInfo> buttonInfoList = buttons[category].ToList();
            if (index > 0)
                buttonInfoList.RemoveAt(index);
            else
            {
                foreach (var button in buttonInfoList.Where(button => button.buttonText == name))
                {
                    buttonInfoList.Remove(button);
                    break;
                }
            }

            buttons[category] = buttonInfoList.ToArray();
        }
    }
}

// Mod Graveyard - mods here are removed for one reason or another
/*
new ButtonInfo { buttonText = "Dick Spawn [<color=cyan>RT</color>]", method =() => Experimental.DickSpawn(), isTogglable = true },
new ButtonInfo { buttonText = "MeowMeow Cube Spawn [<color=cyan>RT</color>]", method = () => Experimental.MeowMeowCubeSpawn(), isTogglable = true },

new ButtonInfo { buttonText = "Eternal Sugar Cookie Spam [<color=cyan>RT</color>]", method =() => Experimental.eternalsugercookieSpammer(), isTogglable = true }, aw man

new ButtonInfo { buttonText = "Get F'd Spawn", method = () => Experimental.GetFuckedNetPlayers(), isTogglable = false },

new ButtonInfo { buttonText = "Ban Gun [JX Modding Game]", method =() => Overpowered.BanGunJXModding(), isTogglable = true },

new ButtonInfo { buttonText = "Teleport To Slingshot", method =() => Utility.BetaTPToSling(), isTogglable = true },

new ButtonInfo { buttonText = "Float Gun", method =() => Overpowered.FloatGun(), isTogglable = true, toolTip = "Attempts to make the person you shoot at float." },

new ButtonInfo { buttonText = "Spam Mute All V2", method =() => Utility.BetaSpamMuteAll(), isTogglable = true, toolTip = "Spams all the report and mute buttons." },
*/