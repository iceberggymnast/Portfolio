using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MiniGameTutorialMGR : MonoBehaviourPun
{
    
    public Button button;  // '준비 완료' Text 활성화 버튼

    // '준비 완료' 버튼 클릭시 활성화 시킬 Text들
    public GameObject player1_Readyset;
    public GameObject player2_Readyset;

    // 접속 맴버 2 명이 면 True가 되어 지정된 Scene이동을 한다.
    public bool player_Ready1;
    public bool player_Ready2;

    public GameObject []MiniGame; // 미니게임 관련 List
    public int questindex; // 퀘스트 관련 Index 값
    public int []MoveScene; // Scene 이동 관련 변수

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (QuestManager.questManager != null)
        {
            QuestMGR();
        }

        MiniGame[questindex].SetActive(true);

    }

    void Update()
    {
        if (player_Ready1 && player_Ready2 == true)
        {
            
            SceneMove();
            Debug.Log("인원이 다 와서 이동합니다.");
            player_Ready1 = false;
        }
        
    }

    // Scene 이동 관련 함수
    void SceneMove()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(MoveScene[questindex]);
        }
    }

    // '준비 완료' Text 출력 버튼
    public void onClick()
    {
        // 버튼의 인터렉션을 비활성화 해준다.
        button.interactable = false;

        // 접속한 클라이언트의 닉네임이 "해님"인지 확인하고
        if (PhotonNetwork.NickName == "해님")
        {
            // 해님이 맞으면 player_Ready1을 참으로 만든다. (RPC함수)
            photonView.RPC(nameof(Readyset), RpcTarget.All, 1);
        }
        else
        {
            // 달님일 경우 (== 해님이 아니면)player_Ready2를 참으로 만든다.
            photonView.RPC(nameof(Readyset), RpcTarget.All, 2);
        }

    }

    void QuestMGR()
    {
        //1. 3번 = 안마 게임        
        if (QuestManager.questManager.questList[3].questState == QuestData.QuestState.progress)
        {
            questindex = 0;
        }
        //2. 5번 = 호랑이 손 치기
        else if (QuestManager.questManager.questList[5].questState == QuestData.QuestState.progress)
        {
            questindex = 1;
        }
        //3. 8번 = 떡 던지기
        else if (QuestManager.questManager.questList[8].questState == QuestData.QuestState.progress)
        {
            questindex = 2;
        }
        //4. 9번 = 밧줄 올라가기
        else if (QuestManager.questManager.questList[9].questState == QuestData.QuestState.progress)
        {
            questindex = 3;
        }
    }

    
    // RPC + 접속 후 접속한 플레이어가 '준비 완료' 버튼 클릭 할 경우...
    // 해당 닉네임에 맞는 준비 완료 텍스트 활성화 한다.
    [PunRPC]
    void Readyset(int player_num)
    {

        if(player_num == 1)
        {
            player_Ready1 = true;
            player1_Readyset.SetActive(true);
        }
        else
        {
            player_Ready2 = true;
            player2_Readyset.SetActive(true);
        }
    }
    
}
