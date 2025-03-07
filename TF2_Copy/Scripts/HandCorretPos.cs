using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCorretPos : MonoBehaviour
{

    private void OnAnimatorIK(int layerIndex)
    {
        if (!animator.GetCurrentAnimatorStateInfo(2).IsName("Reload"))
        { 
            CorrectCatch();
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }
    }

    public Animator animator;
    public Transform gunGrabPos;
    public Vector3 relativeVecHand;
    public void CorrectCatch()
    {
        //playerHandTr.transform.position = gunGrabPos.transform.position + relativeVecHand;
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, gunGrabPos.transform.position + relativeVecHand);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, gunGrabPos.transform.rotation);

    }
}
