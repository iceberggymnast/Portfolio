using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDataDontDestroyOnLoad : MonoBehaviour
{
    static NpcDataDontDestroyOnLoad instance2;

    private void Awake()
    {
        if (instance2 == null)
        {
            instance2 = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance2 != this)
        {
            Destroy(gameObject);
        }
    }

}
