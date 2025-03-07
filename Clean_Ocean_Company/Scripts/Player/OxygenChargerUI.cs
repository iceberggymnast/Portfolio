using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OxygenChargerUI : MonoBehaviour
{
    //산소충전기 보유 개수
    TextMeshProUGUI O2chargerAmount;

    void Start()
    {
        O2chargerAmount = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //print("O2chargerAmount " + O2chargerAmount);
        //print("current_OxygenchargerAmount " + PlayerInfo.instance.current_OxygenchargerAmount);
        O2chargerAmount.text = PlayerInfo.instance.current_OxygenchargerAmount.ToString();
    }
}
