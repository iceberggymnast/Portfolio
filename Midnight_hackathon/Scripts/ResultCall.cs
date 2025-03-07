using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCall : MonoBehaviour
{
    public ClockGameUI gameUI;
    void Start()
    {
        gameUI.StartCoroutine(gameUI.Result(0));
    }

    void Update()
    {
        
    }
}
