using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTextController : MonoBehaviour
{
    public GameObject getScoreTextPrefab; // 점수 획득시 나올 텍스트 프리팹
    private GameObject activeScoreText;

    void Start()
    {
        getScoreTextPrefab.SetActive(false);
    }

    public void GetScoreText()
    {
        if(activeScoreText == null)
        {
            activeScoreText = Instantiate(getScoreTextPrefab);
            activeScoreText.SetActive(true);
            // Text 활성화 후 1초가 지나면 다시 비활성화 시킴.
            StartCoroutine(DelayText(1.0f));
        }
    }

    IEnumerator DelayText(float delay)
    {
        yield return new WaitForSeconds(delay);
        activeScoreText.SetActive(false);
        Destroy(activeScoreText);
        activeScoreText = null;
    }
}
