using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UI.Button;
using System;

public class interaction_PromotionBoard_01 : MonoBehaviour
{
    //홍보 마스코트에 상호작용시 띄워주는 홍보물 캔버스
    public GameObject promotionCanvas;
    public Button backButton;
    Interaction_Base interaction_Base;

    public RectTransform content_01;
    public Button leftButton_01;
    public Button rightButton_01;

    public RectTransform content_02;
    public Button leftButton_02;
    public Button rightButton_02;

    public float moveDuration = 1f; // 이동에 걸리는 시간
    Vector3 targetPosition; // 목표 위치

    //카드뉴스 이미지 버튼 클릭시 뜨는 카드뉴스 캔버스
    public GameObject cardNewsCanvas_parent;

    [SerializeField]
    public List<CardNews> cardNewss;  //각 이미지마다 들어있는 카드뉴스 내용들
    [SerializeField]
    public CardNews currnetCardNews;  //현재 선택된 카드뉴스

    public int currnetIndex = 0;      //현재 어느 페이지 읽고 있는지 인덱스

    //현재 보여지고 있는 카드뉴스 이미지
    public GameObject cardNewsImage;

    //다음버튼/이전버튼
    public Button CardNews_LeftButton;
    public Button CardNews_RightButton;

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;


    void Start()
    {
        promotionCanvas.gameObject.SetActive(false);

        interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = OnPromotionCanvas;

        content_01.anchoredPosition = new Vector2(660, content_01.anchoredPosition.y);
        content_02.anchoredPosition = new Vector2(660, content_01.anchoredPosition.y);

        cardNewsCanvas_parent.gameObject.SetActive(false);

        ArrowBtnCheck();

    }

    void Update()
    {

    }

    //홍보 캔버스 띄워주는 함수
    public void OnPromotionCanvas()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        //커서 숨겨주기
        PlayerInfo.instance.isCusor = true;

        //홍보 캔버스 활성화
        promotionCanvas.gameObject.SetActive(true);

        //플레이어 이동x, 플레이어 쓰레기 수집x, 카메라 회전x, 손전등x
        DisableOrAblePlayerMovement(false);
        DiableOrAblePlayerGetTrash(false);
        DisableOrAbleCameraRotation(false);
        DisableOrAbleFlashLight(false);
    }

    //'홈페이지 방문하기'버튼 클릭 시, 홈페이지 열어주는 함수
    public void OnclickGoToHomepageButton()
    {
        Application.OpenURL("https://www.koem.or.kr/site/koem/main.do");
    }

    //왼쪽 버튼 클릭 시 content 오른쪽으로 이동 (-1864만큼)
    //만약 더 넘길 게 없다면 버튼을 비활성화하고 컬러값 반투명으로
    public void OnClickLeftButton_01()
    {
        //목표 위치 설정(현재 위치에서 +1864이동)
        targetPosition = new Vector3(content_01.localPosition.x + 1864, content_01.localPosition.y, content_01.localPosition.z);

        if (content_01.transform.localPosition.x < 660)
        {
            print("왼버튼 클릭");

            //코루틴 실행해서 부드럽게 이동
            StartCoroutine(SmoothMoveCoroutine(content_01, content_01.localPosition, targetPosition, moveDuration));
        }
        //가장 왼쪽까지 넘겼을 경우
        else
        {
            print("왼쪽 버튼 비활성화");
        }
    }
    //오른쪽 버튼 클릭 시 content 왼쪽으로 이동 (+1864만큼만큼)
    //만약 더 넘길 게 없다면 버튼을 비활성화하고 컬러값 반투명으로
    public void OnClickRightButton_01()
    {
        //목표 위치 설정(현재 위치에서 -1864이동)
        targetPosition = new Vector3(content_01.localPosition.x - 1864, content_01.localPosition.y, content_01.localPosition.z);

        if (content_01.transform.localPosition.x > -3056)
        {
            print("오른버튼 클릭");

            //코루틴 실행해서 부드럽게 이동
            StartCoroutine(SmoothMoveCoroutine(content_01, content_01.localPosition, targetPosition, moveDuration));
        }
        //가장 왼쪽까지 넘겼을 경우
        else
        {
            print("오른쪽 버튼 비활성화");
        }
    }

    //왼쪽 버튼 클릭 시 content 오른쪽으로 이동 (-1864만큼)
    //만약 더 넘길 게 없다면 버튼을 비활성화하고 컬러값 반투명으로
    public void OnClickLeftButton_02()
    {
        //목표 위치 설정(현재 위치에서 +1864이동)
        targetPosition = new Vector3(content_02.localPosition.x + 1864, content_02.localPosition.y, content_02.localPosition.z);


        if (content_02.transform.position.x < 660)
        {
            print("왼버튼 클릭");

            //코루틴 실행해서 부드럽게 이동
            StartCoroutine(SmoothMoveCoroutine(content_02, content_02.localPosition, targetPosition, moveDuration));
        }
        //가장 왼쪽까지 넘겼을 경우
        else
        {
            print("왼쪽 버튼 비활성화");
        }
    }

    //오른쪽 버튼 클릭 시 content 왼쪽으로 이동 (+1864만큼만큼)
    //만약 더 넘길 게 없다면 버튼을 비활성화하고 컬러값 반투명으로
    public void OnClickRightButton_02()
    {
        //목표 위치 설정(현재 위치에서 -1864이동)
        targetPosition = new Vector3(content_02.localPosition.x - 1864, content_02.localPosition.y, content_02.localPosition.z);

        if (content_02.transform.localPosition.x > -3056)
        {
            print("오른버튼 클릭");

            //코루틴 실행해서 부드럽게 이동
            StartCoroutine(SmoothMoveCoroutine(content_02, content_02.localPosition, targetPosition, moveDuration));
        }
        //가장 왼쪽까지 넘겼을 경우
        else
        {
            print("오른쪽 버튼 비활성화");
        }
    }

    //카드뉴스 이미지 버튼들 클릭시, 카드뉴스 캔버스 열어주는 함수
    public void OnClickCardNewsButton(int index)
    {
        currnetCardNews = cardNewss[index];
        promotionCanvas.gameObject.SetActive(false);
        cardNewsCanvas_parent.gameObject.SetActive(true);

        // 팝업된 창에 이미지 기본 세팅
        currnetIndex = 0;

        //현재 이미지 업데이트
        Image imgComponent = cardNewsImage.GetComponent<Image>();
        if (imgComponent != null)
        {
            imgComponent.sprite = currnetCardNews.img[currnetIndex];
        }
        else
        {
            print("이미지 컴포넌트 없음");
        }
        InteractableCheck();
    }

    //카드뉴스 캔버스 닫는 뒤로가기 버튼
    public void OnClickCardNesCanvasbackButton(Button button)
    {
        promotionCanvas.gameObject.SetActive(true);
        cardNewsCanvas_parent.gameObject.SetActive(false);

        //이미지 초기화
        Image imgComponent = cardNewsImage.GetComponent<Image>();
        if (imgComponent != null)
        {
            imgComponent.sprite = null;
        }
    }

    private IEnumerator SmoothMoveCoroutine(RectTransform content, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float elapsedTime = 0f;

        // duration 동안 선형 보간
        while (elapsedTime < duration)
        {
            content.transform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // 한 프레임 대기
        }

        // 최종 위치를 정확히 설정
        content.transform.localPosition = endPosition;

        ArrowBtnCheck();

    }

    //끝까지 넘겼을 때 버튼 비활성화, 아닐 때 활성화 시켜주는 함수
    public void ArrowBtnCheck()
    {

        if (content_01.transform.localPosition.x >= -3056)
        {
            rightButton_01.interactable = true;
        }
        else
        {
            rightButton_01.interactable = false;

        }

        if (content_01.transform.localPosition.x < 660)
        {
            leftButton_01.interactable = true;
        }
        else
        {
            leftButton_01.interactable = false;
        }

        if (content_02.transform.localPosition.x >= -3056)
        {
            rightButton_02.interactable = true;
        }
        else
        {
            rightButton_02.interactable = false;

        }

        if (content_02.transform.localPosition.x < 660)
        {
            leftButton_02.interactable = true;
        }
        else
        {
            leftButton_02.interactable = false;
        }
    }

    //뒤로 가기 버튼
    public void OnclickBackButton()
    {
        promotionCanvas.gameObject.SetActive(false);
        

        //플레이어 이동x, 플레이어 쓰레기 수집x, 카메라 회전x, 손전등x
        DisableOrAblePlayerMovement(true);
        DiableOrAblePlayerGetTrash(true);
        DisableOrAbleCameraRotation(true);
        DisableOrAbleFlashLight(true);

        //다시 상호작용되도록
        interaction_Base.useTrue = false;

        //커서 숨겨주기
        PlayerInfo.instance.isCusor = false;
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

    //카드 뉴스 이미지 넣고 이전/다음 버튼 클릭시 업데이트 해주는 함수
    public void CardNewsUpDate(bool plus)
    {
        //다음 버튼 누르면 plus가 true니까 currnetIndex를 증가시키고
        if (plus)
        {
            currnetIndex++;

        }
        //이전 버튼 누르면 plus가 false니까 currnetIndex를 감소시킴
        else
        {
            currnetIndex--;
        }

        //currnetIndex가 카드뉴스의 전체 장수와 동일하다면
        if (currnetIndex == currnetCardNews.img.Length)
        {
            // 카운트가 넘어갔으니 0으로 
            currnetIndex = 0;

        }
        else if (currnetIndex < 0)
        {
            // 카운트가 마이너스가 되면 카운트의 -1값으로
            currnetIndex = currnetCardNews.img.Length - 1;
        }

        //현재 이미지 업데이트
        Image imgComponent = cardNewsImage.GetComponent<Image>();
        if (imgComponent != null)
        {
            imgComponent.sprite = currnetCardNews.img[currnetIndex];
        }
        else
        {
            print("이미지 컴포넌트 없음");
        }

        InteractableCheck();
    }

    public void InteractableCheck()
    {
        if (currnetIndex == currnetCardNews.img.Length - 1)
        {
            //다음 버튼 비활성화
            CardNews_RightButton.interactable = false;
        }
        else
        {
            CardNews_RightButton.interactable = true;
        }

        if (currnetIndex == 0)
        {
            CardNews_LeftButton.interactable = false;
        }
        else
        {
            CardNews_LeftButton.interactable = true;
        }
    }
}



//카드 뉴스 이미지에 보여질 여러 카드 뉴스 내용들
[Serializable]
public class CardNews
{
    public Sprite[] img;
}