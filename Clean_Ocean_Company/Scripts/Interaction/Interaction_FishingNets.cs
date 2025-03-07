using UnityEngine;

public class Interaction_FishingNets : MonoBehaviour
{
    BoxCollider boxCollider;
    Coroutine speechCoroutine;
    public ChatSpeechBubble speech;
    public GameObject speechBubble;

    public Interaction_MoveGet interaction_MoveGet;

    public bool cleanStart;
    bool hasOilQuestTag = false;


    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        //print("hasOilQuestTag: " + hasOilQuestTag);
        if (cleanStart)
        {
            QuestComplest_WhaleVacuum();
        }
    }


    public void QuestComplest_WhaleVacuum()
    {
        hasOilQuestTag = false;

        //자식 중에 기름태그 없는지 확인하고 없으면 이펙트 띄워주기
        for (int i = 0; i < transform.childCount; i++)
        {
            //각 자식의 태그가 Oil_Quest인지 확인
            if (transform.GetChild(i).CompareTag("Oilbolb"))
            {
                if (transform.childCount < 12)
                {
                    break;
                }
                hasOilQuestTag = true;
                //기름이 있으면 더 이상 체크할 필요 없음
                break;
            }
        }

        //for문 다 돌았는데도 기름 태그 없어서 hasOilQuestTag가 false일 때는 상괭이 위치에 이펙트 띄워주기
        if (!hasOilQuestTag)
        {
            cleanStart = false;
            interaction_MoveGet.isStart = true;
            boxCollider.enabled = false;
        }
    }

    public void InteractionEvent()
    {
        speechBubble.SetActive(true);
        if (speechCoroutine != null)
        {
            StopCoroutine(speechCoroutine);
        }
        speechCoroutine = StartCoroutine(speech.PlayOwn("이건 무슨 소리지? 혹시?", speechCoroutine));

        cleanStart = true;
    }

}
