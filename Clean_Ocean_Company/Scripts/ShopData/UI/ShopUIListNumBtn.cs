using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIListNumBtn : MonoBehaviour
{
    // 하단 번호 탭을 선택하여 리스트의 탭을 바꾸는 기능을 하는 컴포넌트 입니다.

    public ShopUI shopUI;

    public List<Button> btn_Arrow;
    public List<Button> btn_Number;

    int tabnum;
    int currentindex = 1;

    public Sprite selected;
    public Sprite NotSelected;

    public bool isinventory;

    private void Start()
    {
        BtnRangeSet();
    }

    void Update()
    {

    }

    void btnChecked()
    {
        if (currentindex == 1)
        {
            btn_Arrow[0].interactable = false;
        }
        else if (currentindex > 1)
        {
            btn_Arrow[0].interactable = true;
        }

        if (currentindex == tabnum)
        {
            btn_Arrow[1].interactable = false;
        }
        else
        {
            btn_Arrow[1].interactable = true;
        }

        for (int i = 0; i < currentindex + 1; i++)
        {
            if (i + 1 == currentindex)
            {
                Image sprite = btn_Number[i].GetComponent<Image>();
                sprite.sprite = selected;
            }
            else
            {
                Image sprite = btn_Number[i].GetComponent<Image>();
                sprite.sprite = NotSelected;
            }
        }
    }

    public void BtnRangeSet()
    {
        int itemNumber = shopUI.shopData.shops.Count;
        if (isinventory) itemNumber = shopUI.shopData.inventoryCupons.Count;
        tabnum = itemNumber / 5;

        if (itemNumber % 5 > 0)
        {
            tabnum++;
        }

        print("탭 수 : " + tabnum.ToString());

        for (int i = 0; i < btn_Number.Count; i++)
        {
            btn_Number[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < tabnum; i++)
        {
            btn_Number[i].gameObject.SetActive(true);
        }

        btn_Number[0].gameObject.SetActive(true);

        btnChecked();

    }

    public void Btn_onClick(int index)
    {
        shopUI.PageRefresh(index);
        currentindex = index;
        btnChecked();
    }

    public void Btn_Arrow(int index)
    {
        if (index == -1)
        {
            if (currentindex > 1)
            {
                shopUI.PageRefresh(currentindex - 1);
                currentindex = currentindex - 1;
            }
        }
        else
        {
            if (currentindex < tabnum)
            {
                shopUI.PageRefresh(currentindex + 1);
                currentindex = currentindex + 1;
            }
        }
        btnChecked();
    }

}
