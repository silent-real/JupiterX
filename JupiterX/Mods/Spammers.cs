// ============================================================
//  JupiterX | Mods/Spammers.cs
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
using JupiterX.Managers;
using JupiterX.Menu;
using Photon.Pun;
using System;
using System.Reflection;
using UnhollowerRuntimeLib;

namespace JupiterX.Mods
{
    public class Spammers
    {
        private static void PlaySound(int index)
        {
            if (PhotonNetwork.InRoom)
            {
                RPCManager.RigRPC("PlayHandTap", RpcTarget.All, new object[] { index, false, 99999f });
            }
            else
            {
                var vrrigType = Type.GetType("VRRig, Assembly-CSharp");
                if (vrrigType == null)
                    return;
                var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var method = vrrigType.GetMethod("PlayHandTap", flags);
                if (method == null)
                {
                    method = vrrigType.GetMethod("PlayHandTapLocal", flags);
                    if (method == null)
                        return;
                }
                var rigs = UnityEngine.Object.FindObjectsOfType(Il2CppType.From(vrrigType));
                if (rigs == null || rigs.Length == 0)
                    return;
                method.Invoke(rigs[0], new object[] { index, false, 1f });
            }
        }

        public static void HandTapSpam()
        {
            if (Utility.RightGrip)
                PlaySound(0);
            if (Utility.LeftGrip)
                PlaySound(0);
        }
        public static void BarkSpam()
        {
            if (Utility.RightGrip)
                PlaySound(8);
            if (Utility.LeftGrip)
                PlaySound(8);
        }
        public static void CrystalSpam()
        {
            if (Utility.RightGrip)
                PlaySound(21);
            if (Utility.LeftGrip)
                PlaySound(21);
        }
        public static void MetalSpam()
        {
            if (Utility.RightGrip)
                PlaySound(19);
            if (Utility.LeftGrip)
                PlaySound(19);
        }
        public static void GlassSpam()
        {
            if (Utility.RightGrip)
                PlaySound(30);
            if (Utility.LeftGrip)
                PlaySound(30);
        }
        public static void SnowStepSpam()
        {
            if (Utility.RightGrip)
                PlaySound(32);
            if (Utility.LeftGrip)
                PlaySound(32);
        }
        public static void LeafCrunchSpam()
        {
            if (Utility.RightGrip)
                PlaySound(31);
            if (Utility.LeftGrip)
                PlaySound(31);
        }
        public static void RandomSpam()
        {
            if (Utility.RightGrip)
                PlaySound(UnityEngine.Random.Range(0, 61));
            if (Utility.LeftGrip)
                PlaySound(UnityEngine.Random.Range(0, 61));
        }

        public static int soundId;
        public static void DecreaseSoundID()
        {
            soundId--;
            if (soundId < 0)
                soundId = GorillaLocomotion.Player.Instance.materialData.Count - 1;
            Buttons.GetIndex("Custom Sound Spam").overlapText = "Custom Sound Spam <color=grey>[</color><color=cyan>" + soundId + "</color><color=grey>]</color>";
        }

        public static void IncreaseSoundID()
        {
            soundId++;
            soundId %= GorillaLocomotion.Player.Instance.materialData.Count;
            Buttons.GetIndex("Custom Sound Spam").overlapText = "Custom Sound Spam <color=grey>[</color><color=cyan>" + soundId + "</color><color=grey>]</color>";
        }

        public static void CustomSoundSpam()
        {
            if (Utility.RightGrip)
                PlaySound(soundId);
            if (Utility.LeftGrip)
                PlaySound(soundId);
        }
    }
}