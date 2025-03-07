using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    // 리스트에 있는 버튼 프리팹의 값을 적용시켜주는 컴포넌트 입니다.

    // Data
    public Interaction_Shop shopData;

    // 상점 UI 버튼
    public GameObject[] shop_item_btn;

    // 인벤 UI 버튼
    public GameObject[] inventory_item_btn;

    public TMP_Text[] item_info_text;
    public Sprite item_Info_Sprite;

    // Page..
    int page = 1;

    void Start()
    {
        // 컴포넌트를 찾고 해당 값을 기준으로 값을 새로고침 해줌
        if (shopData == null)
        {
            shopData = GameObject.FindObjectOfType<Interaction_Shop>();
        }
        PageRefresh(1);

        ShopBtnSwitch switchs = GetComponent<ShopBtnSwitch>();
        switchs.act = PageRefresh;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void backbtn()
    {
        PlayerInfo.instance.isCusor = false;

        gameObject.SetActive(false);
    }

    public void PageRefresh(int index)
    {
        if (shop_item_btn[0].activeSelf)
        {
            // 0, 1, 2, 3, 4 -> 5, 6, 7, 8, 9... 이런 순서로 표시
            for (int i = 0; i < shop_item_btn.Length; i++)
            {
                ShopUIListButton btnCS = shop_item_btn[i].GetComponent<ShopUIListButton>();
                int c = ((index - 1) * shop_item_btn.Length) + i;
                if (((index - 1) * shop_item_btn.Length) + i < shopData.shops.Count)
                {
                    btnCS.Refresh(shopData.shops[((index - 1) * shop_item_btn.Length) + i]);
                }
                else
                {
                    btnCS.Refresh();
                }
            }
        }

        if (inventory_item_btn[0].activeSelf)
        {
            for (int i = 0; i < inventory_item_btn.Length; i++)
            {
                ShopUIListButton btnCS = inventory_item_btn[i].GetComponent<ShopUIListButton>();
                int para = ((index - 1) * shop_item_btn.Length) + i;
                if (para < shopData.inventoryCupons.Count)
                {
                    btnCS.InventoryDisplay(shopData.inventoryCupons[para]);
                }
                else
                {
                    btnCS.Refresh();
                }
            }
        }

    }
    
    public void PageRefresh()
    {
        if (shop_item_btn[0].activeSelf)
        { 
            // 0, 1, 2, 3, 4 -> 5, 6, 7, 8, 9... 이런 순서로 표시
            for (int i = 0; i < shop_item_btn.Length; i++)
            {
                ShopUIListButton btnCS = shop_item_btn[i].GetComponent<ShopUIListButton>();
                btnCS.Refresh(shopData.shops[((0) * shop_item_btn.Length) + i]);
            }
        }

        if (inventory_item_btn[0].activeSelf)
        {
            for (int i = 0; i < inventory_item_btn.Length; i++)
            {
                ShopUIListButton btnCS = inventory_item_btn[i].GetComponent<ShopUIListButton>();
                if (shopData.inventoryCupons.Count > 0)
                {
                    int para = ((0) * shop_item_btn.Length) + i;
                    if (shopData.inventoryCupons.Count > para)
                    {
                        btnCS.InventoryDisplay(shopData.inventoryCupons[para]);
                    }
                }
            }
        }

    }

    public void PageIndexSet(int index)
    {
        page = index;
        PageRefresh(page);
        SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
        soundManger.UISFXPlayRandom(8, 10);
    }

}
