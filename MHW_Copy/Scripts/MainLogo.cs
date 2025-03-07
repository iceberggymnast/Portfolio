using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainLogo : MonoBehaviour
{
    // 시간 따라서 오브젝트를 실행해줌 
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

        // 클릭을 하면 메뉴창 setavtive

            if (dispearcheck)
            {
            menuUI.SetActive(true);
            gameObject.SetActive(false);
            }
    }


}
