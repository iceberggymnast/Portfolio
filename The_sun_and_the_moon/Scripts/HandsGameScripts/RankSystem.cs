using UnityEngine;
using TMPro;

public class RankSystem : MonoBehaviour
{
    [SerializeField]
    private int maxRankCount = 4; // Max Rank 개수
    [SerializeField]
    private GameObject textPrefab; // Rank 정보 출력하는 Text UI Prefab
    [SerializeField]
    private Transform panelRankInfo; // Text가 배치되는 부모 Panel Transform

    private RankData[] rankDataArray; // Rank 정보 저장하는 RankData Type Array
    private int currentIndex = 0;

    private void Awake()
    {
        rankDataArray = new RankData[maxRankCount];

        LoadRankData(); // 1. 기존의 Rank 정보 불러오기

        CompareRank(); // 2. 1등부터 차례대로 현재 Stage에서 획득한 점수와 비교하기

        PrintRankData(); // 3. Rank Data 출력

        SaveRankData(); // 4. 새로운 Rank 정보 저장
    }

    void LoadRankData()
    {
        for (int i = 0; i < maxRankCount; ++i)
        {
            rankDataArray[i].score = PlayerPrefs.GetInt("RankScore" + i);
            rankDataArray[i].normalHandHitCount = PlayerPrefs.GetInt("RankNormalHandHitCount" + i);
            rankDataArray[i].redHandHitCount = PlayerPrefs.GetInt("RankRedHandHitCount" + i);
        }
    }

    private void SpawnText(string print, Color color)
    {
        //Instantiate()로 textPrefab 복사체를 생성 후, clone 변수에 저장
        GameObject clone = Instantiate(textPrefab);
        //clone의 TextMeshProGUI Component 정보를 얻어와 text 변수에 저장
        TextMeshProUGUI text = clone.GetComponent<TextMeshProUGUI>();

        //생성한 Text UI Object의 부모를 panelRankInfo Object로 설정
        clone.transform.SetParent(panelRankInfo);

        //자식으로 등록되면서 Size가 변환 가능해서 Size를 1로 설정
        clone.transform.localScale = Vector3.one;
        //Text UI에 출력할 내용과 Font Color 설정
        text.text = print;
        text.color = color;
    }

    void CompareRank()
    {
        // 현재 Stage에서 달성한 정보
        RankData currentData = new RankData();
        currentData.score = PlayerPrefs.GetInt("CurrentScore");
        currentData.normalHandHitCount = PlayerPrefs.GetInt("CurrentNormalHandHitCount");
        currentData.redHandHitCount = PlayerPrefs.GetInt("CurrentRedHandHitCount");

        // 1 ~ 4등의 점수와 현재 Stage에서 달성한 점수 비교
        for (int i = 0; i < maxRankCount; ++i)
        {
            if (currentData.score > rankDataArray[i].score)
            {
                // Rank에 들어갈 수 있는 횟수를 달성하면 for문 중지
                currentIndex = i;
                break;
            }
        }

        // currentData의 등수 아래로 점수를 한칸씩 밀어서 저장
        for (int i = maxRankCount - 1; i > 0; --i)
        {
            rankDataArray[i] = rankDataArray[i - 1];

            if (currentIndex == i - 1)
            {
                break;
            }
        }

        // 새로운 점수를 Rank에 집어넣기
        rankDataArray[currentIndex] = currentData;
    }

    void PrintRankData()
    {
        Color color = Color.white;

        for (int i = 0; i < maxRankCount; ++i)
        {
            // 방금 Play의 점수가 Rank에 등록되면 색상을 노란색으로 표시
            color = currentIndex != i ? Color.white : Color.yellow;

            // Text - TextMeshPro 생성 및 원하는 Data 출력
            SpawnText((i + 1).ToString(), color);
            SpawnText(rankDataArray[i].score.ToString(), color);
            SpawnText(rankDataArray[i].normalHandHitCount.ToString(), color);
            SpawnText(rankDataArray[i].redHandHitCount.ToString(), color);
        }
    }

    void SaveRankData()
    {
        for (int i = 0; i < maxRankCount; ++i)
        {
            PlayerPrefs.SetInt("RankScore" + i, rankDataArray[i].score);
            PlayerPrefs.SetInt("RankNormalHandHitCount" + i, rankDataArray[i].normalHandHitCount);
            PlayerPrefs.SetInt("RankRedHandHitCount" + i, rankDataArray[i].redHandHitCount);
        }
    }
}

[System.Serializable]
public struct RankData
{
    public int score;
    public int normalHandHitCount;
    public int redHandHitCount;
}