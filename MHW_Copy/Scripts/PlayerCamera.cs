using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //�÷��̾� ���� ������Ʈ�� ������ ������ �����Ѵ�
    GameObject player;
    GameObject cameraTaget;
    float sensitivity = 1.5f; //���콺 ����
    RaycastHit hit;

    public bool cameralock;
    public Vector3 cameraoffset;
    void Start()
    {
        // ������ �÷��̾� ���� ������Ʈ�� �����Ѵ�.
        player = GameObject.Find("PlayerHunter");
        cameraTaget = GameObject.Find("CameraTaget");
    }

    void Update()
    {
        // �÷��̾��� ��ġ���� �޾ƿ´�.
        Vector3 playerPosiotion = player.transform.position;

        // ī�޶��� ������ �Ǵ� �ڽ��� �÷��̾��� ��ġ�� ������Ų��.
        cameraTaget.transform.position = player.transform.position + cameraoffset;

        // ���콺�� ������ �� ī�޶��� ������ �÷��̾� ������ ���ư���.

        // ���콺�� ������ �޾ƿ´�.
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * -1 * sensitivity;

        // ī�޶� Ÿ���� ���콺 �����ӿ� ���� ȸ���ϳ� Z ���� �״�� �����ؾ���.
        if (!cameralock)
        {
            cameraTaget.transform.localRotation = cameraTaget.transform.localRotation * Quaternion.Euler(mouseY, mouseX, 0.0f);
        }

        // ī�޶��� ������ �����Ϸ� �Ѵ�.
        // �ν����� â���� ���̳ʽ� x ���� ������ 360������ ���ͼ� ��������� �Ѵ�. -1�� = 359�� - 360��
        // 270�� ~ 360�� �� ���̰� ������ -360�� ���� ���� �������ش�.
        float lockX = cameraTaget.transform.rotation.eulerAngles.x;
        if (lockX > 270)
        {
            lockX = lockX - 360.0f;
        }
        lockX = Mathf.Clamp(lockX, -80.0f, 80.0f);

        // ī�޶��� ������ �����Ѵ�. X �� -80 ~80��, Z ���� 0
        cameraTaget.transform.rotation = Quaternion.Euler(lockX, transform.eulerAngles.y, 0);

        // ���� �ε��ϸ� ī�޶� �з����� ��
        // hit�� postion ���� �޾ƿ���, ī�޶��� postion ���� �޾ƿͼ� �Ÿ��� ���
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
