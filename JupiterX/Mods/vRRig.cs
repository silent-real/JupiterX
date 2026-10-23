// ============================================================
//  JupiterX | Mods/vRRig.cs
//  Copyright (c) 2026 Jupiterx (@NAuth). All rights reserved.
//
//  This software and its source code are the property of the
//  author. Unauthorized copying, redistribution, modification,
//  or reuse of any part of this project, in whole or in part,
//  without express written permission is strictly prohibited.
//
//  Version: 2.0.0 | By Silent/Ashley/Nova (@s1lnt)
// ============================================================

using GorillaNetworking;
using JupiterX.Menu;
using Photon.Pun;
using UnityEngine;

namespace JupiterX.Mods
{
    public class vRRig
    {
        private static bool invisActive;
        public static void InvisMonke()
        {
            if (Utility.RightSecondary)
                invisActive = !invisActive;

            if (invisActive)
            {
                Utility.ActualRig().enabled = false;
                Utility.ActualRig().transform.position = new Vector3(20397230f, 32423, 3432);
                Utility.GhostView(true);
            }
            else
            {
                Utility.ActualRig().enabled = true;
                Utility.GhostView(false);
            }
        }

        private static bool ghostActive;
        public static void GhostMonke()
        {
            if (Utility.RightPrimary)
                ghostActive = !ghostActive;

            if (ghostActive)
            {
                Utility.ActualRig().enabled = false;
                Utility.GhostView(true);
            }
            else
            {
                Utility.ActualRig().enabled = true;
                Utility.GhostView(false);
            }
        }

        private static float delaytimebeesbleh;
        public static void Bees()
        {
            if (!PhotonNetwork.InRoom) return;
            VRRig myRig = Utility.myVRRig();
            myRig.enabled = false;
            Utility.GhostView(true);
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig == null || rig == myRig) continue;
                if (Time.time > delaytimebeesbleh)
                {
                    Vector3 targetPos = rig.headMesh.transform.position + new Vector3(0f, 0.6f, 0f);
                    myRig.transform.position = targetPos;
                    myRig.rightHandTransform.position -= myRig.transform.position * 0.3f;
                    myRig.leftHandTransform.position -= myRig.transform.position * 0.3f;
                    delaytimebeesbleh = Time.time + 0.3f;
                }
            }
        }

        public static void SpazRig()
        {
            Utility.ActualRig().head.rigTarget.eulerAngles = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
            Utility.ActualRig().leftHand.rigTarget.eulerAngles = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
            Utility.ActualRig().rightHand.rigTarget.eulerAngles = new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
		}
        public static void Strobe()
        {
            GorillaKeyboardButton buttanpress = new GorillaKeyboardButton();
            GorillaComputer.instance.colorCursorLine = Random.Range(0, 3);
            buttanpress.characterString = Random.Range(0, 10).ToString();
            GorillaComputer.instance.ProcessColorState(buttanpress);
        }
        public static void MoveRigGun()
        {
            if (Main.GetGunInput(false))
            {
                var GunData = Main.RenderGun();
                GameObject NewPointer = GunData.Pointer;
                RaycastHit Ray = GunData.Ray;

                if (Main.GetGunInput(true))
                {
                    Utility.ActualRig().enabled = false;
                    Utility.ActualRig().transform.position = Ray.point;
                    Utility.GhostView(true);
                }
                else
                {
                    Utility.ActualRig().enabled = true;
                    Utility.GhostView(false);
                }
            }
        }

        public static void GrabRig()
        {
            if (Utility.RightGrip)
            {
                Utility.ActualRig().enabled = false;
                Utility.ActualRig().transform.position = Utility.RightHandTransform().position;
                Utility.GhostView(true);
            }
            if (Utility.LeftGrip)
            {
                Utility.ActualRig().enabled = false;
                Utility.ActualRig().transform.position = Utility.LeftHandTransform().position;
                Utility.GhostView(true);
            }
            if (!Utility.LeftGrip && !Utility.RightGrip)
            {
                Utility.ActualRig().enabled = true;
                Utility.GhostView(false);
            }
        }

        public static float yRotation;
        public static void DecapitateRig()
        {
            if (AreHandsDown())
            {
                float targetYRotation = CalculateTorsoYRotation();
                yRotation = Mathf.LerpAngle(yRotation, targetYRotation, .8f);
            }
            else
            {
                yRotation = GorillaTagger.Instance.mainCamera.transform.eulerAngles.y;
            }
        }
        private static bool AreHandsDown()
        {
            return Utility.LeftHandTransform().position.y < GorillaTagger.Instance.mainCamera.transform.position.y && Utility.RightHandTransform().position.y < GorillaTagger.Instance.mainCamera.transform.position.y;
        }

        private static float CalculateTorsoYRotation()
        {
            Vector3 headForward = GorillaTagger.Instance.mainCamera.transform.forward;
            headForward.y = 0;
            headForward.Normalize();
            Vector3 handCenter = (Utility.LeftHandTransform().position + Utility.RightHandTransform().position) / 2f;
            Vector3 handDirection = handCenter - GorillaTagger.Instance.mainCamera.transform.position;
            handDirection.y = 0;
            handDirection.Normalize();
            Vector3 torsoDirection = Vector3.Lerp(headForward, handDirection, 0.45f);
            torsoDirection.Normalize();
            if (Vector3.Dot(torsoDirection, headForward) < 0)
                torsoDirection = headForward;
            return Quaternion.LookRotation(torsoDirection, Vector3.up).eulerAngles.y;
        }

        public static void ParalyzeRig()
        {
            Utility.ActualRig().enabled = false;
            Utility.ActualRig().transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            Utility.ActualRig().transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            Utility.ActualRig().head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;

            Utility.ActualRig().leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.08f + GorillaTagger.Instance.bodyCollider.transform.up * 0.12f;
            Utility.ActualRig().rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.08f + GorillaTagger.Instance.bodyCollider.transform.up * 0.12f;

            Utility.ActualRig().leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(0f, 180f, 180f);
            Utility.ActualRig().rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(0f, 180f, 180f);
        }

        public static void ChickenRig()
        {
            Utility.ActualRig().enabled = false;
            Utility.ActualRig().transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            Utility.ActualRig().transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            Utility.ActualRig().head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;

            Utility.ActualRig().leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.2f + GorillaTagger.Instance.bodyCollider.transform.up * -0.2f;
            Utility.ActualRig().rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.2f + GorillaTagger.Instance.bodyCollider.transform.up * -0.2f;

            Utility.ActualRig().leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            Utility.ActualRig().rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
        }

        public static void AmputateRig()
        {
            Utility.ActualRig().enabled = false;
            Utility.ActualRig().transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            Utility.ActualRig().transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            Utility.ActualRig().head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(160f, 90f, 0f);

            Utility.ActualRig().leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.08f + GorillaTagger.Instance.bodyCollider.transform.up * 0.12f;
            Utility.ActualRig().rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.08f + GorillaTagger.Instance.bodyCollider.transform.up * 0.12f;

            Utility.ActualRig().leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(0f, 180f, 180f);
            Utility.ActualRig().rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(0f, 180f, 180f);
        }
    }
}