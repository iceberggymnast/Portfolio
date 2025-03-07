using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestUIBotton : MonoBehaviour
{
    // 퀘스트 목록 버튼이 생성될 때 버튼에 클릭 이벤트를 할당 해 줍니다.
    QuestUI questUI;
    void Start()
    {
        questUI = GameObject.Find("Canvas_QuestUI").GetComponent<QuestUI>();
        transform.GetComponent<Button>().onClick.AddListener(() => questUI.QuestClick(transform.gameObject));
    }
}
