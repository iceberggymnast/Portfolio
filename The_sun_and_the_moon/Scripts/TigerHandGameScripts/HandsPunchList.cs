using MyGameNamespace;
using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HandsPunchList : MonoBehaviourPunCallbacks
{
    [Serializable]
    public class HandPunch
    {
        public string result;
    }

    private HandPunch handsPunch;
    public HandMovement handMovement;
    public TimeController timeController;

    public Animator playerAnimatorSun; // 해님 플레이어 에셋 애니메이터
    public Animator playerAnimatorMoon; // 달님 플레이어 에셋 애니메이터

    // 게임 진행 시 화면 위에서 확인 할수 있는 플레이어 캐릭터 이미지와 텍스트
    public GameObject ProfileSun;
    public GameObject ProfileMoon;

    public GameObject ProfileNameSun;
    public GameObject ProfileNameMoon;

    public Image scoreBarImage;
    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject leftMissPanel;
    public GameObject rightMissPanel;
    public GameObject resultPanel; // 결과 패널 추가
    public GameObject failPanel; // 실패 패널 추가
    public GameObject image; // 미니 게임에 나오는 우측 이미지
    public BGMManager bgmManager; // BGM 매니저 추가

    public float moveSpeed = 1.0f; // Text Panel이 이동할 때 적용될 속력
    public float moveDistance = 150.0f; // 이동 거리

    private int sum = 0; // 점수를 저장할 정수형 변수
    private int maxScore = 5; // 최대 점수
    private bool isGameActive = true; // 게임이 진행 중인지 확인하는 변수
    private bool[] playerCleared = new bool[2]; // 플레이어의 클리어 상태 저장하는 bool 타입 배열

    void Start()
    {
        handsPunch = new HandPunch();
        handsPunch.result = "test";

        // 모든 UI 요소를 초기화
        if (leftPanel == null || rightPanel == null || leftMissPanel == null || rightMissPanel == null || resultPanel == null || failPanel == null || scoreBarImage == null)
        {
            Debug.LogError("UI 요소가 할당되지 않았습니다. 모두 확인해 주세요.");
        }

        image.SetActive(true);
        leftPanel.SetActive(false);
        rightPanel.SetActive(false);
        leftMissPanel.SetActive(false);
        rightMissPanel.SetActive(false);
        resultPanel.SetActive(false);
        failPanel.SetActive(false);

        // 이미지 초기화
        if (scoreBarImage != null)
        {
            scoreBarImage.fillAmount = 0; // 초기 상태에서 이미지를 0으로 설정 (채워지지 않은 상태)
        }

        if (PhotonNetwork.NickName == "해님")
        {
            playerAnimatorMoon.gameObject.SetActive(false);
            ProfileSun.SetActive(true);
            ProfileNameSun.SetActive(true);
        }
        else
        {
            playerAnimatorSun.gameObject.SetActive(false);
            ProfileMoon.SetActive(true);
            ProfileNameMoon.SetActive(true);
        }
    }

    void Update()
    {
        if (!isGameActive) return; // 게임이 비활성화된 경우 Update 중단

        // 점수에 따라 이미지 업데이트
        UpdateScoreImage();

        // 점수가 5가 되었을 때 게임을 비활성화하고 + 제한 시간 중지시킴.
        if (sum >= maxScore)
        {
            CheckForGameClear(); // 클리어 상태 확인
        }
    }

    // 점수에 따라 이미지 크기나 fillAmount를 조정하는 함수
    void UpdateScoreImage()
    {
        if (scoreBarImage != null)
        {
            // 현재 점수에 따라 fillAmount를 계산 (0에서 1 사이 값)
            float fillValue = (float)sum / maxScore;
            scoreBarImage.fillAmount = Mathf.Clamp(fillValue, 0f, 1f); // 0에서 1 사이로 값 제한
        }
    }

    public void CreateFromJSON(string jsonString)
    {
        if (!isGameActive) return; // 게임이 비활성화된 경우 JSON 처리 중단

        handsPunch = JsonUtility.FromJson<HandPunch>(jsonString);
        if (handsPunch.result != "")
        {
            Debug.Log(handsPunch.result);
            CheckAndIncreaseScore(handsPunch.result); // 왼손과 오른손을 구별하기 위한 수정
        }
    }

    void CheckAndIncreaseScore(string result)
    {
        // 로컬 플레이어만 해당 코드 블록 실행
        //if (!photonView.IsMine) return;

        if (handMovement != null)
        {
            bool isLeftHandActive = handMovement.isLeftHandMoving[0];
            bool isRightHandActive = handMovement.isRightHandMoving[0];

            // 왼손 모션일 때
            if (result == "LEFT")
            {
                // 자신의 캐릭터 애니메이터만 트리거
                if (playerAnimatorSun.gameObject.activeSelf)
                {
                    print("해님 IsLeft");
                    playerAnimatorSun.SetTrigger("IsLeft");
                }
                if (playerAnimatorMoon.gameObject.activeSelf)
                {
                    print("달님 IsLeft");
                    playerAnimatorMoon.SetTrigger("IsLeft");
                }

                if (isLeftHandActive)
                {
                    sum += 1;
                    Debug.Log($"왼손을 쳐서 점수가 {sum}점 올랐습니다.");
                    StartCoroutine(ShowPanel(leftPanel));

                    // 점수 상승 효과음 및 BGM 재생
                    bgmManager.PlayScoreIncreaseBGM();
                    bgmManager.PlayActionBGM();

                    // 이미지 업데이트
                    UpdateScoreImage();

                    // 점수가 최대치에 도달했을 때 즉시 게임 중지
                    if (sum >= maxScore)
                    {
                        StopGame();
                    }
                }
                else
                {
                    StartCoroutine(ShowMissText(leftMissPanel));
                    bgmManager.PlayActionBGM(); // 미스 시에도 BGM 재생
                }
            }

            // 오른손 모션일 때
            else if (result == "RIGHT")
            {
                // 자신의 캐릭터 애니메이터만 트리거
                if (playerAnimatorSun.gameObject.activeSelf)
                {
                    print("해님 IsRight");
                    playerAnimatorSun.SetTrigger("IsRight");
                }
                if (playerAnimatorMoon.gameObject.activeSelf)
                {
                    print("달님 IsRight");
                    playerAnimatorMoon.SetTrigger("IsRight");
                }

                if (isRightHandActive)
                {
                    sum += 1;
                    Debug.Log($"오른손을 쳐서 점수가 {sum}점 올랐습니다.");
                    StartCoroutine(ShowPanel(rightPanel));

                    // 점수 상승 효과음 및 BGM 재생
                    bgmManager.PlayScoreIncreaseBGM();
                    bgmManager.PlayActionBGM();

                    // 이미지 업데이트
                    UpdateScoreImage();

                    // 점수가 최대치에 도달했을 때 즉시 게임 중지
                    if (sum >= maxScore)
                    {
                        StopGame();
                    }
                }
                else
                {
                    StartCoroutine(ShowMissText(rightMissPanel));
                    bgmManager.PlayActionBGM(); // 미스 시에도 BGM 재생
                }
            }

            // 손의 상태를 false로 설정
            handMovement.isLeftHandMoving[0] = false;
            handMovement.isRightHandMoving[0] = false;
        }
        else
        {
            Debug.LogError("handMovement가 null입니다!");
        }
    }

    // 클리어 상태 확인 및 모든 플레이어에게 알림
    void CheckForGameClear()
    {
        photonView.RPC("UpdatePlayerClearStatus", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber - 1); // 클리어 상태를 업데이트
        photonView.RPC("StopGame", RpcTarget.All); // 모든 플레이어에게 게임 종료 RPC 호출
    }

    [PunRPC]
    public void UpdatePlayerClearStatus(int playerIndex)
    {
        playerCleared[playerIndex] = true; // 특정 플레이어의 클리어 상태 업데이트 -> 클리어 시 true 처리
        CheckForAllPlayersClear(); // 모든 플레이어의 클리어 상태 확인
    }

    void CheckForAllPlayersClear()
    {
        // 모든 플레이어가 클리어한 경우
        if (playerCleared[0] && playerCleared[1])
        {
            Debug.Log("모든 플레이어가 클리어했습니다.");
            StartCoroutine(ShowResultPanel());
        }
    }

    IEnumerator ShowPanel(GameObject panel)
    {
        if (panel != null)
        {
            panel.SetActive(true); // 패널 활성화
            yield return new WaitForSeconds(2.0f); // 2초 대기
            panel.SetActive(false); // 패널 비활성화
        }
    }

    IEnumerator ShowMissText(GameObject missPanel)
    {
        if (missPanel != null)
        {
            missPanel.SetActive(true);
            //RectTransform rectTransform = missPanel.GetComponent<RectTransform>();
            //if (rectTransform != null)
            //{
            //    rectTransform.anchoredPosition = new Vector3(0, 0, 0); // y축 0에서 시작
            //}
            //StartCoroutine(MovePanel(missPanel));
            yield return new WaitForSeconds(1.0f);
            missPanel.SetActive(false);
        }
    }

    // ResultPanel 활성화 및 Scene 전환 하는 함수
    IEnumerator ShowResultPanel()
    {
        if (resultPanel != null)
        {
            image.SetActive(false);
            resultPanel.SetActive(true);
            RectTransform rectTransform = resultPanel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector3(0, 0, 0); // y축 0에서 시작
            }
            yield return new WaitForSeconds(2.0f); // 2초 대기
            StartCoroutine(LoadHomeScene()); // 씬 로드
        }
    }

    IEnumerator MovePanel(GameObject panel)
    {
        RectTransform rectTransform = panel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            Vector3 startPos = rectTransform.anchoredPosition;
            Vector3 endPos = startPos + new Vector3(0, moveDistance, 0); // y축 150으로 이동

            float elapsedTime = 0.0f;
            float moveDuration = 1.0f;

            // Move upwards
            while (elapsedTime < moveDuration)
            {
                rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, (elapsedTime / moveDuration));
                elapsedTime += Time.deltaTime * moveSpeed;
                yield return null;
            }
            rectTransform.anchoredPosition = endPos;
        }
    }

    // ResultPanel 활성화 및 Scene 전환 하는 함수 (모든 Player에게 적용 된다.)
    public void StopGameWithFail()
    {
        if (!isGameActive) return; // 이미 게임이 끝났으면 중단

        isGameActive = false; // 게임을 비활성화
        StartCoroutine(ShowFailPanel()); // 실패 패널 활성화
    }

    IEnumerator ShowFailPanel()
    {
        if (failPanel != null)
        {
            image.SetActive(false);
            failPanel.SetActive(true);
            RectTransform rectTransform = failPanel.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector3(0, 0, 0); // y축 0에서 시작
            }
            yield return new WaitForSeconds(2.0f); // 2초 대기
            StartCoroutine(LoadHomeScene()); // 씬 로드
        }
    }

    public string GetTigerPunchResult()
    {
        return handsPunch.result;
    }

    // 모든 플레이어의 게임을 멈추는 함수
    [PunRPC]
    public void StopGame()
    {
        if (!isGameActive) return; // 이미 게임이 끝났으면 중단

        isGameActive = false; // 게임을 비활성화

        // 제한시간을 멈추기 위한 로직 추가
        TimeController timeController = FindObjectOfType<TimeController>();
        if (timeController != null)
        {
            timeController.StopTimer(); // 타이머 멈추기
        }

        // 패널 상태 초기화
        resultPanel.SetActive(false); // 결과 패널 비활성화
        failPanel.SetActive(false); // 실패 패널 비활성화

        // 점수에 따라 패널 활성화
        if (sum >= maxScore) 
        {
            StartCoroutine(ShowResultPanel()); // 결과 패널 활성화
        }
        else
        {
            StartCoroutine(ShowFailPanel()); // 실패 패널 활성화
        }
    }

    IEnumerator LoadHomeScene()
    {
        yield return new WaitForSeconds(2.0f); // 2초 대기
        QuestManager.questManager.QuestAddProgress(5, 0, 1);
        SceneManager.LoadScene("home");
    }

    // 플레이어 2명이 게임 클리어 조건을 만족해야 다음 Scene으로 넘어갈 수 있도록 코드 수정 + 결과를 둘 다 알수 있도록 해야한다.
    // 불필요한 부분은 주석처리
}