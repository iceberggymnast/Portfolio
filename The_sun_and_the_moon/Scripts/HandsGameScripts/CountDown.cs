using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CountDown : MonoBehaviour
{
    [System.Serializable]
    private class CountDownEvent : UnityEvent { }
    private CountDownEvent endOfCountDown; // CountDown 종료 후 외부 Method 실행을 위해 Event Class 사용

    private TextMeshProUGUI textCountDown; // CountDown Text 출력하는 Text UI

    [SerializeField]
    private int maxFontSize; // Font 최대 Size
    [SerializeField]
    private int minFontSize; // Font 최소 Size

    void Awake()
    {
        endOfCountDown = new CountDownEvent();
        textCountDown = GetComponent<TextMeshProUGUI>();
    }

    //public void StartCountDown(UnityAction action, int start = 3, int end = 1)
    //{
    //    StartCoroutine(OnCountDown(action, start, end));
    //}
}
