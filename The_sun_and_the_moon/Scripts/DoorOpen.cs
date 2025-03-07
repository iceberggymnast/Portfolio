using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DoorOpen : MonoBehaviour
{
    // 인터렉션 시 문을 열고 닫을 수 있게 함

    // 움직이게 하려는 문들
    public List<Transform> doorList;

    // 기존 값 저장용
    public List<Vector3> orginTransPos;
    public List<Vector3> orginTransRot;

    // 위치값 Offset
    public List<Vector3> posVec;

    // 로테이선 값 오프셋
    public List<Vector3> rotVec;

    // 열림 닫힘 설정
    public bool isOpen;

    // 문의 속도
    public float openSpeed = 20.0f;

    // 포톤 뷰
    PhotonView photonView;

    // 문 잠금
    public bool locked;

    // 아웃라인 컴포넌트
    public Outline outline;


    void Start()
    {
        // 기존 값들을 저장해준다.
        for (int i = 0; i < doorList.Count; i++)
        {
            orginTransPos.Add(doorList[i].position);
            orginTransRot.Add(doorList[i].eulerAngles);
        }

        photonView = GetComponent<PhotonView>();
        outline = GetComponent<Outline>();
    }

    void Update()
    {
        if (!locked)
        { 
            DoorInteraction();
        }

        // 열림 여부에 따라 값을 설정해준다.
        if (isOpen)
        {
            for (int i = 0; i < doorList.Count; i++)
            {
                doorList[i].position = Vector3.Lerp(doorList[i].position, orginTransPos[i] + posVec[i], Time.deltaTime * openSpeed);
                doorList[i].rotation = Quaternion.Lerp(doorList[i].rotation, Quaternion.Euler(orginTransRot[i] + rotVec[i]), Time.deltaTime * openSpeed);
            }
        }
        else
        {
            for (int i = 0; i < doorList.Count; i++)
            {
                doorList[i].position = Vector3.Lerp(doorList[i].position, orginTransPos[i], Time.deltaTime * openSpeed);
                doorList[i].rotation = Quaternion.Lerp(doorList[i].rotation, Quaternion.Euler(orginTransRot[i]), Time.deltaTime * openSpeed);
            }
        }
    }

    void DoorInteraction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.SphereCast(Camera.main.transform.position, 0.5f, Camera.main.transform.forward, out hit, 10.0f, (1 << 11)))
            {
                print("문 클릭 됨");
                //for (int i = 0; i < doorList.Count; i++)
                //{
                    DoorOpen myDoor = GetComponent<DoorOpen>();
                    DoorOpen Doordetect = hit.collider.gameObject.GetComponent<DoorOpen>();
                    print("문 클릭 체크");
                    if (myDoor.gameObject == Doordetect.gameObject)
                    {
                        print("문 클릭 열기 작동");
                        photonView.RPC(nameof(DoorSetting), RpcTarget.All);
                    }
                //}
            }
        }
    }

    [PunRPC]
    void DoorSetting()
    {
        isOpen = !isOpen;
    }

    public void LockDoor(bool islockdoor)
    {
        if(islockdoor)
        {
            locked = islockdoor;
            if (outline == null) return;
            outline.OutlineColor = Color.red;
            outline.UpdateMaterialProperties();
        }
        else
        {
            locked = islockdoor;
            if (outline == null) return;
            outline.OutlineColor = Color.green;
            outline.UpdateMaterialProperties();
        }
    }
}
