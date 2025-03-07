using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Interaction_NextScene;

public class Interaction_NextScene : MonoBehaviourPunCallbacks
{
    public string interactionName = "배로 돌아가기";

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    public enum SceneName
    {
        AlphaLobby_Yoon2,
        AlphaLobby_Lia
    }

    public SceneName sceneName = SceneName.AlphaLobby_Lia;

    // Start is called before the first frame update
    void Start()
    {
        // Base 스크립트의 델리게이트를 찾아서 상호작용시 원하는 함수를 넣어줌
        Interaction_Base del = GetComponent<Interaction_Base>();
        del.action = LeaveRoom;
        del.intername = interactionName;
    }

    void MoveNextScene()
    {
        PhotonManager_Lobby.instance.CreateRoom();
        PlayerInfo.instance.isCusor = false;
    }

    void LeaveRoom()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        PhotonNetwork.LeaveRoom();
        PlayerInfo.instance.isCusor = false;
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(sceneName.ToString());
    }

    //// 방 생성 완료 후에 씬을 로드하도록 콜백을 사용합니다.
    //public override void OnCreatedRoom()
    //{
    //    PhotonNetwork.LoadLevel(sceneName.ToString());
    //}
}
