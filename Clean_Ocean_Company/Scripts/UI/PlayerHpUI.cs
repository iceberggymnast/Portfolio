using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpUI : MonoBehaviour
{
    public Image PlayerHPBar;
    public Image PlayerHPBar_danger;
    public GameObject Canvas_BlackSplatter;

    void Start()
    {
        
    }

    void Update()
    {
        PlayerHPBar.fillAmount = PlayerInfo.instance.currentPlayerHP / PlayerInfo.instance.maxPlayerHP;
        PlayerHPBar_danger.fillAmount = PlayerInfo.instance.currentPlayerHP / PlayerInfo.instance.maxPlayerHP;

        if(PlayerInfo.instance.currentPlayerHP < 30)
        {
            PlayerHPBar.gameObject.SetActive(false);
            PlayerHPBar_danger.gameObject.SetActive(true);
            Canvas_BlackSplatter.gameObject.SetActive(true);
        }
        else
        {
            PlayerHPBar.gameObject.SetActive(true);
            PlayerHPBar_danger.gameObject.SetActive(false);

        }
    }
}
