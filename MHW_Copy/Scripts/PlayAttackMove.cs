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

        // ���콺 Ŭ���� �� ��� ���⸦ �ߵ� �� (Ű�� Ʈ���� ��)
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            controll.SetBool("isWpUse", true);
            sword.gameObject.SetActive(true);
            swordUn.gameObject.SetActive(false);

        }
        
        // �ߵ��� �϶� shift Ű�� ������ ������
        if (controll.GetBool("isWpUse"))
        {
            if (controll.GetBool("isPutshift"))
            {
                controll.SetBool("isWpUse", false);
            
            }
        }

    }
}


