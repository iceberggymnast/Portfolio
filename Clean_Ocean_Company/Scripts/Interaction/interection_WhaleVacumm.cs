using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interection_WhaleVacumm : MonoBehaviour
{
    bool hasOilQuestTag = false;
    public GameObject efxPrefabs;
    public GameObject efxPos;

    public bool cleanStart;

    void Start()
    {
        Interaction_Base interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = cleaningOilOnWhale;
    }
    private void Update()
    {
        //print("hasOilQuestTag: " + hasOilQuestTag);
        if (cleanStart)
        {
            QuestComplest_WhaleVacuum();
        }

    }

    public void cleaningOilOnWhale()
    {
        //기름 태그 있는지 체크하게 cleanStart값 켜주고
        cleanStart = true;
    }

    public void QuestComplest_WhaleVacuum()
    {
        hasOilQuestTag = false;

        //자식 중에 기름태그 없는지 확인하고 없으면 이펙트 띄워주기
        for (int i = 0; i < transform.childCount; i++)
        {
            //각 자식의 태그가 Oil_Quest인지 확인
            if (transform.GetChild(i).CompareTag("Oilbolb_Quest"))
            {
                hasOilQuestTag = true;
                //기름이 있으면 더 이상 체크할 필요 없음
                break;
            }
        }

        //for문 다 돌았는데도 기름 태그 없어서 hasOilQuestTag가 false일 때는 상괭이 위치에 이펙트 띄워주기
        if (!hasOilQuestTag)
        {
            //고래 아래의 이펙트 pos에 이펙트 띄움
            GameObject efx = Instantiate(efxPrefabs, efxPos.transform.position, Quaternion.identity);

            //퀘스트 완료라고 띄워주기
            PopupUI popupUI = FindObjectOfType<PopupUI>();
            popupUI.PopupActive("상괭이에게 뭍은 기름 흡입기로 씻어주기 퀘스트 완료!", "", 2.0f);

            //2초 뒤
            StartCoroutine(twoScondLater(efx));

            //퀘스트 완료 등록해주기
            QuestManger.instance.Questproceed(0, 1);

            //퀘스트 완료 했으니까 기름 태그 있는지 확인할 필요없어져서 cleanStart꺼주기
            cleanStart = false;

            //상호작용 했을 때 뜨는 문구 바꿔주기
            GetComponent<Interaction_Base>().intername = "이미 씻겨준 상괭이";
        }
        else
        {
            GetComponent<Interaction_Base>().useTrue = true;
        }
    }

    IEnumerator twoScondLater(GameObject efx)
    {
        yield return new WaitForSeconds(3.0f);

        //이펙트 삭제
        Destroy(efx);
    }
}
