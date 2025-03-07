using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//장비별 시작 양
//1. 쓰레기 흡입 속도
//cleaningSpeed: 5
//2. 쓰레기 흡입 범위
//detectionDistance: 5
//3. 쓰레기통 용량
//maxTrashcanCapacity:10
//4. 최대 산소량
//maxOxygen:100

public class buyButton : MonoBehaviour
{
    public int index;
    public interaction_Equipment interaction_Equipment;

    // 업그레이드 시 활성화할 UI 박스들 (5개)
    public GameObject[] blue; 

    // UI에 표시될 현재 업그레이드 비용 텍스트
    public TextMeshProUGUI costText;

    // 현재 버튼 클릭 횟수
    [SerializeField]
    private int clickCount = 0; 

    void Start()
    {
        if (interaction_Equipment == null)
        {
            interaction_Equipment = GameObject.FindObjectOfType<interaction_Equipment>();
        }
        SetColorFill();
    }

    public void OnClickUpgradeButton()
    {
        //현재 클릭 횟수가 5번 보다 적을 때(구입 가능할 때)
        if(clickCount < 5)
        {
            int price = interaction_Equipment.upgradeInfo[index].upgradeCosts[clickCount];
            //업그레이트 비용만큼 포인트 가지고 있는지 확인
            if (PlayerInfo.instance.point >= price)
            {
                //포인트 차감
                PlayerInfo.instance.PointPlusOrMinus(- price);

                //업그레이드 단계 및 UI업데이트
                SaveSys.saveSys.playerupgrades.upgradesLV[index]++;
                SetColorFill();
                interaction_Equipment.ResultPopup(interaction_Equipment.upgradeInfo[index].upgredeName, interaction_Equipment.upgradeInfo[index].upgradeAmount[clickCount - 1], interaction_Equipment.upgradeInfo[index].upgradeAmount[clickCount]);
                Debug.Log($"업그레이드 완료! 현재 장비: {PlayerInfo.instance.cleaningSpeed}");
                SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
                soundManger.UISFXPlayRandom(0, 3);
            }
            else
            {
                Debug.Log($"코인이 부족합니다.");
            }
        }
    }



    void SetColorFill()
    {
        // 나는 몇단계 업그레이드인가
        clickCount = SaveSys.saveSys.playerupgrades.upgradesLV[index];

        // 색상 적용
        //업그레이드 단계에 따라 파란 박스 이미지 켜주기
        if (clickCount <= blue.Length && clickCount > 0)
        {
            for (int i = 0; i < clickCount; i++)
            {
                Image blueImage = blue[i].GetComponent<Image>();

                Color color = blueImage.color;
                color.a = 1; // 알파값 설정
                blueImage.color = color;
            }
        }

        //비용 업데이트
        UpdateCostText();

        // 능력 업데이트
        interaction_Equipment.Setvalue();

        // 데이터 저장
        SaveSys.saveSys.DataSave();
    }

    //UI에 표시될 현재 업그레이드 비용 텍스트 갱신해주는 함수
    void UpdateCostText()
    {
        //남은 업그레이드가 있는 경우 비용 표시, 아니면 "-"라고 표시
        if(clickCount < 5)
        {
            costText.text = interaction_Equipment.upgradeInfo[index].upgradeCosts[clickCount].ToString();
        }
        else
        {
            //P는 꺼주고
            transform.GetChild(2).gameObject.SetActive(false);

            //비용 텍스트에 "최대"라고 적어주기
            transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "최대";

            Debug.Log("최대 업그레이드에 도달했습니다!");
            GetComponent<Button>().interactable = false; // 버튼 비활성화
        }
    }
}
