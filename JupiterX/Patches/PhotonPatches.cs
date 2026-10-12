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
using System.Collections.Generic;

namespace JupiterX.Patches
{
    internal static class PhotonEventGate
    {
        private static bool inRoom;
        private static int lastMasterActor = -1;
        private static readonly HashSet<int> knownActors = new HashSet<int>();

        public static void JoinedRoom()
        {
            if (inRoom)
                return;
            inRoom = true;
            knownActors.Clear();
            foreach (Player p in PhotonNetwork.PlayerList)
                if (p != null)
                    knownActors.Add(p.ActorNumber);
            lastMasterActor = PhotonNetwork.MasterClient?.ActorNumber ?? -1;
            NetworkSystem.RaiseJoinedRoom();
        }

        public static void LeftRoom()
        {
            if (!inRoom)
                return;
            inRoom = false;
            knownActors.Clear();
            lastMasterActor = -1;
            NetworkSystem.RaiseLeftRoom();
        }

        public static void MasterSwitched(Player newMaster)
        {
            int actor = newMaster?.ActorNumber ?? -1;
            if (actor == lastMasterActor)
                return;
            lastMasterActor = actor;
            NetworkSystem.RaiseMasterClientSwitched();
        }

        public static void PlayerEntered(Player player)
        {
            if (player == null || !knownActors.Add(player.ActorNumber))
                return;
            NetworkSystem.RaisePlayerJoinedRoom(player);
        }

        public static void PlayerLeft(Player player)
        {
            if (player == null || !knownActors.Remove(player.ActorNumber))
                return;
            NetworkSystem.RaisePlayerLeftRoom(player);
        }
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnJoinedRoom")]
    public static class OnJoinedRoomPatch
    {
        public static void Postfix() =>
            PhotonEventGate.JoinedRoom();
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnLeftRoom")]
    public static class OnLeftRoomPatch
    {
        public static void Postfix() =>
            PhotonEventGate.LeftRoom();
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnMasterClientSwitched")]
    public static class OnMasterClientSwitchedPatch
    {
        public static void Postfix(Player newMasterClient) =>
            PhotonEventGate.MasterSwitched(newMasterClient);
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    public static class OnPlayerEnteredRoomPatch
    {
        public static void Postfix(Player newPlayer) =>
            PhotonEventGate.PlayerEntered(newPlayer);
    }

    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    public static class OnPlayerLeftRoomPatch
    {
        public static void Postfix(Player otherPlayer) =>
            PhotonEventGate.PlayerLeft(otherPlayer);
    }
}