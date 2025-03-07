using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerPunchList2 : MonoBehaviour
{
    [SerializeField]
    private float maxY; // Hand의 최대 y 위치
    [SerializeField]
    private float minY; // Hand의 최소 y 위치

    [Serializable]
    public class TigerPunch
    {
        public string result;
    }

    TigerPunch tigerPunch;

    [SerializeField]
    private GameController gameController; // Score 증가를 위한 GameController

    [SerializeField]
    private ObjectDetector ObjectDetector; // Mouse 클릭으로 Object 선택을 위한 ObjectDetector
    private Movement3D movement3D; // Hand Object 이동을 위한 Movement
    
    [SerializeField]
    private void Start()
    {
        tigerPunch = new TigerPunch();
        tigerPunch.result = "test";
    }

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();

        // OnHit Method를 ObjectDetector Class의 raycastEvent에 Event로 등록
        // ObjectDetector의 raycastEvent.Invoke(hit.transform); Method가 호출될 때마다 OnHit(Transform target) Method가 호출된다.
        ObjectDetector.raycastEvent.AddListener(OnHit);
    }

    public void CreateFromJSON(string jsonString)
    {

        tigerPunch = JsonUtility.FromJson<TigerPunch>(jsonString);
        if (tigerPunch.result != "")
        {
            print(tigerPunch.result);
            if (tigerPunch.result == "LEFT")
            {
                print("Left Punch");
            }
            else if (tigerPunch.result == "RIGHT")
            {
                print("Right Punch");
            }
        }
    }

    void OnHit(Transform target)
    {
        if (target.CompareTag("Hand"))
        {
            //TigerFSM tiger = target.GetComponent<TigerFSM>();
            HandsFSM hands = target.GetComponent<HandsFSM>();

            // 호랑이가 문 밖에 있을 경우 공격 불가능
            if (hands.HandsState == HandsState.UnderGround) return;

            // Hand의 위치 설정
            transform.position = new Vector3(target.position.x, minY, transform.position.z);

            // Hand에 맞았기 때문에 Tiger의 상태를 바로 UnderGround로 Setting
            hands.ChangeState(HandsState.UnderGround);

            // Camera 흔들기
            ShakeCamera.Instance.OnShakeCamera(0.1f, 0.1f);

            // Score 증가 (+1)
            //gameController.Score += 1;

            // Hands 색상에 따라 점수 처리
            HandsHitProcess(hands);

            // Hand를 위로 이동시키는 Coroutine 재생
            StartCoroutine("MoveUp");
        }
    }

    IEnumerator MoveUp()
    {
        movement3D.MoveTo(Vector3.up);

        while (true)
        {
            if (transform.position.y >= maxY)
            {
                movement3D.MoveTo(Vector3.zero);
                break;
            }
            yield return null;
        }
    }

    // 호랑이 손을 때릴 경우 점수 1점 증가 but 사람 손을 때릴 경우 점수 3점 감소
    void HandsHitProcess(HandsFSM hands)
    {
        if (hands.HandType == HandType.Normal)
        {
            gameController.Score += 1;
        }
        else if (hands.HandType == HandType.Red)
        {
            gameController.Score -= 3;
        }
    }

    
}
