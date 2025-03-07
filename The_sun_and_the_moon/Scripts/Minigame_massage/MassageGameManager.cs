using Photon.Pun;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MassageGameManager : MonoBehaviourPun
{
    //AI가 보내는 result가 있는 스크립트
    JsonToClass jsonToClass;
    //해님 게임오브젝트
    GameObject Player_Sun;
    //달님 게임오브젝트
    GameObject Player_moon;
    //해님 애니메이션
    PlayerAni playerAni;
    //달님 애니메이션
    PlayerAni_moon playerAni_Moon;
    //엄마 애니메이션
    MotherAni MotherAni;

    //갱신할 result값
    [SerializeField]
    int before_result = 0;
    //실시간 result값
    [SerializeField]
    int result;
    //ai가 값을 누적해서 줘서 이걸로 조정해야 함
    int resultOffset = 0;

    //안마 한번 할 때마다 할머니 앉은 위치에 나타날 효과
    public Transform effectPosition;
    //효과 프리팹
    public GameObject massageEffect;

    //승패를 말해주는 Text
    public TextMeshProUGUI gameResult;

    //게임 시작 Text -> Countdown으로 대체
    //public TextMeshProUGUI startMessage;

    //제한시간 게이지 UI
    public Image timeBar;
    ////해님이 게임진행도 게이지 UI
    public Image scoreBar;


    //게임 결과 띄울 때 띄울 panel -> 새 결과 UI를 사용하기 위해 아예 주석처리 하겠습니다. (Yeon)
    //public Image panel;

    public GameObject resultPanel;
    public GameObject failPanel;

    //게임 제한 시간은 10초
    float totalTime = 20.0f;
    //현재 흐르는 게임  시간
    float currentTime = 0f;
    //게임 시작했는지 여부
    bool isStart = false;
    //목표 점수 3점을 채우면 성공했다는 문구가 뜨고 게임 종료됨
    int maxScore = 3;

    //해님달님 프로필 이미지
    GameObject Profile_sun;
    GameObject Profile_moon;

    //해님달님 프로필 이름
    GameObject ProfileName_sun;
    GameObject ProfileName_moon;

    //각 플레이어가 게임 성공했는지 확인하는 변수
    int totalPlayers = 2;  // 예시로 두 명의 플레이어로 설정
    int playersWhoWon = 0;

    //점수 획득 시 나올 사운드
    public AudioSource ScoreSoundSource;

    // BGM 클립 추가 (Yeon)
    public AudioClip bgmClip;
    private AudioSource bgmAudioSource;
    private bool isBgmPlayed = false; // BGM 재생 여부 확인

    // 카운트 다운 관련 변수
    public TextMeshProUGUI countdownText;
    private int countdownTime = 3;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        jsonToClass = FindObjectOfType<JsonToClass>();
        //PhotonNetwork.NickName = "해님";  //test
        //해님달님, 엄마 애니메이션
        //해님달님별로 오브젝트 활성화 또는 비활성화
        Player_Sun = GameObject.Find("Player_sun").gameObject;
        Player_moon = GameObject.Find("Player_moon").gameObject;
        playerAni = GameObject.Find("Player_sun").GetComponentInChildren<PlayerAni>();
        playerAni_Moon = GameObject.Find("Player_moon").GetComponentInChildren<PlayerAni_moon>();

        if (PhotonNetwork.NickName == "해님")
        {
            Player_Sun.gameObject.SetActive(true);
            Player_moon.gameObject.SetActive(false);
        }
        else
        {
            Player_moon.gameObject.SetActive(true);
            Player_Sun.gameObject.SetActive(false);
        }
        MotherAni = GameObject.Find("Mother").GetComponentInChildren<MotherAni>();

        //해님달님별로 프로필 이미지 설정, 프로필 이름 설정
        //닉네임이 해님이면 해님이용 프로필 이미지 및 프로필 이름 활성화
        Profile_sun = GameObject.Find("Canvas_WebAndCam").transform.GetChild(5).transform.GetChild(0).gameObject;
        ProfileName_sun = GameObject.Find("Canvas_WebAndCam").transform.GetChild(6).transform.GetChild(0).gameObject;
        Profile_moon = GameObject.Find("Canvas_WebAndCam").transform.GetChild(5).transform.GetChild(1).gameObject;
        ProfileName_moon = GameObject.Find("Canvas_WebAndCam").transform.GetChild(6).transform.GetChild(1).gameObject;

        if (PhotonNetwork.NickName == "해님")
        {
            Profile_sun.gameObject.SetActive(true);
            ProfileName_sun.gameObject.SetActive(true);
        }
        //닉네임이 달님이면 달님이용 프로필 이미지 및 프로필 이름 활성화
        else
        {
            Profile_moon.gameObject.SetActive(true);
            ProfileName_moon.gameObject.SetActive(true);
        }

        //시작할 때 panel끄기 -> ResultPanel과 FailPanel로 교체 (Yeon)
        //panel.gameObject.SetActive(true);

        resultPanel.SetActive(false);
        failPanel.SetActive(false);

        //게임 시작전 게임시작 메시지 표시
        //startMessage.gameObject.SetActive(true);-> Countdown으로 대체
        Invoke("StartGame", 1.0f); // 1초 후에 게임 시작

        gameResult.text = "";

        //사운드 소스 가져오기
        if (ScoreSoundSource != null)
        {
            ScoreSoundSource = GetComponent<AudioSource>();
        }

        // BGM을 재생하기 위한 AudioSource 추가 (Yeon)
        bgmAudioSource = gameObject.AddComponent<AudioSource>();
        bgmAudioSource.clip = bgmClip;

        // BGM을 처음부터 재생하지 않고 나중에 재생 (Yeon)
        bgmAudioSource.playOnAwake = false;

        // 게임 시작 전 카운트 다운을 시작한다.
        StartCoroutine(StartCountdown());
    }

    void Update()
    {
        //치트키
        if (Input.GetKeyDown(KeyCode.J))
        {
            //홈씬으로 이동 바로 하기
            GameWin();
        }

        //내거일 때만 실행
        //if (!photonView.IsMine) return;

        //게임이 이미 시작했으면 return
        if (!isStart) return;

        //result가 null이 아닐 때 값을 갱신
        if (jsonToClass.resultInt != null)
        {
            // AI에서 받은 누적된 result 값에서 오프셋을 빼서 현재 세션에서의 값으로 변환
            result = jsonToClass.resultInt.result - resultOffset;
        }

        //게임 진행 시간 누적
        currentTime += Time.deltaTime;
        print(currentTime);

        //timeBar는 계속 줄어든다.
        timeBar.fillAmount = 1 - (currentTime / totalTime);

        // 시간에 따라 BGM 재생 로직 추가
        if (currentTime >= totalTime / 2 && !isBgmPlayed) // 시간이 10초 이하로 남았을 때
        {
            bgmAudioSource.Play(); // BGM 재생
            isBgmPlayed = true;    // BGM이 한 번만 재생되도록 설정
        }

        // 20초가 지났는데, 2점을 못 채우면
        if (currentTime >= totalTime && result < maxScore)
        {
            //게임 종료 후 다시 시작
            GameOver();
        }

        if (before_result < result)
        {
            ScoreSoundSource.Play();
            //안마 한번 실행할 때의 애니메이션 넣기
            if (PhotonNetwork.NickName == "해님")
            {
                playerAni.MassageAnimation();
            }
            else
            {
                playerAni_Moon.MassageAnimation_moon();
            }
            MotherAni.MassageAnimation_Mother();

            //카운트가 올라간만큼 갱신해주기
            before_result = result;

            // 점수바 채워주기(result/7점)
            scoreBar.fillAmount = (float)result / maxScore;

            //카운트가 올라갈 때마다 할머니 위치의 EffectPosition위치에(할머니 앉은 바닥쪽) 체력회복하는 Effect넣기
            GameObject MassageEffectInstance;
            MassageEffectInstance = Instantiate(massageEffect);
            MassageEffectInstance.transform.position = effectPosition.position;

            //이펙트는 0.5초 동안만 띄우고 사라짐
            StartCoroutine(EffectDelay(MassageEffectInstance, 1.0f));

            // 만약 result가 최대 점수인 7에 도달하면 게임 승리
            if (result >= maxScore)
            {
                GameWin();
            }
        }
    }

    public IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();  // 카운트다운 시간 출력
            yield return new WaitForSeconds(1f);  // 1초 대기
            countdownTime--;
        }
        countdownText.text = "시작!";  // 카운트다운이 끝나면 "Start!" 출력
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);  // 카운트다운 텍스트 비활성화

        // 게임 시작
        StartGame();  // 카운트다운 종료 후 게임 시작
    }

    void GameWin()
    {
        isStart = false; // 게임 중단

        // ResultPanel과 FailPanel로 교체 (Yeon)
        //panel.gameObject.SetActive(true); // 판넬 활성화

        resultPanel.gameObject.SetActive(true);
        failPanel.gameObject.SetActive(false);

        gameResult.text = "잼잼 안마 성공!"; // 승리 문구 표시
        QuestManager.questManager.QuestAddProgress(3, 0, 1);

        //플레이어가 승리했음을 알리고 다른 플레이어와 동기화
        photonView.RPC(nameof(PlayerWon), RpcTarget.All);
        Debug.Log("PlayerWon RPC called");

        ////안마게임 끝나고 다시 홈씬으로 이동할 때는 
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.LoadLevel(2);
        //}
    }

    [PunRPC]
    void PlayerWon()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playersWhoWon++;  // 승리한 플레이어 수 증가
            Debug.Log($"A player has won. Total players won: {playersWhoWon}");

            // 모든 플레이어가 승리했는지 확인
            if (playersWhoWon >= totalPlayers)
            {
                Debug.Log("All players have won the game!");

                // 홈 씬으로 이동
                PhotonNetwork.LoadLevel(2);
            }
        }
    }

    void GameOver()
    {
        isStart = false; // 게임 중단

        // ResultPanel과 FailPanel로 교체 (Yeon)
        //panel.gameObject.SetActive(true); // 판넬 활성화

        resultPanel.gameObject.SetActive(false);
        failPanel.gameObject.SetActive(true);
        gameResult.text = "잼잼 안마 실패.."; // 실패 문구 표시
        Invoke("RestartGame", 2.0f); // 2초 후에 게임 재시작
    }


    //어머니한테 뜨는 이펙트뜨다가  일정 시간되면 없어지는 코루틴
    IEnumerator EffectDelay(GameObject effectInstance, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(effectInstance);
    }

    void StartGame()
    {
        //게임시작할 때 panel끄고, 시작메시지도 끄기
        //panel.gameObject.SetActive(false);

        // ResultPanel과 FailPanel로 교체 (Yeon)
        resultPanel.gameObject.SetActive(false);
        failPanel.gameObject.SetActive(false);
        //startMessage.gameObject.SetActive(false); -> Countdown으로 대체

        // 현재 누적된 result 값을 오프셋으로 저장
        if (jsonToClass.resultInt != null)
        {
            resultOffset = jsonToClass.resultInt.result;
        }

        //게임 시작
        isStart = true;
        currentTime = 0f;
        before_result = 0;
        result = 0;

        //scoreBar도 초기화
        scoreBar.fillAmount = 0f;
    }

    void RestartGame()
    {
        //승패 여부 알려줬던 text도 끄기
        gameResult.text = "";
        //점수bar도 0으로 초기화
        scoreBar.fillAmount = 0f;
        result = 0;
        //게임 재시작할 때도 panel끄고, 시작메시지도 끄기

        // ResultPanel과 FailPanel로 교체 (Yeon)
        //panel.gameObject.SetActive(false);

        resultPanel.gameObject.SetActive(false);
        failPanel.gameObject.SetActive(false);

        //startMessage.gameObject.SetActive(true);
        // 1초 후에 재시작
        Invoke("StartGame", 1.0f);
    }
}
