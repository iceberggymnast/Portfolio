using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawnerTotalManager : MonoBehaviour
{
    public GameObject TrashSpawner_Ocean;
    int amount1;
    public GameObject TrashSpawner_Ocean1;
    int amount2;
    public GameObject TrashSpawner_Ocean2;
    int amount3;

    //할당한 전체 쓰레기양 
    public float totalTrashAmount;  //수치 적는 칸 아님(다른 데서 당겨서 변수 쓰려고 pubic)

    //현재 쓰레기 양
    public float currentTrashAmout;  //수치 적는 칸 아님(다른 데서 당겨서 변수 쓰려고 pubic)

    //현재 바다방의 쓰레기 용량(해창님 지도 쓰레기 용량은 이걸로 쓰시면 됩니다!)
    int trashPercent;

    void Start()
    {
        //에디터 기준으로 maxTrashAmout 세아림
        amount1 = TrashSpawner_Ocean.GetComponent<TrashSpawnerBase>().maxTrashAmout;
        amount2 = TrashSpawner_Ocean1.GetComponent<TrashSpawnerBase>().maxTrashAmout;
        amount3 = TrashSpawner_Ocean2.GetComponent<TrashSpawnerBase>().maxTrashAmout;

        //세 구역 다 합친 최대 쓰레기양(할당한 전체 쓰레기양이 될거임)
        totalTrashAmount = amount1 + amount2 + amount3;

        // 처음에 쓰레기 양을 업데이트
        UpdateCurrentTrashAmount();
        // 초기 쓰레기 용량 비율 계산
        CalculateTrashPercent();
    }

    public float time;

    void Update()
    {
        time =+ Time.deltaTime;

        if (time >= 5)
        {
            // 현재 맵에 있는 쓰레기 양을 매 프레임마다 업데이트
            UpdateCurrentTrashAmount();
            CalculateTrashPercent();
            time = 0;
        }
    }

    void UpdateCurrentTrashAmount()
    {
        // 현재 맵에 있는 쓰레기 양 세기
        int trashLayer = LayerMask.NameToLayer("Trash");
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> trashObjects = new List<GameObject>();  //레이어가 Trash인 오브젝트를 담을 빈 리스트

        for(int i=0;i< allObjects.Length;i++)
        {
            // 게임 오브젝트가 "Trash" 레이어에 속하는지 확인
            if (allObjects[i].layer == trashLayer)
            {
                trashObjects.Add(allObjects[i]);
            }
        }

        // 현재 쓰레기 양 설정
        currentTrashAmout = trashObjects.Count;
        //print("currentTrashAmout의 수는: " + currentTrashAmout);
    }

    void CalculateTrashPercent()
    {
        // 쓰레기 용량 비율 계산 (0-100)
        if (totalTrashAmount > 0)
        {
            trashPercent = Mathf.RoundToInt(((float)currentTrashAmout / totalTrashAmount) * 100);
        }

        //print("현재방에 존재하는 쓰레기 용량 비율: " + trashPercent + "%");
    }
}
