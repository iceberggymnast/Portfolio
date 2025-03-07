using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyInfo : MonoBehaviour
{
    // 상점 UI 우측에 있는 아이템 정보를 보여주기 위한 기능을 담당하고 있는 컴포넌트 입니다.

    // 표시 할 데이터가 들어갈 곳
    public ShopData data;
    public Interaction_Shop _Shop;

    // UI 요소들
    public TMP_Text itemName;
    public TMP_Text itemPrice;

    public TMP_Text rewardPoint;
    public TMP_Text rewardSouvenir;
    public TMP_Text rewardCupon;

    public TMP_InputField itemNumber;

    public TMP_Text totalPrice;
    public TMP_Text remainPoint;

    public Button btn_Buy;

    public Image icon;

    // 결과창 
    public ShopUIResult resultUI;

    private void Start()
    {
        _Shop = GameObject.FindAnyObjectByType<Interaction_Shop>();
    }

    public void InfoRefesrh()
    {
        SetText();
    }
    
    public void InfoRefesrh(ShopData shopData)
    {
        data = shopData;
        SetText();
    }

    void SetText()
    {
        if (data != null)
        {
            icon.sprite = data.icon;

            // 박스 이름 지정
            itemName.text = data.s_item_Name;

            // 가격 표시
            itemPrice.text = data.s_item_Price.ToString();

            // 리워드 내용 초기화
            rewardPoint.text = "";
            rewardSouvenir.text = "";
            rewardCupon.text = "";
            itemNumber.text = "1";

            for (int i = 0; i < data.rewards.Count; i++)
            {
                if (data.rewards[i].s_Reward_Type == RewardInfo.S_Reward_Type.R_souvenir)
                {
                    if (rewardSouvenir.text != "")
                    {
                        rewardSouvenir.text += "<br>";
                    }

                    rewardSouvenir.text += _Shop.rewardSouvenirs[data.rewards[i].index].souvenirName;
                }
                else if (data.rewards[i].s_Reward_Type == RewardInfo.S_Reward_Type.R_cupon)
                {
                    if (rewardCupon.text != "")
                    {
                        rewardCupon.text += "<br>";
                    }

                    rewardCupon.text += _Shop.rewardCupons[data.rewards[i].index].cuponName;
                }
                else if (data.rewards[i].s_Reward_Type == RewardInfo.S_Reward_Type.R_point)
                {
                    if (rewardPoint.text != "")
                    {
                        rewardPoint.text += " / ";
                    }

                    rewardPoint.text += _Shop.rewardPoints[data.rewards[i].index].pointValue.ToString();
                }
            }

            SetNumber();

            if (rewardPoint.text == "")
            {
                rewardPoint.text = "-";
            }
            if (rewardSouvenir.text == "")
            {
                rewardSouvenir.text = "-";
            }
            if (rewardCupon.text == "")
            {
                rewardCupon.text = "-";
            }

        }
    }

    public void SetNumber()
    {
        int number = int.Parse(itemNumber.text);
        totalPrice.text = (data.s_item_Price * number).ToString() + " P";
        int reminPointint = PlayerInfo.instance.point - (data.s_item_Price * number);
        remainPoint.text = reminPointint.ToString() + " P";

        if (reminPointint < 0)
        {
            btn_Buy.interactable = false;
        }
        else
        {
            btn_Buy.interactable = true;
        }
    }

    public void BuyBtn()
    {
        resultUI.gameObject.SetActive(true);

        for (int i = 0; i < int.Parse(itemNumber.text); i++)
        {
            RewardInfo info = _Shop.ShopBuy(data.s_Item_ID);
            if (info != null)
            {
                resultUI.reward.Add(info);
            }
        }

        resultUI.SetValue();
        SetNumber();
    }

}
