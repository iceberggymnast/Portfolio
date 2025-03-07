using Photon.Pun;
using System;
using System.Collections;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class Interaction_TrashBin : MonoBehaviour
{
    //플레이어
    GameObject player;

    //손전등
    public GameObject flashLight;

    //분리수거 미니게임 시점에서 벗어나는 버튼(돌아가기 버튼)
    [SerializeField]
    Button backButton;

    GameObject Canvas_TrashCan;
    GameObject Canvas_MainUI;
    GameObject Canvas_Player;

    GameObject InteractionCanvas;

    //분리수거 미니게임 전 플레이어에 붙어있는 mainCamera와 UICamera의 원래 위치 및 회전 값 저장
    Transform camPosTransform;
    Vector3 originPlayercamPosPosition;
    Quaternion originPlayercamPosRotation;

    Transform mainCameraTransform;
    Vector3 originMainCameraPosition;
    Quaternion originMainCameraRotation;

    Transform UICameraTransform;
    Vector3 originalUICameraPosition;
    Quaternion originalUICameraRotation;

    //분리수거 미니게임 안할 때, 쓰레기통 UI 원래 위치
    Vector2 originalTrashCanPosition;
    RectTransform trashCanUIRectTransform;

    //분리수거 미니게임 전 손전등이 원래 켜져있었는지 여부
    bool wasFlashlightOn;

    //쓰레기통 오브젝트 이미지들
    GameObject silver_trashcan;
    GameObject glass_trashcan;
    GameObject plastic_trashcan;
    GameObject clothes_trashcan;

    RectTransform droppedImage_rectTransform;
    public RectTransform spawnPos_rectTransform;

    PlayerInfo PlayerInfo;


    GameObject CamPos_Recycling;
    GameObject mainCamera;
    GameObject uiCamera;

    Vector3 currentMainCameraPos;
    Quaternion currentMainCameraRot;

    Vector3 currentUICameraPos;
    Quaternion currentUICameraRot;


    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    //분리수거 안내 팝업(시작하면 잠깐 떴다가 사라짐)
    public GameObject RecyclingIntroductionPopUpCanvas;

    //분리수거 실패 후 띄워지는 오답풀이 캔버스
    public GameObject solvingCanvas;

    Interaction_UICamera_Controller interaction_UICamera_Controller;


    //분리수거 설명창
    GameObject IntroDuctionPanel;

    //설명창 캔버스 그룹
    CanvasGroup canvasGroup;



    void Start()
    {
        Interaction_Base interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = TestBinEvent;
        interaction_UICamera_Controller = GetComponent<Interaction_UICamera_Controller>();

        CamPos_Recycling = GameObject.Find("CamPos_Recycling");

        //캔버스 찾기
        Canvas_TrashCan = GameObject.Find("Canvas_TrashCan").gameObject;           //쓰레기통 UI
        Canvas_MainUI = GameObject.Find("Canvas_MainUI").gameObject;               //잔여 산소%, 쓰레기통 잔여용량%, 포인트
        Canvas_Player = GameObject.Find("Canvas_Player").gameObject;               //플레이어 포인트, 산소UI, 스테미나 UI
        InteractionCanvas = GameObject.Find("InteractionCanvas").gameObject;       //인터렉션 캔버스(스페이스 바 빌보드) 

        interaction_Base.OnPlayerChanged += UpdatePlayerReference;

        //손전등
        //flashLightHandler = player.transform.GetChild(2).GetComponent<FlashLightHandler>();

        //타입별 쓰레기통 이미지들 시작할 때는 꺼주기
        silver_trashcan = Canvas_TrashCan.transform.GetChild(1).gameObject;
        silver_trashcan.SetActive(false);

        glass_trashcan = Canvas_TrashCan.transform.GetChild(2).gameObject;
        glass_trashcan.SetActive(false);

        plastic_trashcan = Canvas_TrashCan.transform.GetChild(3).gameObject;
        plastic_trashcan.SetActive(false);

        clothes_trashcan = Canvas_TrashCan.transform.GetChild(4).gameObject;
        clothes_trashcan.SetActive(false);


        //분리수거 설명창도 꺼주기
        IntroDuctionPanel = Canvas_TrashCan.transform.GetChild(7).gameObject;
        canvasGroup = IntroDuctionPanel.GetComponent<CanvasGroup>();
        IntroDuctionPanel.SetActive(false);


        //쓰레기통 UI의 원래 위치 저장
        trashCanUIRectTransform = Canvas_TrashCan.transform.GetChild(0).GetComponent<RectTransform>();
        originalTrashCanPosition = trashCanUIRectTransform.anchoredPosition;

        // 뒤로 가기 버튼 이벤트 추가
        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        //backButton.onClick.AddListener(OnBackButtonClicked);

        //처음에는 뒤로 가기 버튼 비활성화 및 투명하게 만들기
        SetBackButtonTransparency(0f); // 완전 투명하게 설정
        backButton.interactable = false; // 클릭 불가능하게 설정

        GameObject spawnPos = GameObject.Find("Canvas_TrashCan").transform.GetChild(0).GetChild(5).gameObject;

        trashCanUIRectTransform.localScale = new Vector3(0.4f,0.4f, 0);  //크기 조정
        trashCanUIRectTransform.rotation = Quaternion.Euler(0, 0, 90);  //가로로 눕히기

        solvingCanvas.gameObject.SetActive(false);

        RecyclingIntroductionPopUpCanvas.SetActive(false);

    }

    // `Interaction_Base`의 player가 변경될 때 호출되는 이벤트 핸들러
    private void UpdatePlayerReference(GameObject newPlayer)
    {
        player = newPlayer;
    }


    void TestBinEvent()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }
        interaction_UICamera_Controller.IsStart = false;
        //분리수거 안내 팝업 잠깐 띄웠다가 없어지기
        StartCoroutine(IntroductionPopUp());

        print("분리수거 미니게임 시작");

        StartCoroutine(UIController.instance.FadeOut("OxygenUI", 0.5f));

        if (QuestManger.instance.questDatas[0].questState == QuestState.Accepting || QuestManger.instance.questDatas[0].questState == QuestState.CanCompleted)
        {
            StartCoroutine(UIController.instance.FadeOut("QuestBoard", 0.5f));
        }
        StartCoroutine(UIController.instance.FadeOut("ChatCanvas", 0.5f));
        StartCoroutine(UIController.instance.FadeOut("Canvas_AreaPopUp", 0.5f));

        //커서 켜주기
        PlayerInfo.instance.isCusor = true;

        // 분리수거 미니게임 시작 시 뒤로 가기 버튼 활성화 및 불투명하게 설정
        backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.AddListener(OnBackButtonClicked);

        backButton.interactable = true; // 클릭 가능하게 설정
        SetBackButtonTransparency(1f); // 완전 불투명하게 설정


        //플레이어 위치를 분리수거 미니게임 위치로 이동
        GameObject Pos_Recycling = GameObject.Find("Pos_Recycling").gameObject;
        PlayerInfo.instance.player.transform.SetParent(Pos_Recycling.transform);

        PlayerInfo.instance.player.transform.localPosition = Vector3.zero;
        PlayerInfo.instance.player.transform.localRotation = Quaternion.identity; // 방향도 맞춰줌

        //분리수거 게임할 때 UI 켜줄거만 켜주고 나머지 꺼주기
        Canvas_MainUI.transform.GetChild(0).gameObject.SetActive(false);  //쓰레기통 잔여용량% UI 꺼줌
        Canvas_MainUI.transform.GetChild(1).gameObject.SetActive(true);  //뒤로가기 버튼 UI켜줌
        Canvas_TrashCan.transform.GetChild(0).gameObject.SetActive(true); //쓰레기통 UI 켜주기
        Canvas_TrashCan.transform.GetChild(6).gameObject.SetActive(false);  //쓰레기통 용량 UI꺼줌
         //산소 ui와 스테미나 ui꺼주기
        //Canvas_Player.transform.GetChild(4).gameObject.SetActive(false); //스테미나 UI꺼주기
        InteractionCanvas.gameObject.SetActive(false);  //상호작용 캔버스 꺼주기

        //산소통 보유수가 0보다 크면 산소통 UI 꺼주기
        Canvas_TrashCan.transform.GetChild(5).gameObject.SetActive(false);


        //쓰레기통 이미지들 켜주기(트리거 사용용)
        silver_trashcan.SetActive(true);
        silver_trashcan.GetComponent<Image>().enabled = false;   //이미지는 안보이게 꺼주기

        glass_trashcan.SetActive(true);
        glass_trashcan.GetComponent<Image>().enabled = false;   //이미지는 안보이게 꺼주기

        plastic_trashcan.SetActive(true);
        plastic_trashcan.GetComponent<Image>().enabled = false;   //이미지는 안보이게 꺼주기

        clothes_trashcan.SetActive(true);
        clothes_trashcan.GetComponent<Image>().enabled = false;   //이미지는 안보이게 꺼주기


        //설명창 켜주기
        IntroDuctionPanel.gameObject.SetActive(true);
        // 페이드 인, 유지, 페이드 아웃을 한 번에 실행
        StartCoroutine(FadeInAndOut(canvasGroup, 0.5f, 3.0f));



        //플레이어의 mainCamera의 position과 rotation값을 Camera_Recycling카메라의 position과 rotation값으로 바꿔주기
        CameraRotate cameraRotate = PlayerInfo.instance.player.GetComponentInChildren<CameraRotate>();
        mainCamera = cameraRotate.mainCamera;
        uiCamera = cameraRotate.uiCamera;

        currentMainCameraPos = new Vector3(1.5f, 1.2f, -5);
        currentMainCameraRot = new Quaternion(0, 0, 0, 0);


        currentUICameraPos = new Vector3(1.5f, 1.2f, -5);
        currentUICameraRot = new Quaternion(0, 0, 0, 0);


        mainCamera.transform.position = CamPos_Recycling.transform.position;
        mainCamera.transform.rotation = CamPos_Recycling.transform.rotation;


        //쓰레기통 UI 크기 키워주고, 가로로 방향 눕히기
        GameObject trashCanUIGameObject = Canvas_TrashCan.transform.GetChild(0).gameObject;
        trashCanUIRectTransform.anchoredPosition = new Vector2(20,253);  //위치 설정
        trashCanUIRectTransform.localScale = new Vector3(1.0f,1.0f,0);  //크기 조정
        trashCanUIRectTransform.rotation = Quaternion.Euler(0, 0, 0);  //원래 회전값으로 바꿔주기

        //플레이어 이동x, 플레이어 쓰레기 수집x, 카메라 회전x, 손전등x
        DisableOrAblePlayerMovement(false);
        DiableOrAblePlayerGetTrash(false);
        DisableOrAbleCameraRotation(false);

        //손전등 켜져있었으면 꺼주기
        PlayerInfo.instance.isFlashLightOn = false;

        DisableOrAbleFlashLight(false);
    }

    //분리수거 안내창 팝업 띄우는 코루틴
    IEnumerator IntroductionPopUp()
    {
        RecyclingIntroductionPopUpCanvas.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        RecyclingIntroductionPopUpCanvas.SetActive(false);
    }

    //뒤로 가기 버튼(분리수거 미니게임 -> 원래 위치)
    void OnBackButtonClicked()
    {
        solvingCanvas.SetActive(false);
        interaction_UICamera_Controller.IsStart = true;

        backButton.onClick.RemoveListener(OnBackButtonClicked);

        StartCoroutine(UIController.instance.FadeIn("OxygenUI", 0.5f));
        if (QuestManger.instance.questDatas[0].questState == QuestState.Accepting || QuestManger.instance.questDatas[0].questState == QuestState.CanCompleted)
        {
            StartCoroutine(UIController.instance.FadeIn("QuestBoard", 0.5f));
        }

        StartCoroutine(UIController.instance.FadeIn("ChatCanvas", 0.5f));
        StartCoroutine(UIController.instance.FadeIn("Canvas_AreaPopUp", 0.5f));

        //커서 다시 꺼주기
        PlayerInfo.instance.isCusor = false;

        //상호작용 다시 되게 해주기
        GetComponent<Interaction_Base>().useTrue = false;

        //쓰레기통 UI 원래위치로!
        trashCanUIRectTransform.anchoredPosition = originalTrashCanPosition;

        //산소통 보유수가 0보다 크면 산소통 UI 꺼주기
        if (PlayerInfo.instance.current_OxygenchargerAmount > 0)
        {
            Canvas_TrashCan.transform.GetChild(5).gameObject.SetActive(true);
        }

        // 분리수거 미니게임 종료 시 뒤로 가기 버튼을 비활성화 및 투명하게 설정
        backButton.interactable = false; // 클릭 불가능하게 설정
        SetBackButtonTransparency(0f); // 완전 투명하게 설정

        //CamPos_Recycling의 자식으로 있던 플레이어를 독립시키기
        PlayerInfo.instance.player.transform.SetParent(null);


        mainCamera.transform.localPosition = currentMainCameraPos;
        mainCamera.transform.localRotation = currentMainCameraRot;

        uiCamera.transform.localPosition = currentUICameraPos;
        uiCamera.transform.localRotation = currentUICameraRot;


        //쓰레기통 UI원래 위치와 크기로 되돌리기
        trashCanUIRectTransform.localScale = new Vector3(0.4f, 0.4f, 0);  //크기 조정
        trashCanUIRectTransform.rotation = Quaternion.Euler(0, 0, 90);  //가로로 눕히기

        //모든 UI 활성화
        Canvas_MainUI.transform.GetChild(0).gameObject.SetActive(true);  // 쓰레기통 잔여용량% UI 켜주기
        Canvas_TrashCan.transform.GetChild(6).gameObject.SetActive(true);  //쓰레기통 용량 UI켜주기
        InteractionCanvas.SetActive(true);  // 상호작용 캔버스 켜주기

        //쓰레기통 이미지들 꺼주기(트리거 사용용)
        silver_trashcan.SetActive(false);
        glass_trashcan.SetActive(false);
        plastic_trashcan.SetActive(false);
        clothes_trashcan.SetActive(false);

        //플레이어 이동O, 플레이어 쓰레기 수집O, 카메라 회전O, 손전등O
        DisableOrAblePlayerMovement(true);
        DiableOrAblePlayerGetTrash(true);
        DisableOrAbleCameraRotation(true);
        DisableOrAbleFlashLight(true);
    }


    //분리수거 드래그앤드롭하면 포인트 얻는 함수
    public void RecyclingGame(string droppedTrashItemName, GameObject droppedTrashcanObject, GameObject droppedImage, int trashPoint)
    {
        Debug.Log($"쓰레기: {droppedTrashItemName}, 쓰레기통: {droppedTrashcanObject.name}");

        // 드랍된 쓰레기 이미지와 쓰레기통 타입을 비교해서
        TrashcanType trashcanType = droppedTrashcanObject.GetComponent<TrashcanType>();

        //if (trashcanType.trashcanType == droppedTrashItemName)
        if(droppedTrashItemName.Contains(trashcanType.trashcanType))
        {
            solvingCanvas.gameObject.SetActive(true);
            //print("분리수거 성공! 포인트 획득!");

            //분리수거 성공시 오답풀이를 해주는 캔버스 3초 동안 등장하고 희미해지면서 꺼짐
            StartCoroutine(OnSuccessCanvas());

            //집가서 포인트 제대로 다 다르게 획득되는지 확인하기
            PlayerInfo.instance.PointPlusOrMinus(trashPoint);
            //PlayerInfo.instance.point += trashcanType.RecyclingPoint;
            //print(droppedTrashcanObject.name + "의 재활용 포인트는 :" + trashcanType.RecyclingPoint);

            // 분리수거 성공한 쓰레기 이미지는 삭제
            Debug.Log("Destroying Image: " + droppedImage.name); // 삭제 확인
            Destroy(droppedImage);
        }
        else
        {
            //분리수거 실패시 오답풀이를 해주는 캔버스 3초 동안 등장하고 희미해지면서 꺼짐
            StartCoroutine(OnFailCanvas());

            PlayerInfo.instance.PointPlusOrMinus(-trashPoint);

            //해당 쓰레기 이미지는 spawnPos로 이동해서 다시 쓰레기통에 넣기
            droppedImage_rectTransform = droppedImage.GetComponent<RectTransform>();
            droppedImage_rectTransform.anchoredPosition = spawnPos_rectTransform.anchoredPosition;
        }
    }

    //분리수거 성공 시 띄울 캔버스 코루틴
    IEnumerator OnSuccessCanvas()
    {
        //캔버스 활성화
        solvingCanvas.SetActive(true);
        solvingCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "<size=33><color=green>분리수거 성공! 포인트 획득!</color></size>";

        CanvasGroup canvasGroup = solvingCanvas.GetComponent<CanvasGroup>();
        float duration = 0.5f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 1;


        //3초 대기
        yield return new WaitForSeconds(1.0f);

        //캔버스 비활성화
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1,0, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 0;

        solvingCanvas.SetActive(false);
    }



    //분리수거 실패 시 띄울 캔버스 코루틴
    IEnumerator OnFailCanvas()
    {
        //캔버스 활성화
        solvingCanvas.SetActive(true);
        solvingCanvas.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "<size=33><color=red>잘못된 분리수거...포인트 차감..</color></size><br><br><size=28><color=#00FFFF>음료수캔과 통조림캔은 캔류, 마스크는 천류, <br>빨대와 페트병은 플라스틱류, 유리병은 유리류 입니다.</color></size><br><br>다시 시도해보세요!";

        CanvasGroup canvasGroup = solvingCanvas.GetComponent<CanvasGroup>();
        float duration = 0.5f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 1;


        //3초 대기
        yield return new WaitForSeconds(1.5f);

        //캔버스 비활성화
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
        canvasGroup.alpha = 0;

        solvingCanvas.SetActive(false);
    }



    //버튼 투명도 설정 함수
    void SetBackButtonTransparency(float alpha)
    {
        Image buttonImage = backButton.GetComponent<Image>();
        TextMeshProUGUI buttonText = backButton.GetComponentInChildren<TextMeshProUGUI>(); // 버튼 텍스트 찾기

        //버튼 이미지의 알파 값 설정
        Color buttonColor = buttonImage.color;
        buttonColor.a = alpha; // 알파 값 변경
        buttonImage.color = buttonColor;

        //버튼 텍스트의 알파 값 설정
        if (buttonText != null)
        {
            Color textColor = buttonText.color;
            textColor.a = alpha; // 알파 값 변경
            buttonText.color = textColor;
        }
    }


    //플레이어 이동 못하게/하게 하는 함수
    void DisableOrAblePlayerMovement(bool TrueOrFalse)
    {
        PlayerMove playerMove = PlayerInfo.instance.player.transform.GetComponentInChildren<PlayerMove>();
        playerMove.isMove = TrueOrFalse;
    }

    //플레이어가 쓰레기 수집 못하게/하게 하는 함수
    void DiableOrAblePlayerGetTrash(bool TrueOrFalse)
    {
        Vacuum vacuum = PlayerInfo.instance.player.transform.GetComponentInChildren<Vacuum>();
        vacuum.isStop = TrueOrFalse;
    }

    //카메라가 회전 안하게/회전 하게 하는 함수
    void DisableOrAbleCameraRotation(bool TrueOrFalse)
    {
        CameraRotate cameraRotate = PlayerInfo.instance.player.transform.GetComponentInChildren<CameraRotate>();
        cameraRotate.useRotX = TrueOrFalse;
        cameraRotate.useRotY = TrueOrFalse;
    }
    
    //분리수거 미니게임 할 때 손전등 가능하게/불가능하게 하는 함수
    void DisableOrAbleFlashLight(bool TrueOrFalse)
    {
        FlashLightHandler flashLightHandler = PlayerInfo.instance.player.transform.GetComponentInChildren<FlashLightHandler>();

        PlayerInfo.instance.isFlashStop = !TrueOrFalse;
        flashLightHandler.flashLight.intensity = TrueOrFalse == true ? 10 : 0;
    }


    IEnumerator FadeInAndOut(CanvasGroup canvasGroup, float Duration, float displayDuration)
    {
        //// 페이드 인
        //canvasGroup.alpha = 0;
        //canvasGroup.interactable = false;
        //canvasGroup.blocksRaycasts = false;

        for (float t = 0; t < Duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, t / Duration);
            yield return null;
        }
        //canvasGroup.alpha = 1;
        //canvasGroup.interactable = true;
        //canvasGroup.blocksRaycasts = true;

        // 일정 시간 동안 유지
        yield return new WaitForSeconds(displayDuration);

        //// 페이드 아웃
        //canvasGroup.interactable = false;
        //canvasGroup.blocksRaycasts = false;

        for (float t = 0; t < Duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, t / Duration);
            yield return null;
        }
        //canvasGroup.alpha = 0;
        //canvasGroup.interactable = false;
        //canvasGroup.blocksRaycasts = false;

        // UI 비활성화 (필요 시)
        IntroDuctionPanel.SetActive(false);
    }

}
