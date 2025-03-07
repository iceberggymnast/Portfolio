using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRot : MonoBehaviour
{
    // 회전 속도를 조절하는 변수
    public float rotationSpeed = 100f;

    void Update()
    {
        // Y축을 기준으로 rotationSpeed만큼 회전
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
