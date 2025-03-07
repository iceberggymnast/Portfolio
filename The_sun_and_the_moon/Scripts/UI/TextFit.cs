using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFit : MonoBehaviour
{
    // 텍스트의 사이즈의 높이를 맞춰준다.
    TMP_Text tmptext;
    RectTransform rt;
    void Start()
    {
        tmptext = GetComponent<TMP_Text>();
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.sizeDelta = new Vector2 (rt.sizeDelta.x ,tmptext.preferredHeight);
    }
}
