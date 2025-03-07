using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneMgr : MonoBehaviourPun
{
    public List<Transform> spawnPoint;

    void Awake()
    {

        //플레이어를 생성(현재 Room에 접속 되어있는 친구들도 보이게)


        if (QuestManager.questManager.questList.Count <= 0)
        {
            print("player 생성 : 기본 위치");
            //PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
            if (PhotonNetwork.NickName == "해님")
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[0].position , spawnPoint[0].rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[1].position , spawnPoint[1].rotation);
            }
        }
        else if (QuestManager.questManager.questList[5].questState == QuestData.QuestState.canBeCompleted)
        {
            print("player 생성 : 집 안");
            //PhotonNetwork.Instantiate("Player", new Vector3(0.565f, 0.377f, -5.696f), Quaternion.identity);
            if (PhotonNetwork.NickName == "해님")
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[2].position, spawnPoint[2].rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[3].position, spawnPoint[3].rotation);
            }
        }
        else if (QuestManager.questManager.questList[8].questState == QuestData.QuestState.canBeCompleted)
        {
            print("player 생성 : 나무 위");
            //PhotonNetwork.Instantiate("Player", new Vector3(-4.168099f, 8.81101f, -20.85015f), Quaternion.identity);
            if (PhotonNetwork.NickName == "해님")
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[4].position, spawnPoint[4].rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[5].position, spawnPoint[5].rotation);
            }
        }
        else if (QuestManager.questManager.questList[3].questState == QuestData.QuestState.canBeCompleted || QuestManager.questManager.questList[3].questState == QuestData.QuestState.progress)
        {
            print("player 생성 : 어머니 앞");
            //PhotonNetwork.Instantiate("Player", new Vector3(-4.168099f, 8.81101f, -20.85015f), Quaternion.identity);
            if (PhotonNetwork.NickName == "해님")
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[6].position, spawnPoint[6].rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[7].position, spawnPoint[7].rotation);
            }
        }
        else
        {
            print("player 생성 : 기본 위치");
            //PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
            if (PhotonNetwork.NickName == "해님")
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[0].position, spawnPoint[0].rotation);
            }
            else
            {
                PhotonNetwork.Instantiate("Player", spawnPoint[1].position, spawnPoint[1].rotation);
            }
        }


    }

    void Update()
    {
        
    }
}
