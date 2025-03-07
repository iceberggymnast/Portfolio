using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mainmenuimgscroll : MonoBehaviour
{
    RectTransform rectTransform;
    Image image;
    public float scrollSpeed = 30.0f;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponentInChildren<Image>();
    }

    // ���θ޴��� �̹����� õõ�� ��ũ�� �Ǵ� ȿ��
    void Update()
    {
        rectTransform.anchoredPosition += -Vector2.right * Time.deltaTime * scrollSpeed;
        if (rectTransform.anchoredPosition.x <= -752.0f)
        {
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
