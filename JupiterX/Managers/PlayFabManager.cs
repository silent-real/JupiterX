// ============================================================
//  JupiterX | Managers/PlayFabManager.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using System;
using JupiterX.Menu;
using JupiterX.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JupiterX.Managers
{
    public class PlayFabManager
    {
        public static void CreateAccount(CreateAccountRequest caRequest, Action<CreateAccountResponse> callback)
        {
            if (caRequest == null)
            {
                NotificationManager.SendNotification("Your createaccountrequest is null somehow im a dumbass.", 2f);
                return;
            }
            string url = $"https://{caRequest.TitleId}.playfabapi.com/Client/LoginWithCustomID";
            string json = JsonConvert.SerializeObject(caRequest);
            WebWrapper.Post(url, json, responseJson =>
            {
                if (string.IsNullOrEmpty(responseJson) || responseJson.StartsWith("ERROR:", StringComparison.Ordinal))
                {
                    NotificationManager.SendNotification($"PlayFab login failed: {responseJson}", 3f);
                    callback?.Invoke(null);
                    return;
                }
                try
                {
                    JObject responseData = JObject.Parse(responseJson);
                    JToken data = responseData["data"];
                    if (data == null)
                    {
                        NotificationManager.SendNotification("PlayFab returned no data block.", 3f);
                        callback?.Invoke(null);
                        return;
                    }
                    CreateAccountResponse response = new CreateAccountResponse
                    {
                        PlayFabId = (string)data["PlayFabId"],
                        SessionTicket = (string)data["SessionTicket"],
                        EntityId = (string)data["EntityToken"]["Entity"]["Id"],
                        EntityToken = (string)data["EntityToken"]["EntityToken"],
                        EntityType = (string)data["EntityToken"]["Entity"]["Type"]
                    };
                    callback?.Invoke(response);
                }
                catch (Exception e)
                {
                    NotificationManager.SendNotification($"PlayFab parse error: {e.Message}", 3f);
                    callback?.Invoke(null);
                }
            });
        }

        public class CreateAccountRequest
        {
            public string TitleId;
            public bool CreateAccount;
            public string CustomId;
        }
        public class CreateAccountResponse
        {
            public string PlayFabId;
            public string SessionTicket;
            public string EntityId;
            public string EntityToken;
            public string EntityType;
        }
    }
}