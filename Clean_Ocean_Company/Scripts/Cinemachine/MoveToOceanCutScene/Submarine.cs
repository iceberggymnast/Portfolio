using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour
{
    public float moveSpeed = 3f; // 잠수정의 이동 속도

    void Update()
    {
        // 잠수정을 앞으로 계속 이동
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
