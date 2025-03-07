using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemData : MonoBehaviour 
{
    [Serializable]
    public class ItemInfo
    {
        public int itemId; // 아이템의 id
        public string itemName; // 아이템의 이름 
        public string itemDescription; // 아이템 설명
        public Sprite itemSprite; // 아이템의 아이콘 그림
        public int itemHaveNum; // 아이템을 가지고 있는 갯수

        public bool firstGet; // 처음 먹었는지 체크
        public string itemDescriptionLong; // 아이템 긴 설명 버전


        // 아이템 효과
        public virtual void ItemEffect() 
        {
            print(itemName + "을 사용했다!");
            if(itemId == 5)
            {
                if (QuestManager.questManager.questList[7].questState == QuestData.QuestState.progress)
                {
                    itemHaveNum--;
                    QuestManager.questManager.QuestAddProgress(7, 0, 1);
                    NpcClick npcClick = GameObject.Find("NPC_tree").GetComponent<NpcClick>();
                    npcClick.StartConversation();
                }
            }
            //itemHaveNum--;
        }

        public void ItemDataAdd(int id, string name, string description, string spritename, string longDescription)
        {

            itemId = id;
            itemName = name;
            description = description.Replace("@", ",");
            description = description.Replace("<br/>", "<br>");
            itemDescription = description;
            longDescription = longDescription.Replace("@", ",");
            longDescription = longDescription.Replace("<br/>", "<br>");
            itemDescriptionLong = longDescription;

            // 스프라이트 불러오기
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/itemSprites");
            foreach (Sprite sprite in sprites)
                {
                    if (sprite.name == spritename)
                    {
                        itemSprite = sprite;
                        break;
                    }
            }        
        }

    }
}
