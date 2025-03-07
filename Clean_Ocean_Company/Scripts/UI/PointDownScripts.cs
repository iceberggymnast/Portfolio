using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PointDownScripts : MonoBehaviour
{
    public TMP_Text text;

    void Start()
    {
        transform.localPosition = new Vector3(750, 420, 0);
        transform.DOLocalMoveY(240, 1, false).SetEase(Ease.OutCubic).OnComplete(Destrp);
        transform.DOScale(0, 1).SetEase(Ease.OutCubic);
    }

    public void Destrp()
    {
        Destroy(this.gameObject);
    }


    public void SetValue(int value)
    {
        text.text = "- " + (Mathf.Abs(value)).ToString();
    }
}
