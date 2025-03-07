using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class QuestData 
{
    public QuestInfo questInfo = new QuestInfo();

    public enum QuestState
    {
        cantReceived, // 퀘스트를 받을 수 없음
        canReceived, // 퀘스트를 받을 수 있음
        progress, // 퀘스트를 진행중
        canBeCompleted, // 퀘스트를 클리어 할 수 있음
        completed // 퀘스트를 클리어 함
    }

    [Serializable]
    public class QuestInfo
    {
        public int questId; // 해당 퀘스트의 ID
        public string questName; // 퀘스트의 제목
        public string questDescription; // 퀘스트의 설명 또는 목표

        public int npcId; // 퀘스트를 받을 수 있는 NPC의 ID
        public int requireId; // 퀘스트를 받기위해 필요한 퀘스트 ID

        public QuestState questState = QuestState.cantReceived; // 퀘스트의 현재 상태
        public List<QuestObjectives> objectives = new List<QuestObjectives>();
        public List<QuestReward> reward = new List<QuestReward>();

        public string shortDescription;


        [Serializable]
        public class QuestObjectives
        {
            public bool itemRequire;
            public int itemId;
            public int currentobjectivesNumber;
            public int requireNumber;
        }

        [Serializable]
        public class QuestReward
        {
            public int itemId;
            public int rewardNumber;
        }


        public void QuestAdd(int id, string name, string description, int npc, int requre, string isNeedItem, string itemid, string needItemnum, string rewardid, string ewardnumber, string shortDes)
        {
            questId = id;
            name = name.Replace("@", ",");
            questName = name;
            description = description.Replace("@", ",");
            description = description.Replace("<br/>", "<br>");
            questDescription = description;
            npcId = npc;
            requireId = requre;
            shortDescription = shortDes;

            // 목표 관련 처리
            string[] needitems = isNeedItem.Split('@');
            string[] itemids = itemid.Split('@');
            string[] needItemnums = needItemnum.Split('@');

            for(int i = 0; i < needitems.Length; i++)
            {
                 objectives.Add(new QuestObjectives());
                 objectives[i].itemRequire = bool.Parse(needitems[i]);
                 objectives[i].itemId = int.Parse(itemids[i]);
                 objectives[i].requireNumber = int.Parse(needItemnums[i]);
            }

            // 보상 관련 처리

            string[] rewardids = rewardid.Split('@');
            string[] rewardnumbers = ewardnumber.Split('@');

            for (int i = 0; i < rewardids.Length; i++)
            {
                reward.Add(new QuestReward());
                reward[i].itemId = int.Parse(rewardids[i]);
                reward[i].rewardNumber = int.Parse(rewardnumbers[i]);
            }
        }

    }

}
