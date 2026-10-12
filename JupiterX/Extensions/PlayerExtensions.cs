// ============================================================
//  JupiterX | Extensions/PlayerExtensions.cs
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
using JupiterX.Mods;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace JupiterX.Extensions
{
    public static class PlayerExtensions
    {
        private static VRRig LocalRig =>
            GorillaTagger.Instance != null && PhotonNetwork.InRoom ? GorillaTagger.Instance.myVRRig : null;

        public static VRRig Rig(this Player plr)
        {
            if (plr == null || GorillaParent.instance == null)
                return null;
            int actor = plr.ActorNumber;
            VRRig local = LocalRig;
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig == null || rig == local)
                    continue;
                Player owner = rig.Owner();
                if (owner != null && owner.ActorNumber == actor)
                    return rig;
            }
            return null;
        }

        public static bool TryGetRig(this Player plr, out VRRig rig)
        {
            rig = plr.Rig();
            return rig != null;
        }

        public static Player Owner(this VRRig rig)
        {
            if (rig == null)
                return null;
            PhotonView view = rig.photonView;
            return view != null ? view.Owner : null;
        }

        public static bool IsLocalRig(this VRRig rig) =>
            rig != null && rig == LocalRig;

        public static bool IsLocal(this Player plr) =>
            plr != null && PhotonNetwork.LocalPlayer != null && plr.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;

        public static bool IsSame(this Player a, Player b) =>
            a != null && b != null && a.ActorNumber == b.ActorNumber;

        public static bool IsMaster(this Player plr) =>
            plr != null && plr.IsMasterClient;

        public static bool IsStillInRoom(this Player plr)
        {
            if (plr == null || !PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom == null)
                return false;
            return PhotonNetwork.CurrentRoom.GetPlayer(plr.ActorNumber) != null;
        }

        public static string SafeName(this Player plr, int length = 12) =>
            plr == null ? "Unknown" : Visual.CleanPlayerName(plr.NickName, length);

        public static string SafeUserId(this Player plr) =>
            plr == null || string.IsNullOrEmpty(plr.UserId) ? "Unknown" : plr.UserId;

        public static bool IsTagged(this Player plr)
        {
            VRRig rig = plr.Rig();
            return rig != null && rig.IsTagged();
        }

        public static string Platform(this Player plr)
        {
            VRRig rig = plr.Rig();
            return rig != null ? rig.GetPlatform() : "Unknown";
        }

        public static Color Color(this Player plr)
        {
            VRRig rig = plr.Rig();
            return rig != null ? rig.playerColor() : UnityEngine.Color.white;
        }

        public static Vector3 HeadPosition(this VRRig rig)
        {
            if (rig == null)
                return Vector3.zero;
            if (rig.headMesh != null)
                return rig.headMesh.transform.position;
            if (rig.headConstraint != null)
                return rig.headConstraint.transform.position;
            return rig.transform.position;
        }

        public static Vector3 HeadPosition(this Player plr)
        {
            VRRig rig = plr.Rig();
            return rig != null ? rig.HeadPosition() : Vector3.zero;
        }

        public static float DistanceToLocal(this VRRig rig)
        {
            if (rig == null || GorillaTagger.Instance == null || GorillaTagger.Instance.headCollider == null)
                return float.MaxValue;
            return Vector3.Distance(GorillaTagger.Instance.headCollider.transform.position, rig.HeadPosition());
        }

        public static float DistanceToLocal(this Player plr)
        {
            VRRig rig = plr.Rig();
            return rig != null ? rig.DistanceToLocal() : float.MaxValue;
        }

        public static VRRig ClosestRemoteRig(bool taggedOnly = false)
        {
            if (GorillaParent.instance == null)
                return null;
            VRRig local = LocalRig;
            VRRig best = null;
            float bestDist = float.MaxValue;
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig == null || rig == local || !rig.gameObject.activeInHierarchy)
                    continue;
                if (taggedOnly && !rig.IsTagged())
                    continue;
                float d = rig.DistanceToLocal();
                if (d < bestDist)
                {
                    bestDist = d;
                    best = rig;
                }
            }
            return best;
        }

        public static Player FindByActor(int actorNumber) =>
            PhotonNetwork.CurrentRoom?.GetPlayer(actorNumber);

        public static Player FindByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId) || PhotonNetwork.PlayerList == null)
                return null;
            foreach (Player p in PhotonNetwork.PlayerList)
                if (p != null && p.UserId == userId)
                    return p;
            return null;
        }

        public static Player FindByName(string name)
        {
            if (string.IsNullOrEmpty(name) || PhotonNetwork.PlayerList == null)
                return null;
            foreach (Player p in PhotonNetwork.PlayerList)
                if (p != null && string.Equals(Visual.CleanPlayerName(p.NickName, 64), name, System.StringComparison.OrdinalIgnoreCase))
                    return p;
            return null;
        }
    }
}