using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class HandMovement : MonoBehaviour
{
    // 오브젝트를 참조할 수 있는 public Transform 변수들
    public Transform[] leftHands;
    public Transform[] rightHands;

    public Transform[] handTransforms;

    // 각각의 이동 상태를 제어할 bool 변수들
    public bool[] isLeftHandMoving = new bool[1];
    public bool[] isRightHandMoving = new bool[1];

    public bool[] isHandMoving;

    // 최소 및 최대 z 좌표
    private float minZ = -5.0f;
    private float maxZ = 5.0f;

    // 이동 속도 설정
    public float moveSpeed = 5.0f;

    // 이동 상태를 부드럽게 보간하는 시간
    public float smoothTime = 1.0f;

    private Vector3[] targetPositions = new Vector3[4];
    private float[] lerpTimes = new float[4];

    // 미니게임이 실행 중인지 확인하는 Flag
    private bool isGameRunning = false;  // 게임이 실행 중인지 여부
    private bool isGameStarted = false;  // 게임이 시작되었는지 여부

    // 손 오브젝트가 움직일 수 있는지 여부
    private bool canMove = false;

    void Start()
    {
        // 초기 목표 위치 설정
        SetInitialTargetPositions();

        //StartCoroutine(MovementRandomHand());
    }

    private void SetInitialTargetPositions()
    {
        for (int i = 0; i < leftHands.Length; i++)
        {
            //targetPositions[i] = new Vector3(leftHands[i].position.x, leftHands[i].position.y, isLeftHandMoving[i] ? minZ : maxZ);
            //targetPositions[i + 2] = new Vector3(rightHands[i].position.x, rightHands[i].position.y, isRightHandMoving[i] ? minZ : maxZ);

            targetPositions[i] = new Vector3(leftHands[i].position.x, leftHands[i].position.y, maxZ);
            targetPositions[i + 2] = new Vector3(rightHands[i].position.x, rightHands[i].position.y, maxZ);
        }
    }

    public void MoveHand(Transform hand, bool isMoving, ref Vector3 targetPosition, ref float lerpTime)
    {
        targetPosition = new Vector3(hand.position.x, hand.position.y, isMoving ? minZ : maxZ);

        lerpTime += Time.deltaTime * moveSpeed;
        hand.position = Vector3.Lerp(hand.position, targetPosition, lerpTime);

        if (Vector3.Distance(hand.position, targetPosition) < 0.01f)
        {
            hand.position = targetPosition;
            lerpTime = 0;
        }
    }

    void Update()
    {
        if (isGameRunning && isGameStarted && canMove)
        {
            for (int i = 0; i < leftHands.Length; i++)
            {
                MoveHand(leftHands[i], isLeftHandMoving[i], ref targetPositions[i], ref lerpTimes[i]);
                MoveHand(rightHands[i], isRightHandMoving[i], ref targetPositions[i + 2], ref lerpTimes[i + 2]);
            }
        }
    }

    public void StopHandMovement()
    {
        isGameRunning = false;
        ResetAllHands();
        StopCoroutine(MovementRandomHand());
    }

    public void StartGame()
    {
        isGameStarted = true;
        canMove = true;  // 손 오브젝트의 움직임을 허용
        isGameRunning = true;  // 게임을 실행 상태로 설정
        SetInitialTargetPositions();  // 초기 위치를 다시 설정
        StartCoroutine(MovementRandomHand());  // 손 움직임 시작
    }

    public void StopGame()
    {
        isGameStarted = false;
        StopHandMovement();  // 손 움직임을 멈추고 상태를 리셋
    }

    // 랜덤하게 상태를 변경하는 코루틴 
    public IEnumerator MovementRandomHand()
    {
        while (true)
        {
            float randomTime = Random.Range(1.5f, 2.0f);
            yield return new WaitForSeconds(randomTime);

            if (isGameRunning)
            {
                int randomHand = Random.Range(0, 4);
                ResetAllHands();

                switch (randomHand)
                {
                    case 0:
                        isLeftHandMoving[0] = true;
                        break;
                    case 1:
                        isLeftHandMoving[1] = true;
                        break;
                    case 2:
                        isRightHandMoving[0] = true;
                        break;
                    case 3:
                        isRightHandMoving[1] = true;
                        break;
                }
            }
        }
    }

    // 모든 왼손과 오른손 상태를 false로 설정
    public void ResetAllHands()
    {
        for (int i = 0; i < isLeftHandMoving.Length; i++)
        {
            isLeftHandMoving[i] = false;
        }
        for (int i = 0; i < isRightHandMoving.Length; i++)
        {
            isRightHandMoving[i] = false;
        }
    }
}
