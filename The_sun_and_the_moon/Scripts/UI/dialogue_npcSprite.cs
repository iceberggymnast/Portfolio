using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogue_npcSprite : MonoBehaviour
{
    // NPC 스프라이트 모음
    public List<Sprite> npcSprites;
    public List<Sprite> playerSprites;

    // Sprite id
    int npcId;

    // 스프라이트를 적용할 이미지 컴포넌트
    public Image imgNpc;


    void Update()
    {
        if (QuestManager.questManager != null)
        {
            npcId = QuestManager.questManager.dialogSystem.readCurrentDialogue.npc_id;

            if (npcId == 0) return;
            if (npcId > 0)
            {
                imgNpc.sprite = npcSprites[npcId];
            }
            else
            {
                npcId = Mathf.Abs(npcId);
                imgNpc.sprite = playerSprites[npcId];
            }
        }
    }
}
