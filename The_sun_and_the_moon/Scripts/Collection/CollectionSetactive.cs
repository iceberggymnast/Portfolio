using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectionSetactive : MonoBehaviourPun
{
    GameObject GoodSonHand;
    GameObject RabbitDoll;
    GameObject TigerHand;
    GameObject Book;


    void Start()
    {
        GoodSonHand = GameObject.Find("Cooooooooooollections").transform.GetChild(0).gameObject;
        TigerHand = GameObject.Find("Cooooooooooollections").transform.GetChild(1).gameObject;
        RabbitDoll = GameObject.Find("Cooooooooooollections").transform.GetChild(2).gameObject;
        Book = GameObject.Find("Cooooooooooollections").transform.GetChild(3).gameObject;
    }

    void Update()
    {
        photonView.RPC(nameof(SetActvieCollection), RpcTarget.All);
    }

    [PunRPC]
    public void SetActvieCollection()
    {
        //효자손은 안마게임 퀘스트 완료 후 등장
        if (QuestManager.questManager.questList[3].questState == QuestData.QuestState.canBeCompleted)
        {
            if (GoodSonHand != null)
            {
                GoodSonHand.SetActive(true);
            }
        }
        //호랑이 손은 호랑이 손 때리기 퀘스트 완료 후 등장
        else if (QuestManager.questManager.questList[5].questState == QuestData.QuestState.canBeCompleted)
        {
            if (TigerHand != null)
            {
                TigerHand.SetActive(true);
            }
        }
        //남매 토끼 인형은 호랑이에게 떡 던지기 퀘스트 완료 후 등장
        else if (QuestManager.questManager.questList[8].questState == QuestData.QuestState.canBeCompleted)
        {
            if (RabbitDoll != null)
            {
                RabbitDoll.SetActive(true);
            }
        }
        ////해님달님 동화책은 밧줄타기 퀘스트 완료 후 등장
        //else if (QuestManager.questManager.questList[9].questState == QuestData.QuestState.canBeCompleted)
        //{
        //    if (Book != null)
        //    {
        //        Book.SetActive(true);
        //    }
        //}
    }
}
