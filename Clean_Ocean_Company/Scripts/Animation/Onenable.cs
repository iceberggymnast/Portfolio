using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(CanvasGroup))]
public class Onenable : MonoBehaviour
{
    // 애니메이션 실행 시 실행할 함수
    public UnityEvent onEventTriggered;

    [Flags]
    public enum AnimationType
    {
        None = 0,
        Fade_In = 1 << 0,
        ScaleY = 1 << 1,
        ScaleX = 1 << 2,
        World_Pos = 1 << 3,
        Local_Pos = 1 << 4,
    }

    // 애니메이션 종류 선택
    [SerializeField]
    public AnimationType animationType;

    // 애니메이션 실행 시간
    public float duration = 0.5f;

    // 애니메이션 scale 크기
    public float scaleValue = 0.8f;

    // 애니메이션 시작 위치
    public Vector3 startPos = new Vector3 (0, 0, 0);

    [SerializeField]
    public Ease ease;

    // 담기
    float scaleY;
    float scaleX;
    Vector3 endPos;
    Vector3 endPosL;

    private void Awake()
    {
        scaleY = transform.localScale.y;
        scaleX = transform.localScale.x;
        endPos = transform.position;
        endPosL = transform.localPosition;
    }


    private void OnEnable()
    {
        if (animationType.HasFlag(AnimationType.Fade_In))
        {
            CanvasGroup cg = GetComponent<CanvasGroup>();
            cg.alpha = 0;

            // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
            DOTween.To(
                () => cg.alpha,       // getter: 현재 padding.top 값을 가져옴
            x => {
                cg.alpha = x; // setter: padding.top 값을 설정
            },
                1,                 // 목표 값
                duration                             // 애니메이션 시간(초)
            );
        }
        if (animationType.HasFlag(AnimationType.ScaleY))
        {
            //float scaleY = transform.localScale.y;
            transform.localScale = new Vector3(transform.localScale.x, scaleValue, transform.localScale.z);
            transform.DOScaleY(scaleY, duration).SetEase(ease);
        }
        if (animationType.HasFlag(AnimationType.ScaleX))
        {
            //float scaleX = transform.localScale.x;
            transform.localScale = new Vector3(scaleValue, transform.localScale.y, transform.localScale.z);
            transform.DOScaleX(scaleX, duration).SetEase(ease);
        }
        if (animationType.HasFlag(AnimationType.World_Pos))
        {
            //Vector3 endPos = transform.position;
            transform.position = startPos;

            transform.DOMove(endPos, duration).SetEase(ease);
        }
        if (animationType.HasFlag(AnimationType.Local_Pos))
        {
            //Vector3 endPosL = transform.localPosition;
            transform.localPosition = startPos;
            transform.DOLocalMove(endPosL, duration).SetEase(ease);
        }
        if (onEventTriggered != null)
        {
            StartCoroutine(Event());
        }
    }

   IEnumerator Event()
    {
        yield return new WaitForSeconds(duration);
        TriggerEvent();
    }

    public void TriggerEvent()
    {
        // UnityEvent 호출
        if (onEventTriggered != null)
            onEventTriggered.Invoke();
    }
}
