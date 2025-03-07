using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerInfo;

public struct ChatbotData
{
    public string id;
    public string question;
}

public struct ChatbotHistory
{
    public string id;
}

public class Interaction_Chatbot : MonoBehaviour
{
    public string url;
    public GameObject chatbotCanvas;
    public Button closeCahtbotButton;
    Interaction_Base interaction_Base;

    PlayerMove playerMove;
    CameraRotate cameraRotate;

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    private void Awake()
    {
        if (chatbotCanvas == null) 
        {
            chatbotCanvas = GameObject.Find("Canvas_Chatbot");
        }

        closeCahtbotButton = chatbotCanvas.transform.Find("CloseButton").GetComponent<Button>();
        closeCahtbotButton.onClick.AddListener(OnOffChatbotUI);

        chatbotCanvas.SetActive(false);
    }

    IEnumerator Start()
    {
        interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = OnOffChatbotUI;




        if (SceneManager.GetActiveScene().name.Contains("AlphaScene"))
        {
            yield return new WaitUntil(() => PlayerInfo.instance.player != null);
            playerMove = PlayerInfo.instance.player.GetComponentInChildren<PlayerMove>();
            cameraRotate = PlayerInfo.instance.player.GetComponentInChildren<CameraRotate>();
        }

        yield return null;
    }

    

    void OnOffChatbotUI()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }


        chatbotCanvas.SetActive(!chatbotCanvas.activeSelf);
        PlayerInfo.instance.isCusor = chatbotCanvas.activeSelf;
        interaction_Base.useTrue = chatbotCanvas.activeSelf;

        if (SceneManager.GetActiveScene().name.Contains("AlphaScene"))
        {
            cameraRotate.useRotX = !chatbotCanvas.activeSelf;
            cameraRotate.useRotY = !chatbotCanvas.activeSelf;
            playerMove.isMove = !chatbotCanvas.activeSelf;
        }

    }

    void GetJoin()
    {
        StartCoroutine(UnityWebRequestGET());
    }

    void Test()
    {
        StartCoroutine(UnityWebRequestTest());
    }

    IEnumerator UnityWebRequestTest()
    {
        string apikey = "chatbot";
        string currentUrl = url + apikey;

        ChatbotData chatbotData = new ChatbotData();

        chatbotData.id = "윤종혁";
        chatbotData.question = "해양 오염이 위험한 이유가 뭘까?";

        string questionData = JsonUtility.ToJson(chatbotData);

        byte[] jsonBins = Encoding.UTF8.GetBytes(questionData);

        UnityWebRequest request = new UnityWebRequest(currentUrl, "GET");
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(jsonBins);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 다운로드 핸들러에서 텍스트 값을 받아서 UI에 출력한다.
            string response = request.downloadHandler.text;
            Debug.Log(response);
        }
        else
        {
            Debug.LogError(request.error);
            Debug.LogError(request.result);
        }
    }

    IEnumerator UnityWebRequestGET()
    {
        string apikey = "join";
        string currentUrl = url + apikey;

        Debug.Log(currentUrl);

        using (UnityWebRequest request = UnityWebRequest.Get(currentUrl))
        {
            Debug.Log(request);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                Debug.Log(request.result);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}
