using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Photon.Pun;

public class NpcClick : MonoBehaviour
{
    public GameObject npc;
    public int npcId;



    [PunRPC]
    public void StartConversation()
    {
        npc = QuestManager.questManager.npcList[npcId];
        NpcData npcdata = npc.GetComponent<NpcData>();

        npc = this.gameObject;

        QuestManager.questManager.myPlayer.GetComponent<PlayerState>().targetPos = transform.GetChild(0).gameObject.transform;
        QuestManager.questManager.dialogSystem.StartConversation(npc, npcdata);
    }

    public Transform ReturnTagetPos()
    {
        return transform.GetChild(0).gameObject.transform;
    }
}

