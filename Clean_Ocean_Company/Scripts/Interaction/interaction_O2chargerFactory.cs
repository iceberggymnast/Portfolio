using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interaction_O2chargerFactory : MonoBehaviour
{
    // 한 번에 충전할 수 있는 최대 충전기 수는 3개
    int max_OxygenchargerAmount = 3;

    // 현재 충전 가능한 충전기 수
    int canChargeAmount;

    // 산소 충전기 구매 가능 여부 체크
    bool canBuy;

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    void Start()
    {
        Interaction_Base interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = O2chargerfactory;
    }

    void Update()
    {
        CheckBuyOxygenCharger();
    }

    void O2chargerfactory()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        // 구매 가능할 때만 실행
        if (canBuy)
        {
            // 포인트 차감
            PlayerInfo.instance.PointPlusOrMinus(-canChargeAmount * 10);

            // 산소 충전기를 현재 충전 가능한 만큼 추가
            PlayerInfo.instance.current_OxygenchargerAmount += canChargeAmount;

            //GetComponent<Interaction_Base>().useTrue = true;
            GetComponent<Interaction_Base>().intername = "산소 충전기 공급 완료";

            Debug.Log("산소충전기 " + canChargeAmount + "개 충전 완료");
        }
    }

    void CheckBuyOxygenCharger()
    {
        // 현재 플레이어의 산소 충전기 수 가져오기
        int current_OxygenchargerAmount = PlayerInfo.instance.current_OxygenchargerAmount;

        // 산소 충전기가 최대치 미만일 때만 구매 가능성 체크
        if (current_OxygenchargerAmount < max_OxygenchargerAmount)
        {
            // 충전 가능한 수량 및 가격 계산
            canChargeAmount = max_OxygenchargerAmount - current_OxygenchargerAmount;
            int chargerPrice = canChargeAmount * 10;

            // 포인트가 충분할 때
            if (PlayerInfo.instance.point >= chargerPrice)
            {
                canBuy = true;
                GetComponent<Interaction_Base>().useTrue = false;
                GetComponent<Interaction_Base>().intername = "산소 충전기 충전 가능";
            }
            // 포인트가 부족할 때
            else
            {
                canBuy = false;
                GetComponent<Interaction_Base>().useTrue = true;
                GetComponent<Interaction_Base>().intername = "포인트 부족으로 구매 불가";
            }
        }
        // 최대 수량을 이미 보유한 경우
        else
        {
            canBuy = false;
            GetComponent<Interaction_Base>().useTrue = true;
            GetComponent<Interaction_Base>().intername = "최대 산소충전기 보유 중";
        }
    }
}

