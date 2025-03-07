using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomDispear : MonoBehaviour
{
    // 4번 퀘스트 완료하면 어머니 NPC 오브젝트 셋 액티브 꺼주기 
    void Start()
    {
        if (QuestManager.questManager.questList[4].questState == QuestData.QuestState.completed)
        {
            gameObject.SetActive(false);
        }
    }
}
