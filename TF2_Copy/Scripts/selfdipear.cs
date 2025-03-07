using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selfdipear : MonoBehaviour
{
    public Image image;
    float fadeTimer = 0;
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        fadeTimer += Time.deltaTime;
        Color color = image.color;
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * 20.0f);
        image.color = color;

        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }

}
