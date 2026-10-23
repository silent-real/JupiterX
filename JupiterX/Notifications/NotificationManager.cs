// ============================================================
//  JupiterX | Notifications/NotificationManager.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using JupiterX.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace JupiterX.Notifications
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class NotificationManager : MonoBehaviour
    {
        public NotificationManager(IntPtr e) : base(e) { }
        public static NotificationManager instance;
        public GameObject HUDObj;
        public GameObject HUDObj2;
        private GameObject MainCamera;
        private Material AlertText = new Material(Utility.GUIShader());
        public static Text NotifiText;
        public static Text ModText;
        public static Text StatsText;
        private bool HasInit;
        public static float notificationDecayTime = 1f;
        private static List<Notification> activeNotifications = new List<Notification>();
        public static Dictionary<string, string> information = new Dictionary<string, string>();

        public void Start()
        {
            instance = this;
        }

        private void Init()
        {
            MainCamera = Camera.main.gameObject;
            HUDObj = new GameObject("HUD");
            HUDObj2 = new GameObject("HUD_PARENT");
            HUDObj.transform.SetParent(HUDObj2.transform);
            HUDObj.AddComponent<Canvas>();
            HUDObj.AddComponent<CanvasScaler>();
            HUDObj.AddComponent<GraphicRaycaster>();
            Canvas canvas = HUDObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = MainCamera.GetComponent<Camera>();
            RectTransform rect = HUDObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(5f, 5f);
            HUDObj2.transform.SetParent(MainCamera.transform);
            HUDObj2.transform.position = MainCamera.transform.position;
            rect.localPosition = new Vector3(0f, 0f, 1.6f);
            HUDObj.transform.localScale = Vector3.one;
            rect.rotation = Quaternion.Euler(0f, -270f, 0f);
            NotifiText = CreateText("Notifications", new Vector2(450f, 210f), TextAnchor.LowerLeft, new Vector3(-1f, -1f, -0.5f), 25); // 30
            ModText = CreateText("Mods", new Vector2(450f, 1000f), TextAnchor.UpperLeft, new Vector3(-1f, -1f, -0.5f), 20);
            StatsText = CreateText("Stats", new Vector2(450f, 1000f), TextAnchor.UpperRight, new Vector3(-1f, -1f, 0.5f), 20); // 30
        }

        private Text CreateText(string name, Vector2 size, TextAnchor anchor, Vector3 pos, int fontSize)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(HUDObj.transform);
            Text txt = obj.AddComponent<Text>();
            txt.text = "";
            txt.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            txt.fontSize = fontSize;
            txt.alignment = anchor;
            txt.rectTransform.sizeDelta = size;
            txt.rectTransform.localScale = new Vector3(0.0033f, 0.0033f, 0.0033f);
            txt.rectTransform.localPosition = pos;
            txt.material = AlertText;
            txt.supportRichText = true;
            return txt;
        }

        public void FixedUpdate()
        {
            try
            {
                if (!HasInit && Camera.main != null)
                {
                    Init();
                    HasInit = true;
                }
                if (!HasInit) return;
                HUDObj.GetComponent<CanvasScaler>().dynamicPixelsPerUnit = Settings.lowqualttext ? 1f : 2f;
                HUDObj2.transform.position = MainCamera.transform.position;
                HUDObj2.transform.rotation = MainCamera.transform.rotation;
                UpdateNotifications();
                try
                {
                    Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    NotifiText.font = font;
                    ModText.font = font;
                    StatsText.font = font;
                    NotifiText.fontStyle = FontStyle.Normal;
                    StatsText.fontStyle = FontStyle.Normal;
                    ModText.fontStyle = Settings.advancedArraylist ? (FontStyle)((int)FontStyle.Normal % 2) : FontStyle.Normal;
                }
                catch { }
                ModText.rectTransform.localPosition = new Vector3(-1f, -1f, Settings.flipArraylist ? 0.5f : -0.5f);
                ModText.alignment = Settings.flipArraylist ? TextAnchor.UpperRight : TextAnchor.UpperLeft;
                StatsText.rectTransform.localPosition = new Vector3(-1f, -1f, Settings.flipArraylist ? -0.5f : 0.5f);
                StatsText.alignment = Settings.flipArraylist ? TextAnchor.UpperLeft : TextAnchor.UpperRight;
                if (information.Count > 0)
                {
                    TextGenerationSettings settings = ModText.GetGenerationSettings(ModText.rectTransform.rect.size);
                    List<string> stats = information.Select(item => $"<color=cyan>{item.Key}</color> {item.Value}").OrderByDescending(item => StatsText.cachedTextGenerator.GetPreferredWidth(item, settings)).ToList();
                    StatsText.text = string.Join("\n", stats);
                    StatsText.color = Color.white;
                }
                else 
                    StatsText.text = "";
                if (Settings.showEnabledModsVR)
                {
                    List<string> mods = new List<string>();
                    foreach (var category in Buttons.buttons)
                    {
                        foreach (var b in category)
                        {
                            if (Buttons.buttons[Buttons.GetCategory("Temporary Category")].Contains(b) || b.hideFromArraylist)
                                continue;

                            if (b.enabled)
                            {
                                string t = b.overlapText ?? b.buttonText;
                                if (Settings.lowercaseMode)
                                    t = t.ToLower();
                                mods.Add(t);
                            }
                        }
                    }
                    ModText.text = string.Join("\n", mods.OrderByDescending(x => x.Replace("<.*?>", "").Length));
                    ModText.color = Settings.textColors[0];
                }
                else ModText.text = "";
            }
            catch (Exception e)
            {
                Utility.Log(e.Message, JupxLogType.Error);
            }
        }

        public static void SendNotification(string text, float duration = -1f)
        {
            if (!Settings.Notifications) return;
            if (NotifiText == null) return;
            if (duration < 0)
                duration = notificationDecayTime;
            if (!text.EndsWith("\n"))
                text += "\n";
            if (Settings.disableNotificationSound)
            {
                Utility.PlaySound(Utility.notificationSound);
            }
            activeNotifications.Add(new Notification
            {
                Text = text,
                ExpireTime = Time.time + duration
            });
        }

        private void UpdateNotifications()
        {
            float time = Time.time;
            activeNotifications.RemoveAll(n => time >= n.ExpireTime);
            NotifiText.text = string.Concat(activeNotifications.Select(n => n.Text));
            if (Settings.lowercaseMode)
                NotifiText.text = NotifiText.text.ToLower();
        }

        public static void ClearAllNotifications()
        {
            activeNotifications.Clear();
            if (NotifiText != null)
                NotifiText.text = "";
        }
    }

    class Notification
    {
        public string Text;
        public float ExpireTime;
    }
}