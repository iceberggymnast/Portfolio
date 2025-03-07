using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopData 
{
        [Header("상점 박스 아이템 ID")]
        [Tooltip("중복 사용 X.")]
        public int s_Item_ID = 0;
        [Header("상자 재고")]
        public int s_item_Stock = 10;
        [Header("상자 가격")]
        public int s_item_Price = 1000;
        [Header("상점 아이템 이미지")]
        public string s_item_Sprite = "sprite_0";

        [Space(10.0f)]
        [Header("상점 아이템 이름")]
        public string s_item_Name = "";
        [Header("상점 아이템 설명")]
        public string s_item_Description = "2022년 송별 기념 설명";

        [Space(10.0f)]
        [Header("리워드 정보")]
        public List<RewardInfo> rewards = new List<RewardInfo>();

        [Header("확률 정보")]
        public List<float> probability = new List<float>();

        public Sprite icon;
}

[Serializable]
public class RewardInfo
{
    [Header("리워드 타입")]
    [SerializeField]
    public S_Reward_Type s_Reward_Type = S_Reward_Type.R_point;

    [Header("타입 별 인덱스")]
    public int index;

    public enum S_Reward_Type
    {
        // 포인트
        R_point,
        // 쿠폰
        R_cupon,
        // 기념품
        R_souvenir
    }
}

[Serializable]
public class RewardPoint
{
    public int index = 0;
    public int pointValue = 10;
}

[Serializable]
public class RewardCupon
{
    public int index = 0;
    public string cuponName = "함덕해수욕장 샤워장 할인권";
    public string cupon_Sprite = "sprite_0";
    public Sprite icon;
    [SerializeField]
    public List<RewardData> rewardData;
    public string exchangeLink = "www.naver.com";
}

[Serializable]
public class RewardSouvenir
{
    public int index = 0;
    public string souvenirName = "호돌이 피규어";
    public string souvenir_Sprite = "sprite_0";
    public Sprite icon;
    [SerializeField]
    public List<RewardData> rewardData;
    public string exchangeLink = "www.naver.com";
}

[Serializable]
public class RewardData
{
    [Header("바코드")]
    public string bacode = "123456";
    [Header("발행 일자")]
    public string publishedDate = "2024.01.01";
    [Header("만료 일자")]
    public string expireDate = "2024.12.30";
    [Header("유저 발행 여부")]
    public bool isPublication;
}


