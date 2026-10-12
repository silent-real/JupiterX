// ============================================================
//  JupiterX | Mods/Name.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using JupiterX.Managers;
using Photon.Pun;
using System.IO;

namespace JupiterX.Mods
{
    public class Name
    {
        public static void MenuNameTag() =>
            PhotonNetwork.LocalPlayer.NickName = "<color=cyan>JupiterX V2</color> <color=grey>By</color> <color=magenta>Nova</color>\nhttps://discord.gg/dtQdz59FJG";

        public static void ChangeNameSpaz(string name, string[] colors)
        {
            int random = UnityEngine.Random.Range(0, colors.Length);
            PhotonNetwork.LocalPlayer.NickName = $"<color={colors[random]}>{name}</color>";
        }

        public static void ChangeName(string name, string color) =>
            PhotonNetwork.LocalPlayer.NickName = "<color=" + color + ">" + name + "</color>";

        public static void CustomName()
        {
            string filePath = Path.Combine(UnityEngine.Application.persistentDataPath, "JupiterX/CustomLocalName.txt");
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "your name here");
            }
            else
            {
                PhotonNetwork.LocalPlayer.NickName = File.ReadAllText(filePath);
            }
        }

        public static void CustomInputName()
        {
            KeyboardManager.Input("Enter a custom name", name =>
            {
                ChangeName(name, "white");
            });
        }
    }
}