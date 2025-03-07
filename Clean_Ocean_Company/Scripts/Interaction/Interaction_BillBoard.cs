using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_BillBoard : MonoBehaviour
{
    public GameObject uiCamera;
    public bool isStart = false;

    public void SetUICamera(GameObject camera)
    {
        uiCamera = camera;
        isStart = true;
    }

    private void OnDisable()
    {
        isStart = false;
    }


    private void LateUpdate()
    {
        if (!isStart || uiCamera == null) return;
        transform.LookAt(transform.position + uiCamera.transform.rotation * Vector3.forward, uiCamera.transform.rotation * Vector3.up);
    }

    private void OnDestroy()
    {
        isStart = false;
    }
}
