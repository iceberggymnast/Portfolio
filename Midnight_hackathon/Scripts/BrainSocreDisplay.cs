using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainSocreDisplay : MonoBehaviour
{
    // Game objects and score values
    public int socoreValue;
    GameObject socoreValueTextObj;
    GameObject socreValuegraphObj;

    // Components for update
    Text socoreValueText;
    RectTransform socreValuegraphRectTransform;

    void Start()
    {
        // Get the components when start
        socoreValueTextObj = transform.GetChild(1).gameObject;
        socoreValueText = socoreValueTextObj.GetComponent<Text>();

        socreValuegraphObj = transform.GetChild(2).GetChild(0).gameObject;
        socreValuegraphRectTransform = socreValuegraphObj.GetComponent<RectTransform>();

    }

    void Update()
    {
        // Limit the score value from 0 to 100.
        socoreValue = Mathf.Clamp(socoreValue, 0, 100);

        // Express the score value as text
        string socoreText = socoreValue.ToString() + "%";
        socoreValueText.text = socoreText;

        // The score values ​​are expressed graph
        float socoreToRot = socoreValue * 1.8f;
        socreValuegraphRectTransform.eulerAngles = Vector3.Lerp(socreValuegraphRectTransform.eulerAngles, new Vector3(0, 0, 180 - socoreToRot), Time.deltaTime * 10.0f);
    }
}
