using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OxygenGauge : MonoBehaviour
{
    TextMeshProUGUI Oxygen;
    PlayerInfo playerInfo;

    void Start()
    {
        Oxygen = GameObject.Find("Oxygen").GetComponent<TextMeshProUGUI>();
        playerInfo = PlayerInfo.instance;
    }

    void Update()
    {
        Oxygen.text = Mathf.FloorToInt(playerInfo.oxygen).ToString();
    }
}
