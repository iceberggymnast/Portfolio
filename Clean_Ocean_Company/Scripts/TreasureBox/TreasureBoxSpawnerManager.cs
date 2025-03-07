using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TreasureBoxSpawnerManager : MonoBehaviour
{
    //보급상자 프리팹
    public GameObject treasureBoxPrefab;

    //생성되는 y축은 해수면 높이로 고정
    public float spawn_Ypos = 20.0f;

    //천장에 붙은 플레인들
    public GameObject[] planeObjects;

    //보급상자 생성 시간 간격
    public float TreasureBoxMadeTime = 1.0f;

    //최대 보급상자 개수
    int maxTreasureBoxes = 3;

    //현재 생성된 보급상자 개수
    int currentTreasureBoxCount;

    void Start()
    {
        // 보급상자 생성 루틴 시작
        StartCoroutine(SpawnTerasureBoxRoutine());

        
    }

    private void Update()
    {
       
    }

    IEnumerator SpawnTerasureBoxRoutine()
    {
        while (true)
        {
            // 보급상자 개수가 최대치에 도달하지 않은 경우에만 생성
            if (currentTreasureBoxCount < maxTreasureBoxes)
            {
                SpawnRandomTreasureBox();
                //print("currentTreasureBoxCount는 " + currentTreasureBoxCount);
            }
            else
            {
                //Debug.Log("최대 보물상자 개수에 도달했습니다.");
            }

            yield return new WaitForSeconds(TreasureBoxMadeTime);
        }
    }

    void SpawnRandomTreasureBox()
    {
        // 랜덤한 플레인 선택
        GameObject selectedPlane = planeObjects[Random.Range(0, planeObjects.Length)];

        // 선택한 플레인의 크기 가져오기
        Vector3 planeSize = selectedPlane.GetComponent<MeshRenderer>().bounds.size;

        // 플레인 위의 랜덤 위치 설정
        float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2) + selectedPlane.transform.position.x;
        float randomZ = Random.Range(-planeSize.z / 2, planeSize.z / 2) + selectedPlane.transform.position.z;
        Vector3 spawnPosition = new Vector3(randomX, spawn_Ypos, randomZ);

        // 랜덤한 보급상자 프리팹 선택 후 생성
        GameObject treasureInstance = PhotonNetwork.Instantiate("TreasureBox(FinalTreasureBox)", spawnPosition, Quaternion.identity);

        // 보급상자 객체에 Rigidbody 추가하여 아래로 떨어지게 함
        Rigidbody rb = treasureInstance.GetComponent<Rigidbody>();
        rb.useGravity = true;

        // 보급상자 생성 시 카운트 증가
        currentTreasureBoxCount++;
    }
}
