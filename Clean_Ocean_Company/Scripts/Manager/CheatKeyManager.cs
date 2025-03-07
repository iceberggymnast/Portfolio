using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatKeyManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        cheatkey_Point();
        cheaatkey_O2charger();
    }

    public void cheatkey_Point()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerInfo.instance.PointPlusOrMinus(100);
            print("포인트 100 획득");
        }
    }

    public void cheaatkey_O2charger()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerInfo.instance.current_OxygenchargerAmount += 3;
            print("산소 충전기 3개 추가");

        }
    }
}
