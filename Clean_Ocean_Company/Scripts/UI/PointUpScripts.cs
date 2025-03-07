using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointUpScripts : MonoBehaviour
{
    public TMP_Text textPoint;
    public RectTransform rectTransform;
    public float speed = 10;

    public AnimationCurve curve2 = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    [Button]
    public void Pointcoroutine(Vector3 startpos, Vector3 endpos, int value)
    {
        StartCoroutine(Play(startpos, endpos, value));
    }

    IEnumerator Play(Vector3 startpos, Vector3 endpos, int value)
    {
        textPoint.text = "+ " + value.ToString();
        rectTransform.anchoredPosition = startpos;

        float offset = 0;
        float offset1 = 0;
        float offset2 = 0;

        while (offset < 1)
        {
            offset += Time.deltaTime * speed;
            offset1 = curve2.Evaluate(offset);
            rectTransform.anchoredPosition = Vector3.Lerp(startpos, endpos, offset1);
            offset2 = curve.Evaluate(offset);
            rectTransform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 0), offset2);
            yield return null;
        }

        PointTextResfresh resfresh = GameObject.FindFirstObjectByType<PointTextResfresh>();
        resfresh.PointRefresh();

        Destroy(this.gameObject);
    }

}
