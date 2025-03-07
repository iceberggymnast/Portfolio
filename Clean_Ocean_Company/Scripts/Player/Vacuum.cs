using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Vacuum : MonoBehaviour
{
    public PhotonView photonView;

    //플레이어한테 붙어있는 손전등
    public GameObject flashLight;
    //플레이어 찾아주기
    GameObject player;
    //이전 아웃라인 오브젝트 리스트
    List<GameObject> previousOutLineObjects = new List<GameObject>();

    [SerializeField]
    private double currentTrashcanAmout = 0;  //현재 채워진 쓰레기양
    //쓰레기통 용량 %로 표시(UI)
    public TextMeshProUGUI trashcanCapacity_UI;

    //손전등 범위 넓히려면 아래 두개 조정하기
    public float detectionRadius = 2.0f;
    public float coneAngle = 45.0f; // 원뿔의 최대 각도

    // 기름덩어리 프리팹
    public GameObject oilBlobPrefab;

    public bool isStop = true;

    //청소기 쪽으로 움직이기 전 기름 원래 위치 및 사이즈(다시 생성하기 위해서)
    Vector3 oilPosition;
    Vector3 oilScale;

    interection_WhaleVacumm interection_WhaleVacumm;

    public AudioSet audioSet;

    GameObject Canvas_TrashCan;

    Trashcan_UI trashcanUI;

    private void Awake()
    {
        interection_WhaleVacumm = FindObjectOfType<interection_WhaleVacumm>();
        //trashcanUI = GameObject.FindObjectOfType<Trashcan_UI>();
        //Canvas_TrashCan = GameObject.FindGameObjectWithTag("Canvas_TrashCan");
        //trashcanCapacity_UI = Canvas_TrashCan.transform.GetChild(6).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //trashcanCapacity_UI = trashcanUI.trashPercentageText;
    }

    IEnumerator Start()
    {
        //flashLight = GameObject.Find("Player").transform.GetChild(2).gameObject;
        //player = GameObject.Find("Player").gameObject;
        // PlayerInfo.instance 및 player 초기화 확인
        yield return new WaitUntil(() => PlayerInfo.instance != null);
        Debug.Log("PlayerInfo.instance 초기화 확인");

        yield return new WaitUntil(() => PlayerInfo.instance.player != null);
        Debug.Log("Player 초기화 확인");

        yield return new WaitUntil(() => PlayerInfo.instance.trashcan_UI != null && PlayerInfo.instance.trashcan_UI.trashPercentageText != null);
        Debug.Log("trashcan_UI 및 trashPercentageText 초기화 확인");


        trashcanCapacity_UI = PlayerInfo.instance.trashcan_UI.trashPercentageText;

        //if (trashcanCapacity_UI == null)
        //{
        //    Debug.LogError("trashcanCapacity_UI가 null입니다.");
        //    yield break;
        //}

        while (trashcanCapacity_UI == null)
        {
            Debug.LogError("trashcanCapacity_UI가 null입니다.");
            yield return null;
        }



        yield return new WaitUntil(() => trashcanCapacity_UI != null);

        audioSet = GetComponent<AudioSet>();

        if (photonView.IsMine)
        {
            StartCoroutine(InitializeTrashcanUI());
        }
    }

    private IEnumerator InitializeTrashcanUI()
    {
        yield return new WaitUntil(() => trashcanCapacity_UI != null || GameObject.FindWithTag("Trashcan_capacity").transform.GetChild(1) != null);

        if (trashcanCapacity_UI == null)
        {
            trashcanCapacity_UI = GameObject.FindWithTag("Trashcan_capacity").transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        }

        if (trashcanCapacity_UI != null)
        {
            trashcanCapacity_UI.text = "0%";
            //Debug.Log("trashcanCapacity_UI 텍스트가 0%로 설정되었습니다.");
        }
        else
        {
            Debug.LogError("Trashcan_capacity UI를 찾을 수 없습니다.");
        }
    }


    void Update()
    {
        if (trashcanCapacity_UI == null) return;
        if (!isStop) return;
        if (PlayerInfo.instance.isFlashStop) return;
        if (photonView.IsMine)
        {
            OutLine_flashLight();
            CleaningTrash();
        }
    }

    public void OutLine_flashLight()
    {
        // SphereCast로 감지된 오브젝트들을 필터링
        RaycastHit[] hits = Physics.SphereCastAll(flashLight.transform.position, detectionRadius, flashLight.transform.forward, PlayerInfo.instance.detectionDistance);

        // 손전등 레이 시각적으로 표시 (디버그용)
        Debug.DrawRay(flashLight.transform.position, flashLight.transform.forward * PlayerInfo.instance.detectionDistance, Color.green); // 레이 시각화

        List<GameObject> currentHitObjects = new List<GameObject>();

        // 이전에 활성화된 오브젝트의 아웃라인을 모두 꺼줌
        for (int i = 0; i < previousOutLineObjects.Count; i++)
        {
            if (previousOutLineObjects[i].gameObject != null)
            {
                previousOutLineObjects[i].GetComponent<Trash>().isOnOutline = false;
                //Debug.Log($"{previousOutLineObjects[i].name} 아웃라인 비활성화");
            }
        }

        // 감지된 오브젝트에 아웃라인 적용
        for (int i = 0; i < hits.Length; i++)
        {
            GameObject obj = hits[i].collider.gameObject;

            // 감지된 오브젝트가 6번 레이어(쓰레기)인지/기름 덩어리인지/퀘스트용 기름 덩어리 인지 확인
            if (obj.layer == 6 || obj.CompareTag("Oilbolb") || obj.CompareTag("Oilbolb_Quest"))
            {
                // 손전등 방향과 오브젝트 간의 각도를 계산
                Vector3 directionToObj = (obj.transform.position - flashLight.transform.position).normalized;
                float angleToObj = Vector3.Angle(flashLight.transform.forward, directionToObj);

                // 각도가 원뿔의 범위 내에 있는지 확인 (예: 45도)
                if (angleToObj <= coneAngle)
                {
                    obj.GetComponent<Trash>().isOnOutline = true;
                    currentHitObjects.Add(obj);
                    //Debug.Log($"{obj.name} 아웃라인 활성화");
                }
            }
        }
        // 이전 감지된 오브젝트 리스트를 현재 감지된 오브젝트 리스트로 교체
        previousOutLineObjects = new List<GameObject>(currentHitObjects);
    }

    public ParticleSystem particle;

    // 쓰레기 수거하는 함수(레이 중심축 기준, 1.0F 내로 떨어진 쓰레기들은 더 빨리 흡수되도록 함)
    public void CleaningTrash()
    {
        for (int i = 0; i < previousOutLineObjects.Count; i++)
        {
            GameObject trash = previousOutLineObjects[i];

            // 기름 덩어리의 위치와 크기를 미리 저장
            if (trash.CompareTag("Oilbolb"))
            {
                oilPosition = trash.transform.position;
                oilScale = trash.transform.localScale;
            }
        }

        //왼클릭 시작시 청소기 시작소리 재생
        if (Input.GetMouseButtonDown(0))
        {
            audioSet.OBJSFXPlay(0, false);
            particle.Play();
        }

        //왼클릭 뗐을 시 청소기 종료소리 재생
        if (Input.GetMouseButtonUp(0))
        {
            audioSet.OBJSFXStop(0);
            audioSet.OBJSFXStop(1);
            audioSet.OBJSFXStop(2);
            audioSet.OBJSFXPlay(2, false);
            particle.Stop();
        }

        //왼클릭 하고있을 때 청소기 진행소리 재생
        if (Input.GetMouseButton(0))
        {
            audioSet.OBJSFXPlay(1, false);

            // 중심축을 기준으로 쓰레기 흡수 반경 설정
            float radiusThreshold = 2.0f; // 흡입이 빠르게 되는 반경
            float fastSpeedMultiplier = 2.0f; // 반경 내 속도 증가 배율

            for (int i = 0; i < previousOutLineObjects.Count; i++)
            {
                GameObject trash = previousOutLineObjects[i];
                PhotonView pv = trash.GetComponent<PhotonView>();
                if (!pv.IsMine)
                {
                    pv.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
                }

                // 손전등 중심축(레이 방향)과 쓰레기 간의 최소 거리 계산
                Vector3 rayDirection = flashLight.transform.forward;
                Vector3 toTrash = trash.transform.position - flashLight.transform.position;
                float distanceToRay = Vector3.Cross(rayDirection, toTrash).magnitude / rayDirection.magnitude;

                // 흡입 속도 조절: 특정 반경 내에서는 빠르게, 그 외에는 기본 속도 적용
                float adjustedCleaningSpeed = PlayerInfo.instance.cleaningSpeed;
                if (distanceToRay <= radiusThreshold)
                {
                    adjustedCleaningSpeed *= fastSpeedMultiplier; // 반경 내 속도 증가
                }

                // 손전등 방향으로 쓰레기 이동
                Vector3 directionToFlashlight = (flashLight.transform.position - trash.transform.position).normalized;
                trash.transform.position += directionToFlashlight * adjustedCleaningSpeed * Time.deltaTime;

                // 손전등과의 거리 확인
                float distanceToFlashlight = Vector3.Distance(trash.transform.position, flashLight.transform.position);

                //쓰레기일 경우
                if (distanceToFlashlight < 1.5f && !trash.name.Contains("Oil") && trash.GetComponent<Trash>().isConsume == false) // 가까운 경우
                {
                    trash.GetComponent<Trash>().isConsume = true;
                    currentTrashcanAmout += 0.3;
                    if (currentTrashcanAmout < PlayerInfo.instance.maxTrashcanCapacity)
                    {
                        //// 쓰레기 렌더러 및 콜라이더 비활성화
                        //Renderer trashRenderer = trash.transform.GetChild(0).GetComponent<Renderer>();
                        //Collider trashCollider = trash.GetComponent<Collider>();
                        //if (trashRenderer != null) trashRenderer.enabled = false;
                        //if (trashCollider != null) trashCollider.enabled = false;

                        TrashSpawnerTotalManager trashSpawnerTotalManager = FindObjectOfType<TrashSpawnerTotalManager>();

                        //쓰레기 흡수하는 사운드
                        audioSet.OBJSFXPlayRandom(3, 4, true);

                        Debug.Log($"쓰레기 획득! 현재 용량: {currentTrashcanAmout / PlayerInfo.instance.maxTrashcanCapacity}");
                        // 쓰레기 용량 %로 표시(UI)
                        trashcanCapacity_UI.text = Mathf.FloorToInt((float)currentTrashcanAmout / PlayerInfo.instance.maxTrashcanCapacity * 100).ToString() + "%";

                        GameObject Canvas_TrashCan = GameObject.Find("Canvas_TrashCan").gameObject;
                        Trashcan_UI trashcan_UI = Canvas_TrashCan.GetComponent<Trashcan_UI>();
                        trashcan_UI.AddTrashImage(trash.GetComponent<Trash>().trashImage, trash.GetComponent<Trash>().trashName, trash.GetComponent<Trash>().trashPoint, "trash");
                    
                        pv.RPC(nameof(Trash.disableRenderer), RpcTarget.All);
                    }
                    else
                    {
                        Debug.Log("쓰레기용량이 꽉 찼습니다.");
                    }
                }

                //기름 덩어리일 경우
                if (distanceToFlashlight < 1.5f && trash.CompareTag("Oilbolb"))
                {
                    
                    // 기름덩어리를 청소기 쪽으로 이동
                    Vector3 directionToVacuum = (transform.position - trash.transform.position).normalized;
                    trash.transform.position += directionToVacuum * PlayerInfo.instance.cleaningSpeed * Time.deltaTime;

                    // 청소기와의 거리 확인
                    float distanceToVacuum = Vector3.Distance(trash.transform.position, transform.position);
                    if (distanceToVacuum < 20f) // 청소기에 가까운 경우
                    {
                        // UI 업데이트
                        GameObject Canvas_TrashCan = GameObject.Find("Canvas_TrashCan").gameObject;
                        Trashcan_UI trashcan_UI = Canvas_TrashCan.GetComponent<Trashcan_UI>();
                        trashcan_UI.AddTrashImage(trash.GetComponent<Trash>().trashImage, trash.GetComponent<Trash>().trashName, trash.GetComponent<Trash>().trashPoint, "oil");
                        pv.RPC(nameof(Trash.disableRenderer), RpcTarget.All);
                        
                        //기름 흡수하는 사운드
                        audioSet.OBJSFXPlayRandom(5,6,true);

                        Debug.Log($"{trash.name} 기름덩어리를 청소했습니다.");
                       
                        pv.RPC(nameof(Trash.disableRenderer), RpcTarget.All);

                        // 리스트에서 제거
                        previousOutLineObjects.RemoveAt(i);
                        i--; // 루프 인덱스 조정
                    }
                }

                //퀘스트용이거나, 물고기 주민에게 붙은 기름 덩어리일 경우
                if (distanceToFlashlight < 3f && trash.CompareTag("Oilbolb_Quest"))
                {
                    // 기름덩어리를 청소기 쪽으로 이동
                    Vector3 directionToVacuum = (transform.position - trash.transform.position).normalized;
                    trash.transform.position += directionToVacuum * PlayerInfo.instance.cleaningSpeed * Time.deltaTime;

                    // 청소기와의 거리 확인
                    float distanceToVacuum = Vector3.Distance(trash.transform.position, transform.position);
                    if (distanceToVacuum < 2f) // 청소기에 가까운 경우
                    {
                        // UI 업데이트
                        GameObject Canvas_TrashCan = GameObject.Find("Canvas_TrashCan").gameObject;
                        Trashcan_UI trashcan_UI = Canvas_TrashCan.GetComponent<Trashcan_UI>();
                        trashcan_UI.AddTrashImage(trash.GetComponent<Trash>().trashImage, trash.GetComponent<Trash>().trashName, trash.GetComponent<Trash>().trashPoint, "oil");

                        // 기름덩어리 삭제
                        pv.RPC(nameof(Trash.disableRenderer), RpcTarget.All);

                        print("기름 덩어리 삭제됨");

                        //기름 흡수하는 사운드
                        audioSet.OBJSFXPlayRandom(5, 6, true);

                        Debug.Log($"{trash.name} 기름덩어리를 청소했습니다.");


                        // 리스트에서 제거
                        previousOutLineObjects.RemoveAt(i);
                        i--; // 루프 인덱스 조정
                    }
                }
            }
        }
    }





    //기름덩어리 다시 생성 코루틴 실행 rpc함수
    [PunRPC]
    public void RespawnOilBlob(Vector3 position, Vector3 scale, float delay)
    {
        
        //StartCoroutine(RespawnOilBlobCorutine(position,scale,delay));
        print("기름덩어리 다시 생성함");
    }

    //// 기름덩어리를 다시 생성하는 코루틴
    //private IEnumerator RespawnOilBlobCorutine(Vector3 position, Vector3 scale, float delay)
    //{
    //    yield return new WaitForSeconds(delay); // 지정된 시간 기다림
    //    print("기름덩어리 다시 생성했는지 확인");

    //    //사라진 자리에 랜덤으로 새로운 기름덩어리를 다시 생성
    //    int randomIndex = UnityEngine.Random.Range(0, oilListName.Count);
    //    string selceted = oilListName[randomIndex];
    //    GameObject selectedTrashPrefab = oilPrefabs[randomIndex];
    //    GameObject newOilBlob = PhotonNetwork.Instantiate("Trash/" + selceted, position, Quaternion.identity); // 저장된 위치에 새로운 기름덩어리 생성
    //    newOilBlob.transform.localScale = scale; // 저장된 크기로 크기 설정
    //}

    private void OnDrawGizmos()
    {
        if (flashLight != null && PlayerInfo.instance != null && PlayerInfo.instance.detectionDistance > 0)
        {
            // 원뿔 기즈모를 그림
            Gizmos.color = Color.yellow;

            // 원뿔의 각도에 맞춰 주변 점들 생성
            Vector3 leftEdge = Quaternion.Euler(0, -coneAngle, 0) * flashLight.transform.forward * PlayerInfo.instance.detectionDistance;
            Vector3 rightEdge = Quaternion.Euler(0, coneAngle, 0) * flashLight.transform.forward * PlayerInfo.instance.detectionDistance;
            Vector3 topEdge = Quaternion.Euler(-coneAngle, 0, 0) * flashLight.transform.forward * PlayerInfo.instance.detectionDistance;
            Vector3 bottomEdge = Quaternion.Euler(coneAngle, 0, 0) * flashLight.transform.forward * PlayerInfo.instance.detectionDistance;

            // 기즈모로 원뿔의 경계를 그리기
            Gizmos.DrawLine(flashLight.transform.position, flashLight.transform.position + leftEdge);
            Gizmos.DrawLine(flashLight.transform.position, flashLight.transform.position + rightEdge);
            Gizmos.DrawLine(flashLight.transform.position, flashLight.transform.position + topEdge);
            Gizmos.DrawLine(flashLight.transform.position, flashLight.transform.position + bottomEdge);
        }
    }

}
////기름 덩어리 위치를 저장할 리스트
//List<Vector3> oilBlobPositions = new List<Vector3>();

//// 기름덩어리 없애는 함수
//public void CleaningOilbolb()
//{
//    // 기름덩어리 감지 범위를 설정합니다.
//    RaycastHit[] hits = Physics.SphereCastAll(flashLight.transform.position, 3, flashLight.transform.forward, PlayerInfo.instance.detectionDistance);
//    List<GameObject> detectedOilblobs = new List<GameObject>(); // 감지된 기름덩어리 목록

//    // 레이를 맞은 기름덩어리의 아웃라인을 활성화
//    for (int i = 0; i < hits.Length; i++)
//    {
//        GameObject obj = hits[i].collider.gameObject;

//        // 감지된 오브젝트가 기름덩어리인지 확인
//        if (obj.CompareTag("Oilbolb"))
//        {
//            detectedOilblobs.Add(obj); // 감지된 기름덩어리 추가

//            // 기름덩어리 오브젝트의 아웃라인 활성화해주기
//            Outline outline = obj.GetComponent<Outline>();
//            if (outline != null)
//            {
//                outline.OutlineColor = Color.red;
//                outline.OutlineWidth = 5.0f;
//            }

//            // 기름덩어리 위치 저장(중복 저장 방지)
//            if (!oilBlobPositions.Contains(obj.transform.position))
//            {
//                oilBlobPositions.Add(obj.transform.position);
//            }
//        }
//        //물고기들에게 묻은 기름덩어리일 때
//        else if (interection_WhaleVacumm.cleanStart && obj.CompareTag("Oilbolb_Quest"))
//        {
//            detectedOilblobs.Add(obj); // 감지된 기름덩어리 추가

//            // 기름덩어리 오브젝트의 아웃라인 활성화해주기
//            Outline outline = obj.GetComponent<Outline>();
//            if (outline != null)
//            {
//                outline.OutlineColor = Color.red;
//                outline.OutlineWidth = 5.0f;
//            }

//            // 기름덩어리 위치 저장(중복 저장 방지)
//            if (!oilBlobPositions.Contains(obj.transform.position))
//            {
//                oilBlobPositions.Add(obj.transform.position);
//            }
//        }

//    }

//// CleaningOilbolb 함수 내
//if (Input.GetMouseButton(0))
//{
//    for (int i = 0; i < detectedOilblobs.Count; i++)
//    {
//        GameObject obj = detectedOilblobs[i];

//        // 기름덩어리를 청소기 쪽으로 이동
//        Vector3 directionToVacuum = (transform.position - obj.transform.position).normalized;
//        obj.transform.position += directionToVacuum * PlayerInfo.instance.cleaningSpeed * Time.deltaTime;

//        // 청소기와의 거리 확인
//        float distanceToVacuum = Vector3.Distance(obj.transform.position, transform.position);
//        if (distanceToVacuum < 0.5f) // 청소기에 가까운 경우
//        {
//            Destroy(obj); // 기름덩어리 삭제
//            //쓰레기통 UI에 기름 덩어리 이미지 생성하고 서서히 희미해지면서 사라지게 하기
//            GameObject Canvas_TrashCan = GameObject.Find("Canvas_TrashCan").gameObject;
//            Trashcan_UI trashcan_UI = Canvas_TrashCan.GetComponent<Trashcan_UI>();
//            trashcan_UI.AddTrashImage(obj.GetComponent<Trash>().trashImage, obj.GetComponent<Trash>().trashName, obj.GetComponent<Trash>().trashPoint, "oil");

//            Debug.Log($"{obj.name} 기름덩어리를 청소했습니다.");

//            // 만약 Oilbolb라면, 몇 초 뒤 동일한 위치에 다시 생성
//            if (obj.CompareTag("Oilbolb"))
//            {
//                StartCoroutine(RespawnOilBlob(obj.transform.position, 5.0f));
//            }

//            detectedOilblobs.RemoveAt(i); // detectedOilblobs에서 제거
//            i--; // 루프 인덱스 조정
//        }
//    }
//}

//    // 감지된 기름덩어리 목록이 아닌 오브젝트의 아웃라인 비활성화
//    GameObject[] allOilBlobs = GameObject.FindGameObjectsWithTag("Oilbolb");
//    for (int i = 0; i < allOilBlobs.Length; i++)
//    {
//        GameObject oilBlob = allOilBlobs[i];
//        if (!detectedOilblobs.Contains(oilBlob)) // 현재 감지된 목록에 없는 경우
//        {
//            Outline outline = oilBlob.GetComponent<Outline>();
//            if (outline != null)
//            {
//                outline.OutlineColor = Color.clear; // 아웃라인 색상을 투명으로 설정
//                outline.OutlineWidth = 0.0f; // 아웃라인 폭을 0으로 설정
//            }
//        }
//    }
//}




