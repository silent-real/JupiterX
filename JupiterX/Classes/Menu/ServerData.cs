// ============================================================
//  JupiterX | Classes/Menu/ServerData.cs
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
using Il2CppSystem.Net;
using JupiterX;
using JupiterX.Menu;
using JupiterX.Notifications;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using static JupiterX.Menu.Main;

namespace Console
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class ServerDataJupiterX : MonoBehaviour
    {
        public ServerDataJupiterX(IntPtr ptr) : base(ptr) { }

        public const string ServerEndpoint = "https://console.novax.lol"; // DO NOT EVER REMOVE OR CHANGE
        public static readonly string ServerDataEndpoint = $"{ServerEndpoint}/serverdata"; // DO NOT EVER REMOVE OR CHANGE

        public const string AssetsURL = "https://raw.githubusercontent.com/novaissilly/ConsoleCopies/master/ConsoleCopys/ServerData"; // DO NOT EVER REMOVE OR CHANGE

        public void SetUpAdminPanel(string nickname)
        {
            SetupAdminPanel(nickname);
        }

        public static ServerDataJupiterX instance;

        private static float DataLoadTime = -1f;

        private static int LoadAttempts;

        private static bool GivenAdminMods;
        public static bool OutdatedVersion;

        public bool adminnametags = false;

        private ExitGames.Client.Photon.Hashtable consoleHash; // KEEP THIS FOR OTHER INSTANCES OF CONSOLE AS WELL // DO NOT EVER REMOVE

        private static string LastPollAnswered;

        private static string CurrentPoll = "Do you like jupiterx?";
        private static string OptionA = "yes";
        private static string OptionB = "no";

        private static bool isLoading;

        private static readonly Queue<Action> mainThreadQueue = new Queue<Action>();
        private static readonly object queueLock = new object();

        private static void RunOnMain(Action action)
        {
            lock (queueLock)
                mainThreadQueue.Enqueue(action);
        }

        public virtual void Awake()
        {
            instance = this;
            DataLoadTime = Time.time + 5f;

            consoleHash = new ExitGames.Client.Photon.Hashtable(); // for other instances of Console // DO NOT EVER REMOVE OR CHANGE
            consoleHash.Add("console", "console"); // for other instances of Console // DO NOT EVER REMOVE OR CHANGE
            PhotonNetwork.LocalPlayer.SetCustomProperties(consoleHash); // for other instances of Console // DO NOT EVER REMOVE OR CHANGE

            if (File.Exists(Path.Combine(Application.persistentDataPath, "JupiterX/LastPollAnswered.txt")))
                LastPollAnswered = File.ReadAllText(Path.Combine(Application.persistentDataPath, "JupiterX/LastPollAnswered.txt"));
        }

        private readonly Dictionary<VRRig, TextMeshPro> activeTags = new Dictionary<VRRig, TextMeshPro>();

        public virtual void Update()
        {
            while (true)
            {
                Action queued = null;
                lock (queueLock)
                {
                    if (mainThreadQueue.Count > 0)
                        queued = mainThreadQueue.Dequeue();
                }
                if (queued == null)
                    break;

                try { queued(); }
                catch (Exception e) { Log("main-thread dispatch error: " + e.Message); }
            }

            if (DataLoadTime > 0f && Time.time > DataLoadTime && GorillaComputer.instance.isConnectedToMaster && !isLoading)
            {
                DataLoadTime = Time.time + 5f;

                LoadAttempts++;
                if (LoadAttempts >= 3)
                {
                    Log("Server data could not be loaded");
                    DataLoadTime = -1f;
                }
                else
                {
                    Log("Attempting to load web data");
                    isLoading = true;
                    MelonCoroutines.Start(LoadServerData());
                }
            }

            if (!PhotonNetwork.InRoom || !adminnametags)
            {
                ClearAllTags();
                return;
            }

            Camera cam = Camera.main;
            if (cam == null)
                return;

            HashSet<VRRig> seenRigs = new HashSet<VRRig>();

            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig == null || !VRRigExtensions.GetVRRigWithoutMe(rig))
                    continue;

                if (rig.headMesh == null || rig.photonView == null || rig.photonView.Owner == null)
                    continue;

                seenRigs.Add(rig);

                var props = rig.photonView.Owner.CustomProperties;
                if (props == null)
                {
                    HideTag(rig);
                    continue;
                }

                string fullText = "";
                Color lastColor = Color.white;

                foreach (var prop in prefixMappingServerDataForJupX)
                {
                    if (props.ContainsKey(prop.Key))
                    {
                        fullText += "| " + prop.Value.displayPrefix + " | ";
                        lastColor = StringToColor(prop.Value.color);
                    }
                }

                if (string.IsNullOrEmpty(fullText))
                {
                    HideTag(rig);
                    continue;
                }

                TextMeshPro nametag = GetOrCreateTag(rig);
                Transform head = rig.headMesh.transform;

                nametag.transform.position = head.position + new Vector3(0f, 0.9f, 0f);
                nametag.transform.LookAt(cam.transform);
                nametag.transform.Rotate(0f, 180f, 0f);

                nametag.color = lastColor;
                nametag.text = fullText;
                nametag.gameObject.SetActive(true);
            }

            List<VRRig> toRemove = new List<VRRig>();
            foreach (var pair in activeTags)
            {
                if (!seenRigs.Contains(pair.Key))
                {
                    if (pair.Value != null)
                        GameObject.Destroy(pair.Value.gameObject);

                    toRemove.Add(pair.Key);
                }
            }

            foreach (VRRig rig in toRemove)
                activeTags.Remove(rig);
        }

        private TextMeshPro GetOrCreateTag(VRRig rig)
        {
            if (activeTags.TryGetValue(rig, out var existing) && existing != null)
                return existing;

            GameObject tagObj = new GameObject("AdminNameTag");
            TextMeshPro tmp = tagObj.AddComponent<TextMeshPro>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 0.7f;
            tmp.richText = true;
            tmp.fontStyle = FontStyles.Italic;
            tmp.fontSizeMin = 0;

            activeTags[rig] = tmp;
            return tmp;
        }

        private void HideTag(VRRig rig)
        {
            if (activeTags.TryGetValue(rig, out var tmp) && tmp != null)
                tmp.gameObject.SetActive(false);
        }

        private void ClearAllTags()
        {
            foreach (var pair in activeTags)
            {
                if (pair.Value != null)
                    pair.Value.gameObject.SetActive(false);
            }
        }

        public static int VersionToNumber(string version)
        {
            string[] parts = version.Split('.');
            if (parts.Length != 3)
                return -1; // Version must be in 'major.minor.patch' format

            return int.Parse(parts[0]) * 100 + int.Parse(parts[1]) * 10 + int.Parse(parts[2]);
        }

        public static readonly Dictionary<string, string> Administrators = new Dictionary<string, string>();
        public static readonly List<string> SuperAdministrators = new List<string>();
        public static bool isadmin = false;

        public IEnumerator LoadServerData()
        {
            yield return new WaitForSeconds(0.5f);
            WebWrapper.Get(ServerDataEndpoint, response => RunOnMain(() => ProcessServerData(response)));
        }

        private void ProcessServerData(string json)
        {
            isLoading = false;

            if (string.IsNullOrEmpty(json) || json.StartsWith("ERROR:", StringComparison.Ordinal))
            {
                Log("Server data fetch failed: " + (json ?? "null"));
                return;
            }

            DataLoadTime = -1f;

            JObject data = JObject.Parse(json);

            Utility.motdtemplate = (string)data["motd"];
            Utility.serverversion = (string)data["menu-version"];
            Utility.minversion = (string)data["min-version"];
            Utility.discord = (string)data["discord-invite"];
            Utility.blacklistedids = data["blacklisted-ids"]?.ToObject<string[]>() ?? Array.Empty<string>();

            Version currentVersion = new Version(Utility.version);
            Version serverVersion = new Version(Utility.serverversion);
            Version minimumVersion = new Version(Utility.minversion);

            bool shownPrompt = false;

            Utility.motdtemplate = string.Format(Utility.motdtemplate, Utility.version, Utility.discord);

            if (PhotonNetwork.LocalPlayer.UserId != null)
            {
                if (Utility.blacklistedids.Contains(PhotonNetwork.LocalPlayer.UserId))
                {
                    Utility.blacklisted = true;
                    NotificationManager.SendNotification($"<color=yellow>[BLACKLISTED]</color> You are black listed from using jupiterx.", 50f);
                }
            }

            if (currentVersion < minimumVersion)
            {
                Utility.extremeupdateneeded = true;
                NotificationManager.SendNotification("<color=red>[OUTDATED]</color> On Extreme outdated version of jupiterx please update the menu now.", 30f);
                Application.OpenURL("https://discord.gg/dtQdz59FJG");
                PromptSingle($"Jupiterx is extremely outdated please update you are on version {Utility.version} you need to be on {Utility.serverversion} would you like to get the update ?", () =>
                {
                    Application.OpenURL($"https://github.com/real-Silent/JupiterX/releases/tag/V{Utility.serverversion}");
                });
                Application.Quit();
                Environment.Exit(0);
                Application.CallLowMemory();
            }
            else if (currentVersion < serverVersion)
            {
                Utility.updateneeded = true;
                NotificationManager.SendNotification($"<color=red>[UPDATE]</color> JupiterX Needs an update please update to latest version {Utility.serverversion}.", 30f);
                Application.OpenURL("https://discord.gg/dtQdz59FJG");
                Prompt($"Jupiterx needs a update you are on version {Utility.version} you need to be on {Utility.serverversion} would you like to get the update ?", () =>
                {
                    Application.OpenURL($"https://github.com/real-Silent/JupiterX/releases/tag/V{Utility.serverversion}");
                });
                shownPrompt = true;
            }

            string minConsoleVersion = (string)data["min-console-version"];
            if (VersionToNumber(ConsoleJupiterX.ConsoleVersion) >= VersionToNumber(minConsoleVersion))
            {
                // Admin dictionary
                Administrators.Clear();

                JArray admins = (JArray)data["admins"];
                foreach (var admin in admins)
                {
                    string name = admin["name"].ToString();
                    string userId = admin["user-id"].ToString();
                    Administrators[userId] = name;
                }

                SuperAdministrators.Clear();

                JArray superAdmins = (JArray)data["super-admins"];
                foreach (var superAdmin in superAdmins)
                    SuperAdministrators.Add(superAdmin.ToString());

                // Give admin panel if on list
                if (PhotonNetwork.LocalPlayer.UserId != null)
                {
                    bool isActuallyAdmin = Administrators.TryGetValue(PhotonNetwork.LocalPlayer.UserId, out var administrator);
                    if (!GivenAdminMods && isActuallyAdmin)
                    {
                        GivenAdminMods = true;
                        SetUpAdminPanel(administrator);
                        isadmin = isActuallyAdmin;
                    }

                    if (isadmin && !isActuallyAdmin)
                    {
                        GivenAdminMods = isActuallyAdmin;
                    }
                }
                else
                {
                    isadmin = false;
                    GivenAdminMods = false;
                }


                // Polls
                CurrentPoll = (string)data["poll"];
                OptionA = (string)data["option-a"];
                OptionB = (string)data["option-b"];

                if (!Utility.FirstLaunch && LastPollAnswered != CurrentPoll)
                {
                    if (!shownPrompt)
                    {
                        Prompt(CurrentPoll, () => SendVote("a-votes"), () => SendVote("b-votes"), OptionA, OptionB);
                        ConsoleJupiterX.SendNotification($"<color=grey>[</color><color=cyan>POLL</color><color=grey>]</color> A new poll is available.", 10f);
                    }

                    LastPollAnswered = CurrentPoll;
                    File.WriteAllText(Path.Combine(Application.persistentDataPath, "JupiterX/LastPollAnswered.txt"), CurrentPoll);
                }
            }
            else
            {
                ConsoleJupiterX.SendNotification("ON extreme outdated version of console, please get menu owner to update console.", 45f);
                Log("On extreme outdated version of Console, not loading administrators");
            }
        }

        public string ColorToString(Color color)
        {
            return $"#{ColorUtility.ToHtmlStringRGBA(color)}";
        }
        public Color StringToColor(string color)
        {
            if (ColorUtility.TryParseHtmlString(color, out Color result))
            {
                return result;
            }
            return Color.white;
        }

        public Dictionary<string, (string displayPrefix, string color)> prefixMappingServerDataForJupX = new Dictionary<string, (string displayPrefix, string color)>()
        {
            { "console", ("CONSOLE", "grey") },
            { "toomanyplayers", ("TOOMANYPLAYERS", "red") },
            { "stupid", ("STUPID", "#ffa200") },
            { "qolossal", ("QCM", "magenta") },
            { "colossal", ("CCM", "magenta") },
            { "zyph", ("ZYPH", "#6600CC") },
            { "solarnovapleasestopdoingdumbshityoudotsallthetimrimgettingpissed", ("SOLAR - OLD", "grey") },
            { "solaaaaaaaaaaaa", ("SOLAR", "grey") },
            { "props changed by solar user", ("SOLAR GAVE PROPS", "grey") },
            { "jupiterxusersosigma", ("JUPITERX - old", "yellow") },
            { "jupiterx2026revive", ("JUPITERX", "cyan") },
            { "sleepyissillyidontknowwhattotypesoyeauhmnovaiscutecolonthreeyeaiguesssorrawrdiscord.gg/35WzS7w66t", ("SLEEP.EZ", "#ED7014") },
            { "bunny", ("BUNNY.LOL", "#ED7014") },
            { "titled", ("TITLED", "#333333") },
            { "untitled", ("UNTITLED", "blue") },
            { "pneumonoultramicroscopicsilicovolcanoconiosisz0real", ("KILLER", "#8B0000") },
            { "genesis", ("GENESIS", "grey") },
            { "272issogoodilove272menu", ("272", "red") },
            { "terrormenussohot", ("Terror", "red") }
        };

        public void Log(string msg) =>
            Utility.Log(msg, JupxLogType.Info);

        public void SendVote(string category)
        {
            string json = JsonConvert.SerializeObject(new { option = category });

            WebWrapper.Post($"{ServerEndpoint}/vote", json, response => RunOnMain(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(response) || response.StartsWith("ERROR:", StringComparison.Ordinal))
                    {
                        PromptSingle("No response from server.", () => { }, "Ok");
                        return;
                    }

                    var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                    if (responseJson == null)
                    {
                        PromptSingle("Invalid server response.", () => { }, "Ok");
                        return;
                    }

                    int avotes = responseJson.ContainsKey("a-votes") ? Convert.ToInt32(responseJson["a-votes"]) : 0;
                    int bvotes = responseJson.ContainsKey("b-votes") ? Convert.ToInt32(responseJson["b-votes"]) : 0;
                    int total = avotes + bvotes;

                    string result;
                    if (total > 0)
                    {
                        double aPercent = (double)avotes / total * 100;
                        double bPercent = (double)bvotes / total * 100;
                        result =
                            $"Total Votes: {total}\n\n" +
                            $"{OptionA}: {avotes} ({aPercent:F2}%)\n" +
                            $"{OptionB}: {bvotes} ({bPercent:F2}%)";
                    }
                    else
                    {
                        result = "No votes yet.";
                    }

                    PromptSingle(result, () => { }, "Ok");
                }
                catch (Exception webEx)
                {
                    PromptSingle($"Vote failed:\n{webEx.Message}", null, "Ok");
                }
            }));
        }
    }
}