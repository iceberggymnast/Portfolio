using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TigerRope : MonoBehaviour
{
    public GameObject tigerRopePrefab;
    public Vector3 StartPoint;
    public Vector3 endPoint;
    public float fallSpeed;

    GameObject ropeIntance;

    //호랑이 로프 생성 및 이동 코루틴 실행하는 함수
    public void SpawnAndMoveRope()
    {
        print("111111111111111");
        //로프를 StartPoint에서 생성
        ropeIntance = Instantiate(tigerRopePrefab, StartPoint, Quaternion.identity);
        ropeIntance.SetActive(true);
        //로프 이동 코루틴 시작
        StartCoroutine(MoveRope());
    }


    // 로프가 목표 위치까지 이동하는 코루틴
    IEnumerator MoveRope()
    {
        while (ropeIntance.transform.position != endPoint)
        {
            // 목표 위치로 이동
            ropeIntance.transform.position = Vector3.MoveTowards(
                ropeIntance.transform.position,
                endPoint,
                fallSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    //호랑이 첫번째 로프 없어지는 함수
    public void FirstRopeRemove()
    {
        StartCoroutine(twoScond());
    }

    IEnumerator twoScond()
    {
        yield return new WaitForSeconds(1.66318f);
        ropeIntance.SetActive(false);
    }
}
