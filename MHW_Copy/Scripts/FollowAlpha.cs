using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowAlpha : MonoBehaviour
{
    // 자신 이미지의 알파 값에 따라 자녀 오브젝트들의 몇몇 컴포넌트들의 알파도 조절하고 싶다.

    float myalpha;
    Image meimg;

    public List<GameObject> childlist;

    void Start()
    {
        meimg = GetComponent<Image>();

        // 자식이 몇개인지 보고 해당 갯수만큼 넣어주기 
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
            // Image 컴포넌트가 있는 경우
            Image childImage = child.GetComponent<Image>();
            if (childImage != null)
            {
                Color color = childImage.color;
                childImage.color = new Color(color.r, color.g, color.b, myalpha);
            }

            // SpriteRenderer 컴포넌트가 있는 경우
            SpriteRenderer childSprite = child.GetComponent<SpriteRenderer>();
            if (childSprite != null)
            {
                Color color = childSprite.color;
                childSprite.color = new Color(color.r, color.g, color.b, myalpha);
            }

        }
    }
}
