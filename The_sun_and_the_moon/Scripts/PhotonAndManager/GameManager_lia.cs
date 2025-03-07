using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager_lia : MonoBehaviour
{
    GameObject playerFactory;

    void Start()
    {
        //플레이어를 생성(현재 Room에 접속 되어있는 친구들도 보이게)
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
