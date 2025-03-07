using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBtnSwitch : MonoBehaviour
{
    // 탭 기능 같은걸 만들때 캔버스 같은걸 스위치 시켜주는 기능입니다.
    // 함수는 버튼 온 클릭에 할당해서 사용합니다.

    public List<GameObject> switchOBJ;

    public Action act;

    private void Start()
    {
        Btn_Switch(0);
    }

    public void Btn_Switch(int index)
    {
        for (int i = 0; i < switchOBJ.Count; i++)
        {
            switchOBJ[i].SetActive(false);
        }

        switchOBJ[index].SetActive(true);

        if (act != null) act();
    }


}
