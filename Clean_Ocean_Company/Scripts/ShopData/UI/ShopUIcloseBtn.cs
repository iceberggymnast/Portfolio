using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIcloseBtn : MonoBehaviour
{
    // 버튼을 누르면 해당 게임 오브젝트가 꺼집니다.

    // 닫을 게임오브젝트
    public GameObject closeOBJ;

    public void Btn_Close()
    {
        closeOBJ.SetActive(false);
    }
}

