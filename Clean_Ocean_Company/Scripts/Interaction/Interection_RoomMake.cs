using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class Interection_RoomMake : MonoBehaviourPunCallbacks
{
    public float cutSeceneTime = 5;

    public string loadSceneName;
    public SoundManger soundManger;

    // 맵 데이터
    [SerializeField]
    public List<MapData> mapData;

    // 지역 데이터
    [SerializeField]
    public List<LocationData> locationData;
    public LocationData currentLocation;

    public int currnetIndex = 0;

    // 맵 선택 UI
    public GameObject uiCanvas;

    // 위치 텍스트
    public GameObject locationText;

    // 화살표 버튼
    public Button btn1;
    public Button btn2;

    // 맵 기본 부모
    public GameObject mapParent;

    // 맵 움직일 이미지
    public GameObject objMapSet;

    // 맵 플레그 보관
    public GameObject objMapFlag;

    // 버튼 (여기로 이동)
    public Button overHere;

    // 처리
    GameObject go;

    // 선택한 미션 인덱스
    public int currentMissonIndex;

    // 미션 선택 화면인지 지도 선택 화면인지 체크
    public bool screenState;

    // 지도 선택 화면
    public GameObject map;
    // 미션 선택 화면
    public GameObject misson;
    // 로비 선택 화면
    public GameObject missonLobbyList;
    // 방만들기 화면
    public GameObject createRoom;
    public Button Btn_RoomMakePopup;

    // 텍스트
    public TMP_Text MissonName;
    public TMP_Text Missonprogress;
    public TMP_Text MissonShortExplanation;
    public TMP_Text Missontrash;
    public TMP_Text MissontrashRate;

    public TMP_Text MissonExplanation;

    UASS uASS;



    // 실행 함수 넣어줌
    void Start()
    {
        uASS = GameObject.FindAnyObjectByType<UASS>();

        if (soundManger == null)
        {
            soundManger = GameObject.FindObjectOfType<SoundManger>();
        }

        PhotonNetwork.JoinLobby();

        PhotonNetwork.AutomaticallySyncScene = true;

        Interaction_Base basee = GetComponent<Interaction_Base>();
        basee.action += OpenUI;

        QuestArrayReference();
        SetMissonData();

        soundManger.BGMSetting(3);

    }

    // 퀘스트 찾아 참조
    public void QuestArrayReference()
    {
        QuestManger qm = GameObject.FindAnyObjectByType<QuestManger>();

        for (int i = 0; i < locationData.Count; i++)
        {
            for (int j = 0; j < locationData[i].questInfo.Length; j++)
            {
                for (int l = 0; l < qm.questDatas.Count; l++)
                {
                    if (locationData[i].questInfo[j] == qm.questDatas[l].questID)
                    {
                        locationData[i].questData.Add(qm.questDatas[l]);
                        break;
                    }
                }
            }
        }
    }

    // UI 셋액티브
    public void OpenUI()
    {
        uiCanvas.SetActive(true);
        PlayerInfo.instance.isCusor = true;
    }

    // 맵 지정
    public void Mapset(bool plus)
    {
        // 설정
        if (plus)
        {
            currnetIndex++;
        }
        else
        {
            currnetIndex--;
        }

        if (currnetIndex == mapData.Count)
        {
            // 카운트가 넘어갔으니 0으로 
            currnetIndex = 0;
        }
        else if (currnetIndex < 0)
        {
            // 카운트가 마이너스가 되면 카운트의 -1값으로
            currnetIndex = mapData.Count - 1;
        }

        // 텍스트 적용
        TMP_Text text1 = locationText.transform.GetChild(1).GetComponent<TMP_Text>();
        TMP_Text text2 = locationText.transform.GetChild(2).GetComponent<TMP_Text>();
        text1.text = mapData[currnetIndex].name;
        text2.text = mapData[currnetIndex].name;
        MissonExplanation.text = mapData[currnetIndex].Explanation;

        // 텍스트 애니메이션 나와야 함
        if (plus)
        {
            btn1.interactable = false;
            btn2.interactable = false;
            locationText.transform.DOLocalMoveX(-623, 1, false).OnComplete(Reset);
        }
        else
        {
            btn1.interactable = false;
            btn2.interactable = false;
            locationText.transform.DOLocalMoveX(623, 1, false).OnComplete(Reset);
        }

        go = Instantiate(mapData[currnetIndex].pos, objMapFlag.transform);
        mapParent.transform.DOScale(new Vector3(0.8f, 0.8f, 1), 0.5f).SetLoops(2, LoopType.Yoyo);
        objMapSet.transform.SetParent(go.transform, true);
        objMapSet.transform.DOLocalMove(new Vector3(go.transform.localPosition.x * -2, go.transform.localPosition.y * -2, 0), 1, false).OnComplete(MapReset);

    }

    // 지도 움직임 애니메이션이 끝나면 초기화
    void MapReset()
    {
        objMapSet.transform.SetParent(mapParent.transform,true);
        Destroy(go);
    }

    // 텍스트 애니메이션이 끝나면 초기화
    private void Reset()
    {
        btn1.interactable = true;
        btn2.interactable = true;
        TMP_Text text = locationText.transform.GetChild(0).GetComponent<TMP_Text>();
        text.text = mapData[currnetIndex].name;
        locationText.transform.DOLocalMoveX(0, 0.01f, false);
    }

    // 지도에 지역 선택시 실행되는거
    public void MapSelect(int index)
    {
        overHere.interactable = true;
        currentLocation = locationData[index];
        MissonName.text = currentLocation.name;
        Missonprogress.text = "지역탐색율 0%";
        MissonShortExplanation.text = currentLocation.explanation;
        Missontrash.text = "쓰레기량";
        MissontrashRate.text = currentLocation.trashRate.ToString() + " %";
    }

    // 여기로 이동 선택할때 나오는거
    public void CreateRoom()
    {
        if (screenState)
        {
            uASS.enabled = false;
            SceneManager.LoadScene("CutScene_MoveToOcean", LoadSceneMode.Additive);
            uiCanvas.SetActive(false);
            StartCoroutine(JoinRoomFun(currnetRoomName));
            //StartCoroutine(CreatRoomFun(currentLocation.questData[currentMissonIndex].questName, currentLocation.questInfo));
        }
        else
        {
            screenState = true;
            map.SetActive(false);
            missonLobbyList.SetActive(true);
            Btn_RoomMakePopup.interactable = true;
            btn1.interactable = false;
            btn2.interactable = false;
            overHere.interactable = false;
        }
    }

    public IEnumerator CreatRoomFun(string roomName, int[] info)
    {
        yield return new WaitForSeconds(cutSeceneTime);
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        RoomOptions roomOptions = new RoomOptions();
        print(roomName + " 방 생성 시도");

        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "QuestInfo", info },
                {"QuestDataIndex", missonDatasIndex },
            };
        roomOptions.CustomRoomProperties = customProperties;
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "QuestDataIndex" };

        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public IEnumerator JoinRoomFun(string roomName)
    {
        yield return new WaitForSeconds(cutSeceneTime);
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        print("방 참가 시도 : " + roomName);
        PhotonNetwork.JoinRoom(roomName);
    }

    // 뒤로가기 눌렀을때
    public void GoBack()
    {
        if (screenState)
        {
            screenState = false;
            map.SetActive(true);
            missonLobbyList.SetActive(false);
            Btn_RoomMakePopup.interactable= false;
            overHere.interactable = false;
            btn1.interactable = true;
            btn2.interactable = true;
        }
        else
        {
            uiCanvas.SetActive(false);
            PlayerInfo.instance.isCusor = false;
            Interaction_Base basee = GetComponent<Interaction_Base>();
            basee.useTrue = false;
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        PlayerInfo.instance.isCusor = false;
        print("방 생성 완료");
    }

    public override void OnJoinedRoom()
    { 
        base.OnJoinedRoom();
        PlayerInfo.instance.isCusor = false;
        print("방 참가 완료");
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(1.0f);
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(loadSceneName);
        }
        else
        {
            SceneManager.LoadScene(loadSceneName);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("생성 실패.. 참가 시도");
        PhotonNetwork.JoinRoom(currentLocation.questData[currentMissonIndex].questName);
    }

    public List<RoomInfo> roomLists;
    // 룸 리스트 업데이트 시 호출되는 메서드
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        roomLists = roomList;
    }

    public override void OnJoinedLobby()
    {
        print("JoinLobby Finish");
    }

    public void Btn_OK()
    {
        if (soundManger != null)
        {
            soundManger.UISFXPlayRandom(8, 10);
        }
        else
        {
            soundManger = GameObject.FindObjectOfType<SoundManger>();
            soundManger.UISFXPlayRandom(8, 10);
        }
    }


    // 베타 때 추가
    public List<MissonData> missonDatas;
    public int missonDatasIndex = 0;

    public TMP_Text missonName;
    public TMP_Text missonExplanation;
    public TMP_Text missondetailexplanation;
    public TMP_Text missonPoint;

    public TMP_Text count;

    public GameObject clearText;

    public TMP_InputField roomName;

    public Button btn_createRoom;

    public string currnetRoomName;

    // 방 만들기 관련 기능 구현 
    public void SetMissonData()
    {
        missonName.text = missonDatas[missonDatasIndex].missonName;
        missonExplanation.text = missonDatas[missonDatasIndex].explanation;
        missondetailexplanation.text = missonDatas[missonDatasIndex].detailexplanation;
        missonPoint.text = "현재 습득 가능한 총 잔여 포인트 : <br>" + missonDatas[missonDatasIndex].maxPoint.ToString("N0") + " 포인트";
        clearText.SetActive(missonDatas[missonDatasIndex].isClear);
    }

    // 화살표 눌렀을때 
    public void Arrow(bool isPlus)
    {
        if (isPlus)
        {
            if (missonDatasIndex ==  missonDatas.Count - 1)
            {
                missonDatasIndex = 0;
            }
            else
            {
                missonDatasIndex++;
            }
        }
        else
        {
            if (missonDatasIndex == 0)
            {
                missonDatasIndex = missonDatas.Count - 1;
            }
            else
            {
                missonDatasIndex--;
            }
        }

        count.text = (missonDatasIndex + 1).ToString() + " / " + missonDatas.Count.ToString();
        SetMissonData();
    }

    public void Btn_CreateRoomUIPopup()
    {
        createRoom.SetActive(true);
    }

    public void Btn_CreateRoomBack()
    {
        createRoom.SetActive(false);
    }

    public void Btn_CreateRoom()
    {
        for (int i = 0; i < roomLists.Count; i++)
        {
            if(roomLists[i].Name == roomName.text)
            {
                print("방의 이름이 중복");
                return;
            }
        }
        uASS.enabled = false;
        SceneManager.LoadScene("CutScene_MoveToOcean", LoadSceneMode.Additive);
        uiCanvas.SetActive(false);
        StartCoroutine(CreatRoomFun(roomName.text, missonDatas[missonDatasIndex].questIndex));
    }

    public void InputfieldCheck()
    {

        if (roomName.text != "")
        {
            btn_createRoom.interactable = true;
        }
        else
        {
            btn_createRoom.interactable = false;
        }
    }

}

[Serializable]
public class MapData
{
    public string name = "황해의 어떤 바닷속";
    public GameObject pos;
    public string Explanation;
}

[Serializable]
public class LocationData
{
    public int id;
    public string name = "해초 숲";
    public string explanation = "기름에 뒤덮인 지역입니다. <br> 슬러지 제거 작업이 필요합니다.";
    public int trashRate = 89;
    [SerializeField]
    public List<string> issue;
    public int[] questInfo;
    public List<QuestData> questData;
}

[Serializable]
public class MissonData
{
    public string missonName = "[사전 퀘스트] 아기 상괭이를 돕자!";
    public string explanation = "기름에 뒤덮인 바다 속, 아기 상괭이가<br>다급하게 도움을 요청하는데…..";
    public string detailexplanation = "해당 퀘스트를 모두 클리어해야,<br>공용 퀘스트를 진행할 수 있습니다.";
    public int maxPoint = 32300;
    public bool isClear = false;
    public int[] questIndex;
}