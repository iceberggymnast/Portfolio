using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_Porpoise : MonoBehaviour
{
    string testCSV;
    
    public int csvDataIndex = 0;
    public int dialogueID;
    public Image markImage;
    public GameObject player;

    SpeechBubble mySpeechBubble;
    Interaction_Base interaction_Base;
    PopupUI popupUI;
    Transform dir;
    Transform pos;

    public Transform cameraPos;
    public Transform playerPos;

    public QuestUI questUI;

    private List<DialogueInfo> csvDatas = new List<DialogueInfo>();

    public CameraRotate cameraRotate;

    public PlayerMove playerMove;

    GameObject mainCamera;
    GameObject uiCamera;

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    private void Awake()
    {
        mySpeechBubble = transform.GetComponentInChildren<SpeechBubble>();
        questUI = GameObject.FindObjectOfType<QuestUI>();
        popupUI = GameObject.FindObjectOfType<PopupUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = StartDialogueEvent;

        // `Interaction_Base`의 OnPlayerChanged 이벤트 구독
        interaction_Base.OnPlayerChanged += UpdatePlayerReference;

        // CSV 데이터 파싱 및 저장
        var dialogueDictionary = CSVManager.Get().Parse("Porpoise");
        if (dialogueDictionary.ContainsKey(dialogueID.ToString())) // 예: dialogueID가 "1"인 대화들
        {
            csvDatas = dialogueDictionary[dialogueID.ToString()];
        }
        QuestManger.instance.QuestSetMark(markImage, QuestManger.instance.questDatas[0].questState);
        mySpeechBubble.AddCSVDataEvnt += AddCSVData;
    }

    


    private void Update()
    {
        if(mySpeechBubble.buttonParent.activeSelf) ChatManager.Instance.isStartChat = true;
        if (mySpeechBubble.selectButtonList.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                AcceptButton();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                RefuseButton();
            }
        }
        if (mySpeechBubble.nextButton.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextButton();
            }
        }
        if (mySpeechBubble.finishButton.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FinishButton();
            }
        }
    }


    // `Interaction_Base`의 player가 변경될 때 호출되는 이벤트 핸들러
    private void UpdatePlayerReference(GameObject newPlayer)
    {
        player = newPlayer;
        cameraRotate = player.transform.parent.GetComponentInChildren<CameraRotate>();
        mainCamera = cameraRotate.mainCamera;
        uiCamera = cameraRotate.uiCamera;
        playerMove = player.GetComponent<PlayerMove>();
        pos = player.transform.parent.GetChild(2).transform;
    }


    void StartDialogueEvent()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        cameraRotate.useRotX = false;
        cameraRotate.useRotY = false;
        playerMove.isMove = false;

        PlayerInfo.instance.isCusor = true;

        if (csvDataIndex >= csvDatas.Count) return;

        var currentData = csvDatas[csvDataIndex];



        StartCoroutine(UIController.instance.FadeOut("ChatCanvas", 0.15f));
        StartCoroutine(UIController.instance.FadeOut("OxygenUI", 0.15f));
        StartCoroutine(UIController.instance.FadeOut("Canvas_TrashCan", 0.15f));
        StartCoroutine(UIController.instance.FadeOut("Canvas_Player", 0.15f));


        mySpeechBubble.gameObject.SetActive(true);
        mySpeechBubble.PlaySpeechBubbleEvent(currentData.name, currentData.conversation, currentData.isFix, currentData.eventType, csvDataIndex, csvDatas, pos);


        csvDataIndex++;

        PlayerInfo.instance.player.transform.position = playerPos.transform.position;


        PlayerInfo.instance.player.transform.GetChild(0).LookAt(transform.position);

        

        Vector3 playerToPorpoise = new Vector3((PlayerInfo.instance.player.transform.position.x + transform.position.x) / 2,
                                               (PlayerInfo.instance.player.transform.position.y + transform.position.y) / 2 + 0.5f,
                                               (PlayerInfo.instance.player.transform.position.z + transform.position.z) / 2);

        interaction_Base.useTrue = true;

        

        

        mainCamera.transform.position = cameraPos.position;
        uiCamera.transform.position = cameraPos.position;

        mainCamera.transform.LookAt(playerToPorpoise);
        uiCamera.transform.LookAt(playerToPorpoise);
    }


    public void NextButton()
    {
        StartCoroutine(INextButton());
    }

    IEnumerator INextButton()
    {
        yield return null;



        if (csvDataIndex >= csvDatas.Count) yield break;



        var currentData = csvDatas[csvDataIndex];


        

        Debug.Log("다음");
        Debug.Log($"다음 csvDatas : {csvDataIndex}");

        mySpeechBubble.PlaySpeechBubbleEvent(currentData.name, currentData.conversation, currentData.isFix, currentData.eventType, csvDataIndex, csvDatas, pos);



    }

    void AddCSVData(int value)
    {
        csvDataIndex = value + 1;
    }


    public void AcceptButton()
    {
        var currentData = csvDatas[ParseNextIndex(true)];

        csvDataIndex = ParseNextIndex(true);


        mySpeechBubble.PlaySpeechBubbleAccept(currentData.name, currentData.conversation, currentData.isFix, currentData.eventType, csvDataIndex, pos);

        
    }

    public void RefuseButton()
    {
        var currentData = csvDatas[ParseNextIndex(false)];

        csvDataIndex = ParseNextIndex(false);

        mySpeechBubble.PlaySpeechBubbleRefuse(currentData.name, currentData.conversation, currentData.isFix, currentData.eventType, csvDataIndex, pos);

    }

    public void FinishButton()
    {
        mySpeechBubble.PlaySpeechBubbleFinish();

        csvDataIndex = 0;

        mySpeechBubble.buttonEvent += FinishSpeech;
    }

    void FinishSpeech()
    {
        popupUI.PopupActive(QuestManger.instance.questDatas[0].questName, "", 1.5f);
        QuestManger.instance.QuestSetMark(markImage, QuestManger.instance.questDatas[0].questState);

        cameraRotate.useRotX = true;
        cameraRotate.useRotY = true;
        playerMove.isMove = true;

        mainCamera.transform.rotation = new Quaternion(0, 0, 0, 0);
        uiCamera.transform.rotation = new Quaternion(0, 0, 0, 0);

        mainCamera.transform.localPosition = new Vector3(1.5f, 1.2f, -5f);
        uiCamera.transform.localPosition = new Vector3(1.5f, 1.2f, -5f);


        interaction_Base.useTrue = false;

        ChatManager.Instance.isStartChat = false;

        StartCoroutine(UIController.instance.FadeIn("ChatCanvas", 0.4f));
        StartCoroutine(UIController.instance.FadeIn("OxygenUI", 0.4f));
        StartCoroutine(UIController.instance.FadeIn("Canvas_TrashCan", 0.4f));
        StartCoroutine(UIController.instance.FadeIn("Canvas_Player", 0.15f));
    }

    private int ParseNextIndex(bool isAccepted)
    {
        int nextIndex = csvDataIndex; // 기본 이동: 한 칸 아래로 이동

        if (isAccepted == true)
        {
            nextIndex = csvDataIndex; // 수락 시 한 칸 아래로 이동
        }
        else if (isAccepted == false)
        {
            nextIndex = csvDataIndex + 1; // 거절 시 두 칸 아래로 이동
        }

        // 인덱스가 유효한 범위 내에 있는지 확인
        if (nextIndex >= csvDatas.Count) nextIndex = csvDatas.Count - 1;

        return nextIndex;
    }

}
