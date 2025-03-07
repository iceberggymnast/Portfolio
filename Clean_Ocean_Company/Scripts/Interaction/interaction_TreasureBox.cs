using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Color = UnityEngine.Color;

//로비에 왔을 때 , 이지세이브에 보관하고 있다가, 보급상자에서는 실패했을 경우에 playerinfo에 테마가 있는지 확인하고, 없으면 전체 문제에서 내기

public class interaction_TreasureBox : MonoBehaviourPun
{
    //퀴즈의 질문과 선택지, 정답을 담는 Serializable 클래스
    [Serializable]
    public class QuizData
    {
        public string q;
        public string n1;
        public string n2;
        public string n3;
        public string n4;
        public string a;
        public string t;
        public string s;
    }

    //플레이어
    GameObject player;

    //퀴즈 캔버스
    public GameObject QuizCanvas;

    //텍스트 색상 변수
    Color selectedColor = Color.green; //선택한 보기 색상
    Color defaultColor = Color.white; //기본 색상

    //정답 제출하기 버튼
    public Button submitButton;


    // 원래 위치를 저장할 변수
    Vector3 originalPlayerPosition;
    Quaternion originalPlayerRotation;

    //통신여부나 정답 또는 오답 여부 띄울 텍스트
    public TextMeshProUGUI message;

    //오답일 때 풀이 띄워줄 텍스트
    public TextMeshProUGUI Sovingmessage;

    //오답일 때 풀이 띄워줄 텍스트 배경
    public GameObject SovingmessageBG;


    //AI 통신용 퀴즈 데이터
    QuizData quizData;

    //미리 가지고 있던 퀴즈 데이터
    List<QuizData> preloadedQuizzes;

    //플레이어가 선택한 답안(1, 2, 3, 4 중 하나)
    int selectedAnswer;

    //퀴즈 풀기시도는 3번만 가능
    int attempts = 3;

    //텍스트들(문제, 보기들)
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI option1Text;
    public TextMeshProUGUI option2Text;
    public TextMeshProUGUI option3Text;
    public TextMeshProUGUI option4Text;

    //보급상자 뚜껑
    public GameObject door;
    Animator animator;

    //질문이 로드되었는지 여부
    bool hasQuizLoaded = false;

    //정답 제출 가능 여부(제출하기 버튼 연타 방지)
    bool canSubmit = true;

    //남은 기회 횟수
    public TextMeshProUGUI remainChance;

    //보급상자 풀고 있음을 체크
    public bool isSoving = false;

    //플레이어마다 부여된 문제 테마
    string type;

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    //사운드
    AudioSet audioSet;

    //모델링만 있는 보급상자(퀴즈 풀때 비출거임)
    Camera ModelingTreasureCamera;

    void Start()
    {
         ModelingTreasureCamera = GameObject.Find("ModelingTreasureCamera").GetComponent<Camera>();
        if (ModelingTreasureCamera)
        {
            ModelingTreasureCamera.enabled = false;

        }

        audioSet = GetComponent<AudioSet>();

        //보급상자에 붙은 Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        //각 보기 버튼에 이벤트 연결
        option1Text.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(1));
        option2Text.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(2));
        option3Text.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(3));
        option4Text.GetComponent<Button>().onClick.AddListener(() => SelectAnswer(4));

        //정답 제출하기 버튼 이벤트 연결
        submitButton.onClick.AddListener(() => CheckAnswer(selectedAnswer));

        //Interaction_Base의 action에 함수 넣어주기
        Interaction_Base interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = OpenTreasure;

        //퀴즈 캔버스 비활성화
        QuizCanvas.gameObject.SetActive(false);


        // 미리 설정한 퀴즈 리스트 초기화
        //InitializePreloadedQuizzes();
        InitializePreloadedQuizzes_a();
        InitializePreloadedQuizzes_b();
        InitializePreloadedQuizzes_c();
        InitializePreloadedQuizzes_d();
        InitializePreloadedQuizzes_e();
        InitializePreloadedQuizzes_f();
        InitializePreloadedQuizzes_g();
        InitializePreloadedQuizzes_h();
        InitializePreloadedQuizzes_i();
        InitializePreloadedQuizzes_j();
        InitializePreloadedQuizzes_k();
        InitializePreloadedQuizzes_l();


        //로딩중 텍스트 비활성화
        message.gameObject.SetActive(false);
        //풀이 텍스트 배경 및 풀이 텍스트 비활성화
        SovingmessageBG.gameObject.SetActive(false);
        Sovingmessage.gameObject.SetActive(false);


        type = SaveSys.saveSys.LoadQuizType();  
        

        print("타입은 " + type + "입니다.");

    }


    void Update()
    {
        remainChance.text = attempts.ToString();
    }

    //상호작용하여 퀴즈를 시작하는 함수
    public void OpenTreasure()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        if (!isSoving)
        {
            //StartCoroutine(SendPlayerId(PhotonNetwork.NickName));

            //보급상자가 사용중이란 걸 모든 플레이어에게 알려주기
            photonView.RPC(nameof(isSovingRPC), RpcTarget.All, true);

            //퀴즈 가져오기
            StartCoroutine(GetQuizData());

            //퀴즈 캔버스 열고 커서 켜주기
            QuizCanvas.gameObject.SetActive(true);

            //ModelingTreasureCamera 켜주기
            ModelingTreasureCamera.enabled = true;

            PlayerInfo.instance.isCusor = true;

            //플레이어 이동x, 플레이어 쓰레기 수집x, 카메라 회전x, 손전등x
            DisableOrAblePlayerMovement(false);
            DiableOrAblePlayerGetTrash(false);
            DisableOrAbleCameraRotation(false);
            DisableOrAbleFlashLight(false);

            //보기 텍스트 색상 다 디폴트 색상으로 해주기(검정색)
            option1Text.color = defaultColor;
            option1Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = defaultColor;
            option2Text.color = defaultColor;
            option2Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = defaultColor;
            option3Text.color = defaultColor;
            option3Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = defaultColor;
            option4Text.color = defaultColor;
            option4Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = defaultColor;
        }
        //이미 보급상자 다른 사람이 풀때
        else
        {

        }
    }

    [System.Serializable]
    public class Payload
    {
        public string id;
    }

    [PunRPC]
    void isSovingRPC(bool TorF)
    {
        isSoving = TorF;
    }

    IEnumerator GetQuizData()
    {
        //제출하기 버튼 활성화
        submitButton.gameObject.SetActive(false);
        message.gameObject.SetActive(true);
        message.text = "보급상자 AI 보안 프로그램 가동 중....\r\n잠시만 기다려주시길 바랍니다.";
        settingTxt(false);
        WWWForm form = new WWWForm();
        form.AddField("id", PhotonNetwork.NickName);
        
        using (UnityWebRequest request = new UnityWebRequest("http://221.163.19.142:9100/qna", "GET"))
        {
            Debug.Log("요청 URL: " + request.url);
            request.timeout = 10;

            Payload payload = new Payload();
            payload.id = PhotonNetwork.NickName;

            string jsonPayload = JsonUtility.ToJson(payload);

            //string jsonPayload = JsonUtility.ToJson(new { id = PhotonNetwork.NickName });
            print(jsonPayload);
            
            // JSON 데이터 첨부
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 헤더 설정 (Content-Type)
            request.SetRequestHeader("Content-Type", "application/json");
            

            yield return request.SendWebRequest();
            Debug.Log("요청 완료");


            if (request.result == UnityWebRequest.Result.Success)
            {
                message.gameObject.SetActive(false);
                message.text = "";
                settingTxt(true);

                string jsonResponse = request.downloadHandler.text;
                Debug.Log("서버로부터 받은 응답: " + jsonResponse);
                quizData = JsonUtility.FromJson<QuizData>(jsonResponse);
                hasQuizLoaded = true;
                ShowQuiz();
            }
            else
            {
                Debug.LogError("Error: " + request.error);
                //Debug.LogWarning("서버 연결에 실패하여 로컬 퀴즈로 대체합니다.");
                settingTxt(false);
                message.gameObject.SetActive(true);
                message.text = "서버 연결에 실패하여 자체 보안 프로그램으로 대체합니다.";
                //플레이어에게 부여된 테마별로 로컬 퀴즈 테마도 뽑기
                if(type == "해양쓰레기 발생원인")
                {
                    InitializePreloadedQuizzes_a();
                }
                else if(type == "해양쓰레기 현황")
                {
                    InitializePreloadedQuizzes_b();
                }
                else if (type == "해양쓰레기 피해 및 위험성")
                {
                    InitializePreloadedQuizzes_c();
                }
                else if (type == "해양쓰레기 피해 사례")
                {
                    InitializePreloadedQuizzes_d();
                }
                else if (type == "태평양 쓰레기섬")
                {
                    InitializePreloadedQuizzes_e();
                }
                else if (type == "미세플라스틱")
                {
                    InitializePreloadedQuizzes_f();
                }
                else if (type == "허베이스피릿호 원유유출 사고")
                {
                    InitializePreloadedQuizzes_g();
                }
                else if (type == "호주 검은 공 사건")
                {
                    InitializePreloadedQuizzes_h();
                }
                else if (type == "약품 사고")
                {
                    InitializePreloadedQuizzes_i();
                }
                else if (type == "폐어구에 걸린 돌고래")
                {
                    InitializePreloadedQuizzes_j();
                }
                else if (type == "우리나라 바다 거북")
                {
                    InitializePreloadedQuizzes_k();
                }
                else if (type == "상괭이")
                {
                    InitializePreloadedQuizzes_l();
                }
                //로그인 실패거나 등등 예외의 경우로 테마 못 받아온 경우에는 그냥 무작위로 전체 문제에서 추출
                else
                {
                    InitializePreloadedQuizzes();
                }
                LoadRandomLocalQuiz();
                ShowQuiz();
            }
            
    }
}

    //퀴즈를 출력하는 함수
    private void ShowQuiz()
    {
        message.gameObject.SetActive(false);
        message.text = "";
        settingTxt(true);

        //문제 및 보기4가지 업데이트
        questionText.text = quizData.q;
        option1Text.text = quizData.n1;
        option2Text.text = quizData.n2;
        option3Text.text = quizData.n3;
        option4Text.text = quizData.n4;

        //제출하기 버튼 활성화
        submitButton.gameObject.SetActive(true);
    }

    //플레이어가 답안을 선택하는 메서드
    private void SelectAnswer(int answer)
    {
        //선택한 답안을 저장
        selectedAnswer = answer;

        //Debug.Log($"선택한 답안: {selectedAnswer}");

        //선택한 답안의 텍스트는 초록색으로 변경하고 나머지는 기본 색상으로 설정
        //3항 조건 연산자 사용(condition ? trueResult : falseResult -> condition이 true이면 trueResult를 반환하고, false이면 falseResult를 반환)
        //1번 보기 선택시
        option1Text.color = answer == 1 ? selectedColor : defaultColor;
        if (answer == 1)
        {
            option1Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedColor;
        }
        else
        {
            option1Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = defaultColor;
        }
        //2번 보기 선택시
        option2Text.color = answer == 2 ? selectedColor : defaultColor;
        if (answer == 2)
        {
            option2Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedColor;
        }
        else
        {
            option2Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = defaultColor;
        }
        //3번 보기 선택시
        option3Text.color = answer == 3 ? selectedColor : defaultColor;
        if (answer == 3)
        {
            option3Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedColor;
        }
        else
        {
            option3Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = defaultColor;
        }
        //4번 보기 선택시
        option4Text.color = answer == 4 ? selectedColor : defaultColor;
        if (answer == 4)
        {
            option4Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = selectedColor;
        }
        else
        {
            option4Text.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = defaultColor;
        }
    }

    //플레이어가 선택한 답을 확인하는 함수(정답 제출하기 버튼 클릭시)
    public void CheckAnswer(int playerChoice)
    {
        if (attempts > 0 && canSubmit)
        {
            //제출 버튼 비활성화
            canSubmit = false;

            if (playerChoice.ToString() == quizData.a)
            {
                submitButton.gameObject.SetActive(false);

                message.gameObject.SetActive(true);
                message.text = "정답! 보물상자가 열렸습니다!\r\n포인트 100획득!\r\n축하드립니다!";
                settingTxt(false);
                Debug.Log("정답입니다! 보물상자가 열렸습니다.");


                //플레이어 이동 및 카메라 회전 다시 다 되게 해주고, 퀴즈 캔버스 닫기
                //2초 뒤에 OnBackButtonPressed를 실행
                StartCoroutine(DelayAndCloseCanvas(2f, true));

            }
            else
            {
                attempts--;

                message.gameObject.SetActive(true);
                message.text = "오답.. 정답은 " + quizData.a + $"번입니다.<br>" +"남은 기회는" + attempts + "번" + "<br>다시 도전해보세요.<br>";
                Sovingmessage.gameObject.SetActive(true);
                SovingmessageBG.gameObject.SetActive(true);
                Sovingmessage.text = $"<size=28><color=#00FFFF>{quizData.s}</color></size><br><br><color=#00FF10>문제를 더 잘 풀고싶다면 챗봇에게 힌트를 얻고 오세요!</color>";
                settingTxt(false);
                Debug.Log("오답입니다!");

                if (attempts > 0)
                {
                    Debug.Log($"남은 기회: {attempts}번");

                    //플레이어 이동 및 카메라 회전 다시 다 되게 해주고, 퀴즈 캔버스 닫기
                    // 2초 뒤에 OnBackButtonPressed를 실행
                    StartCoroutine(DelayAndCloseCanvas(4f, false));
                }
                else
                {
                    message.gameObject.SetActive(true);
                    message.text = $"오답.. 정답은 {quizData.a}번입니다.<br>" + $"남은 기회는 {attempts}번<br>" + "보물상자가 사라집니다.<br><br><color=#00FF10>문제를 더 잘 풀고싶다면 챗봇에게 힌트를 얻고 오세요!</color>";

                    Sovingmessage.gameObject.SetActive(true);
                    SovingmessageBG.gameObject.SetActive(true);
                    Sovingmessage.text = $"<size=28><color=#00FFFF>{quizData.s}</color></size><br>";
                    settingTxt(false);

                    Debug.Log($"보물상자 사라집니다");


                    // 2초 뒤에 OnBackButtonPressed를 실행
                    StartCoroutine(DelayAndCloseCanvas(2f, false));
                }
            }
            canSubmit = true;
        }
    }

    // 서버에 플레이어 ID를 보내는 코루틴
    IEnumerator SendPlayerId(string playerId)
    {
        // JSON 데이터 생성
        string jsonPayload = JsonUtility.ToJson(new { id = playerId });

        // 서버 URL
        string url = "http://221.163.19.142:9100/player-id";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            // JSON 데이터 첨부
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();

            // 헤더 설정 (Content-Type)
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("요청 보냄: " + jsonPayload);

            // 요청 보내기
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("서버 응답: " + request.downloadHandler.text);
            }
            else
            {
                //Debug.LogError("요청 실패: " + request.error);
            }
        }
    }

    IEnumerator TimeoutCoroutine()
    {

        //Debug.Log("서버 응답 없음. 로컬 퀴즈로 대체합니다.");
        settingTxt(false);
        message.gameObject.SetActive(true);
        message.text = "서버 연결에 실패하여 자체 보안 프로그램으로 대체합니다.";
        yield return new WaitForSeconds(5f); // 5초 대기
        LoadRandomLocalQuiz();
        ShowQuiz();
    }

    //로컬 퀴즈 리스트에서 랜덤으로 퀴즈 로드
    private void LoadRandomLocalQuiz()
    {
        int randomIndex = UnityEngine.Random.Range(0, preloadedQuizzes.Count);
        quizData = preloadedQuizzes[randomIndex];
        hasQuizLoaded = true;
    }


    //<해양쓰레기 발생원인>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_a()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "해양쓰레기의 발생 원인으로 옳은 것은?",
                n1 = "장마철 폭우나 태풍으로 인해 버려진 쓰레기가 하천을 따라 바다로 유입된다.",
                n2 = "해양쓰레기는 오직 조류의 배설물로만 발생한다.",
                n3 = "해양쓰레기는 고의적으로 바다에 버려진 폐기물만을 포함한다.",
                n4 = "해양쓰레기는 주로 바다에서만 발생하며 육상에서는 아무런 영향을 미치지 않는다.",
                a = "1",
                t = "해양쓰레기 발생원인",
                s = "장마철 폭우와 태풍은 쓰레기를 하천으로 유입시켜 해양쓰레기를 증가시키기 때문입니다."
            },
            new QuizData
            {
                q = "해양쓰레기 발생원인에 대한 설명 중 옳은 것은?",
                n1 = "해양쓰레기는 산업 폐기물에서만 발생한다.",
                n2 = "일상적인 생활에서 배출되는 쓰레기는 강을 따라 바다로 유입된다.",
                n3 = "해양쓰레기는 대기 중의 유해물질이 바다로 직접 유입되어 생성된다.",
                n4 = "해양쓰레기는 주로 해양 생물의 분해작용에 의해 발생한다.",
                a = "2",
                t = "해양쓰레기 발생원인",
                s = "일상생활에서 발생한 쓰레기가 하수나 강을 통해 해양으로 흘러들어가 바다의 오염을 초래합니다."
            },
            new QuizData
            {
                q = "해양쓰레기의 발생 원인으로 옳은 것은?",
                n1 = "일상생활에서 배출되는 쓰레기가 하천과 강을 따라 바다로 들어온다.",
                n2 = "해양쓰레기는 주로 해양 생물의 자연 분변으로만 발생한다.",
                n3 = "해양쓰레기는 대기 중에서만 발생하여 바다로 흘러든다.",
                n4 = "해양쓰레기는 해양에서 발생한 알갱이들의 자발적인 분해로만 존재한다.",
                a = "1",
                t = "해양쓰레기 발생원인",
                s = "일상생활 쓰레기가 수로를 통해 해양으로 유입되기 때문입니다."
            },
            new QuizData
            {
                q = "해양쓰레기의 발생 원인 중 옳은 것은?",
                n1 = "해양쓰레기는 자연적으로 생겨나는 것만 포함된다.",
                n2 = "해양쓰레기의 대부분은 공기 중에서 발생한다.",
                n3 = "일상생활에서 배출된 쓰레기가 하천을 통해 바다로 유입된다.",
                n4 = "해양쓰레기는 주로 해양 활동에서만 발생한다.",
                a = "3",
                t = "해양쓰레기 발생원인",
                s = "일상생활에서 발생한 쓰레기가 하천으로 흘러 들어가 바다에 유입되기 때문입니다."
            },
            new QuizData
            {
                q = "해양쓰레기의 발생원인으로 알맞은 것은?",
                n1 = "해양쓰레기는 대부분의 경우 산업에서 발생한 폐기물이다.",
                n2 = "일상적인 생활에서 배출된 쓰레기가 하천과 강을 따라 바다로 들어온다.",
                n3 = "해양쓰레기는 주로 해양 생물에서 발생한다.",
                n4 = "해양쓰레기는 오로지 선박에서 발생하는 쓰레기에 한정된다.",
                a = "2",
                t = "해양쓰레기 발생원인",
                s = "일상 생활의 쓰레기가 하천을 통해 바다로 유입되므로 해양쓰레기 발생의 주요 원인입니다."
            }
        };
    }

    //<해양쓰레기 현황>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_b()
    {

        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "해양쓰레기로 인해 가장 직접적인 영향을 받는 것은 무엇인가?",
                n1 = "미세먼지",
                n2 = "육상 식물",
                n3 = "바다생물",
                n4 = "인간",
                a = "3",
                t = "해양쓰레기 현황",
                s = "해양쓰레기가 바다생물에 미치는 영향은 생명체의 서식지 파괴와 오염으로 인한 건강 저하 때문입니다."

            },
            new QuizData
            {
                q = "해양쓰레기의 육상 기인 종류가 아닌 것은?",
                n1 = "수산업에서 발생한 폐어구",
                n2 = "소각장에서 발생한 재활용 불가능한 쓰레기",
                n3 = "자연재해로 인한 유실된 건축 자재",
                n4 = "산업폐수에서 발생한 오염물질",
                a = "1",
                t = "해양쓰레기 현황",
                s = "수산업에서 발생한 폐어구는 해양에서 유래하므로 육상 기인에 해당하지 않습니다."
            }
        };
    }

    //<해양쓰레기 피해 및 위험성>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_c()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "해양쓰레기로 인해 해양생태계에서 발생하는 문제로 옳은 것은?",
                n1 = "미세플라스틱이 해양생태계를 위협하고 바다를 오염시킨다.",
                n2 = "해양쓰레기는 해양생물에게 전혀 영향을 미치지 않는다.",
                n3 = "해양쓰레기는 주기적으로 자연적으로 분해되어 없어지는 경향이 있다.",
                n4 = "해양쓰레기는 주로 식물에서 발생하며, 해양에 영향을 미치지 않는다.",
                a = "1",
                t = "해양쓰레기 피해 및 위험성",
                s = "미세플라스틱은 해양생물의 섭취 및 서식지 파괴로 생태계를 위협합니다."
            },
            new QuizData
            {
                q = "해양쓰레기가 바다 생물에 미치는 영향으로 옳은 것은?",
                n1 = "해양쓰레기는 해양 생물의 번식에 긍정적인 영향을 미친다.",
                n2 = "해양쓰레기는 지구의 온도를 낮추는 데 기여한다.",
                n3 = "해양쓰레기로 인해 해양 생태계가 위협받고 생물이 살 수 없는 상황이 발생할 수 있다.",
                n4 = "해양쓰레기는 바다 생물에게 영양분을 제공하여 생태계를 활성화한다.",
                a = "3",
                t = "해양쓰레기 피해 및 위험성",
                s = "해양쓰레기는 생체의 서식지를 파괴하여 생명체의 생존을 위협합니다."
            },            
            new QuizData
            {
                q = "해양쓰레기의 피해 및 위험성에 대한 설명 중 올바른 것은?",
                n1 = "해양쓰레기는 생물들이 좋아하는 서식지를 제공한다.",
                n2 = "해양쓰레기는 해양생태계를 위협하며, 바다 밑바닥을 오염시켜 생물의 생존을 어렵게 만든다.",
                n3 = "해양쓰레기는 바다의 수온을 낮추는 효과가 있다.",
                n4 = "해양쓰레기는 바다의 생물 다양성을 증진시킨다.",
                a = "2",
                t = "해양쓰레기 피해 및 위험성",
                s = "해양쓰레기는 생물 서식지를 파괴하고, 생존에 필요한 환경을 악화시키기 때문입니다."
            },            
            new QuizData
            {
                q = "해양쓰레기로 인해 발생하는 문제 중 옳은 것은?",
                n1 = "해양쓰레기는 바다에서 완전히 분해되는 데 오랜 시간이 걸린다.",
                n2 = "해양쓰레기는 주로 어업에서 발생하며, 육상의 쓰레기와는 관련이 없다.",
                n3 = "해양쓰레기는 자연적으로 신속하게 분해되어 해양생태계에 아무런 영향을 미치지 않는다.",
                n4 = "해양쓰레기는 플라스틱으로만 구성되어 있으며, 생물에게는 아무런 위험을 주지 않는다.",
                a = "1",
                t = "해양쓰레기 피해 및 위험성",
                s = "해양쓰레기는 분해가 매우 느려, 환경 문제를 유발합니다."
            },
            new QuizData
            {
                q = "해양쓰레기로 인해 바다 생물에게 미치는 영향에 대한 설명으로 옳은 것은?",
                n1 = "미세플라스틱은 해양생태계를 위협하고 바다를 오염시킨다.",
                n2 = "미세플라스틱은 바다 생물에게 긍정적인 영향을 미치고 생태계를 개선한다.",
                n3 = "해양쓰레기는 언제나 바다의 수온을 높여서 생물에게 이로운 영향을 준다.",
                n4 = "해양쓰레기는 바다 생물의 먹이사슬에 영향을 주지 않는다.",
                a = "1",
                t = "해양쓰레기 피해 및 위험성",
                s = "미세플라스틱은 해양 생물의 섭취 및 생태계 균형을 교란시켜 오염을 초래합니다."
            }
        };
    }

    //<해양쓰레기 피해 사례>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_d()
    {

        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "해양쓰레기로 인해 발생하는 피해 중 바다생물에게 직접적인 피해를 주는 원인으로 거리가 먼 것은?",
                n1 = "해양쓰레기를 먹이로 오인해 해양 생물이 먹는 경우",
                n2 = "어선의 그물에 해양쓰레기가 걸리는 경우",
                n3 = "피서객들이 해변에서 요가를 하는 경우",
                n4 = "해양쓰레기가 바다에서 유해 화학물질을 방출하는 경우",
                a = "3",
                t = "해양쓰레기 피해 사례",
                s = "피서객들의 요가는 해양쓰레기와 무관하여, 바다생물에 직접적인 피해를 주지 않기 때문입니다."
            }
        };
    }

    //<태평양 해양 쓰레기 섬>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_e()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "태평양 해양 쓰레기섬의 규모 중 맞는 것은?",
                n1 = "태평양 해양 쓰레기섬은 단지 약 10만 ㎢의 면적을 차지한다.",
                n2 = "약 160만 ㎢의 면적을 차지한다.",
                n3 = "태평양 해양 쓰레기섬은 약 100만 ㎢의 면적을 차지한다.",
                n4 = "태평양 해양 쓰레기섬은 약 50만 ㎢의 면적을 차지한다.",
                a = "2",
                t = "태평양 쓰레기섬",
                s = "태평양 해양 쓰레기섬은 약 160만 ㎢로, 이는 면적이 광범위하여 해양 생태계에 큰 영향을 미칩니다."
            },
            new QuizData
            {
                q = "태평양 해양 쓰레기섬(GPGP)에 대한 설명 중 옳은 것은?",
                n1 = "태평양 해양 쓰레기섬은 플라스틱만으로 형성된 지역이다.",
                n2 = "태평양 해양 쓰레기섬은 매년 지구에 새로운 해양 쓰레기가 증가함에 따라 줄어들고 있다.",
                n3 = "해양 쓰레기섬의 규모는 약 160만 ㎢로, 무게는 약 8만 톤이다.",
                n4 = "태평양 해양 쓰레기섬은 약 100만 ㎢로, 무게는 약 20만 톤이다.",
                a = "3",
                t = "태평양 쓰레기섬",
                s = "GPGP의 면적과 무게는 연구에 기반한 정확한 수치로, 해양 쓰레기 문제의 심각성을 나타냅니다."
            },
            new QuizData
            {
                q = "태평양 해양 쓰레기섬(GPGP)의 발견자는 누구인가?",
                n1 = "알렉스 존슨(Alex Johnson)",
                n2 = "제임스 카메론(James Cameron)",
                n3 = "찰스 무어(Charles Moore)",
                n4 = "리처드 브랜슨(Richard Branson)",
                a = "3",
                t = "태평양 쓰레기섬",
                s = "찰스 무어는 1997년 태평양에서 해양 쓰레기섬을 발견하여 이를 최초로 보고하였습니다."
            },
            new QuizData
            {
                q = "태평양 쓰레기섬(GPGP)에서 발견된 플라스틱 쓰레기의 개수는 대략 얼마인가?",
                n1 = "약 5천억 개",
                n2 = "약 1조 8천억 개",
                n3 = "약 100억 개",
                n4 = "약 500억 개",
                a = "2",
                t = "태평양 쓰레기섬",
                s = "GPGP에서 플라스틱 쓰레기 수는 해양 생태계 영향 분석 결과에 기반하여 약 1조 8천억 개로 추산되었습니다."
            }
        };
    }

    //<미세 플라스틱>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_f()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "미세 플라스틱이 해양 생물에게 미치는 영향으로 옳은 것은?",
                n1 = "미세 플라스틱은 바다의 수질 개선에 기여한다.",
                n2 = "미세 플라스틱은 해양 생물의 성장에만 긍정적인 영향을 준다.",
                n3 = "미세 플라스틱은 해양 생물에게 아무런 영향을 미치지 않는다.",
                n4 = "물리적인 상처, 장폐색, 산화 스트레스 등의 여러 가지 부작용을 겪을 수 있다.",
                a = "4",
                t = "미세플라스틱",
                s = "미세 플라스틱은 해양 생물에게 물리적 손상과 생리적 스트레스를 유발하여 건강을 해칩니다."
            },
            new QuizData
            {
                q = "미세 플라스틱이 해양 생물에 미치는 영향으로 알맞은 것은?",
                n1 = "미세 플라스틱은 해양 생물에게 도움이 되며 영양분을 제공한다.",
                n2 = "해양 생물은 미세 플라스틱을 소화할 수 있어 건강에 아무런 영향을 미치지 않는다.",
                n3 = "해양 생물은 미세 플라스틱을 섭취하여 여러 가지 부작용을 겪을 수 있다.",
                n4 = "미세 플라스틱은 해양 생물의 생태계에 전혀 영향을 미치지 않는다.",
                a = "3",
                t = "미세플라스틱",
                s = "미세 플라스틱은 해양 생물의 건강에 해로운 화학물질을 포함해 부작용을 일으킬 수 있습니다."
            },
            new QuizData
            {
                q = "미세플라스틱이 해양 생물에 미치는 영향을 올바르게 설명한 것은?",
                n1 = "미세플라스틱은 해양 생물에 전혀 영향을 미치지 않는다.",
                n2 = "해양 동물은 미세플라스틱을 섭취하면 더 건강해진다.",
                n3 = "미세플라스틱은 해양 생물의 생태계에서 자연스럽게 분해된다.",
                n4 = "미세플라스틱을 섭취한 해양 동물은 물리적인 상처, 장폐색, 에너지 할당 감소 등 여러 가지 부작용을 겪을 수 있다.",
                a = "4",
                t = "미세플라스틱",
                s = "미세플라스틱 섭취로 생물에 물리적 피해와 에너지 감소가 발생하기 때문입니다."
            },
             new QuizData
            {
                 q = "미세 플라스틱이 해양 생물에게 미치는 부작용으로 알맞은 것은?",
                n1 = "미세 플라스틱은 해양 생물의 성장에 긍정적인 효과를 준다.",
                n2 = "물리적인 상처, 장폐색, 에너지 할당 감소 등이 발생할 수 있다.",
                n3 = "미세 플라스틱은 해양 생물에게 아무런 영향을 미치지 않는다.",
                n4 = "미세 플라스틱은 해양 생물에게 특수한 영양소를 제공한다.",
                a = "2",
                t = "미세플라스틱",
                s = "미세 플라스틱은 해양 생물에 물리적 손상을 주며, 장 폐쇄를 유발하고 에너지 소비를 증가시킵니다."
            }
        };
    }

    //<허베이스피릿호 원유유출 사고>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_g()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "허베이스피릿호 원유유출사고로 인해 발생한 해양오염의 주된 원인은 무엇인가?",
                n1 = "원유유출사고는 선원의 실수로 인해 발생했다.",
                n2 = "허베이스피릿호는 태풍으로 인해 침몰했다.",
                n3 = "삼성중공업 소유의 크레인이 예인선과의 연결이 끊어져 유조선과 충돌했다.",
                n4 = "사고의 주된 원인은 유조선의 자율운항 시스템 고장이다.",
                a = "3",
                t = "허베이스피릿호 원유유출 사고",
                s = "해양오염의 주된 원인은 크레인의 조작 미숙으로 인해 유조선과의 충돌이 발생했기 때문입니다."
            },
            new QuizData
            {
                q = "허베이스피릿호 원유유출 사고의 주된 원인은 무엇인가?",
                n1 = "허베이스피릿호의 엔진이 고장이 나서 사고가 발생하였다.",
                n2 = "예인선과의 연결이 끊어져 크레인이 유조선과 충돌하였다.",
                n3 = "해양경찰의 안전 점검을 무시해서 사고가 발생하였다.",
                n4 = "허베이스피릿호는 원유 대신 생수만을 운반하고 있었다.",
                a = "2",
                t = "허베이스피릿호 원유유출 사고",
                s = "예인선과의 연결이 끊어짐으로써 유조선의 조타가 불가능해져, 크레인이 충돌하는 사고가 발생하였기 때문입니다."
            },
            new QuizData
            {
                q = "허베이스피릿호 원유 유출 사고로 인해 발생한 해양오염 지역 중 올바른 것은?",
                n1 = "허베이스피릿호 사고로 인해 발생한 해양오염은 제주도 근처에서만 발생했다.",
                n2 = "태안해안국립공원을 중심으로 해상 203㎢, 해안 54㎢의 해역오염이 발생했다.",
                n3 = "해양오염이 발생하지 않았다.",
                n4 = "허베이스피릿호 사고로 인해 부산에서만 오염이 발생했다.",
                a = "2",
                t = "허베이스피릿호 원유유출 사고",
                s = "태안해안국립공원은 유출 사고의 주요 피해 지역으로, 조사 결과 해당 면적이 해양오염의 범위를 정확히 나타냅니다."
            },
            new QuizData
            {
                q = "허베이스피릿호 원유유출 사고와 관련하여 옳은 것은?",
                n1 = "사고 지역은 제주도 앞바다였다.",
                n2 = "허베이스피릿호 원유유출 사고는 2010년 4월 20일에 발생하였다.",
                n3 = "2007년 12월 7일 충남 앞 바다에서 발생하여 약 12,547㎘의 원유가 유출되었다.",
                n4 = "이 사고로 유출된 원유의 양은 약 5,000㎘였다.",
                a = "3",
                t = "허베이스피릿호 원유유출 사고",
                s = "허베이스피릿호 사고는 원유 유출의 대표적 사례로, 유출량이 명확히 기록되었기에 정답입니다."
            }
        };
    }

    //<검은 공 사건>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_h()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "호주 시드니 해변에 떠밀려온 검은 공의 성분으로 알려진 것은?",
                n1 = "검은 공은 물고기의 주요 먹이로 사용된다.",
                n2 = "검은 공은 주로 나무로 이루어져 있다.",
                n3 = "탄화수소 기반 오염물질이다.",
                n4 = "검은 공은 해양 생물의 가치를 높이기 위한 해양 미술 작품이다.",
                a = "3",
                t = "호주 검은 공 사건",
                s = "검은 공은 탄화수소를 포함, 해양 오염의 주원인으로 파악됩니다."
            },
            new QuizData
            {
                q = "호주 시드니 해변에 떠밀려온 검은 공은 어떤 성분으로 되어 있는가?",
                n1 = "검은 공은 식물성 기름으로 만들어져 있다.",
                n2 = "검은 공은 주로 철로 만들어져 있다.",
                n3 = "탄화수소 기반 오염물질이다.",
                n4 = "검은 공은 금속으로 만들어져 있으며, 해양 생물의 배설물이다.",
                a = "3",
                t = "호주 검은 공 사건",
                s = "검은 공은 탄화수소 기반으로, 석유 제품에서 유래된 오염물질입니다."
            },
            new QuizData
            {
                q = "호주 시드니 해변에 떠밀려온 검은 공은 무엇으로 알려져 있는가?",
                n1 = "해양에서 자연적으로 생성되는 해양식물의 일종이다.",
                n2 = "해양 생물의 배설물로 알려져 있는 자연적인 물질이다.",
                n3 = "호주의 시드니 해변 근처에서 자주 발견되는 루비다.",
                n4 = "탄화수소 기반 오염물질이다.",
                a = "4",
                t = "호주 검은 공 사건",
                s = "검은 공은 탄화수소 기반 오염물질로, 해양 오염의 지표입니다."
            },
            new QuizData
            {
                q = "호주 시드니 해변에 떠밀려온 '검은 공'에 대한 설명으로 올바른 것은 무엇인가?",
                n1 = "검은 공은 화산 폭발로 인해 발생한 자연적인 현상이다.",
                n2 = "검은 공은 단순한 해조류로 이루어져 있다.",
                n3 = "검은 공은 탄화수소 기반 오염물질로, 석유가 다른 물질과 응고되어 생성되었다.",
                n4 = "검은 공은 해양 생물의 배설물로 형성되었다.",
                a = "3",
                t = "호주 검은 공 사건",
                s = "검은 공은 석유 오염으로 인한 탄화수소의 응집체로, 해양 오염을 나타냅니다."
            },
            new QuizData
            {
                q = "호주 시드니 해변에서 발견된 검은 공의 성분으로 정확한 것은 무엇입니까?",
                n1 = "검은 공의 성분은 금속 합금이다.",
                n2 = "검은 공의 성분은 방사성 물질이다.",
                n3 = "검은 공의 성분은 식물성 오일이다.",
                n4 = "탄화수소 기반 오염물질",
                a = "4",
                t = "호주 검은 공 사건",
                s = "검은 공은 탄화수소의 집합체로, 오염물질로 판단됩니다."
            },
        };
    }

    //<약품 사고>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_i()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "약품 사고로 인해 해양 생물에 부정적인 영향을 미치는 것은 무엇인가?",
                n1 = "해양 생물에 대한 약품의 영향을 연구하는 것은 필요하지 않다.",
                n2 = "항우울제에 노출된 민물가재는 포식자를 마주해도 피하지 않는다.",
                n3 = "약품 사고는 해양 생물에게 어떤 영향을 미치지 않는다.",
                n4 = "항생제가 노출된 민물고기는 생존율이 높아진다.",
                a = "2",
                t = "약품 사고",
                s = "항우울제가 민물가재의 행동을 변화시켜, 포식자에게 발각될 위험을 증가시킵니다."
            },
            new QuizData
            {
                q = "해양 생물에 미치는 약품 오염의 영향으로 옳은 것은?",
                n1 = "항생제에 노출된 해양 생물의 생존율은 항상 증가한다.",
                n2 = "항우울제에 노출된 민물가재가 포식자에 반응하지 않는 행동이 생존에 부정적 영향을 미친다.",
                n3 = "약품 오염은 해양 생물에게 전혀 영향을 미치지 않는다.",
                n4 = "항우울제에 노출된 민물가재는 포식자의 반응을 더욱 잘하게 된다.",
                a = "2",
                t = "약품 사고",
                s = "해당 행동은 포식자에게 더 쉽게 잡히게 하여 생존에 부정적 영향을 미칩니다."
            }
        };
    }

    //<폐어구에 걸린 돌고래>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_j()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "제주바다에서 발견된 폐어구에 걸린 돌고래의 문제와 관련하여 옳은 것은?",
                n1 = "남방큰돌고래는 제주에서 발견되지 않는 종이다.",
                n2 = "제주에서 폐어구에 걸린 돌고래는 2주 만에 구출되었다.",
                n3 = "폐어구는 돌고래의 생존에 전혀 영향을 미치지 않는다.",
                n4 = "제주에서 폐어구에 걸린 남방큰돌고래 종달이가 10개월 만에 낚싯줄 제거에 성공했다.",
                a = "4",
                t = "폐어구에 걸린 돌고래",
                s = "제주에서 남방큰돌고래가 폐어구로부터 구출된 사례를 통해 해양 생태계 보호의 중요성을 강조합니다."
            },
            new QuizData
            {
                q = "제주바다에서 폐어구에 걸린 돌고래인 '종달이'는 어떤 문제를 겪었나요?",
                n1 = "종달이는 먹이를 찾지 못해 힘들어했다.",
                n2 = "낚싯줄에 감겨서 활동이 제한되었다.",
                n3 = "돌고래는 폐어구에 걸렸지만, 그로 인해 수동적인 이동을 할 수 있었다.",
                n4 = "종달이는 바다의 오염물질로 인해 영향을 받았다.",
                a = "2",
                t = "폐어구에 걸린 돌고래",
                s = "'종달이'는 낚싯줄에 감겨 있어 활동이 제한되고, 생명에 위협을 받아 힘들어했습니다."
            },
            new QuizData
            {
                q = "폐어구에 걸린 돌고래의 문제에 대한 설명으로 올바른 것은?",
                n1 = "폐어구로 인해 돌고래의 생명과 생태계가 위협받고 있다.",
                n2 = "돌고래는 폐어구로부터 완전히 안전하다.",
                n3 = "폐어구는 돌고래의 생명에 아무런 영향을 미치지 않는다.",
                n4 = "폐어구는 돌고래를 보살피는 데 도움이 된다.",
                a = "1",
                t = "폐어구에 걸린 돌고래",
                s = "폐어구는 돌고래를 걸리게 하여 생명과 생태계에 위협을 초래하기 때문입니다."
            },
            new QuizData
            {
                q = "폐어구에 걸린 돌고래와 관련하여 옳은 것은?",
                n1 = "폐어구는 돌고래의 수영 능력을 향상시킨다.",
                n2 = "제주바다에서는 폐어구 문제가 전혀 발생하지 않는다.",
                n3 = "폐어구에 걸린 돌고래는 항상 구조된다.",
                n4 = "폐어구로 인해 해양 생명이 위협받고 있다.",
                a = "4",
                t = "폐어구에 걸린 돌고래",
                s = "폐어구는 해양 생물을 잡아 죽이고, 건강을 해치므로 해양 생명이 위협받습니다."
            },
            new QuizData
            {
                q = "폐어구에 걸린 돌고래가 제주바다에서 발견된 이유로 알맞은 것은?",
                n1 = "바다의 해양 쓰레기가 증가하고 있기 때문이다.",
                n2 = "돌고래는 폐어구와 무관하게 자연에서만 발견된다.",
                n3 = "폐어구는 바다에서 발생하지 않고, 주로 강에서 발견된다.",
                n4 = "제주바다의 돌고래들은 해양 쓰레기와는 아무런 관계가 없다.",
                a = "1",
                t = "폐어구에 걸린 돌고래",
                s = "해양 쓰레기 증가로 폐어구가 많아져, 돌고래가 걸릴 위험이 커졌기 때문입니다."
            }
        };
    }

    //<바다 거북>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_k()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
              q = "우리나라 연안에서 발견된 바다 거북의 폐사체 중 플라스틱을 섭식한 비율은 얼마인가?",
                n1 = "70.5%",
                n2 = "90.2%",
                n3 = "82.4%",
                n4 = "55.1%",
                a = "3",
                t = "우리나라 바다 거북",
                s = "해양 거북 폐사체에서 플라스틱 섭식이 높은 비율은 해양 오염의 심각성을 나타냅니다."
            },
            new QuizData
            {
                q = "우리나라 연안에서 발견된 바다거북의 34마리 중 플라스틱을 섭식한 비율로 알맞은 것은?",
                n1 = "0마리",
                n2 = "31마리",
                n3 = "10마리",
                n4 = "28마리",
                a = "4",
                t = "우리나라 바다 거북",
                s = "34마리 중 28마리가 플라스틱을 섭식했다면, 섭식 비율은 약 82.4%로 계산됩니다."
            },
            new QuizData
            {
                q = "우리나라 연안에서 발견된 바다거북 중, 해양 플라스틱 섭식한 비율은?",
                n1 = "30%",
                n2 = "50%",
                n3 = "100%",
                n4 = "80%",
                a = "4",
                t = "우리나라 바다 거북",
                s = "바다거북은 해양 플라스틱을 섭식하는 비율이 높아, 생태계 오염의 직접적인 영향을 받습니다."
            },
            new QuizData
            {
                q = "우리나라 연안에서 발견된 바다 거북 중 플라스틱을 섭식한 비율에 대한 설명으로 알맞은 것은?",
                n1 = "모든 바다 거북이 플라스틱을 섭식하는 것으로 알려져 있다.",
                n2 = "플라스틱을 섭식한 바다 거북은 전혀 발견되지 않았다.",
                n3 = "10마리 중 8마리가 플라스틱을 먹은 것으로 확인되었다.",
                n4 = "10마리 중 2마리만이 플라스틱을 섭식한 것으로 확인되었다.",
                a = "3",
                t = "우리나라 바다 거북",
                s = "플라스틱 섭식 비율이 높아 환경 문제를 드러냅니다."
            }
        };
    }

    //<상괭이>_로컬 퀴즈 리스트 초기화
    private void InitializePreloadedQuizzes_l()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
              q= "상괭이",
              n1= "해양쓰레기는 해양 생태계를 보호한다.",
              n2= "해양쓰레기는 선박 사고의 원인이 될 수 있다.",
              n3= "해양쓰레기는 바다의 수온을 낮춘다.",
              n4= "해양쓰레기는 해양 생물의 번식을 촉진한다.",
              a= "2",
              t = "상괭이",
              s = "풀이"
            }
        };
    }


    //테마정보 없을 시_로컬 퀴즈 리스트는 검은공으로 초기화
    private void InitializePreloadedQuizzes()
    {
        preloadedQuizzes = new List<QuizData>
        {
            new QuizData
            {
                q = "호주 시드니 해변의 검은 공 사건과 관련하여 옳은 것은?",
                n1 = "검은 공은 해양 쓰레기와는 관계없이 별개로 발생한다.",
                n2 = "검은 공은 자연적인 해양 자원으로 알려져 있다.",
                n3 = "검은 공은 탄화수소 기반 오염물질로, 바다로 빠져나간 석유와 이물질이 응고돼 생성되었다.",
                n4 = "검은 공은 주로 해양 생물에 의해 생성된 것이다.",
                a = "3",
                t = "호주 검은 공 사건",
                s = "검은 공은 석유와 이물질의 응집체입니다."
            },
            new QuizData
            {
                q = "검은 공 사건과 관련하여 거리가 가까운 것은?",
                n1 = "검은 공 사건은 해양 생물의 번식에 긍정적인 영향을 미쳤다.",
                n2 = "해변에 떠밀려온 탄화수소 기반 오염물질로 확인된 검은 공이다.",
                n3 = "검은 공은 바다에서 발견된 플라스틱 조각으로만 이루어져 있다.",
                n4 = "검은 공 사건은 육상의 폐기물 처리 실패로 인한 것이다.",
                a = "2",
                t = "호주 검은 공 사건",
                s = "검은 공 사건은 탄화수소 오염물질과 관련이 있습니다."
            },
            new QuizData
            {
                q = "해양 생물에 미치는 약품으로 인한 영향에 대한 설명 중 옳은 것은?",
                n1 = "미복용 약품이 하수구를 통해 흘러들어가 어패류에 생태계 교란을 일으킨다.",
                n2 = "약품은 하수구를 통해 바다로 흘러갈 수 없다.",
                n3 = "의약품이 해양 생물에게 직접적으로 유익한 영향을 미친다.",
                n4 = "해양 생물은 약품에 대해 면역성이 강해 영향을 받지 않는다.",
                a = "1",
                t = "약품 사고",
                s = "약품이 하수구로 유입되어 생태계에 해로운 영향을 미치기 때문입니다."
            },
            new QuizData
            {
                q = "해양쓰레기로 인해 바다생물이 입는 피해 중 올바른 것은?",
                n1 = "바다생물은 해양쓰레기를 소화시킬 수 없다.",
                n2 = "해양쓰레기는 해양생물의 생태계에 긍정적인 영향을 미친다.",
                n3 = "해양쓰레기는 대부분 자연적으로 분해된다.",
                n4 = "해양쓰레기는 주로 해저에서 자연적으로 정화된다.",
                a = "1",
                t = "해양쓰레기 피해 사례",
                s = "바다동물은 플라스틱을 소화하지 못합니다."
            },
        };
    }

    // 뒤로가기 버튼 이벤트 메서드
    private void OnBackButtonPressed()
    {
        //ModelingTreasureCamera
        //켜주기
        ModelingTreasureCamera.enabled = false;

        photonView.RPC(nameof(isSovingRPC), RpcTarget.All, false);

        // 상자 뚜껑 닫기 및 사운드
        door.transform.localEulerAngles = new Vector3(0, door.transform.localEulerAngles.y, door.transform.localEulerAngles.z);
        audioSet.OBJSFXPlay(1,false);

        //퀴즈 캔버스 닫고, 커서 꺼주기
        QuizCanvas.gameObject.SetActive(false);
        photonView.RPC(nameof(isCursorRPC), RpcTarget.All, false);
        //PlayerInfo.instance.isCusor = false;

        //플레이어 이동x, 플레이어 쓰레기 수집x, 카메라 회전x, 손전등x
        DisableOrAblePlayerMovement(true);
        DiableOrAblePlayerGetTrash(true);
        DisableOrAbleCameraRotation(true);
        DisableOrAbleFlashLight(true);

        //상호작용 다시 되게 해주기
        GetComponent<Interaction_Base>().useTrue = false;
    }

    [PunRPC]
    public void isCursorRPC(bool TorF)
    {
        PlayerInfo.instance.isCusor = TorF;

    }

    void settingTxt(bool TorF)
    {
        questionText.gameObject.SetActive(TorF);
        option1Text.gameObject.SetActive(TorF);
        option2Text.gameObject.SetActive(TorF);
        option3Text.gameObject.SetActive(TorF);
        option4Text.gameObject.SetActive(TorF);
    }

    // 새로운 코루틴 추가
    private IEnumerator DelayAndCloseCanvas(float delay, bool isCorrect)
    {
        yield return new WaitForSeconds(delay); // 주어진 시간만큼 대기

        message.gameObject.SetActive(false);
        message.text = "";
        Sovingmessage.gameObject.SetActive(false);
        SovingmessageBG.gameObject.SetActive(false);
        Sovingmessage.text = "";

        settingTxt(true);
        OnBackButtonPressed(); // 뒤로가기 버튼을 눌러 UI를 닫음

        //정답일 때
        if (isCorrect)
        {
            //상호작용 했을 때 뜨는 문구 바꿔주기
            GetComponent<Interaction_Base>().intername = "보급상자에서 포인트 획득!";

            
            //상자 뚜껑 열리는 애니메이션 및 사운드
            animator.SetTrigger("openDoor");
            audioSet.OBJSFXPlay(0,false);

            //1초 뒤
            yield return new WaitForSeconds(1f);

            //정답 맞췄으니까 포인트 지급
            PlayerInfo.instance.PointPlusOrMinus(100);

            //1초 뒤
            OnBackButtonPressed(); // 뒤로가기 버튼을 눌러 UI를 닫음

            //보급상자 삭제
            Destroy(gameObject);
        }

        //정답아닐 때
        if (!(attempts > 0))
        {
            //1초 뒤
            yield return new WaitForSeconds(1f);

            OnBackButtonPressed(); // 뒤로가기 버튼을 눌러 UI를 닫음

            //보급상자 삭제
            Destroy(gameObject);

        }


        DisableOrAblePlayerMovement(true);
        DiableOrAblePlayerGetTrash(true);
        DisableOrAbleCameraRotation(true);
        DisableOrAbleFlashLight(true);

        submitButton.gameObject.SetActive(true);

    }



    //플레이어 이동 못하게/하게 하는 함수
    public void DisableOrAblePlayerMovement(bool TrueOrFalse)
    {
        PlayerMove playerMove = PlayerInfo.instance.player.transform.GetComponentInChildren<PlayerMove>();
        playerMove.isMove = TrueOrFalse;
    }

    //플레이어가 쓰레기 수집 못하게/하게 하는 함수
    public void DiableOrAblePlayerGetTrash(bool TrueOrFalse)
    {
        Vacuum vacuum = PlayerInfo.instance.player.transform.GetComponentInChildren<Vacuum>();
        vacuum.isStop = TrueOrFalse;
    }

    //카메라가 회전 안하게/회전 하게 하는 함수
    public void DisableOrAbleCameraRotation(bool TrueOrFalse)
    {
        CameraRotate cameraRotate = PlayerInfo.instance.player.transform.GetComponentInChildren<CameraRotate>();
        cameraRotate.useRotX = TrueOrFalse;
        cameraRotate.useRotY = TrueOrFalse;
    }

    //분리수거 미니게임 할 때 손전등 가능하게/불가능하게 하는 함수
    public void DisableOrAbleFlashLight(bool TrueOrFalse)
    {
        FlashLightHandler flashLightHandler = PlayerInfo.instance.player.transform.GetComponentInChildren<FlashLightHandler>();

        PlayerInfo.instance.isFlashStop = !TrueOrFalse;
        flashLightHandler.flashLight.intensity = TrueOrFalse == true ? 10 : 0;
    }
}

