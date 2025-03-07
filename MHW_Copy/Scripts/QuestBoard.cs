using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoard : MonoBehaviour
{
    public GameObject questboardDepth01;
    public GameObject questboardHover;

    public GameObject playeranimation;
    bool truelock;
    void Start()
    {
        if (questboardDepth01 == null)
        {
            questboardDepth01 = GameObject.Find("QuestboardDepth01");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (!truelock)
        {
            questboardHover.SetActive(true);
        }
            if (Input.GetKeyDown(KeyCode.F))
            {
            Cursor.lockState = CursorLockMode.None;
            playeranimation.GetComponent<Playeranimation>().inputLock = true;
            playeranimation.GetComponent<Playeranimation>().cantClick = true;
            questboardDepth01.SetActive(true);
            questboardHover.SetActive(false);
            truelock = true;
            }
    }

    public void OnTriggerExit(Collider other)
    {
            questboardHover.SetActive(false);
    }
}
