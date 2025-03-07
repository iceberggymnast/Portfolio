using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTest : MonoBehaviour
{
    public QuestManager questManager;
    public ItemManager itemManager;

    public int itemid;
    public int itemnum;

    public int questnum;

    public GameObject grand;
    public NpcData granddat;
    public DialogSystem dialogSystem;


    public void ItemAdd()
    {
        QuestManager.questManager.itemManager.ItemAdd(itemid, itemnum);
    }

    public void Questadd()
    {
        QuestManager.questManager.QuestAccept(questnum);
    }

    public void Questclear()
    {
        QuestManager.questManager.QuestClear(questnum);
    }

    public void Npctalk()
    {
        QuestManager.questManager.dialogSystem.StartConversation(grand, granddat);

    }

}
