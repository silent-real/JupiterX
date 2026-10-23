// ============================================================
//  JupiterX | Classes/Menu/TimedBehaviour.cs
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
    public class TimedBehaviour : MonoBehaviour
    {
        public TimedBehaviour(IntPtr ptr) : base(ptr) { }
        public void Start()
        {
            startTime = Time.time;
        }

        public void Update()
        {
            if (!complete)
            {
                progress = Mathf.Clamp((Time.time - startTime) / duration, 0f, 1f);
                if (Time.time - startTime > duration)
                {
                    if (loop)
                    {
                        OnLoop();
                    }
                    else
                    {
                        complete = true;
                    }
                }
            }
        }

        public void OnLoop()
        {
            startTime = Time.time;
        }

        public bool complete = false;

        public bool loop = true;

        public float progress = 0f;

        protected bool paused = false;

        protected float startTime;

        protected float duration = 2f;
    }
}