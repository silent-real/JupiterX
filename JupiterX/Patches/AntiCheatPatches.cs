// ============================================================
//  JupiterX | Patches/AntiCheatPatches.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using HarmonyLib;
using JupiterX.Notifications;
using Photon.Pun;

namespace JupiterX.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "SendReport")]
    public class AntiCheatPatches
    {
        public static bool AntiCheatSelf;
        public static bool AntiCheatAll;

        public static void Prefix(string susReason, string susId, string susNick)
        {
            if (AntiCheatSelf)
            {
                if (susId == PhotonNetwork.LocalPlayer.UserId)
                    NotificationManager.SendNotification($"<color=grey>[</color><color=yellow>ANTICHEAT</color><color=grey>]</color> AntiCheat Reported you for {susReason}.");
            }
            if (AntiCheatAll)
            {
                NotificationManager.SendNotification($"<color=grey>[</color><color=yellow>ANTICHEAT</color><color=grey>]</color> AntiCheat Reported {susNick} for {susReason}.");
            }
        }
    }
}