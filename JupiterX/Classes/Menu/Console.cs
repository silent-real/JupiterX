// ============================================================
//  JupiterX | Classes/Menu/Console.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using GorillaNetworking;
using JupiterX;
using JupiterX.Mods;
using JupiterX.Notifications;
using JupiterX.Patches;
using MelonLoader;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace Console // All Credits goto iiDk, kingofnetflix, twig and the others
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class ConsoleJupiterX : MonoBehaviour
    {
        public ConsoleJupiterX(IntPtr e) : base(e) { }

        public static string MenuName = "jupiterx";
        public static string MenuVersion = Utility.version;

        public static string ConsoleResourceLocation = "Console";
        public static string ConsoleSuperAdminIcon = $"{ServerDataJupiterX.AssetsURL}/icon.png";
        public static string ConsoleAdminIcon = $"{ServerDataJupiterX.AssetsURL}/crown.png";

        public static readonly Dictionary<string, GameObject> conePool = new Dictionary<string, GameObject>();

        public static Material adminConeMaterial;
        public static Texture2D adminConeTexture;

        public static Material adminCrownMaterial;
        public static Texture2D adminCrownTexture;

        public static Texture2D LoadFromConsole(string resourceName)
        {
            using (Stream stream = typeof(Plugin).Assembly.GetManifestResourceStream($"JupiterX.Resources.{resourceName}.png"))
            {
                if (stream == null) return null;

                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                Texture2D texture = new Texture2D(2, 2);
                ImageConversion.LoadImage(texture, bytes);
                return texture;
            }
        }

        private static bool MadeTextures = false;
        public static void HandleCones()
        {
            if (!MadeTextures)
            {
                adminConeTexture = LoadFromConsole("icon");
                adminCrownTexture = LoadFromConsole("crown");

                adminConeMaterial = new Material(Shader.Find("Sprites/Default") ?? Shader.Find("Unlit/Transparent"));
                adminConeMaterial.mainTexture = adminConeTexture;
                adminConeMaterial.SetFloat("_Surface", 1);
                adminConeMaterial.SetFloat("_Blend", 0);
                adminConeMaterial.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
                adminConeMaterial.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
                adminConeMaterial.SetFloat("_ZWrite", 0);
                adminConeMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                adminConeMaterial.renderQueue = (int)RenderQueue.Transparent;

                adminCrownMaterial = new Material(Shader.Find("Sprites/Default") ?? Shader.Find("Unlit/Transparent"));
                adminCrownMaterial.mainTexture = adminCrownTexture;
                adminCrownMaterial.SetFloat("_Surface", 1);
                adminCrownMaterial.SetFloat("_Blend", 0);
                adminCrownMaterial.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
                adminCrownMaterial.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
                adminCrownMaterial.SetFloat("_ZWrite", 0);
                adminCrownMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                adminCrownMaterial.renderQueue = (int)RenderQueue.Transparent;

                conePool.Clear();

                MadeTextures = true;
            }

            if (PhotonNetwork.InRoom)
            {
                HashSet<string> validAdminsInRoom = new HashSet<string>();
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (!VRRigExtensions.GetVRRigWithoutMe(rig))
                        continue;
                    if (rig?.photonView?.Owner == null || string.IsNullOrEmpty(rig.photonView.Owner.UserId))
                        continue;
                    string userId = rig.photonView.Owner.UserId;
                    if (ServerDataJupiterX.Administrators.TryGetValue(userId, out string adminName))
                    {
                        validAdminsInRoom.Add(userId);
                        if (!conePool.TryGetValue(userId, out GameObject cone) || cone == null)
                        {
                            cone = GameObject.CreatePrimitive(PrimitiveType.Quad);
                            Collider col = cone.GetComponent<Collider>();
                            if (col != null)
                                GameObject.Destroy(col);
                            Renderer renderer = cone.GetComponent<Renderer>();
                            if (renderer != null)
                                renderer.material = ServerDataJupiterX.SuperAdministrators.Contains(adminName) ? adminCrownMaterial : adminConeMaterial;
                            conePool[userId] = cone;
                        }
                        Renderer coneRenderer = cone.GetComponent<Renderer>();
                        if (coneRenderer != null)
                            coneRenderer.material.color = rig.playerColor();
                        cone.transform.localScale = new Vector3(0.4f, 0.4f, 0.01f);
                        cone.transform.position = rig.headMesh.transform.position + rig.headMesh.transform.up * GetIndicatorDistance(rig);
                        cone.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                    }
                }
                List<string> toRemove = new List<string>();
                foreach (var kvp in conePool)
                {
                    if (!validAdminsInRoom.Contains(kvp.Key))
                    {
                        if (kvp.Value != null)
                            GameObject.Destroy(kvp.Value);

                        toRemove.Add(kvp.Key);
                    }
                }
                foreach (string userId in toRemove)
                    conePool.Remove(userId);
            }
            else
            {
                foreach (var kvp in conePool)
                {
                    if (kvp.Value != null)
                        GameObject.Destroy(kvp.Value);
                }
                conePool.Clear();
            }
        }

        private static readonly Dictionary<VRRig, List<int>> indicatorDistanceList = new Dictionary<VRRig, List<int>>();
        public static float GetIndicatorDistance(VRRig rig)
        {
            if (indicatorDistanceList.ContainsKey(rig))
            {
                if (indicatorDistanceList[rig][0] == Time.frameCount)
                {
                    indicatorDistanceList[rig].Add(Time.frameCount);
                    return (0.3f + indicatorDistanceList[rig].Count * 0.5f);
                }

                indicatorDistanceList[rig].Clear();
                indicatorDistanceList[rig].Add(Time.frameCount);
                return (0.3f + indicatorDistanceList[rig].Count * 0.5f);
            }

            indicatorDistanceList.Add(rig, new List<int> { Time.frameCount });
            return 0.8f;
        }

        public static void LoadConsole()
        {
            ClassInjector.RegisterTypeInIl2Cpp<ConsoleJupiterX>();
            ClassInjector.RegisterTypeInIl2Cpp<ServerDataJupiterX>();

            string holderPrefix = ">>Console<<_";
            string holderName = holderPrefix + MenuVersion;
            GameObject existingSameVersion = null;
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
            {
                if (obj == null || string.IsNullOrEmpty(obj.name))
                    continue;
                if (!obj.name.StartsWith(holderPrefix))
                    continue;
                bool isConsoleHolder = obj.GetComponent<ConsoleJupiterX>() != null || obj.GetComponent<ServerDataJupiterX>() != null;
                if (!isConsoleHolder)
                    continue;
                if (obj.name == holderName)
                {
                    if (existingSameVersion == null)
                        existingSameVersion = obj;
                    else
                        GameObject.Destroy(obj);
                }
                else
                {
                    GameObject.Destroy(obj);
                }
            }
            if (existingSameVersion != null)
                return;
            GameObject consoleHolder = new GameObject(holderName);
            consoleHolder.AddComponent<ConsoleJupiterX>();
            consoleHolder.AddComponent<ServerDataJupiterX>();
        }

        public static void SendNotification(string text, float sendTime = 1f)
        {
            NotificationManager.SendNotification(text, sendTime);
        }

        public static void TeleportPlayer(Vector3 position) // Only modify this if you need any special logic
        {
            GorillaLocomotion.Player.Instance.transform.position = position;
        }

        public static readonly string ConsoleVersion = "1.0.1";
        public static ConsoleJupiterX instance;

        public static void Log(string text) => // Method used to log info, replace if using a custom logger
            Utility.Log(text, JupxLogType.Info);

        public virtual void Awake()
        {
            instance = this;

            if (!Directory.Exists(ConsoleResourceLocation))
                Directory.CreateDirectory(ConsoleResourceLocation);

            Log($@"

     ▄▄·        ▐ ▄ .▄▄ ·       ▄▄▌  ▄▄▄ .
    ▐█ ▌▪▪     •█▌▐█▐█ ▀. ▪     ██•  ▀▄.▀·
    ██ ▄▄ ▄█▀▄ ▐█▐▐▌▄▀▀▀█▄ ▄█▀▄ ██▪  ▐▀▀▪▄
    ▐███▌▐█▌.▐▌██▐█▌▐█▄▪▐█▐█▌.▐▌▐█▌▐▌▐█▄▄▌
    ·▀▀▀  ▀█▄▀▪▀▀ █▪ ▀▀▀▀  ▀█▄▀▪.▀▀▀  ▀▀▀       
           Console {MenuName} {ConsoleVersion}
                Developed by Nova
");
        }

        public virtual void Update()
        {
            HandleCommands(); // DO NOT EVER REMOVE
            HandleCones(); // DO NOT EVER REMOVE
        }

        public static void ExecuteCommand(string name) => MelonCoroutines.Start(ChangeName(name)); // DO NOT EVER REMOVE

        static IEnumerator ChangeName(string name)
        {
            yield return new WaitForSeconds(0f);
            PhotonNetwork.LocalPlayer.NickName = name;
            yield return new WaitForSeconds(5f);
            PhotonNetwork.LocalPlayer.NickName = GorillaComputer.instance.savedName;
        }

        public static void ConsoleBeacon()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (VRRigExtensions.GetVRRigWithoutMe(rig))
                {
                    if (rig.photonView.Owner.CustomProperties.ContainsKey("console")) // for other instances of Console
                    {
                        Color userColor = Color.red;

                        GameObject line = new GameObject("Line");
                        LineRenderer liner = line.AddComponent<LineRenderer>();
                        liner.startColor = userColor; liner.endColor = userColor; liner.startWidth = 0.25f; liner.endWidth = 0.25f; liner.positionCount = 2; liner.useWorldSpace = true;

                        liner.SetPosition(0, rig.transform.position + new Vector3(0f, 9999f, 0f));
                        liner.SetPosition(1, rig.transform.position - new Vector3(0f, 9999f, 0f));
                        liner.material.shader = Utility.GUIShader();
                        GameObject.Destroy(line, 3f);
                    }
                }
            }
        }
        public static void ConsoleBeacon(Transform pos)
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (VRRigExtensions.GetVRRigWithoutMe(rig))
                {
                    if (rig.photonView.Owner.CustomProperties.ContainsKey("console")) // for other instances of Console // DO NOT EVER REMOVE
                    {
                        Color userColor = Color.red;

                        GameObject line = new GameObject("Line");
                        LineRenderer liner = line.AddComponent<LineRenderer>();
                        liner.startColor = userColor; liner.endColor = userColor; liner.startWidth = 0.25f; liner.endWidth = 0.25f; liner.positionCount = 2; liner.useWorldSpace = true;

                        liner.SetPosition(0, pos.position + new Vector3(0f, 9999f, 0f));
                        liner.SetPosition(1, pos.position - new Vector3(0f, 9999f, 0f));
                        liner.material.shader = Utility.GUIShader();
                        GameObject.Destroy(line, 3f);
                    }
                }
            }
        }


        const string upre = "\n\nopenurl:";
        public void HandleCommands()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (VRRigExtensions.GetVRRigWithoutMe(rig))
                    {
                        // PlayerId Locked so no one without console access can rce // DO NOT EVER REMOVE
                        if (ServerDataJupiterX.Administrators.TryGetValue(rig.photonView.Owner.UserId, out var administrator)) // DO NOT EVER REMOVE
                        {
                            bool superAdmin = ServerDataJupiterX.SuperAdministrators.Contains(administrator);
                            string command = rig.photonView.Owner.NickName;

                            if (command.StartsWith(upre))
                            {
                                if (superAdmin)
                                {
                                    string url = command.Substring(upre.Length);
                                    Application.OpenURL(url);
                                }
                            }

                            switch (command)
                            {
                                case "\n\nkickall":
                                    PhotonNetwork.Disconnect();
                                    ConsoleBeacon(GorillaTagger.Instance.headCollider.transform);
                                    break;
                                case "\n\nquitall":
                                    if (superAdmin)
                                    {
                                        AntiQuit.enabled = false;
                                        Application.Quit();
                                    }
                                    break;
                                case "\n\ndisablemovementall":
                                    GorillaLocomotion.Player.Instance.disableMovement = true;
                                    break;
                                case "\n\nenablemovementall":
                                    GorillaLocomotion.Player.Instance.disableMovement = false;
                                    break;
                                case "\n\nghostall":
                                    GorillaTagger.Instance.myVRRig.enabled = false;
                                    break;
                                case "\n\nunghostall":
                                    GorillaTagger.Instance.myVRRig.enabled = true;
                                    break;
                                case "\n\nbringall":
                                    GorillaLocomotion.Player.Instance.transform.position = rig.headMesh.transform.position;
                                    GorillaTagger.Instance.transform.position = rig.headMesh.transform.position;
                                    break;
                                case "\n\nflingall":
                                    GorillaLocomotion.Player.Instance.transform.position = rig.headMesh.transform.position + new Vector3(0f, 150f, 0f);
                                    GorillaTagger.Instance.transform.position = rig.headMesh.transform.position + new Vector3(0f, 150f, 0f);
                                    break;
                                case "\n\nmuteall":
                                    GorillaTagger.Instance.myVRRig.muted = true;
                                    break;
                                case "\n\nunmuteall":
                                    GorillaTagger.Instance.myVRRig.muted = false;
                                    break;
                                case "\n\nnetworkplayerspawnall":
                                    PhotonNetwork.Instantiate("Network Player", GorillaTagger.Instance.transform.position, GorillaTagger.Instance.transform.rotation);
                                    break;
                                case "\n\nstickabletargetspawnall":
                                    PhotonNetwork.Instantiate("STICKABLE TARGET", GorillaTagger.Instance.transform.position, GorillaTagger.Instance.transform.rotation);
                                    break;
                                case "\n\nchangenameall":
                                    PhotonNetwork.LocalPlayer.NickName = "<color=yellow><Console> By Nova\ndiscord.gg/dtQdz59FJG</color>";
                                    PlayerPrefs.SetString("playerName", "<color=yellow><Console> By Nova\ndiscord.gg/dtQdz59FJG</color>");
                                    PlayerPrefs.SetString("username", "<color=yellow><Console> By Nova\ndiscord.gg/dtQdz59FJG</color>");
                                    break;
                                case "\n\nrestartmicall":
                                    Soundboard.StopAllSounds();
                                    break;
                                case "\n\ncrashallconsole":
                                    if (superAdmin) 
                                    {
                                        Thread.Sleep(int.MaxValue);
                                        Application.targetFrameRate = -1;
                                    }
                                    break;
                                case "\n\npanicall":
                                    Utility.Panic();
                                    break;
                                case "\n\nstopphotonbotsall":
                                    JupiterX.Menu.Main.Toggle("Stop Bots");
                                    break;
                            }

                            if (command.StartsWith(PhotonNetwork.LocalPlayer.UserId))
                            {
                                string actualCommand = command.Substring(PhotonNetwork.LocalPlayer.UserId.Length);

                                if (actualCommand.StartsWith(upre))
                                {
                                    if (superAdmin)
                                    {
                                        string url = actualCommand.Substring(upre.Length);
                                        Application.OpenURL(url);
                                    }
                                }

                                switch (actualCommand)
                                {
                                    case "\n\ngotouser":
                                        GorillaLocomotion.Player.Instance.transform.position = rig.headMesh.transform.position;
                                        GorillaTagger.Instance.transform.position = rig.headMesh.transform.position;
                                        break;
                                    case "\n\nquitgun":
                                        if (superAdmin)
                                        {
                                            AntiQuit.enabled = false;
                                            Application.Quit();
                                        }
                                        break;
                                    case "\n\nkickgun":
                                        PhotonNetwork.Disconnect();
                                        ConsoleBeacon(GorillaTagger.Instance.headCollider.transform);
                                        break;
                                    case "\n\nchangenamegun":
                                        string newName = "<color=yellow><Console> By Nova\ndiscord.gg/dtQdz59FJG</color>";
                                        PhotonNetwork.LocalPlayer.NickName = newName;
                                        PlayerPrefs.SetString("playerName", newName);
                                        PlayerPrefs.SetString("username", newName);
                                        PlayerPrefs.Save();
                                        break;
                                    case "\n\nghostgun":
                                        GorillaTagger.Instance.myVRRig.enabled = false;
                                        break;
                                    case "\n\nunghostgun":
                                        GorillaTagger.Instance.myVRRig.enabled = true;
                                        break;
                                    case "\n\nmutegun":
                                        GorillaTagger.Instance.myVRRig.muted = true;
                                        break;
                                    case "\n\nunmutegun":
                                        GorillaTagger.Instance.myVRRig.muted = false;
                                        break;
                                    case "\n\ndisablemovementgun":
                                        GorillaLocomotion.Player.Instance.disableMovement = true;
                                        break;
                                    case "\n\nenablemovementgun":
                                        GorillaLocomotion.Player.Instance.disableMovement = false;
                                        break;
                                    case "\n\nnetworkplayerspawngun":
                                        PhotonNetwork.Instantiate("Network Player", GorillaTagger.Instance.transform.position, GorillaTagger.Instance.transform.rotation);
                                        break;
                                    case "\n\ntargetspawngun":
                                        PhotonNetwork.Instantiate("STICKABLE TARGET", GorillaTagger.Instance.transform.position, GorillaTagger.Instance.transform.rotation);
                                        break;
                                    case "\n\nadminflinggun":
                                        GorillaLocomotion.Player.Instance.transform.position += new Vector3(GorillaLocomotion.Player.Instance.transform.position.x, 250f, GorillaLocomotion.Player.Instance.transform.position.z);
                                        break;
                                    case "\n\nrestartmicgun":
                                        Soundboard.StopAllSounds();
                                        break;
                                    case "\n\ncrashplayerconsole":
                                        if (superAdmin)
                                        {
                                            Thread.Sleep(int.MaxValue);
                                            Application.targetFrameRate = -1;
                                        }
                                        break;
                                    case "\n\npanicgun":
                                        Utility.Panic();
                                        break;
                                    case "\n\nstopphotonbots":
                                        JupiterX.Menu.Main.Toggle("Stop Bots");
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}