using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    // 스트링 값을 받아서 대화 말풍선을 띄워주는 기능을 합니다.
    public GameObject bubbleOBJ;

    public GameObject briefingUI;

    // 자녀 오브젝트들
    public Image bubble;
    public TMP_Text tmp_text_npc;
    public TMP_Text tmp_text_conversation;

    public GameObject buttonParent;

    public GameObject selectButtonList;
    public GameObject nextButton;
    public GameObject finishButton;

    public Action buttonEvent;
    public event Action<int> AddCSVDataEvnt;

    

    public Transform myTransform;
    Transform playerTransform;

    private Coroutine ownCoroutine;


    public NPCSpeaker npcSpeaker;


    public enum SpeechType
    {
        Chatting,
        Quest
    }

    public SpeechType speechType = SpeechType.Quest;

    AudioSource audioSource;

    private void Awake()
    {
        buttonParent = GameObject.FindWithTag("ButtonParent");
        selectButtonList = buttonParent.transform.Find("SelectButtonList").gameObject;
        nextButton = buttonParent.transform.Find("NextButton").gameObject;
        finishButton = buttonParent.transform.Find("FinishButton").gameObject;

        audioSource = npcSpeaker.audioSource;

        if (speechType == SpeechType.Quest)
        {
            myTransform = transform.parent;
        }
    }

    private void Start()
    {
        if (buttonParent == null) return;
        selectButtonList.SetActive(false);
        nextButton.SetActive(false);
        finishButton.SetActive(false);
        buttonParent.SetActive(false);
        this.gameObject.SetActive(false);
        //briefingUI = GameObject.Find("BriefingUI");
    }

    //private void Update()
    //{
    //    if (speechType == SpeechType.Quest)
    //    {
    //        transform.LookAt(Camera.main.transform.position);
    //        transform.eulerAngles = new Vector3(transform.eulerAngles.x * -1, transform.eulerAngles.y + 180.0f, transform.eulerAngles.z);
    //    }
    //}

    [Button]
    public Coroutine PlaySpeechBubble(string npcName, string conversation, bool isFix, Transform pos)
    {
        transform.position = pos.position;
        Coroutine co = StartCoroutine(Play(npcName, conversation, isFix));
        return co;
    }
    

    public Coroutine PlaySpeechBubble(string conversation, bool isFix, Transform pos)
    {
        transform.position = pos.position;
        Coroutine co = StartCoroutine(Play(conversation, isFix));
        return co;
    }

    public Coroutine PlaySpeechBubble(string conversation, Coroutine speechCoroutine)
    {
        Coroutine co = StartCoroutine(PlayOwn(conversation, speechCoroutine));
        print("5");
        return co;
    }

    public Coroutine PlaySpeechBubbleEvent(string npcName, string conversation, bool isFix, string eventType, int csvDataIndex, List<DialogueInfo> csvDatas, Transform playerPos)
    {
        if (npcName == "Player")
        {
            playerTransform = playerPos;
            transform.parent.position = playerTransform.position;
        }
        else if (npcName != "Player")
        {
            transform.parent.position = myTransform.position;
        }

        Coroutine co = StartCoroutine(PlayTest(npcName, conversation, isFix, eventType, csvDataIndex, csvDatas, playerPos));
        return co;
    }



    public Coroutine PlaySpeechBubbleAccept(string npcName, string conversation, bool isFix, string eventType, int csvDataIndex, Transform playerPos)
    {
        if (npcName == "Player")
        {
            playerTransform = playerPos;
            transform.parent.position = playerTransform.position;
        }
        else if (npcName != "Player")
        {
            transform.parent.position = myTransform.position;
        }


        Coroutine co = StartCoroutine(PlayAceept(npcName, conversation, isFix, eventType, csvDataIndex));
        return co;
    }

    public Coroutine PlaySpeechBubbleRefuse(string npcName, string conversation, bool isFix, string eventType, int csvDataIndex, Transform playerPos)
    {
        if (npcName == "Player")
        {
            playerTransform = playerPos;
            transform.parent.position = playerTransform.position;
        }
        else if (npcName != "Player")
        {
            transform.parent.position = myTransform.position;
        }


        Coroutine co = StartCoroutine(PlayRefuse(npcName, conversation, isFix, eventType, csvDataIndex));
        return co;
    }


    public Coroutine PlaySpeechBubbleFinish()
    {
        Coroutine co = StartCoroutine(PlayFinish());
        return co;
    }

    IEnumerator PlayTest(string npcName, string conversation, bool isFix, string eventType, int csvDataIndex, List<DialogueInfo> csvDatas, Transform playerPos)
    {
        npcSpeaker.PlayAudioClip(csvDataIndex);

        bubbleOBJ.SetActive(true);
        tmp_text_npc.text = npcName;
        tmp_text_conversation.text = conversation;

        float size = tmp_text_conversation.preferredHeight;

        float tagetSize = 0;

        RectTransform img = bubble.GetComponent<RectTransform>();


        float value = (size / 48.4f);

        if (value < 1.1f)
        {
            tagetSize = img.sizeDelta.y;
        }
        else if (value >= 1.1f)
        {
            value -= 1;
            tagetSize = (size * value) + img.sizeDelta.y;
        }
        else if(value >= 3)
        {
            tagetSize = (size * value) + img.sizeDelta.y;
        }

        img.sizeDelta = new Vector3(img.sizeDelta.x, 0);

        Vector3 targetsizeV3 = new Vector3(img.sizeDelta.x, tagetSize);
        while (img.sizeDelta.y < targetsizeV3.y - 0.1f)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, targetsizeV3, Time.deltaTime * 14.0f);
            yield return null;
        }

        if (!isFix)
        {
            if (eventType == "0")
            {
                yield return new WaitForSeconds(3);

                AddCSVDataEvnt(csvDataIndex);

                csvDataIndex++;
                if (csvDataIndex < csvDatas.Count)
                {
                    var nextData = csvDatas[csvDataIndex];
                    PlaySpeechBubbleEvent(nextData.name, nextData.conversation, nextData.isFix, nextData.eventType, csvDataIndex, csvDatas, playerPos);
                }
                yield break;
            }

            yield return new WaitForSeconds(3);


            tmp_text_npc.text = "";
            tmp_text_conversation.text = "";

            while (img.sizeDelta.y > 130)
            {
                img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
                yield return null;
            }

            bubbleOBJ.SetActive(false);
        }
        else
        {
            AddCSVDataEvnt(csvDataIndex);
            switch (eventType)
            {
                case "0":
                    buttonParent.SetActive(true);
                    nextButton.SetActive(true);
                    break;
                case "1":
                    nextButton.SetActive(false);
                    selectButtonList.SetActive(true);
                    break;
                case "2":
                    selectButtonList.SetActive(false);
                    finishButton.SetActive(true);
                    break;
            }
        }
    }



    IEnumerator PlayAceept(string npcName, string conversation, bool isFix, string eventType, int csvDataIndex)
    {
        npcSpeaker.PlayAudioClip(csvDataIndex);

        // 말풍선을 활성화 해주고 값을 초기화
        bubbleOBJ.SetActive(true);
        tmp_text_npc.text = npcName;
        tmp_text_conversation.text = conversation;
        // 텍스트 사이즈 체크 후 약간의 애니메이션
        float size = tmp_text_conversation.preferredHeight;

        QuestManger.instance.QuestSetState(0, QuestState.Accepting);

        float tagetSize = 0;

        RectTransform img = bubble.GetComponent<RectTransform>();


        float value = (size / 48.4f);

        if (value < 1.1f)
        {
            tagetSize = img.sizeDelta.y;
        }
        else if (value >= 1.1f)
        {
            value -= 1;
            tagetSize = (size * value) + img.sizeDelta.y;
        }
        else if (value >= 3)
        {
            tagetSize = (size * value) + img.sizeDelta.y;
        }

        // 약간의 애니메이션
        Vector3 targetsizeV3 = new Vector3(img.sizeDelta.x, tagetSize);
        while (img.sizeDelta.y < targetsizeV3.y - 0.1f)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, targetsizeV3, Time.deltaTime * 14.0f);
            yield return null;
        }

        // 텍스트 설정
        tmp_text_npc.text = npcName;
        tmp_text_conversation.text = conversation;

        if (isFix)
        {
            tmp_text_conversation.text += " <size=3><bounce a=0.00003><b>↵</b></bounce></size>";
        }

        // 대화냐 아니냐에 따라 갈림
        if (!isFix)
        {
            yield return new WaitForSeconds(3);

            // 텍스트 초기화
            tmp_text_npc.text = "";
            tmp_text_conversation.text = "";

            // 애니메이션 효과 나오면서 닫힘
            while (img.sizeDelta.y > 130)
            {
                img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
                yield return null;
            }

            bubbleOBJ.SetActive(false);
        }
        else
        {

            switch (eventType)
            {
                case "0":
                    buttonParent.SetActive(true);
                    nextButton.SetActive(true);
                    break;
                case "1":
                    nextButton.SetActive(false);
                    selectButtonList.SetActive(true);
                    break;
                case "2":
                    selectButtonList.SetActive(false);
                    finishButton.SetActive(true);
                    break;
            }

        }

    }

    IEnumerator PlayRefuse(string npcName, string conversation, bool isFix, string eventType, int csvDataIndex)
    {
        npcSpeaker.PlayAudioClip(csvDataIndex);

        // 말풍선을 활성화 해주고 값을 초기화
        bubbleOBJ.SetActive(true);
        tmp_text_npc.text = "";
        tmp_text_conversation.text = "";
        // 텍스트 사이즈 체크 후 약간의 애니메이션
        float size = tmp_text_conversation.preferredHeight;


        // 말풍선은 작게 잡아주기
        float tagetSize = 0;

        RectTransform img = bubble.GetComponent<RectTransform>();


        float value = (size / 48.4f);

        if (value < 1.1f)
        {
            tagetSize = img.sizeDelta.y;
        }
        else if (value >= 1.1f)
        {
            value -= 1;
            tagetSize = (size * value) + img.sizeDelta.y;
        }
        else if (value >= 3)
        {
            tagetSize = (size * value) + img.sizeDelta.y;
        }

        // 약간의 애니메이션
        Vector3 targetsizeV3 = new Vector3(img.sizeDelta.x, tagetSize);
        while (img.sizeDelta.y < targetsizeV3.y - 0.1f)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, targetsizeV3, Time.deltaTime * 14.0f);
            yield return null;
        }

        // 텍스트 설정
        tmp_text_npc.text = npcName;
        tmp_text_conversation.text = conversation;

        if (isFix)
        {
            tmp_text_conversation.text += " <size=3><bounce a=0.00003><b>↵</b></bounce></size>";
        }

        // 대화냐 아니냐에 따라 갈림
        if (!isFix)
        {
            yield return new WaitForSeconds(3);

            // 텍스트 초기화
            tmp_text_npc.text = "";
            tmp_text_conversation.text = "";

            // 애니메이션 효과 나오면서 닫힘
            while (img.sizeDelta.y > 130)
            {
                img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
                yield return null;
            }

            bubbleOBJ.SetActive(false);
        }
        else
        {

            switch (eventType)
            {
                case "0":
                    buttonParent.SetActive(true);
                    nextButton.SetActive(true);
                    break;
                case "1":
                    nextButton.SetActive(false);
                    selectButtonList.SetActive(true);
                    break;
                case "2":
                    selectButtonList.SetActive(false);
                    finishButton.SetActive(true);
                    break;
            }
        }

    }

    IEnumerator PlayFinish()
    {
        RectTransform con = tmp_text_conversation.gameObject.GetComponent<RectTransform>();

        // 말풍선은 작게 잡아주기
        RectTransform img = bubble.GetComponent<RectTransform>();


        yield return null;

        // 텍스트 초기화
        tmp_text_npc.text = "";
        tmp_text_conversation.text = "";

        Vector2 dir = new Vector2(img.sizeDelta.x, img.sizeDelta.y);

        // 애니메이션 효과 나오면서 닫힘
        while (img.sizeDelta.y > 130)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
            yield return null;
        }

        img.sizeDelta = dir;

        bubbleOBJ.SetActive(false);
        finishButton.SetActive(false);
        buttonParent.SetActive(false);

        PlayerInfo.instance.isCusor = false;

        buttonEvent();
    }

    IEnumerator Play(string npcName, string conversation, bool isFix)
    {
        // 말풍선을 활성화 해주고 값을 초기화
        bubbleOBJ.SetActive(true);
        tmp_text_npc.text = "";
        tmp_text_conversation.text = conversation;
        // 텍스트 사이즈 체크 후 약간의 애니메이션
        float size = tmp_text_conversation.preferredHeight;
        print(size);
        tmp_text_conversation.text = "";


        // 높이 잡아주기 
        RectTransform name = tmp_text_npc.gameObject.GetComponent<RectTransform>();
        name.sizeDelta = new Vector3(name.sizeDelta.x, size + 6);

        RectTransform con = tmp_text_conversation.gameObject.GetComponent<RectTransform>();
        con.sizeDelta = new Vector3(con.sizeDelta.x, size);

        // 말풍선은 작게 잡아주기
        RectTransform img = bubble.GetComponent<RectTransform>();
        float tagetSize = (size + 19) * 10;
        img.sizeDelta = new Vector3(img.sizeDelta.x, 0);
        
        // 약간의 애니메이션
        Vector3 targetsizeV3 = new Vector3(img.sizeDelta.x, tagetSize);
        while (img.sizeDelta.y < targetsizeV3.y - 0.1f)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, targetsizeV3, Time.deltaTime * 20.0f);
            yield return null;
        }

        // 텍스트 설정
        tmp_text_npc.text = npcName;
        tmp_text_conversation.text = conversation;
        
        if (isFix)
        {
            tmp_text_conversation.text += " <size=3><bounce a=0.00003><b>↵</b></bounce></size>";
        }

        // 대화냐 아니냐에 따라 갈림
        if (!isFix)
        {
            yield return new WaitForSeconds(3);

            // 텍스트 초기화
            tmp_text_npc.text = "";
            tmp_text_conversation.text = "";

            // 애니메이션 효과 나오면서 닫힘
            while (img.sizeDelta.y > 130)
            {
                img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
                yield return null;
            }

            bubbleOBJ.SetActive(false);
        }
        else
        {

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            // 텍스트 초기화
            tmp_text_npc.text = "";
            tmp_text_conversation.text = "";

            // 애니메이션 효과 나오면서 닫힘
            while (img.sizeDelta.y > 130)
            {
                img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
                yield return null;
            }

            bubbleOBJ.SetActive(false);

        }

    } 

    public IEnumerator PlayOwn(string conversation, Coroutine speechCoroutine)
    {
        print("6");
        // 말풍선을 활성화 해주고 값을 초기화
        bubbleOBJ.SetActive(true);
        tmp_text_npc.text = "";
        tmp_text_conversation.text = conversation;
        // 텍스트 사이즈 체크 후 약간의 애니메이션
        float size = tmp_text_conversation.preferredHeight;
        tmp_text_conversation.text = "";


        // 말풍선은 작게 잡아주기
        RectTransform img = bubble.GetComponent<RectTransform>();
        float tagetSize = size + 200;
        img.sizeDelta = new Vector3(img.sizeDelta.x, 0);

        // 약간의 애니메이션
        Vector3 targetsizeV3 = new Vector3(img.sizeDelta.x, tagetSize);
        while (img.sizeDelta.y < targetsizeV3.y - 0.1f)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, targetsizeV3, Time.deltaTime * 14.0f);
            yield return null;
        }

        // 텍스트 설정
        tmp_text_conversation.text = conversation;

        yield return new WaitForSeconds(3);

        // 텍스트 초기화
        tmp_text_npc.text = "";
        tmp_text_conversation.text = "";

        // 애니메이션 효과 나오면서 닫힘
        while (img.sizeDelta.y > 130)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
            yield return null;
        }

        bubbleOBJ.SetActive(false);

        speechCoroutine = null;
    }
    
    IEnumerator Play(string conversation, bool isFix)
    {

        // 말풍선을 활성화 해주고 값을 초기화
        bubbleOBJ.SetActive(true);
        tmp_text_npc.text = "";
        tmp_text_conversation.text = conversation;
        // 텍스트 사이즈 체크 후 약간의 애니메이션
        float size = tmp_text_conversation.preferredHeight;
        print(size);
        tmp_text_conversation.text = "";


        // 높이 잡아주기 
        RectTransform name = tmp_text_npc.gameObject.GetComponent<RectTransform>();
        name.sizeDelta = new Vector3(name.sizeDelta.x, size + 6);

        RectTransform con = tmp_text_conversation.gameObject.GetComponent<RectTransform>();
        con.sizeDelta = new Vector3(con.sizeDelta.x, size);

        // 말풍선은 작게 잡아주기
        RectTransform img = bubble.GetComponent<RectTransform>();
        float tagetSize = (size + 19) * 10;
        img.sizeDelta = new Vector3(img.sizeDelta.x, 0);

        // 약간의 애니메이션
        Vector3 targetsizeV3 = new Vector3(img.sizeDelta.x, tagetSize);
        while (img.sizeDelta.y < targetsizeV3.y - 0.1f)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, targetsizeV3, Time.deltaTime * 14.0f);
            yield return null;
        }

        // 텍스트 설정
        tmp_text_conversation.text = conversation;

        // 대화냐 아니냐에 따라 갈림
        if (!isFix)
        {
            yield return new WaitForSeconds(3);

            // 텍스트 초기화
            tmp_text_npc.text = "";
            tmp_text_conversation.text = "";

            // 애니메이션 효과 나오면서 닫힘
            while (img.sizeDelta.y > 130)
            {
                img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
                yield return null;
            }

            bubbleOBJ.SetActive(false);
        }
        else
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            // 텍스트 초기화
            tmp_text_npc.text = "";
            tmp_text_conversation.text = "";

            // 애니메이션 효과 나오면서 닫힘
            while (img.sizeDelta.y > 130)
            {
                img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
                yield return null;
            }

            bubbleOBJ.SetActive(false);

        }

    }
}
