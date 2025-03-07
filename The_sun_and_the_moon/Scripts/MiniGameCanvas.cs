using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameCanvas : MonoBehaviour
{
    // 상단 프로필 이미지를 띄워줄 용도
    public List<GameObject> pic;
    public List<GameObject> text;

    // 플레이어를 인식할 인덱스
    public int playerNum;

    // 타이머
    public float timer;
    float time = 0;
    public Image timeBar;
    public TMP_Text timeText;

    // 승리 여부 감지
    public List<bool> sunMoonWin;

    // 팝업 이미지
    public List<GameObject> winPopup;

    // 성공, 실패 이미지
    public GameObject winObj;
    public GameObject failObj;

    // 씬 이동 카운트
    public float moveTime = 3;
    float moveTimer;

    // 진행도 올릴 퀘스트 index;
    public int questIndex;

    // 원하는 기능 넣어쓰기
    public Action action;

    // 스코어 바
    public Image socoreBar;
    public float socoreBarValue;
    bool loadScence;

    void Start()
    {
        timeText.text = timer.ToString() + " 초";

        // 달님인지 확인
        if (PhotonNetwork.NickName != "해님")
        {
            playerNum = 1;
        }

        // 인덱스를 적용 후 맞는 게임 오브젝트를 활성화 해준다.
        pic[playerNum].SetActive(true);
        text[playerNum].SetActive(true);

    }

    void Update()
    {
        time += Time.deltaTime;
        socoreBar.fillAmount = socoreBarValue;
        timeBar.fillAmount = 1 - (time / timer);

        if (timer < time)
        {
            // 시간 다 되면 실패..
            failObj.SetActive(true);
            moveTimer += Time.deltaTime;
            if (moveTimer > moveTime)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC(nameof(Clear), RpcTarget.All);
                }
            }
        }

        if (sunMoonWin[0] && sunMoonWin[1])
        {
            // 둘다 클리어
            winObj.SetActive(true);
            moveTimer += Time.deltaTime;
            if (moveTimer > moveTime)
            {
                PhotonView photonView = GetComponent<PhotonView>();
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC(nameof(Clear), RpcTarget.All);
                }
            }
        }

        // J키를 입력하면 자동으로 클리어 처리
        if (Input.GetKeyDown(KeyCode.J))
        {
            PhotonView photonView = GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(Clear), RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public IEnumerator WinPopup(int index)
    {
        sunMoonWin[index] = true;
        winPopup[index].SetActive(true);
        yield return new WaitForSeconds(3);
        winPopup[index].SetActive(false);
    }

    [PunRPC]
    public void Clear()
    {
        if (loadScence) return;
        loadScence = true;

        if (action != null)
        {
            action();
        }

        if (questIndex != 0)
        {
           QuestManager.questManager.QuestAddProgress(questIndex, 0, 1);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (SceneManager.GetActiveScene().buildIndex == 6)
            {
                PhotonNetwork.LoadLevel(9);
            }
            else
            {
                PhotonNetwork.LoadLevel(2);
            }
        }
    }
}
