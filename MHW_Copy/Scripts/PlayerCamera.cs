using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //플레이어 게임 오브젝트를 참조할 변수를 선언한다
    GameObject player;
    GameObject cameraTaget;
    float sensitivity = 1.5f; //마우스 감도
    RaycastHit hit;

    public bool cameralock;
    public Vector3 cameraoffset;
    void Start()
    {
        // 변수에 플레이어 게임 오브젝트를 참조한다.
        player = GameObject.Find("PlayerHunter");
        cameraTaget = GameObject.Find("CameraTaget");
    }

    void Update()
    {
        // 플레이어의 위치값을 받아온다.
        Vector3 playerPosiotion = player.transform.position;

        // 카메라의 기준이 되는 박스가 플레이어의 위치를 고정시킨다.
        cameraTaget.transform.position = player.transform.position + cameraoffset;

        // 마우스로 조작할 때 카메라의 각도가 플레이어 주위로 돌아간다.

        // 마우스의 감도를 받아온다.
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * -1 * sensitivity;

        // 카메라 타겟을 마우스 움직임에 따라 회전하나 Z 축은 그대로 유지해야함.
        if (!cameralock)
        {
            cameraTaget.transform.localRotation = cameraTaget.transform.localRotation * Quaternion.Euler(mouseY, mouseX, 0.0f);
        }

        // 카메라의 각도를 제한하려 한다.
        // 인스펙터 창에서 마이너스 x 값은 실제로 360값으로 나와서 적용해줘야 한다. -1도 = 359도 - 360도
        // 270도 ~ 360도 값 사이가 나오면 -360을 해준 값을 적용해준다.
        float lockX = cameraTaget.transform.rotation.eulerAngles.x;
        if (lockX > 270)
        {
            lockX = lockX - 360.0f;
        }
        lockX = Mathf.Clamp(lockX, -80.0f, 80.0f);

        // 카메라의 각도를 제한한다. X 축 -80 ~80도, Z 축은 0
        cameraTaget.transform.rotation = Quaternion.Euler(lockX, transform.eulerAngles.y, 0);

        // 벽에 부딛하면 카메라가 밀려나야 함
        // hit한 postion 값을 받아오고, 카메라의 postion 값을 받아와서 거리를 계산
        RaycastHit hit;
        Vector3 distance;

        if (Physics.Raycast(transform.position, transform.forward * -1, out hit, 10) || Physics.Raycast(transform.position, transform.right * -1, out hit, 10) || Physics.Raycast(transform.position, transform.right, out hit, 10))
        {
            distance = transform.position - hit.transform.position;
            //print(distance.magnitude);
            if (distance.magnitude < 3)
            {
                transform.localPosition += new Vector3(0, 0, 1);
            }
            else if (distance.magnitude > 5 && transform.localPosition.z > -50)
            {
                transform.localPosition += new Vector3(0, 0, -1);
            }
        }
        else if (transform.localPosition.z > -50)
        {
            transform.localPosition += new Vector3(0, 0, -1);
        }

    }
}
