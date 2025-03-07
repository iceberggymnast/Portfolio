using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightHandler : MonoBehaviourPun
{
    PlayerMove playerMove;
    public GameObject flash;
    public Light flashLight;  //드래그앤드랍용 아님! 다른 스크립트에도 쓰려고 public달아둠
    public GameObject playerCamera;

    void Start()
    {
    }

    void Update()
    {
        if (PlayerInfo.instance.isFlashStop) return;
        if (photonView.IsMine)
        {
            OnOffFlashLight();
        }
    }

    //바다일 때만 손전등 켜기
    public void OnOffFlashLight()
    {
        //우클릭 시
        if (Input.GetMouseButtonDown(1))
        {
            //손전등 켜져있다면 끄기
            if (PlayerInfo.instance.isFlashLightOn)
            {
                flashLight.intensity = 0;

                //손전등 상태를 '꺼짐'으로 변경
                PlayerInfo.instance.isFlashLightOn = false;
            }
            //손전등이 꺼져있다면 켜기
            else
            {
                //손전등 켜져있었으면 꺼주기
                flashLight.intensity = 10;

                //손전등 상태를 '켜짐'으로 변경
                PlayerInfo.instance.isFlashLightOn = true;
            }
        }
    }

    
}
