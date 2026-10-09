// ============================================================
//  JupiterX | Patches/AntiQuit.cs
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
using System;
using UnityEngine;

namespace JupiterX.Patches
{
    [HarmonyPatch]
    internal class AntiQuit
    {
        public static bool enabled = true;

        [HarmonyPatch(typeof(Application), nameof(Application.Quit), new Type[] { })]
        [HarmonyPrefix]
        private static bool BlockQuit() =>
            !enabled;

        [HarmonyPatch(typeof(Application), nameof(Application.Quit), new Type[] { typeof(int) })]
        [HarmonyPrefix]
        private static bool BlockQuitWithCode(int exitCode) =>
            !enabled;

        [HarmonyPatch(typeof(Environment), nameof(Environment.Exit), new Type[] { typeof(int) })]
        [HarmonyPrefix]
        private static bool BlockExit(int exitCode) =>
            !enabled;

        [HarmonyPatch(typeof(Environment), nameof(Environment.FailFast), new Type[] { typeof(string) })]
        [HarmonyPrefix]
        private static bool BlockFailFast(string message) =>
            !enabled;

        [HarmonyPatch(typeof(Environment), nameof(Environment.FailFast), new Type[] { typeof(string), typeof(Exception) })]
        [HarmonyPrefix]
        private static bool BlockFailFastException(string message, Exception exception) =>
            !enabled;
    }
}