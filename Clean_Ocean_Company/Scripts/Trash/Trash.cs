using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Trash : MonoBehaviour
{
    public bool isOnOutline = false;   //쓰레기 아웃라인 표시되었는지 여부
    public Sprite trashImage;   //쓰레기통에 넣을 쓰레기 이미지
    public string trashName;    //쓰레기 이름
    public int trashPoint;      //쓰레기 분리수거 시 얻는 포인트

    PhotonView pv;

    Vector3 pos;
    Vector3 rot;
    Vector3 scale;

    public bool isConsume = false;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        pos = transform.position;
        rot = transform.eulerAngles;
        scale = transform.localScale;
    }

    void Update()
    {
        if (isOnOutline)
        {
            GetComponent<Outline>().OutlineWidth = 7;
        }
        else
        {
            GetComponent<Outline>().OutlineWidth = 0;
        }
    }

    [PunRPC]
    public void disableRenderer()
    {
        if(gameObject != null)
        {
            if (gameObject.tag == "Oilbolb")
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    OilSpawner spawner = GameObject.FindFirstObjectByType<OilSpawner>();
                    if(spawner != null)
                    {
                        spawner.addList(pos, rot, scale);
                    }
                }
            }

            if (pv.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }

        }
    }


}
