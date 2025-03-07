using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diver_Ani : MonoBehaviour
{
    // 전등 사용 유무에 따라 애니메이션이 다름


    public Animator animator;
    Transform cameraTransform;
    public GameObject playerCamera;
    public GameObject handTarget;
    public GameObject legTarget;
    public PlayerMove playerMove;

    [Range(0.0f, 1.0f)]
    public float handOffset;
    
    [Range(0.0f, 1.0f)]
    public float legOffset;

    public Vector3 v3offset;

    public float horizontal;
    public float vertical;

    private void Start()
    {
        animator = GetComponent<Animator>();
        cameraTransform = Camera.main.transform;
    }

    // 나중에 손전등 쓸때 적용 될듯
    private void Update()
    {
        if (!playerMove.isMove) return;
        horizontal = Input.GetAxis("Horizontal");// 왼오
        vertical = Input.GetAxis("Vertical");// 앞뒤

        if (PlayerInfo.instance.isFlashLightOn)
        {
            RaycastHit hit;
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit);

            
            if (hit.collider == null || (hit.point - transform.position).magnitude < 4)
            {
                //Vector3 direction = playerCamera.transform.GetChild(0).position - playerCamera.transform.position;
                //Quaternion target = Quaternion.LookRotation(direction);
                //target = Quaternion.Euler(new Vector3(target.eulerAngles.x - 115, target.eulerAngles.y, 80));
                //handTarget.transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime);
                handTarget.transform.LookAt(playerCamera.transform.GetChild(0));
            }
            else
            {
                //print(hit.collider.name);
                //Vector3 direction = hit.point - transform.position;
                //Quaternion target = Quaternion.LookRotation(direction);
                //target = Quaternion.Euler(new Vector3(target.eulerAngles.x - 115, target.eulerAngles.y, 80));
                //handTarget.transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime);
    
                handTarget.transform.LookAt(hit.point);
            }

            // 회전 보정
            handTarget.transform.eulerAngles = new Vector3(handTarget.transform.eulerAngles.x , handTarget.transform.eulerAngles.y, -90);
            
            // 나중에 팔꿈치 회전값 보정 추가
            handTarget.transform.parent.eulerAngles = new Vector3(handTarget.transform.parent.eulerAngles.x, handTarget.transform.parent.eulerAngles.y, handTarget.transform.parent.eulerAngles.z);

            handOffset += Time.deltaTime;
            if (handOffset >= 1) handOffset = 1;
        }
        else
        {
            handOffset -= Time.deltaTime;
            if (handOffset <= 0) handOffset = 0;
        }


        if (PlayerInfo.instance.isFlashLightOn)
        {
            if (vertical > 0)
            {
                animator.SetFloat("Z", vertical);
                v3offset = new Vector3(0, v3offset.y, v3offset.z);
            }
            else if (vertical == 0)
            {
                animator.SetFloat("Z", 0);
                v3offset = new Vector3(0, v3offset.y, v3offset.z);
            }
            else
            {
                v3offset = new Vector3(vertical * 50, v3offset.y, v3offset.z);
            }

            v3offset = new Vector3(v3offset.x, v3offset.y, horizontal * -50);
        }
        else
        {
            float value = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
            Mathf.Clamp(value, 0, 1);
            animator.SetFloat("Z", value);
        }
    }

    private void LateUpdate()
    {
        if (!playerMove.isMove) return;
        MoveSpine();
    }

    private void OnAnimatorIK()
    {
        if (!playerMove.isMove) return;
        Vector3 lookPosition = cameraTransform.position + cameraTransform.forward * 10.0f;

        HandLight();
    }

    // 팔 IK 조절
    void HandLight()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handOffset);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handOffset);

        animator.SetIKPosition(AvatarIKGoal.RightHand, handTarget.transform.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, handTarget.transform.rotation);
    }

    // 다리 IK 조절
    void MoveLeg()
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, legOffset);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, legOffset);

        animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, legOffset);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, legOffset);
    }

    // 다리 트랜스폼 조절
    void MoveSpine()
    {
        Transform leftUpperLegTr = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        Transform rightUpperLegTr = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);

        leftUpperLegTr.eulerAngles += v3offset;
        rightUpperLegTr.eulerAngles += v3offset;
    }

    void AnimationBlend()
    {

    }
}
