using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BriefingUI : MonoBehaviour
{
    public static BriefingUI instance;

    public TMP_Text text;

    Coroutine coroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        //StartCoroutine(UIController.instance.FadeOut("BriefingUI", 0.2f));
    }

    public void SetText(string values, float time)
    {
        StartCoroutine(UIController.instance.FadeIn("BriefingUI", 0.5f));
        string[] value = values.Split('@');
        int count = value.Length;
        //time = time / count;
        //Debug.Log($"나뉜 시간 : {time} = {time / count} . count : {count}");
        StartCoroutine(ISetBriefingText(count, value, time));
    }

    

    IEnumerator ISetBriefingText(int count, string[] value, float time)
    {
        for (int i = 0; i < count; i++)
        {
            text.text = value[i];
            yield return new WaitForSeconds(time);
        }
        StartCoroutine(UIController.instance.FadeOut("BriefingUI",1f));
    }
}
