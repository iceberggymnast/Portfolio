using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIInventoryInfo : MonoBehaviour
{

    // 보관함 인벤토리 정보를 나타낼 것이다.

    // 저장할 인벤토리 데이터 하나
    public InventoryCupon data;

    // 텍스트 
    public TMP_Text cuponName;
    public TMP_Text timeStamp;
    public TMP_Text place;
    public TMP_Text number;

    // 스프라이트
    public Image icon;


    public void SetInventoryInfo(InventoryCupon input)
    {
        data = input;
        if (data.cupon != null)
        {
            cuponName.text = data.cupon.cuponName;
            place.text = data.cupon.exchangeLink;
            icon.sprite = data.cupon.icon;
        }
        else if (data.souvenir != null) 
        {
            cuponName.text = data.souvenir.souvenirName;
            place.text = data.souvenir.exchangeLink;
            icon.sprite = data .souvenir.icon;
        }
            timeStamp.text = data.rewardData.publishedDate + " ~ " + data.rewardData.expireDate;
            number.text = data.rewardData.bacode;
    }

}
