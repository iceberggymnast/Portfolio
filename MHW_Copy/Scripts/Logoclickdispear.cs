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

        // ó�� ������Ʈ �� �� ���̵� ��
        if (popup == false)
        {
            if (gameObject.GetComponent<Image>() != null)
            {
                // �� ������ popup true ��
                if (gameObject.GetComponent<Image>().color.a >= 1)
                {
                    popup = true;
                }
                gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, gameObject.GetComponent<Image>().color.a + Time.deltaTime * dispearspeed);
            }

            // �ؽ�Ʈ�� ���
            if (gameObject.GetComponent<TMP_Text>() != null)
            {
                // �� ������ popup true ��
                if (gameObject.GetComponent<TMP_Text>().color.a >= 1)
                {
                    popup = true;
                }
                gameObject.GetComponent<TMP_Text>().color = new Color(gameObject.GetComponent<TMP_Text>().color.r, gameObject.GetComponent<TMP_Text>().color.g, gameObject.GetComponent<TMP_Text>().color.b, gameObject.GetComponent<TMP_Text>().color.a + Time.deltaTime * dispearspeed);
            }
        }




        // �ΰ� Ŭ���ϸ� �������
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            click = true;
        }

        // �̹����� ��� 
        if(click)
        {
            // �̹����� ���
            if (gameObject.GetComponent<Image>() != null)
            {
                // �� ������������ Click false��
                if (gameObject.GetComponent<Image>().color.a < 0)
                {
                    click = false;
                }
                gameObject.GetComponent<Image>().color = new Color(gameObject.GetComponent<Image>().color.r, gameObject.GetComponent<Image>().color.g, gameObject.GetComponent<Image>().color.b, gameObject.GetComponent<Image>().color.a - Time.deltaTime * dispearspeed);
            }

            // �ؽ�Ʈ�� ���
            if (gameObject.GetComponent<TMP_Text>() != null)
            {
                // �� ������������ click false��
                if (gameObject.GetComponent<TMP_Text>().color.a < 0)
                {
                    click = false;
                }
                gameObject.GetComponent<TMP_Text>().color = new Color(gameObject.GetComponent<TMP_Text>().color.r, gameObject.GetComponent<TMP_Text>().color.g, gameObject.GetComponent<TMP_Text>().color.b, gameObject.GetComponent<TMP_Text>().color.a - Time.deltaTime * dispearspeed);
            }
        }

    }
}
