using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objRotate : MonoBehaviourPun
{
    //회전 속력
    public float rotSpeed = 200.0f;
    //회전 값
    float rotX;
    float rotY;
    //회전 가능 여부
    public bool useRotX;
    public bool useRotY;

    //PhotonView(직접 넣어주기)
    //public PhotonView pv;

    public PlayerState playerState;

    void Start()
    {
        if (playerState == null)
        {
            playerState = GetComponent<PlayerState>();
        } 

        if (playerState == null)
        {
            playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();
        }
        //playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        // 현재 플레이어의 PhotonView를 찾습니다.
        //PhotonView myPhotonView = PhotonView.Get(this);  // 현재 스크립트가 붙어있는 객체의 PhotonView
        //if (myPhotonView != null)
        //{
        //    GameObject player = myPhotonView.gameObject;  // PhotonView가 붙어 있는 오브젝트가 현재 플레이어입니다.
        //    playerState = player.GetComponent<PlayerState>();
        //}
        if(!photonView.IsMine)
        {
            GameObject playerCamera = transform.GetChild(0).gameObject;
            playerCamera.SetActive(false);
        }
    }

    void Update()
    {
        if (!photonView.IsMine) { return; }
        //만약에 내것이라면
        if (!playerState.isOpenUI)
        {
            //1. 마우스의 움직임을 받아오자.
            float mx = Input.GetAxis("Mouse X");  //좌우
            float my = Input.GetAxis("Mouse Y");  //상하
                                                  //2. 회전 값을 변경( 누적 )
            if (useRotX) rotX += my * rotSpeed * Time.deltaTime;  //상하
            if (useRotY) rotY += mx * rotSpeed * Time.deltaTime;  //좌우
                                                                  //rotX의 값을 제한(최소값, 최대값)
            rotX = Mathf.Clamp(rotX, -60, 60);
            //3. 구해진 회전 값을 나의 회전 값으로 셋팅
            transform.localEulerAngles = new Vector3(-rotX, rotY, 0);  //회전값
        }
    }
}
