using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonOffline : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버 연결 성공");

        // 로비 입장
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        print("로비 입장!");

        // 방 생성 또는 입장 시도
        PhotonNetwork.JoinRoom("Test");
    }

}
