using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyPromotionPopUP : MonoBehaviour
{
    public GameObject canvas;
    float canvasGroupAlpha;

    void Start()
    {
        PlayerInfo.instance.isCusor = true;
        canvasGroupAlpha = GetComponent<CanvasGroup>().alpha;
        canvas.gameObject.SetActive(true);
    }

    void Update()
    {
        
    }

    public void OnClickCloseButton()
    {
        canvas.gameObject.SetActive(false);
        PlayerInfo.instance.isCusor = false;

    }
}
