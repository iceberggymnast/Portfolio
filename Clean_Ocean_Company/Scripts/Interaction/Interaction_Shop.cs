using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Interaction_Shop : MonoBehaviour
{
    // 상점 오브젝트에 인터렉션 베이스와 함께 들어가는 컴포넌트 입니다.

    /*
     * 구현부
     * 기본 베이스 스크립트에 상호작용 할 함수를 구현
     * 해당 함수를 실행하면 백엔드에서 상점 + 보관함 정보 + 포인트 정보를 가져옴
     * 불러와진 데이터에 따라 UI에 배치됨 (데이터 정의는 정해야 함)
     * 구매할 경우 결과값을 받거나 내부에서 돌림
     * 결과 창을 표시
     * 
     * 일단 내부에서만 처리해서 보여주고 나중에 백엔드 통신으로 변경할 예정
     */

    // 통신 데이타가 될 쇼핑 정보 리스트들
    public List<ShopData> shops = new List<ShopData>();
    public List<RewardPoint> rewardPoints = new List<RewardPoint>();
    public List<RewardCupon> rewardCupons = new List<RewardCupon>();
    public List<RewardSouvenir> rewardSouvenirs = new List<RewardSouvenir>();

    // 플레이어의 보관함에 보관할 상품들
    public List<InventoryCupon> inventoryCupons;

    public GameObject shopUI;

    private string interactionName = "상점 열기";

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    void Start()
    {
        if (shopUI == null)
        {
            shopUI = GameObject.Find("Canvas_Shop");
        }

        // Base 스크립트의 델리게이트를 찾아서 상호작용시 원하는 함수를 넣어줌
        Interaction_Base del = GetComponent<Interaction_Base>();
        del.action = ShopOpen;
        del.intername = interactionName;
        Interaction_Shop interaction_Shop = GetComponent<Interaction_Shop>();
        //string shopdata = JsonUtility.ToJson(interaction_Shop, true);
        //print(shopdata);

        if (inventoryCupons == null)
        {
            if (SaveSys.saveSys.loadDataInventory != null)
            {
                inventoryCupons = SaveSys.saveSys.loadDataInventory;
            }
            else
            {
                inventoryCupons = new List<InventoryCupon>();
            }
        }
    }

    void ShopOpen()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        shopUI.SetActive(true);
        if (inventoryCupons == null)
        {
            inventoryCupons = SaveSys.saveSys.loadDataInventory;
        }
        PlayerInfo.instance.isCusor = true;
    }

    public void closeUI()
    {
        Interaction_Base del = GetComponent<Interaction_Base>();
        del.useTrue = false;
    }

    // 상점 데이터를 불러옴
    void ShopInfoLoad()
    {
        // 알파 이후 DB 구현
    }

    // 상점 구매 관련 기능 구현
    public RewardInfo ShopBuy(int index)
    {
        // 재고가 있는지 체크하여 예외처리
        //if (shops[index].s_item_Stock <= 0) return null;
        // 해당 상품 가격을 체크
        int price = shops[index].s_item_Price;
        // 그만큼의 가격을 가지고 있으면 해당 금액만큼 차감한다.
        if (price <= PlayerInfo.instance.point)
        {
            // 해당 가격 만큼 차감시킨다.
            PlayerInfo.instance.PointPlusOrMinus(-price);
            // 박스의 재고를 차감한다.
            shops[index].s_item_Stock--;
            // 랜덤 값을 구한다.
            float probabilityValue = Random.Range(0.0f, 1.0f);
            // 리워드 인덱스 기본값 선언
            int rewardindex = -1;

            // 나온 확률 결과값에 따라 Check
            for (int i = 0; i < shops[index].probability.Count; i++)
            {
                // 확률 값과 확률 표를 비교해본다.
                if (probabilityValue <= shops[index].probability[i])
                {
                    // 해당 상품의 리워드 인덱스를 기록
                    rewardindex = i;
                    break;
                }
            }

            print(rewardindex);

            if (rewardindex == -1)
            {
                // for 문 돌리고 나서 rewardindex가 -1이면 꽝임.
                print(" 앗..! 꽝 입 니 다!! ");
            }
            else
            {
                // 아닐경우 해당 index 의 상품을 지급해야 함
                RewardInfo rewardinfo = shops[index].rewards[rewardindex];
                print(rewardinfo.s_Reward_Type.ToString() + rewardinfo.index.ToString());

                // 포인트인지 현물상품인지 체크
                // 포인트일 경우
                if (rewardinfo.s_Reward_Type == RewardInfo.S_Reward_Type.R_point)
                {
                    // 해당 포인트만큼 지급
                    RewardPoint rewardPoint = rewardPoints[index];
                    PlayerInfo.instance.point += rewardPoint.pointValue;
                    print(rewardPoint.pointValue.ToString() + " 만큼 지급하여 " + PlayerInfo.instance.point.ToString() + " 포인트만큼 보유중입니다.");
                }
                // 기프티콘일 경우
                else if (rewardinfo.s_Reward_Type == RewardInfo.S_Reward_Type.R_cupon)
                {

                    // 해당 쿠폰이 뭔지 가져온다.
                    RewardCupon rewardCupon = rewardCupons[rewardinfo.index];

                    // 새로운 쿠폰 코드를 생성시켜 준다.
                    RewardData rewardData = new RewardData();
                    rewardData.bacode = GenerateBarcode(16);

                    // 쿠폰 정보를 담는다
                    InventoryCupon inventoryCupon = new InventoryCupon();
                    inventoryCupon.cupon = rewardCupon;
                    inventoryCupon.rewardData = rewardData;

                    // 담은 데이터를 보관함에 담아준다
                    inventoryCupons.Add(inventoryCupon);

                    print(rewardCupon.cuponName + " 당첨!");

                }
                //기념품류일 경우
                else if (rewardinfo.s_Reward_Type == RewardInfo.S_Reward_Type.R_souvenir)
                {
                    // 해당 기념품이 뭔지 가져온다.
                    RewardSouvenir reward = rewardSouvenirs[rewardinfo.index];

                    // 새로운 쿠폰 코드를 생성시켜 준다.
                    RewardData rewardData = new RewardData();
                    rewardData.bacode = GenerateBarcode(16);

                    // 쿠폰 정보를 담는다
                    InventoryCupon inventoryCupon = new InventoryCupon();
                    inventoryCupon.souvenir = reward;
                    inventoryCupon.rewardData = rewardData;

                    // 담은 데이터를 보관함에 담아준다
                    inventoryCupons.Add(inventoryCupon);

                    print(reward.souvenirName + " 당첨!");
                }

                SaveSys.saveSys.DataSave();
                print("구매 완료");
                return rewardinfo;

            }
        }
        else
        {
            // 금액이 부족함
            print("금액이 부족합니다.");
        }

        return null;
    }

    // 바코드 생성 기능 
    string GenerateBarcode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        // string 대신 StringBuilder를 쓰면 새 문자열을 수정할때마다 부하가 적다.
        StringBuilder result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int randomIndex = Random.Range(0, chars.Length);
            result.Append(chars[randomIndex]);
        }

        return result.ToString();
    }

}


// 획득한 쿠폰을 보관할 곳
[Serializable]
public class InventoryCupon
{
    public RewardCupon cupon;
    public RewardSouvenir souvenir;
    public RewardData rewardData;
}

