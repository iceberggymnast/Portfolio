using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton_Lobby : MonoBehaviourPun
{
    // 뒤로 가기 버튼을 누르기 전 플레이어의 위치와 회전을 저장하는 변수
    private Vector3 originalPlayerPosition;
    private Quaternion originalPlayerRotation;

    // 뒤로 가기 버튼 클릭 시 닫을 캔버스
    public GameObject closeCanvas;
    public GameObject interactionCanvas;
    public bool isClose = false;

    // 플레이어 오브젝트 (위치 및 회전 저장 및 복원에 사용)
    GameObject player;

    void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            //플레이어 찾기
           
                player = PlayerInfo.instance.player;

                // 현재 플레이어의 위치와 회전을 저장
                originalPlayerPosition = player.transform.position;
                originalPlayerRotation = player.transform.rotation;
            
        }        
    }

    // 뒤로 가기 버튼 클릭 시 호출될 함수
    public void OnBackButtonClicked()
    {
        

        //닫혔음을 표시
        isClose = true;
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            // 저장해둔 위치와 회전으로 플레이어를 이동 및 회전
            player.transform.position = originalPlayerPosition;
            player.transform.rotation = originalPlayerRotation;
        }


        // closeCanvas가 활성화되어 있다면 비활성화
        closeCanvas.SetActive(false);
    }
}
