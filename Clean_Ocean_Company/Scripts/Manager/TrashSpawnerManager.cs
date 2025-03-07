using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawnerBase : MonoBehaviour
{
    // 여러 종류의 쓰레기 오브젝트 프리팹
    public List<GameObject> trashPrefabs;
    public List<string> trashListName = new List<string>() { "clothes_Mask", "glass_brokenGlassPiece", "glass_glassBottle", "plastic_bag", "plastic_petBottle", "plastic_straw", "silver_sodaCan", "silver_tomatoCan" };

    // 생성되는 y축은 해수면 높이로 고정
    public float spawn_Ypos = 20.0f;

    // 천장에 붙은 플레인 크기
    public GameObject planeObject;
    Vector3 planeSize;

    // 쓰레기 생성 시간 간격
    public float trashMadeTime = 1.0f;

    // 최대 및 최소 Y 값 (쓰레기가 떨어질 끝 지점)
    public float minFallY = 0.5f; // 최소 Y값
    public float maxFallY = 5.0f; // 최대 Y값

    //지역별 최대 생성할 쓰레기 양
    public int maxTrashAmout = 300;

    //총 쓰레기 양 관리
    TrashSpawnerTotalManager trashSpawnerTotalManager;

    void Start()
    {
        // 플레인의 크기 가져오기
        planeSize = planeObject.GetComponent<MeshRenderer>().bounds.size;

        // TrashSpawnerTotalManager 참조 가져오기
        trashSpawnerTotalManager = FindObjectOfType<TrashSpawnerTotalManager>();

        // 쓰레기 생성 루틴 시작
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SpawnTrashRoutine());

        }
    }

    IEnumerator SpawnTrashRoutine()
    {
        while (true)
        {
            //현재 맵의 쓰레기양이 핟당한 쓰레기보다 적게 있을 때
            if (trashSpawnerTotalManager.currentTrashAmout < trashSpawnerTotalManager.totalTrashAmount)
            {
                // 쓰레기 생성
                SpawnRandomTrash();

                trashSpawnerTotalManager.currentTrashAmout++;

                // 다음 쓰레기 생성까지 대기
                yield return new WaitForSeconds(trashMadeTime);
            }
            //현재 맵의 쓰레기양이 할당한 쓰레기보다 같거나 많이 있을 때
            else
            {
                yield return null;
            }
            
        }
    }

    void SpawnRandomTrash()
    {
        // 플레인 위의 랜덤 위치 설정
        float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2) + planeObject.transform.position.x;
        float randomZ = Random.Range(-planeSize.z / 2, planeSize.z / 2) + planeObject.transform.position.z;
        Vector3 spawnPosition = new Vector3(randomX, spawn_Ypos, randomZ);

        // 랜덤한 쓰레기 프리팹 선택 후 생성
        int randomIndex = Random.Range(0, trashListName.Count);
        string selceted = trashListName[randomIndex];
        GameObject selectedTrashPrefab = trashPrefabs[randomIndex];
        GameObject trashInstance = PhotonNetwork.Instantiate("Trash/Trash3DPrefabs/" + selceted, spawnPosition, Quaternion.identity);

        if(trashInstance != null)
        {
        // 쓰레기 객체에 Rigidbody 추가하여 아래로 떨어지게 함
        Rigidbody rb = trashInstance.GetComponent<Rigidbody>();

        rb.useGravity = true;

        // 끝 지점 Y값을 랜덤으로 설정
        float randomEndY = Random.Range(minFallY, maxFallY);
        StartCoroutine(DisableGravityAtY(trashInstance, randomEndY));

        }
    }

    IEnumerator DisableGravityAtY(GameObject trashInstance, float endY)
    {
        Rigidbody rb = trashInstance != null ? trashInstance.GetComponent<Rigidbody>() : null;

        // 쓰레기가 끝 Y 위치에 도달할 때까지 대기
        while (trashInstance != null && rb != null)
        {
            // Y 위치가 endY 이하로 떨어졌는지 확인
            if (trashInstance.transform.position.y <= endY)
            {
                // Y 위치에 도달하면 중력 비활성화
                rb.useGravity = false;
                break; // 반복문 종료
            }

            yield return null; // 프레임 대기
        }
    }
}