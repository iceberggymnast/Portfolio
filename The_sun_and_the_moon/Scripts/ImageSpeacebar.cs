using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSpeacebar : MonoBehaviour
{
    public Image img;

    public Sprite imgSprite0;
    public Sprite imgSprite1;

    bool ticTok = true;

    float time = 0;

    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.5f)
        {
            TicTok();
        }
    }

    void TicTok()
    {
        ticTok = !ticTok;
        time = 0;

        if (ticTok)
        {
            img.sprite = imgSprite0;
        }
        else
        {
            img.sprite = imgSprite1;
        }
    }
}
