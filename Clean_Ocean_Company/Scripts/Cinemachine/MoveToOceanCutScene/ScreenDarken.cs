using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class ScreenDarken : MonoBehaviour
{
    public Image darkImage;  // UI Image 컴포넌트 (검은색 이미지)
    public float darkenSpeed = 0.5f;  // 어두워지는 속도
    private bool isDarkening = false;

    void Start()
    {
        if (darkImage != null)
        {
            darkImage.color = new Color(0f, 0f, 0f, 0f);  // 처음에는 투명한 상태로 시작
        }

        // 씬에 SignalReceiver가 없으면 시그널 작동을 멈추게 처리
        if (FindObjectOfType<SignalReceiver>() == null)
        {
            Debug.LogWarning("SignalReceiver가 없으므로 ScreenDarken 시그널은 작동하지 않습니다.");
            this.enabled = false;  // ScreenDarken 스크립트 비활성화
        }
    }

    void Update()
    {
        // 어두워지는 상태일 때
        if (isDarkening)
        {
            Color color = darkImage.color;
            color.a = Mathf.MoveTowards(color.a, 1f, darkenSpeed * Time.deltaTime);  // Alpha 값 증가
            darkImage.color = color;
        }
    }

    public void StartDarkening()
    {
        isDarkening = true;  // 어두워지는 효과 시작
    }

    public void StopDarkening()
    {
        isDarkening = false;  // 어두워지는 효과 멈춤
    }

    public GameObject cam;
    public void Shake()
    {
        cam.transform.DOShakePosition(0.8f, 0.2f, 20);
    }

    public void GoToStartScene()
    {
        PhotonNetwork.LoadLevel("StartScene");
    }
}
