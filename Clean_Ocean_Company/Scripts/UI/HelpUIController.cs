using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HelpUIController : MonoBehaviour
{
    GameObject helpUI;
    public GameObject helpMediapipeUI;
    public GameObject helpGameKeyUI;

    public bool isHelpUiClose
    {
        get => ihelpuiclose;
        set
        {
            ihelpuiclose = value;
            OnBoolChange?.Invoke(ihelpuiclose);
        }
    }

    private bool ihelpuiclose = false;

    bool isStart = false;


    public event Action<bool> OnBoolChange;
    

    IEnumerator Start()
    {

        yield return new WaitUntil(() => PlayerInfo.instance.player != null);
        helpUI = GameObject.Find("Help");
        yield return new WaitUntil(() => helpUI != null);

        helpMediapipeUI = helpUI.transform.GetChild(0).gameObject;
        helpGameKeyUI = helpUI.transform.GetChild(1).gameObject;

        OnBoolChange += CloseHelpUI;

        helpMediapipeUI.SetActive(false);
        //helpGameKeyUI.SetActive(false);


        yield return new WaitForSeconds(8f);
        if (!isStart)
        {
            isStart = true;
            StartCoroutine(UIController.instance.FadeOut("HelpGameKeyUI", 0.5f));
        }
    }

    void CloseHelpUI(bool value)
    {
        if (value == true && !isStart)
        {
            isStart = true;
            StartCoroutine(UIController.instance.FadeOut("HelpGameKeyUI", 0.5f));
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            helpGameKeyUI.SetActive(!helpGameKeyUI.activeSelf);
        }
    }
}
