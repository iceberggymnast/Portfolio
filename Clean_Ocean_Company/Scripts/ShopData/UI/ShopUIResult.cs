using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIResult : MonoBehaviour
{
    // 리워드는 여기에 들어올 듯
    public List<RewardInfo> reward;
    public ShopUI mainCanvas;

    // 화면에 보일 결과 텍스트
    public TMP_Text result;

    // 데이터 취합용 변수
    int totalPoint;
    public List<string> rewardList;

    public GameObject openCheset;

    public Image resultImage;
    public Sprite coinImg;

    private void OnEnable()
    {
        if (openCheset != null)
        {
            openCheset.SetActive(true);
        }
    }

    private void OnDisable()
    {
        // 값 초기화 
        reward.Clear();
        result.text = "";
        totalPoint = 0;
        rewardList.Clear();
    }

    public void ResetSprite()
    {
        resultImage.sprite = null;
    }

    public void SetValue()
    {
        for (int i = 0; i < reward.Count; i++)
        {
            // 타입 구분하여 텍스트 설정
            if (reward[i].s_Reward_Type == RewardInfo.S_Reward_Type.R_point)
            {
                int addPoint = mainCanvas.shopData.rewardPoints[reward[i].index].pointValue;
                totalPoint += addPoint;
                if (resultImage.sprite == null)
                {
                    resultImage.sprite = coinImg;
                }
            }
            else if (reward[i].s_Reward_Type == RewardInfo.S_Reward_Type.R_cupon)
            {
                string value = mainCanvas.shopData.rewardCupons[reward[i].index].cuponName;
                if (resultImage.sprite == null || resultImage.sprite == coinImg)
                {
                    resultImage.sprite = mainCanvas.shopData.rewardCupons[reward[i].index].icon;
                }
                rewardList.Add(value);
            }
            else if (reward[i].s_Reward_Type == RewardInfo.S_Reward_Type.R_souvenir)
            {
                string value =mainCanvas.shopData.rewardSouvenirs[reward[i].index].souvenirName;
                if (resultImage.sprite == null || resultImage.sprite == coinImg)
                {
                    resultImage.sprite = mainCanvas.shopData.rewardSouvenirs[reward[i].index].icon;
                }
                rewardList.Add(value);
            }
        }
        
        // 값 정리가 완료되면 text 값 설정
        if (totalPoint > 0)
        {
            result.text += "포인트 : " + totalPoint.ToString() + "<br>";
        }

        for (int i = 0; i < rewardList.Count; i++)
        {
            // 갯수가 많아지면 컷
            if (i > 2)
            {
                result.text += "...";
                break;
            }

            result.text += rewardList[i] + "<br>";
        }

    }


}
