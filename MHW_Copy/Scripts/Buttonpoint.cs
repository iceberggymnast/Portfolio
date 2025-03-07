using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Buttonpoint : MonoBehaviour, IPointerEnterHandler
{
    public GameObject uiCanvas;
    public SelectMenu selectmenu;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (gameObject.GetComponent<AudioSource>() != null)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            for (int i = 0; i < selectmenu.menuList.Count; i++)
            {
                if (selectmenu.menuList[i] == gameObject)
                {
                    selectmenu.menuNum = i;
                    break;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (uiCanvas == null)
        {
            uiCanvas = GameObject.Find("Canvas");
        }
        selectmenu = uiCanvas.GetComponent<SelectMenu>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
