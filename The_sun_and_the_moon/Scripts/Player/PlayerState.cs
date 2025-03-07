//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Playables;
using TMPro;
using Photon.Pun;

public class PlayerState : MonoBehaviourPun
{
    [Header("에디터 내에서 테스트해볼 경우 체크해 주세요")]
    public bool testMove;
    [Space (10.0f)]

    //UI가 켜졌는지 확인하는 bool 값
    public bool isOpenUI = false;
    //UI켜졌을 때 크로스헤어도 비활성화하자
    GameObject crossHair;
    //UI켜졌을 때 월드의 수집품이름 커서두면 뜨는 것도 비활성화하자 또는 투명하게 만들자
    OutlineAndCursor outlineAndCursor;
    TextMeshProUGUI Name_UI;

    //NPC 앞을 비출 빈 게임 오브젝트들
    //NPC마다 추가하기
    //public Transform npcTransform;

    public Transform targetPos;
    public bool isNPCtalking = false;

    GameObject myCamera;

    void Start()
    {
        myCamera = transform.GetChild(2).GetChild(0).gameObject;
    }

    void Update()
    {
        if(photonView.IsMine)
        {

            CursorState();

            //isNPCtalking값이 true이면
            if (isNPCtalking)
            {
                //targetPos로 이동하고
                myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, targetPos.position, Time.deltaTime);
                myCamera.transform.rotation = Quaternion.Lerp(myCamera.transform.rotation, targetPos.rotation, Time.deltaTime);
            }
            //아니면
            else
            {
                //로컬 transform의 값을 0,0,0로 이동한다.
                myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, Vector3.zero, Time.deltaTime);
                myCamera.transform.localRotation = Quaternion.Lerp(myCamera.transform.localRotation, Quaternion.identity, Time.deltaTime);
            }
        }
    }

    //커서를 잠구는 함수
    public void CursorState()
    {
        if(crossHair == null)
        {
            crossHair = GameObject.Find("crossHair");
        }

        if(Name_UI == null)
        {
            outlineAndCursor = FindObjectOfType<OutlineAndCursor>();
            //if (Name_UI == null) return;
            Name_UI = outlineAndCursor.Name_UI;
        }

        if (isOpenUI == false)
        {
            // 커서를 꺼준다
            Cursor.visible = false;
            
            Cursor.lockState = CursorLockMode.Locked;
            //크로스헤어 활성화 해준다.
            crossHair.SetActive(true);
            //커서둘 때 수집품 이름 뜨는 텍스트 UI도 활성화 해준다.
            Name_UI.gameObject.SetActive(true);


        }
        else if (isOpenUI == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            crossHair.SetActive(false);
            //커서둘 때 수집품 이름 뜨는 텍스트 UI도 비활성화 해준다.
            Name_UI.gameObject.SetActive(false);
        }
    }
}
