using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorDisableLightOnOff : MonoBehaviour
{
    public Behaviour script;

    void Update()
    {
        if (PlayerInfo.instance != null)
        {
            if (PlayerInfo.instance.isFlashLightOn)
            {
                script.enabled = true;
            }
            else
            {
                script.enabled = false;
            }
        }
    }
}
