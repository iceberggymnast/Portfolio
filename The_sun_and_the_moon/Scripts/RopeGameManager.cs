using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGameManager : MonoBehaviourPun
{
    // 밧줄을 얼마나 타고 올라갈지 목표지점?
    public float targetHeigh;

    // 카메라가 따라갈 게임 오브젝트
    public List<GameObject> playermodel;
    public int playersel;

    // 위치 오프셋
    public Vector3 offset;

    // 고정 오프셋
    public float fixedOffset = 6;

    // 결과 값을 받아 올 스크립트
    public JsonToClass jsonToClass;

    // 오르기용 목표 지점 계산
    Vector3 tagetPos;

    // 감지용 이전 결과 값
    [SerializeField]
    int beforeResult;
    // 결과 값
    [SerializeField]
    int result;

    // 애니메이션
    public List<Animator> anim; 

    // 끝났음
    public GameObject canvas;

    // 미니게임 연동
    public MiniGameCanvas miniGameCanvas;

    // 밧줄타기 성공할 때 마다 나오는 효과음 클립
    public AudioClip climbingSoundClip;

    private AudioSource bgm; // 오디오 재생을 위한 AudioSource

    // 스크롤 넘길 값 계산
    float firstH;
    float socoreOffset;

    void Start()
    {
        firstH = playermodel[0].transform.position.y;

        // 내가 조작하고 있는 캐릭터가 뭘까
        if (PhotonNetwork.NickName == "해님")
        {
            playersel = 1;
        }

        beforeResult = 0;
        tagetPos = playermodel[playersel].transform.position;

        // AudioSource가 이미 존재하는지 확인하고 없으면 추가
        bgm = gameObject.AddComponent<AudioSource>();
        bgm.playOnAwake = false; // 자동 재생 비활성화
    }

    void Update()
    {
        socoreOffset = (playermodel[playersel].transform.position.y - firstH) / (targetHeigh - firstH);
        miniGameCanvas.socoreBarValue = socoreOffset;

        // 결과 값을 받아옴
        if (jsonToClass.resultInt != null)
        {
            result = jsonToClass.resultInt.result;
        }
        // 만약 before값과 다르면 값이 변한것임
        if (result > beforeResult)
        {
            //밧줄타는 효과음 재생
            if (!bgm.isPlaying)
            {
                bgm.clip = climbingSoundClip; // AudioClip 설정
                bgm.Play(); // 작업 했는데 재생이 되지 않아 수정
            }

            // 값을 동기화 시켜준다.
            beforeResult = result;

            if (playersel == 0)
            { 
                photonView.RPC(nameof(Climbing), RpcTarget.All, 0);
            }
            else
            {
                photonView.RPC(nameof(Climbing), RpcTarget.All, 1);
            }
        }

        // 타겟 값으로 player를 움직이게 만든다
        //playermodel.transform.position = Vector3.Lerp(playermodel.transform.position, tagetPos, Time.deltaTime * 10);

        // 카메라의 높이는 플레이어랑 동일하게 맞춰준다.. (목포지점에 어느정도 도달하기 전까진)
        if (targetHeigh - fixedOffset > playermodel[playersel].transform.position.y)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, playermodel[playersel].transform.position.y, Camera.main.transform.position.z) + offset;
        }

        // player 모델의 위치가 목표 높이에 도달 하면..
        if (playermodel[0].transform.position.y >= targetHeigh)
        {
            // 난 게임이 끝!
            print("yay");
            //canvas.SetActive(true);
            miniGameCanvas.StartCoroutine(miniGameCanvas.WinPopup(1));
        }

        if (playermodel[1].transform.position.y >= targetHeigh)
        {
            // 난 게임이 끝!
            print("yay");
            //canvas.SetActive(true);
            miniGameCanvas.StartCoroutine(miniGameCanvas.WinPopup(0));
        }

    }



    [PunRPC]
    void Climbing(int player)
    {
        // 올라간다
        tagetPos = playermodel[player].transform.position + Vector3.up;
        anim[player].SetTrigger("GoingUp");
    }

}
