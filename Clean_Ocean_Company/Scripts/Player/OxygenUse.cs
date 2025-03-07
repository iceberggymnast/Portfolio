//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class OxygenUse : MonoBehaviourPun
//{
//    public GameObject OxygenChargerPrefab;
//    GameObject O2chargerUI;

//    GameObject player;
//    void Start()
//    {

//        O2chargerUI = GameObject.Find("Canvas_TrashCan").transform.GetChild(5).gameObject;
//        PlayerInfo.instance.OnPlayerChanged += SetPlayer;
//    }

//    void SetPlayer(GameObject newPlayer)
//    {
//        player = newPlayer;
//    }

//    void Update()
//    {
//        installOxygenCharger();
//        OnOrOfff();
//    }

//    //T키 누르면 플레이어 앞에 산소충전기 오브젝트 생성해주는 함수
//    public void installOxygenCharger()
//    {
//        PlayerInfo playerInfo = PlayerInfo.instance;

//        if (Input.GetKeyDown(KeyCode.T))
//        {
//            if(playerInfo.current_OxygenchargerAmount > 0)
//            {
//                //플레이어 앞 5만큼 떨어진 위치 계산
//                Vector3 spawnPosition = player.transform.position + player.transform.forward * 5.0f;

//                //산소 충전기 생성
//                Instantiate(OxygenChargerPrefab, spawnPosition, player.transform.rotation);

//                //산소 충전기 개수 감소
//                playerInfo.current_OxygenchargerAmount--;

//                //확인용
//                print("산소 충전기 생성! 남은 충전기 수:" + playerInfo.current_OxygenchargerAmount);
//            }
//            else
//            {
//                print("현재 보유한 산소충전기 없음");
//            }
//        }
//    }

//    //현재 보유 산소충전기가 없으면 UI꺼주고, 있으면 UI켜주는 함수
//    public void OnOrOfff()
//    {

//        //현재 보유한 산소충전기가 있으면
//        //커서가 꺼져 -> 미니게임 안하고 있을 때, 충전기 보유하고 있으면 켜주고
//        if(PlayerInfo.instance.isCusor == false && PlayerInfo.instance.current_OxygenchargerAmount > 0)
//        {
//            //산소충전기 UI 켜주기
//            O2chargerUI.SetActive(true);
//        }
//        //현재 보유한 산소충전기가 없으면
//        else
//        {
//            //산소충전기 UI 꺼주기
//            O2chargerUI.SetActive(false);
//        }
//    }
//}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenUse : MonoBehaviourPun
{
    public GameObject OxygenChargerPrefab;
    public GameObject OxygenChargerPreviewPrefab; // 미리보기 오브젝트 프리팹

    private GameObject O2chargerUI;
    public GameObject player;
    private GameObject previewInstance; // 미리보기 오브젝트 인스턴스

    //GameObject Canvas_TrashCan;

    Trashcan_UI trashcanUI;

    private void Awake()
    {
        //Canvas_TrashCan = GameObject.FindGameObjectWithTag("Canvas_TrashCan");
        //O2chargerUI = Canvas_TrashCan.transform.GetChild(5).gameObject;
        //trashcanUI = GameObject.FindObjectOfType<Trashcan_UI>();
        //O2chargerUI = trashcanUI.o2chargerUI;
    }

    IEnumerator Start()
    {
        //yield return new WaitUntil(() => O2chargerUI != null);

        // PlayerInfo.instance 및 player 초기화 확인
        yield return new WaitUntil(() => PlayerInfo.instance != null);
        Debug.Log("PlayerInfo.instance 초기화 확인");

        yield return new WaitUntil(() => PlayerInfo.instance.player != null);
        Debug.Log("Player 초기화 확인");

        // trashcan_UI 및 o2chargerUI 초기화 확인
        yield return new WaitUntil(() => PlayerInfo.instance.trashcan_UI != null && PlayerInfo.instance.trashcan_UI.o2chargerUI != null);
        Debug.Log("trashcan_UI 및 o2chargerUI 초기화 확인");

        O2chargerUI = PlayerInfo.instance.trashcan_UI.o2chargerUI;

        //if (O2chargerUI == null)
        //{
        //    Debug.LogError("O2chargerUI가 null입니다.");
        //    yield break;
        //}

        while (O2chargerUI == null)
        {
            Debug.LogError("O2chargerUI가 null입니다.");
            yield return null;
        }


        PlayerInfo.instance.OnPlayerChanged += SetPlayer;
    }

    void SetPlayer(GameObject newPlayer)
    {
        player = newPlayer;
    }

    void Update()
    {
        if (O2chargerUI == null) return;
        HandleOxygenChargerPreview();
        OnOrOff();
    }

    // T 키를 누르면 미리보기 오브젝트 표시, 키를 뗐을 때 설치 완료
    private void HandleOxygenChargerPreview()
    {
        PlayerInfo playerInfo = PlayerInfo.instance;

        if (playerInfo.current_OxygenchargerAmount > 0) // 산소 충전기가 있는 경우에만 동작
        {
            // T 키를 누르고 있는 동안 미리보기 오브젝트 표시
            if (Input.GetKey(KeyCode.T))
            {
                if (previewInstance == null)
                {
                    // 플레이어 앞 5만큼 떨어진 위치에 미리보기 오브젝트 생성
                    Vector3 previewPosition = player.transform.position + player.transform.forward * 5.0f;
                    previewInstance = Instantiate(OxygenChargerPreviewPrefab, previewPosition, player.transform.rotation);
                }
                else
                {
                    // 플레이어 앞 위치에 미리보기 오브젝트 위치 업데이트
                    previewInstance.transform.position = player.transform.position + player.transform.forward * 5.0f;
                }
            }
            // T 키를 뗐을 때 미리보기 오브젝트를 제거하고 실제 산소 충전기를 설치
            else if (Input.GetKeyUp(KeyCode.T) && previewInstance != null)
            {
                // 미리보기 오브젝트의 위치에 산소 충전기 설치
                PhotonNetwork.Instantiate("Player/O2charger", previewInstance.transform.position, previewInstance.transform.rotation);
                Destroy(previewInstance); // 미리보기 오브젝트 제거

                playerInfo.current_OxygenchargerAmount--; // 산소 충전기 개수 감소
                print("산소 충전기 설치 완료! 남은 충전기 수:" + playerInfo.current_OxygenchargerAmount);
            }
        }
        else if (previewInstance != null)
        {
            Destroy(previewInstance); // 충전기가 없을 때 미리보기 오브젝트 제거
        }
    }

    // 현재 보유 산소충전기가 없으면 UI 꺼주고, 있으면 UI 켜주는 함수
    private void OnOrOff()
    {
        if (PlayerInfo.instance.isCusor == false && PlayerInfo.instance.current_OxygenchargerAmount > 0)
        {
            O2chargerUI.SetActive(true);
        }
        else
        {
            O2chargerUI.SetActive(false);
        }
    }
}

