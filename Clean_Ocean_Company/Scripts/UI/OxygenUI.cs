using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OxygenUI : MonoBehaviour
{
    public Image oxygenBar;
    public Image oxygenBar_danger;
    public TMP_Text text_oxygen;
    private void Update()
    {
        ImgSetValue();
        
    }

    public void ImgSetValue()
    {
        if (PlayerInfo.instance != null)
        {
            float oxygenOffset = PlayerInfo.instance.oxygen/ PlayerInfo.instance.maxOxygen;
            text_oxygen.text = ((int)PlayerInfo.instance.oxygen).ToString();
            oxygenBar.fillAmount = oxygenOffset;
            oxygenBar_danger.fillAmount = oxygenOffset;
            if(oxygenBar.fillAmount < 0.3)
            {
                oxygenBar_danger.gameObject.SetActive(true);
                oxygenBar.gameObject.SetActive(false);
            }
            else
            {
                oxygenBar_danger.gameObject.SetActive(false);
                oxygenBar.gameObject.SetActive(true);
            }
        }
        else
        {
            oxygenBar.fillAmount = Time.time * 0.5f;
            oxygenBar_danger.fillAmount = Time.time * 0.5f;
            text_oxygen.text = "PlayerInfo 없음";
        }

    }
}
