using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UASSManager : MonoBehaviour
{
    // 플레이어가 처음 생성되면 UASS 라이트에 참조가 없어서 넣어주는 기능을 함 
    public Light baseLight;
    public GameObject player;

    private void Update()
    {
        if (player == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                PhotonView pv = players[i].GetComponent<PhotonView>();
                if (pv.IsMine)
                {
                    player = players[i];
                    break;
                }
            }
        }

        if (player  != null)
        {
            UASS uass = player.GetComponentInChildren<UASS>();
            uass.light = baseLight;
            Destroy(this.gameObject);
        }
    }
}
