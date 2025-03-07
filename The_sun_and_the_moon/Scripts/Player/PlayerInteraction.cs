//using System.Collections;
//using System.Collections.Generic;
using Photon.Pun;
using System.Collections;
using UnityEngine;
//using UnityEngine.Rendering;

public class PlayerInteraction : MonoBehaviour
{
    DialogSystem dialogSystem;
    //PlayerMove playerMove;
    public PlayerState playerState;

    ItemManager itemManager;

    PhotonView photonView0;

    public AudioSource itemBGM;  // 인스펙터에서 연결
    public AudioSource collectionBGM;  // 인스펙터에서 연결

    void Start()
    {
        //dialogSystem = GetComponent<DialogSystem>();
        dialogSystem = FindObjectOfType<DialogSystem>();
        //playerMove = GetComponent<PlayerMove>();
        playerState = GetComponent<PlayerState>();
        itemManager = FindObjectOfType<ItemManager>();
        photonView0 = GetComponent<PhotonView>();


        //outline = FindObjectOfType<Outline>();
    }

    void Update()
    {
        if (photonView0.IsMine)
        {
            GetCollection_p();
        }
    }

    //플레이어가 왼클릭했을 때 클릭한 수집품은 사라지고 수집품 박스에 해당 수집품의 이름이 활성화됨
    public void GetCollection_p()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            //sphere의 반지름은 0.5f로 설정해둬서 0.5m거리 밖의 것들만 raycast가 인지가 됨! 나중에 더 멀리 또는 가깝게 하고싶으면 radius값을 조정하기!
            //레이에 맞은 layer가 수집품(8번)일 때
            if (!dialogSystem.isConversation)
            {
                if (Physics.SphereCast(Camera.main.transform.position, 0.5f, Camera.main.transform.forward, out hit, 5.0f, (1 << 8)))
                {
                    //수집품 클릭했는지 확인하고 수집품 정보 팝업 창 띄우기
                    print("콜렉션 확인");
                    Cooooooooooooollection cooooooooooooollection = hit.transform.GetComponent<Cooooooooooooollection>();
                    CooooooooooooollectionManager cooooooooooooollectionManager = FindObjectOfType<CooooooooooooollectionManager>();
                    PhotonView photonView = cooooooooooooollectionManager.GetComponent<PhotonView>();
                    print("포톤 뷰 가지고 옴");
                    photonView.RPC(nameof(cooooooooooooollectionManager.OnCollectionClick), RpcTarget.All, cooooooooooooollection.CollectionID);
                    print("rpc함수는 됨");
                    //get한 수집품 정보를 수집품 박스에 반영해주기
                    photonView.RPC(nameof(cooooooooooooollectionManager.OnCollectionClick_BoxUploading), RpcTarget.All, cooooooooooooollection.CollectionID);
                    print("get한 수집품 정보를 수집품 박스에 반영해줨");
                    //클릭한 수집품은 사라지게 하기
                    //photonView.RPC(nameof(DestroyGetCollection), RpcTarget.All, hit.collider.name);
                    //Destroy(hit.transform.gameObject);
                    //print("사라진 거 확인함");

                    collectionBGM.Play();  // BGM 재생
                }
            }
            //레이에 맞은 layer가 Item(9번)일 때
            if (!dialogSystem.isConversation)
            {
                if (Physics.SphereCast(Camera.main.transform.position, 0.5f, Camera.main.transform.forward, out hit, 5.0f, (1 << 9)))
                {
                    DropItem dropItem = hit.transform.GetComponent<DropItem>();
                    ItemManager itemManager2 = QuestManager.questManager.itemManager;
                    PhotonView photonView = itemManager2.GetComponent<PhotonView>();
                    photonView.RPC(nameof(itemManager2.ItemAdd), RpcTarget.All, dropItem.itemID, dropItem.addeditemCNT);
                    //itemManager.ItemAdd(dropItem.itemID, dropItem.addeditemCNT);

                    Destroy(hit.transform.gameObject);

                    itemBGM.Play();  // BGM 재생
                }
            }

            //레이에 맞은 layer가 NPC(10번)일 때
            if (!dialogSystem.isConversation)
            {
                if (Physics.SphereCast(Camera.main.transform.position, 0.5f, Camera.main.transform.forward, out hit, 10.0f, (1 << 10)))
                {
                    print("NPC 확인");
                    NpcClick npcClick = hit.collider.gameObject.GetComponent<NpcClick>();
                    PhotonView photonView = npcClick.GetComponent<PhotonView>();
                    photonView.RPC(nameof(npcClick.StartConversation), RpcTarget.All);
                    playerState.targetPos = npcClick.ReturnTagetPos();
                    //playerState.targetPos = hit.collider.gameObject.GetComponent<NpcClick>().StartConversation();
                }
            }
            else
            {
                // 대화 중이라면 플레이어의 마우스 조작을 받음
                DialogSystem dialogSystem2 = QuestManager.questManager.dialogSystem;
                if (dialogSystem2.sunisDialog || dialogSystem2.moonisDialog) return;
                PhotonView photonView = dialogSystem2.GetComponent<PhotonView>();
                photonView.RPC(nameof(dialogSystem2.NextConversation), RpcTarget.All);
                //dialogSystem.NextConversation();
            }

        }
    }

    void OnEnable()
    {
        //1초 지난 후에 퀘스트 진행 시작
        StartCoroutine(QuestDelay());
    }

    IEnumerator QuestDelay()
    {
        //퀘스트 수락 후, 이야기 쭉 듣고
        yield return new WaitForSeconds(1.0f);
        if (QuestManager.questManager != null)
        {
            QuestManager qm = QuestManager.questManager;
            PhotonView photonView = qm.GetComponent<PhotonView>();
            photonView.RPC(nameof(qm.QuestAccept), RpcTarget.All, 1);
            photonView.RPC(nameof(qm.QuestAddProgress), RpcTarget.All, 1, 0, 1);
            //QuestManager.questManager.QuestAccept(1);
            //QuestManager.questManager.QuestAddProgress(1, 0, 1);
        }
    }

    [PunRPC]
    public void DestroyGetCollection(string collectionName)
    {
        Debug.Log("DestroyGetCollection 호출됨: " + collectionName);
        Transform Cooooooooooollections = GameObject.Find("Cooooooooooollections").transform;
        for (int i = 0; i < Cooooooooooollections.childCount; i++)
        {
            Transform child = Cooooooooooollections.GetChild(i);
            if (child.name == collectionName)
            {
                child.gameObject.SetActive(false);
                Debug.Log("클릭한 수집품 비활성화: " + collectionName);
                return;
            }
        }
        Debug.LogWarning("해당 수집품을 찾을 수 없음: " + collectionName);
    }
}