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
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace JupiterX.Mods
{
    public class Spammers
    {
        private static void PlaySound(int index)
        {
            if (PhotonNetwork.InRoom)
            {
                PlaySound(index, false, 9999f);
                //RPCManager.RigRPC("PlayHandTap", RpcTarget.All, new object[] { index, false, 99999f });
            }
            else
            {
                PlayHandTapLocal(index, false, 999999f);
            }
        }

        public static unsafe void PlaySound(params object[] args)
        {
            var array = new Il2CppReferenceArray<Il2CppSystem.Object>(args.Length);
            for (int i = 0; i < args.Length; i++)
            {
                object arg = args[i];
                if (arg == null)
                {
                    array[i] = null;
                }
                else if (arg is Il2CppSystem.Object il2cppObj)
                {
                    array[i] = il2cppObj;
                }
                else if (arg is string str)
                {
                    array[i] = (Il2CppSystem.Object)str;
                }
                else if (arg is int intValue)
                {
                    IntPtr ptr = IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<int>.NativeClassPtr);
                    *(int*)IL2CPP.il2cpp_object_unbox(ptr) = intValue;
                    array[i] = new Il2CppSystem.Object(ptr);
                }
                else if (arg is bool boolValue)
                {
                    IntPtr ptr = IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<bool>.NativeClassPtr);
                    *(bool*)IL2CPP.il2cpp_object_unbox(ptr) = boolValue;
                    array[i] = new Il2CppSystem.Object(ptr);
                }
                else if (arg is float floatValue)
                {
                    IntPtr ptr = IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<float>.NativeClassPtr);
                    *(float*)IL2CPP.il2cpp_object_unbox(ptr) = floatValue;
                    array[i] = new Il2CppSystem.Object(ptr);
                }
            }
            GorillaTagger.Instance.myVRRig.photonView.RPC("PlayHandTap", RpcTarget.All, array);
        }

        public static void PlayHandTapLocal(int soundIndex, bool isLeftHand, float volume)
        {
            var taggerType = Type.GetType("GorillaTagger, Assembly-CSharp");
            var rigType = Type.GetType("VRRig, Assembly-CSharp");
            if (taggerType == null || rigType == null)
                return;
            var instanceProp = taggerType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
            object taggerInstance = instanceProp?.GetValue(null);
            if (taggerInstance == null)
                return;
            var rigField = taggerType.GetField("offlineVRRig", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            object rigInstance = rigField?.GetValue(taggerInstance)
                ?? taggerType.GetProperty("offlineVRRig", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(taggerInstance);
            if (rigInstance == null)
                return;
            var tapMethod = rigType.GetMethod("PlayHandTap", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) ??  rigType.GetMethod("PlayHandTapLocal", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (tapMethod == null)
                return;
            var ps = tapMethod.GetParameters();
            var args = new object[ps.Length];
            bool intSet = false, boolSet = false, floatSet = false;
            for (int i = 0; i < ps.Length; i++)
            {
                var pt = ps[i].ParameterType;
                if (pt == typeof(int) && !intSet) { args[i] = soundIndex; intSet = true; }
                else if (pt == typeof(bool) && !boolSet) { args[i] = isLeftHand; boolSet = true; }
                else if (pt == typeof(float) && !floatSet) { args[i] = Mathf.Clamp01(volume); floatSet = true; }
                else args[i] = ps[i].HasDefaultValue ? ps[i].DefaultValue : pt.IsValueType ? Activator.CreateInstance(pt) : null;
            }
            tapMethod.Invoke(rigInstance, args);
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