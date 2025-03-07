using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectionAndRoobyMgr : MonoBehaviourPunCallbacks
{
    // 마스터: ConnetionScene
    // 로비 및 방: CharacterSelectScene
    // 게임: GameScene

    //마스터 서버 접속 - 로비 입장 - 방 생성 및 입장 다 될 때까지 버튼 비활성화할 건데, 그때 띄울 LoadingText UI
    public GameObject LoadingText;
    public Button Btn_gotoCharacterSelectScene;

    //서버 연결 상태 표시할 텍스트 UI
    public TextMeshProUGUI connectionStatusText;

    //InputField.text가져오기
    public TMP_InputField URL_InputField;

    //현재 씬의 빌드 인덱스
    int currentSceneIndex;

    //원래 입력되어있던 InputField 값
    string OriginTnputFieldText = "";

    public TMP_Text dropDownText;

    ////서버로 연결하고 방 입장할 때 띄울 텍스트("서버 연결 후, 해님달님 방 입장하는 중")
    //public GameObject LoadingServerText;


    void Start()
    {
        //시작할 때는 로딩중 텍스트 비활성화
        LoadingText.SetActive(false);

        // 초기 텍스트 설정
        connectionStatusText.text = "서버 상태: 연결되지 않음";

        //이미 저장된 url이 있다면
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("URL_InputField.text")))
        {
            //저장된 url이 있으면 InputField에 해당 url을 표시
            URL_InputField.text = PlayerPrefs.GetString("URL_InputField.text");
        }
    }

        void Update()
        {
            //InputField값이 변경되얶다면
            if(URL_InputField.text != OriginTnputFieldText)
            {
                //PlayerPrefs에 갱신
                OriginTnputFieldText = URL_InputField.text;
                PlayerPrefs.SetString("URL_InputField.text", OriginTnputFieldText);
                PlayerPrefs.Save();
            }
        }


    //캐릭터 선택하기 버튼 누르면 마스터 - 로비 - 방 만들어지고 - 캐릭터 선택 씬으로 바로 감
    void BTN_gotoCharacterSelectScene()
        {
            //버튼 다시 못 누르게 비활성화해두고
            Btn_gotoCharacterSelectScene.interactable = false;
        
            //로딩중 텍스트 띄워주기
            LoadingText.SetActive(true);

            //print(PlayerPrefs.GetString("URL_InputField.text"));

            // InputField에서 엔터를 치거나 포커스를 읽었을 때, url이 유효한지 검사하는 코루틴 실행
            //URL_InputField.onEndEdit.AddListener(OnUrlInputEndEdit);

            //캐릭터 선택하는 버튼 눌렀을 때 이전에 저장되었던 url주소로 서버 연결 성공했는지 바로 확인
            //이전에 입력된 주소값이 있었다면 그 값이 유효한지 검사(재시작 시)
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("URL_InputField.text")))
            {
                StartCoroutine(CheckUrlValidity(PlayerPrefs.GetString("URL_InputField.text")));
            }
            //저장된 url없으면 InputField에서 입력한 url로 유효성 검사(맨 처음 시작 시)
            else if (!string.IsNullOrEmpty(URL_InputField.text))
            {
                StartCoroutine(CheckUrlValidity(URL_InputField.text));
            }
            //버튼만 누르고 InputField에 아무것도 입력 안한 상태면
            else
            {
                connectionStatusText.text = "URL주소를 입력하세요 :>";
                Btn_gotoCharacterSelectScene.interactable = true;
                LoadingText.SetActive(false);
            }
        }
    
    // 마스터 서버 접속
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("마스터 서버 연결 성공");

        // 로비 입장
        PhotonNetwork.JoinLobby();
    }

    // 로비 들어가는 함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        print("로비 입장!");

        // 방 생성 또는 입장 시도
        CreateOrJoinRoom();
    }

    //방이 없으면 해님달님 방을 생성하고, 방이 있으면 그 방에 바로 들어가는 함수
    private void CreateOrJoinRoom()
    {
        // 방 찾기 시도
        PhotonNetwork.JoinRoom(dropDownText.text); // 특정 방에 입장 시도
    }

    //미리 만들어진 방이 없어서 방 입장에 실패하면(JoninRoom이 실패하면) 방을 만드는 오버로드 함수
    //(처음 시작할 때는 무조건 방 입장에 실패하니까 바로 방 옵션 설정되고 OnCreatedRoom 오버로드 함수 실행됨)
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        // 방 입장 실패 시 방 생성 시도
        print("방 입장 실패 : " + message + " - 방을 생성합니다.");

        //방 입장에 실패 시, 버튼 누를 수 있게 활성화
        Btn_gotoCharacterSelectScene.interactable = true;

        //최대입장인원2명, 방이 생성되었을 때 다른 플레이어가 입장 가능하게 IsOpen을 True로 설정, 그 방이 다른 플레이어에게 보이도록 IsVisible을 True로 설정
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        roomOption.IsOpen = true;
        roomOption.IsVisible = true;

        //방 만들기
        PhotonNetwork.CreateRoom(dropDownText.text, roomOption);
    }

    //방 만드는 오버로드 함수
    //첫번째 유저는 방 생성 후 바로 그 방에 들어가게 됨
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료!");
    }

    //방에 입장하는 오버로드 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        print("방 참여 완료!");
        //로딩중 텍스트 띄워주기
        //LoadingText.SetActive(false);

        //버튼 누를 수 있게 활성화
        //
        //Btn_gotoCharacterSelectScene.interactable = true;

        //캐릭터 선택 씬으로 이동
        PhotonNetwork.LoadLevel(1);
        print("캐릭터 선택 씬 입장!");



        //로딩중 텍스트 꺼주기
        LoadingText.SetActive(false);
    }

    //인풋 필드에서 엔터를 치거나 포커스를 잃으면 호출될 함수
    public void OnUrlInputEndEdit(string input)
    {
        //입력한 URL로 서버 연결 시도
        if (!string.IsNullOrEmpty(input))
        {
            StartCoroutine(CheckUrlValidity(input.Trim()));
        }
    }


    // URL 유효성 검사 코루틴
    public IEnumerator CheckUrlValidity(string url)
    {
        // URL에 "/reset" 추가
        string checkUrl = url + "reset";

        using (UnityWebRequest request = UnityWebRequest.Get(checkUrl))
        {
            // 요청을 보내고 응답을 기다림
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("유효한 URL입니다!");

                // 서버 연결 성공 메시지 표시
                connectionStatusText.text = "서버 연결 성공!";

                //유효한 URL인거 확인 후에 포톤 서버에 연결
                PhotonNetwork.ConnectUsingSettings();

                // PlayerPrefs에 URL_InputField에 입력한게 있으면 URL 문자열을 저장
                PlayerPrefs.SetString("URL_InputField.text", URL_InputField.text);   //키,밸류 순
                PlayerPrefs.Save();

                //url검사는 끝났고 끝난 결과 텍스트 보는 용도로 코루틴
                //yield return 3초 기다렸다가
                //캐릭터씬으로 이동
            }
            else
            {
                // 서버 연결 실패 메시지 표시
                connectionStatusText.text = "서버 연결 실패: " + request.error;

                //서버 연결 실패시 다시 캐릭터 선택하러가기 버튼 활성화해서 url유효성검사하고 서버 연결 시도하기
                Btn_gotoCharacterSelectScene.interactable = true;

                //서버 연결 실패했으니까 다시 로딩중 글씨는 끄기
                LoadingText.SetActive(false);

            }
        }
    }

    [PunRPC]
    void LoadingTxt(bool isActive)
    {
        LoadingText.gameObject.SetActive(isActive);
    }
}
