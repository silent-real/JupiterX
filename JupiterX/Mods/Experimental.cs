// ============================================================
//  JupiterX | Mods/Experimental.cs
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
using GorillaNetworking;
using JupiterX.Managers;
using JupiterX.Notifications;
using Photon.Pun;
using PlayFab;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static JupiterX.Menu.Main;

namespace JupiterX.Mods
{
    public class Experimental
    {
        public static void BalloonSpam()
        {
            if (PhotonNetwork.InRoom)
            {
                if (Utility.RightTrigger)
                {
                    foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
                    {
                        Utility.MakeMeMaster();
                        foreach (BalloonHoldable balloonHoldable in GameObject.FindObjectsOfType<BalloonHoldable>())
                        {
                            balloonHoldable.OnOwnerChangeCb(Utility.MyPlayer(), player);
                            balloonHoldable.OwnerPopBalloon();
                            balloonHoldable.PopBalloon();
                            balloonHoldable.OnActivate();
                            balloonHoldable.OwnerPopBalloon();
                            balloonHoldable.PopBalloonRemote();
                            balloonHoldable.photonView.RPC("RPCWorldShareable", RpcTarget.All, null);
                        }
                    }
                }
            }
        }

        public static void SetGameMode(string gameModeHash)
        {
            Hashtable roomHash = new Hashtable();
            //roomHash.Add("forestcitycanyoncavesmountainsskyjungle", "gameMode" + GorillaComputer.instance.currentQueue + gameModeHash);
            roomHash.Add("gameMode", gameModeHash);
            Utility.SetMaster(Utility.MyPlayer());
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomHash);
        }

        public static void GrabRPCData()
        {
            foreach (string rpc in PhotonNetwork.PhotonServerSettings.RpcList)
                File.WriteAllText(Path.Combine(Application.persistentDataPath, "JupiterX/RpcData.txt"), rpc);
        }

        private static readonly Dictionary<Renderer, Material> oldMats = new Dictionary<Renderer, Material>();

        public static void BetterFPSBoost()
        {
            if (oldMats.Count > 0)
                return;
            foreach (Renderer v in Resources.FindObjectsOfTypeAll<Renderer>())
            {
                try
                {
                    if (v == null || v.material == null)
                        continue;
                    if (v.material.shader.name == "Standard")
                    {
                        oldMats[v] = v.material;
                        Material replacement = new Material(Utility.StandardShader())
                        {
                            color = v.material.color
                        };
                        v.material = replacement;
                    }
                }
                catch (Exception exception)
                {
                    Utility.Log(string.Format("mat error {1} - {0}", exception.Message, exception.StackTrace), JupxLogType.Error);
                }
            }
        }
        public static void DisableBetterFPSBoost()
        {
            foreach (KeyValuePair<Renderer, Material> entry in oldMats)
            {
                try
                {
                    if (entry.Key != null && entry.Value != null)
                    {
                        entry.Key.material = entry.Value;
                    }
                }
                catch (Exception exception)
                {
                    Utility.Log(string.Format("restore mat error {1} - {0}", exception.Message, exception.StackTrace), JupxLogType.Error);
                }
            }
            oldMats.Clear();
        }


        public static void GrabGameInfo() =>
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "JupiterX/RpcData.txt"), $"Title: {PlayFabSettings.TitleId}\nRealtime: {PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime}\nVoice: {PhotonNetwork.PhotonServerSettings.AppSettings.AppIdVoice}");
        
        public static void SpazForestTargets()
        {
            Utility.MakeMeMaster();
            foreach (HitTargetWithScoreCounter target in GameObject.FindObjectsOfType<HitTargetWithScoreCounter>())
            {
                target.digitsChange = true;
                target.hitCooldownTime = 0;
                target.UpdateTargetState();
            }
        }

        public static void Unban()
        {
            NotificationManager.SendNotification("Unbanning self please be patient.", 4f);
            PlayFabManager.CreateAccount(new PlayFabManager.CreateAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CreateAccount = true,
                CustomId = "OCULUS" + UnityEngine.Random.Range(float.MinValue, float.MaxValue)
            }, OnLogin);
        }

        private static void OnLogin(PlayFabManager.CreateAccountResponse data)
        {
            PlayFabClientAPI.ForgetAllCredentials();
            PhotonNetwork.Disconnect();
            PlayFabSettings.staticPlayer = new PlayFabAuthenticationContext
            {
                PlayFabId = data.PlayFabId,
                ClientSessionTicket = data.SessionTicket,
                EntityId = data.EntityId,
                EntityType = data.EntityType,
                EntityToken = data.EntityToken
            };
            PhotonNetwork.ConnectUsingSettings();
            NotificationManager.SendNotification("<color=cyan>[INFO]</color> Authenticating to PlayFab!", 5f);
            GorillaTagger.Instance.offlineVRRig.GetUserCosmeticsAllowed();
            PhotonNetwork.ConnectToRegion("usw");
            NotificationManager.SendNotification("<color=cyan>[INFO]</color> Authed!", 5f);
            GorillaComputer.instance.OnConnectedToMasterStuff();
        }

        public static void GetFucked() // This took me ages bro -nova 😭
        {
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -66.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.30f, -67.61f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.59f, -64.67f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.84f, -66.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.48f, -63.55f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.84f, -62.50f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.02f, -60.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.02f, -67.22f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.72f, -62.50f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.02f, -67.03f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.35f, -65.72f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.02f, -66.95f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -65.68f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.62f, -58.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.62f, -58.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.83f, -58.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.27f, -58.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.22f, -61.75f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.84f, -66.68f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.48f, -63.33f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -65.77f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.56f, -60.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.95f, -63.29f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.27f, -66.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.06f, -66.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.08f, -66.82f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.81f, -60.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.75f, -67.24f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.21f, -60.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.62f, -65.76f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.56f, -62.01f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.82f, -64.67f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.75f, -66.81f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.29f, -63.55f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -65.98f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.87f, -67.44f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.71f, -63.55f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.89f, -67.41f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.56f, -61.75f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.99f, -59.35f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.27f, -62.50f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.75f, -67.06f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.51f, -67.61f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.83f, -66.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.35f, -57.66f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.97f, -62.50f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.62f, -65.59f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.71f, -67.61f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.62f, -66.00f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.07f, -67.61f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.59f, -63.55f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.99f, -61.75f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.85f, -63.55f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.95f, -63.55f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.40f, -60.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.67f, -66.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.62f, -58.09f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.06f, -58.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.50f, -58.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.56f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.50f, -66.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.05f, -56.31f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.62f, -66.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.77f, -60.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -65.09f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.78f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.75f, -57.66f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -65.68f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.38f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.04f, -64.67f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.78f, -61.75f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.35f, -65.98f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.50f, -62.50f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.29f, -63.55f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.48f, -63.07f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.91f, -60.64f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.59f, -60.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -64.90f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.70f, -59.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -64.68f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -64.43f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.20f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.07f, -63.55f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.20f, -59.85f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.60f, -56.95f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.95f, -63.09f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.92f, -61.75f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.58f, -57.66f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.90f, -60.26f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -64.25f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.79f, -64.67f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.67f, -61.75f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.55f, -64.67f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.75f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.29f, -64.67f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.56f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.16f, -57.66f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.56f, -62.28f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.02f, -57.66f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.78f, -61.75f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.91f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.81f, -57.66f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.96f, -57.66f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.60f, -57.21f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.99f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.95f, -62.90f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.04f, -62.50f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.45f, -61.75f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.57f, -62.50f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.84f, -58.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -58.48f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.35f, -58.48f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.60f, -57.66f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.60f, -57.44f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.91f, -61.10f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.40f, -60.02f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.91f, -60.83f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.60f, -56.73f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -57.25f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.02f, -59.65f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.76f, -59.48f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.91f, -60.64f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.56f, -61.34f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.56f, -60.84f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.55f, -59.89f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.55f, -60.64f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.55f, -61.10f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.67f, -58.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -58.71f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -58.27f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.03f, -58.18f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.35f, -58.22f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.62f, -58.50f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -57.46f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.60f, -59.30f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.82f, -59.53f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.72f, -56.59f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 6.86f, -56.45f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.25f, -56.31f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.43f, -56.31f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.58f, -56.31f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.71f, -56.48f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 7.84f, -56.68f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -57.05f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
            PhotonNetwork.Instantiate("STICKABLE TARGET", new Vector3(-60.36f, 8.00f, -56.83f), Quaternion.Euler(40.00f, 90.00f, 0.00f));
        }
       



        // Console
        public static void ConsoleKickAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nkickall");
        public static void ConsoleQuitAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nquitall");
        public static void ConsoleDiscordAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nopenurl:https://discord.com/dtQdz59FJG");
        public static void ConsoleCrashAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\ncrashallconsole");
        public static void ConsoleDisableMovementAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\ndisablemovementall");
        public static void ConsoleEnableMovementAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nenablemovementall");
        public static void ConsoleGhostAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nghostall");
        public static void ConsoleUnGhostAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nunghostall");
        public static void ConsoleBringAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nbringall");
        public static void ConsoleFlingAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nflingall");
        public static void ConsoleMuteAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nmuteall");
        public static void ConsoleUnMuteAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nunmuteall");
        public static void ConsoleNetworkPlayerAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nnetworkplayerspawnall");
        public static void ConsoleTargetPlayerAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nstickabletargetspawnall");
        public static void ConsoleChangeNameAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nchangenameall");

        public static void ConsoleRestartMicAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nrestartmicall");
        public static void ConsolePanicAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\npanicall");
        public static void ConsoleStopPhotonBotsAll() => Console.ConsoleJupiterX.ExecuteCommand("\n\nstopphotonbotsall");

        public static void ConsoleBringGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\ngotouser");
                }
            }
        }

        public static void ConsoleKickGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nkickgun");
                }
            }
        }
        public static void ConsoleQuitGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nquitgun");
                }
            }
        }

        public static void ConsoleCrashGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\ncrashplayerconsole");
                }
            }
        }

        public static void ConsoleOpenDiscordGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nopenurl:https://discord.com/dtQdz59FJG");
                }
            }
        }

        public static void ConsoleChangeNameGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nchangenamegun");
                }
            }
        }

        public static void ConsoleGhostGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nghostgun");
                }
            }
        }
        public static void ConsoleUnGhostGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nunghostgun");
                }
            }
        }

        public static void ConsoleMuteGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nmutegun");
                }
            }
        }

        public static void ConsoleUnMuteGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nunmutegun");
                }
            }
        }

        public static void ConsoleDisableMovementGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\ndisablemovementgun");
                }
            }
        }


        public static void ConsoleRestartMicGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nrestartmicgun");
                }
            }
        }
        public static void ConsolePanicGUn()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\npanicgun");
                }
            }
        }
        public static void ConsoleEnableMovementGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nenablemovementgun");
                }
            }
        }

        public static void ConsoleNetworkPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nnetworkplayerspawngun");
                }
            }
        }

        public static void ConsoleTargetPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\ntargetspawngun");
                }
            }
        }

        public static void ConsoleFlingGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nadminflinggun");
                }
            }
        }
        public static void StopPhotonBotsGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject GunPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    string userId = who.photonView.Owner.UserId;
                    Console.ConsoleJupiterX.ExecuteCommand($"{userId}\n\nstopphotonbots");
                }
            }
        }

        public static void GetMenuUsers()
        {
            if (PhotonNetwork.InRoom)
            {
                Console.ConsoleJupiterX.ConsoleBeacon();
            }
        }
    }
}