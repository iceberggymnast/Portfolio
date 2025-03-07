using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodScreen : MonoBehaviour
{
    public PlayerState playerState;
    public Image Image1;
    public Image Image2;
    public Image Image3;

    void Update()
    {
        if (playerState.playerHP < 100)
        {
            Color color = Image1.color;
            color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 5.0f);
            Image1.color = color;
        }
        else
        {
            Color color = Image1.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * 5.0f);
            Image1.color = color;
        }
        
        if (playerState.playerHP < 80)
        {
            Color color = Image2.color;
            color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 20.0f);
            Image2.color = color;
        }
        else
        {
            Color color = Image2.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * 20.0f);
            Image2.color = color;
        }

        if (playerState.playerHP < 40)
        {
            Color color = Image3.color;
            color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 20.0f);
            Image3.color = color;
        }
        else
        {
            Color color = Image3.color;
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * 20.0f);
            Image3.color = color;
        }
    }
}
