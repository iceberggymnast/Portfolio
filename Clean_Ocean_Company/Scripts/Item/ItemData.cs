using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    [Space(10.0f)]
    [Header ("아이템 ID")]
    [Tooltip ("ID가 이미 사용되고 나서 해당 아이템을 사용하지 않게 되었을때 해당 ID는 중복 사용하면 안됩니다.")]
    public int item_ID = 0;
    [Header("아이템 이미지")]
    public string item_Sprite = "sprite_0";

    [Space(10.0f)]
    [Header("아이템 이름")]
    public string item_Name = "플라스틱 빨대";
    [Header("아이템 설명")]
    public string item_Description = "쓰레기다.";
    [Header("아이템 타입")]
    [SerializeField]
    public Item_Type item_Type = Item_Type.Regular_Item;

    public enum Item_Type
    {
        // 일반 아이템
        Regular_Item,
        // 소비 아이템
        Useable_Items,
        // 장비 아이템
        Equipment_Item,
        // 커스터마이징 아이템
        Customizing_Items
    }

    [Space(10.0f)]
    [Header("아이템 사용시 소비 여부")]
    public bool item_Expendables = false;
    [Header("아이템이 인벤토리에 보일지 여부")]
    public bool item_Visible = true;
    [Header("아이템의 가치")]
    public int item_Price = 10;
    [Header("현재 내구도")]
    public float item_Durability = 1;
    [Header("내구도 소모율")]
    public float item_Durability_Rate = 0.01f;
    [Header("내구도 사용 여부")]
    public bool item_Durability_Use = false;

    [Space (10.0f)]
    [Header("현재 소지중인 아이템 갯수")]
    public int item_number = 0;

}
