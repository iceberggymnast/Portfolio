using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class StoryObserver : MonoBehaviour
{
    // 퀘스트 상황을 보고 상태를 반영해주게 만든다. 

    public float quest4time;
    float quest5time;
    float quest6time;
    bool bo1;
    bool bo2;
    bool bo3;

    // 어머니 조작 관련
    public NavMeshAgent momAgent;
    public Transform momGonePos;

    // 호랑이 조작 관련
    public GameObject tigerModel;
    public NavMeshAgent tigerAgent;
    public DoorOpen door;

    public List<GameObject> movePoint;

    private void Start()
    {
        Questtigger tri = GameObject.Find("6_quest_trigger").GetComponent<Questtigger>();
        movePoint = tri.movePoint;
    }

    void Update()
    {
        // 아이템 줍는 퀘 다 깨면 아이템 삭제 시켜줌
        if (QuestManager.questManager.questList[2].questState == QuestData.QuestState.completed)
        {
            if(GameObject.Find("Items").transform.childCount > 0)
            {
                Destroy(GameObject.Find("Items").transform.GetChild(0).gameObject);
            }
        }

        // 안마 게임 끝나고 나서
        if (QuestManager.questManager.questList[4].questState == QuestData.QuestState.progress)
        {
            momAgent.SetDestination(momGonePos.position);

            /*
            CharacterController cc = QuestManager.questManager.myPlayer.GetComponent<CharacterController>();
            if (cc != null && !bo1)
            {
                cc.enabled = false;
                // 플레이어를 이동 시킨다.
                QuestManager.questManager.myPlayer.transform.position = new Vector3(0.565f, 0.377f, -5.696f);
                cc.enabled = true;
                bo1 = true;

            }
            quest4time += Time.deltaTime;*/
                NpcClick npcClick = GameObject.Find("NPC2").GetComponent<NpcClick>();
            if (quest4time > 10)
            {
                door.LockDoor(true);
                // 시간이 지나면 해당 퀘스트의 진행도 올림
                QuestManager.questManager.QuestAddProgress(4, 0, 1);
                PhotonView pvnpc = npcClick.GetComponent<PhotonView>();
                // 시간이 지나면 대화가 걸린다
                if (PhotonNetwork.IsMasterClient)
                {
                    pvnpc.RPC(nameof(npcClick.StartConversation), RpcTarget.All);
                }
                
                //npcClick.StartConversation();
            }
        }

            // 호랑이 미니게임 끝내고 대화
            if (QuestManager.questManager.questList[5].questState == QuestData.QuestState.canBeCompleted)
        {
            /*
            CharacterController cc = QuestManager.questManager.myPlayer.GetComponent<CharacterController>();

            if (cc != null && bo2)
            {
                cc.enabled = false;
                QuestManager.questManager.myPlayer.transform.position = new Vector3(0.565f, 0.377f, -5.696f);
                cc.enabled = true;
                bo2 = true;
            } */
            door.LockDoor(true);
            NpcClick npcClick = GameObject.Find("NPC2").GetComponent<NpcClick>();
            quest5time += Time.deltaTime;
            if (quest5time > 5)
            {
                PhotonView pvnpc = npcClick.GetComponent<PhotonView>();
                if (PhotonNetwork.IsMasterClient)
                {
                    pvnpc.RPC(nameof(npcClick.StartConversation), RpcTarget.All);
                }
                //npcClick.StartConversation();
            }

            tigerModel.SetActive(true);

        }

            // 도망치기 퀘스트 활성화
            if (QuestManager.questManager.questList[6].questState == QuestData.QuestState.progress)
        {
            tigerAgent.SetDestination(QuestManager.questManager.myPlayer.transform.position);
        }

        // 떡던지기 클리어 되면
        if (QuestManager.questManager.questList[8].questState == QuestData.QuestState.canBeCompleted)
        {
            NpcClick npcClick = GameObject.Find("NPC_tree").GetComponent<NpcClick>();
            CharacterController characterController = QuestManager.questManager.myPlayer.GetComponent<CharacterController>();
            characterController.enabled = false;

            if (PhotonNetwork.NickName == "해님")
            {
                QuestManager.questManager.myPlayer.transform.position = movePoint[0].transform.position;
            }
            else
            {
                QuestManager.questManager.myPlayer.transform.position = movePoint[1].transform.position;
            }

            characterController.enabled = true;
            quest6time += Time.deltaTime;

            if (quest6time > 5)
            {
                PhotonView pvnpc = npcClick.GetComponent<PhotonView>();
                if (PhotonNetwork.IsMasterClient)
                {
                    pvnpc.RPC(nameof(npcClick.StartConversation), RpcTarget.All);
                }
                //npcClick.StartConversation();
            }
        }




    }
}
