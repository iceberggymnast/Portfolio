using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField]
    private GameObject textPressAnyKey; // "아무 키나 누르세요." 출력 Text

    void Awake()
    {
        StartCoroutine("Scenario");
    }

    IEnumerator Scenario()
    {
        // "아무 키나 누르세요." Text 출력
        textPressAnyKey.SetActive(true);

        // Mouse 왼쪽 버튼을 눌러 "YeonScene"으로 이동
        while (true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("YeonScene");
            }
            yield return null;
        }
    }
}
