using System.Collections;
using UnityEngine;

public class Hands : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private TigerPunchList tigerPunchList;  // TigerPunchList 연결

    private Movement3D movement3D;

    private void Awake()
    {
        movement3D = GetComponent<Movement3D>();
    }

    void Update()
    {
        // tigerPunchList에서 결과를 가져옴
        string tigerPunchResult = tigerPunchList.GetTigerPunchResult();

        // "LEFT" 또는 "RIGHT"가 감지될 때 OnHit 실행
        if (tigerPunchResult == "LEFT" || tigerPunchResult == "RIGHT")
        {
            // "TigerHands"에 해당하는 타겟을 지정하고 OnHit 호출
            Transform target = FindTigerHand();  // 타겟을 찾는 함수 구현 필요
            if (target != null)
            {
                OnHit(target);
            }
        }
    }

    void OnHit(Transform target)
    {
        if (target.CompareTag("TigerHands"))
        {
            HandsFSM hands = target.GetComponent<HandsFSM>();
            if (hands.HandsState == HandsState.UnderGround) return;

            transform.position = new Vector3(target.position.x, 0, transform.position.z);
            hands.ChangeState(HandsState.UnderGround);
            HandsHitProcess(hands);
        }
    }

    void HandsHitProcess(HandsFSM hands)
    {
        if (hands.HandType == HandType.Normal)
        {
            gameController.NormalHandCount++;
            gameController.Score += 1;
        }
        else if (hands.HandType == HandType.Red)
        {
            gameController.RedHandCount++;
            gameController.Score -= 3;
        }
    }

    Transform FindTigerHand()
    {
        // "TigerHands" 태그가 붙은 손을 찾아 반환하는 로직 구현
        GameObject tigerHand = GameObject.FindGameObjectWithTag("TigerHands");
        if (tigerHand != null)
        {
            return tigerHand.transform;
        }
        return null;
    }
}
