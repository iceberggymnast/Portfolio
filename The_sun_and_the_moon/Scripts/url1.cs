using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class url1 : MonoBehaviour
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

        // 씬에 따라 URL 설정
        SetURL();
    }



    private void SetURL()
    {
        //현재 씬이 마사지 게임이거나 밧줄 타기 게임 씬일 때
        if (currentSceneIndex == 7 || currentSceneIndex == 9)
        {
            //PlayerPrefs에 저장했던 URL문자열 불러오기
            string URL = PlayerPrefs.GetString("URL_InputField.text", "AI서버 URL이 저장되지 않았습니다. 인풋필드에 입력하세요.");
            aiProtocol = FindObjectOfType<AiProtocol>();


            //만약 현재 씬이 마사지 게임 씬일 때(씬 인덱스 7번) 
            if (currentSceneIndex == 6)
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
            else if (currentSceneIndex == 9)
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
