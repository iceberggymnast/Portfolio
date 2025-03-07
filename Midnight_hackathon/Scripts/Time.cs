using UnityEngine;
using UnityEngine.UI;

public class HandGestureTimeGame : MonoBehaviour
{
    public GameObject clockUI; // 시계 UI
    public GameObject correctGestureEffect; // 정답 피드백 효과
    public Text timerText; // 타이머를 표시할 UI 텍스트
    public GameObject gameOverPanel; // 게임 종료 시 표시할 UI 패널
    
    // private ARTrackedImageManager trackedImageManager;
    private float timeRemaining = 60f; // 60초 제한시간
    private bool isGameOver = false;
    void Start()
    {
        // trackedImageManager = GetComponent<ARTrackedImageManager>();
        // trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        UpdateTimerUI();
    }
    
    void Update()
    {
        if (isGameOver)
            return;
    
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            EndGame();
        }
    }
    
    // private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    // {
    //     if (isGameOver)
    //         return;
    //
    //     foreach (var trackedImage in eventArgs.updated)
    //     {
    //         if (IsGestureCorrect(trackedImage))
    //         {
    //             TriggerSuccessEffect();
    //             EndGame();
    //         }
    //     }
    // }
    
    // private bool IsGestureCorrect(ARTrackedImage trackedImage)
    // {
    //     // 손동작을 기반으로 시간을 판별하는 로직을 작성
    //     // 여기서는 간단히 True를 반환하지만, 실제 구현에서는 손의 위치와 시계의 시간 비교 로직을 추가해야 함.
    //     return true;
    // }
    
    private void TriggerSuccessEffect()
    {
        correctGestureEffect.SetActive(true);
        // 추가 피드백: 소리, 점수 증가 등
    }
    
    private void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(timeRemaining).ToString() + "s";
    }
    
    private void EndGame()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        // 게임 종료 처리: 결과 표시, 다시 시작 버튼 등
    }
}