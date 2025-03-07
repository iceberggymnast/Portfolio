using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    // 접힐 수 있는 퀘스트 정보 표시 UI
    public List<GameObject> listFoldContent;

    // 표시 UI의 사이즈 설정용 리스트
    public List<RectTransform> foldContentSize;
    // 버튼의 사이즈
    float buttonSize = 56;
    // 여유 공간
    float margin = 5;

    public GameObject questInfoPrefab;
    

    void Start()
    {
        
    }

    void Update()
    {
        FoldContent();
    }

    // 세부 퀘스트 리스트를 접고 필 수 있게 하는 함수
    public void ListFold(int number)
    {
        if (listFoldContent[number].activeSelf)
        {
            listFoldContent[number].SetActive(false);
        }
        else
        {
            listFoldContent[number].SetActive(true);
        }
    }

    // 해당 값에 맞는 퀘스트 콘텐츠 생성
    public void FoldContent()
    {
        
        // 받을 수 있는 퀘스트
        if (listFoldContent[0].activeSelf)
        {
            // 리스트에 저장된 퀘스트의 갯수를 불러온다..
            int count = QuestManager.questManager.QuestcanReceive.Count;

            MakeUI(0, count);

        }

        // 진행중인 퀘스트
        if (listFoldContent[1].activeSelf)
        {
            // 리스트에 저장된 퀘스트의 갯수를 불러온다..
            int count = QuestManager.questManager.questProgress.Count;

            MakeUI(1, count);

        }

        // 완료한 퀘스트
        if (listFoldContent[2].activeSelf)
        {
            // 리스트에 저장된 퀘스트의 갯수를 불러온다..
            int count = QuestManager.questManager.questComplete.Count;

            MakeUI(2, count);
        }

        Refresh();

    }

    // 넣어야 하는 퀘스트 갯수 만큼 생성해주고 제거해준다.
    public void MakeUI(int listValue , int count)
    {
        // 갯수만큼 사이즈를 계산하여 늘려준다.
        foldContentSize[listValue].sizeDelta = new Vector2(foldContentSize[1].sizeDelta.x, (buttonSize + margin) * count);

        // 현재 생성된 자식 프리팹 갯수
        int currentUINum = listFoldContent[listValue].transform.childCount;

        // 생성 또는 제거해야 할 값을 구한다
        int result = count - currentUINum;

        // 양수면 생성, 음수면 삭제, 0은 그대로
        if (result > 0)
        {
            for (int i = 0; i < result; i++)
            {
                Instantiate(questInfoPrefab, listFoldContent[listValue].transform);
            }
        }
        else if (result < 0)
        {
            for (int i = 0; i < Mathf.Abs(result); i++)
            {
                Destroy(listFoldContent[listValue].transform.GetChild(i).gameObject);
            }

        }
        else
        {
              
        }
    }

    // UI 내용을 갱신 시켜 준다.
    public void Refresh()
    {
        // 받을 수 있는 퀘스트
        if (listFoldContent[0].activeSelf)
        {
            // 텍스트 컨포넌트를 모두 받아온다.
            TMP_Text[] textlist = listFoldContent[0].GetComponentsInChildren<TMP_Text>();

            // 텍스트에 퀘스트 제목을 입력해준다
            for (int i = 0; i < textlist.Length;i++)
            {
                textlist[i].text = QuestManager.questManager.QuestcanReceive[i].questName;
                textlist[i].gameObject.name = QuestManager.questManager.QuestcanReceive[i].questId.ToString();
            }
        }

        // 진행중인 퀘스트
        if (listFoldContent[1].activeSelf)
        {
            // 텍스트 컨포넌트를 모두 받아온다.
            TMP_Text[] textlist = listFoldContent[1].GetComponentsInChildren<TMP_Text>();

            // 텍스트에 퀘스트 제목을 입력해준다
            for (int i = 0; i < textlist.Length; i++)
            {
                textlist[i].text = QuestManager.questManager.questProgress[i].questName;
                textlist[i].gameObject.name = QuestManager.questManager.questProgress[i].questId.ToString();
            }

        }

        // 완료한 퀘스트
        if (listFoldContent[2].activeSelf)
        {
            // 텍스트 컨포넌트를 모두 받아온다.
            TMP_Text[] textlist = listFoldContent[2].GetComponentsInChildren<TMP_Text>();

            // 텍스트에 퀘스트 제목을 입력해준다
            for (int i = 0; i < textlist.Length; i++)
            {
                textlist[i].text = QuestManager.questManager.questComplete[i].questName;
                textlist[i].gameObject.name = QuestManager.questManager.questComplete[i].questId.ToString();
            }
        }
    }


    public TMP_Text questname;
    public TMP_Text questinfo;

    // 클릭하면 퀘스트 정보를 출력해줌
    public void QuestClick(GameObject go)
    {

        int questId = int.Parse(go.transform.GetChild(0).name);

        questname.text = QuestManager.questManager.questList[questId].questName;
        questinfo.text = QuestManager.questManager.questList[questId].questDescription;

    }

    private void OnEnable()
    {
        QuestUiPopup();
    }

    public void QuestUiPopup()
    {

        if (QuestManager.questManager.questProgress.Count > 0)
        {
            questname.text = QuestManager.questManager.questProgress[0].questName;
            questinfo.text = QuestManager.questManager.questProgress[0].questDescription;
        }
        else if (QuestManager.questManager.QuestcanReceive.Count > 0)
        {
            questname.text = QuestManager.questManager.QuestcanReceive[0].questName;
            questinfo.text = QuestManager.questManager.QuestcanReceive[0].questDescription;
        }
        else if (QuestManager.questManager.questComplete.Count > 0)
        {
            questname.text = QuestManager.questManager.questComplete[0].questName;
            questinfo.text = QuestManager.questManager.questComplete[0].questDescription;
        }
        else
        {
            questname.text = "";
            questinfo.text = "";
        }
    }



}
