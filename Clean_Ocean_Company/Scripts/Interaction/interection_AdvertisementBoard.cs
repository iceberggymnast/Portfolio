using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interection_AdvertisementBoard : MonoBehaviour
{
    public GameObject advertisementBoardUI;
    public BackButton_Lobby backButton_Lobby;

    void Start()
    {
        Interaction_Base interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = aaaaaa;
    }

    void Update()
    {
        if (backButton_Lobby.isClose)
        {
            //상호작용 다시 되게 해주기
            GetComponent<Interaction_Base>().useTrue = false;
        }
    }

    public void aaaaaa()
    {
        print("홍보게시판 강화 기계 상호작용 테스트");
        advertisementBoardUI.SetActive(true);
        PlayerInfo.instance.isCusor = true;
    }
}
