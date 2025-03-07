using Photon.Pun;
using SingularityGroup.HotReload;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_LobbytoRoom : MonoBehaviourPun
{
    // 미션 맵 선택 UI
    public GameObject ui;

    void Start()
    {
        Interaction_Base act = GetComponent<Interaction_Base>();
        act.action = SceneMoveEvent;
    }

    void SceneMoveEvent()
    {
        PhotonNetwork.LoadLevel("ad");
    }

    public void OpenUI()
    {
        ui.SetActive(true);
    }
}
