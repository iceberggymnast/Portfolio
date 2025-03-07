using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    //public Transform playerPos;
    //private static bool playerCreated = false;

    //int playerId = 0;


    //private void Start()
    //{
        

    //    if (!PhotonNetwork.IsMasterClient) return;
    //    print("1");
    //    //CreatePlayer();
    //}


    //void CreatePlayer()
    //{
    //    print("2");
    //    GameObject player = PhotonNetwork.Instantiate("Player/Player", playerPos.position, Quaternion.identity);
    //    player.name = "Player";
    //    PhotonView pv = player.GetPhotonView();
    //    print($"2{pv.ViewID}");
    //    RegisterToManager(pv);
    //}


    //private void RegisterToManager(PhotonView pv)
    //{
    //    print($"3{pv.ViewID}");
    //    playerId = PhotonNetwork.LocalPlayer.ActorNumber;
    //    int viewId = pv.ViewID;

    //    // Manager의 RegisterPlayerObject RPC 호출
    //    PlayerInfo.instance.photonView.RPC("RegisterPlayerObject", RpcTarget.All, playerId, viewId);
    //}
}
