using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipImages : MonoBehaviour
{
    public List<Sprite> toolTipImages;
    public List<string> toolTipText;

    //툴팁이미지 넣을 곳
    public Image setSprite;

    //툴팁텍스트 넣을 곳
    public TextMeshProUGUI setText;

    void Start()
    {
        setText.gameObject.SetActive(false);

        StartCoroutine(RandomSet());
    }

    //랜덤으로 툴팁이미지와 텍스트를 띄워주는 함수
    IEnumerator RandomSet()
    {
        while(true)
        {
            //이미지 할당
            setSprite.GetComponent<Image>().sprite = toolTipImages[Random.Range(0, toolTipImages.Count)];

            setText.gameObject.SetActive(true);

            //텍스트 할당
            setText.GetComponent<TextMeshProUGUI>().text = toolTipText[Random.Range(0, toolTipText.Count)];

            yield return new WaitForSeconds(1f);
        }
    }

    
}
