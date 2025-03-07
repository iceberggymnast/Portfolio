using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDontDestroyOnLoad : MonoBehaviour
{
    static UIDontDestroyOnLoad instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

}
