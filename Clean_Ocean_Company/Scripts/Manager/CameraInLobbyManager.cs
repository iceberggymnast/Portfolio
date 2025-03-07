using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInLobbyManager : MonoBehaviour
{
    [SerializeField] Transform cameraPos;  // Inspector에서 할당
    [SerializeField] Camera mainCamera; // 메인 카메라
    [SerializeField] Camera uiCamera; // 상호작용 UI 용 카메라
    [SerializeField] float lerpSpeed = 5f; // 보간 속도

    private void Awake()
    {
        mainCamera = transform.GetChild(0).GetComponent<Camera>();
        uiCamera = transform.GetChild(1).GetComponent<Camera>();
    }

    private void Start()
    {
        if (cameraPos == null)
        {
            cameraPos = GameObject.Find("CameraPos").transform; // 필요할 경우에만 찾음
        }
    }
    private void FixedUpdate()
    {
        Vector3 dir = Vector3.Lerp(transform.position, cameraPos.position, lerpSpeed * Time.deltaTime);
        transform.position = dir;
        mainCamera.transform.position = transform.position;
        uiCamera.transform.position = transform.position;
    }

}
