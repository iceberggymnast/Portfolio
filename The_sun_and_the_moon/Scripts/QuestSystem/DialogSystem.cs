using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogSystem : MonoBehaviourPun
{
    // 대화 중인 현재 Npc
    [SerializeField]
    GameObject tagetNpc;

    // 대화중인 현재 npc의 데이터
    [SerializeField]
    NpcData npcData;

    // 대화 관련 게임 오브젝트들(UI)
    public GameObject dialogUI;

    // 현재 참조중인 대화 데이터
    DialogueData currentDialogue;

    public DialogueData readCurrentDialogue { get { return currentDialogue; } }

    // 대화 창 팝업 여부 참조
    public bool isConversation = false;

    // 대화 텍스트가 출력 중인 코루틴이 작동중인지 확인
    bool isDialog = false;
    public PlayerState playerState;

    // 멀티 관련 구현
    int actorid;

    // 상대방 대화 체크
    public bool sunisDialog;
    public bool moonisDialog;

    private void Start()
    {
        /*
        // 나의 액터 아이디를 가져온다.
        actorid = PhotonNetwork.LocalPlayer.ActorNumber;

        // 씬에 있는 플레이어 태그가 달린 오브젝트들을 가져온다.
        List<GameObject> playerList = GameObject.FindGameObjectsWithTag("Player").ToList();

        // 해당 오브젝트의 포톤뷰의 액터 id가 내꺼랑 일치하는지 확인한다.
        for (int i = 0; i < playerList.Count; i++)
        {
            // 플레이어에 있는 포톤뷰 컴포넌트를 가져온다
            PhotonView playerphoton = playerList[i].transform.GetComponent<PhotonView>();

            // 해당 컴포넌트의 actor 아이디가 로컬 id와 일치하는지 확인한다.
            if ( playerphoton.Owner.ActorNumber  == actorid)
            {
                // 내 플레이어가 맞으면 할당해준다.
                GameObject myPlayer = playerList[i];
                playerState = myPlayer.GetComponent<PlayerState>();
                break;
            }

        }
        */

        dialogUI = GameObject.FindWithTag("Interface").transform.GetChild(0).gameObject;
        //playerState = FindObjectOfType<PlayerState>();
    }

    bool playerStateChack;
    void Update()
    {
        if (playerState == null && !playerStateChack)
        {
            if (QuestManager.questManager.myPlayer != null)
            {
                playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();
                playerStateChack = true;
            }
        }
    }

    [PunRPC]
    // 대화 시작 호출 함수 (호출해서 쓰세요)
    public void StartConversation(GameObject npc, NpcData data)
    {
        
        // 대화를 하려는 NPC에 대한 변수를 참조하게 가져옴
        tagetNpc = npc;
        npcData = data;

        // 마우스가 화면에 뜨게 함
        if (playerState != null)
        {
            playerState.isNPCtalking = true;
            playerState.isOpenUI = true;
        }
        else
        {
            playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();
            if (playerState != null)
            {
                playerState.isNPCtalking = true;
                playerState.isOpenUI = true;
            }
        }

        // 대화창 캔버스를 셋액티브 함
        dialogUI.SetActive(true);

        // 현재 dialogue_group_id를 읽어서 해당 위치의 대화를 시작
        currentDialogue = FindConversation(data.dialogue_group_id, 0);
        TypeCheck();

        // 대화가 시작 되었음을 설정
        isConversation = true;
    }

    // 대화 진행 호출 함수
    public void OnGoingConversation()
    {
        // UI 내의 현재 발화자에 텍스트를 적용함
        dialogUI.transform.GetChild(1).GetComponent<TMP_Text>().text = currentDialogue.npc_name;
        // 텍스트를 띄워줌
        StartCoroutine(TextPrint(currentDialogue.text));

    }

    // 대화 텍스트 출력 효과
    IEnumerator TextPrint(string text)
    {
        TMP_Text contentOfConversation = dialogUI.transform.GetChild(2).GetComponent<TMP_Text>();

        isDialog = true;
        //PlayerisDialog(0, true);
        //PlayerisDialog(1, true);

        
        if (PhotonNetwork.NickName == "해님")
        {
            photonView.RPC(nameof(PlayerisDialog), RpcTarget.All, 0, true);
        }
        else
        {
            photonView.RPC(nameof(PlayerisDialog), RpcTarget.All, 1, true);
        } 

        // 텍스트를 타자치듯이 출력시킨다.
        for (int i = 0; i < text.Length; i++)
        {
            contentOfConversation.text = text.Substring(0, i + 1);
            yield return new WaitForSeconds(0.005f);
        }

        isDialog = false;

        if (PhotonNetwork.NickName == "해님")
        {
            photonView.RPC(nameof(PlayerisDialog), RpcTarget.All, 0, false);
        }
        else
        {
            photonView.RPC(nameof(PlayerisDialog), RpcTarget.All, 1, false);
        }

    }

    [PunRPC]
    void PlayerisDialog(int sunormoon, bool value)
    {
        if (sunormoon == 0)
        {
            sunisDialog = value;
        }
        else
        {
            moonisDialog = value;
        }

        print (sunisDialog.ToString () + ", " + moonisDialog.ToString());
        
    }

    [PunRPC]
    // 대화 진행 함수
    public void NextConversation()
    {
        // 코루틴이 안 끝났으면 진행이 되지 않는다.
        if (!sunisDialog && !moonisDialog && !isDialog)
        {
            // 다음 대화를 받아온다.
            currentDialogue = FindConversation(npcData.dialogue_group_id, currentDialogue.next_sequence_id);
            TypeCheck();
        }
    }



    void TypeCheck()
    {
        // 타입별로 다른 함수를 실행해줌
        if (currentDialogue.type == "text")
        {
            OnGoingConversation();
        }
        else if (currentDialogue.type == "event")
        {
            TypeEvent();
        }
        
    }

    public void TypeEvent()
    {
        // 현재 text에 입력된 내용을 받아서 판단

        // Exit이 text에 입력되어 있으면 해당 내용 실행
        if (currentDialogue.text == "Exit")
        {
            // 마우스가 안뜨게 설정
            if (playerState != null)
            {
                playerState.isNPCtalking = false;
                playerState.isOpenUI = false;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            // 대화창 캔버스를 끔
            dialogUI.SetActive(false);

            // 대화가 끝났음을 설정
            isConversation = false;

            //event_code를 참고하여 1이면 dialogue_group_id를 1 더해줌
            if (currentDialogue.event_code == 1)
            {
                npcData.dialogue_group_id++;
                print("해당 npc는 다음 대화가 걸릴 예정입니다");
            }
        }
        // QuestAccept이 text에 입력되어 있으면 해당 내용 실행
        else if (currentDialogue.text == "QuestAccept")
        {
            // 이벤트 코드에 있는 퀘스트 index 퀘스트를 받아보려고 한다.
           QuestManager.questManager.QuestAccept(currentDialogue.event_code);
            // 다음 대화로 넘어간다.
            NextConversation();
        }
        else if (currentDialogue.text == "QuestClear")
        {
            // 퀘스트를 클리어 했으면
            if(QuestManager.questManager.QuestClear(currentDialogue.event_code))
            {
                npcData.dialogue_group_id++;
                print("퀘스트 완료 됨");
                NextConversation();
            }
            // 퀘스트가 클리어 조건이 되지 않거나 이미 클리어 했으면
            else
            {
                print("퀘스트 완료 되지 않음");
                NextConversation();
            }
        }
        else if (currentDialogue.text == "PlayGame")
        {
            int mapcode = currentDialogue.event_code;

            NextConversation();

            // 이벤트 코드에 있는 int를 매개변수로 씬을 전환
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(mapcode);
            }
        }
        else if (currentDialogue.text == "Animation")
        {
            // 대화중인 NPC의 애니메이터 컴포넌트를 가져온다
            Animator anim = tagetNpc.GetComponentInChildren<Animator>();

            if (anim != null)
            {
                print(currentDialogue.event_code);
                anim.SetTrigger(currentDialogue.event_code.ToString());
            }
            else
            {
                print("애니메이션 컴포넌트가 없습니다");
            }

            NextConversation();
        }
        else
        {
            print("이벤트가 실행되지 않았습니다");
            NextConversation();
        }
    }

    // 다음 대화를 위해 찾아줌
    public DialogueData FindConversation(int group_id, int next_sequence_id)
    {
        DialogueData findDialogue;
        for (int i = 0; i < npcData.dialogueDatas.Count; i++)
        {
            if (npcData.dialogueDatas[i].dialogue_group_id == group_id)
            {
                if (npcData.dialogueDatas[i].sequence_id == next_sequence_id)
                {
                    findDialogue = npcData.dialogueDatas[i];
                    return findDialogue;
                }
            }
        }
        print("대화 데이터를 찾을 수 없습니다.");
        return null;
    }
    
    // 강제 대화 종료 (TEST용)
    public void button()
    {
        // 마우스가 안뜨게 설정
        if (playerState != null)
        {
            playerState.isNPCtalking = false;
            playerState.isOpenUI = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // 대화창 캔버스를 끔
        dialogUI.SetActive(false);

        // 대화가 끝났음을 설정
        isConversation = false;

        //event_code를 참고하여 1이면 dialogue_group_id를 1 더해줌
        if (currentDialogue.event_code == 1)
        {
            npcData.dialogue_group_id++;
            print("해당 npc는 다음 대화가 걸릴 예정입니다");
        }
    }
}
