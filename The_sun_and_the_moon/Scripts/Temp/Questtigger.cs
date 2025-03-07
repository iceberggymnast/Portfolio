using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Questtigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (QuestManager.questManager.questList[6].questState == QuestData.QuestState.progress)
        {
            PhotonView photonView = GetComponent<PhotonView>();
            photonView.RPC(nameof(MovePlayer), RpcTarget.All);
        }
    }

    public List<GameObject> movePoint;

    [PunRPC]
    public void MovePlayer()
    {
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

        QuestManager.questManager.QuestAddProgress(6, 0, 1);
        NpcClick npcClick = GameObject.Find("NPC_tree").GetComponent<NpcClick>();
        //QuestManager.questManager.myPlayer.transform.position = npcClick.gameObject.transform.position;
        npcClick.StartConversation();
    }
}
