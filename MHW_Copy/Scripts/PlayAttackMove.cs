using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayAttackMove : MonoBehaviour
{
    Animator controll;
    GameObject playerModel;
    public GameObject sword;
    public GameObject swordUn;

    void Start()
    {
    playerModel = GameObject.Find("test2");
    controll = playerModel.GetComponent<Animator>();
    }

    void Update()
    {

        // 마우스 클릭을 할 경우 무기를 발도 함 (키도 트리거 함)
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            controll.SetBool("isWpUse", true);
            sword.gameObject.SetActive(true);
            swordUn.gameObject.SetActive(false);

        }
        
        // 발도중 일때 shift 키를 누르면 납도함
        if (controll.GetBool("isWpUse"))
        {
            if (controll.GetBool("isPutshift"))
            {
                controll.SetBool("isWpUse", false);
            
            }
        }

    }
}


