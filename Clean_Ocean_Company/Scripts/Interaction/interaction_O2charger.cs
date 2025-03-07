using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interaction_O2charger : MonoBehaviourPun
{
    int canChargeAmount;

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    void Start()
    {
        Interaction_Base interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = O2charger;

    }

    void Update()
    {

    }

    void O2charger()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        //산소 100퍼센트 채워줌
        PlayerInfo.instance.oxygen = PlayerInfo.instance.maxOxygen;

        //충전되었다는 이펙트 

        //시간 지내면 산소충전기 오브젝트 삭제됨
        photonView.RPC(nameof(removeO2Charge), RpcTarget.All);
        //Destroy(gameObject, 10.0f);

        //충전되었다는 이펙트 산소충전기에 표시해주고
        print("산소충전완료! 현재 산소량:" + PlayerInfo.instance.oxygen);

        GetComponent<Interaction_Base>().useTrue = false;
    }

    [PunRPC]
    //삭제 rpc
    public void removeO2Charge()
    {
        Destroy(gameObject, 10.0f);

    }
}
