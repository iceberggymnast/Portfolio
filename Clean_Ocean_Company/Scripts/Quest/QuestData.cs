using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestData
{
    public int questID = 0;
    public string questName = "아기 상괭이 친구들 구출하기";
    public string questDescription = 
        "석유시추선이 침몰하여 석유 슬러지 지대가 생겨버렸다. 상괭이가 석유 슬러지 지대에서 머물면 위험하니 데려와서 위험 물질을 제거 시켜야한다.";
    public string questShortDescription = "상괭이를 옮기고 씻기자";
    public int questCount = 0;
    public int questTargetCount = 200;
    public bool is_group_quest = true;
    public bool countVisble = true;
    [SerializeField]
    public QuestState questState = QuestState.Acceptable;
    public int rewardPoint = 10;
    public bool usable;
}

[Serializable]
public enum QuestState
{
    Not_Acceptable = 0, // 퀘스트 받기 불가능 상태
    Acceptable = 1, // 퀘스트를 받을 수 있는 상태
    Accepting = 2, // 퀘스트를 받고 진행중인 상태
    CanCompleted = 3, // 퀘스트를 받았는데 완료 가능한 상태 (목표 달성)
    Completed = 4 // 퀘스트 완료 된 상태
}

