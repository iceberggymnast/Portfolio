using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowAlpha : MonoBehaviour
{
    // �ڽ� �̹����� ���� ���� ���� �ڳ� ������Ʈ���� ��� ������Ʈ���� ���ĵ� �����ϰ� �ʹ�.

    float myalpha;
    Image meimg;

    public List<GameObject> childlist;

    void Start()
    {
        meimg = GetComponent<Image>();

        // �ڽ��� ����� ���� �ش� ������ŭ �־��ֱ� 
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject != null)
            {
                GameObject addobj = transform.GetChild(i).gameObject;
                childlist.Add(transform.GetChild(i).gameObject);
            }
            else
            {
                break;
            }
        }
    }

    void Update()
    {
        myalpha = meimg.color.a;

        foreach (GameObject child in childlist)
        {
            // Image ������Ʈ�� �ִ� ���
            Image childImage = child.GetComponent<Image>();
            if (childImage != null)
            {
                Color color = childImage.color;
                childImage.color = new Color(color.r, color.g, color.b, myalpha);
            }

            // SpriteRenderer ������Ʈ�� �ִ� ���
            SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
            if (childSprite != null)
            {
                Color color = childSprite.color;
                childSprite.color = new Color(color.r, color.g, color.b, myalpha);
            }

        }
    }
}
