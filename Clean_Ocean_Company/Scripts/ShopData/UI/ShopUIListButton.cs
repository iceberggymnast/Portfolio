using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIListButton : MonoBehaviour
{
    // 상점, 인벤토리 페이지 리스트의 버튼 오브젝트에 들어가는 컴포넌트 입니다

    // 표시 할 데이터가 들어갈 곳

    // 상점일 경우
    public ShopData data;

    // 인벤일 경우
    public InventoryCupon inventoryData;

    // 프리팹 자식들
    public TMP_Text itemName;
    public TMP_Text itemPrice;
    public Image itemIcon;

    // 구매 정보 스크립트
    public ShopBuyInfo info;
    public ShopUIInventoryInfo invenInfo;
    public ShopUIResult resultUI;

    // 내 버튼
    public Button btn;

    // 첫 팝업
    public GameObject startPopup;
    public GameObject shopInfo;

    // 투명 이미지
    public Sprite alphaSprite;

    void Start()
    {
        info = GameObject.FindFirstObjectByType<ShopBuyInfo>();
        invenInfo = GameObject.FindFirstObjectByType<ShopUIInventoryInfo>();
        btn = GetComponent<Button>();
        //Refresh();
    }

    // 상점 데이터를 매개변수로 받아서 내용을 적용시켜 준다.
    public void Refresh(ShopData shopData)
    {
        data = shopData;
        itemIcon.sprite = data.icon;
        btn.interactable = true;
        itemName.text = data.s_item_Name;
        itemPrice.text = data.s_item_Price.ToString();
    }

    // 매개변수가 없을 경우엔 데이터가 있는지 확인하고 적용시켜 준다.
    public void Refresh()
    {
        if (data != null)
        {
            itemName.text = data.s_item_Name;
            itemPrice.text = data.s_item_Price.ToString();
        }
        else
        {
            itemIcon.sprite = alphaSprite;
            itemName.text = "";
            itemPrice.text = "";
        }

        itemIcon.sprite = alphaSprite;
        btn.interactable = false;

        itemName.text = "";
        itemPrice.text = "";
    }

    // 버튼을 누르면 해당 상점 데이터를 Return 해준다.
    public void buttonClick()
    {
            if (startPopup.activeSelf)
            {
                startPopup.SetActive(false);
                shopInfo.SetActive(true);
            }
        if (data.s_item_Name != "")
        {

            info.InfoRefesrh(data);
        }
        else if (inventoryData != null)
        {
            invenInfo.SetInventoryInfo(inventoryData);
        }
    }

    public void InventoryDisplay(InventoryCupon data)
    {
        inventoryData = data;

        if (inventoryData != null)
        {
            if (inventoryData.cupon != null)
            {
                itemName.text = inventoryData.cupon.cuponName;
                itemIcon.sprite = inventoryData.cupon.icon;
            }
            else if (inventoryData.souvenir != null)
            {
                itemName.text = inventoryData.souvenir.souvenirName;
                itemIcon.sprite = inventoryData.souvenir.icon;
            }    
                itemPrice.text = "";
                btn.interactable = true;
        }
        else
        {
            itemIcon.sprite = alphaSprite;
            btn.interactable = false;
        }
    }


}
