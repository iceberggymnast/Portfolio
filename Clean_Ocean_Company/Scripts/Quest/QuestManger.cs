using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class QuestManger : MonoBehaviour
{
    [SerializeField]
    public List<QuestData> questDatas = new List<QuestData>();

    public List<Sprite> sprites = new List<Sprite>();

    public static QuestManger instance;

    


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void  Start()
    {
        // 프리셋으로 만들어 놓은 퀘스트 하나를 추가한다.
        //questDatas.Add(new QuestData());

        // 수락가능 상태로 변경한다
        //QuestSetState(0, QuestState.Acceptable);
        if (UIController.instance != null)
        {
            StartCoroutine(UIController.instance.FadeOut("QuestBoard", 0.01f));
        }
    }

    public void QuestSetMark(Image markImage, QuestState questState)
    {
        if (questState == QuestState.Acceptable)
        {
            markImage.sprite = sprites[1];
        }
        else if (questState == QuestState.Accepting)
        {
            markImage.sprite = sprites[2];
        }
        else if (questState == QuestState.CanCompleted)
        {
            markImage.sprite = sprites[0];
        }
    }

    // 퀘스트의 상태 값을 조절하는 기능
    public void QuestSetState(int quest_ID, QuestState questState)
    {
        int questListIndex = QuestFind(quest_ID);
        QuestUI questUI = GameObject.FindObjectOfType<QuestUI>();

        if (quest_ID == -1)
        {
            print("해당 ID를 가진 퀘스트를 찾을 수 없습니다.");
            return;
        }

        // 해당 퀘스트 데이터를 들어온 매개변수 별로 다르게 처리한다.
        if (questState == QuestState.Not_Acceptable)
        {
            // 퀘스트를 받을 수 없게 설정하는 기능은 없다.
            print(" 해당 기능은 사용 할 수 없습니다. ");
        }

        else if (questState == QuestState.Acceptable)
        {
            // 퀘스트 수락이 가능하게 바꾸는 기능은 Not_Acceptable 일때만 가능하다.
            if (questDatas[questListIndex].questState == QuestState.Not_Acceptable)
            {
                questDatas[questListIndex].questState = QuestState.Acceptable;
                print(questDatas[questListIndex].questName + "은 수락 가능한 상태로 변경 되었습니다.");
            }
            else
            {
                print("해당 퀘스트는 수락 가능한 상태로 변경 불가능한 상태입니다. Code : " + questDatas[questListIndex].questState.ToString());
            }
        }

        else if (questState == QuestState.Accepting)
        {
            // 퀘스트 수락은 Acceptable일때만 가능하다.
            if (questDatas[questListIndex].questState == QuestState.Acceptable)
            {
                questDatas[questListIndex].questState = QuestState.Accepting;
                print(questDatas[questListIndex].questName + "은 수락되었습니다.");
                questUI.QuestUISet();
                StartCoroutine(UIController.instance.FadeIn("QuestBoard", 0.1f));
            }
            else
            {
                print("해당 퀘스트는 수락한 상태로 변경 불가능한 상태입니다. Code : " + questDatas[questListIndex].questState.ToString());
            }
        }

        else if (questState == QuestState.CanCompleted)
        {
            // 퀘스트 완료가능한 상태로 변경 가능한건 Acceptable이며 목표를 달성했을때만 가능합니다.
            if (questDatas[questListIndex].questState == QuestState.Accepting)
            {
                if (questDatas[questListIndex].questCount >= questDatas[questListIndex].questTargetCount)
                {
                    questDatas[questListIndex].questState = QuestState.CanCompleted;
                    print(questDatas[questListIndex].questName + "은 완료 가능한 상태입니다..");
                }
                else
                {
                    print("해당 퀘스트는 완료 가능 상태로 변경 불가능한 상태입니다. Code : " 
                        + questDatas[questListIndex].questCount.ToString() + " / " + questDatas[questListIndex].questTargetCount.ToString());
                }
            }
            else
            {
                print("해당 퀘스트는 완료 가능 상태로 변경 불가능한 상태입니다. Code : " + questDatas[questListIndex].questState.ToString());
            }
        }

        else if (questState == QuestState.Completed)
        {
            // 완료는 CanCompleted인 상태에서만 가능하다.
            if (questDatas[questListIndex].questState == QuestState.CanCompleted)
            {
                questDatas[questListIndex].questState = QuestState.Completed;
                print(questDatas[questListIndex].questName + "를 완료한 상태입니다..");
                // 보상 추가 함수 추가 예정
            }
            else
            {
                print("해당 퀘스트는 완료 상태로 변경 불가능한 상태입니다. Code : " + questDatas[questListIndex].questState.ToString());
            }
        }
    }


    // 퀘스트의 진행도를 상승시키는 기능
    public void Questproceed(int quest_ID, int add_Value)
    {
        int questListIndex = QuestFind(quest_ID);

        QuestUI questUI = GameObject.FindObjectOfType<QuestUI>();

        if (questDatas[questListIndex].is_group_quest)
        {
            // 그룹 퀘스트는 수락 여부 상관 없이 진행도가 오름
            questDatas[questListIndex].questCount += add_Value;
        }
        else if (questDatas[questListIndex].questState == QuestState.Accepting)
        {
            // 그룹 퀘스트가 아닌데 수락중이면 진행도가 오름
            questDatas[questListIndex].questCount += add_Value;
        }
        else
        {
            print("해당 퀘스트는 진행도를 올릴 수 없는 상태입니다. Code : " + questDatas[questListIndex].questState.ToString());
        }

        // 진행도 적용 후 퀘스트 완료 가능 여부를 판단
        if (questDatas[questListIndex].questCount >= questDatas[questListIndex].questTargetCount)
        {
            // 가능한 사항이면 완료로 바꿔줌
            QuestSetState(questListIndex, QuestState.CanCompleted);
        }
        else
        {
            // 불가능하면 현재 상태를 출력함
            print("해당 퀘스트 진행도 업데이트 :  " + questDatas[questListIndex].questCount.ToString() + " / " + questDatas[questListIndex].questTargetCount.ToString());
            questUI.QuestUISet();
        }
    }

    // 퀘스트 id를 받아 퀘스트를 찾음
    public int QuestFind(int quest_ID)
    {
        // 해당 퀘스트 id를 가진 데이터를 찾는다.
        for (int i = 0; i < questDatas.Count; i++)
        {
            // 해당 데이터가 맞을 경우 i의 값을 저장한다.
            if (questDatas[i].questID == quest_ID)
            {
                return i;
            }
        }
        return -1;
    }

}
