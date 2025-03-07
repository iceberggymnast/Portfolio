using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    List<Dictionary<string, CanvasGroup>> canvasGroupList = new List<Dictionary<string, CanvasGroup>>();

    public bool boolEvent = false;

    public static UIController instance;



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
        
        CanvasGroup[] canvanObjects = GameObject.FindObjectsByType<CanvasGroup>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (canvanObjects == null) return;

        foreach (CanvasGroup c in canvanObjects)
        {
            // 새 딕셔너리를 만들어 이름과 CanvasGroup을 추가
            Dictionary<string, CanvasGroup> canvasDict = new Dictionary<string, CanvasGroup>();
            canvasDict.Add(c.name, c);
            // 리스트에 딕셔너리 추가
            canvasGroupList.Add(canvasDict);
        }
        if (canvasGroupList == null) return;
    }

    public CanvasGroup FindCanvasGroupByName(string name)
    {
        // 리스트 안에 있는 딕셔너리를 순회
        foreach (var dict in canvasGroupList)
        {
            // 딕셔너리 안에서 해당 이름의 CanvasGroup이 있는지 확인
            if (dict.ContainsKey(name))
            {
                return dict[name];  // 해당 CanvasGroup 반환
            }
        }

        // 찾지 못한 경우 null 반환
        return null;
    }


    public IEnumerator IChangeBoolEvent(float time, bool value)
    {
        yield return new WaitForSeconds(time);
        boolEvent = value;
    }

    public IEnumerator FadeOut(string name, float time)
    {
        CanvasGroup canvasGroup = FindCanvasGroupByName(name);

        float fadeDuration = time; // 원하는 시간으로 설정하세요.

        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup with name '{name}' not found. FadeOut cancelled.");
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            progress += rate * Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, progress);
            yield return null;
        }

        canvasGroup.alpha = 0; // 확실히 0으로 설정
    }

    public IEnumerator FadeIn(string name, float time)
    {
        CanvasGroup canvasGroup = FindCanvasGroupByName(name);

        float fadeDuration = time; // 원하는 시간으로 설정하세요.

        if (canvasGroup == null)
        {
            Debug.LogError($"CanvasGroup with name '{name}' not found. FadeOut cancelled.");
            yield break;
        }

        float startAlpha = canvasGroup.alpha;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            progress += rate * Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(progress, 1, startAlpha);
            yield return null;
        }

        canvasGroup.alpha = 1; // 확실히 0으로 설정
    }

}
