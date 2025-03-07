using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class ButtonHandler : MonoBehaviour
{
    public GameObject canvas;  // 활성화할 Canvas
    public TextMeshProUGUI countdownText;  // 카운트다운 텍스트
    public Button button;  // 클릭할 버튼

    void Start()
    {
        // Canvas를 비활성화
        canvas.SetActive(false);
    }

    public void OnButtonClick()
    {
        // Canvas 활성화
        canvas.SetActive(true);

        // 버튼 비활성화
        button.gameObject.SetActive(false);

        // 카운트다운 시작
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        int countdownTime = 3;  // 3초 카운트다운

        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();  // 카운트다운 시간 출력
            yield return new WaitForSeconds(1f);  // 1초 대기
            countdownTime--;
        }

        countdownText.text = "시작!";  // 카운트다운이 끝나면 "시작!" 출력
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);  // 카운트다운 텍스트 비활성화
    }
}
