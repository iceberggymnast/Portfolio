using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectMgr : MonoBehaviourPunCallbacks
{
    // 마스터: ConnetionScene
    // 로비 및 방: CharacterSelectScene
    // 게임: GameScene

    //이 스크립트 부터는 방에 입장한 후에 닉네임 설정하고 캐릭터 선택하는 기능 시작

    //해님 선택 버튼
    public Button SelectCharacterBTN_01;
    //달님 선택 버튼
    public Button SelectCharacterBTN_02;

    //서버로 연결하고 방 입장할 때 띄울 텍스트("서버 연결 후, 해님달님 방 입장하는 중")
    public GameObject LoadingText;
    //플레이어 2명 모일때까지 띄워줄 텍스트("함께할 플레이어 기다리는 중..")
    public GameObject WaitPlayerText;

    //이야기 영감님 관련 오브젝트들
    public GameObject StoryGranfa;

    //캐릭터 선택 관련 오브젝트들
    public GameObject CharacterSelect;

    //캐릭터 선택하면 뜨는 체크표시
    public GameObject Img_sunCheck;
    public GameObject Img_moonCheck;

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(UiLoad), RpcTarget.AllBuffered);
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        // 방에 플레이어가 아직 두 명이 모이지 않았다면 "플레이어 기다리는 중" 텍스트를 켜기
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            photonView.RPC(nameof(WaitPlayerTxt), RpcTarget.All, true);
        }
    }

    [PunRPC]
    void UiLoad()
    {
        SceneManager.LoadScene("QuestManagerScence", LoadSceneMode.Additive);
    }

    private void Update()
    {
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);



        // 마스터(방장)이라면
        // 인원 다 들어왔다면
        if (PhotonNetwork.IsMasterClient)
        {

            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                //둘다 들어왔으니까 "플레이어 기다리는 중"텍스트 꺼주기
                photonView.RPC(nameof(WaitPlayerTxt), RpcTarget.All, false);

                photonView.RPC(nameof(ShowStoryGranfa), RpcTarget.All, true);

                //코루틴 실행 2초
                StartCoroutine(Wait2Seconds());
                // photonView.RPC(nameof(ShowChacterButton), RpcTarget.All, true);
            }
        }
    }

    #region
    //void CheckTwoPlayer()
    //{
    //    //플레이어가 2명이 아닐 때는,
    //    if (PhotonNetwork.PlayerList.Length != 1)
    //    {
    //        //플레이어 기다리는 중 텍스트 켜기
    //        photonView.RPC(nameof(WaitText), RpcTarget.All, true);
    //        //캐릭터 선택 관련 게임오브젝트들 끄기
    //        photonView.RPC(nameof(ShowChacterButton), RpcTarget.All, false);
    //    }

    //    //플레이어가 두명 모였을 때만
    //    //캐릭터 선택씬으로 들어온 플레이어가 2명일 때,
    //    else if (PhotonNetwork.PlayerList.Length == 2)
    //    {
    //        //플레이어 기다리는 중 텍스트 끄기
    //        photonView.RPC(nameof(WaitText), RpcTarget.All, false);
    //        twoPlayer();
    //    }
    //}

    //플레이어 두명 모였을 때 실행
    //void twoPlayer()
    //{
    //    //해님달님방 입장중이라고 띄워줬다가
    //    photonView.RPC(nameof(LoadingTxt), RpcTarget.All, false);

    //    //이야기 영감 게임오브젝트 켜기
    //    photonView.RPC(nameof(ShowStoryGranfa), RpcTarget.All, true);

    //    //2초 뒤에 코루틴 통해서 이야기 영감 게임 오브젝트 끄고, 캐릭터 선택 관련 게임오브젝트들 켜기, 캐릭터 버튼 활성화
    //    StartCoroutine(Wait2Seconds());

    //}
    #endregion

    //이야기 영감님 텍스트 읽는 시간 2초
    //동기화 관련 참고: 코루틴을 호출하는 부분만 Photon RPC로 동기화하면 모든 클라이언트에서 똑같이 실행됨!
    IEnumerator Wait2Seconds()
    {
        yield return new WaitForSeconds(2.0f);

        //이야기 영감 게임오브젝트 끄기
        photonView.RPC(nameof(ShowStoryGranfa), RpcTarget.All, false);

        //캐릭터 선택 관련 게임오브젝트들 켜기
        photonView.RPC(nameof(ShowChacterButton), RpcTarget.All, true);

        //버튼도 활성화
        SelectCharacterBTN_01.interactable = true;
        SelectCharacterBTN_02.interactable = true;
    }
    
    [PunRPC]
    void WaitPlayerTxt(bool isActive)
    {
        WaitPlayerText.gameObject.SetActive(isActive);
    }

    //이야기 영감 UI
    [PunRPC] 
    void ShowStoryGranfa(bool isActive)
    {
        StoryGranfa.gameObject.SetActive(isActive);
    }

    //캐릭터 선택 UI
    [PunRPC]
    void ShowChacterButton(bool isActive)
    {
        CharacterSelect.SetActive(isActive);
    }

    //해님이 캐릭터 클릭 시 닉네임을 sun으로 저장
    public void Btn_gotoGameScene_01()
    {
        //여기에 닉네임 설정
        PhotonNetwork.NickName = "해님";

        //SelectCharacter함수의 매개변수로 "해님"이 전달되면서 해당 Client의 닉네임은 해님이 됨, 체크표시도 동기화
        photonView.RPC(nameof(SelectCharacter), RpcTarget.All, "해님",1);
    }

    //해님이 캐릭터 클릭 시 닉네임을 moon으로 저장
    public void Btn_gotoGameScene_02()
    {
        //닉네임 달님
        PhotonNetwork.NickName = "달님";

        //SelectCharacter함수의 매개변수로 "달님"이 전달되면서 해당 Client의 닉네임은 달님이 됨, 체크표시도 동기화
        photonView.RPC(nameof(SelectCharacter), RpcTarget.All, "달님",2);
    }

    //캐릭터 버튼을 선택하면 그에 따라 닉네임이 설정되고(해님/달님), 게임 씬으로 이동하는 함수
    [PunRPC]
    void SelectCharacter(string characterName, int buttonIndex)
    {
        //선택된 캐릭터의 닉네임을 설정
        //PhotonNetwork.NickName = characterName;

        //버튼 중복 클릭 안하도록 비활성화
        if(buttonIndex == 1)
        {
            SelectCharacterBTN_01.interactable = false;
            Img_sunCheck.SetActive(true);

        }
        else if(buttonIndex == 2)
        {
            SelectCharacterBTN_02.interactable = false;
            Img_moonCheck.SetActive(true);

        }

        //선택한 캐릭터에 체크표시
        //UpdateCheckMarks(buttonIndex);
        if(Img_sunCheck.activeSelf && Img_moonCheck.activeSelf)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                // 게임 씬으로 이동
                PhotonNetwork.LoadLevel(2); 
            }
        }
    }

    void UpdateCheckMarks(int buttonIndex)
    {
        // 체크 표시 이미지 활성화
        Img_moonCheck.SetActive(buttonIndex == 2);
    }
}
