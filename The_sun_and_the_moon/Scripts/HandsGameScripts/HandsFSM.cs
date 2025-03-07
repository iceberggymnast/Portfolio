using System.Collections;
using UnityEngine;

// 문 밖에 대기, 문 안에 대기, 문 밖 -> 문 안으로 이동 and 문 안 -> 문 밖으로 이동
public enum HandsState { UnderGround = 0, OnGround, MoveUp, MoveDown }

// 손 종류 (호랑이 손 (점수 +) and (사람 손 (점수 -)))
public enum HandType { Normal = 0, Red }

public class HandsFSM : MonoBehaviour
{
    [SerializeField]
    private float waitTimeOnGround; // 문 안으로 나와서 문 밖으로 나가기까지 걸리는 시간
    [SerializeField]
    private float limitMinY; // 문 밖으로 나갈 수 있는 최소 y 위치
    [SerializeField]
    private float limitMaxY; // 문 안으로 들어올 수 있는 최대 y 위치

    private Movement3D movement3D; // 문 밖/안 이동을 위한 Movement3D
    private MeshRenderer meshRenderer; // Hand 의 색상 설정을 위한 MeshRenderer

    private HandType handType; // Hand 종류
    private Color defaultColor; // 기본 호랑이 손 색상(Yellow)

    // Hands의 현재 상태 
    public HandsState HandsState { private set; get; }

    // Hands의 종류 (HandType에 따라 Hand 색상 변경)
    public HandType HandType
    {
        set
        {
            handType = value;

            switch (handType)
            {
                case HandType.Normal:
                    meshRenderer.material.color = defaultColor;
                    break;
                case HandType.Red:
                    meshRenderer.material.color = Color.red;
                    break;
            }
        }
        get => handType;
    }

    // Hands가 배치되어 있는 순번 (왼 쪽 부터 0번)
    [field: SerializeField]
    public int HandsIndex { private set; get; }


    void Awake()
    {
        movement3D = GetComponent<Movement3D>();
        meshRenderer = GetComponent<MeshRenderer>();

        defaultColor = meshRenderer.material.color; // Hands의 최초 색상 저장

        ChangeState(HandsState.UnderGround);
    }

    public void ChangeState(HandsState newState)
    {
        // 열거형 변수를 ToString() Method를 이용해 문자열로 변환하면 "UnderGround"와 같이 열거형 요소 이름 변환

        // 이전에 재생 중이던 상태 종료
        StopCoroutine(HandsState.ToString());
        // 상태 변경
        HandsState = newState;
        // 새로운 상태 재생
        StartCoroutine(HandsState.ToString());
    }

    // 호랑이가 문 밖에서 대기하는 상태로 최초의 Ground 위치로 호랑이 위치 설정
    IEnumerator UnderGround()
    {
        movement3D.MoveTo(Vector3.zero);
        // 호랑이의 위치를 문 밖에 있는 limitMinY 위치로 설정
        transform.position = new Vector3(transform.position.x, limitMinY, transform.position.z);

        yield return null;
    }

    // 호랑이가 문 안으로 들어오는 상태로 waitTimeOnGround동안 대기
    IEnumerator OnGround()
    {
        movement3D.MoveTo(Vector3.zero);
        // 호랑이의 위치를 문 안에 있는 limitMaxY 위치로 설정
        transform.position = new Vector3(transform.position.x, limitMaxY, transform.position.z);

        // waitTimeOnGround 시간 동안 대기
        yield return new WaitForSeconds(waitTimeOnGround);

        // 호랑이의 상태를 MoveDown으로 변경
        ChangeState(HandsState.MoveDown);
    }

    // 호랑이가 문 안으로 들어오는 상태 (maxYPosOnGround 위치까지 이동)
    IEnumerator MoveUp()
    {
        movement3D.MoveTo(Vector3.up);

        while (true)
        {
            // If, 호랑이의 Y위치가 limitMaxY 이면 상태 변경
            if (transform.position.y >= limitMaxY)
            {
                // OnGround 상태로 변경
                ChangeState(HandsState.OnGround);
            }
            yield return null;
        }
    }

    // 호랑이가 문 밖으로 나가는 상태 (minYPosOnGround 위치까지 이동)
    IEnumerator MoveDown()
    {
        movement3D.MoveTo(Vector3.down);

        while (true)
        {
            // If, 호랑이의 Y위치가 limitMinY 이면 상태이면 반복문 중지
            if (transform.position.y <= limitMinY)
            {
                // UnderGround 상태로 변경
                ChangeState(HandsState.UnderGround);
            }
            yield return null;
        }
    }
}