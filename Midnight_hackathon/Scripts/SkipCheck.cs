using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCheck : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject gameselect;
    void Start()
    {
        if(MainmenuSkip.Instance.skip)
        {
            mainmenu.SetActive(false);
            gameselect.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
