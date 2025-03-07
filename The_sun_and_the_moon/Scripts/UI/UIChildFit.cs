using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChildFit : MonoBehaviour
{
    // 자녀의 크기 변화에 맞게 UI 사이즈를 맞춰준다
    public RectTransform momRect;
    public RectTransform sonRect;

    // 오프셋
    public Vector2 offset;

    // 반전 여부
    public bool reverse;

    void Update()
    {
        if (reverse)
        {
            Vector2 reverseSon = new Vector2(sonRect.sizeDelta.y, sonRect.sizeDelta.x);
            momRect.sizeDelta = Vector3.Lerp(momRect.sizeDelta, offset + reverseSon, Time.deltaTime * 20.0f);
        }
        else
        {
            momRect.sizeDelta = Vector3.Lerp(momRect.sizeDelta, offset + sonRect.sizeDelta, Time.deltaTime * 20.0f);
        }
    }
}
