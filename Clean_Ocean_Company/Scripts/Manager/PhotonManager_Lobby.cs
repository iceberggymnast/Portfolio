using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager_Lobby : MonoBehaviourPunCallbacks
{
    public static PhotonManager_Lobby instance;

    private void Awake()
    {
        instance = this;        
    }

    public void CreateRoom()
    {
        // 방 옵션 설정 (예: 최대 플레이어 수 설정)
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;

        // 방 생성, 방 이름은 유니크하게 설정
        PhotonNetwork.CreateRoom("MyRoom", roomOptions, TypedLobby.Default);
    }

    // 방 생성 실패 시 호출되는 콜백 함수 (예: 이미 같은 이름의 방이 있을 경우)
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"방 생성 실패: {message}");
    }
}
