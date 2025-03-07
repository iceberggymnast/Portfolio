using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public float speed = 5.0f; // 이동 속도
    private Vector3 targetPosition;

    void Update()
    {
        // 목표 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    // 목표 위치를 설정하는 메서드
    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
    }
}
