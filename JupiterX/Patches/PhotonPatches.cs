// ============================================================
//  JupiterX | Patches/PhotonPatches.cs
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
using Photon.Pun;
using Photon.Realtime;

namespace JupiterX.Patches
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnJoinedRoom")]
    public static class OnJoinedRoomPatch
    {
        public static void Postfix()
        {
            NetworkSystem.RaiseJoinedRoom();
        }
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnLeftRoom")]
    public static class OnLeftRoomPatch
    {
        public static void Postfix()
        {
            NetworkSystem.RaiseLeftRoom();
        }
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnMasterClientSwitched")]
    public static class OnMasterClientSwitchedPatch
    {
        public static void Postfix(Player newMasterClient)
        {
            NetworkSystem.RaiseMasterClientSwitched();
        }
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    public static class OnPlayerEnteredRoomPatch
    {
        public static void Postfix(Player newPlayer)
        {
            NetworkSystem.RaisePlayerJoinedRoom(newPlayer);
        }
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    public static class OnPlayerLeftRoomPatch
    {
        public static void Postfix(Player otherPlayer)
        {
            NetworkSystem.RaisePlayerLeftRoom(otherPlayer);
        }
    }
}