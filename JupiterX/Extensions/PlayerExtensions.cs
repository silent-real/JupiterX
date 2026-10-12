// ============================================================
//  JupiterX | Extensions/PlayerExtensions.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using Photon.Realtime;

namespace JupiterX.Extensions
{
    public static class PlayerExtensions
    {
        public static VRRig Rig(this Photon.Realtime.Player plr)
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                {
                    if (rig.photonView.Owner == plr)
                    {
                        return rig;
                    }
                }
            }
            return null;
        }
    }
}