using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class url : MonoBehaviour
{
    //InputField.text가져오기
    public TMP_InputField URL_InputField;

    //서버 연결 상태 표시할 텍스트 UI
    public TextMeshProUGUI connectionStatusText;

    //url이랑 urlPath가져오기 위한 aiProtocol 스크립트
    AiProtocol aiProtocol;

    //호랑이 url가져오기 위한 webPost 스크립트
    WebPost webPost;

    //현재 씬의 빌드 인덱스
    int currentSceneIndex;

    //캐릭터 선택하기 버튼 가져오기
    ConnectionAndRoobyMgr connectionAndRoobyMgr;

    void Start()
    {
        connectionAndRoobyMgr = FindObjectOfType<ConnectionAndRoobyMgr>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;


        // 씬에 따라 URL 설정
        SetURL();
    }



    private void SetURL()
    {
        //현재 씬이 마사지 게임이거나 밧줄 타기 게임 씬일 때
        if (currentSceneIndex == 3 || currentSceneIndex == 6)
        {
            //PlayerPrefs에 저장했던 URL문자열 불러오기
            string URL = PlayerPrefs.GetString("URL_InputField.text", "AI서버 URL이 저장되지 않았습니다. 인풋필드에 입력하세요.");
            aiProtocol = FindObjectOfType<AiProtocol>();


            //만약 현재 씬이 마사지 게임 씬일 때(씬 인덱스 7번) 
            if (currentSceneIndex == 3)
            {
                // aiProtocol이 null이 아닌지 확인
                if (aiProtocol != null)
                {
                    //url설정
                    aiProtocol.url = URL;
                    //urlPath는 massage
                    if(PhotonNetwork.NickName == "달님")
                    {
                        aiProtocol.actorId = 2;
                    }
                    aiProtocol.urlPath = "massage";
                }
            }
            //만약 현재 씬이 밧줄 타기 게임 씬일 때(씬 인덱스 9번)
            else if (currentSceneIndex == 6)
            {
                // aiProtocol이 null이 아닌지 확인
                if (aiProtocol != null)
                {
                    //url설정
                    aiProtocol.url = URL;
                    //urlPath는 tether
                    aiProtocol.urlPath = "tether";
                }
            }
        }

        //현재 씬이 호랑이 손 맞추기 게임 씬일 때
        if (currentSceneIndex == 8)
        {
            //webPost스크립트 가져오기
            webPost = FindObjectOfType<WebPost>();
            //PlayerPrefs에 저장했던 URL문자열 불러오기
            string URL = PlayerPrefs.GetString("URL_InputField.text", "AI서버 URL이 저장되지 않았습니다. 인풋필드에 입력하세요.");

            //url설정
            webPost.url = URL + "tiger_hand1";
        }
    }



}


//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Networking;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class url : MonoBehaviour
//{
//    //InputField.text가져오기
//    public TMP_InputField URL_InputField;

//    //서버 연결 상태 표시할 텍스트 UI
//    public TextMeshProUGUI connectionStatusText;

//    //url이랑 urlPath가져오기 위한 aiProtocol 스크립트
//    AiProtocol aiProtocol;

//    //호랑이 url가져오기 위한 webPost 스크립트
//    WebPost webPost;

//    //현재 씬의 빌드 인덱스
//    int currentSceneIndex;

//    //캐릭터 선택하기 버튼 가져오기
//    ConnectionAndRoobyMgr connectionAndRoobyMgr;

//    void Start()
//    {
//        connectionAndRoobyMgr = FindObjectOfType<ConnectionAndRoobyMgr>();

//        //현재 씬의 빌드 인덱스
//        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
//        Debug.Log("현재 씬 번호: " + currentSceneIndex);

//        if(currentSceneIndex == 0)
//        {
//            // InputField의 onEndEdit 이벤트에 리스너 추가
//            // 캐릭터 선택하러가기 버튼 클릭했다면
//            // InputField에서 엔터를 치거나 포커스를 읽었을 때, url이 유효한지 검사하는 코루틴 실행
//            URL_InputField.onEndEdit.AddListener(OnUrlInputEndEdit);

//            // 초기 텍스트 설정
//            connectionStatusText.text = "서버 상태: 연결되지 않음";
//        }

//        //현재 씬이 connection씬이 아닐때
//        //url저장
//        else
//        {
//            // 씬에 따라 URL 설정
//            SetURL();
//        }
//    }

//    // 인풋 필드에서 엔터를 치거나 포커스를 잃으면 호출될 함수
//    public void OnUrlInputEndEdit(string input)
//    {
//        //입력한 URL로 서버 연결 시도
//        if (!string.IsNullOrEmpty(input) )
//        {
//            StartCoroutine(CheckUrlValidity(input.Trim()));
//        }
//    }

//    private void SetURL()
//    {
//        //현재 씬이 마사지 게임이거나 밧줄 타기 게임 씬일 때
//        if (currentSceneIndex == 7 || currentSceneIndex == 9)
//        {
//            //PlayerPrefs에 저장했던 URL문자열 불러오기
//            string URL = PlayerPrefs.GetString("URL_InputField.text", "AI서버 URL이 저장되지 않았습니다. 인풋필드에 입력하세요.");
//            aiProtocol = FindObjectOfType<AiProtocol>();


//            //만약 현재 씬이 마사지 게임 씬일 때(씬 인덱스 7번) 
//            if (currentSceneIndex == 7)
//            {
//                // aiProtocol이 null이 아닌지 확인
//                if (aiProtocol != null)
//                {
//                    //url설정
//                    aiProtocol.url = URL;
//                    //urlPath는 massage
//                    aiProtocol.urlPath = "massage";
//                }
//            }
//            //만약 현재 씬이 밧줄 타기 게임 씬일 때(씬 인덱스 9번)
//            else if (currentSceneIndex == 9)
//            {
//                // aiProtocol이 null이 아닌지 확인
//                if (aiProtocol != null)
//                {
//                    //url설정
//                    aiProtocol.url = URL;
//                    //urlPath는 tether
//                    aiProtocol.urlPath = "tether";
//                }
//            }
//        }

//        //현재 씬이 호랑이 손 맞추기 게임 씬일 때
//        if (currentSceneIndex == 8)
//        {
//            //webPost스크립트 가져오기
//            webPost = FindObjectOfType<WebPost>();
//            //PlayerPrefs에 저장했던 URL문자열 불러오기
//            string URL = PlayerPrefs.GetString("URL_InputField.text", "AI서버 URL이 저장되지 않았습니다. 인풋필드에 입력하세요.");

//            //url설정
//            webPost.url = URL + "tiger_hand1";
//        }
//    }


//    // URL 유효성 검사 코루틴
//    public IEnumerator CheckUrlValidity(string url)
//    {
//        // URL에 "/reset" 추가
//        string checkUrl = url + "reset";

//        using (UnityWebRequest request = UnityWebRequest.Get(checkUrl))
//        {
//            // 요청을 보내고 응답을 기다림
//            yield return request.SendWebRequest();

//            if (request.result == UnityWebRequest.Result.Success)
//            {
//                Debug.Log("유효한 URL입니다!");

//                // 서버 연결 성공 메시지 표시
//                connectionStatusText.text = "서버 연결 성공!";

//                // PlayerPrefs에 URL_InputField에 입력한게 있으면 URL 문자열을 저장
//                if(URL_InputField.text != null)
//                {
//                    PlayerPrefs.SetString("URL_InputField.text", URL_InputField.text);   //키,밸류 순
//                    PlayerPrefs.Save();
//                }
//                //인풋 필드에 값이 저장된 게 없으면 이전의 값으로 입력된
//                else
//                {
//                    PlayerPrefs.SetString("URL_InputField.text", PlayerPrefs.GetString("URL_InputField.text"));   //키,밸류 순
//                    PlayerPrefs.Save();
//                }

//                // 씬에 따라 URL 설정
//                SetURL();
//            }
//            else
//            {
//                // 서버 연결 실패 메시지 표시
//                connectionStatusText.text = "서버 연결 실패: " + request.error;
//                Debug.LogError("서버 연결 실패: " + request.error);
//            }
//        }
//    }
//}
