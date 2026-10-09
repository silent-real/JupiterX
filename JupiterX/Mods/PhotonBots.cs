// ============================================================
//  JupiterX | Mods/PhotonBots.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using ExitGames.Client.Photon;
using JupiterX.Classes.Menu;
using JupiterX.Managers;
using JupiterX.Menu;
using JupiterX.Notifications;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnhollowerBaseLib;
using UnityEngine;

namespace JupiterX.Mods
{
    /*
     Thanks to zenith for giving me this <3
     Moca no one glazes you or wtv you say lmao
     Photon bots are ass >w<
     */
    public static class PhotonBots
    {
        public static int botCount = 10;
        public static string botPrefix = "JUPITERX";

        private static readonly List<BotConnection> bots = new List<BotConnection>();
        private static System.Collections.IEnumerator spawnRoutine = null;
        private static bool spawning = false;

        internal static readonly string[] rigPrefabs = new string[]
        {
            "GorillaPrefabs/Gorilla Player Networked",
            "GorillaPrefabs/Gorilla Player Actual"
        };

        internal const byte PUN_INSTANTIATE_EVENT = 202;
        internal const int MAX_VIEW_IDS = 1000;

        public static int BotCount => bots.Count;

        public static string GetCurrentRegion()
        {
            try
            {
                string region = PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion;
                try
                {
                    if (!string.IsNullOrEmpty(PhotonNetwork.CloudRegion))
                        region = PhotonNetwork.CloudRegion.Split('/')[0].Replace("*", "").Replace("/", "");
                    if (string.IsNullOrEmpty(region))
                        region = PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion;
                    if (string.IsNullOrEmpty(region))
                        region = "us";
                }
                catch { region = PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion; }
                if (string.IsNullOrEmpty(region))
                    region = "us";
                return region;
            }
            catch { return "us"; }
        }

        public static void IncreaseBotCount()
        {
            botCount++;
            if (botCount > 50)
                botCount = 50;
            UpdateCountLabel();
        }

        public static void DecreaseBotCount()
        {
            botCount--;
            if (botCount < 1)
                botCount = 1;
            UpdateCountLabel();
        }

        private static void UpdateCountLabel()
        {
            try
            {
                ButtonInfo btn = Buttons.GetIndex("Bot Count");
                if (btn != null)
                    btn.overlapText = "Bot Count <color=grey>[</color><color=cyan>" + botCount + "</color><color=grey>]</color>";
            }
            catch { }
        }

        public static void SpawnBots()
        {
            if (!PhotonNetwork.InRoom)
            {
                NotificationManager.SendNotification("<color=red>[BOTS]</color> Join a room first!", 3f);
                return;
            }
            if (spawning || bots.Count > 0)
            {
                NotificationManager.SendNotification("<color=yellow>[BOTS]</color> Bots already running, stopping first...", 3f);
                StopBots();
            }

            string roomName = PhotonNetwork.CurrentRoom.Name;
            string region = GetCurrentRegion();
            Vector3 center = GetSpawnCenter();
            AppSettings baseCfg = BuildSettings();

            NotificationManager.SendNotification("<color=cyan>[BOTS]</color> Spawning " + botCount + " bots in [" + roomName + "] region [" + region + "]", 4f);

            spawning = true;
            spawnRoutine = BootstrapAndSpawn(roomName, center, baseCfg, botCount);
            Plugin.StartCoroutine(spawnRoutine);
        }

        public static bool botTaps = false;
        public static bool botColors = false;
        public static int tapSoundId = 0;
        private static float tapTimer = 0f;
        private static int tapCursor = 0;
        private static float colorTimer = 0f;
        private static int colorCursor = 0;

        public static void NextTapSound()
        {
            tapSoundId++;
            if (tapSoundId > 61) tapSoundId = 0;
            UpdateTapSoundLabel();
        }

        public static void PrevTapSound()
        {
            tapSoundId--;
            if (tapSoundId < 0) tapSoundId = 61;
            UpdateTapSoundLabel();
        }

        private static void UpdateTapSoundLabel()
        {
            try
            {
                ButtonInfo btn = Buttons.GetIndex("Tap Sound");
                if (btn != null)
                    btn.overlapText = "Tap Sound <color=grey>[</color><color=cyan>" + tapSoundId + "</color><color=grey>]</color>";
            }
            catch { }
        }

        public static bool botEventSpam = false;
        public static bool botPrefabSpam = false;
        public static bool botCrash = false;
        private static float eventSpamTimer = 0f;
        private static float prefabSpamTimer = 0f;
        private static float crashTimer = 0f;

        private static void PumpBotAttack(float delta)
        {
            if (bots.Count == 0) return;
            try
            {
                if (botEventSpam)
                {
                    eventSpamTimer += delta;
                    if (eventSpamTimer >= 0.1f)
                    {
                        eventSpamTimer = 0f;
                        for (int i = 0; i < bots.Count; i++)
                        {
                            try { bots[i].SendJunkEvents(); } catch { }
                        }
                    }
                }
                if (botPrefabSpam)
                {
                    prefabSpamTimer += delta;
                    if (prefabSpamTimer >= 0.2f)
                    {
                        prefabSpamTimer = 0f;
                        for (int i = 0; i < bots.Count; i++)
                        {
                            try { bots[i].SendSpamPrefab(); } catch { }
                        }
                    }
                }
                if (botCrash)
                {
                    crashTimer += delta;
                    if (crashTimer >= 0.2f)
                    {
                        crashTimer = 0f;
                        for (int i = 0; i < bots.Count; i++)
                        {
                            try { bots[i].SendCrashEvents(); } catch { }
                        }
                    }
                }
            }
            catch { }
        }

        private static void PumpBotAudio(float delta)
        {
            if (bots.Count == 0) return;
            try
            {
                if (botTaps)
                {
                    tapTimer += delta;
                    if (tapTimer >= 0.15f)
                    {
                        tapTimer = 0f;
                        int idx = tapCursor % bots.Count;
                        tapCursor++;
                        try { bots[idx].SendTap(tapSoundId); } catch { }
                    }
                }
                if (botColors)
                {
                    colorTimer += delta;
                    if (colorTimer >= 1f)
                    {
                        colorTimer = 0f;
                        int idx = colorCursor % bots.Count;
                        colorCursor++;
                        try
                        {
                            float t = Time.time + idx * 2.1f;
                            float r = Mathf.Sin(t * 1.3f) * 0.5f + 0.5f;
                            float g = Mathf.Sin(t * 1.7f + 2f) * 0.5f + 0.5f;
                            float b = Mathf.Sin(t * 2.1f + 4f) * 0.5f + 0.5f;
                            bots[idx].SendBotColor(r, g, b);
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }

        public static void StopBots()
        {
            spawning = false;
            if (spawnRoutine != null)
            {
                try { Plugin.StopCoroutine(spawnRoutine); } catch { }
                spawnRoutine = null;
            }
            try
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    try { bots[i].Cleanup(); } catch { }
                }
            }
            catch { }
            bots.Clear();
            NotificationManager.SendNotification("<color=cyan>[BOTS]</color> Stopped / cleared bots.", 3f);
        }

        public static int moveMode = 0;
        public static VRRig orbitTarget;
        public static float orbitRadius = 3.5f;
        public static float orbitSpeed = 60f;
        private static float orbitAngle = 0f;
        public static bool freezeBots = false;
        private static readonly string[] botNamePresets = new string[] { "JUPITERX", "ZENITHMODZ", "Novax", "NSWARM", "meorw" }; // NSwarm is a CSwarm reference -silent/nova
        private static int botNamePresetIndex = 0;

        public static void OrbitMe()
        {
            moveMode = 1;
            orbitTarget = null;
            ExclusiveFormation("Orbit Me");
            UpdateMovement();
        }

        public static void StackBots()
        {
            moveMode = 3;
            ExclusiveFormation("Stack Bots");
            UpdateMovement();
        }

        public static void LineBots()
        {
            moveMode = 4;
            ExclusiveFormation("Line Bots");
            UpdateMovement();
        }

        public static void SpazBots()
        {
            moveMode = 5;
            ExclusiveFormation("Spaz Bots");
            UpdateMovement();
        }

        public static void StopMovement()
        {
            moveMode = 0;
            mirrorSource = 0;
            orbitTarget = null;
            ExclusiveFormation();
            ExclusiveMirror();
        }

        public static void StopOrbitGun()
        {

        }

        public static int mirrorSource = 0;

        public static void MirrorMe()
        {
            mirrorSource = 1;
            ExclusiveMirror("Mirror Me");
        }

        public static void StopMirrorMe()
        {
            if (mirrorSource == 1) mirrorSource = 0;
        }

        public static void StopMirrorGun()
        {

        }

        public static void MirrorGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                RaycastHit Ray = GunData.Ray;
                if (Main.GetGunInput(true))
                {
                    try
                    {
                        if (Ray.collider != null)
                        {
                            VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                            VRRig me = null;
                            try { if (GorillaTagger.Instance != null) me = Utility.myVRRig(); } catch { }
                            if (who != null && who != me)
                            {
                                orbitTarget = who;
                                moveMode = 2;
                                mirrorSource = 2;
                                ExclusiveFormation("Orbit Gun");
                                ExclusiveMirror("Mirror Gun");
                            }
                        }
                    }
                    catch { }
                }
            }
            if (moveMode == 2) UpdateMovement();
        }

        private static void ExclusiveMirror(params string[] keep)
        {
            string[] mirrors = new string[] { "Mirror Me", "Mirror Gun" };
            foreach (string name in mirrors)
            {
                bool skip = false;
                foreach (string k in keep)
                {
                    if (k == name) { skip = true; break; }
                }
                if (skip) continue;
                try
                {
                    ButtonInfo b = Buttons.GetIndex(name);
                    if (b != null) b.enabled = false;
                }
                catch { }
            }
        }

        private static void TryGetMirrorPose(Vector3 bodyPos, ref Quaternion headRot, ref Vector3 lPos, ref Quaternion lRot, ref Vector3 rPos, ref Quaternion rRot, ref int yawInt)
        {
            if (mirrorSource == 0) return;
            try
            {
                VRRig src = null;
                bool isMe = false;
                if (mirrorSource == 2)
                {
                    if (!IsOrbitTargetValid()) return;
                    src = orbitTarget;
                }
                else
                {
                    if (GorillaTagger.Instance == null || Utility.myVRRig() == null) return;
                    src = Utility.myVRRig();
                    isMe = true;
                }
                if (src == null) return;

                Vector3 srcBody = src.transform.position;

                try
                {
                    Transform ht = null;
                    if (isMe && GorillaTagger.Instance.headCollider != null)
                        ht = GorillaTagger.Instance.headCollider.transform;
                    if (ht == null)
                    {
                        try { if (src.head != null && src.head.rigTarget != null) ht = src.head.rigTarget.transform; } catch { }
                    }
                    if (ht == null)
                    {
                        try { if (src.headConstraint != null) ht = src.headConstraint.transform; } catch { }
                    }
                    if (ht == null)
                    {
                        try { if (src.headMesh != null) ht = src.headMesh.transform; } catch { }
                    }
                    if (ht != null)
                    {
                        headRot = ht.rotation;
                        try { yawInt = (int)ht.rotation.eulerAngles.y; } catch { }
                    }
                }
                catch { }

                try
                {
                    if (src.leftHandTransform != null)
                    {
                        lPos = bodyPos + (src.leftHandTransform.position - srcBody);
                        lRot = src.leftHandTransform.rotation;
                    }
                }
                catch { }

                try
                {
                    if (src.rightHandTransform != null)
                    {
                        rPos = bodyPos + (src.rightHandTransform.position - srcBody);
                        rRot = src.rightHandTransform.rotation;
                    }
                }
                catch { }
            }
            catch { }
        }

        private static void ExclusiveFormation(params string[] keep)
        {
            string[] formations = new string[] { "Orbit Me", "Orbit Gun", "Stack Bots", "Line Bots", "Spaz Bots" };
            foreach (string name in formations)
            {
                bool skip = false;
                foreach (string k in keep)
                {
                    if (k == name) { skip = true; break; }
                }
                if (skip) continue;
                try
                {
                    ButtonInfo b = Buttons.GetIndex(name);
                    if (b != null) b.enabled = false;
                }
                catch { }
            }
        }

        public static void BringBots()
        {
            if (bots.Count == 0)
            {
                NotificationManager.SendNotification("<color=red>[BOTS]</color> No bots spawned.", 2f);
                return;
            }
            Vector3 center = GetSpawnCenter();
            if (center == Vector3.zero) return;
            try
            {
                int total = bots.Count;
                for (int i = 0; i < total; i++)
                {
                    float a = (float)i * 2.399963f;
                    bots[i].SetTargetPos(center + new Vector3(Mathf.Cos(a) * 1.5f, 0.3f, Mathf.Sin(a) * 1.5f));
                }
            }
            catch { }
        }

        public static void BotsToGun()
        {
            if (bots.Count == 0) return;
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                if (Main.GetGunInput(true))
                {
                    try
                    {
                        Vector3 p = GunData.Pointer.transform.position;
                        int total = bots.Count;
                        for (int i = 0; i < total; i++)
                        {
                            float a = (float)i * 2.399963f;
                            bots[i].SetTargetPos(p + new Vector3(Mathf.Cos(a) * 1.5f, 0.3f, Mathf.Sin(a) * 1.5f));
                        }
                    }
                    catch { }
                }
            }
        }

        public static void NextBotName()
        {
            botNamePresetIndex = (botNamePresetIndex + 1) % botNamePresets.Length;
            botPrefix = botNamePresets[botNamePresetIndex];
            UpdateBotNameLabel();
        }

        public static void PrevBotName()
        {
            botNamePresetIndex = (botNamePresetIndex - 1 + botNamePresets.Length) % botNamePresets.Length;
            botPrefix = botNamePresets[botNamePresetIndex];
            UpdateBotNameLabel();
        }

        private static void UpdateBotNameLabel()
        {
            try
            {
                ButtonInfo btn = Buttons.GetIndex("Bot Name");
                if (btn != null)
                    btn.overlapText = "Bot Name <color=grey>[</color><color=cyan>" + botPrefix + "</color><color=grey>]</color>";
            }
            catch { }
        }

        public static void RadiusUp()
        {
            orbitRadius = Mathf.Min(15f, orbitRadius + 1f);
            UpdateRadiusLabel();
        }

        public static void RadiusDown()
        {
            orbitRadius = Mathf.Max(1f, orbitRadius - 1f);
            UpdateRadiusLabel();
        }

        private static void UpdateRadiusLabel()
        {
            try
            {
                ButtonInfo btn = Buttons.GetIndex("Orbit Radius");
                if (btn != null)
                    btn.overlapText = "Orbit Radius <color=grey>[</color><color=cyan>" + orbitRadius + "</color><color=grey>]</color>";
            }
            catch { }
        }

        public static void SpeedUp()
        {
            orbitSpeed = Mathf.Min(180f, orbitSpeed + 10f);
            UpdateSpeedLabel();
        }

        public static void SpeedDown()
        {
            orbitSpeed = Mathf.Max(0f, orbitSpeed - 10f);
            UpdateSpeedLabel();
        }

        private static void UpdateSpeedLabel()
        {
            try
            {
                ButtonInfo btn = Buttons.GetIndex("Orbit Speed");
                if (btn != null)
                    btn.overlapText = "Orbit Speed <color=grey>[</color><color=cyan>" + orbitSpeed + "</color><color=grey>]</color>";
            }
            catch { }
        }

        public static void OrbitGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                RaycastHit Ray = GunData.Ray;
                if (Main.GetGunInput(true))
                {
                    try
                    {
                        if (Ray.collider != null)
                        {
                            VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                            VRRig me = null;
                            try { if (GorillaTagger.Instance != null) me = Utility.myVRRig(); } catch { }
                            if (who != null && who != me)
                            {
                                orbitTarget = who;
                                moveMode = 2;
                                try
                                {
                                    ButtonInfo meBtn = Buttons.GetIndex("Orbit Me");
                                    if (meBtn != null) meBtn.enabled = false;
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
            }
            if (moveMode == 2)
                UpdateOrbitPositions();
        }

        private static string GetRigDisplayName(VRRig rig)
        {
            try
            {
                if (rig != null && rig.photonView != null && rig.photonView.Owner != null && !string.IsNullOrEmpty(rig.photonView.Owner.NickName))
                    return rig.photonView.Owner.NickName;
            }
            catch { }
            return "player";
        }

        private static bool IsOrbitTargetValid()
        {
            try
            {
                if (orbitTarget == null) return false;
                if (orbitTarget.gameObject == null) return false;
                if (orbitTarget.photonView == null || orbitTarget.photonView.Owner == null) return false;
                return true;
            }
            catch { return false; }
        }

        private static Vector3 GetOrbitCenter()
        {
            try
            {
                if (moveMode == 2 && IsOrbitTargetValid())
                    return orbitTarget.transform.position;
                if (moveMode == 1 || moveMode == 2)
                    return GetSpawnCenter();
            }
            catch { }
            return Vector3.zero;
        }

        private static void UpdateOrbitPositions()
        {
            if (bots.Count == 0) return;
            Vector3 center = GetOrbitCenter();
            if (center == Vector3.zero) return;
            try
            {
                orbitAngle += orbitSpeed * Time.deltaTime;
                if (orbitAngle >= 360f) orbitAngle -= 360f;
                int total = bots.Count;
                for (int i = 0; i < total; i++)
                {
                    float deg = orbitAngle + (360f / total) * i;
                    float rad = deg * 0.0174532924f;
                    bots[i].SetTargetPos(center + new Vector3(Mathf.Cos(rad) * orbitRadius, 0.3f, Mathf.Sin(rad) * orbitRadius));
                }
            }
            catch { }
        }

        private static void UpdateMovement()
        {
            switch (moveMode)
            {
                case 1:
                case 2:
                    UpdateOrbitPositions();
                    break;
                case 3:
                    UpdateStackPositions();
                    break;
                case 4:
                    UpdateLinePositions();
                    break;
                case 5:
                    UpdateSpazPositions();
                    break;
            }
        }

        private static void UpdateStackPositions()
        {
            if (bots.Count == 0) return;
            Vector3 center = GetSpawnCenter();
            if (center == Vector3.zero) return;
            try
            {
                for (int i = 0; i < bots.Count; i++)
                    bots[i].SetTargetPos(center + new Vector3(0f, 1f + i * 1.2f, 0f));
            }
            catch { }
        }

        private static void UpdateLinePositions()
        {
            if (bots.Count == 0) return;
            Vector3 center = GetSpawnCenter();
            if (center == Vector3.zero) return;
            try
            {
                Vector3 fwd = Vector3.forward;
                try
                {
                    if (GorillaTagger.Instance != null && GorillaTagger.Instance.headCollider != null)
                    {
                        Vector3 f = GorillaTagger.Instance.headCollider.transform.forward;
                        f.y = 0f;
                        if (f.sqrMagnitude > 0.001f) fwd = f.normalized;
                    }
                }
                catch { }
                Vector3 right = new Vector3(fwd.z, 0f, -fwd.x);
                int total = bots.Count;
                for (int i = 0; i < total; i++)
                    bots[i].SetTargetPos(center + right * ((i - (total - 1) / 2f) * 2f) + new Vector3(0f, 0.3f, 0f));
            }
            catch { }
        }

        private static void UpdateSpazPositions()
        {
            if (bots.Count == 0) return;
            Vector3 center = GetSpawnCenter();
            if (center == Vector3.zero) return;
            try
            {
                for (int i = 0; i < bots.Count; i++)
                    bots[i].SetTargetPos(center + new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(0.3f, 3.5f), UnityEngine.Random.Range(-3f, 3f)));
            }
            catch { }
        }

        public static List<Vector3> GetBotPositions()
        {
            List<Vector3> list = new List<Vector3>();
            try
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    try { if (bots[i] != null) list.Add(bots[i].GetTargetPos()); } catch { }
                }
            }
            catch { }
            return list;
        }

        internal static Vector3 GetSpawnCenter()
        {
            try
            {
                if (GorillaTagger.Instance != null)
                {
                    if (GorillaTagger.Instance.bodyCollider != null)
                        return GorillaTagger.Instance.bodyCollider.transform.position;
                    if (GorillaTagger.Instance.headCollider != null)
                        return GorillaTagger.Instance.headCollider.transform.position;
                }
                if (GorillaLocomotion.Player.Instance != null)
                    return GorillaLocomotion.Player.Instance.transform.position;
            }
            catch { }
            return new Vector3(-64f, 12.5f, -83f);
        }

        private static AppSettings BuildSettings()
        {
            var src = PhotonNetwork.PhotonServerSettings.AppSettings;
            AppSettings cfg = new AppSettings();
            try { src.CopyTo(cfg); } catch { }
            try
            {
                string region = GetCurrentRegion();
                if (!string.IsNullOrEmpty(region))
                    cfg.FixedRegion = region;
            }
            catch { }
            try
            {
                if (PhotonNetwork.NetworkingClient != null && !string.IsNullOrEmpty(PhotonNetwork.NetworkingClient.AppVersion))
                    cfg.AppVersion = PhotonNetwork.NetworkingClient.AppVersion;
            }
            catch { }
            try { cfg.UseNameServer = true; } catch { }
            return cfg;
        }

        private static AppSettings CloneSettings(AppSettings src)
        {
            AppSettings cfg = new AppSettings();
            try
            {
                if (src != null)
                    src.CopyTo(cfg);
                else
                    return BuildSettings();
            }
            catch { return BuildSettings(); }
            return cfg;
        }

        private static System.Collections.IEnumerator BootstrapAndSpawn(string roomName, Vector3 center, AppSettings baseCfg, int count)
        {
            int spawned = 0;
            while (spawned < count)
            {
                if (!spawning || !PhotonNetwork.InRoom)
                    break;

                string name = botPrefix + (spawned + 1).ToString("00");
                float angle = (float)spawned * 2.399963f;
                Vector3 spawnPos = center + new Vector3(Mathf.Cos(angle) * 1.5f, 0.3f, Mathf.Sin(angle) * 1.5f);

                try
                {
                    BotConnection bot = new BotConnection(name, CloneSettings(baseCfg), roomName, spawnPos, spawned);
                    bots.Add(bot);
                    bot.Connect();
                }
                catch (Exception ex)
                {
                    Utility.Log("[PhotonBots] Failed to create bot " + name + ": " + ex.Message, JupxLogType.Error);
                }
                spawned++;
                float waited = 0f;
                while (waited < 0.5f)
                {
                    if (!spawning) break;
                    PumpAll();
                    waited += Time.deltaTime;
                    yield return null;
                }
            }
            while (spawning && bots.Count > 0)
            {
                if (!PhotonNetwork.InRoom)
                    break;
                PumpAll();
                yield return null;
            }

            spawning = false;
            spawnRoutine = null;
        }

        private static void PumpAll()
        {
            try
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    try
                    {
                        bots[i].Service();
                        bots[i].Tick();
                    }
                    catch { }
                }
                try { PumpBotAudio(Time.deltaTime); } catch { }
                try { PumpBotAttack(Time.deltaTime); } catch { }
            }
            catch { }
        }

        private class BotConnection
        {
            private readonly string botName;
            private readonly AppSettings settings;
            private string roomName;
            private readonly Vector3 spawnPos;
            private Vector3 targetPos;
            private readonly int botIndex;
            private readonly LoadBalancingClient client;
            private bool inRoom = false;
            private bool rigSent = false;
            private int rigViewId = -1;
            private int lastRigSendTick = 0;
            private bool lobbySent = false;
            private bool joinSent = false;
            private float lobbyRetryTime = 0f;
            private float joinRetryTime = 0f;

            public BotConnection(string botName, AppSettings settings, string roomName, Vector3 spawnPos, int botIndex)
            {
                this.botName = botName;
                this.settings = settings;
                this.roomName = roomName;
                this.spawnPos = spawnPos;
                this.targetPos = spawnPos;
                this.botIndex = botIndex;
                ExitGames.Client.Photon.ConnectionProtocol protocol = ExitGames.Client.Photon.ConnectionProtocol.Udp;
                try { protocol = PhotonNetwork.PhotonServerSettings.AppSettings.Protocol; } catch { }
                client = new LoadBalancingClient(protocol);
                try { client.AuthValues = new AuthenticationValues { UserId = Guid.NewGuid().ToString() }; } catch { }
                try { client.NickName = botName; } catch { }
            }

            public void Connect()
            {
                try { client.ConnectUsingSettings(settings); } catch { }
            }

            public void SetTargetPos(Vector3 pos)
            {
                try { targetPos = pos; } catch { }
            }

            public Vector3 GetTargetPos()
            {
                try { return targetPos; } catch { return Vector3.zero; }
            }

            public void Service()
            {
                if (client != null)
                {
                    try { client.Service(); } catch { }
                }
            }

            public void Tick()
            {
                if (client == null)
                    return;
                try
                {
                    if (!inRoom)
                    {
                        if (client.InRoom)
                        {
                            inRoom = true;
                            joinSent = false;
                            try
                            {
                                if (client.LocalPlayer != null)
                                {
                                    if (client.LocalPlayer.NickName != botName)
                                        client.LocalPlayer.NickName = botName;
                                    try { client.LocalPlayer.SetPlayerNameProperty(); } catch { }
                                }
                            }
                            catch { }
                            CreateRig();
                            return;
                        }
                        if (client.InLobby)
                        {
                            string follow = null;
                            try { if (PhotonNetwork.InRoom) follow = PhotonNetwork.CurrentRoom.Name; } catch { }
                            if (!string.IsNullOrEmpty(follow) && follow != roomName)
                            {
                                roomName = follow;
                                joinSent = false;
                            }
                            if (!joinSent || Time.time > joinRetryTime)
                            {
                                try
                                {
                                    if (client.OpJoinRoom(new EnterRoomParams { RoomName = roomName }))
                                    {
                                        joinSent = true;
                                        joinRetryTime = Time.time + 3f;
                                    }
                                }
                                catch { joinRetryTime = Time.time + 3f; }
                            }
                            return;
                        }
                        bool ready = false;
                        try { ready = client.IsConnectedAndReady; } catch { }
                        if (ready)
                        {
                            if (!lobbySent || Time.time > lobbyRetryTime)
                            {
                                try
                                {
                                    if (client.OpJoinLobby(TypedLobby.Default))
                                    {
                                        lobbySent = true;
                                        lobbyRetryTime = Time.time + 3f;
                                    }
                                }
                                catch { lobbyRetryTime = Time.time + 3f; }
                            }
                        }
                        else
                        {
                            lobbySent = false;
                            joinSent = false;
                        }
                        return;
                    }
                    if (!client.InRoom)
                    {
                        inRoom = false;
                        rigSent = false;
                        joinSent = false;
                        return;
                    }
                    int now = Environment.TickCount;
                    if (!PhotonBots.freezeBots && now - lastRigSendTick > 100)
                    {
                        lastRigSendTick = now;
                        UpdateRig(now);
                    }
                }
                catch { }
            }

            public void Cleanup()
            {
                try
                {
                    if (client != null && client.IsConnected)
                        client.Disconnect(DisconnectCause.DisconnectByClientLogic);
                }
                catch { }
            }

            private void UpdateRig(int timestamp)
            {
                if (client == null || client.CurrentRoom == null || client.LocalPlayer == null || rigViewId < 0)
                    return;
                try
                {
                    float yaw = (botIndex * 47f) % 360f;
                    int yawInt = (int)yaw;
                    Quaternion headRot = Quaternion.Euler(0f, yaw, 0f);
                    Vector3 lPos = new Vector3(-1f, -0.45f, 0f);
                    Quaternion lRot = Quaternion.identity;
                    Vector3 rPos = new Vector3(-1f, -0.45f, 0f);
                    Quaternion rRot = Quaternion.Euler(0f, yaw, 0f);
                    try { PhotonBots.TryGetMirrorPose(targetPos, ref headRot, ref lPos, ref lRot, ref rPos, ref rRot, ref yawInt); } catch { }
                    object[] viewData = new object[]
                    {
                        rigViewId,
                        false,
                        null,
                        headRot,
                        lPos,
                        lRot,
                        rPos,
                        rRot,
                        targetPos,
                        yawInt,
                        0,
                        0
                    };
                    object[] eventData = new object[]
                    {
                        timestamp,
                        null,
                        viewData
                    };
                    client.OpRaiseEvent(206, BoxManager.BoxAny(eventData), new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.DoNotCache
                    }, SendOptions.SendUnreliable);
                }
                catch { }
            }

            private void CreateRig()
            {
                if (client == null || client.CurrentRoom == null || client.LocalPlayer == null) return;
                if (rigSent) return;
                try
                {
                    int actor = client.LocalPlayer.ActorNumber;
                    int baseViewId = actor * PhotonBots.MAX_VIEW_IDS;
                    int[] viewIds = { baseViewId + 1, baseViewId + 2, baseViewId + 3 };
                    rigViewId = viewIds[0];
                    spamViewId = baseViewId + 10;
                    int timestamp = Environment.TickCount;
                    foreach (string prefab in PhotonBots.rigPrefabs)
                    {
                        try { SendInstantiate(prefab, spawnPos, viewIds, timestamp); } catch { }
                    }
                    rigSent = true;
                }
                catch { }
            }

            public void SendRpc(string methodName, object[] args)
            {
                if (client == null || client.CurrentRoom == null || client.LocalPlayer == null || rigViewId < 0) return;
                try
                {
                    ExitGames.Client.Photon.Hashtable rpcData = new ExitGames.Client.Photon.Hashtable();
                    rpcData.Add((byte)0, BoxManager.BoxAny(rigViewId));
                    rpcData.Add((byte)2, BoxManager.BoxAny(Environment.TickCount));
                    rpcData.Add((byte)3, BoxManager.BoxAny(methodName));
                    rpcData.Add((byte)4, BoxManager.BoxAny(args));
                    client.OpRaiseEvent(200, rpcData, new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.DoNotCache
                    }, SendOptions.SendReliable);
                }
                catch { }
            }

            public void SendTap(int tapId)
            {
                if (!inRoom) return;
                try { SendRpc("PlayHandTap", new object[] { tapId, false, 99999f }); } catch { }
            }

            public void SendBotColor(float r, float g, float b)
            {
                if (!inRoom) return;
                try { SendRpc("InitializeNoobMaterial", new object[] { r, g, b }); } catch { }
            }

            private int spamViewId = -1;
            private int spamAlt = 0;
            public void SendJunkEvents()
            {
                if (!inRoom) return;
                try
                {
                    RaiseEventOptions opts = new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.DoNotCache
                    };
                    for (int i = 0; i < 2; i++)
                    {
                        ExitGames.Client.Photon.Hashtable junk = new ExitGames.Client.Photon.Hashtable();
                        junk.Add((byte)0, new Il2CppSystem.Object());
                        junk.Add((byte)1, new Il2CppSystem.Object());
                        junk.Add((byte)2, new Il2CppSystem.Object());
                        junk.Add((byte)3, new Il2CppSystem.Object());
                        junk.Add((byte)4, new Il2CppSystem.Object());
                        client.OpRaiseEvent(207, junk, opts, SendOptions.SendUnreliable);
                    }
                }
                catch { }
            }

            public void SendSpamPrefab()
            {
                if (!inRoom) return;
                try
                {
                    if (spamViewId < 0) return;
                    int vid = spamViewId++;
                    spamAlt++;
                    string prefab = (spamAlt % 2 == 0) ? "bulletPrefab" : "STICKABLE TARGET";
                    Vector3 pos = targetPos + new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(0f, 3f), UnityEngine.Random.Range(-3f, 3f));
                    var views = new Il2CppStructArray<int>(1);
                    views[0] = vid;
                    ExitGames.Client.Photon.Hashtable data = new ExitGames.Client.Photon.Hashtable();
                    data.Add((byte)0, BoxManager.BoxAny(prefab));
                    data.Add((byte)1, BoxManager.BoxAny(pos));
                    data.Add((byte)2, BoxManager.BoxAny(Quaternion.identity));
                    data.Add((byte)3, BoxManager.BoxAny((byte)0));
                    data.Add((byte)4, views.Cast<Il2CppSystem.Object>());
                    data.Add((byte)5, null);
                    data.Add((byte)6, BoxManager.BoxAny(Environment.TickCount));
                    data.Add((byte)7, BoxManager.BoxAny(vid));
                    client.OpRaiseEvent(PhotonBots.PUN_INSTANTIATE_EVENT, data, new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.DoNotCache
                    }, SendOptions.SendReliable);
                }
                catch { }
            }

            public void SendCrashEvents()
            {
                if (!inRoom) return;
                try
                {
                    RaiseEventOptions opts = new RaiseEventOptions
                    {
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.DoNotCache
                    };
                    for (int i = 0; i < 4; i++)
                    {
                        client.OpRaiseEvent(2, null, opts, SendOptions.SendUnreliable);
                        client.OpRaiseEvent(3, null, opts, SendOptions.SendUnreliable);
                    }
                }
                catch { }
            }

            private void SendInstantiate(string prefabName, Vector3 pos, int[] viewIds, int timestamp)
            {
                var views = new Il2CppStructArray<int>(viewIds.Length);
                for (int i = 0; i < viewIds.Length; i++)
                    views[i] = viewIds[i];
                ExitGames.Client.Photon.Hashtable data = new ExitGames.Client.Photon.Hashtable();
                data.Add((byte)0, BoxManager.BoxAny(prefabName));
                data.Add((byte)1, BoxManager.BoxAny(pos));
                data.Add((byte)2, BoxManager.BoxAny(Quaternion.identity));
                data.Add((byte)3, BoxManager.BoxAny((byte)0));
                data.Add((byte)4, views.Cast<Il2CppSystem.Object>());
                data.Add((byte)5, null);
                data.Add((byte)6, BoxManager.BoxAny(timestamp));
                data.Add((byte)7, BoxManager.BoxAny(viewIds[0]));
                client.OpRaiseEvent(PhotonBots.PUN_INSTANTIATE_EVENT, data, new RaiseEventOptions // RaiseEvent leaked oh no -nova
                {
                    Receivers = ReceiverGroup.Others,
                    CachingOption = EventCaching.AddToRoomCache
                }, SendOptions.SendReliable);
            }
        }
    }
}