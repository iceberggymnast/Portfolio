using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class StartButtonMassage : MonoBehaviour
{
    public GameObject tutorialPanel;
    public Button tutorialReadyButton;
    public TextMeshProUGUI ready1Text;
    public TextMeshProUGUI ready2Text;
    public float delayBetweenTexts = 1.0f; // Ready1과 Ready2 사이의 딜레이
    public string gameSceneName = "Minigame_massage";
    public ConnectionMGRMassage connectionMGRMassage;

    private bool isTutorialReady = false;

    void Start()
    {
        ready1Text.gameObject.SetActive(false);
        ready2Text.gameObject.SetActive(false);

        tutorialReadyButton.onClick.AddListener(OnTutorialReadyButtonClick);
    }

    // '준비 완료' 버튼 클릭 시 호출되는 메서드
    void OnTutorialReadyButtonClick()
    {
        isTutorialReady = true;
        ready1Text.gameObject.SetActive(true);  // Ready1 텍스트 활성화
        StartCoroutine(ActivateReady2AfterDelay());  // 일정 딜레이 후 Ready2 활성화
    }

    IEnumerator ActivateReady2AfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenTexts);  // 지정된 시간 대기
        ready2Text.gameObject.SetActive(true);  // Ready2 텍스트 활성화
        connectionMGRMassage.PlayerReady();  // 준비 완료 알림 전송
    }

    // 플레이어가 로비에 들어왔을 때 호출되는 메서드
    public void UpdateRoomStatus(bool status)
    {
        tutorialReadyButton.gameObject.SetActive(status);  // 튜토리얼 준비 버튼 활성화
    }
}
