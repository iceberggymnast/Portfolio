using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnShareCanvas : MonoBehaviour
{
    public GameObject sharingCanvas;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClickSharingButton()
    {
        sharingCanvas.SetActive(true);
    }

    public void OnClickExitButton()
    {
        sharingCanvas.SetActive(false);
    }
}
