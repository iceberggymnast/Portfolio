using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMassageScene : MonoBehaviour
{
    // 플레이어의 태그
    [SerializeField]
    private string playerTag = "Player";

    // 트리거 충돌이 발생할 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 플레이어인지 확인
        if (other.CompareTag(playerTag))
        {
            // 씬을 전환
            PhotonNetwork.LoadLevel("Minigame_massage");
        }
    }
}
