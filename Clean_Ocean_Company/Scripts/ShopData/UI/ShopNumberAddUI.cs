using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopNumberAddUI : MonoBehaviour
{
    // 구매 갯수를 화살표 버튼을 눌러 설정할 수 있도록 할당하는 버튼 기능입니다.

    public TMP_InputField inputField;
    public ShopBuyInfo shopBuyInfo;

    public void AddNum (int num)
    {
        int currentnum = int.Parse(inputField.text);
        currentnum += num;

        if ( currentnum <= 0 )
        {
            currentnum = 1;
        }

        inputField.text = currentnum.ToString();

        shopBuyInfo.SetNumber();
    }
}
