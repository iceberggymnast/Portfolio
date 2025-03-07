using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MediapipeTrash : MonoBehaviour
{
    int cleanCount = 3;

    float transparency = 1f;

    Image trashImage;

    Color trashImageColor;

    GameObject porpoiseImageList;

    private void Start()
    {
        trashImage = GetComponent<Image>();
        trashImageColor = trashImage.color;

        porpoiseImageList = transform.parent.parent.gameObject;

        trashImageColor = new Color(trashImageColor.r, trashImageColor.g, trashImageColor.b, transparency);

        trashImage.color = trashImageColor;
    }

    public void CleanTrashEvent()
    {
        cleanCount--;
        transparency -= 1f / 3f;
        trashImageColor.a = transparency;
        trashImage.color = trashImageColor;
        if (cleanCount == 0)
        {
            MediapipeManager.Instance.CleanTrashEvent();

            Destroy(gameObject);
        }
    }
}
