// ============================================================
//  JupiterX | Menu/Main.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using easyInputs;
using JupiterX.Classes.Menu;
using JupiterX.Extensions;
using JupiterX.Managers;
using JupiterX.Mods;
using JupiterX.Notifications;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static JupiterX.Menu.Buttons;
using static JupiterX.Settings;

namespace JupiterX.Menu
{
	public class Main
	{
        public static void OnPreload()
        {
            NetworkSystem.JoinedRoom += OnJoinedRoom;
            NetworkSystem.LeftRoom += OnLeftRoom;
            NetworkSystem.MasterClientSwitched += OnMasterClientSwitched;

            NetworkSystem.PlayerJoinedRoom += OnPlayerJoined;
            NetworkSystem.PlayerLeftRoom += OnPlayerLeave;
        }

        public static void OnDeinitMelon()
        {
            NetworkSystem.JoinedRoom -= OnJoinedRoom;
            NetworkSystem.LeftRoom -= OnLeftRoom;
            NetworkSystem.MasterClientSwitched -= OnMasterClientSwitched;

            NetworkSystem.PlayerJoinedRoom -= OnPlayerJoined;
            NetworkSystem.PlayerLeftRoom -= OnPlayerLeave;
        }

        public static void Prefix()
		{
            if (Utility.canusemenu == false)
            {
                NotificationManager.SendNotification("<color=red>[INFO]</color> Menu is locked!", 15f);
                return;
            }

            if (Utility.blacklisted)
            {
                NotificationManager.SendNotification("<color=yellow>[BLACKLISTED]</color> You can not use jupiterx anymore. please join the discord https://novax.lol/d", 15f);
                return;
            }

            Utility.RightPrimary = EasyInputs.GetPrimaryButtonDown(EasyHand.RightHand);
            Utility.RightSecondary = EasyInputs.GetSecondaryButtonDown(EasyHand.RightHand);
            Utility.RightGrip = EasyInputs.GetGripButtonDown(EasyHand.RightHand);
            Utility.RightTrigger = EasyInputs.GetTriggerButtonDown(EasyHand.RightHand);
            Utility.RightTriggerFloat = EasyInputs.GetTriggerButtonFloat(EasyHand.RightHand);
            Utility.RightJoystick = EasyInputs.GetThumbStickButtonDown(EasyHand.RightHand);
            Utility.RightJoystickAxis = EasyInputs.GetThumbStick2DAxis(EasyHand.RightHand);

            Utility.LeftPrimary = EasyInputs.GetPrimaryButtonDown(EasyHand.LeftHand);
            Utility.LeftSecondary = EasyInputs.GetSecondaryButtonDown(EasyHand.LeftHand);
            Utility.LeftGrip = EasyInputs.GetGripButtonDown(EasyHand.LeftHand);
            Utility.LeftTrigger = EasyInputs.GetTriggerButtonDown(EasyHand.LeftHand);
            Utility.LeftTriggerFloat = EasyInputs.GetTriggerButtonFloat(EasyHand.LeftHand);
            Utility.LeftJoystick = EasyInputs.GetThumbStickButtonDown(EasyHand.LeftHand);
            Utility.LeftJoystickAxis = EasyInputs.GetThumbStick2DAxis(EasyHand.LeftHand);

            try
			{
                Utility.toOpen = bothHands ? (Utility.LeftSecondary || Utility.RightSecondary) : (!RightHanded && Utility.LeftSecondary || (RightHanded && Utility.RightSecondary));

                if (menu == null)
				{
					if (Utility.toOpen)
					{
                        if (!DisableMenuSounds)
                            Utility.PlaySound(Utility.menuOpenSound);
                        CreateMenu();

                        menuOpenCount++;
                        if (menuOpenCount == 100)
                            AchievementManager.UnlockAchievement(new AchievementManager.Achievement
                            {
                                name = "Persistent",
                                description = "Open the menu 100 times."
                            });

                        if (dynamicAnimations)
                            Plugin.StartCoroutine(GrowCoroutine());

                        RecenterMenu();
						if (reference == null)
						    CreateReference();
					}
				}
				else
				{
					if (Utility.toOpen)
					    RecenterMenu();
					else
					{
						if (!dynamicAnimations)
                        {
                            Rigidbody comp = menu.GetOrAddComponent<Rigidbody>();
                            switch (Utility.MainDropType)
                            {
                                case 0: // Destroy
                                    UnityEngine.Object.Destroy(menu, Time.deltaTime);
                                    break;

                                case 1: // Drop
                                    comp.velocity = Vector3.zero;
                                    UnityEngine.Object.Destroy(menu, 5);
                                    break;

                                case 2: // Drop
                                    comp.useGravity = false;
                                    comp.velocity = RightHanded ? Utility.ThrowMenu(EasyHand.RightHand) : Utility.ThrowMenu(EasyHand.LeftHand);
                                    UnityEngine.Object.Destroy(menu, 5);
                                    break;

                                case 3: // Throw
                                    comp.velocity = RightHanded ? Utility.ThrowMenu(EasyHand.RightHand) : Utility.ThrowMenu(EasyHand.LeftHand);
                                    UnityEngine.Object.Destroy(menu, 5);
                                    break;
                            }
                            menu = null;
                            UnityEngine.Object.Destroy(reference);
                            reference = null;
                        }
                        else
                        {
                            Plugin.StartCoroutine(ShrinkCoroutine());

                            GameObject.Destroy(reference);
                            reference = null;
                        }

                        if (!DisableMenuSounds)
                            Utility.PlaySound(Utility.menuCloseSound);
                    }
				}
			}
			catch (Exception exc)
			{
				Utility.Log(string.Format("{0} // Error with drawing {1} : {2}", Utility.name, exc.StackTrace, exc.Message), JupxLogType.Error);
			}

            if (Utility.isTriggers)
            {
                if (menu != null)
				{
                    if (Utility.LeftTrigger)
                    {
                        if (!Utility.hasTriggeredOnceL)
                        {
                            Utility.hasTriggeredOnceL = true;
                            Toggle("PreviousPage");
                        }
                    }
                    else
                        Utility.hasTriggeredOnceL = false;
                    if (Utility.RightTrigger)
                    {
                        if (!Utility.hasTriggeredOnceR)
                        {
                            Utility.hasTriggeredOnceR = true;
                            Toggle("NextPage");
                        }
                    }
                    else
                        Utility.hasTriggeredOnceR = false;
                }
            }

            Utility.lastDeltaTime = (1f / Time.deltaTime).ToString();

            // Constant
            try
            {
                // Execute Enabled mods
                if (fpsObject != null)
                    fpsObject.text = "FPS: " + Utility.fps;

                foreach (ButtonInfo[] buttonlist in buttons)
				{
					foreach (ButtonInfo v in buttonlist)
					{
						if (v.enabled)
						{
							if (v.method != null)
							{
								try
								{
									v.method.Invoke();
								}
								catch (Exception exc)
								{
                                    Utility.Log(string.Format("{0} // Error with mod {1} at {2}: {3}", Utility.name, v.buttonText, exc.StackTrace, exc.Message), JupxLogType.Error);
								}
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				Utility.Log(string.Format("{0} // Error with executing mods at {1}: {2}", Utility.name, exc.StackTrace, exc.Message), JupxLogType.Error);
			}

            if (pointerTrail)
            {
                try
                {
                    TrailRenderer trail = reference.GetOrAddComponent<TrailRenderer>();
                    _trailMat ??= new Material(Shader.Find("Sprites/Default"));
                    trail.startColor = backgroundColor.GetColor(0);
                    trail.endColor = backgroundColor.GetColor(1);
                    trail.startWidth = 0.015f;
                    trail.endWidth = 0f;
                    trail.minVertexDistance = 0.05f;
                    trail.material = _trailMat;
                    trail.time = 2f;
                }
                catch { }
            }

            if (menuTrail)
            {
                try
                {
                    TrailRenderer trail = menu.GetOrAddComponent<TrailRenderer>();
                    _trailMat ??= new Material(Shader.Find("Sprites/Default"));
                    trail.startColor = backgroundColor.GetColor(0);
                    trail.endColor = backgroundColor.GetColor(1);
                    trail.startWidth = 0.015f;
                    trail.endWidth = 0f;
                    trail.minVertexDistance = 0.05f;
                    trail.material = _trailMat;
                    trail.time = 2f;
                }
                catch { }
            }
        }

        // Functions
        public static void CreateMenu()
		{
			// Menu Holder
			menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
			UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
			UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
			UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
			menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);

            // Menu Background
            menuBackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
			UnityEngine.Object.Destroy(menuBackground.GetComponent<Rigidbody>());
			UnityEngine.Object.Destroy(menuBackground.GetComponent<BoxCollider>());
			menuBackground.transform.parent = menu.transform;
			menuBackground.transform.rotation = Quaternion.identity;
            menuBackground.transform.localScale = new Vector3(0.1f, 1f, 1f);
			menuBackground.transform.position = new Vector3(0.05f, 0f, 0f);

            if (Rounding)
                RoundMenuObject(menuBackground);

            ColorChanger colorChanger = menuBackground.GetOrAddComponent<ColorChanger>();
            colorChanger.colors = backgroundColor;
            colorChanger.Start();

            if (menuoutline)
            {
                GameObject outlineMenu = GameObject.CreatePrimitive(PrimitiveType.Cube);
                outlineMenu.transform.parent = menu.transform;
                UnityEngine.Object.Destroy(outlineMenu.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(outlineMenu.GetComponent<BoxCollider>());
                outlineMenu.transform.localScale = new Vector3(0.08f, 1.05f, 1.05f);
                outlineMenu.transform.position = new Vector3(0.05f, 0f, 0f);
                outlineMenu.GetComponent<Renderer>().material.color = buttonColors[0].GetCurrentColor();
            }

			// Canvas
			canvasObject = new GameObject();
			canvasObject.transform.parent = menu.transform;
			Canvas canvas = canvasObject.GetOrAddComponent<Canvas>();
			CanvasScaler canvasScaler = canvasObject.GetOrAddComponent<CanvasScaler>();
			canvasObject.GetOrAddComponent<GraphicRaycaster>();
			canvas.renderMode = RenderMode.WorldSpace;
			canvasScaler.dynamicPixelsPerUnit = lowqualttext ? 1000f : 2500f;

			// Title and FPS
			Text text = new GameObject
			{
				transform =
					{
						parent = canvasObject.transform
					}
			}.GetOrAddComponent<Text>();
			text.font = currentFont;

            if (CustomMenuTitle)
            {
                if (MenuTitle)
                {
                    string path = Path.Combine(Application.persistentDataPath, "JupiterX/CustomTitle.txt");
                    string CustomTitle;
                    if (File.Exists(path))
                    {
                        CustomTitle = File.ReadAllText(path);
                    }
                    else
                    {
                        CustomTitle = "Your Text Here";
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        File.WriteAllText(path, CustomTitle);
                    }
                    text.text = CustomTitle + (DisablePageNumber ? "" : " <color=cyan>[</color><color=white>" + (pageNumber + 1).ToString() + "</color><color=cyan>]</color>");
                }
                else
                    text.text = "";
            }
            else
            {
                if (MenuTitle)
                    text.text = Utility.name + (DisablePageNumber ? "" : " <color=cyan>[</color><color=white>" + (pageNumber + 1).ToString() + "</color><color=cyan>]</color>");
                else
                    text.text = "";
            }

            if (lowercaseMode)
                text.text = text.text.ToLower();
            if (uppercaseMode)
                text.text = text.text.ToUpper();

            text.fontSize = 1;
			text.color = textColors[0];
			text.supportRichText = true;
			text.fontStyle = Utility.currentFontStyle;
			text.alignment = TextAnchor.MiddleCenter;
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = 0;
			RectTransform component = text.GetComponent<RectTransform>();
			component.localPosition = Vector3.zero;
			component.sizeDelta = new Vector2(0.28f, 0.05f);
            if (NoAutoSizeText)
                component.sizeDelta = new Vector2(9f, 0.015f);
            component.position = new Vector3(0.06f, 0f, 0.165f);
			component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

			if (VersionText)
			{
                Text buildLabel = new GameObject
                {
                    transform =
                {
                    parent = canvasObject.transform
                }
                }.GetOrAddComponent<Text>();
                buildLabel.font = currentFont;
                buildLabel.text = $"Build {Utility.version}";

                if (lowercaseMode)
                    buildLabel.text = buildLabel.text.ToLower();

                if (uppercaseMode)
                    buildLabel.text = buildLabel.text.ToUpper();

                buildLabel.fontSize = 1;
                buildLabel.color = textColors[0];
                buildLabel.supportRichText = true;
                buildLabel.fontStyle = FontStyle.Italic;
                buildLabel.alignment = TextAnchor.MiddleRight;
                buildLabel.resizeTextForBestFit = true;
                buildLabel.resizeTextMinSize = 0;
                component = buildLabel.GetComponent<RectTransform>();
                component.localPosition = Vector3.zero;
                component.sizeDelta = new Vector2(0.28f, 0.02f);
                component.position = new Vector3(0.04f, 0.0f, -0.17f);

                component.rotation = Quaternion.Euler(new Vector3(0f, 90f, 90f));
			}

            if (showfps)
            {
                fpsObject = new GameObject
                {
                    transform =
                    {
                        parent = canvasObject.transform
                    }
                }.GetOrAddComponent<Text>();
                fpsObject.font = currentFont;
                fpsObject.text = "FPS: " + Utility.fps;
                fpsObject.color = textColors[0];
                fpsObject.fontSize = 1;
                fpsObject.supportRichText = true;
                fpsObject.fontStyle = Utility.currentFontStyle;
                fpsObject.alignment = TextAnchor.MiddleCenter;
                fpsObject.horizontalOverflow = UnityEngine.HorizontalWrapMode.Overflow;
                fpsObject.resizeTextForBestFit = true;
                fpsObject.resizeTextMinSize = 0;

                if (lowercaseMode)
                    fpsObject.text = fpsObject.text.ToLower();
                if (uppercaseMode)
                    fpsObject.text = fpsObject.text.ToUpper();

                RectTransform component2 = fpsObject.GetComponent<RectTransform>();
                component2.localPosition = Vector3.zero;
                component2.sizeDelta = new Vector2(0.28f, 0.02f);
                if (NoAutoSizeText)
                    component2.sizeDelta = new Vector2(9f, 0.015f);
                component2.position = new Vector3(0.06f, 0f, 0.135f);
                component2.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
            }

            float hkbStartTime = -0.3f;

            if (!DisconnectButton)
            {
                AddButton(-0.3f, -1, GetIndex("Disconnect"));
                hkbStartTime -= 0.1f;
            }

            if (quickActions.Count > 0)
            {
                foreach (string action in quickActions)
                {
                    ButtonInfo button = GetIndex(action);
                    if (button == null)
                    {
                        quickActions.Remove(action);
                        continue;
                    }

                    AddButton(hkbStartTime, -1, button);
                    hkbStartTime -= 0.1f;
                }
            }

            if (!disableReturnButton && Buttons.CurrentCategoryName != "Main")
                AddReturnButton(false);

            // Buttons
            // Page Buttons
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

			UnityEngine.Object.Destroy(gameObject.GetComponent<Rigidbody>());
			gameObject.GetComponent<BoxCollider>().isTrigger = true;
			gameObject.transform.parent = menu.transform;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.transform.localScale = Utility.PageObjScale;
			gameObject.transform.localPosition = Utility.PageObjectPosLeft;
            gameObject.GetOrAddComponent<ButtonCollider>().relatedText = "NextPage"; // PreviousPage

            colorChanger = gameObject.GetOrAddComponent<ColorChanger>();
			colorChanger.colors = buttonColors[0];
			colorChanger.Start();

			text = new GameObject
			{
				transform =
						{
							parent = canvasObject.transform
						}
			}.GetOrAddComponent<Text>();
			text.font = currentFont;
			text.text = "<";
			text.fontSize = 1;
			text.color = textColors[0];
			text.alignment = TextAnchor.MiddleCenter;
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = 0;
			component = text.GetComponent<RectTransform>();
			component.localPosition = Vector3.zero;
			component.sizeDelta = new Vector2(0.2f, 0.03f);
            if (NoAutoSizeText)
                component.sizeDelta = new Vector2(9f, 0.015f);
            component.localPosition = Utility.PageTextPosLeft; ;
			component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

			GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

			UnityEngine.Object.Destroy(gameObject2.GetComponent<Rigidbody>());
            gameObject2.GetComponent<BoxCollider>().isTrigger = true;
            gameObject2.transform.parent = menu.transform;
            gameObject2.transform.rotation = Quaternion.identity;
            gameObject2.transform.localScale = Utility.PageObjScale;
            gameObject2.transform.localPosition = Utility.PageObjectPosRight;
            gameObject2.GetOrAddComponent<ButtonCollider>().relatedText = "PreviousPage"; // NextPage

            colorChanger = gameObject2.GetOrAddComponent<ColorChanger>();
			colorChanger.colors = buttonColors[0];
			colorChanger.Start();

            if (lowercaseMode)
                text.text = text.text.ToLower();
            if (uppercaseMode)
                text.text = text.text.ToUpper();

            text = new GameObject
			{
				transform =
						{
							parent = canvasObject.transform
						}
			}.GetOrAddComponent<Text>();
			text.font = currentFont;
			text.text = ">";
			text.fontSize = 1;
			text.color = textColors[0];
			text.alignment = TextAnchor.MiddleCenter;
			text.resizeTextForBestFit = true;
			text.resizeTextMinSize = 0;
			component = text.GetComponent<RectTransform>();
			component.localPosition = Vector3.zero;
			component.sizeDelta = new Vector2(0.2f, 0.03f);
            if (NoAutoSizeText)
                component.sizeDelta = new Vector2(9f, 0.015f);
            component.localPosition = Utility.PageTextPosRight;
			component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

			if (Rounding)
			{
				RoundMenuObject(gameObject);
				RoundMenuObject(gameObject2);
			}

            int buttonIndexOffset = 0;
            ButtonInfo[] renderButtons = new ButtonInfo[] { };

            if (CurrentPrompt != null)
                RenderPrompt();
            else
            {
                try
                {
                    if (CurrentCategoryName == "Main")
                    {
                        List<ButtonInfo> buttons = new List<ButtonInfo>();
                        foreach (var button in Buttons.buttons[CurrentCategoryIndex])
                        {
                            if (!skipButtons.Contains(button.buttonText))
                                buttons.Add(button);
                        }
                        renderButtons = buttons.ToArray();
                    }
                    else if (CurrentCategoryName == "Favorite")
                    {
                        foreach (var favoriteMod in favorites.Where(favoriteMod => Buttons.GetIndex(favoriteMod) == null).ToList())
                            favorites.Remove(favoriteMod);
                        renderButtons = StringsToInfos(favorites.ToArray());
                    }
                    else if (CurrentCategoryName == "Enabled")
                    {
                        List<ButtonInfo> enabledMods = new List<ButtonInfo>() { };
                        int categoryIndex = 0;
                        foreach (ButtonInfo[] buttonlist in buttons)
                        {
                            foreach (ButtonInfo v in buttonlist)
                            {
                                if (v.enabled && (!categoryNames[categoryIndex].Contains("Settings")))
                                    enabledMods.Add(v);
                            }
                            categoryIndex++;
                        }
                        enabledMods = enabledMods.OrderBy(v => v.buttonText).ToList();
                        enabledMods.Insert(0, GetIndex("Exit Enabled"));

                        renderButtons = enabledMods.ToArray();
                    }
                    else
                        renderButtons = Buttons.buttons[Buttons.CurrentCategoryIndex];

                    renderButtons = renderButtons.Skip(pageNumber * (buttonsPerPage - buttonIndexOffset)).Take(buttonsPerPage - buttonIndexOffset).ToArray();

                    // Mod Buttons
                    for (int i = 0; i < renderButtons.Length; i++)
                        AddButton((i + buttonIndexOffset + 0.1f) * 0.1f, i, renderButtons[i]);
                }
                catch 
                {
                    Utility.Log("Menu draw is erroring, returning to home page", JupxLogType.Warning);
                    CurrentCategoryName = "Main";
                }
            }

            menu.transform.localScale *= menuScale;
        }

        private static void AddButton(float offset, int buttonIndex, ButtonInfo method)
        {
            if (!method.label)
            {
                GameObject buttonObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                buttonObject.GetComponent<BoxCollider>().isTrigger = true;
                buttonObject.transform.parent = menu.transform;
                buttonObject.transform.rotation = Quaternion.identity;

                buttonObject.transform.localScale = new Vector3(0.09f, 0.9f, 0.1f * 0.8f);

                buttonObject.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - offset);

                ButtonCollider Button = buttonObject.GetOrAddComponent<ButtonCollider>();
                Button.relatedText = method.buttonText;

                ColorChanger colorChanger = buttonObject.GetOrAddComponent<ColorChanger>();
                colorChanger.Start();

                if (incrementalButtons)
                {
                    if (method.incremental)
                    {
                        buttonObject.transform.localScale -= new Vector3(0f, 0.254f, 0f);
                        GameObject.Destroy(Button);

                        RenderIncrementalButton(false, offset, buttonIndex, method);
                        RenderIncrementalButton(true, offset, buttonIndex, method);
                    }
                }

                if (lastClickedName != method.buttonText)
                {
                    if (method.enabled)
                    {
                        buttonObject.GetComponent<Renderer>().material.color = buttonColors[1].GetCurrentColor();
                    }
                    else
                    {
                        buttonObject.GetComponent<Renderer>().material.color = buttonColors[0].GetCurrentColor();
                    }
                }

                if (Rounding)
                    RoundMenuObject(buttonObject);
            }

            Text buttonText = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.GetOrAddComponent<Text>();

            string targetButtonText = method.overlapText ?? method.buttonText;

            buttonText.font = currentFont;
            targetButtonText = method.buttonText;

            if (inputTextColor != "cyan")
                targetButtonText = targetButtonText.Replace(" <color=grey>[</color><color=cyan>", $" <color=grey>[</color><color={inputTextColor}>");

            if (lowercaseMode)
                targetButtonText = targetButtonText.ToLower();
            if (uppercaseMode)
                targetButtonText = targetButtonText.ToUpper();

            if (method.overlapText != null)
                targetButtonText = method.overlapText;

            if (favorites.Contains(method.buttonText))
                targetButtonText += " ✦";

            buttonText.supportRichText = true;
            buttonText.fontSize = 1;
            buttonText.color = method.enabled ? textColors[1] : textColors[0];

            buttonText.text = targetButtonText;

            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.fontStyle = Utility.currentFontStyle;
            buttonText.resizeTextForBestFit = true;
            buttonText.resizeTextMinSize = 0;

            RectTransform textTransform = buttonText.GetComponent<RectTransform>();
            textTransform.localPosition = Vector3.zero;
            textTransform.sizeDelta = new Vector2(method.incremental && incrementalButtons ? .18f : .2f, .03f * (0.1f / 0.1f));
            if (NoAutoSizeText)
                textTransform.sizeDelta = new Vector2(9f, 0.015f);

            textTransform.localPosition = new Vector3(.064f, 0, .111f - offset / 2.6f);
            textTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        private static void RenderIncrementalButton(bool increment, float offset, int buttonIndex, ButtonInfo method)
        {
            if (!method.label)
            {
                GameObject buttonObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

                buttonObject.GetComponent<BoxCollider>().isTrigger = true;
                buttonObject.transform.parent = menu.transform;
                buttonObject.transform.rotation = Quaternion.identity;

                buttonObject.transform.localScale = new Vector3(0.09f, 0.102f, 0.1f * 0.8f);
                buttonObject.transform.localPosition = new Vector3(0.56f, 0.399f, 0.28f - offset);

                ButtonCollider button = buttonObject.GetOrAddComponent<ButtonCollider>();
                button.relatedText = method.buttonText;
                button.incremental = true;
                button.positive = increment;

                if (increment)
                    buttonObject.transform.localPosition = new Vector3(buttonObject.transform.localPosition.x, -buttonObject.transform.localPosition.y, buttonObject.transform.localPosition.z);

                if (lastClickedName != method.buttonText + (increment ? "+" : "-"))
                {
                    ColorChanger colorChanger = buttonObject.GetOrAddComponent<ColorChanger>();
                    colorChanger.colors = buttonColors[0];
                }
            }

            RenderIncrementalText(increment, offset);
        }

        public static void RenderIncrementalText(bool increment, float offset)
        {
            Text buttonText = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.GetOrAddComponent<Text>();

            buttonText.font = currentFont;
            buttonText.text = increment ? "+" : "-";
            buttonText.supportRichText = true;
            buttonText.fontSize = 1;
            buttonText.color = textColors[1];

            buttonText.alignment = TextAnchor.MiddleCenter;
            buttonText.fontStyle = Utility.currentFontStyle;
            buttonText.resizeTextForBestFit = true;
            buttonText.resizeTextMinSize = 0;

            RectTransform textTransform = buttonText.GetComponent<RectTransform>();
            textTransform.localPosition = Vector3.zero;
            textTransform.sizeDelta = new Vector2(.2f, .03f * (0.1f / 0.1f));

            textTransform.localPosition = new Vector3(.064f, increment ? -0.12f : 0.12f, .111f - offset / 2.6f);
            textTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        public static void ReloadMenu()
        {
            if (menu != null)
            {
                GameObject.Destroy(menu);
                menu = null;

                CreateMenu();
            }

            if (reference != null)
            {
                GameObject.Destroy(reference);
                reference = null;

                CreateReference();
            }
        }

        private static void AddReturnButton(bool offcenteredPosition)
        {
            GameObject buttonObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            buttonObject.GetComponent<BoxCollider>().isTrigger = true;
            buttonObject.transform.parent = menu.transform;
            buttonObject.transform.rotation = Quaternion.identity;

            buttonObject.transform.localScale = new Vector3(0.09f, 0.102f, 0.08f);
            buttonObject.transform.localPosition = new Vector3(0.56f, -0.450f, -0.58f);

            if (offcenteredPosition)
                buttonObject.transform.localPosition += new Vector3(0f, 0.16f, 0f);

            buttonObject.GetOrAddComponent<ButtonCollider>().relatedText = "Global Return";

            if (lastClickedName != "Global Return")
            {
                ColorChanger colorChanger = buttonObject.GetOrAddComponent<ColorChanger>();
                colorChanger.colors = colorChanger.colors = buttonColors[0];
            }

            Image returnImage = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.GetOrAddComponent<Image>();

            if (returnIcon == null)
                returnIcon = LoadTexture("return");

            if (returnMat == null)
                returnMat = new Material(returnImage.material);

            returnImage.material = returnMat;
            returnImage.material.SetTexture("_MainTex", returnIcon);
            returnImage.color = textColors[1];

            RectTransform imageTransform = returnImage.GetComponent<RectTransform>();
            imageTransform.localPosition = Vector3.zero;
            imageTransform.sizeDelta = new Vector2(.03f, .03f);

            imageTransform.localPosition = new Vector3(.064f, -0.35f / 2.6f, -0.58f / 2.6f);

            if (offcenteredPosition)
                imageTransform.localPosition += new Vector3(0f, 0.0475f, 0f);

            imageTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }

        /*private static void AddSearchButton()
        {
            GameObject buttonObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

            buttonObject.GetComponent<BoxCollider>().isTrigger = true;
            buttonObject.transform.parent = menu.transform;
            buttonObject.transform.rotation = Quaternion.identity;

            buttonObject.transform.localScale = new Vector3(0.09f, 0.102f, 0.08f);
            buttonObject.transform.localPosition = new Vector3(0.56f, -0.450f, -0.58f);

            buttonObject.GetOrAddComponent<ButtonCollider>().relatedText = "Search";

            ColorChanger colorChanger = buttonObject.GetOrAddComponent<ColorChanger>();
            colorChanger.colors = buttonColors[KeyboardManager.KeyboardEnabled  ? 1 : 0];

            Image searchImage = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.GetOrAddComponent<Image>();
            if (searchIcon == null)
                searchIcon = LoadTexture("search");

            if (searchMat == null)
                searchMat = new Material(searchImage.material);

            searchImage.material = searchMat;
            searchImage.material.SetTexture("_MainTex", searchIcon);
            searchImage.color = textColors[KeyboardManager.KeyboardEnabled ? 2 : 1];

            RectTransform imageTransform = searchImage.GetComponent<RectTransform>();
            imageTransform.localPosition = Vector3.zero;
            imageTransform.sizeDelta = new Vector2(.03f, .03f);

            imageTransform.localPosition = new Vector3(.064f, -0.35f / 2.6f, -0.58f / 2.6f);

            imageTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }*/

        public static Texture2D LoadTexture(string fileName)
        {
            using (Stream stream = typeof(Plugin).Assembly.GetManifestResourceStream($"JupiterX.Resources.{fileName}.png"))
            {
                if (stream == null) return null;

                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                Texture2D texture = new Texture2D(2, 2);
                ImageConversion.LoadImage(texture, bytes);
                return texture;
            }
        }

        public static void RecenterMenu()
		{
            if (RightHanded || (bothHands && Utility.RightSecondary))
            {
                menu.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                Vector3 rotation = GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles;
                rotation += new Vector3(0f, 0f, 180f);
                menu.transform.rotation = Quaternion.Euler(rotation);
            }
            else
            {
                menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
            }
            if (flipMenu)
            {
                Vector3 rotation = menu.transform.rotation.eulerAngles;
                rotation += new Vector3(0f, 0f, 180f);
                menu.transform.rotation = Quaternion.Euler(rotation);
            }

            if (smoothMenuPosition)
            {
                smoothTargetPosition = smoothTargetPosition == Vector3.zero ? menu.transform.position : Vector3.Lerp(smoothTargetPosition, menu.transform.position, Time.deltaTime * 10f);

                menu.transform.position = smoothTargetPosition;
            }

            if (!smoothMenuRotation) return;
            smoothTargetRotation = smoothTargetRotation == Quaternion.identity ? menu.transform.rotation : Quaternion.Lerp(smoothTargetRotation, menu.transform.rotation, Time.deltaTime * 10f);

            menu.transform.rotation = smoothTargetRotation;
        }

		public static void CreateReference()
		{
			reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            if (bothHands)
            {
                if (Utility.RightSecondary)
                    reference.transform.parent = GorillaTagger.Instance.leftHandTransform;
                else
                    reference.transform.parent = GorillaTagger.Instance.rightHandTransform;
            }
            reference.transform.parent = RightHanded ? GorillaTagger.Instance.leftHandTransform : GorillaTagger.Instance.rightHandTransform;
            reference.transform.localPosition = new Vector3(0.013f, -0.025f, 0.1f);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
			buttonCollider = reference.GetComponent<SphereCollider>();
            reference.GetComponent<Renderer>().material.color = backgroundColor.GetCurrentColor();
            if (hidepointer)
                reference.GetComponent<Renderer>().enabled = false;
            else
                reference.GetComponent<Renderer>().enabled = true;
		}

        public static void Toggle(string buttonText, bool fromMenu = false, bool ignoreForce = false)
        {
            int lastPage = ((Buttons.buttons[Buttons.CurrentCategoryIndex].Length + buttonsPerPage - 1) / buttonsPerPage) - 1;

            switch (Buttons.CurrentCategoryName)
            {
                case "Favorite":
                    lastPage = ((favorites.Count + buttonsPerPage - 1) / buttonsPerPage) - 1;
                    break;

                case "Enabled":
                    List<string> enabledMods = new List<string>() { "Exit Enabled" };
                    int categoryIndex = 0;

                    foreach (ButtonInfo[] buttonlist in Buttons.buttons)
                    {
                        foreach (ButtonInfo v in buttonlist)
                        {
                            if (v.enabled && !Buttons.categoryNames[categoryIndex].Contains("Settings"))
                                enabledMods.Add(v.buttonText);
                        }
                        categoryIndex++;
                    }

                    lastPage = ((enabledMods.Count + buttonsPerPage - 1) / buttonsPerPage) - 1;
                    break;
            }

            switch (buttonText)
            {
                case "Accept Prompt":
                    if (CurrentPrompt != null)
                    {
                        CurrentPrompt.AcceptAction?.Invoke();
                        if (prompts.Count > 0) prompts.RemoveAt(0);
                        ReloadMenu();
                    }
                    return;

                case "Decline Prompt":
                    if (CurrentPrompt != null)
                    {
                        CurrentPrompt.DeclineAction?.Invoke();
                        if (prompts.Count > 0) prompts.RemoveAt(0);
                        ReloadMenu();
                    }
                    return;

                case "Disconnect":
                    PhotonNetwork.Disconnect();
                    break;

                case "Home":
                    Buttons.CurrentCategoryName = "Main";
                    pageNumber = 0;
                    break;

                case "PreviousPage":
                    pageNumber = (pageNumber - 1 < 0) ? lastPage : pageNumber - 1;
                    break;

                case "NextPage":
                    pageNumber = (pageNumber + 1 > lastPage) ? 0 : pageNumber + 1;
                    break;

                default:
                    HandleButtonAction(buttonText, fromMenu, ignoreForce);
                    break;
            }

            ReloadMenu();
        }

        private static void HandleButtonAction(string buttonText, bool fromMenu, bool ignoreForce)
        {
            ButtonInfo target = GetIndex(buttonText);
            if (target == null)
            {
                Utility.Log($"{buttonText} does not exist", JupxLogType.Warning);
                return;
            }
            string newIndicator = " <color=grey>[</color><color=cyan>New</color><color=grey>]</color>";
            if (target.overlapText != null && target.overlapText.Contains(newIndicator))
            {
                target.overlapText = target.overlapText.Replace(newIndicator, "");
            }
            bool gripHeld = Utility.LeftGrip || (Utility.RightJoystickAxis.y > 0.5f && Utility.LeftTriggerFloat > 0.5f);
            bool triggerHeld = Utility.LeftTriggerFloat > 0.5f;

            switch (true)
            {
                case true when fromMenu && !ignoreForce && gripHeld:
                    if (target.buttonText == "Exit Favorite" && !disableFavoritingKeybind) return;

                    if (favorites.Contains(target.buttonText))
                    {
                        favorites.Remove(target.buttonText);
                        NotificationManager.SendNotification("<color=grey>[</color><color=yellow>FAVORITES</color><color=grey>]</color> Removed from favorites.");
                    }
                    else
                    {
                        favorites.Add(target.buttonText);
                        NotificationManager.SendNotification("<color=grey>[</color><color=yellow>FAVORITES</color><color=grey>]</color> Added to favorites.");
                    }
                    break;
                case true when fromMenu && !ignoreForce && triggerHeld:
                    if (!quickActions.Contains(target.buttonText))
                    {
                        quickActions.Add(target.buttonText);
                        NotificationManager.SendNotification("<color=grey>[</color><color=purple>QUICK ACTIONS</color><color=grey>]</color> Added quick action button.");
                    }
                    else
                    {
                        quickActions.Remove(target.buttonText);
                        NotificationManager.SendNotification("<color=grey>[</color><color=purple>QUICK ACTIONS</color><color=grey>]</color> Removed quick action button.");
                    }
                    break;
                default:
                    if (target.isTogglable)
                    {
                        target.enabled = !target.enabled;

                        if (target.enabled)
                        {
                            if (fromMenu)
                                NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>ENABLE</color><color=grey>]</color> {target.toolTip}");

                            try { target.enableMethod?.Invoke(); }
                            catch (Exception exc)
                            {
                                Utility.Log($"Error enabling {target.buttonText}: {exc.Message}", JupxLogType.Error);
                            }
                        }
                        else
                        {
                            if (fromMenu)
                                NotificationManager.SendNotification($"<color=grey>[</color><color=red>DISABLE</color><color=grey>]</color> {target.toolTip}");
                            try { target.disableMethod?.Invoke(); }
                            catch (Exception exc)
                            {
                                Utility.Log($"Error disabling {target.buttonText}: {exc.Message}", JupxLogType.Error);
                            }
                        }
                    }
                    else
                    {
                        if (fromMenu)
                            NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>ENABLE</color><color=grey>]</color> {target.toolTip}");

                        try { target.method?.Invoke(); }
                        catch (Exception exc)
                        {
                            Utility.Log($"Error running {target.buttonText}: {exc.Message}", JupxLogType.Error);
                        }
                    }
                    break;
            }
        }

        public static void ToggleIncremental(string buttonText, bool increment, bool fromMenu = false, bool ignoreForce = false, bool reload = true)
        {
            ButtonInfo target = Buttons.GetIndex(buttonText);
            if (target == null)
            {
                Utility.Log($"{buttonText} does not exist", JupxLogType.Warning);
                return;
            }
            string newIndicator = " <color=grey>[</color><color=cyan>New</color><color=grey>]</color>";
            if (target.overlapText != null && target.overlapText.Contains(newIndicator))
            {
                target.overlapText = target.overlapText.Replace(newIndicator, "");
            }
            if (target.label)
                return;
            bool triggerHeld = Utility.LeftTriggerFloat > 0.5f;
            switch (true)
            {
                case true when fromMenu && !ignoreForce && triggerHeld:
                    if (!quickActions.Contains(target.buttonText))
                    {
                        quickActions.Add(target.buttonText);
                        NotificationManager.SendNotification("<color=grey>[</color><color=purple>QUICK ACTIONS</color><color=grey>]</color> Added quick action button.");
                    }
                    else
                    {
                        quickActions.Remove(target.buttonText);
                        NotificationManager.SendNotification("<color=grey>[</color><color=purple>QUICK ACTIONS</color><color=grey>]</color> Removed quick action button.");
                    }
                    break;
                default:
                    if (dynamicAnimations)
                        lastClickedName = buttonText + (increment ? "+" : "-");
                    bool boost = incrementalBoost && Utility.RightGrip;
                    if (increment)
                    {
                        NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>INCREMENT</color><color=grey>]</color> {target.toolTip}");
                        if (boost)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                if (target.enableMethod == null) continue;
                                try { target.enableMethod.Invoke(); }
                                catch (Exception exc)
                                {
                                    Utility.Log($"Error with mod enableMethod {target.buttonText} at {exc.StackTrace}: {exc.Message}", JupxLogType.Error);
                                }
                            }
                        }
                        else
                        {
                            try { target.enableMethod?.Invoke(); }
                            catch (Exception exc)
                            {
                                Utility.Log($"Error enabling {target.buttonText}: {exc.Message}", JupxLogType.Error);
                            }
                        }
                    }
                    else
                    {
                        NotificationManager.SendNotification($"<color=grey>[</color><color=red>DECREMENT</color><color=grey>]</color> {target.toolTip}");
                        if (boost)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                if (target.enableMethod == null) continue;
                                if (target.disableMethod == null) continue;
                                try { target.disableMethod.Invoke(); }
                                catch (Exception exc)
                                {
                                    Utility.Log($"Error with mod disableMethod {target.buttonText} at {exc.StackTrace}: {exc.Message}", JupxLogType.Error);
                                }
                            }
                        }
                        else
                        {
                            try { target.disableMethod?.Invoke(); }
                            catch (Exception exc)
                            {
                                Utility.Log($"Error disabling {target.buttonText}: {exc.Message}", JupxLogType.Error);
                            }
                        }
                    }
                    break;
            }
            if (reload)
                ReloadMenu();
        }

        public static Material promptMat;

        public static string lastClickedName = "";
        public static GradientColorKey[] GetSolidGradient(Color color)
		{
			return new GradientColorKey[] { new GradientColorKey(color, 0f), new GradientColorKey(color, 1f) };
		}

        // Prompt Stuff
        public static string ExtractPromptImage(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            var match = System.Text.RegularExpressions.Regex.Match(input, @"<(?<url>https?://[^>]+)>");
            if (match.Success)
                return match.Groups["url"].Value;
            return null;
        }

        public static string GetFileExtension(string fileName) =>
            fileName.ToLower().Split('.')[fileName.Split('.').Length - 1];

        public static void CleanVisualsOnLeave(VRRig rig = null)
        {
            if (rig)
            {
                Visual.boxEspPool.Remove(rig);
                Visual.capsuleEspPool.Remove(rig);
                Visual.sphereEspPool.Remove(rig);
                Visual.nameTagPool.Remove(rig);
                Visual.IDnameTagPool.Remove(rig);
                Visual.PlatformnameTagPool.Remove(rig);
                Visual.MasternameTagPool.Remove(rig);
                Visual.TaggednameTagPool.Remove(rig);
            }
            else
            {
                Visual.boxEspPool.Clear();
                Visual.capsuleEspPool.Clear();
                Visual.sphereEspPool.Clear();
                Visual.nameTagPool.Clear();
                Visual.IDnameTagPool.Clear();
                Visual.PlatformnameTagPool.Clear();
                Visual.MasternameTagPool.Clear();
                Visual.TaggednameTagPool.Clear();
            }
        }

        public static void OnJoinedRoom()
        {
            lastRoom = PhotonNetwork.CurrentRoom.Name;
            if (!disableRoomNotifications)
                NotificationManager.SendNotification("<color=grey>[</color><color=blue>JOIN ROOM</color><color=grey>]</color> Room Code: " + lastRoom);
        }

        public static void OnLeftRoom()
        {
            if (clearNotificationsOnDisconnect)
                NotificationManager.ClearAllNotifications();

            if (!disableRoomNotifications)
                NotificationManager.SendNotification("<color=grey>[</color><color=blue>LEAVE ROOM</color><color=grey>]</color> Room Code: " + lastRoom);
            CleanVisualsOnLeave();
        }

        public static void OnMasterClientSwitched()
        {
            if (disableMasterClientNotifications)
                return;
            NotificationManager.SendNotification("<color=grey>[</color><color=purple>MASTER</color><color=grey>]</color> You are now master client.");

            if (!Utility.IsMaster())
                GetIndex("MasterLabel").overlapText = "You are not master client.";
            else
                GetIndex("MasterLabel").overlapText = "You are master client.";
        }

        public static void OnPlayerJoined(Photon.Realtime.Player player)
        {
            if (player != PhotonNetwork.LocalPlayer && !Main.disablePlayerNotifications)
            {
                NotificationManager.SendNotification($"<color=grey>[</color><color=green>JOIN</color><color=grey>]</color> Name: {Utility.CleanPlayerName(player.NickName)}");
            }
        }

        public static void OnPlayerLeave(Photon.Realtime.Player player)
        {
            if (player != PhotonNetwork.LocalPlayer && !Main.disablePlayerNotifications)
                NotificationManager.SendNotification($"<color=grey>[</color><color=red>LEAVE</color><color=grey>]</color> Name: {Utility.CleanPlayerName(player.NickName)}");

            CleanVisualsOnLeave(player.Rig());
        }

        private static void RenderPrompt()
        {
            if (CurrentPrompt == null)
                return;

            Text promptText = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.GetOrAddComponent<Text>();
            promptText.font = currentFont;
            promptText.text = CurrentPrompt.Message;

            string promptImageUrl = ExtractPromptImage(CurrentPrompt.Message);
            if (promptImageUrl != null)
                promptText.text = promptText.text.Replace($"<{promptImageUrl}>", "");

            promptText.fontSize = 1;
            promptText.lineSpacing = 0.8f;
            promptText.color = textColors[0];

            promptText.supportRichText = true;
            promptText.fontStyle = Utility.currentFontStyle;
            promptText.alignment = TextAnchor.MiddleCenter;
            promptText.resizeTextForBestFit = true;
            promptText.resizeTextMinSize = 0;
            RectTransform component = promptText.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(0.28f, CurrentPrompt.IsText ? 0.25f : 0.28f);

            component.localPosition = new Vector3(0.06f, 0f, CurrentPrompt.IsText ? -0.025f : 0f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            if (promptImageUrl != null)
            {
                string fileName = promptImageUrl.Split('/')[^1];
                string fileExtension = GetFileExtension(fileName);

                Image promptImage = new GameObject
                {
                    transform =
                    {
                        parent = canvasObject.transform
                    }
                }.GetOrAddComponent<Image>();

                component.sizeDelta = new Vector2(component.sizeDelta.x, 0.03f);
                component.localPosition = new Vector3(0.06f, 0f, 0.1f);

                if (promptMat == null)
                    promptMat = new Material(promptImage.material);

                promptImage.material = promptMat;

                RectTransform imageTransform = promptImage.GetComponent<RectTransform>();
                imageTransform.localPosition = Vector3.zero;
                imageTransform.sizeDelta = new Vector2(.2f, .2f);

                imageTransform.localPosition = new Vector3(0.06f, 0f, string.IsNullOrEmpty(promptText.text) ? 0f : -0.03f);
                imageTransform.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

            }
            {
                GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);

                button.GetComponent<BoxCollider>().isTrigger = true;
                button.transform.parent = menu.transform;
                button.transform.rotation = Quaternion.identity;
                button.transform.localScale = new Vector3(0.09f, CurrentPrompt.DeclineText == null ? 0.9f : 0.4375f, 0.08f);
                button.transform.localPosition = new Vector3(0.56f, CurrentPrompt.DeclineText == null ? 0f : 0.2375f, -0.43f);

                button.GetOrAddComponent<ButtonCollider>().relatedText = "Accept Prompt";

                if (lastClickedName != "Accept Prompt")
                {
                    ColorChanger colorChanger = button.GetOrAddComponent<ColorChanger>();
                    colorChanger.colors = buttonColors[0];
                }

                Text text = new GameObject { transform = { parent = canvasObject.transform } }.GetOrAddComponent<Text>();
                text.font = currentFont;
                text.fontStyle = Utility.currentFontStyle;
                text.text = CurrentPrompt.AcceptText;
                text.fontSize = 1;
                text.alignment = TextAnchor.MiddleCenter;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 0;
                text.color = textColors[1];

                RectTransform textRect = text.GetComponent<RectTransform>();
                textRect.sizeDelta = new Vector2(0.2f, 0.03f);
                textRect.localPosition = new Vector3(0.064f, CurrentPrompt.DeclineText != null ? 0.075f : 0f, -0.16f);
                textRect.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

                if (Rounding)
                    RoundMenuObject(button);
            }

            if (CurrentPrompt.DeclineText != null)
            {
                GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);

                button.GetComponent<BoxCollider>().isTrigger = true;
                button.transform.parent = menu.transform;
                button.transform.rotation = Quaternion.identity;
                button.transform.localScale = new Vector3(0.09f, 0.4375f, 0.08f);
                button.transform.localPosition = new Vector3(0.56f, -0.2375f, -0.43f);

                button.GetOrAddComponent<ButtonCollider>().relatedText = "Decline Prompt";

                if (lastClickedName != "Decline Prompt")
                {
                    ColorChanger colorChanger = button.GetOrAddComponent<ColorChanger>();
                    colorChanger.colors = buttonColors[0];
                }
                Text text = new GameObject { transform = { parent = canvasObject.transform } }.GetOrAddComponent<Text>();
                text.font = currentFont;
                text.fontStyle = Utility.currentFontStyle;
                text.text = CurrentPrompt.DeclineText;
                text.fontSize = 1;
                text.alignment = TextAnchor.MiddleCenter;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 0;

                text.color = textColors[1];

                RectTransform textRect = text.GetComponent<RectTransform>();
                textRect.sizeDelta = new Vector2(0.2f, 0.03f);

                textRect.localPosition = new Vector3(0.064f, -0.075f, -0.16f);
                textRect.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

                if (Rounding)
                    RoundMenuObject(button);
            }
        }

        public class PromptData
        {
            public bool IsText;
            public string Message;

            public string AcceptText;
            public string DeclineText;

            public Action AcceptAction;
            public Action DeclineAction;
        }

        public static List<PromptData> prompts = new List<PromptData>();

        public static PromptData CurrentPrompt
        {
            get
            {
                if (prompts.Count > 0)
                    return prompts[0];
                else
                    return null;
            }
        }

        public static Material promptMaterial;
        public static void Prompt(string Message, Action Accept = null, Action Decline = null, string AcceptButton = "Yes", string DeclineButton = "No")
        {
            prompts.Add(new PromptData
            {
                Message = Message,
                AcceptAction = Accept ?? (() => { }),
                DeclineAction = Decline ?? (() => { }),
                AcceptText = AcceptButton,
                DeclineText = DeclineButton,
                IsText = false
            });

            if (menu != null && prompts.Count <= 1)
                ReloadMenu();
        }

        public static void PromptSingle(string Message, Action Accept = null, string AcceptButton = "Yes")
        {
            prompts.Add(new PromptData
            {
                Message = Message,
                AcceptAction = Accept ?? (() => { }),
                DeclineAction = null,
                AcceptText = AcceptButton,
                DeclineText = null,
                IsText = false
            });

            if (menu != null && prompts.Count <= 1)
                ReloadMenu();
        }

        public static void PromptText(string Message, Action Accept = null, Action Decline = null, string AcceptButton = "Yes", string DeclineButton = "No")
        {
            prompts.Add(new PromptData
            {
                Message = Message,
                AcceptAction = Accept ?? (() => { }),
                DeclineAction = Decline ?? (() => { }),
                AcceptText = AcceptButton,
                DeclineText = DeclineButton,
                IsText = true
            });

            if (menu != null && prompts.Count <= 1)
                ReloadMenu();
        }

        public static void PromptSingleText(string Message, Action Accept = null, string AcceptButton = "Yes")
        {
            prompts.Add(new PromptData
            {
                Message = Message,
                AcceptAction = Accept ?? (() => { }),
                DeclineAction = null,
                AcceptText = AcceptButton,
                DeclineText = null,
                IsText = true
            });

            if (menu != null && prompts.Count <= 1)
                ReloadMenu();
        }

        public static string[] InfosToStrings(ButtonInfo[] array) =>
            array.Select(button => button.buttonText).ToArray();

        public static ButtonInfo[] StringsToInfos(string[] array) =>
            array.Select(Buttons.GetIndex).ToArray();

        public static void SetupAdminPanel(string playername)
        {
            List<ButtonInfo> buttons = Buttons.buttons[GetCategory("Main")].ToList();
            buttons.Add(new ButtonInfo { buttonText = "Admin", method = () => Buttons.CurrentCategoryName = "Admin", isTogglable = false, toolTip = "Opens the admin mods." });
            Buttons.buttons[GetCategory("Main")] = buttons.ToArray();
            NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>{(playername == "NOVA" ? "OWNER" : "ADMIN")}</color><color=grey>]</color> Welcome, {playername}! Admin mods have been enabled.", 10f);
        }

        public static IEnumerator GrowCoroutine()
        {
            GameObject menuObject = menu;

            float elapsedTime = 0f;
            Vector3 target = menu.transform.localScale;
            while (elapsedTime < (slowDynamicAnimations ? 0.1f : 0.05f))
            {
                if (menuObject == null)
                    yield break;

                menuObject.transform.localScale = Vector3.Lerp(Vector3.zero, target, elapsedTime / (slowDynamicAnimations ? 0.1f : 0.05f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (menuObject == null)
                yield break;

            menuObject.transform.localScale = target;
        }

        public static IEnumerator ShrinkCoroutine()
        {
            Transform menuTransform = menu.transform;
            menu = null;

            Vector3 before = menuTransform.localScale;
            float elapsedTime = 0f;
            while (elapsedTime < (slowDynamicAnimations ? 0.1f : 0.05f))
            {
                menuTransform.localScale = Vector3.Lerp(before, Vector3.zero, elapsedTime / (slowDynamicAnimations ? 0.1f : 0.05f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            GameObject.Destroy(menuTransform.gameObject);
        }


        // Variables
        // Important
        // Objects
        public static GameObject menu;
		public static GameObject menuBackground;
		public static GameObject reference;
		public static GameObject canvasObject;

		public static SphereCollider buttonCollider;
		public static Camera TPC;
		public static Text fpsObject;

        public static string NoRichtextTags(string input, string replace = "")
        {
            Regex notags = new Regex("<.*?>", RegexOptions.IgnoreCase);
            return notags.Replace(input, replace);
        }

        // Data
        public static int pageNumber = 0;
        public static int framePressCooldown;

        public static bool lastInRoom = false;
        public static string lastRoom = "";

        public static bool disableMasterClientNotifications;
        public static bool disableRoomNotifications;
        public static bool disablePlayerNotifications;
        public static bool clearNotificationsOnDisconnect;

        public static List<string> quickActions = new List<string> { };

        public static List<string> favorites = new List<string> { "Exit Favorite" };
        public static readonly List<string> skipButtons = new List<string> { };

        public static string inputTextColor = "cyan";

        public static int _currentCategoryIndex;

        public static bool disableSearchButton;
        public static bool disableReturnButton;

        public static Vector3 MidPosition;
        public static Vector3 MidVelocity;
        private static bool midInitialized;

        public static bool SmoothGunPointer;
        public static bool smallGunPointer;
        public static bool disableGunPointer;
        public static bool disableGunLine;
        public static bool SwapGunHand;
        public static bool GriplessGuns;
        public static bool TriggerlessGuns;
        public static bool HardGunLocks;
        public static bool GunSounds;
        public static bool GunParticles;
        public static int gunVariation;
        public static int GunDirection;
        public static int lineQuality = 50;

        public static bool slowDynamicAnimations;

        public static bool incrementalButtons = true;
        public static bool incrementalBoost;

        public static bool GunSpawned;
        public static bool gunLocked;
        public static VRRig lockTarget;

        public static bool lastGunSpawned;
        public static bool lastGunTrigger;

        public static Texture2D searchIcon;
        public static Texture2D returnIcon;

        public static Material searchMat;
        public static Material returnMat;

        private static int menuOpenCount;

        public static void RoundMenuObject(GameObject toRound, float Bevel = 0.02f)
        {
            if (toRound.transform.parent != menu?.transform)
            {
                RoundObject(toRound, Bevel);
                return;
            }

            Renderer ToRoundRenderer = toRound.GetComponent<Renderer>();
            GameObject BaseA = GameObject.CreatePrimitive(PrimitiveType.Cube);
            BaseA.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            GameObject.Destroy(BaseA.GetComponent<Collider>());

            BaseA.transform.parent = menu.transform;
            BaseA.transform.rotation = Quaternion.identity;
            BaseA.transform.localPosition = toRound.transform.localPosition;
            BaseA.transform.localScale = toRound.transform.localScale + new Vector3(0f, Bevel * -2.55f, 0f);

            GameObject BaseB = GameObject.CreatePrimitive(PrimitiveType.Cube);
            BaseB.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            GameObject.Destroy(BaseB.GetComponent<Collider>());

            BaseB.transform.parent = menu.transform;
            BaseB.transform.rotation = Quaternion.identity;
            BaseB.transform.localPosition = toRound.transform.localPosition;
            BaseB.transform.localScale = toRound.transform.localScale + new Vector3(0f, 0f, -Bevel * 2f);

            GameObject RoundCornerA = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            RoundCornerA.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            GameObject.Destroy(RoundCornerA.GetComponent<Collider>());

            RoundCornerA.transform.parent = menu.transform;
            RoundCornerA.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 0f, 90f);

            RoundCornerA.transform.localPosition = toRound.transform.localPosition + new Vector3(0f, toRound.transform.localScale.y / 2f - Bevel * 1.275f, toRound.transform.localScale.z / 2f - Bevel);
            RoundCornerA.transform.localScale = new Vector3(Bevel * 2.55f, toRound.transform.localScale.x / 2f, Bevel * 2f);

            GameObject RoundCornerB = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            RoundCornerB.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            GameObject.Destroy(RoundCornerB.GetComponent<Collider>());

            RoundCornerB.transform.parent = menu.transform;
            RoundCornerB.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 0f, 90f);

            RoundCornerB.transform.localPosition = toRound.transform.localPosition + new Vector3(0f, -(toRound.transform.localScale.y / 2f) + Bevel * 1.275f, toRound.transform.localScale.z / 2f - Bevel);
            RoundCornerB.transform.localScale = new Vector3(Bevel * 2.55f, toRound.transform.localScale.x / 2f, Bevel * 2f);

            GameObject RoundCornerC = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            RoundCornerC.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            GameObject.Destroy(RoundCornerC.GetComponent<Collider>());

            RoundCornerC.transform.parent = menu.transform;
            RoundCornerC.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 0f, 90f);

            RoundCornerC.transform.localPosition = toRound.transform.localPosition + new Vector3(0f, toRound.transform.localScale.y / 2f - Bevel * 1.275f, -(toRound.transform.localScale.z / 2f) + Bevel);
            RoundCornerC.transform.localScale = new Vector3(Bevel * 2.55f, toRound.transform.localScale.x / 2f, Bevel * 2f);

            GameObject RoundCornerD = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            RoundCornerD.GetComponent<Renderer>().enabled = ToRoundRenderer.enabled;
            GameObject.Destroy(RoundCornerD.GetComponent<Collider>());

            RoundCornerD.transform.parent = menu.transform;
            RoundCornerD.transform.rotation = Quaternion.identity * Quaternion.Euler(0f, 0f, 90f);

            RoundCornerD.transform.localPosition = toRound.transform.localPosition + new Vector3(0f, -(toRound.transform.localScale.y / 2f) + Bevel * 1.275f, -(toRound.transform.localScale.z / 2f) + Bevel);
            RoundCornerD.transform.localScale = new Vector3(Bevel * 2.55f, toRound.transform.localScale.x / 2f, Bevel * 2f);

            GameObject[] ToChange = {
                BaseA,
                BaseB,
                RoundCornerA,
                RoundCornerB,
                RoundCornerC,
                RoundCornerD
            };

            foreach (GameObject Changed in ToChange)
            {
                ClampColor TargetChanger = Changed.GetOrAddComponent<ClampColor>();
                TargetChanger.targetRenderer = ToRoundRenderer;
            }

            ToRoundRenderer.enabled = false;
        }

        public static Color RandomColor(byte range = 255, byte alpha = 255) =>
            new Color32((byte)UnityEngine.Random.Range(0, range), (byte)UnityEngine.Random.Range(0, range), (byte)UnityEngine.Random.Range(0, range), alpha);

        public static Vector3 RandomVector3(float range = 1f) =>
            new Vector3(UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range), UnityEngine.Random.Range(-range, range));

        public static void RoundObject(GameObject toRound, float bevel = 0.02f)
        {
            static GameObject CreatePrimitive(PrimitiveType type, Transform parent, bool rendererEnabled)
            {
                GameObject obj = GameObject.CreatePrimitive(type);
                obj.GetComponent<Renderer>().enabled = rendererEnabled;

                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                    GameObject.Destroy(collider);

                obj.transform.SetParent(parent, false);
                return obj;
            }

            Renderer renderer = toRound.GetComponent<Renderer>();
            if (renderer == null) return;

            Transform parent = toRound.transform;
            Vector3 scale = parent.localScale;
            bool rendererEnabled = renderer.enabled;

            GameObject baseA = CreatePrimitive(PrimitiveType.Cube, parent, rendererEnabled);
            baseA.transform.localPosition = Vector3.zero;
            baseA.transform.localRotation = Quaternion.identity;
            baseA.transform.localScale = new Vector3(scale.x, scale.y - bevel * 2f, scale.z);

            GameObject baseB = CreatePrimitive(PrimitiveType.Cube, parent, rendererEnabled);
            baseB.transform.localPosition = Vector3.zero;
            baseB.transform.localRotation = Quaternion.identity;
            baseB.transform.localScale = new Vector3(scale.x, scale.y, scale.z - bevel * 2f);

            GameObject[] corners = new GameObject[4];
            Vector3[] cornerOffsets = {
                new Vector3(0f, scale.y / 2f - bevel, scale.z / 2f - bevel),
                new Vector3(0f, -scale.y / 2f + bevel, scale.z / 2f - bevel),
                new Vector3(0f, scale.y / 2f - bevel, -scale.z / 2f + bevel),
                new Vector3(0f, -scale.y / 2f + bevel, -scale.z / 2f + bevel)
            };

            for (int i = 0; i < 4; i++)
            {
                corners[i] = CreatePrimitive(PrimitiveType.Cylinder, parent, rendererEnabled);
                corners[i].transform.localPosition = cornerOffsets[i];
                corners[i].transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                corners[i].transform.localScale = new Vector3(bevel * 2f, scale.x / 2f, bevel * 2f);
            }

            GameObject[] allObjects = { baseA, baseB, corners[0], corners[1], corners[2], corners[3] };
            foreach (GameObject obj in allObjects)
            {
                ClampColor clampColor = obj.GetOrAddComponent<ClampColor>();
                clampColor.targetRenderer = renderer;
            }
            renderer.enabled = false;
        }

        private static VRRig _giveGunTarget;
        public static VRRig giveGunTarget
        {
            get
            {
                if (!GorillaParent.instance.vrrigs.Contains(_giveGunTarget))
                    _giveGunTarget = null;

                return _giveGunTarget;
            }
            set => _giveGunTarget = value;
        }

        public static int TransparentFX = LayerMask.NameToLayer("TransparentFX");
        public static int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        public static int Zone = LayerMask.NameToLayer("Zone");
        public static int GorillaTrigger = LayerMask.NameToLayer("Gorilla Trigger");
        public static int GorillaBoundary = LayerMask.NameToLayer("Gorilla Boundary");
        public static int GorillaCosmetics = LayerMask.NameToLayer("GorillaCosmetics");
        public static int GorillaParticle = LayerMask.NameToLayer("GorillaParticle");

        private static int? noInvisLayerMask;
        public static int NoInvisLayerMask()
        {
            noInvisLayerMask ??= ~(
                1 << LayerMask.NameToLayer("TransparentFX") |
                1 << LayerMask.NameToLayer("Ignore Raycast") |
                1 << LayerMask.NameToLayer("Zone") |
                1 << LayerMask.NameToLayer("Gorilla Trigger") |
                1 << LayerMask.NameToLayer("Gorilla Boundary") |
                1 << LayerMask.NameToLayer("GorillaCosmetics") |
                1 << LayerMask.NameToLayer("GorillaParticle"));

            return noInvisLayerMask ?? 131585; //GorillaLocomotion.Player.Instance.locomotionEnabledLayers;
        }


        public static GameObject Pointer;
        public static RaycastHit Ray;
        public static LineRenderer line;

        public static (RaycastHit Ray, GameObject Pointer) RenderGun()
        {
            if (Pointer == null)
            {
                Pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                Pointer.transform.localScale = smallGunPointer ? Vector3.one * 0.03f : Vector3.one * 0.1f;
                var renderer = Pointer.GetComponent<Renderer>();
                renderer.enabled = disableGunPointer ? false : true;
                renderer.material.shader = Utility.GUIShader();
                GameObject.Destroy(Pointer.GetComponent<Collider>());
            }

            Color gunColor = gunLocked || GetGunInput(true) ? buttonColors[1].GetCurrentColor() : buttonColors[0].GetCurrentColor();
            Pointer.GetComponent<Renderer>().material.color = gunColor;

            Transform gunHand = SwapGunHand ? GorillaTagger.Instance.leftHandTransform : GorillaTagger.Instance.rightHandTransform;

            Vector3 gunDirection = gunHand.forward + -gunHand.up;
            switch (GunDirection)
            {
                case 0:
                default:
                    gunDirection = gunHand.forward + -gunHand.up;
                    break;
                case 1:
                    gunDirection = -gunHand.up;
                    break;
                case 2:
                    gunDirection = gunHand.forward;
                    break;
                case 3:
                    gunDirection = gunHand.up + gunHand.forward;
                    break;
            }

            Physics.Raycast(gunHand.position, gunDirection, out Ray, float.PositiveInfinity, NoInvisLayerMask());

            Vector3 EndPosition = gunLocked ? lockTarget.headMesh.transform.position : Ray.point;
            Pointer.transform.position = EndPosition;

            if (!disableGunLine)
            {
                if (line == null)
                {
                    line = Pointer.GetOrAddComponent<LineRenderer>();
                    line.useWorldSpace = true;
                    line.material = new Material(Utility.GUIShader());
                    line.startWidth = 0.02f;
                    line.endWidth = 0.02f;
                    line.numCapVertices = 4;
                    line.numCornerVertices = 4;
                }

                Vector3 StartPosition = gunHand.position;
                Vector3 Up = -gunHand.up;
                Vector3 Right = gunHand.right;

                int Step = Mathf.Max(2, lineQuality);
                switch (gunVariation)
                {
                    case 1: // Lightning
                        line.positionCount = Step;
                        line.SetPosition(0, StartPosition);
                        for (int i = 1; i < Step - 1; i++)
                        {
                            Vector3 Position = Vector3.Lerp(StartPosition, EndPosition, i / (Step - 1f));
                            line.SetPosition(i, Position + (UnityEngine.Random.value > 0.75f
                                ? new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f))
                                : Vector3.zero));
                        }
                        line.SetPosition(Step - 1, EndPosition);
                        break;
                    case 2: // Wavy
                        line.positionCount = Step;
                        line.SetPosition(0, StartPosition);
                        for (int i = 1; i < Step - 1; i++)
                        {
                            float value = i / (float)Step * 50f;
                            Vector3 Position = Vector3.Lerp(StartPosition, EndPosition, i / (Step - 1f));
                            line.SetPosition(i, Position + Up * (Mathf.Sin(Time.time * -10f + value) * 0.1f));
                        }
                        line.SetPosition(Step - 1, EndPosition);
                        break;
                    case 3: // Blocky
                        line.positionCount = Step;
                        line.SetPosition(0, StartPosition);
                        for (int i = 1; i < Step - 1; i++)
                        {
                            Vector3 Position = Vector3.Lerp(StartPosition, EndPosition, i / (Step - 1f));
                            line.SetPosition(i, new Vector3(Mathf.Round(Position.x * 25f) / 25f, Mathf.Round(Position.y * 25f) / 25f, Mathf.Round(Position.z * 25f) / 25f));
                        }
                        line.SetPosition(Step - 1, EndPosition);
                        break;
                    case 4: // Sinewave
                        Step = Mathf.Max(2, lineQuality / 2);
                        line.positionCount = Step;
                        line.SetPosition(0, StartPosition);
                        for (int i = 1; i < Step - 1; i++)
                        {
                            Vector3 Position = Vector3.Lerp(StartPosition, EndPosition, i / (Step - 1f));
                            line.SetPosition(i, Position + Up * (Mathf.Sin(Time.time * 10f) * (i % 2 == 0 ? 0.1f : -0.1f)));
                        }
                        line.SetPosition(Step - 1, EndPosition);
                        break;
                    case 5: // Spring
                        line.positionCount = Step;
                        line.SetPosition(0, StartPosition);
                        for (int i = 1; i < Step - 1; i++)
                        {
                            float value = i / (float)Step * 50f;
                            Vector3 Position = Vector3.Lerp(StartPosition, EndPosition, i / (Step - 1f));
                            line.SetPosition(i, Position + Right * (Mathf.Cos(Time.time * -10f + value) * 0.1f) + Up * (Mathf.Sin(Time.time * -10f + value) * 0.1f));
                        }
                        line.SetPosition(Step - 1, EndPosition);
                        break;
                    case 6: // Bouncy
                        line.positionCount = Step;
                        line.SetPosition(0, StartPosition);
                        for (int i = 1; i < Step - 1; i++)
                        {
                            float value = i / (float)Step * 15f;
                            line.SetPosition(i, Vector3.Lerp(StartPosition, EndPosition, i / (Step - 1f)) + Up * (Mathf.Abs(Mathf.Sin(Time.time * -10f + value)) * 0.3f));
                        }
                        line.SetPosition(Step - 1, EndPosition);
                        break;
                    case 7: // Bezier
                        {
                            Step = Mathf.Max(8, lineQuality);
                            Vector3 baseMid = Vector3.Lerp(StartPosition, EndPosition, 0.5f);
                            float angle = Time.time * 3f;
                            Vector3 wobbleOffset =  Up * (Mathf.Sin(angle) * 0.15f) +  Right * (Mathf.Cos(angle * 1.3f) * 0.15f);
                            Vector3 targetMid = baseMid + wobbleOffset;
                            if (!midInitialized)
                            {
                                MidPosition = targetMid;
                                MidVelocity = Vector3.zero;
                                midInitialized = true;
                            }
                            float dt = Mathf.Min(Time.deltaTime, 0.033f);
                            MidVelocity += (targetMid - MidPosition) * 40f * dt;
                            MidVelocity *= 0.90f;
                            MidPosition += MidVelocity * dt;
                            line.positionCount = Step;
                            Vector3[] points = new Vector3[Step];
                            for (int i = 0; i < Step; i++)
                            {
                                float t = i / (float)(Step - 1);
                                float omt = 1f - t;
                                points[i] = (omt * omt) * StartPosition + (2f * omt * t) * MidPosition + (t * t) * EndPosition;
                            }
                            line.SetPositions(points);
                            break;
                        }
                    default:
                        line.positionCount = 2;
                        line.SetPosition(0, StartPosition);
                        line.SetPosition(1, EndPosition);
                        break;
                }

                line.startColor = gunColor;
                line.endColor = gunColor;
            }
            else if (line != null)
            {
                line.positionCount = 0;
            }
            return (Ray, Pointer);
        }

        public static void DestroyPointer()
        {
            if (SwapGunHand)
            {
                if (!Utility.LeftGrip)
                {
                    if (Pointer != null)
                    {
                        GameObject.Destroy(Pointer);
                        Pointer = null;
                    }
                }
            }
            else
            {
                if (!Utility.RightGrip)
                {
                    if (Pointer != null)
                    {
                        GameObject.Destroy(Pointer);
                        Pointer = null;
                    }
                }
            }
        }
        public static bool GetGunInput(bool isShooting)
        {
            if (giveGunTarget != null)
            {
                if (isShooting)
                    return TriggerlessGuns || (SwapGunHand ? giveGunTarget.leftIndex.calcT > 0.5f : giveGunTarget.rightIndex.calcT > 0.5f);
                else
                    return GriplessGuns || (SwapGunHand ? giveGunTarget.leftMiddle.calcT > 0.5f : giveGunTarget.rightMiddle.calcT > 0.5f);
            }

            if (isShooting)
                return TriggerlessGuns || (SwapGunHand ? Utility.LeftTrigger : Utility.RightTrigger) || Mouse.current.leftButton.isPressed;
            else
                return GriplessGuns || (SwapGunHand ? Utility.LeftGrip : Utility.RightGrip) || (HardGunLocks && gunLocked && !Utility.RightSecondary) || Mouse.current.rightButton.isPressed;
        }

        public static Vector3 GetGunDirection(Transform transform) =>
            new[] { transform.forward, -transform.up, 
                transform == GorillaTagger.Instance.rightHandTransform ? GorillaTagger.Instance.rightHandTransform.forward : GorillaTagger.Instance.leftHandTransform.forward, 
                GorillaTagger.Instance.headCollider.transform.forward }[GunDirection];
    }
}