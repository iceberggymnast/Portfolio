using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Logoclickdispear : MonoBehaviour
{
    bool click;
    public float dispearspeed = 1.0f;
    bool popup;
    void Start()
    {
        
    }


    void Update()
    {

        // 처음 업데이트 될 때 페이드 인
        if (popup == false)
        {
            if (gameObject.GetComponent<Image>() != null)
            {
                // 다 켜지면 popup true 로
                if (gameObject.GetComponent<Image>().color.a >= 1)
                {
                    popup = true;
                }
                gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, gameObject.GetComponent<Image>().color.a + Time.deltaTime * dispearspeed);
            }

            // 텍스트일 경우
            if (gameObject.GetComponent<TMP_Text>() != null)
            {
                // 다 켜지면 popup true 로
                if (gameObject.GetComponent<TMP_Text>().color.a >= 1)
                {
                    popup = true;
                }
                gameObject.GetComponent<TMP_Text>().color = new Color(gameObject.GetComponent<TMP_Text>().color.r, gameObject.GetComponent<TMP_Text>().color.g, gameObject.GetComponent<TMP_Text>().color.b, gameObject.GetComponent<TMP_Text>().color.a + Time.deltaTime * dispearspeed);
            }
        }




        // 로고를 클릭하면 사라져요
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            click = true;
        }

        // 이미지의 경우 
        if(click)
        {
            // 이미지일 경우
            if (gameObject.GetComponent<Image>() != null)
            {
                // 다 투명해졌으면 Click false로
                if (gameObject.GetComponent<Image>().color.a < 0)
                {
                    click = false;
                }
                gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, gameObject.GetComponent<Image>().color.a - Time.deltaTime * dispearspeed);
            }

            // 텍스트일 경우
            if (gameObject.GetComponent<TMP_Text>() != null)
            {
                // 다 투명해졌으면 click false로
                if (gameObject.GetComponent<TMP_Text>().color.a < 0)
                {
                    click = false;
                }
                gameObject.GetComponent<TMP_Text>().color = new Color(gameObject.GetComponent<TMP_Text>().color.r, gameObject.GetComponent<TMP_Text>().color.g, gameObject.GetComponent<TMP_Text>().color.b, gameObject.GetComponent<TMP_Text>().color.a - Time.deltaTime * dispearspeed);
            }
        }

    }
}
