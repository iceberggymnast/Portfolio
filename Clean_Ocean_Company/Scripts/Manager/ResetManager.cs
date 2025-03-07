using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ResetManager : MonoBehaviourPun
{
    public static ResetManager instance;
    public GameObject cheatCanvas;
    public Image markImage;

    public GameObject player;

    public PlayerMove playerMove;

    public GameObject posParent;

    public Transform safetyPos;
    public Transform contaminatedPos;
    public Transform dangerousPos;

    public List<Button> buttonList;

    public PhotonView pv;

    public GameObject mediapipePorpoiseCanvas;

    MediapipePorpoise mediapipePorpoise;

    MediapipeThirdManager mediapipeThirdManager;

    public Interaction_UICamera_Controller interaction_UICamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    IEnumerator Start()
    {
        if (pv.IsMine)
        {
            mediapipePorpoiseCanvas = GameObject.Find("Mediapipe3DCanvas");

            mediapipePorpoise = GameObject.FindObjectOfType<MediapipePorpoise>();

            yield return new WaitUntil(() => mediapipePorpoise != null);

            mediapipeThirdManager = mediapipePorpoise.mediapipeThirdManager;

            posParent = GameObject.Find("PosParent");

            if (posParent != null)
            {
                safetyPos = posParent.transform.GetChild(0);
                contaminatedPos = posParent.transform.GetChild(1);
                dangerousPos = posParent.transform.GetChild(2);
            }




            GameObject porpoise = GameObject.Find("Porpoise");

            if (porpoise != null)
            {
                markImage = porpoise.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();

                buttonList[0].onClick.AddListener(() => MoveContaminatedAreaButtonOnclick(player));
                buttonList[1].onClick.AddListener(() => MoveDangerousAreaButtonOnclick(player));
                buttonList[2].onClick.AddListener(() => MoveSafetyAreaButtonOnclick(player));
            }

        }

        yield return null;
    }






    public void OnOffCheatUI()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            cheatCanvas.SetActive(!cheatCanvas.activeSelf);
            if (PlayerInfo.instance != null)
            {
                PlayerInfo.instance.isCusor = cheatCanvas.activeSelf;
            }
        }
    }


    #region 지금 사용 안함
    public void RestartEvent()
    {
        if (PlayerInfo.instance != null)
        {
            PlayerInfo.instance.isCusor = true;
        }
        PhotonNetwork.LoadLevel("StartScene");
    }

    

    public void MoveMainScene()
    {
        if (PlayerInfo.instance != null)
        {
            PlayerInfo.instance.isCusor = false;
        }
        PhotonNetwork.LoadLevel("TestBuildPlayerScene_Yoon");
    }

    public void MoveLobbyScene()
    {
        if (PlayerInfo.instance != null)
        {
            PlayerInfo.instance.isCusor = true;
        }
        //PhotonNetwork.LoadLevel("LobbyScene");
        PhotonNetwork.LoadLevel("LobbyScene_1024");
        PlayerInfo.instance.isCusor = false;
    }

    #endregion

    public void QuestCheat()
    {
        if (QuestManger.instance != null)
        {

            QuestManger.instance.questDatas[0].questCount = 200;
            QuestUI questUI = FindAnyObjectByType<QuestUI>();
            
            QuestManger.instance.QuestSetState(0, QuestState.CanCompleted);

            questUI.QuestUISet();

            QuestManger.instance.QuestSetMark(markImage, QuestManger.instance.questDatas[0].questState);


            QuestManger.instance.QuestSetState(0, QuestState.Completed);

            PopupUI popupUI = GameObject.FindObjectOfType<PopupUI>();
            popupUI.PopupActive($"퀘스트 완료<br><size=50><color=#00FFFF>{QuestManger.instance.questDatas[QuestManger.instance.QuestFind(0)].questName}</color></size>", "", 1.5f);
            PlayerInfo.instance.PointPlusOrMinus(100);

            if (mediapipeThirdManager != null && mediapipeThirdManager.gameObject.activeSelf)
            {
                mediapipeThirdManager.Event_FinishMediapipe();
            }
            if (mediapipePorpoise != null)
            {
                mediapipePorpoise.SetPorpoiseColor(true);
            }

            interaction_UICamera.IsStart = true;

            StartCoroutine(UIController.instance.FadeIn("Canvas_TrashCan", 0.5f));
            StartCoroutine(UIController.instance.FadeIn("ChatCanvas", 0.5f));
            StartCoroutine(UIController.instance.FadeIn("Canvas_Player", 0.5f));
            StartCoroutine(UIController.instance.FadeIn("Canvas_MainUI", 0.5f));
            StartCoroutine(UIController.instance.FadeIn("InteractionCanvas", 0.5f));
        }
    }

    public void MoveContaminatedAreaButtonOnclick(GameObject player)
    {
        print("오염지대 클릭");
        playerMove.isMove = false;
        player.transform.position = contaminatedPos.position;
        StartCoroutine(ResumeMovement());
    }

    public void MoveSafetyAreaButtonOnclick(GameObject player)
    {
        print("안전지대 클릭");
        playerMove.isMove = false;
        player.transform.position = safetyPos.position;
        StartCoroutine(ResumeMovement());
    }

    public void MoveDangerousAreaButtonOnclick(GameObject player)
    {
        print("위험지대 클릭");
        playerMove.isMove = false;
        player.transform.position = dangerousPos.position;
        StartCoroutine(ResumeMovement());
    }


    private IEnumerator ResumeMovement()
    {
        yield return new WaitForSeconds(0.1f);
        playerMove.isMove = true;
    }


    private void Update()
    {
        if (pv.IsMine)
        {
            OnOffCheatUI();
        }
    }
}
