using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using TMPro;

public class interaction_Equipment : MonoBehaviour
{
    public GameObject equipmentUI;
    public BackButton_Lobby backButton_Lobby;

    [SerializeField]
    public List<UpgradeInfo> upgradeInfo;

    //툴팁 말풍선
    public GameObject toolTipMessageCanvas;

    void Start()
    {
        SaveSys.saveSys.DataLoad();
        Setvalue();
        Interaction_Base interaction_Base = GetComponent<Interaction_Base>();
        interaction_Base.action = EquipmentOpen;
    }

    void Update()
    {
        if (backButton_Lobby.isClose)
        {
            //상호작용 다시 되게 해주기
            GetComponent<Interaction_Base>().useTrue = false;
        }
    }

    public void EquipmentOpen()
    {
        //툴팁 메시지 꺼주기
        if (toolTipMessageCanvas != null)
        {
            toolTipMessageCanvas.GetComponent<ToolTipBubble>().isFirstAction = true;
        }

        //print("장비 강화 기계 상호작용 테스트");
        equipmentUI.SetActive(true);
        PlayerInfo.instance.isCusor = true;
    }

    public void Setvalue()
    {
        if (upgradeInfo.Count == 4)
        {
            for (int i = 0; i < upgradeInfo.Count; i++)
            {
                SetfloattValueToPlayerInfo(upgradeInfo[i].upgredeEquipment, upgradeInfo[i].upgradeAmount[SaveSys.saveSys.playerupgrades.upgradesLV[i]]);
            }
        }
        else
        {
            print("업그레이드 정보가 담긴 변수 리스트의 Count가 4가 아닙니다.");
        }
    }

    // sting 값을 읽어서 value값으로 설정해줌
    void SetfloattValueToPlayerInfo(string name, float value)
    {
        object playerInfoInstance = PlayerInfo.instance;

        // 필드 찾기
        FieldInfo fieldInfo = playerInfoInstance.GetType().GetField(name, BindingFlags.Public | BindingFlags.Instance);
        if (fieldInfo != null && fieldInfo.FieldType == typeof(float))
        {
            fieldInfo.SetValue(playerInfoInstance, value); // 필드의 값을 설정
            Debug.Log(name + " 필드의 값이 " + value + "로 설정되었습니다.");
        }
        else
        {
            Debug.LogError("float type 필드를 찾을 수 없습니다: " + name);
        }
    }

    public GameObject resultWindow;
    public TMP_Text upgradeName;
    public TMP_Text before;
    public TMP_Text after;

    public void ResultPopup(string name, float beforeValue, float afterValue)
    {
        resultWindow.SetActive(true);
        upgradeName.text = name;
        before.text = beforeValue.ToString();
        after.text = afterValue.ToString();
    }

    public void Btn_ResultClose()
    {
        resultWindow.SetActive(false);
        print("Result Close");
    }

}

[Serializable]
public class UpgradeInfo
{
    //업그레이드 할 장비(Playerinfo.instance에서 가져올거임)
    public string upgredeEquipment;
    public string upgredeName;

    // 업그레이드 비용 리스트
    public List<int> upgradeCosts = new List<int> { 100, 200, 300, 400, 500 };   //장비에 맞게 에디터에 값 넣기(장비별 시작 양 참고)

    // 증가량 리스트
    public List<float> upgradeAmount = new List<float> { 1.0f , 1.5f, 3.0f, 5.5f, 8.5f, 12.0f };
}

