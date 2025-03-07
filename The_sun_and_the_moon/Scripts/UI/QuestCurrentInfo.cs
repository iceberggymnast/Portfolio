using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestCurrentInfo : MonoBehaviour
{
    // 백그라운드 배경 사이즈
    public RectTransform backgroundTr;
    // 퀘스트 이름 text
    public TMP_Text questName;
    public RectTransform questnameTr;
    // 목표 아이템 이름
    public TMP_Text tagetName;
    public RectTransform tagetNameTr;
    // 목표 필요 갯수
    public TMP_Text tagetnum;
    public RectTransform tagetnumTr;

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 9)
        {
            gameObject.SetActive(false);
        }

        // 진행중인 퀘스트가 있는지 확인한다
        if (QuestManager.questManager.questProgress.Count > 0)
        {
            // 진행중인 퀘스트의 첫번째 정보를 기준으로 한다
            // 퀘스트 이름을 설정한다
            questName.text = QuestManager.questManager.questProgress[0].questName;

            string tagetnamelist = "";
            string tagetnumberlist = "";
            for(int i = 0; i < QuestManager.questManager.questProgress[0].objectives.Count; i++)
            {
                // 필요 아이템 이름을 넣어주고 줄바꿈을 넣는다
                if (QuestManager.questManager.questProgress[0].objectives[i].itemRequire)
                {
                    int itemid = QuestManager.questManager.questProgress[0].objectives[i].itemId;
                    string itemname = QuestManager.questManager.itemManager.itemList[itemid].itemName;
                    tagetnamelist += itemname + "<br>";
                }
                else
                {
                    // 아이템이 필요하지 않은 퀘스트라면 짧은 설명을 넣는다
                    tagetnamelist += QuestManager.questManager.questProgress[0].shortDescription ;
                    tagetnamelist += "<br>";
                }
                // 필요 아이템 갯수와 보유수량을 적어준다.
                tagetnumberlist += QuestManager.questManager.questProgress[0].objectives[i].currentobjectivesNumber + " ";
                tagetnumberlist += "/";
                tagetnumberlist += QuestManager.questManager.questProgress[0].objectives[i].requireNumber + " ";
                tagetnumberlist += "<br>";
            }
            tagetName.text = tagetnamelist;
            tagetnum.text = tagetnumberlist;

        }
        else
        {
            questName.text = "진행중인 퀘스트가 없습니다.";
            tagetName.text = "";
            tagetnum.text = "";
        }
        // 크기를 재설정 해준다
        //questnameTr.sizeDelta = new Vector2(questnameTr.sizeDelta.x, questName.preferredHeight);
        tagetNameTr.sizeDelta = new Vector2(tagetNameTr.sizeDelta.x, tagetName.preferredHeight);
        tagetnumTr.sizeDelta = new Vector2(tagetnumTr.sizeDelta.x, tagetnum.preferredHeight);

        float ysize = questnameTr.sizeDelta.y + tagetNameTr.sizeDelta.y;
        if (tagetName.text.Length == 0) ysize -= tagetNameTr.sizeDelta.y;
        backgroundTr.sizeDelta = new Vector2(backgroundTr.sizeDelta.x, ysize);
    }
}
