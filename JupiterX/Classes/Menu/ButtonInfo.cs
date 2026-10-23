// ============================================================
//  JupiterX | Classes/Menu/ButtonInfo.cs
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

namespace JupiterX.Classes.Menu
{
    public class ButtonInfo
    {
        public string buttonText = "-";
        public string overlapText;

        public string toolTip = ".";

        public Action method;
        public Action enableMethod;
        public Action disableMethod;

        public bool enabled;
        public bool isTogglable = true;

        public bool label = false;
        public bool incremental;

        public bool hideFromArraylist;
        public bool dontPanic = false;
    }
}