using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuBotton : MonoBehaviour
{
    public bool isMainMenu;
    public GameObject pause;


    void Update()
    {
        if (pause != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMainMenu)
            {
                Time.timeScale = 0.0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                pause.SetActive(true);
                isMainMenu = true;
            }
            else if (isMainMenu)
            {
                Time.timeScale = 1.0f;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                pause.SetActive(false);
                isMainMenu = false;
            }
        }
    }

}
