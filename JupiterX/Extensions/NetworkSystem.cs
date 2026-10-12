// ============================================================
//  JupiterX | Extensions/NetworkSystem.cs
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
using Photon.Realtime;

namespace JupiterX
{
    public static class NetworkSystem
    {
        public static event Action JoinedRoom;
        public static event Action LeftRoom;
        public static event Action MasterClientSwitched;

        public static event Action<Player> PlayerJoinedRoom;
        public static event Action<Player> PlayerLeftRoom;

        public static void RaiseJoinedRoom()
            => JoinedRoom?.Invoke();

        public static void RaiseLeftRoom()
            => LeftRoom?.Invoke();

        public static void RaiseMasterClientSwitched()
            => MasterClientSwitched?.Invoke();

        public static void RaisePlayerJoinedRoom(Player player)
            => PlayerJoinedRoom?.Invoke(player);

        public static void RaisePlayerLeftRoom(Player player)
            => PlayerLeftRoom?.Invoke(player);
    }
}