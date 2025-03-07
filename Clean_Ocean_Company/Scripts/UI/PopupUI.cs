using Febucci.UI.Core;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class PopupUI : MonoBehaviour
{
    // 대화창이나 지대 진입 시 팝업 기능을 사용 할 수 있습니다.

    public Image background;
    public Image background_Blur;
    public TMP_Text textContent;
    public TMP_Text briefingTextContent;
    public bool isCompleted;
    public float speed = 1;
    public TypewriterCore textani;

    private Coroutine popupCoroutine;

    UnityEvent unityEvent;

    private void Start()
    {
        textani.onTextShowed.RemoveListener(SetTrue); // 기존 연결 제거
        textani.onTextShowed.AddListener(SetTrue);   // 새로 연결

        isCompleted = false;
    }

    [Button]
    public void PopupActive(string content, string brieinfContent, float waitforseconds)
    {
        background.gameObject.SetActive(true);
        background_Blur.gameObject.SetActive(true);

        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine); // 기존 코루틴 중지
            popupCoroutine = null; // 명확히 초기화
        }
        popupCoroutine = StartCoroutine(IPopupActive(content, brieinfContent, waitforseconds));
    }

    IEnumerator IPopupActive(string content, string brieinfContent, float waitforseconds)
    {
        if (isCompleted) isCompleted = !isCompleted;
        // 코루틴 시작 전에 popupCoroutine을 null로 초기화
        popupCoroutine = null;


        // 값 초기화
        textContent.text = "";
        briefingTextContent.text = "";
        float alpha = 0;
        textContent.color = new Color(1, 1, 1, 1);
        briefingTextContent.color = new Color(briefingTextContent.color.r, briefingTextContent.color.g, briefingTextContent.color.b, 1);

        // 이미지 페이드 인
        while (alpha < 1)
        {
            alpha += Time.deltaTime * speed;
            background.color = new Color(1, 1, 1, alpha);
            //background_Blur.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        textContent.text = content;


        yield return null;

        
        
        textani.StartShowingText(true);

        //BriefingUI.instance.SetText(brieinfContent, waitforseconds);
        SetText(brieinfContent, waitforseconds);
        yield return new WaitUntil(() => isCompleted);

        //yield return new WaitForSeconds(waitforseconds);

        while (alpha > 0)
        {
            alpha -= Time.deltaTime * speed;
            background.color = new Color(1, 1, 1, alpha);
            //background_Blur.color = new Color(1, 1, 1, alpha);
            textContent.color = new Color(1, 1, 1, alpha);
            briefingTextContent.color = new Color(briefingTextContent.color.r, briefingTextContent.color.g, briefingTextContent.color.b, alpha);
            yield return null;
        }

        popupCoroutine = null; // 코루틴 종료 시 null로 초기화
        background.gameObject.SetActive(false);
        background_Blur.gameObject.SetActive(false);
    }


    

    public void SetText(string values, float time)
    {
        //StartCoroutine(UIController.instance.FadeIn("BriefingUI", 0.01f));
        string[] value = values.Split('@');
        int count = value.Length;
        //time = time / count;
        //Debug.Log($"나뉜 시간 : {time} = {time / count} . count : {count}");
        StartCoroutine(ISetBriefingText(count, value, time));
    }

    string[] texts = new string[10];



    IEnumerator ISetBriefingText(int count, string[] value, float time)
    {
        texts = value;
        for (int i = 0; i < count; i++)
        {
            briefingTextContent.text = value[i];
            yield return new WaitForSeconds(time);
        }
        //StartCoroutine(UIController.instance.FadeOut("BriefingUI", 1f));
    }

    public void SetTrue()
    {
        if (briefingTextContent.GetParsedText() == texts[texts.Length - 1])
        {
            isCompleted = true;
        }
    }


}
