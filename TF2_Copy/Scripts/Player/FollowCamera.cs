using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class FollowCamera : MonoBehaviour
{
    public enum CameraType
    {
        BasicType,
        RunType,
        FireType,
        SniperType
    }

    public CameraType cameraType;

    public Transform player;
    public GameObject grenadePos;

    public float followSpeed = 3.0f;

    bool dead;

    CameraController cameraController;
    Animator playerAnim;

    

    void Start()
    {
        player = GameObject.Find("Player").transform;
        grenadePos = GameObject.Find("GrenadeFirePos");
        cameraController = player.GetComponent<CameraController>();
        playerAnim = Camera.main.gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        

    }
    void LateUpdate()
    {

        if (cameraType == CameraType.BasicType || cameraType == CameraType.RunType)
        {
            // ī�޶��� ���� ������ Player ���� �������� �����Ѵ�.
            //transform.forward = player.forward;

            if (!dead)
            {
                // �÷��̾�� 1��Ī �������� �����Ѵ�.
                transform.position = cameraController.camList[0].position;
                transform.rotation = cameraController.camList[0].rotation;
                grenadePos.SetActive(true);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, cameraController.camList[1].position, Time.deltaTime * followSpeed);
                Vector3 lookangle = cameraController.camList[0].position - cameraController.camList[1].position;
                transform.forward = lookangle;
                grenadePos.SetActive(false);
            }
        }
        else if (cameraType != CameraType.RunType && !dead)
        {
            transform.rotation = cameraController.camList[0].rotation;
        }
        


    }


    public void checkDeathCam(bool death)
    {
        dead = death;
    }

}
