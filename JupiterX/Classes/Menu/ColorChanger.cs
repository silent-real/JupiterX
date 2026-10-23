// ============================================================
//  JupiterX | Classes/Menu/ColorChanger.cs
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
    public class ColorChanger : MonoBehaviour
    {
        public ColorChanger(IntPtr e) : base(e) { }
        public void Start()
        {
            if (colors == null)
            {
                Destroy(this);
                return;
            }

            targetRenderer = GetComponent<Renderer>();

            if (colors.IsFlat())
            {
                Update();
                Destroy(this);
                return;
            }

            Update();
        }

        public void Update()
        {
            targetRenderer.enabled = overrideTransparency ?? !colors.transparent;

            if (colors.transparent)
                return;

            targetRenderer.material.color = colors.GetCurrentColor();

            Color color = targetRenderer.material.color;
            color.a = 0.5f;
            targetRenderer.material.color = color;
        }

        public Renderer targetRenderer;
        public ExtGradient colors;
        public bool? overrideTransparency;
    }
}