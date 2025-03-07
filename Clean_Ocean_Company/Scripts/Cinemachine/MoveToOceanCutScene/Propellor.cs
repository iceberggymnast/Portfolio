using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propellor : MonoBehaviour
{
    float rotationSpeed = 300f; // 프로펠러 회전 속도 (초당 각도)

    void Update()
    {
        // z축을 기준으로 프로펠러를 회전
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
