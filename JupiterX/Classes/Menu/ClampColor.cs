// ============================================================
//  JupiterX | Classes/Menu/ClampColor.cs
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
using UnityEngine;

namespace JupiterX.Classes.Menu
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public class ClampColor : MonoBehaviour
    {
        public ClampColor(IntPtr e) : base(e) { }
        public void Start()
        {
            targetRenderer.gameObject.GetComponent<ColorChanger>()?.Start();

            gameObjectRenderer = GetComponent<Renderer>();
            Update();
        }

        public void Update()
        {
            if (gameObjectRenderer.material.shader != targetRenderer.material.shader)
                gameObjectRenderer.material = new Material(targetRenderer.material.shader);

            if (targetRenderer.material.mainTexture != null && gameObjectRenderer.material.mainTexture != targetRenderer.material.mainTexture)
                gameObjectRenderer.material.mainTexture = targetRenderer.material.mainTexture;

            gameObjectRenderer.material.color = targetRenderer.material.color;
        }

        public Renderer gameObjectRenderer;
        public Renderer targetRenderer;
    }
}