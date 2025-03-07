using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectionMGRMassage : MonoBehaviourPunCallbacks
{
    public StartButtonMassage startButtonScript;
    private const int MAX_PLAYERS = 2;
    private int readyPlayerCount = 0;  // 준비 완료한 플레이어 수를 추적하는 변수

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Master Server에 접속되었을 때 호출되는 Function
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        JoinLobby();
    }

    public void JoinLobby()
    {
        // Lobby에 바로 참가
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        JoinOrCreateRoom();
    }

    public void JoinOrCreateRoom()
    {
        RoomOptions roomOption = new RoomOptions
        {
            MaxPlayers = MAX_PLAYERS,
            IsVisible = true,
            IsOpen = true
        };

        PhotonNetwork.JoinOrCreateRoom("3rd_Project", roomOption, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        startButtonScript.UpdateRoomStatus(true);  // 플레이어가 입장했을 때 버튼 상태 업데이트
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        startButtonScript.UpdateRoomStatus(true);
    }

    // 플레이어가 준비 완료 버튼을 누를 때 호출될 메서드
    public void PlayerReady()
    {
        readyPlayerCount++;

        // 두 명의 플레이어가 모두 준비 완료했을 때
        if (readyPlayerCount == MAX_PLAYERS)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("Minigame_massage");
    }
}
