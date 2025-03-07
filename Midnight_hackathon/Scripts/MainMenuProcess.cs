using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuProcess : MonoBehaviour
{
    GameObject canvas_Mainmenu;
    GameObject canvas_SelectContent;
    GameObject canvas_BrainInfomation;

    
    void Start()
    {
         canvas_Mainmenu = GameObject.Find("Canvas_Mainmenu");
         canvas_SelectContent = GameObject.Find("Canvas_SelectContent");
         canvas_BrainInfomation = GameObject.Find("Canvas_BrainInfomation");
    }

    void Update()
    {
        MainmenuLogoMotion();
    }

    GameObject logo;
    bool logoScale;
    void MainmenuLogoMotion()
    {
        if (logo == null)
        {
            logo = canvas_Mainmenu.transform.GetChild(0).gameObject;
        }

        if (logoScale)
        {
            logo.transform.localScale = Vector3.Lerp(logo.transform.localScale, new Vector3(2, 2, 2), Time.deltaTime);
            if (logo.transform.localScale.x < 2.05f)
            {
                logoScale = false;
            }
        }
        else
        {
            logo.transform.localScale = Vector3.Lerp(logo.transform.localScale, new Vector3(2.3f, 2.3f, 2.3f), Time.deltaTime);
            if (logo.transform.localScale.x > 2.2f)
            {
                logoScale = true;
            }
        }


    }
}
