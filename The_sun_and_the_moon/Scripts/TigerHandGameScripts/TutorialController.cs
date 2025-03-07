using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject tutorialPanel; // 튜토리얼 패널 오브젝트
    public GameObject timeControllerObject; // TimeController가 붙어있는 오브젝트
    public GameObject handsPunchListObject; // HandsPunchList가 붙어있는 오브젝트

    void Start()
    {
        // 게임 시작 전에는 TimeController와 HandsPunchList를 비활성화
        timeControllerObject.SetActive(false);
        handsPunchListObject.SetActive(false);
    }

    // StartButton 클릭 시 호출될 메서드
    public void OnStartButtonClick()
    {
        // StartButton 클릭 시 튜토리얼 패널 비활성화
        tutorialPanel.SetActive(false);

        // TimeController와 HandsPunchList 활성화
        timeControllerObject.SetActive(true);
        handsPunchListObject.SetActive(true);
    }
}