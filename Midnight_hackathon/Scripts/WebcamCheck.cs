using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamCheck : MonoBehaviour
{
    private void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            Debug.Log(devices[i].name);
        }
    }



}
