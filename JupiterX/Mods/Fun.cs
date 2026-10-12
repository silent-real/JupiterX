// ============================================================
//  JupiterX | Mods/Fun.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using JupiterX.Classes.Mods;
using JupiterX.Managers;
using JupiterX.Menu;
using JupiterX.Notifications;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.Unity.UtilityScripts;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JupiterX.Mods
{
    public class Fun
    {
        private static float headSpinSpeed = 10f;
        public static void SpinHead(string axis)
        {
            if (Utility.ActualRig().enabled)
            {
                Vector3 rot = Utility.ActualRig().head.trackingRotationOffset;
                switch (axis.ToLower())
                {
                    case "x":
                        rot.x += headSpinSpeed;
                        break;
                    case "y":
                        rot.y += headSpinSpeed;
                        break;
                    case "z":
                        rot.z += headSpinSpeed;
                        break;
                    default:
                        return;
                }
                Utility.ActualRig().head.trackingRotationOffset = rot;
            }
            else
            {
                switch (axis.ToLower())
                {
                    case "x":
                        Utility.ActualRig().head.rigTarget.transform.rotation = Quaternion.Euler(Utility.ActualRig().head.rigTarget.transform.rotation.eulerAngles + new Vector3(headSpinSpeed, 0f, 0f));
                        break;
                    case "y":
                        Utility.ActualRig().head.rigTarget.transform.rotation = Quaternion.Euler(Utility.ActualRig().head.rigTarget.transform.rotation.eulerAngles + new Vector3(0f, headSpinSpeed, 0f));
                        break;
                    case "z":
                        Utility.ActualRig().head.rigTarget.transform.rotation = Quaternion.Euler(Utility.ActualRig().head.rigTarget.transform.rotation.eulerAngles + new Vector3(0f, 0f, headSpinSpeed));
                        break;
                    default:
                        return;
                }
            }
        }

        public static void FixHead()
        {
            Vector3 rot = Utility.ActualRig().head.trackingRotationOffset;
            rot = Vector3.zero;
            Utility.ActualRig().head.trackingRotationOffset = rot;
        }

        public static void UpsideDownHead()
        {
            Vector3 rot = Utility.ActualRig().head.trackingRotationOffset;
            rot.z = 180f;
            Utility.ActualRig().head.trackingRotationOffset = rot;
        }

        public static void BrokenNeck()
        {
            Vector3 rot = Utility.ActualRig().head.trackingRotationOffset;
            rot.z = 90f;
            Utility.ActualRig().head.trackingRotationOffset = rot;
        }

        public static void BackwardsHead()
        {
            Vector3 rot = Utility.ActualRig().head.trackingRotationOffset;
            rot.y = 180f;
            Utility.ActualRig().head.trackingRotationOffset = rot;
        }

        public static void SidewaysHead()
        {
            Vector3 rot = Utility.ActualRig().head.trackingRotationOffset;
            rot.y = 90f;
            Utility.ActualRig().head.trackingRotationOffset = rot;
        }

        public static float lastBangTime;
        public static readonly float BPM = 159f;
        public static void HeadBang()
        {
            Vector3 rot = Utility.ActualRig().head.trackingRotationOffset;
            if (Time.time > lastBangTime)
            {
                rot.x = 50f;
                lastBangTime = Time.time + 60f / BPM;
            }
            else
                rot.x = Mathf.Lerp(rot.x, 0f, 0.1f);
            Utility.ActualRig().head.trackingRotationOffset = rot;
        }

        public static void CubeAll()
        {
            if (Utility.RightTrigger)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        PhotonNetwork.Instantiate("bulletPrefab", rig.transform.position + new Vector3(0, 0, 0.6f), rig.headConstraint.transform.rotation);
                    }
                }
            }
        }

        public static void FixMic()
        {
            Recorder rec = GameObject.FindObjectsOfType<Recorder>().FirstOrDefault();
            if (rec == null)
            {
                NotificationManager.SendNotification("<color=yellow>[NULL]</color> 'rec' is null how the fuck did this happen 😭");
                return;
            }

            rec.Bitrate = 20000;
            rec.SourceType = Recorder.InputSourceType.Microphone;
            rec.SourceType = Recorder.InputSourceType.Microphone;
            rec.GetOrAddComponent<MicAmplifier>().AmplificationFactor = 1;
            rec.GetOrAddComponent<MicAmplifier>().BoostValue = 0;
        }
        public static void LowQualityMic()
        {
            Recorder rec = GameObject.FindObjectsOfType<Recorder>().FirstOrDefault();
            if (rec == null)
            {
                NotificationManager.SendNotification("<color=yellow>[NULL]</color> 'rec' is null how the fuck did this happen 😭");
                return;
            }

            rec.SourceType = Recorder.InputSourceType.Microphone;
            rec.Bitrate = 12000;
            rec.VoiceDetection = true;
            rec.VoiceDetectionThreshold = 0.02f;
            rec.GetOrAddComponent<MicAmplifier>().AmplificationFactor = 1f;
            rec.GetOrAddComponent<MicAmplifier>().BoostValue = 0f;
        }

        public static void HighQualityMic()
        {
            Recorder rec = GameObject.FindObjectsOfType<Recorder>().FirstOrDefault();
            if (rec == null)
            {
                NotificationManager.SendNotification("<color=yellow>[NULL]</color> 'rec' is null how the fuck did this happen 😭");
                return;
            }

            rec.SourceType = Recorder.InputSourceType.Microphone;
            rec.Bitrate = 32000;
            rec.VoiceDetection = false;
            rec.GetOrAddComponent<MicAmplifier>().AmplificationFactor = 2f;
            rec.GetOrAddComponent<MicAmplifier>().BoostValue = 10f;
        }

        public static void BassBoostMic()
        {
            Recorder rec = GameObject.FindObjectsOfType<Recorder>().FirstOrDefault();
            if (rec == null)
            {
                NotificationManager.SendNotification("<color=yellow>[NULL]</color> 'rec' is null how the fuck did this happen 😭");
                return;
            }

            rec.SourceType = Recorder.InputSourceType.Microphone;
            rec.Bitrate = 18000;
            rec.VoiceDetection = false;
            rec.GetOrAddComponent<MicAmplifier>().AmplificationFactor = 2.5f;
            rec.GetOrAddComponent<MicAmplifier>().BoostValue = 15f;
        }
        public static void BassBoostMicExtreme()
        {
            Recorder rec = GameObject.FindObjectsOfType<Recorder>().FirstOrDefault();
            if (rec == null)
            {
                NotificationManager.SendNotification("<color=yellow>[NULL]</color> 'rec' is null how the fuck did this happen 😭");
                return;
            }

            rec.SourceType = Recorder.InputSourceType.Microphone;
            rec.Bitrate = 16000;
            rec.VoiceDetection = false;
            rec.GetOrAddComponent<MicAmplifier>().AmplificationFactor = 3f;
            rec.GetOrAddComponent<MicAmplifier>().BoostValue = 25f;
        }

        public static void GetIdSelf()
        {
            NotificationManager.SendNotification($"<color=cyan>[INFO]</color> Your userid is {Utility.MyPlayer().UserId}");
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "JupiterX/Ids")))
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "JupiterX/Ids"));
            else if (!File.Exists(Path.Combine(Application.persistentDataPath, "JupiterX/Ids/SelfId.txt")))
                File.WriteAllText(Path.Combine(Application.persistentDataPath, "JupiterX/Ids/SelfId.txt"), Utility.MyPlayer().UserId);
            else
                File.WriteAllText(Path.Combine(Application.persistentDataPath, "JupiterX/Ids/SelfId.txt"), Utility.MyPlayer().UserId);
        }

        public static void GetIdGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        NotificationManager.SendNotification($"<color=cyan>[INFO]</color> There userid is {who.photonView.Owner.UserId}");
                        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "JupiterX/Ids")))
                            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "JupiterX/Ids"));
                        else if (!File.Exists(Path.Combine(Application.persistentDataPath, $"JupiterX/Ids/{who.photonView.Owner.NickName}_UserId.txt")))
                            File.WriteAllText(Path.Combine(Application.persistentDataPath, $"JupiterX/Ids/{who.photonView.Owner.NickName}_UserId.txt"), who.photonView.Owner.UserId);
                        else
                            File.WriteAllText(Path.Combine(Application.persistentDataPath, $"JupiterX/Ids/{who.photonView.Owner.NickName}_UserId.txt"), who.photonView.Owner.UserId);
                    }
                }
            }
        }

        public static void GetIdAll()
        {
            if (!PhotonNetwork.InRoom)
            {
                NotificationManager.SendNotification("<color=red>[ERROR]</color> Are you in a room ?");
                return;
            }
            foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
            {
                NotificationManager.SendNotification($"<color=cyan>[INFO]</color> There userid is {plr.UserId}");
                if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "JupiterX/Ids")))
                    Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "JupiterX/Ids"));
                else if (!File.Exists(Path.Combine(Application.persistentDataPath, $"JupiterX/Ids/{plr.NickName}_UserId.txt")))
                    File.WriteAllText(Path.Combine(Application.persistentDataPath, $"JupiterX/Ids/{plr.NickName}_UserId.txt"), plr.UserId);
                else
                    File.WriteAllText(Path.Combine(Application.persistentDataPath, $"JupiterX/Ids/{plr.NickName}_UserId.txt"), plr.UserId);
            }
        }

        public static void GrabRoomInfo()
        {
            if (!PhotonNetwork.InRoom)
            {
                NotificationManager.SendNotification("<color=red>[ERROR]</color> Are you in a room ?");
                return;
            }
            foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"---------------{PhotonNetwork.CurrentRoom.Name}---------------");
                stringBuilder.AppendLine($"NickName: {plr.NickName}, UserId: {plr.UserId}, Cosmetics: {RigManager.GetVRRigFromPlayer(plr).concatStringOfCosmeticsAllowed}");
                stringBuilder.Append("\nRoom Info Pulled By JupiterX");
                string payload = stringBuilder.ToString();
                NotificationManager.SendNotification($"<color=cyan>[INFO]</color> Grabbed player info for room {PhotonNetwork.CurrentRoom.Name}");
                if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "JupiterX/RoomInfo")))
                    Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "JupiterX/RoomInfo"));
                else if (!File.Exists(Path.Combine(Application.persistentDataPath, $"JupiterX/RoomInfo/{PhotonNetwork.CurrentRoom.Name}_Info.txt")))
                    File.WriteAllText(Path.Combine(Application.persistentDataPath, $"JupiterX/RoomInfo/{PhotonNetwork.CurrentRoom.Name}_Info.txt"), payload);
                else
                    File.WriteAllText(Path.Combine(Application.persistentDataPath, $"JupiterX/RoomInfo/{PhotonNetwork.CurrentRoom.Name}_Info.txt"), payload);
            }
        }

        public static void FixHandTaps() =>
            GorillaTagger.Instance.handTapVolume = 0.1f;
        public static void LoadHandTaps() =>
            GorillaTagger.Instance.handTapVolume = float.MaxValue;
        public static void SilentHandTaps() =>
            GorillaTagger.Instance.handTapVolume = 0f;
        public static void NoTapCooldown() =>
            GorillaTagger.Instance.tapCoolDown = 0f;
        public static void ResetTapcooldown() =>
            GorillaTagger.Instance.tapCoolDown = 0.33f;


        private static bool autoclickstate;
        public static void AutoClicker()
        {
            autoclickstate = !autoclickstate;
            if (Utility.LeftTriggerFloat > 0.5f)
            {
                Utility.ActualRig().leftHand.calcT = autoclickstate ? 1f : 0f;
                Utility.ActualRig().leftHand.MapMyFinger(autoclickstate ? 1f : 0f);
            }
            if (Utility.RightTriggerFloat > 0.5f)
            {
                Utility.ActualRig().rightHand.calcT = autoclickstate ? 1f : 0f;
                Utility.ActualRig().rightHand.MapMyFinger(autoclickstate ? 1f : 0f);
            }
        }

        public static void MuteGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig)
                    {
                        rig.muted = true;
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                    }
                }
            }
        }

        public static void UnMuteGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig)
                    {
                        rig.muted = false;
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).PressButton(false, GorillaPlayerLineButton.ButtonType.Mute);
                    }
                }
            }
        }

        public static void MuteAll()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        rig.muted = true;
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                    }
                }
            }
        }

        public static void UnMuteAll()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        rig.muted = false;
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).PressButton(false, GorillaPlayerLineButton.ButtonType.Mute);
                    }
                }
            }
        }

        public static void ReportGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).PressButton(true, GorillaPlayerLineButton.ButtonType.Report);
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).PressButton(true, GorillaPlayerLineButton.ButtonType.Cheating);
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).ReportPlayer(rig.photonView.Owner.UserId, GorillaPlayerLineButton.ButtonType.Cheating, rig.photonView.Owner.NickName);
                    }
                }
            }
        }

        public static void ReportAll()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).PressButton(true, GorillaPlayerLineButton.ButtonType.Report);
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).PressButton(true, GorillaPlayerLineButton.ButtonType.Cheating);
                        GameObject.FindObjectsOfType<GorillaPlayerScoreboardLine>().FirstOrDefault<GorillaPlayerScoreboardLine>(line => line.linePlayer.UserId == rig.photonView.Owner.UserId).ReportPlayer(rig.photonView.Owner.UserId, GorillaPlayerLineButton.ButtonType.Cheating, rig.photonView.Owner.NickName);
                    }
                }
            }
        }

        public static void ChangeDoorState(GTDoor.DoorState state) =>
            RPCManager.GTDoorRPC("ChangeDoorState", RpcTarget.AllViaServer, new object[] { state });
    }
}