using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public TMP_Text QuestText;

    public Image questImage;

    private void Awake()
    {
        QuestText = transform.GetComponentInChildren<TMP_Text>();

        if (questImage == null)
        {
            transform.GetChild(0).GetChild(2).GetComponent<Image>();
        }
    }

    public void QuestUISet()
    {
        if (QuestManger.instance != null && questImage != null)
        {
            for (int i = 0; QuestManger.instance.questDatas.Count > 0; i++)
            {
                if (QuestManger.instance.questDatas[i].questState != QuestState.Not_Acceptable)
                {
                    string progress = "  " + QuestManger.instance.questDatas[i].questCount.ToString() + " / " + QuestManger.instance.questDatas[i].questTargetCount.ToString();
                    QuestText.text = QuestManger.instance.questDatas[i].questShortDescription + progress;
                    QuestManger.instance.QuestSetMark(questImage, QuestManger.instance.questDatas[i].questState);
                    return;
                }
                if (QuestManger.instance.questDatas[i].questState != QuestState.CanCompleted)
                {
                    string progress = "  " + QuestManger.instance.questDatas[i].questCount.ToString() + " / " + QuestManger.instance.questDatas[i].questTargetCount.ToString();
                    QuestText.text = QuestManger.instance.questDatas[i].questShortDescription + progress;
                    QuestManger.instance.QuestSetMark(questImage, QuestManger.instance.questDatas[i].questState);
                    return;
                }
            }
            QuestText.text = " 진행 중인 미션이 없습니다.";
        }
        else
        {
            print("퀘스트 매니져 없음");
        }
    }

    
}
