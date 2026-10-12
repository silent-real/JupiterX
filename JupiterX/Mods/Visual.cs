// ============================================================
//  JupiterX | Mods/Visual.cs
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
using GorillaNetworking;
using JupiterX.Menu;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

namespace JupiterX.Mods
{
    public class Visual
    {
        private static readonly HashSet<VRRig> scratchLive = new HashSet<VRRig>();
        private static readonly List<VRRig> scratchStale = new List<VRRig>();

        private static Camera cachedCam;
        private static int camFrame = -1;
        private static Camera MainCam
        {
            get
            {
                if (camFrame != Time.frameCount)
                {
                    camFrame = Time.frameCount;
                    cachedCam = Camera.main;
                }
                return cachedCam;
            }
        }

        private static VRRig cachedLocal;
        private static int localFrame = -1;
        private static VRRig LocalRig
        {
            get
            {
                if (localFrame != Time.frameCount)
                {
                    localFrame = Time.frameCount;
                    cachedLocal = Utility.myVRRig();
                }
                return cachedLocal;
            }
        }

        private static Font builtinFont;
        private static Font BuiltinFont =>
            builtinFont != null ? builtinFont : (builtinFont = Resources.GetBuiltinResource<Font>("Arial.ttf"));

        private static bool IsRemoteRig(VRRig rig, VRRig local) =>
            rig != null && rig != local && rig.gameObject.activeInHierarchy;

        private static bool CanRunInRoom() =>
            PhotonNetwork.InRoom && GorillaParent.instance != null;

        private static void PruneStale<T>(Dictionary<VRRig, T> pool, HashSet<VRRig> live) where T : Component
        {
            scratchStale.Clear();
            foreach (var kvp in pool)
            {
                if (kvp.Key == null || kvp.Value == null || !live.Contains(kvp.Key))
                    scratchStale.Add(kvp.Key);
            }

            foreach (VRRig rig in scratchStale)
            {
                if (pool.TryGetValue(rig, out T comp) && comp != null)
                    UnityEngine.Object.Destroy(comp.gameObject);
                pool.Remove(rig);
            }
        }

        private static void ClearPool<T>(Dictionary<VRRig, T> pool) where T : Component
        {
            foreach (T comp in pool.Values)
            {
                if (comp != null)
                    UnityEngine.Object.Destroy(comp.gameObject);
            }
            pool.Clear();
        }

        public static TrailRenderer trailRenderer;
        public static void DrawGun()
        {
            if (!Main.GetGunInput(false))
                return;

            var GunData = Main.RenderGun();
            GameObject NewPointer = GunData.Pointer;

            if (trailRenderer == null)
            {
                GameObject trailHolder = new GameObject("JupiterX_DrawGunTrail");
                trailRenderer = trailHolder.AddComponent<TrailRenderer>();
                trailRenderer.startWidth = 0.1f;
                trailRenderer.endWidth = 0.1f;
                trailRenderer.minVertexDistance = 0.05f;
                trailRenderer.material = new Material(Utility.GUIShader());
                trailRenderer.time = float.PositiveInfinity;
                trailRenderer.startColor = Color.black;
                trailRenderer.endColor = Color.black;
                trailRenderer.shadowCastingMode = ShadowCastingMode.Off;
                trailRenderer.receiveShadows = false;
            }

            trailRenderer.emitting = Main.GetGunInput(true);
            trailRenderer.transform.position = NewPointer.transform.position;
        }

        public static void DisableDrawGun()
        {
            if (trailRenderer != null)
                UnityEngine.Object.Destroy(trailRenderer.gameObject);
            trailRenderer = null;
        }

        public static readonly List<Renderer> disabledRenderers = new List<Renderer>();
        private static bool xrayActive;

        public static void Xray()
        {
            if (Utility.RightTrigger)
            {
                if (xrayActive)
                    return;

                xrayActive = true;
                foreach (Renderer renderer in UnityEngine.Object.FindObjectsOfType<Renderer>())
                {
                    if (renderer == null || renderer is SkinnedMeshRenderer || !renderer.enabled || !renderer.gameObject.activeSelf)
                        continue;
                    renderer.enabled = false;
                    disabledRenderers.Add(renderer);
                }
            }
            else if (xrayActive)
            {
                DisableXray();
            }
        }

        public static void DisableXray()
        {
            foreach (Renderer renderer in disabledRenderers)
            {
                if (renderer != null)
                    renderer.enabled = true;
            }
            disabledRenderers.Clear();
            xrayActive = false;
        }

        public static void NoSmoothRigs()
        {
            if (!CanRunInRoom())
                return;

            VRRig local = LocalRig;
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig == null || rig == local)
                    continue;
                rig.lerpValueBody = 2f;
                rig.lerpValueFingers = 1f;
            }
        }

        public static void ReSmoothRigs()
        {
            if (!CanRunInRoom())
                return;

            VRRig local = LocalRig;
            if (local == null)
                return;

            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig == null || rig == local)
                    continue;
                rig.lerpValueBody = local.lerpValueBody;
                rig.lerpValueFingers = local.lerpValueFingers;
            }
        }

        public static readonly Dictionary<VRRig, Renderer> boxEspPool = new Dictionary<VRRig, Renderer>();
        public static readonly Dictionary<VRRig, Renderer> capsuleEspPool = new Dictionary<VRRig, Renderer>();
        public static readonly Dictionary<VRRig, Renderer> sphereEspPool = new Dictionary<VRRig, Renderer>();

        private static Material espTaggedMat;
        private static Material espNormalMat;

        private static void EnsureEspMaterials()
        {
            if (espTaggedMat == null)
                espTaggedMat = new Material(Utility.GUIShader()) { color = Color.red };
            if (espNormalMat == null)
                espNormalMat = new Material(Utility.GUIShader()) { color = Color.grey };
        }

        private static void UpdateShapeESP(Dictionary<VRRig, Renderer> pool, PrimitiveType shape)
        {
            if (!CanRunInRoom())
            {
                ClearPool(pool);
                return;
            }

            EnsureEspMaterials();
            VRRig local = LocalRig;
            scratchLive.Clear();

            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (!IsRemoteRig(rig, local))
                    continue;

                scratchLive.Add(rig);

                if (!pool.TryGetValue(rig, out Renderer rend) || rend == null)
                {
                    GameObject obj = GameObject.CreatePrimitive(shape);
                    obj.name = "JupiterX_ESP_" + shape;
                    UnityEngine.Object.Destroy(obj.GetComponent<Collider>());
                    obj.transform.localScale = Vector3.one * 0.2f;

                    rend = obj.GetComponent<Renderer>();
                    rend.shadowCastingMode = ShadowCastingMode.Off;
                    rend.receiveShadows = false;
                    pool[rig] = rend;
                }

                rend.transform.SetPositionAndRotation(rig.headConstraint.transform.position, rig.transform.rotation);

                Material want = rig.IsTagged() ? espTaggedMat : espNormalMat;
                if (rend.sharedMaterial != want)
                    rend.sharedMaterial = want;
            }

            PruneStale(pool, scratchLive);
        }

        public static void BoxESP() => UpdateShapeESP(boxEspPool, PrimitiveType.Cube);
        public static void DisableBoxESP() => ClearPool(boxEspPool);
        public static void CapsuleESP() => UpdateShapeESP(capsuleEspPool, PrimitiveType.Capsule);
        public static void DisableCapsuleESP() => ClearPool(capsuleEspPool);
        public static void SphereESP() => UpdateShapeESP(sphereEspPool, PrimitiveType.Sphere);
        public static void DisableSphereESP() => ClearPool(sphereEspPool);

        public static readonly Dictionary<VRRig, TextMesh> nameTagPool = new Dictionary<VRRig, TextMesh>();
        public static readonly Dictionary<VRRig, TextMesh> IDnameTagPool = new Dictionary<VRRig, TextMesh>();
        public static readonly Dictionary<VRRig, TextMesh> PlatformnameTagPool = new Dictionary<VRRig, TextMesh>();
        public static readonly Dictionary<VRRig, TextMesh> MasternameTagPool = new Dictionary<VRRig, TextMesh>();
        public static readonly Dictionary<VRRig, TextMesh> TaggednameTagPool = new Dictionary<VRRig, TextMesh>();

        private static TextMesh CreateTextMesh(string name, int fontSize, float characterSize, FontStyle style = FontStyle.Normal)
        {
            GameObject holder = new GameObject(name);
            TextMesh tag = holder.AddComponent<TextMesh>();
            tag.font = BuiltinFont;
            tag.fontSize = fontSize;
            tag.characterSize = characterSize;
            tag.fontStyle = style;
            tag.anchor = TextAnchor.MiddleCenter;
            tag.alignment = TextAlignment.Center;

            MeshRenderer mr = holder.GetComponent<MeshRenderer>();
            mr.sharedMaterial = BuiltinFont.material;
            mr.shadowCastingMode = ShadowCastingMode.Off;
            mr.receiveShadows = false;
            return tag;
        }

        private static void FaceCamera(Transform t, Camera cam)
        {
            if (cam == null)
                return;
            t.LookAt(cam.transform);
            t.Rotate(0f, 180f, 0f);
        }

        private static void UpdateTags(Dictionary<VRRig, TextMesh> pool, int line, Func<VRRig, Photon.Realtime.Player, string> getText)
        {
            if (!CanRunInRoom())
            {
                ClearPool(pool);
                return;
            }

            VRRig local = LocalRig;
            Camera cam = MainCam;
            scratchLive.Clear();

            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (!IsRemoteRig(rig, local))
                    continue;

                Photon.Realtime.Player owner = rig.photonView != null ? rig.photonView.Owner : null;
                if (owner == null)
                    continue;

                scratchLive.Add(rig);

                if (!pool.TryGetValue(rig, out TextMesh tag) || tag == null)
                {
                    tag = CreateTextMesh("JupiterX_NameTag_" + line, 38, 0.03f);
                    pool[rig] = tag;
                }

                string text = getText(rig, owner);
                if (tag.text != text)
                    tag.text = text;
                tag.color = rig.playerColor();

                Transform t = tag.transform;
                t.position = rig.headConstraint.transform.position + new Vector3(0f, 1.15f - line * 0.15f, 0f);
                FaceCamera(t, cam);
            }

            PruneStale(pool, scratchLive);
        }

        public static void NameTags() =>
            UpdateTags(nameTagPool, 0, (rig, owner) => CleanPlayerName(owner.NickName));
        public static void DisableNameTags() =>
            ClearPool(nameTagPool);
        public static void IDNameTags() =>
            UpdateTags(IDnameTagPool, 1, (rig, owner) => owner.UserId);
        public static void DisableIDNameTags() =>
            ClearPool(IDnameTagPool);

        private static readonly Dictionary<VRRig, string> platformCache = new Dictionary<VRRig, string>();
        private static string GetPlatformCached(VRRig rig, Photon.Realtime.Player owner)
        {
            if (!platformCache.TryGetValue(rig, out string p) || p == null)
            {
                p = rig.GetPlatform();
                platformCache[rig] = p;
            }
            return p;
        }
        public static void PlatformTags() =>
            UpdateTags(PlatformnameTagPool, 2, GetPlatformCached);
        public static void DisablePlatformNameTags()
        {
            ClearPool(PlatformnameTagPool);
            platformCache.Clear();
        }

        public static void MasterTags() =>
            UpdateTags(MasternameTagPool, 3, (rig, owner) => owner.IsMasterClient ? "Master" : "Not Master");
        public static void DisableMasterNameTags() =>
            ClearPool(MasternameTagPool);
        public static void TaggedTags() =>
            UpdateTags(TaggednameTagPool, 4, (rig, owner) => rig.IsTagged() ? "Tagged" : "");
        public static void DisableTaggedNameTags() =>
            ClearPool(TaggednameTagPool);

        private static readonly Regex RichTextRegex = new Regex("<.*?>", RegexOptions.IgnoreCase);
        private static readonly Dictionary<string, string> cleanNameCache = new Dictionary<string, string>();

        public static string NoRichtextTags(string input, string replace = "")
        {
            input ??= "";
            return RichTextRegex.Replace(input, replace);
        }

        public static string CleanPlayerName(string input, int length = 12)
        {
            input ??= "";
            bool useCache = length == 12;
            if (useCache && cleanNameCache.TryGetValue(input, out string cached))
                return cached;

            string result = NoRichtextTags(input);
            if (result.Length > length)
                result = result[..length];

            if (useCache)
            {
                if (cleanNameCache.Count > 256)
                    cleanNameCache.Clear();
                cleanNameCache[input] = result;
            }
            return result;
        }

        private static readonly Dictionary<VRRig, LineRenderer> tracersPool = new Dictionary<VRRig, LineRenderer>();
        private static Material tracerMaterial;

        public static void Tracers()
        {
            if (!CanRunInRoom())
            {
                CleanUpTracers();
                return;
            }

            VRRig localRig = LocalRig;
            Vector3 handPos = Utility.RightHandTransform().position;
            scratchLive.Clear();

            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (!IsRemoteRig(rig, localRig) || rig.headMesh == null)
                    continue;

                scratchLive.Add(rig);

                if (!tracersPool.TryGetValue(rig, out LineRenderer line) || line == null)
                {
                    line = CreateTracer();
                    tracersPool[rig] = line;
                }

                Color color = rig.IsTagged() ? Color.red : Color.grey;
                line.startColor = color;
                line.endColor = color;
                line.SetPosition(0, rig.headMesh.transform.position);
                line.SetPosition(1, handPos);
            }

            PruneStale(tracersPool, scratchLive);
        }

        public static void CleanUpTracers() => ClearPool(tracersPool);

        private static LineRenderer CreateTracer()
        {
            if (tracerMaterial == null)
                tracerMaterial = new Material(Utility.GUIShader());

            GameObject holder = new GameObject("JupiterX_Tracer");
            LineRenderer line = holder.AddComponent<LineRenderer>();
            line.sharedMaterial = tracerMaterial;
            line.positionCount = 2;
            line.useWorldSpace = true;
            line.startWidth = 0.01f;
            line.endWidth = 0.01f;
            line.shadowCastingMode = ShadowCastingMode.Off;
            line.receiveShadows = false;
            return line;
        }

        private static bool fullBrightCached;
        private static bool originalFog;
        private static Color originalAmbient;

        public static void FullBright()
        {
            if (!fullBrightCached)
            {
                originalFog = RenderSettings.fog;
                originalAmbient = RenderSettings.ambientLight;
                fullBrightCached = true;
            }
            RenderSettings.fog = false;
            RenderSettings.ambientLight = Color.white;
        }

        public static void DisableFullBright()
        {
            if (!fullBrightCached)
                return;
            RenderSettings.fog = originalFog;
            RenderSettings.ambientLight = originalAmbient;
            fullBrightCached = false;
        }

        private static readonly HashSet<VRRig> chammedRigs = new HashSet<VRRig>();
        private static Material chamsNormalMat;
        private static Material chamsTaggedMat;
        public static void Chams(bool chams)
        {
            if (!chams || !CanRunInRoom())
            {
                DisableChams();
                return;
            }
            if (chamsTaggedMat == null)
                chamsTaggedMat = new Material(Utility.GUIShader()) { color = new Color(0.6f, 0f, 0f, 0.6f) };
            if (chamsNormalMat == null)
                chamsNormalMat = new Material(Utility.GUIShader());
            Color bg = Settings.backgroundColor.GetCurrentColor();
            chamsNormalMat.color = new Color(bg.r, bg.g, bg.b, 0.6f);
            VRRig local = LocalRig;
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (!IsRemoteRig(rig, local) || rig.mainSkin == null)
                    continue;
                Material want = rig.IsTagged() ? chamsTaggedMat : chamsNormalMat;
                if (rig.mainSkin.sharedMaterial != want)
                    rig.mainSkin.sharedMaterial = want;
                chammedRigs.Add(rig);
            }
        }

        public static void DisableChams()
        {
            if (chammedRigs.Count == 0)
                return;
            foreach (VRRig rig in chammedRigs)
            {
                if (rig == null || rig.mainSkin == null || rig.materialsToChangeTo == null)
                    continue;

                int idx = rig.setMatIndex;
                if (idx >= 0 && idx < rig.materialsToChangeTo.Length)
                    rig.mainSkin.material = rig.materialsToChangeTo[idx];
            }
            chammedRigs.Clear();
        }

        private static readonly Dictionary<int, TextMesh> _labels = new Dictionary<int, TextMesh>();
        private static void DrawLabel(int id, Transform target, string labelObjName, string text, Color color, int index = 0)
        {
            if (target == null)
            {
                RemoveLabel(id);
                return;
            }
            if (!_labels.TryGetValue(id, out TextMesh label) || label == null)
            {
                label = CreateTextMesh("JupiterX_Label_" + labelObjName, 22, 0.1f, FontStyle.Italic);
                label.transform.localScale = Vector3.one * 0.25f;
                _labels[id] = label;
            }
            if (label.text != text)
                label.text = text;
            label.color = color;
            Transform t = label.transform;
            t.position = target.position + new Vector3(0f, 0.1f + index * 0.15f, 0f);
            FaceCamera(t, MainCam);
        }

        public static void RemoveLabel(int id)
        {
            if (_labels.TryGetValue(id, out TextMesh label))
            {
                if (label != null)
                    UnityEngine.Object.Destroy(label.gameObject);
                _labels.Remove(id);
            }
        }

        private static string velStrCache = "";
        private static int velKeyCache = int.MinValue;
        public static void VelocityLabel()
        {
            if (GorillaTagger.Instance == null || GorillaTagger.Instance.bodyCollider == null)
            {
                RemoveLabel(0);
                return;
            }
            Rigidbody rb = GorillaTagger.Instance.bodyCollider.attachedRigidbody;
            if (rb == null)
            {
                RemoveLabel(0);
                return;
            }
            float speed = rb.velocity.magnitude;
            int velKey = Mathf.RoundToInt(speed * 10f);
            if (velKey != velKeyCache)
            {
                velKeyCache = velKey;
                velStrCache = $"{speed:F1}m/s";
            }
            DrawLabel(0, Utility.RightHandTransform(), "Velocity", velStrCache,
                speed >= GorillaLocomotion.Player.Instance.maxJumpSpeed ? Color.green : Color.white);
        }

        private static string FormatTimer(int seconds)
        {
            int minutes = seconds / 60;
            int remainingSeconds = seconds % 60;
            return $"{minutes:D2}:{remainingSeconds:D2}";
        }

        private static float startTime;
        private static float endTime;
        private static bool lastWasTagged;
        private static string timeStrCache = "";
        private static int timeSecCache = int.MinValue;

        public static void TimeLabel()
        {
            if (!PhotonNetwork.InRoom)
            {
                RemoveLabel(3);
                return;
            }
            if (GetInfectedCached().Count == 0)
            {
                startTime = Time.time;
                RemoveLabel(3);
                return;
            }
            bool playerIsTagged = LocalRig.IsTagged();
            if (playerIsTagged && !lastWasTagged)
                endTime = Time.time - startTime;
            else if (!playerIsTagged && lastWasTagged)
                startTime = Time.time;
            lastWasTagged = playerIsTagged;

            int secs = Mathf.FloorToInt(playerIsTagged ? endTime : Time.time - startTime);
            if (secs != timeSecCache)
            {
                timeSecCache = secs;
                timeStrCache = FormatTimer(secs);
            }
            DrawLabel(3, Utility.RightHandTransform(), "Time", timeStrCache,
                playerIsTagged ? Color.green : Color.white);
        }

        private static string nearStrCache = "";
        private static int nearKeyCache = int.MinValue;
        public static void NearbyTaggerLabel()
        {
            if (GorillaTagger.Instance == null || GorillaParent.instance == null || LocalRig.IsTagged())
            {
                RemoveLabel(1);
                return;
            }
            Vector3 headPos = GorillaTagger.Instance.headCollider.transform.position;
            float closestSqr = float.MaxValue;
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig == null || rig.headMesh == null || !rig.gameObject.activeInHierarchy || !rig.IsTagged())
                    continue;
                float sqr = (headPos - rig.headMesh.transform.position).sqrMagnitude;
                if (sqr < closestSqr)
                    closestSqr = sqr;
            }
            if (closestSqr == float.MaxValue)
            {
                RemoveLabel(1);
                return;
            }
            float closest = Mathf.Sqrt(closestSqr);
            int nearKey = Mathf.RoundToInt(closest * 10f);
            if (nearKey != nearKeyCache)
            {
                nearKeyCache = nearKey;
                nearStrCache = $"{closest:F1}m";
            }
            Color colorn = Color.green;
            if (closest < 30f) colorn = Color.yellow;
            if (closest < 20f) colorn = new Color32(255, 90, 0, 255);
            if (closest < 10f) colorn = Color.red;
            DrawLabel(1, Utility.LeftHandTransform(), "NearbyTagger", nearStrCache, colorn);
        }

        private static string leftStrCache = "";
        private static int leftCountCache = int.MinValue;
        public static void LastLabel()
        {
            if (!PhotonNetwork.InRoom || PhotonNetwork.CurrentRoom == null)
            {
                RemoveLabel(2);
                return;
            }
            int infectedCount = GetInfectedCached().Count;
            if (infectedCount == 0)
            {
                RemoveLabel(2);
                return;
            }
            int left = PhotonNetwork.CurrentRoom.PlayerCount - infectedCount;
            if (left != leftCountCache)
            {
                leftCountCache = left;
                leftStrCache = left + " left";
            }
            DrawLabel(2, Utility.LeftHandTransform(), "LastLabel", leftStrCache,
                left <= 1 && !LocalRig.IsTagged() ? Color.green : Color.white);
        }

        private static readonly List<Photon.Realtime.Player> infectedCache = new List<Photon.Realtime.Player>();
        private static int infectedCacheFrame = -1;
        private static List<Photon.Realtime.Player> GetInfectedCached()
        {
            if (infectedCacheFrame == Time.frameCount)
                return infectedCache;
            infectedCacheFrame = Time.frameCount;
            infectedCache.Clear();
            if (!PhotonNetwork.InRoom || GorillaGameManager.instance == null || GorillaComputer.instance == null)
                return infectedCache;
            switch (GorillaComputer.instance.currentGameMode)
            {
                case "INFECTION":
                    GorillaTagManager tagManager = GorillaGameManager.instance.TryCast<GorillaTagManager>();
                    if (tagManager == null)
                        break;
                    if (tagManager.isCurrentlyTag)
                    {
                        if (tagManager.currentIt != null)
                            infectedCache.Add(tagManager.currentIt);
                    }
                    else
                    {
                        AddFromIl2CppList(tagManager.currentInfected);
                    }
                    break;
                case "HUNT":
                    GorillaHuntManager huntManager = GorillaGameManager.instance.TryCast<GorillaHuntManager>();
                    if (huntManager != null)
                        AddFromIl2CppList(huntManager.currentHunted);
                    break;
            }
            return infectedCache;
        }

        private static void AddFromIl2CppList(Il2CppSystem.Collections.Generic.List<Photon.Realtime.Player> source)
        {
            if (source == null)
                return;
            int count = source.Count;
            for (int i = 0; i < count; i++)
            {
                Photon.Realtime.Player player = source.get_Item(i);
                if (player != null)
                    infectedCache.Add(player);
            }
        }
        public static List<Photon.Realtime.Player> InfectedList() =>
            new List<Photon.Realtime.Player>(GetInfectedCached());
    }
}