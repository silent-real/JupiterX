// ============================================================
//  JupiterX | Mods/Overpowered.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using ExitGames.Client.Photon;
using GorillaNetworking;
using JupiterX.Classes.Mods;
using JupiterX.Managers;
using JupiterX.Menu;
using JupiterX.Notifications;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;

namespace JupiterX.Mods
{
    public class Overpowered
    {
        public static void RigSpam()
        {
            if (PhotonNetwork.InRoom)
            {
                Utility.MakeMeMaster();
                if (Utility.RightTrigger)
                {
                    GameObject.Destroy(Utility.myVRRig().gameObject);
                }
            }
        }

        public static void AlawysMaster()
        {
            if (PhotonNetwork.InRoom)
            {
                Utility.MakeMeMaster();
            }
        }

        public static void LongNamePub() =>
            CreatePublic(" JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX  JUPITERX ");
        
        public static void CreatePublic(string roomName)
        {
            Hashtable customProps = new Hashtable();
            customProps.Add("gameMode", GorillaComputer.instance.currentQueue + GorillaComputer.instance.currentGameMode);
            RoomOptions roomOptions = new RoomOptions()
            {
                SuppressPlayerInfo = false,
                MaxPlayers = 10,
                PublishUserId = false,
                CustomRoomProperties = customProps
            };
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }

        public static void CreatePublic(byte playercount)
        {
            Hashtable customProps = new Hashtable();
            customProps.Add("gameMode", GorillaComputer.instance.currentQueue + GorillaComputer.instance.currentGameMode);
            RoomOptions roomOptions = new RoomOptions()
            {
                SuppressPlayerInfo = false,
                MaxPlayers = playercount,
                PublishUserId = false,
                CustomRoomProperties = customProps
            };
            PhotonNetwork.CreateRoom(new string(Enumerable.Range(0, 4).Select(_ => "ABCDEFGHIJKLMNOPQRSTUVWXYZ"[UnityEngine.Random.Range(0, 26)])
                .ToArray()), roomOptions);
        }

        public static void MatSpamAll()
        {
            Utility.MakeMeMaster();
            foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
            {
                foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                {
                    if (tagman.currentInfected.Contains(plr))
                    {
                        tagman.currentInfected.Remove(plr);
                        tagman.EndInfectionGame();
                    }
                    else
                    {
                        tagman.AddInfectedPlayer(plr);
                        tagman.UpdateInfectionState();
                    }
                    tagman.UpdateInfectionState();
                }
            }
        }

        public static void MatSpamGun()
        {
            Utility.MakeMeMaster();
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.gunLocked && Main.lockTarget != null)
                {
                    foreach (GorillaTagManager tagman in GameObject.FindObjectsOfType<GorillaTagManager>())
                    {
                        if (tagman.currentInfected.Contains(Main.lockTarget.photonView.Owner))
                        {
                            tagman.currentInfected.Remove(Main.lockTarget.photonView.Owner);
                            tagman.EndInfectionGame();
                        }
                        else
                        {
                            tagman.AddInfectedPlayer(Main.lockTarget.photonView.Owner);
                            tagman.UpdateInfectionState();
                        }
                        tagman.UpdateInfectionState();
                    }
                }

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        Main.gunLocked = true;
                        Main.lockTarget = who;
                    }
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }

        public static void SetMasterGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;


                if (Main.gunLocked && Main.lockTarget != null)
                {
                    if (PhotonNetwork.MasterClient != Main.lockTarget.photonView.Owner)
                        Utility.SetMaster(Main.lockTarget.photonView.Owner);
                }

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        Main.gunLocked = true;
                        Main.lockTarget = who;
                    }
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }

        public static void DisableNetworkTriggersSS()
        {
            Utility.SetMaster(PhotonNetwork.LocalPlayer);
            Hashtable hash = new Hashtable();
            hash.Add("gameMode", "forestcavescanyoncitymountainsMODDEDMODDEDINFECTIONINFECTION");
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        public static void EnableNetworkTriggersSS()
        {
            Utility.SetMaster(PhotonNetwork.LocalPlayer);
            Hashtable hash = new Hashtable();
            hash.Add("gameMode", "forestDEFUALTDEFAULTINFECTIONINFECTION");
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        public static void KickAll()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
                {
                    GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Add(plr.UserId);
                    if (GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(plr.UserId))
                    {
                        for (int i = 0; i < 25; i++)
                        {
                            GorillaGameManager.instance.photonView.RPC("JoinPubWithFreinds", plr, null);
                        }
                    }
                }
            }
        }

        public static void LagOnYouTouch()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        if (Vector3.Distance(rig.headMesh.transform.position, Utility.myVRRig().rightHandTransform.position) <= 0.50f || Vector3.Distance(rig.headMesh.transform.position, Utility.myVRRig().leftHandTransform.position) <= 0.50f)
                        {
                            for (int i = 0; i < 50; i++)
                            {
                                PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                                PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                            }
                            PhotonNetwork.SendAllOutgoingCommands();
                        }
                    }
                }
            }
        }

        public static void CrashOnYouTouch()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        if (Vector3.Distance(rig.headMesh.transform.position, Utility.myVRRig().rightHandTransform.position) <= 0.50f || Vector3.Distance(rig.headMesh.transform.position, Utility.myVRRig().leftHandTransform.position) <= 0.50f)
                        {
                            for (int i = 0; i < 150; i++)
                            {
                                PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                                PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { TargetActors = new int[] { rig.photonView.Owner.ActorNumber } }, SendOptions.SendUnreliable);
                            }
                            PhotonNetwork.SendAllOutgoingCommands();
                        }
                    }
                }
            }
        }

        public static void CrashGunV2()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.gunLocked && Main.lockTarget != null)
                {
                    for (int i = 0; i < 25; i++)
                    {
                        Utility.myVRRig().photonView.RPC("RequestCosmetics", RpcTarget.Others, null);
                        Utility.myVRRig().photonView.RPC("UpdatePlayerCosmetic", RpcTarget.Others, null);
                        Utility.BetaCrashPlayer(Main.lockTarget.photonView.Owner);
                    }
                }

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        Main.gunLocked = true;
                        Main.lockTarget = who;
                    }
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }

        public static void KickGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.gunLocked && Main.lockTarget != null)
                {
                    GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Add(Main.lockTarget.photonView.Owner.UserId);
                    PhotonView photonView = GorillaGameManager.instance.photonView;
                    if (Main.lockTarget != null && GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(Main.lockTarget.photonView.Owner.UserId))
                    {
                        for (int i = 0; i < 25; i++)
                        {
                            photonView.RPC("JoinPubWithFreinds", Main.lockTarget.photonView.Owner, null);
                        }
                    }
                }

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        Main.gunLocked = true;
                        Main.lockTarget = who;
                    }
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }


        public static void BanGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who && who != Utility.myVRRig())
                    {
                        Utility.BetaBanAll(who.photonView.Owner.UserId);
                    }
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }
        public static void SlowAll()
        {
            Utility.SetMaster(Utility.MyPlayer());
            Utility.myVRRig().photonView.RPC("SetTaggedTime", RpcTarget.Others, null);
        }

        public static void SlowGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;


                if (Main.gunLocked && Main.lockTarget != null)
                {
                    Utility.SetMaster(Utility.MyPlayer());
                    Utility.myVRRig().photonView.RPC("SetTaggedTime", Main.lockTarget.photonView.Owner, null);
                }

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        Main.gunLocked = true;
                        Main.lockTarget = who;
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }

        public static void CrashAllV2()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && rig != Utility.myVRRig())
                    {
                        if (Utility.RightTrigger)
                        {
                            Utility.BetaCrashAllV2(rig);
                            Utility.BetaCrashAllV2(rig);
                            Utility.BetaCrashAllV2(rig);
                            Utility.BetaCrashAllV2(rig);
                            Utility.BetaCrashAllV2(rig);
                            PhotonNetwork.SendAllOutgoingCommands();
                        }
                    }
                }
            }
        }

        public static void CrashAllV3()
        {
            if (PhotonNetwork.InRoom)
            {
                if (Utility.RightTrigger)
                {
                    Hashtable hash = new Hashtable(1);
                    hash.Add(0, BoxManager.BoxAny(float.NaN));
                    PhotonNetwork.NetworkingClient.OpRaiseEvent(207, hash, null, ExitGames.Client.Photon.SendOptions.SendUnreliable);
                    PhotonNetwork.NetworkingClient.OpRaiseEvent((byte)UnityEngine.Random.Range(200, 212), hash, null, ExitGames.Client.Photon.SendOptions.SendUnreliable);
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
        }

        public static void CrashAllV4()
        {
            if (PhotonNetwork.InRoom)
            {
                if (Utility.RightTrigger)
                {
                    for (int i = 0; i < 150; i++)
                    {
                        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
        }

        public static void CrashAllV6()
        {
            if (PhotonNetwork.InRoom)
            {
                if (Utility.RightTrigger)
                {
                    for (int i = 0; i < 700; i++)
                    {
                        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                    }
                    for (int j = 0; j < 700; j++)
                    {
                        PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                    }
                }
            }
        }

        public static void CrashAllV5()
        {
            if (PhotonNetwork.InRoom)
            {
                if (Utility.RightTrigger)
                {
                    foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
                    {
                        PhotonNetwork.DestroyPlayerObjects(plr);
                        PhotonNetwork.SendDestroyOfPlayer(plr.ActorNumber);
                        PhotonNetwork.OpRemoveCompleteCacheOfPlayer(plr.ActorNumber);
                    }
                    for (int i = 0; i < 150; i++)
                    {
                        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent((byte)UnityEngine.Random.Range(200, 212), null, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendUnreliable);
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
        }

        public static void CrashGunV5()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                Photon.Realtime.Player plr = Ray.collider.GetComponentInParent<PhotonView>().Owner;

                if (Main.GetGunInput(true))
                {
                    for (int i = 0; i < 150; i++)
                    {
                        PhotonNetwork.DestroyPlayerObjects(plr);
                        PhotonNetwork.SendDestroyOfPlayer(plr.ActorNumber);
                        PhotonNetwork.OpRemoveCompleteCacheOfPlayer(plr.ActorNumber);
                        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent((byte)UnityEngine.Random.Range(200, 212), null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
        }

        public static void CrashGunV3()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                Photon.Realtime.Player plr = Ray.collider.GetComponentInParent<PhotonView>().Owner;

                if (Main.GetGunInput(true))
                {
                    for (int i = 0; i < 150; i++)
                    {
                        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
        }

        public static void ActualInstaCrashGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                Photon.Realtime.Player plr = Ray.collider.GetComponentInParent<PhotonView>().Owner;

                if (Main.GetGunInput(true))
                {
                    for (int i = 0; i < 700; i++)
                    {
                        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent(2, null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                    }
                    for (int j = 0; j < 700; j++)
                    {
                        PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent(3, null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
        }

        public static void CrashGunV4()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                Photon.Realtime.Player plr = Ray.collider.GetComponentInParent<PhotonView>().Owner;

                if (Main.GetGunInput(true))
                {
                    for (int i = 0; i < 150; i++)
                    {
                        PhotonNetwork.RaiseEvent((byte)UnityEngine.Random.Range(200, 214), null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                        PhotonNetwork.RaiseEvent((byte)UnityEngine.Random.Range(200, 214), null, new RaiseEventOptions { TargetActors = new int[] { plr.actorNumber } }, SendOptions.SendUnreliable);
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
        }

        public static void CrashAll()
        {
            if (PhotonNetwork.InRoom)
            {
                Utility.MakeMeMaster();
                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
                {
                    if (Utility.RightTrigger)
                    {
                        Utility.BetaCrashPlayer(player);
                        PhotonNetwork.SendAllOutgoingCommands();
                    }
                }
            }
        }

        public static void FloatGun()
        {
            if (Main.GetGunInput(false))
            {   
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.gunLocked && Main.lockTarget != null)
                {
                    Vector3 basePos = Main.lockTarget.headMesh.transform.position;
                    Vector3 offset = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f));
                    Utility.BetaSpawnPrefab("bulletPrefab", basePos + offset, Quaternion.identity);
                }

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        Main.gunLocked = true;
                        Main.lockTarget = who;
                    }
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }

        public static void CrashGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                Utility.SetMaster(Utility.MyPlayer());

                if (Main.gunLocked && Main.lockTarget != null)
                {
                    Utility.BetaCrashPlayer(Main.lockTarget.photonView.Owner);
                    Utility.BetaDestroyPlayers(Main.lockTarget.photonView.Owner);
                    Utility.BetaDestroyPlayers(Main.lockTarget.photonView.Owner);
                    Utility.BetaDestroyPlayers(Main.lockTarget.photonView.Owner);
                    Utility.BetaDestroyPlayers(Main.lockTarget.photonView.Owner);
                    Utility.BetaCrashPlayer(Main.lockTarget.photonView.Owner);
                }

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who)
                    {
                        Main.gunLocked = true;
                        Main.lockTarget = who;
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }

        public static void LagGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                Utility.SetMaster(Utility.MyPlayer());

                if (Main.gunLocked && Main.lockTarget != null)
                {
                    for (int i = 0; i < 225; i++)
                    {
                        RPCManager.RigRPC("SetJumpMultiplier", Main.lockTarget.photonView.Owner, new object[] { float.NaN });
                        RPCManager.RigRPC("SetJumpLimit", Main.lockTarget.photonView.Owner, new object[] { float.NaN });
                        RPCManager.RigRPC("RequestCosmetics", Main.lockTarget.photonView.Owner, new object[] { });
                        GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", Main.lockTarget.photonView.Owner, null);
                        GorillaGameManager.instance.photonView.RPC("UpdatePlayerCosmetic", Main.lockTarget.photonView.Owner, null);
                    }
                }

                if (Main.GetGunInput(true))
                {
                    VRRig who = Ray.collider.GetComponentInParent<VRRig>();
                    if (who != null && who != Utility.myVRRig())
                    {
                        Main.gunLocked = true;
                        Main.lockTarget = who;
                    }
                    PhotonNetwork.SendAllOutgoingCommands();
                }
            }
            else
            {
                Main.lockTarget = null;
                if (Main.gunLocked)
                    Main.gunLocked = false;
            }
        }

        public static void LagAll()
        {
            if (Utility.RightTrigger)
            {
                for (int i = 0; i < 225; i++)
                {
                    RPCManager.RigRPC("SetJumpMultiplier", RpcTarget.Others, new object[] { float.NaN });
                    RPCManager.RigRPC("SetJumpLimit", RpcTarget.Others, new object[] { float.NaN });
                    RPCManager.RigRPC("RequestCosmetics", RpcTarget.Others, new object[] { });
                    GorillaTagger.Instance.myVRRig.photonView.RPC("RequestCosmetics", RpcTarget.Others, null);
                    GorillaGameManager.instance.photonView.RPC("UpdatePlayerCosmetic", RpcTarget.Others, null);
                }
                PhotonNetwork.SendAllOutgoingCommands();
            }
        }


        public static HalloweenGhostChaser lucy
        {
            get
            {
                return GameObject.FindObjectsOfType<HalloweenGhostChaser>()[0] ?? GameObject.FindObjectsOfType<HalloweenGhostChaser>().FirstOrDefault();
            }
        }

        public static void SpawnBlueLucy()
        {
            if (Utility.IsMaster())
            {
                lucy.timeGongStarted = Time.time;
                lucy.currentState = HalloweenGhostChaser.ChaseState.Gong;
                lucy.isSummoned = false;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }
        public static void InstantSpawnBlueLucy()
        {
            if (Utility.IsMaster())
            {
                lucy.timeGongStarted = 0f;
                lucy.currentState = HalloweenGhostChaser.ChaseState.Gong;
                lucy.isSummoned = false;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        public static void SpawnRedLucy()
        {
            if (Utility.IsMaster())
            {
                lucy.timeGongStarted = Time.time;
                lucy.currentState = HalloweenGhostChaser.ChaseState.Gong;
                lucy.isSummoned = false;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }
        public static void InstantSpawnRedLucy()
        {
            if (Utility.IsMaster())
            {
                lucy.timeGongStarted = 0f;
                lucy.currentState = HalloweenGhostChaser.ChaseState.Gong;
                lucy.isSummoned = true;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        public static void DepawnLucy()
        {
            if (Utility.IsMaster())
            {
                lucy.currentState = HalloweenGhostChaser.ChaseState.Dormant;
                lucy.isSummoned = false;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        public static void MoveLucyGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (Main.GetGunInput(true))
                {
                    if (Utility.IsMaster())
                    {
                        lucy.transform.position = Pointer.transform.position;
                    }
                    else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
                }
            }
        }
        public static void GrabLucy()
        {
            if (Utility.RightGrip)
            {
                if (Utility.IsMaster())
                {
                    lucy.grabTime = 0f;
                    lucy.transform.position = Utility.RightHandTransform().transform.position;
                    lucy.transform.rotation = Utility.RightHandTransform().transform.rotation;
                }
                else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
            }
            if (Utility.LeftGrip)
            {
                if (Utility.IsMaster())
                {
                    lucy.grabTime = 0f;
                    lucy.transform.position = Utility.LeftHandTransform().transform.position;
                    lucy.transform.rotation = Utility.LeftHandTransform().transform.rotation;
                }
                else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
            }
        }

        public static void LucyChaseSelf()
        {
            if (Utility.IsMaster())
            {
                lucy.currentState = HalloweenGhostChaser.ChaseState.Chasing;
                lucy.targetPlayer = Utility.MyPlayer();
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        public static void LucyChaseGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (Main.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (Utility.IsMaster())
                    {
                        if (rig != null && rig != Utility.myVRRig())
                        {
                            lucy.currentState = HalloweenGhostChaser.ChaseState.Chasing;
                            lucy.targetPlayer = RigManager.GetPlayerFromVRRig(rig);
                        }
                    }
                    else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
                }
            }
        }

        public static void LucyAttackSelf()
        {
            if (Utility.IsMaster())
            {
                lucy.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                lucy.grabTime = Time.time;
                lucy.targetPlayer = Utility.MyPlayer();
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        public static void LucyAttackGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (Main.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (Utility.IsMaster())
                    {
                        if (rig != null && rig != Utility.myVRRig())
                        {
                            lucy.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                            lucy.grabTime = Time.time;
                            lucy.targetPlayer = RigManager.GetPlayerFromVRRig(rig);
                        }
                    }
                    else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
                }
            }
        }

        public static void SlowLucy()
        {
            if (Utility.IsMaster())
            {
                lucy.currentSpeed = 0.4f;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }
        public static void FastLucy()
        {
            if (Utility.IsMaster())
            {
                lucy.currentSpeed = 5f;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }
        public static void LucyOrbitSelf()
        {
            if (Utility.IsMaster())
            {
                lucy.transform.RotateAround(Utility.MainTransform().position, Vector3.up, 90f * Time.deltaTime);
                lucy.transform.LookAt(Utility.MainTransform());
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        public static void LucyOrbitGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (Main.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (Utility.IsMaster())
                    {
                        if (rig != null && rig != Utility.myVRRig())
                        {
                            lucy.transform.RotateAround(rig.headMesh.transform.position, Vector3.up, 90f * Time.deltaTime);
                            lucy.transform.LookAt(rig.headMesh.transform);
                        }
                    }
                    else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
                }
            }
        }

        public static void LucyFloatSelf()
        {
            if (Utility.IsMaster())
            {
                lucy.targetPlayer = PhotonNetwork.LocalPlayer;
                lucy.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                lucy.timeRiseStarted = 0f;
                lucy.followTarget = Utility.myVRRig().head.rigTarget;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        public static void LucyFloatGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (Main.GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (Utility.IsMaster())
                    {
                        lucy.targetPlayer = rig.photonView.Owner;
                        lucy.currentState = HalloweenGhostChaser.ChaseState.Grabbing;
                        lucy.timeRiseStarted = 0f;
                        lucy.followTarget = rig.head.rigTarget;
                    }
                    else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
                }
            }
        }

        private static float chasespazdelayyhing = 0f;
        public static void LucyChaseSpaz()
        {
            if (Utility.IsMaster())
            {
                lucy.currentState = HalloweenGhostChaser.ChaseState.Chasing;
                if (Time.time > chasespazdelayyhing)
                {
                    lucy.ChooseRandomTarget();
                    chasespazdelayyhing = Time.time + 0.15f;
                }
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        public static void SpawnColouredLucy(Color color)
        {
            if (Utility.IsMaster())
            {
                lucy.defaultColor = color;
                lucy.summonedColor = color;
                lucy.timeGongStarted = Time.time;
                lucy.currentState = HalloweenGhostChaser.ChaseState.Gong;
                lucy.isSummoned = false;
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }

        private static float lucyspaztimedelaything = 0f;
        public static void SpazLucy()
        {
            if (Utility.IsMaster())
            {
                if (Time.time > lucyspaztimedelaything)
                {
                    lucy.timeGongStarted = 0f;
                    lucy.currentState = lucy.currentState == HalloweenGhostChaser.ChaseState.Dormant ? HalloweenGhostChaser.ChaseState.Gong : HalloweenGhostChaser.ChaseState.Dormant;
                    lucyspaztimedelaything = Time.time + 0.15f;
                }
            }
            else { NotificationManager.SendNotification("<color=red>[ERROR]</color> You are not master client!", 3f); }
        }
    }
}