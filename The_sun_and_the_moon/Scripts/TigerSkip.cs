using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerSkip : MonoBehaviourPun
{
    // J 버튼 누르면 스킵됨

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            photonView.RPC(nameof(Skip), RpcTarget.All);
        }
    }

    [PunRPC]
    void Skip()
    {
        QuestManager.questManager.QuestAddProgress(5, 0, 1);
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.LoadLevel(2); // 게임 종료시 HomeScene 이동
            PhotonNetwork.LoadLevel(2); // 게임 종료시 HomeScene 이동
        }
    }
}
