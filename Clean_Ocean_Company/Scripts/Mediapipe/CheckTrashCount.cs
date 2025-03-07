using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckTrashCount : MonoBehaviour
{
    public int trashIndex = 0;
    public Image posImage;
    bool isStop = false;


    private void Update()
    {
        if (isStop) return;
        if (trashIndex > 0)
        {
            trashIndex = transform.childCount;
        }
        else if (trashIndex == 0)
        {
            posImage.color = Color.green;
            isStop = true;
        }
    }
}
