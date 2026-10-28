// ============================================================
//  JupiterX | Mods/Advantage.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using JupiterX.Notifications;
using Photon.Pun;
using UnityEngine;
using static JupiterX.Menu.Main;

namespace JupiterX.Mods
{
    public class Advantage
    {
        public static void TagAll()
        {
            if (Utility.IsMaster())
            {
                foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                {
                    foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
                    {
                        tagman.AddInfectedPlayer(plr);
                    }
                }
            }
            else
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        if (Utility.ActualRig().mainSkin.material.name.Contains("fected") && !rig.mainSkin.material.name.Contains("fected"))
                        {
                            Utility.ActualRig().enabled = false;
                            Utility.ActualRig().transform.position = rig.headConstraint.transform.position;
                            Utility.ActualRig().rightHandTransform.transform.position = rig.headConstraint.transform.position;
                            Utility.RightHandTransform().position = rig.headConstraint.transform.position;
                            NotificationManager.SendNotification("<color=grey>[</color><color=cyan>INFO</color><color=grey>]</color> Tagged all!", 7f);
                        }
                        else
                        {
                            Utility.ActualRig().enabled = true;
                            NotificationManager.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> You are not tagged.", 10f);
                        }
                    }
                }
            }
        }

        public static void TagAura()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != Utility.myVRRig())
                {
                    if (Utility.ActualRig().mainSkin.material.name.Contains("fected") && !rig.mainSkin.material.name.Contains("fected"))
                    {
                        float dis = Vector3.Distance(Utility.MainTransform().position, rig.headConstraint.transform.position);
                        if (dis < 0.75f)
                        {
                            Utility.ActualRig().rightHandTransform.transform.position = rig.headConstraint.transform.position;
                            Utility.RightHandTransform().position = rig.headConstraint.transform.position;
                        }
                    }
                }
            }
        }

        public static void TagGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who != null && who != Utility.myVRRig())
                    {
                        GorillaGameManager.instance.GetComponent<PhotonView>().RPC("ReportTagRPC", RpcTarget.MasterClient, 
                            new Il2CppSystem.Object[] { who.photonView.Owner });
                    }
                }
            }
        }

        public static void TagGunRPC()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    Utility.MakeMeMaster();
                    foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                    {
                        tagman.AddInfectedPlayer(lockTarget.photonView.Owner);
                    }
                }

                if (GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who != null && who != Utility.myVRRig())
                    {
                        gunLocked = true;
                        lockTarget = who;
                    }
                }
            }
            else
            {
                lockTarget = null;
                if (gunLocked)
                    gunLocked = false;
            }
        }

        public static void FlickTagGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.Pointer;
                if (GetGunInput(true))
                {
                    Utility.RightHandTransform().position = NewPointer.transform.position;
                }
            }
        }
    }
}