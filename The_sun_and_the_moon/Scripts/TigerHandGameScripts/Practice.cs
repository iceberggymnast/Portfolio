//using UnityEngine;
//using System.Collections;
//using Unity.VisualScripting;

//public class Practice : MonoBehaviour
//{
//    // 1. Scene에 4개의 오브젝트가 있다.
//    // (호랑이 왼손, 호랑이 오른손, 사람 왼손, 사람 오른 손)

//    // 4개의 오브젝트를 적용하자. (사람 손은 끝에 "P" 로 호랑이는 끝에 "T"를 붙여 구분한다.)
//    public Transform leftHandP;
//    public Transform rightHandP;
//    public Transform leftHandT;
//    public Transform righHandT;

//    // 손 오브젝트들이 운동하는 속력
//    public float moveSpeed = 2.0f;

//    // 이동 상태를 부드럽게 보간하는 시간
//    public float smoothTime = 2.0f;

//    private Vector3 leftHandPTargetPos, leftHandTTargetPos;
//    private Vector3 rightHandPTargetPos, rightHandTTargetPos;
//    private float leftHand1LerpTime, leftHand2LerpTime;
//    private float rightHand1LerpTime, rightHand2LerpTime;

//    // 손 오브젝트들의 운동 높이 최댓값과 최솟값
//    // (오브젝트들은 현재 X축으로 -90도 회전된 상태이다.)
//    public float minZ = 5.0f; // 손이 문 안에 있는 상태
//    public float maxZ = 5.0f; // 손이 문 바깥에 있는 상태

//    // 각 오브젝트들의 움직임을 제어할 bool 타입 변수
//    private bool MoveUp_LP = false;
//    private bool MoveUp_RP = false;
//    private bool MoveUp_LT = false;
//    private bool MoveUp_RT = false;

//    // 오브젝트 목표 위치 설정에 사용할 변수
//    // (사람 왼손, 호랑이 왼손, 사람 오른손, 호랑이 오른손)
//    private Vector3 leftPPos, leftTPos, rightPPos, rightTPos;

//    void Start()
//    {
//        // 각 오브젝트들의 목표 위치 설정
//        // 사람
//        leftPPos = new Vector3(leftHandP.position.x, leftHandP.position.y, MoveUp_LP ? maxZ : minZ);
//        rightPPos = new Vector3(rightHandP.position.x, rightHandP.position.y, MoveUp_RP ? maxZ : minZ);

//        // 호랑이
//        leftTPos = new Vector3(leftHandT.position.x, leftHandT.position.y, MoveUp_LT ? maxZ : minZ);
//        rightTPos = new Vector3(righHandT.position.x, righHandT.position.y, MoveUp_RT ? maxZ : minZ);

//        // 랜덤하게 상태 변경하는 코루틴을 시작함.
//        StartCoroutine((HandMovement()));
//    }

//    // 이동하는 손을 설정하는 메서드
//    public void MoveHand(Transform hand, bool isMoving, ref Vector3 targetPosition, ref float lerpTime)
//    {
//        // 목표 z 위치 설정
//        targetPosition = new Vector3(hand.position.x, hand.position.y, isMoving ? minZ : maxZ);

//        // 부드럽게 보간하여 위치 변경
//        lerpTime += Time.deltaTime * moveSpeed;
//        hand.position = Vector3.Lerp(hand.position, targetPosition, lerpTime);

//        // 목표에 도달하면 lerpTime을 초기화
//        if (Vector3.Distance(hand.position, targetPosition) < 0.01f)
//        {
//            hand.position = targetPosition;
//            lerpTime = 0;
//        }
//    }

//    void Update()
//    {
//        // 각각의 Hand 오브젝트를 이동시키기 위한 메서드 호출
//        MoveHand(leftHandP, MoveUp_LP, ref leftPPos, ref leftHand1LerpTime);
//        MoveHand(leftHandT, MoveUp_RP, ref leftTPos, ref leftHand2LerpTime);
//        MoveHand(rightHandP, MoveUp_LT, ref rightPPos, ref rightHand1LerpTime);
//        MoveHand(righHandT, MoveUp_RT, ref rightTPos, ref rightHand2LerpTime);
//    }

//    // 각 오브젝트들이 순차적으로 true/false가 되도록 조정하는 코루틴(게임 진행 중 오브젝트들이 동시에 나오는 것을 방지하기 위해 넣음.)
//    IEnumerator HandMovement()
//        {
//            while (true)
//            {
//                float random = Random.Range(1.0f, 3.0f); // 1 ~ 3초사이의 랜덤 대기 시간 설정
//                yield return new WaitForSeconds(random);
//            }

//            // 현재 움직이는 손 무작위로 변경
//            int randomHand = Random.Range(0, 4);
//            switch (randomHand)
//            {
//                // 사람 왼손 만 true
//                case 0:
//                    MoveUp_LP = true;
//                    MoveUp_RP = false;
//                    MoveUp_LT = false;
//                    MoveUp_RT = false;
//                    break;

//                // 사람 오른손 만 true
//                case 1:
//                    MoveUp_RP = false;
//                    MoveUp_LP = true;
//                    MoveUp_LT = false;
//                    MoveUp_RT = false;
//                    break;

//                // 호랑이 왼손 만 true
//                case 2:
//                    MoveUp_RP = false;
//                    MoveUp_LT = false;
//                    MoveUp_LP = true;
//                    MoveUp_RT = false;
//                    break;

//                // 호랑이 오른손 만 true
//                case 3:
//                    MoveUp_RP = false;
//                    MoveUp_LT = false;
//                    MoveUp_RT = false;
//                    MoveUp_LP = true;
//                    break;
//            }
//        }
//}
