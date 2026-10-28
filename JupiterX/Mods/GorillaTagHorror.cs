// ============================================================
//  JupiterX | Mods/GorillaTagHorror.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using HarmonyLib;
using JupiterX.Notifications;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static JupiterX.Menu.Main;

namespace JupiterX.Mods
{
    public class GorillaTagHorror
    {
        private static GameObject Timmy
        {
            get
            {
                return Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x != null && x.name.ToLower().Contains("timmy")).FirstOrDefault();
            }
        }
        private static GameObject Stalker
        {
            get
            {
                return Resources.FindObjectsOfTypeAll<GameObject>().Where(x => x != null && x.name.ToLower().Contains("stalker")).FirstOrDefault();
            }
        }
        private static void SpawnHorrorPrefab(string name, Vector3 pos, Quaternion rot) => PhotonNetwork.Instantiate($"horror/{name}", pos, rot);
        private static GameObject SpawnTimmy(Vector3 pos, Quaternion rot) => PhotonNetwork.Instantiate("horror/timmy", pos, rot);
        private static GameObject SpawnStalker(Vector3 pos, Quaternion rot) => PhotonNetwork.Instantiate("horror/stalker", pos, rot);

        private static float lastTimmyUpdate;
        private static List<GameObject> cachedTimmys = new List<GameObject>();

        private static List<GameObject> GetTimmys()
        {
            if (Time.time - lastTimmyUpdate < 1f)
                return cachedTimmys;
            lastTimmyUpdate = Time.time;
            cachedTimmys.Clear();
            foreach (var obj in GameObject.FindObjectsOfType<Transform>())
            {
                var name = obj.name;
                if (name.Contains("timmy"))
                    cachedTimmys.Add(obj.gameObject);
            }
            return cachedTimmys;
        }

        private static float lastStalkerUpdate;
        private static List<GameObject> cachedStalkers = new List<GameObject>();
        private static List<GameObject> GetStalkers()
        {
            if (Time.time - lastStalkerUpdate < 1f)
                return cachedStalkers;
            lastStalkerUpdate = Time.time;
            cachedStalkers.Clear();
            foreach (var obj in GameObject.FindObjectsOfType<Transform>())
            {
                var name = obj.name;
                if (name.Contains("stalker"))
                    cachedStalkers.Add(obj.gameObject);
            }
            return cachedStalkers;
        }

        private static float lastMonsterUpdate;
        private static List<GameObject> cachedMonsters = new List<GameObject>();
        private static List<GameObject> GetAllMonsters()
        {
            if (Time.time - lastMonsterUpdate < 1f)
                return cachedMonsters;
            lastMonsterUpdate = Time.time;
            cachedMonsters.Clear();
            foreach (var obj in GameObject.FindObjectsOfType<Transform>())
            {
                var go = obj.gameObject;
                if (go.GetComponent("EnemyController") != null)
                    cachedMonsters.Add(go);
            }
            return cachedMonsters;
        }

        public static void SpawnTimmy()
        {
            SpawnHorrorPrefab("timmy", GorillaTagger.Instance.headCollider.transform.position * 0.3f, Quaternion.identity);
        }
        public static void SpawnStalker()
        {
            SpawnHorrorPrefab("stalker", GorillaTagger.Instance.headCollider.transform.position * 0.3f, Quaternion.identity);
        }

        public static void TimmyESP(bool disable)
        {
            List<GameObject> timmys = GetTimmys();
            foreach (var item in timmys)
            {
                Renderer[] rends = item.GetComponentsInChildren<Renderer>(true);
                foreach (var r in rends)
                {
                    r.material.shader = disable ? Utility.StandardShader() : Utility.GUIShader();
                    r.material.color = disable ? new Color(0f, 0f, 0f) : new Color(0f, 0.6f, 0f);
                }
            }
        }

        public static void StalkerESP(bool disable)
        {
            List<GameObject> stalkers = GetStalkers();
            foreach (var item in stalkers)
            {
                Renderer[] rends = item.GetComponentsInChildren<Renderer>(true);
                foreach (var r in rends)
                {
                    r.material.shader = disable ? Utility.StandardShader() : Utility.GUIShader();
                    r.material.color = disable ? new Color(0f, 0f, 0f) : new Color(0f, 0.6f, 0f);
                }
            }
        }

        public static void TimmyTracers()
        {
            List<GameObject> timmys = GetTimmys();
            foreach (var item in timmys)
            {
                GameObject tracerholder = new GameObject();
                LineRenderer tracer = tracerholder.AddComponent<LineRenderer>();
                tracer.useWorldSpace = true;
                tracer.material.shader = Utility.GUIShader();
                tracer.positionCount = 2;
                tracer.startColor = Color.grey;
                tracer.startColor = Color.grey;
                tracer.startWidth = 0.02f;
                tracer.endWidth = 0.02f;
                tracer.SetPosition(0, Utility.RightHandTransform().position);
                tracer.SetPosition(1, item.transform.position);
                GameObject.Destroy(tracerholder, Time.deltaTime);
            }
        }
        public static void StalkerTracers()
        {
            List<GameObject> stalkers = GetStalkers();
            foreach (var item in stalkers)
            {
                GameObject tracerholder = new GameObject();
                LineRenderer tracer = tracerholder.AddComponent<LineRenderer>();
                tracer.useWorldSpace = true;
                tracer.material.shader = Utility.GUIShader();
                tracer.positionCount = 2;
                tracer.startColor = Color.grey;
                tracer.startColor = Color.grey;
                tracer.startWidth = 0.02f;
                tracer.endWidth = 0.02f;
                tracer.SetPosition(0, Utility.RightHandTransform().position);
                tracer.SetPosition(1, item.transform.position);
                GameObject.Destroy(tracerholder, Time.deltaTime);
            }
        }

        public static void TimmySpam()
        {
            if (Utility.RightGrip)
            {
                Transform hand = Utility.RightHandTransform();
                SpawnHorrorPrefab("timmy", hand.position + hand.forward * 5f, hand.rotation);
            }
            if (Utility.LeftGrip)
            {
                Transform hand = Utility.LeftHandTransform();
                SpawnHorrorPrefab("timmy", hand.position + hand.forward * 5f, hand.rotation);
            }
        }

        public static void StalkerSpam()
        {
            if (Utility.RightGrip)
            {
                Transform hand = Utility.RightHandTransform();
                SpawnHorrorPrefab("stalker", hand.position + hand.forward * 5f, hand.rotation);
            }
            if (Utility.LeftGrip)
            {
                Transform hand = Utility.LeftHandTransform();
                SpawnHorrorPrefab("stalker", hand.position + hand.forward * 5f, hand.rotation);
            }
        }

        public static void TimmyGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (GetGunInput(true))
                {
                    SpawnHorrorPrefab("timmy", Pointer.transform.position, Pointer.transform.rotation);
                }
            }
        }

        public static void StalkerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (GetGunInput(true))
                {
                    SpawnHorrorPrefab("stalker", Pointer.transform.position, Pointer.transform.rotation);
                }
            }
        }

        public static void KillGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget)
                {
                    GameObject stalker = SpawnStalker(lockTarget.headMesh.transform.position, lockTarget.headMesh.transform.rotation);
                    GameObject.Destroy(stalker);
                }

                if (GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        lockTarget = rig;
                        gunLocked = true;
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

        public static void TimmyRapeGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget)
                {
                    Timmy.transform.position = Vector3.Lerp(Timmy.transform.position, lockTarget.headMesh.transform.position, 0.8f);
                }

                if (GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        lockTarget = rig;
                        gunLocked = true;
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

        public static void StalkerRapeGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget)
                {
                    Stalker.transform.position = Vector3.Lerp(Stalker.transform.position, lockTarget.headMesh.transform.position, 0.8f);
                }

                if (GetGunInput(true))
                {
                    VRRig rig = Ray.collider.GetComponentInParent<VRRig>();
                    if (rig != null && rig != GorillaTagger.Instance.myVRRig)
                    {
                        lockTarget = rig;
                        gunLocked = true;
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

        public static void KillAll()
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != Utility.myVRRig())
                {
                    GameObject stalker = SpawnStalker(rig.headMesh.transform.position, Quaternion.identity);
                    GameObject.Destroy(stalker);
                }
            }
        }

        public static void FlingGun(string name)
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    if (Ray.collider.gameObject.name.ToLower().Contains(name))
                    {
                        Ray.collider.gameObject.transform.position += new Vector3(Ray.collider.gameObject.transform.position.x, Ray.collider.gameObject.transform.position.y + 250f, Ray.collider.gameObject.transform.position.z);
                    }
                }
            }
        }
        public static void FlingGunComponent(string name)
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    if (Ray.collider.gameObject.GetComponent(name))
                    {
                        Ray.collider.gameObject.transform.position += new Vector3(Ray.collider.gameObject.transform.position.x, Ray.collider.gameObject.transform.position.y + 250f, Ray.collider.gameObject.transform.position.z);
                    }
                }
            }
        }

        public static void BringAllMonsters()
        {
            List<GameObject> monters = GetAllMonsters();
            foreach (var item in monters)
            {
                item.transform.position = GorillaTagger.Instance.headCollider.transform.position;
            }
        }
        public static void BringMonstersGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (GetGunInput(true))
                {
                    List<GameObject> monsters = GetAllMonsters();
                    foreach (var item in monsters)
                    {
                        item.transform.position = Pointer.transform.position;
                    }
                }
            }
        }

        public static void KillTimmyGun()
        {
            if (GetGunInput(false))
            {
                Utility.MakeMeMaster();
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    if (Ray.collider.gameObject.name.ToLower().Contains("timmy"))
                    {
                        Ray.collider.gameObject.transform.position = new Vector3(0f, -6969f, 0f);
                    }
                }
            }
        }

        public static void KillStalkerGun()
        {
            if (GetGunInput(false))
            {
                Utility.MakeMeMaster();
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    if (Ray.collider.gameObject.name.ToLower().Contains("stalker"))
                    {
                        Ray.collider.gameObject.transform.position = new Vector3(0f, -6969f, 0f);
                    }
                }
            }
        }
        public static void KillMonsterGun()
        {
            if (GetGunInput(false))
            {
                Utility.MakeMeMaster();
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;
                if (GetGunInput(true))
                {
                    if (Ray.collider.gameObject.GetComponent("EnemyController"))
                    {
                        Ray.collider.gameObject.transform.position = new Vector3(0f, -6969f, 0f);
                    }
                }
            }
        }

        public static void KillAllTimmys()
        {
            Utility.MakeMeMaster();
            List<GameObject> timmys = GetTimmys();
            foreach (var item in timmys)
            {
                item.transform.position = new Vector3(0f, -6969f, 0f);
            }
        }

        public static void KillAllStalkers()
        {
            Utility.MakeMeMaster();
            List<GameObject> stalkers = GetStalkers();
            foreach (var item in stalkers)
            {
                item.transform.position = new Vector3(0f, -6969f, 0f);
            }
        }
        public static void KillAllMonsters()
        {
            Utility.MakeMeMaster();
            List<GameObject> monsters = GetAllMonsters();
            foreach (var item in monsters)
            {
                item.transform.position = new Vector3(0f, -6969f, 0f);
            }
        }

        public static void ExplodeTimmyGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (GetGunInput(true))
                {
                    GameObject timmy = SpawnTimmy(Pointer.transform.position, Pointer.transform.rotation);
                    Rigidbody timmybody = timmy.GetComponent<Rigidbody>();
                    timmybody.AddExplosionForce(500f, timmy.transform.position, 10f);
                }
            }
        }
        public static void ExplodeStalkerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject Pointer = GunData.Pointer;
                if (GetGunInput(true))
                {
                    GameObject stalker = SpawnStalker(Pointer.transform.position, Pointer.transform.rotation);
                    Rigidbody stalkerbody = stalker.GetComponent<Rigidbody>();
                    stalkerbody.AddExplosionForce(500f, stalker.transform.position, 10f);
                }
            }
        }
        public static void ExplodeMonsters()
        {
            Utility.MakeMeMaster();
            List<GameObject> monsters = GetAllMonsters();
            foreach (var item in monsters)
            {
                Rigidbody itemrbody = item.GetComponent<Rigidbody>();
                itemrbody.AddExplosionForce(500f, item.transform.position, 10f);
            }
        }

        public static void BecomeTimmy()
        {
            Utility.MakeMeMaster();
            Timmy.GetComponent<Collider>().enabled = false;
            Timmy.transform.position = GorillaTagger.Instance.headCollider.transform.position;
            Timmy.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
        }
        public static void BecomeStalker()
        {
            Utility.MakeMeMaster();
            Stalker.GetComponent<Collider>().enabled = false;
            Stalker.transform.position = GorillaTagger.Instance.headCollider.transform.position;
            Stalker.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
        }

        private static Component enemyController;
        private static Component GetEnemyController()
        {
            if (Timmy == null) return null;
            if (enemyController == null)
            {
                enemyController = Timmy.GetComponent(Il2CppSystem.Type.GetType("EnemyController"));
            }
            return enemyController;
        }
        public static void FastTimmys()
        {
            var comp = GetEnemyController();
            if (comp == null) return;
            Traverse.Create(comp).Field("moveSpeed").SetValue(70f);
        }
        public static void ResetTimmy()
        {
            var comp = GetEnemyController();
            if (comp == null) return;
            Traverse.Create(comp).Field("moveSpeed").SetValue(1f);
        }
        public static void SlowTimmys()
        {
            var comp = GetEnemyController();
            if (comp == null) return;
            Traverse.Create(comp).Field("moveSpeed").SetValue(0.01f);
        }

        public static void SpazTimmys()
        {
            List<GameObject> timmys = GetTimmys();
            foreach (var item in timmys)
            {
                item.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
            }
        }
        public static void SpazStalkers()
        {
            List<GameObject> stalkers = GetStalkers();
            foreach (var item in stalkers)
            {
                item.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f), UnityEngine.Random.Range(0f, 360f));
            }
        }

        private static GameObject trap = null;
        public static void PlaceTrap()
        {
            if (Utility.RightTrigger)
            {
                if (trap == null)
                {
                    trap = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    trap.transform.localScale = new Vector3(0.1f, 0.2f, 0.2f);
                    trap.transform.position = Utility.RightHandTransform().position;
                    trap.transform.rotation = Utility.RightHandTransform().rotation;
                    trap.GetComponent<Renderer>().material.color = Color.green;
                    GameObject.Destroy(trap.GetComponent<Collider>());
                }
                trap.transform.position = Utility.RightHandTransform().position;
                trap.transform.rotation = Utility.RightHandTransform().rotation;
            }
        }
        public static void DestroyTrap()
        {
            if (trap != null)
            {
                GameObject.Destroy(trap);
                trap = null;
            }
        }
        public static void TimmysToTrap()
        {
            if (trap == null)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> Trap has not been placed!");
                return;
            }
            List<GameObject> timmys = GetTimmys();
            foreach (var item in timmys)
            {
                item.transform.position = Vector3.MoveTowards(item.transform.position, trap.transform.position, 11f * Time.deltaTime);
            }
        }
        public static void StalkersToTrap()
        {
            if (trap == null)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> Trap has not been placed!");
                return;
            }
            List<GameObject> stalkers = GetStalkers();
            foreach (var item in stalkers)
            {
                item.transform.position = Vector3.MoveTowards(item.transform.position, trap.transform.position, 11f * Time.deltaTime);
            }
        }
        public static void MonstersToTrap()
        {
            if (trap == null)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> Trap has not been placed!");
                return;
            }
            List<GameObject> monsters = GetAllMonsters();
            foreach (var item in monsters)
            {
                item.transform.position = Vector3.MoveTowards(item.transform.position, trap.transform.position, 5f * Time.deltaTime);
            }
        }

        public static void TimmyWork()
        {
            if (Utility.RightTrigger)
            {
                Vector3 pos = new Vector3(-67f, 100f, 18);
                SpawnTimmy(pos, Quaternion.identity);
            }
        }
        public static void StalkerWork()
        {
            if (Utility.RightTrigger)
            {
                Vector3 pos = new Vector3(-67f, 100f, 18);
                SpawnStalker(pos, Quaternion.identity);
            }
        }

        public static void JoystickControlTimmys()
        {
            List<GameObject> timmys = GetTimmys();
            Vector2 axis = Utility.RightJoystickAxis;
            foreach (var item in timmys)
            {
                Vector3 pos = item.transform.position;
                pos.x += axis.x;
                pos.z += axis.y;
                item.transform.position = pos;
            }
        }
        public static void JoystickControlStalkers()
        {
            List<GameObject> stalkers = GetStalkers();
            Vector2 axis = Utility.RightJoystickAxis;
            foreach (var item in stalkers)
            {
                Vector3 pos = item.transform.position;
                pos.x += axis.x;
                pos.z += axis.y;
                item.transform.position = pos;
            }
        }
        public static void JoystickControlMonters()
        {
            List<GameObject> monsters = GetAllMonsters();
            Vector2 axis = Utility.RightJoystickAxis;
            foreach (var item in monsters)
            {
                Vector3 pos = item.transform.position;
                pos.x += axis.x;
                pos.z += axis.y;
                item.transform.position = pos;
            }
        }

        public static void FullBright(bool enabled)
        {
            if (enabled)
            {
                RenderSettings.fog = false;
                RenderSettings.ambientLight = Color.white;
            }
            else
            {
                RenderSettings.fog = true;
                RenderSettings.ambientLight = Color.black;
            }
        }
    }
}