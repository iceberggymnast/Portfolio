using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_QuestWait : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    public Mark mark = new Mark();
    public Image img;

    public enum Mark
    {
        blank,
        question,
        process,
        done
    }

    void Start()
    {
        
    }

    void Update()
    {
        img.transform.LookAt(QuestManager.questManager.myPlayer.transform.position);

        switch (mark)
        {
            case Mark.blank:
                img.sprite = sprites[0];
                break;
            case Mark.question:
                img.sprite = sprites[1];
                break;
            case Mark.process:
                img.sprite = sprites[2];
                break;
            case Mark.done:
                img.sprite = sprites[3];
                break;
        }

        if (QuestManager.questManager.questList[1].questState == QuestData.QuestState.canBeCompleted ||
            QuestManager.questManager.questList[2].questState == QuestData.QuestState.canBeCompleted)
        {
            mark = Mark.done;
        }
        else if (QuestManager.questManager.questList[2].questState == QuestData.QuestState.canReceived ||
           QuestManager.questManager.questList[3].questState == QuestData.QuestState.canReceived)
        {
            mark = Mark.question;
        }
        else if (QuestManager.questManager.questList[2].questState == QuestData.QuestState.progress)
        {
            mark = Mark.process;
        }
        else
        {
            mark = Mark.blank;
        }
    }
}
