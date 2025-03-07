using System.Collections;
using UnityEngine;

public class HandsSpawner : MonoBehaviour
{
    [SerializeField]
    private HandsFSM[] hands; // Map에 있는 Hands 오브젝트
    [SerializeField]
    private float spawnTime; // Hands 등장 주기

    // Hands 등장 확률(Normal : 80[%], Red : 20[%])
    private int[] spawnPercents = new int[2] { 80, 20 };

    public void Start()
    {
        StartCoroutine("SpawnHands");
    }

    IEnumerator SpawnHands()
    {
        while (true)
        {
            // 0 ~ Tigers.Length - 1 중 임의의 숫자 선택
            int index = Random.Range(0, hands.Length);

            // 선택된 Hands의 속성 설정
            hands[index].HandType = SpawnHandType();

            // index안에 호랑이 상태를 MoveUp으로 변경
            hands[index].ChangeState(HandsState.MoveUp);

            // spawnTime 시간동안 대기
            yield return new WaitForSeconds(spawnTime);
        }
    }
    HandType SpawnHandType()
    {
        int percent = Random.Range(0, 100);
        float cumulative = 0;

        for(int i = 0; i < spawnPercents.Length; ++i)
        {
            cumulative += spawnPercents[i];

            if(percent < cumulative)
            {
                return (HandType)i;
            }
        }
        return HandType.Normal;
    }
}



