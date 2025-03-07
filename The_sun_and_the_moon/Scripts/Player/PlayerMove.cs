using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;

//PhotonView에 쉽게 접근하기 위해 MonoBehaviourPun를 상속받음
public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    CharacterController cc;
    public float moveSpeed;
    public GameObject cam;
    float yVelocity;
    float gravity = -9.81f;
    // 점프 높이
    public float jumpHeight = 2.0f;


    public PlayerState playerState;

    //Canvas_QuestUI
    public GameObject Canvas_QuestUI;
    //Canvas_Inventory
    public GameObject Canvas_Inventory;

    //발자국 소리
    public AudioSource walkingSoundSource;
    public AudioClip walkingSoundclip;

    //애니메이션
    public Animator HomePlayerCC;


    void Start()
    {
        //마지막 머지 하고 이 주석 풀기
        //QuestManager.questManager.myPlayer에 PlayerMove스크립트를 넣어준다
        if (QuestManager.questManager != null)
        {
            if (photonView.IsMine)
            {
                QuestManager.questManager.myPlayer = this.gameObject;

                //닉네임에 따라 모델링을 다르게 설정해주기
                if (PhotonNetwork.NickName == "해님")
                {
                    //해님이 캐릭터 켜고
                    transform.GetChild(0).gameObject.SetActive(true);
                    //달님이 캐릭터는 끄기
                    transform.GetChild(1).gameObject.SetActive(false);
                    //해님이 Animator가져오기
                    HomePlayerCC = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
                }
                else
                {
                    //해님이 캐릭터는 끄고
                    transform.GetChild(0).gameObject.SetActive(false);
                    //달님이 캐릭터는 켜기
                    transform.GetChild(1).gameObject.SetActive(true);
                    //달님이 Animator가져오기
                    HomePlayerCC = transform.GetChild(1).GetChild(0).GetComponent<Animator>();

                }
            }
        }
        //Transform interfaceTransform = GameObject.Find("[Interface]---------------------------").transform;
        GameObject interfaceTransform = GameObject.FindWithTag("Interface");
        

        //Canvas_QuestUI = interfaceTransform.Find("Canvas_QuestUI").gameObject;
        //Canvas_Inventory = interfaceTransform.Find("Canvas_Inventory").gameObject;
        if (interfaceTransform != null)
        {
            Canvas_QuestUI = interfaceTransform.transform.GetChild(2).gameObject;
            Canvas_Inventory = interfaceTransform.transform.GetChild(3).gameObject;
        }

        if (playerState == null)
        {
            playerState = GetComponent<PlayerState>();
        }
        // 현재 플레이어의 PhotonView를 찾습니다.
        //PhotonView myPhotonView = PhotonView.Get(this);  // 현재 스크립트가 붙어있는 객체의 PhotonView
        //if (myPhotonView != null)
        //{
        //    GameObject player = myPhotonView.gameObject;  // PhotonView가 붙어 있는 오브젝트가 현재 플레이어입니다.
        //    playerState = player.GetComponent<PlayerState>();
        //}

        //캐릭터 컨트롤러 가져오자.
        if (cc == null)
        {
            cc = GetComponent<CharacterController>();
        }
        //내 것일 때만 카메라를 활성화하자. 또는 testMove를 활성화했을 경우
        if (photonView.IsMine || playerState.testMove)
        {
            cam.SetActive(true);
        }

        if (Canvas_QuestUI != null && Canvas_Inventory != null)
        {
            Canvas_QuestUI.gameObject.SetActive(false);
            Canvas_Inventory.gameObject.SetActive(false);
        }

        ////QuestManager 에게 내캐릭터를 알려주자
        //if(photonView.IsMine)
        //{
        //    QuestManager.questManager.myPlayer = gameObject;
        //}

        if(walkingSoundSource == null)
        {
            walkingSoundSource = GetComponent<AudioSource>();
        }

    }

    void Update()
    {
        //내 것일 때만 컨트롤 하자!
        if (photonView.IsMine || playerState.testMove)
        {
            if (!playerState.isOpenUI)
            {
                Move();
            }

            OpenOrCloseQuestUI();
            OpenOrCloseInventory();
        }

    }



    //Player이동 함수
    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = transform.TransformDirection(dir);
        dir.Normalize();

        // 캐릭터가 움직이고 있는지 체크
        bool isMoving = (h != 0 || v != 0);

        //애니메이션에 사용할 속도 값 계산
        float speed = dir.magnitude;
        print(dir.magnitude);

        //움직이고 있으면 걷는 애니메이션 재생
        if(PhotonNetwork.NickName == "해님")
        {
            //걸을 때마다 내 캐릭터가 움직임
            HomePlayerCC.SetFloat("Speed",speed);
        }
        else
        {
            HomePlayerCC.SetFloat("Speed", speed);

        }


        //만약에 땅에 있으면 yVelocity를 0으로 초기화
        if (cc.isGrounded)
        {
            yVelocity = 0;

            // 스페이스바를 눌렀을 때 점프
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);  // 점프 속도 계산
            }

            //움직일 때 걷는 소리 재생
            if(isMoving && !walkingSoundSource.isPlaying)
            {
                walkingSoundSource.clip = walkingSoundclip;
                walkingSoundSource.Play();
            }
            //멈췄을 때는 걷는 소리 중지
            else if(!isMoving && walkingSoundSource.isPlaying)
            {
                walkingSoundSource.Stop();
            }

        }



        //yVelocity값을 중력에 의해서 변경시키자.
        yVelocity += gravity * Time.deltaTime;

        //dir.y 에 yVelocity값을 셋팅
        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    //아이템 인벤토리 캔버스 켜는 함수
    void OpenOrCloseQuestUI()
    {
        if (!playerState.isOpenUI && Input.GetKeyDown(KeyCode.I))
        {
            Canvas_Inventory.gameObject.SetActive(true);
            playerState.isOpenUI = true;
            print("아이템 인벤토리 열림");
        }
        //아이템 인벤토리 캔버스가 켜진 상태에서 I키 한번 더 누르면 캔버스가 닫힌다.
        else if (playerState.isOpenUI && Input.GetKeyDown(KeyCode.I))
        {
            Canvas_Inventory.gameObject.SetActive(false);
            playerState.isOpenUI = false;
            print("아이템 인벤토리 닫힘");
        }
    }


    //퀘스트 캔버스 켜는 함수
    void OpenOrCloseInventory()
    {
        if (!playerState.isOpenUI && Input.GetKeyDown(KeyCode.Q))
        {
            Canvas_QuestUI.gameObject.SetActive(true);
            playerState.isOpenUI = true;
            print("퀘스트 창 열림");
        }
        else if (playerState.isOpenUI && Input.GetKeyDown(KeyCode.Q))
        {
            Canvas_QuestUI.gameObject.SetActive(false);
            playerState.isOpenUI = false;
            print("퀘스트 창 닫힘");
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //만약에 내가 데이터를 보낼 수 있는 상태라면 (내 것이라면)
        if (stream.IsWriting)
        {
            ////PhotonServer한테 나의 위치값을 보낸다.
            //stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
            //첫번째 자식의 활성화 상태를 보낸다.
            stream.SendNext(transform.GetChild(0).gameObject.activeSelf);  
            //두번째 자식의 활성화 상태를 보낸다.
            stream.SendNext(transform.GetChild(1).gameObject.activeSelf);  
        }
        //데이터를 받을 수 있는 상태라면 (내 것이 아니라면)
        else if (stream.IsReading)
        {
            ////위치값을 받자.
            //transform.position = (Vector3)stream.ReceiveNext();  //ReceiveNext()함수의 반환 자료형은 GameObject라서 Position값을 받기 위해 vector3로 형 변환을 해줌
            //transform.rotation = (Vector3)stream.ReceiveNext();
            //첫번째 자식의 활성화 상태를 받기
            bool isSun = (bool)stream.ReceiveNext();
            bool isMoon = (bool)stream.ReceiveNext();

            //활성화 상태에 따라 모델링을 끄거나 켜기
            transform.GetChild(0).gameObject.SetActive(isSun);   //해님이 모델링 활성화할지 비활성화할지
            transform.GetChild(1).gameObject.SetActive(isMoon);  //달님이 모델링 활성화할지 비활성화할지

        }
    }

}
