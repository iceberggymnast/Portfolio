using UnityEngine;

public class StartButton : MonoBehaviour
{
    public MiniGameTutorialMGR tutorialMGR; // MiniGameTutorialMGR 스크립트 참조

    // Start 버튼을 눌렀을 때 호출되는 함수
    public void OnStartButtonClicked()
    {
        int tutorialIndex = GetTutorialIndexFromQuestState(); // 퀘스트 상태로부터 인덱스 값 가져오기

        //tutorialMGR.ShowTutorial(tutorialIndex); // 해당 인덱스의 튜토리얼 활성화
    }

    int GetTutorialIndexFromQuestState()
    {
        // 퀘스트 상태를 기반으로 적절한 튜토리얼 인덱스를 반환하는 로직을 여기에 작성
        // 임시로 0~3의 랜덤 값을 반환하도록 설정 (실제 퀘스트 상태에 맞게 수정 필요)
        return Random.Range(0, 4);
    }
}