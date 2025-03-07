using System.Collections;
using UnityEngine;
using TMPro;

public class HandsHitTextViewer : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 30.0f; // Text 이동 속도
    private Vector2 defaultPosition; // 이동 Animation이 있어 초기 위치 저장
    private TextMeshProUGUI textHit;
    private RectTransform rectHit;

    void Awake()
    {
        textHit = GetComponent<TextMeshProUGUI>();
        rectHit = GetComponent<RectTransform>();
        defaultPosition = rectHit.anchoredPosition;

        gameObject.SetActive(false);
    }

    public void OnHit(string hitData, Color color)
    {
        // Object를 화면에 보이도록 Setting
        gameObject.SetActive(true);

        // Hand Object 타격 시 출력할 정보 Setting
        textHit.text = hitData;

        // Text가 위로 이동하며 점점 사라지는 OnAnimation() Coroutine 실행
        StopCoroutine("OnAnimation");
        StartCoroutine("OnAnimation", color);
    }

    IEnumerator OnAnimation(Color color)
    {
        rectHit.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;

        while (color.a > 0)
        {
            // 투명도 1 ~ 0 으로 Setting
            color.a -= Time.deltaTime;
            textHit.color = color;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}