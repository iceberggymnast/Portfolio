using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interaction_MoveGet : MonoBehaviour
{
    // 상호작용 했을때 놓고 잡고 할 수 있음

    // 잡기 여부
    public bool isGrab;

    public MonoBehaviour cs;

    Interaction_Base del;

    HelpUIController helpUIController;

    PlayerInteraction playerInteraction;

    Animator animator;

    public GameObject playerModel;

    GameObject hipObject;

    public FlashLightHandler flashLighter;

    public Vector3 dirPos;
    public Vector3 dirRot;

    bool isOcean = false;


    public Transform taget;

    public bool isStart = false;

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;


    void Start()
    {
        del = GetComponent<Interaction_Base>();
        del.action = Interaction;
        del.OnPlayerChanged += UpdatePlayerReference;
        helpUIController = GameObject.FindObjectOfType<HelpUIController>();
    }

    private void UpdatePlayerReference(GameObject newPlayer)
    {
        playerModel = newPlayer;
        animator = playerModel.transform.GetChild(0).GetComponent<Animator>();
        playerInteraction = playerModel.GetComponent<PlayerInteraction>();
        Vacuum vacuum = playerModel.GetComponent<Vacuum>();
        flashLighter = vacuum.flashLight.GetComponent<FlashLightHandler>();
    }

    void Update()
    {
        /*
        if (isGrab)
        {
            // posOffset.rotation에 x와 y 축 회전값을 추가적으로 설정
            Vector3 dir = new Vector3(hipObject.transform.localRotation.x + 90, hipObject.transform.rotation.y, hipObject.transform.rotation.z);

            dir = dir + dirRot;

            transform.localPosition = Vector3.Lerp(transform.localPosition, dirPos, Time.deltaTime * 10.0f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(dir), Time.deltaTime * 10.0f);
        }
        */

        if (Input.GetKeyDown(KeyCode.Space) && isGrab)
        {
            Interaction();
        }
    }
    public void Interaction()
    {

        if (!isStart) return;
        //taget = transform.GetChild(2).GetComponent<Transform>();

        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        taget = transform.GetChild(2).GetComponent<Transform>();


        // Dotween으로 모션 적용
        PlayerMove playerMove =  PlayerInfo.instance.player.transform.GetChild(0).GetComponent<PlayerMove>();
        playerMove.isMove = false;
        
        // 해당 오브젝트의 잡기 여부를 전환해준다.
        isGrab = !isGrab;
        animator.SetBool("Hug", isGrab);
        PlayerInfo.instance.isFlashLightOn = false;
        //PlayerInfo.instance.isFlashStop = isGrab;
        //flashLighter.flashLight.intensity = (isGrab == true) ? 0f : 150f;
        //transform.gameObject.layer = (isGrab == true) ? 0 : 10;

        if (isGrab)
        {
            PlayerInfo.instance.player.transform.DOMove(taget.position, 1.1f);
            PlayerInfo.instance.player.transform.GetChild(0).DORotateQuaternion(taget.rotation, 1.1f).OnComplete(MoveDone);
        }
        else
        {
            MoveDone();
        }    
    }

    void MoveDone()
    {


        
        // 변화된 상태를 확인
        if (isGrab)
        {
            // 잡아짐
            //transform.parent = PlayerInfo.instance.playerObj.transform.GetChild(0).GetChild(0).GetChild(0);
            Transform posTarget = PlayerInfo.instance.player.transform.Find("Model/Diver_proto/Armature/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:LeftShoulder/mixamorig:LeftArm/mixamorig:LeftForeArm/mixamorig:LeftHand");
            transform.SetParent(posTarget, true);
            //hipObject = transform.parent.gameObject;
            //PlayerInfo.instance.currnetInteraction = transform.gameObject;
            helpUIController.helpMediapipeUI.SetActive(true);
            playerInteraction.pointHitTrue = true;
            PlayerInfo.instance.player.transform.GetChild(0).DORotate(new Vector3(0, PlayerInfo.instance.player.transform.GetChild(0).transform.eulerAngles.y, 0), 1.0f);
        }
        else
        {
            // 놓아짐
            transform.parent = null;
            PlayerInfo.instance.currnetInteraction = null;
            Debug.Log("1111");
            if (isOcean)
            {
                Debug.Log("222");
                PlayerMove playerMove2 = PlayerInfo.instance.player.transform.GetChild(0).GetComponent<PlayerMove>();
                playerMove2.isMove = true;

                cs.enabled = true;
                this.enabled = false;
            }
            helpUIController.helpMediapipeUI.SetActive(false);
            del.useTrue = false;
            playerInteraction.pointHitTrue = false;
            //transform.rotation = Quaternion.identity;
        }

        PlayerMove playerMove = PlayerInfo.instance.player.transform.GetChild(0).GetComponent<PlayerMove>();
        playerMove.isMove = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ocean")
        {
            isOcean = true;
        }
    }

}
