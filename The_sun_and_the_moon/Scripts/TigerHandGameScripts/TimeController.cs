using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Photon.Pun;

namespace MyGameNamespace
{
    public class TimeController : MonoBehaviour
    {
        public TextMeshProUGUI countdownText;  // 카운트다운을 표시할 UI Text
        public GameObject failPanel;
        public HandMovement handMovement;
        public Image timerImage; // Timer 이미지

        // BGM 재생을 위해 넣을 AudioSource
        public AudioSource bgmSource;
        public AudioClip bgmClip; // 제한 시간이 10초가 되었을 때 재생할 BGM

        private float timeRemaining = 40.0f;  // 제한시간 40초로 설정
        private bool isTimerRunning = false;  // 타이머가 실행 중인지 확인하는 플래그
        private bool isBgmPlaying = false; // BGM이 재생 중인지 확인하는 플래그
        private int countdownTime = 3;        // 카운트다운 시간을 3초로 설정

        void Start()
        {
            failPanel.SetActive(false);

            if (handMovement == null)
            {
                handMovement = FindObjectOfType<HandMovement>();
            }
            if (bgmSource == null)
            {
                bgmSource = gameObject.AddComponent<AudioSource>(); // BGM 재생을 위한 AudioSource 이다.
            }

            StartCoroutine(StartCountdown());  // 게임 시작 시 카운트다운 코루틴 시작
        }

        public IEnumerator StartCountdown()
        {
            while (countdownTime > 0)
            {
                countdownText.text = countdownTime.ToString();  // 카운트다운 시간 출력
                yield return new WaitForSeconds(1f);  // 1초 대기
                countdownTime--;
            }
            countdownText.text = "시작!";  // 카운트다운이 끝나면 "시작!" 출력
            yield return new WaitForSeconds(1f);
            countdownText.gameObject.SetActive(false);  // 카운트다운 텍스트 비활성화
            StartTimer();  // 카운트다운 종료 후 타이머 시작
            if (handMovement != null)
            {
                handMovement.StartGame();  // 카운트다운 종료 후 손 움직임 시작
            }
        }

        void Update()
        {
            if (isTimerRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;  // 매 프레임마다 시간 감소
                    UpdateTimerUI();  // UI 업데이트

                    // If, 남은 시간이 10초가 되었고 BGM이 아직 재생되지 않았을 경우 BGM 재생시킨다.
                    if(timeRemaining <= 10.0f && !isBgmPlaying)
                    {
                        PlayBGM();
                    }
                }
                else
                {
                    timeRemaining = 0;
                    isTimerRunning = false;
                    StopBGM(); // 미니 게임 종료 시 BGM도 정지시킨다.
                    EndGame();  // 제한시간이 다 되면 EndGame 함수 호출
                }
            }
        }

        void StartTimer()
        {
            isTimerRunning = true;  // 타이머 실행 시작
        }

        void UpdateTimerUI()
        {
            // 시간에 따른 이미지 fillAmount 업데이트
            if (timerImage != null)
            {
                timerImage.fillAmount = Mathf.Clamp01(timeRemaining / 40.0f); // 40초를 기준으로 fillAmount 값을 줄임
            }
        }

        public void StopTimer()  // 타이머를 중지하는 함수 추가
        {
            isTimerRunning = false;
        }

        void EndGame()
        {
            failPanel.SetActive(true);
            if (handMovement != null)
            {
                handMovement.StopHandMovement();  // 손 움직임 멈춤
            }
            FindObjectOfType<HandsPunchList>().StopGame();

            // Scene 동작 정지 또는 종료 처리
            Debug.Log("Time's up! Ending the game.");
            Time.timeScale = 0;  // 게임 정지
            // SceneManager.LoadScene("EndScene");  // 또는 종료 화면으로 전환 (필요에 따라 추가)

            // 퀘스트 진행도 상승 및 씬 이동 작업
            Time.timeScale = 1;
            QuestManager.questManager.QuestAddProgress(5, 0, 1);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(2); // 게임 종료시 HomeScene 이동
            }
        }

        void PlayBGM()
        {
            bgmSource.clip = bgmClip;  // 재생할 BGM 클립 설정
            bgmSource.loop = false;    // 반복 재생 안함
            bgmSource.Play();          // BGM 재생 시작
            isBgmPlaying = true;       // BGM 재생 중 플래그 설정
        }

        void StopBGM()
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();  // BGM 정지
            }
            isBgmPlaying = false;  // BGM 재생 중 플래그 해제
        }
    }
}