using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartScenePhotonManager : MonoBehaviourPunCallbacks
{
    public StartSceneManager startMG;

    public enum ServerType
    {
        ConnectServer,
        Offline
    }

    public ServerType serverType = ServerType.ConnectServer;

    private void Awake()
    {
        switch (serverType)
        {
            case ServerType.ConnectServer:
                OnConnect();
                break;
            case ServerType.Offline:
                PhotonNetwork.OfflineMode = true;
                break;
        }
    }

    public void OnConnect()
    {
        // 마스터 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("OnConnectedToMaster Finish");
    }

    public override void OnJoinedLobby()
    {
        print("JoinLobby Finish");
    }

    // 로그인, 계정 만들기 관련 인풋필드
    public TMP_Text loginId;
    public TMP_Text loginPw;
    public TMP_Text createId;
    public TMP_Text createPw;


    // 회원가입, 로그인 관련 (시도)
    public void JoinId(string urlValue)
    {
        StartCoroutine(UnityWebRequests(urlValue, "login", loginId.text, loginPw.text));
    }
    
    public void CreateId(string urlValue)
    {
        StartCoroutine(UnityWebRequests(urlValue, "join", createId.text, createPw.text));
    }


    IEnumerator UnityWebRequests(string urlValue, string api, string idValue, string pwValue)
    {
        string apikey = api;
        string currentUrl = urlValue + apikey;

        Account account = new Account();

        account.id = idValue;
        account.pw = pwValue;

        string accountData = JsonUtility.ToJson(account);

        byte[] jsonBins = Encoding.UTF8.GetBytes(accountData);

        UnityWebRequest request = new UnityWebRequest(currentUrl, "GET");
        request.timeout = 8;
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(jsonBins);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 다운로드 핸들러에서 텍스트 값을 받아서 UI에 출력한다.
            string response = request.downloadHandler.text;
            Result result = new Result();
            result = JsonUtility.FromJson<Result>(response);

            // 포톤네트워크 닉네임 설정
            PhotonNetwork.NickName = idValue;
            SaveSys.saveSys.setPath();
            SaveSys.saveSys.SaveQuizType(result.result);

            // 로그인 판단 실행
            startMG.LoginResult(result.result);
            Debug.Log(response);
        }
        else
        {
            PhotonNetwork.NickName = idValue;
            Debug.LogError(request.error);
            Debug.LogError(request.result);
            startMG.LoginResult(request.error + request.result);
            SaveSys.saveSys.SaveQuizType("호주 검은 공 사건");
        }
    }

    public class Account
    {
        public string id;
        public string pw;
    }

    public class Result
    {
        public string result;
    }
}
