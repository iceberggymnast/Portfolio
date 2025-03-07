using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectionMGR : MonoBehaviourPunCallbacks
{
    
    public StartButton startButtonScript;
    private const int MAX_PLAYERS = 2;  // 최대 플레이어 수
    private int readyPlayerCount = 0;   // 준비 완료한 플레이어 수를 추적하는 변수

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

     

    }

    void Update()
    {
        // Tutorial Scene에 접속 시 현재 방에 있는 Player 수가 처음 설정했던 최대 Player수와 동일한 지 확인
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {

        }
    }
    

    // 플레이어가 준비 완료 버튼을 누를 때 호출될 메서드
    public void PlayerReady()
    {
        readyPlayerCount++;

        //// 준비완료 버튼을 Player 양 쪽이 전부 누른 상태인지?
        //if ()
        //{
        //    // 이동 하려는 미니게임 구분 해서 조건문 작성한다.
        //    if ()
        //    {

        //    }
        //}
    }


    void StartGame_Massage()
    {
        SceneManager.LoadScene("Minigame_massage");
    }

    void StartGame_TigerHand()
    {
        SceneManager.LoadScene("TigerHandScene");
    }

    void StartGame_Rope()
    {
        SceneManager.LoadScene("Minigame_tether");
    }

    
}
