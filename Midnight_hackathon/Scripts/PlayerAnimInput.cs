using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimInput : MonoBehaviour
{
    Animator animator;

    Transform leftHand;
    Transform leftLowerArm;
    Transform rightHand;
    Transform rightLowerArm;

    public Vector3 lefthandPos;
    public Vector3 rightHandPos;

    Vector3 lefthanddir;
    Vector3 rightHanddir;

    public UDPServer udpServer;
    public Vector2 calcOffset = new Vector2(1280 / 2, 720);

    private void Start()
    {
        animator = GetComponent<Animator>();

        leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        rightLowerArm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
        leftLowerArm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
    }

    private void Update()
    {
        // Get Current User Pose
        GetCurrentPose();
        lefthanddir = (leftHand.position - leftLowerArm.position).normalized;
        rightHanddir = (rightHand.position - rightLowerArm.position).normalized;
    }

    private void GetCurrentPose()
    {
        if (udpServer == null) return;
        var pose = udpServer.CurrentPoseInfo;
        Vector2 lhPos = new Vector2(pose.rhX - calcOffset.x, calcOffset.y - pose.rhY);
        lefthandPos = (new Vector3(lhPos.x, lhPos.y, -100));
        Vector2 rhPos = new Vector2(pose.lhX - calcOffset.x, calcOffset.y - pose.lhY);
        rightHandPos = (new Vector3(rhPos.x, rhPos.y, -100));
    }

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(lefthanddir));
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position + lefthandPos);
        animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(rightHanddir));
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position + rightHandPos);
    }
}