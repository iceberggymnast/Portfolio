using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLogo : MonoBehaviour
{
    // �ð� ���� ������Ʈ�� �������� 
    float currenttimes;
    public List<GameObject> uilsit;

    public GameObject menuUI;
    bool dispearcheck;
    void Start()
    {
        
    }

    void Update()
    {
        currenttimes += Time.deltaTime;

        //print(currenttimes);

        if (currenttimes > 0.3f && uilsit[0].GetComponent<Image>().color.a <= 0)
        {
            dispearcheck = true;
        }

        // Ŭ���� �ϸ� �޴�â setavtive

            if (dispearcheck)
            {
            menuUI.SetActive(true);
            gameObject.SetActive(false);
            }
    }


}
