using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameCountDown : MonoBehaviour
{
    public Behaviour minigameMGR;
    public Behaviour canvasMGR;
    public List<Behaviour> componantList;

    public TMP_Text countDownText;
    public Image backImg;

    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        countDownText.text = "준비...";
        yield return new WaitForSeconds(2);
        countDownText.text = "3";
        yield return new WaitForSeconds(1);
        countDownText.text = "2";
        yield return new WaitForSeconds(1);
        countDownText.text = "1";
        yield return new WaitForSeconds(1);
        countDownText.text = "시작!";
        yield return new WaitForSeconds(1);
        countDownText.text = "";
        backImg.enabled = false;

        yield return null;

        minigameMGR.enabled = true;
        canvasMGR.enabled = true;

        if(componantList.Count > 0)
        {
            for(int i = 0; i < componantList.Count; i++)
            {
                componantList[i].enabled = true;
            }
        }
    }
}
