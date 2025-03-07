using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public struct JsonChatData
{
    public string result;
    public string image;
    public string link;
}


public class ChatbotVerticalLayout : MonoBehaviour
{

    public enum ServerType
    {
        home,
        Alpha,
        Dummy
    }

    public ServerType serverType;

    public string url;


    public TMP_InputField chatInputfield;

    string type;

    public GameObject playerChatBoxPrefab;
    public GameObject AIBoxPrefab;
    public GameObject AITodayPrefab;
    public GameObject AIButtonPrefab;

    PlayerMoveInLobby playerMoveInLobby;

    const float textHeight = 17.9f;

    // Content 의 Transform
    public RectTransform trContent;

    // ChatView 의 Transform
    public RectTransform trChatView;

    public bool isStartChat = false;

    List<float> buttonList = new List<float>();

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name.Contains("Lobby"))
        {
            playerMoveInLobby = GameObject.FindWithTag("Player").GetComponent<PlayerMoveInLobby>();
        }
    }

    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name.Contains("Lobby"))
            playerMoveInLobby.isMoving = false;
    }

    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().name.Contains("Lobby"))
            playerMoveInLobby.isMoving = true;
    }

    IEnumerator Start()
    {
        chatInputfield.onSubmit.AddListener(OnSubmit);

        type = SaveSys.saveSys.LoadQuizType();

        buttonList.Add(200);

        switch (serverType)
        {
            case ServerType.home:
                url = "http://221.163.19.142:9100/";
                break;
            case ServerType.Alpha:
                url = "http://221.163.19.142:9100/";
                break;
            case ServerType.Dummy:
                url = "http://221.163.19.142:9100/";
                break;
        }


        yield return null;
    }

    private void Update()
    {
        EnterEvent();
    }

    public void OnSubmit(string s)
    {
        if (s.Length == 0) return;

        // 플레이어 채팅 박스 생성
        GameObject playerChatBox = Instantiate(playerChatBoxPrefab, trContent.position, Quaternion.identity);
        playerChatBox.transform.SetParent(trContent, false);




        // 채팅 텍스트 설정
        TMP_Text playerText = playerChatBox.GetComponentInChildren<TMP_Text>();
        playerText.text = s;

        RectTransform playerChatBoxRectTransform = playerChatBox.GetComponent<RectTransform>();


        // Text의 현재 높이에 기본 높이 값을 나누기
        float playerBoxHeightIndex = playerText.preferredHeight / textHeight > 1 ? playerText.preferredHeight : 0;

        // RectTransform 설정
        RectTransform playerRectTransform = playerChatBox.transform.GetChild(0).GetComponent<RectTransform>();
        playerRectTransform.sizeDelta = new Vector2(playerRectTransform.sizeDelta.x, playerRectTransform.sizeDelta.y + playerBoxHeightIndex);




        RectTransform playerTextRectTransform = playerText.GetComponent<RectTransform>();
        playerTextRectTransform.sizeDelta = new Vector2(playerTextRectTransform.sizeDelta.x, playerTextRectTransform.sizeDelta.y + playerBoxHeightIndex);

        AutoScrollBottom(playerRectTransform.sizeDelta.y);
        trChatView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

        AIResultEvent(s);


        // 입력 필드 초기화
        chatInputfield.text = "";
    }


    public void OnSubmitButton(string s)
    {
        if (s.Length == 0) return;
        


        // 플레이어 채팅 박스 생성
        GameObject playerChatBox = Instantiate(playerChatBoxPrefab, trContent.position, Quaternion.identity);


        playerChatBox.transform.SetParent(trContent, false);


        // 채팅 텍스트 설정
        TMP_Text playerText = playerChatBox.GetComponentInChildren<TMP_Text>();
        playerText.text = s;


        // Text의 현재 높이에 기본 높이 값을 나누기
        float playerBoxHeightIndex = playerText.preferredHeight / textHeight > 1 ? playerText.preferredHeight : 0;

        // RectTransform 설정
        RectTransform playerRectTransform = playerChatBox.transform.GetChild(0).GetComponent<RectTransform>();
        playerRectTransform.sizeDelta = new Vector2(playerRectTransform.sizeDelta.x, playerRectTransform.sizeDelta.y + playerBoxHeightIndex);


        RectTransform playerTextRectTransform = playerText.GetComponent<RectTransform>();
        playerTextRectTransform.sizeDelta = new Vector2(playerTextRectTransform.sizeDelta.x, playerTextRectTransform.sizeDelta.y + playerBoxHeightIndex);

        AutoScrollBottom(playerRectTransform.sizeDelta.y);

        if (s == "오늘의 예상 퀴즈는?")
        {
            AIResultEvent(s);
            return;
        }



        AIResultEvent(s);


        // 입력 필드 초기화
        chatInputfield.text = "";
    }


    void TypeResultEvent(string type)
    {
        StartCoroutine(AskType(type));
    }

    IEnumerator AskType(string value)
    {

        yield return new WaitForSeconds(1.5f);


        GameObject aiChatBox = Instantiate(AITodayPrefab, trContent);
        trChatView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

        TMP_Text aiText = aiChatBox.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

        if (type != null)
        {
            aiText.text = $"오늘의 퀴즈는 <color=#00FFFF>\n\"{type}</color>\" 테마에서 나올 수 있습니다.\n아래의 링크를 통해 미리 공부하고 가는 것을 추천드려요!";
        }
        else
        {
            aiText.text = $"오늘의 퀴즈는 <color=#00FFFF>\n\"{"호주 검은 공 사건"}</color>\" 테마에서 나올 수 있습니다.\n아래의 링크를 통해 미리 공부하고 가는 것을 추천드려요!";
        }

        #region Chatbot의 Text 높이
        // Text의 현재 높이에 기본 높이 값을 나누기
        float aiBoxHeightIndex = aiText.preferredHeight / textHeight > 1 ? aiText.preferredHeight : 0;

        // RectTransform 설정
        RectTransform aiRectTransform = aiChatBox.transform.GetChild(0).GetComponent<RectTransform>();
        aiRectTransform.sizeDelta = new Vector2(aiRectTransform.sizeDelta.x, aiRectTransform.sizeDelta.y + aiBoxHeightIndex);
        float height = aiRectTransform.sizeDelta.y;

        RectTransform aiTextRectTransform = aiText.GetComponent<RectTransform>();
        aiTextRectTransform.sizeDelta = new Vector2(aiTextRectTransform.sizeDelta.x, height - 23.86f);
        #endregion


        AutoScrollBottom(aiRectTransform.sizeDelta.y);
    }


    void EnterEvent()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isStartChat)
        {
            Debug.Log("Enter 키로 입력 시작");
            isStartChat = true;
            chatInputfield.ActivateInputField();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && isStartChat)
        {
            Debug.Log("Enter 키로 입력 완료");
            isStartChat = false;
            OnSubmit(chatInputfield.text);
            chatInputfield.DeactivateInputField();
        }
    }


    void AIResultEvent(string value)
    {
        StartCoroutine(AskChatbot(value));
    }

    IEnumerator AskChatbot(string value)
    {
        string apikey = "chatbot";
        if (serverType == ServerType.Dummy)
        {
            apikey = "testchatbot";
        }
        string currentUrl = url + apikey;

        ChatbotData chatbotData = new ChatbotData();

        chatbotData.id = !string.IsNullOrEmpty(PhotonNetwork.NickName) ? PhotonNetwork.NickName : "TestPlayer";
        chatbotData.question = value;

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

            JsonChatData jsonChatData = JsonUtility.FromJson<JsonChatData>(response);

            string result = jsonChatData.result;

            GameObject aiChatBox = Instantiate(AIBoxPrefab, trContent);
            trChatView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;

            TMP_Text aiText = aiChatBox.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();

            //aiText.text = $"오늘의 퀴즈는 <color=#00FFFF>\n\"{type}</color>\" 테마에서 나올 수 있습니다.\n아래의 링크를 통해 미리 공부하고 가는 것을 추천드려요!\n\n{result}";
            aiText.text = result;

            byte[] binaries = Convert.FromBase64String(jsonChatData.image);

            RawImage img_response = aiChatBox.GetComponentInChildren<RawImage>();

            if (binaries.Length > 0)
            {
                Texture2D texture = new Texture2D(256, 256);
                texture.LoadImage(binaries);
                img_response.texture = texture;
            }

            #region Chatbot의 Text 높이
            // Text의 현재 높이에 기본 높이 값을 나누기
            float aiBoxHeightIndex = aiText.preferredHeight / textHeight > 1 ? aiText.preferredHeight : 0;

            // RectTransform 설정
            RectTransform aiRectTransform = aiChatBox.transform.GetChild(0).GetComponent<RectTransform>();
            aiRectTransform.sizeDelta = new Vector2(aiRectTransform.sizeDelta.x, aiRectTransform.sizeDelta.y + aiBoxHeightIndex);

            float height = aiRectTransform.sizeDelta.y;

            RectTransform aiTextRectTransform = aiText.GetComponent<RectTransform>();
            aiTextRectTransform.sizeDelta = new Vector2(aiTextRectTransform.sizeDelta.x, height - 23.86f);
            #endregion

            TMP_Text aiLinkText = aiChatBox.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            aiLinkText.text = jsonChatData.link;

            float aiLinkHeightIndex = aiLinkText.preferredHeight / textHeight > 1 ? aiLinkText.preferredHeight : 0;

            RectTransform aiImageRectTransform = aiChatBox.transform.GetChild(1).GetChild(0).GetComponent<RectTransform>();

            RectTransform aiLinkRectTransform = aiChatBox.transform.GetChild(1).GetChild(1).GetComponent<RectTransform>();
            aiLinkRectTransform.sizeDelta = new Vector2(aiLinkRectTransform.sizeDelta.x, aiLinkRectTransform.sizeDelta.y + aiLinkHeightIndex);

            RectTransform aiLinkTextRectTransform = aiText.GetComponent<RectTransform>();
            aiLinkTextRectTransform.sizeDelta = new Vector2(aiLinkTextRectTransform.sizeDelta.x, aiLinkTextRectTransform.sizeDelta.y + aiLinkHeightIndex);


            Button linkButton = aiChatBox.transform.GetChild(1).GetChild(1).GetComponent<Button>();
            linkButton.onClick.AddListener(() => PrivateButton(jsonChatData.link));


            print($"aiRectTransform : {aiRectTransform.sizeDelta.y}");
            print($"aiTextRectTransform : {aiTextRectTransform.sizeDelta.y}");
            print($"aiLinkRectTransform : {aiLinkRectTransform.sizeDelta.y}");
            print($"aiLinkTextRectTransform : {aiLinkTextRectTransform.sizeDelta.y}");
            print($"aiImageRectTransform : {aiImageRectTransform.sizeDelta.y}");
            print($"{aiLinkTextRectTransform.sizeDelta.y - aiLinkRectTransform.sizeDelta.y}");

            RectTransform prefabRectTransform = aiChatBox.GetComponent<RectTransform>();
            print($"prefabRectTransform 1 : {prefabRectTransform.sizeDelta.y}");

            prefabRectTransform.sizeDelta = new Vector2(prefabRectTransform.sizeDelta.x , aiRectTransform.sizeDelta.y + aiLinkRectTransform.sizeDelta.y + aiImageRectTransform.sizeDelta.y + 30);


            print($"prefabRectTransform 2 : {prefabRectTransform.sizeDelta.y}");

            print($"{aiLinkTextRectTransform.sizeDelta.y - aiLinkRectTransform.sizeDelta.y}");

            AutoScrollBottom(aiRectTransform.sizeDelta.y + 315 + aiLinkRectTransform.sizeDelta.y + 15);
            //AutoScrollBottom(aiRectTransform.sizeDelta.y);
        }
        else
        {
            Debug.LogWarning(request.error);
            Debug.LogWarning(request.result);
            TypeResultEvent("오늘의 예상 퀴즈는?");
        }
    }

    public void PrivateButton(string link)
    {
        Application.OpenURL(link);
    }

    IEnumerator HistoryChatbot(string value)
    {
        string apikey = "get_history";

        string currentUrl = url + apikey;

        ChatbotHistory chatbotHistory = new ChatbotHistory();

        chatbotHistory.id = !string.IsNullOrEmpty(PhotonNetwork.NickName) ? PhotonNetwork.NickName : "TestPlayer";

        string historyData = JsonUtility.ToJson(chatbotHistory);

        byte[] jsonBins = Encoding.UTF8.GetBytes(historyData);

        UnityWebRequest request = new UnityWebRequest(currentUrl, "GET");
        request.SetRequestHeader("Content-Type", "application/json");
        request.uploadHandler = new UploadHandlerRaw(jsonBins);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // 다운로드 핸들러에서 텍스트 값을 받아서 UI에 출력한다.
            string response = request.downloadHandler.text;

            string result = JsonUtility.ToJson(response);

            byte[] values = Encoding.UTF8.GetBytes(result);

            Debug.Log(values[0]);

            //GameObject go = Instantiate(AIBoxPrefab, trContent);

            //TMP_Text text = go.GetComponentInChildren<TMP_Text>();
            //text.text = result;

            //RectTransform rect = go.transform.GetChild(0).GetComponent<RectTransform>();
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, text.preferredHeight);

            //rect.anchoredPosition = new Vector2(-200, currentY);

            //// 다음 채팅 박스의 Y 위치 업데이트
            //currentY -= text.preferredHeight + 30; // +30은 박스 간 간격을 위해 추가


            //RectTransform areaRect = go.GetComponent<RectTransform>();




            //AutoScrollBottom();
        }
        else
        {
            Debug.LogWarning(request.error);
            Debug.LogWarning(request.result);
            TypeResultEvent("오늘의 예상 퀴즈는?");
        }
    }

    public void AutoScrollBottom(float lastHeight)
    {
        // Content의 높이 갱신
        RectTransform chatContentTr = trContent.GetComponent<RectTransform>();

        buttonList.Add(lastHeight + 50);

        float currentHeight = 0;

        foreach (var button in buttonList)
        {
            currentHeight += button;
        }


        if (trContent.sizeDelta.y < currentHeight)
        {
            trContent.sizeDelta = new Vector2(trContent.sizeDelta.x, currentHeight + 50);
        }

        // 이전 바닥에 닿아있었다면
        //if (prevContentH - trChatView.sizeDelta.y <= trContent.anchoredPosition.y)
        //{


        //     Content의 높이 조정
        //    float newHeight = trContent.sizeDelta.y + lastHeight + 50; // 여백 포함
        //    trContent.sizeDelta = new Vector2(trContent.sizeDelta.x, newHeight);
        //}

        trChatView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}
