using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingUI : MonoBehaviour
{
    // 미션 지역 선택 화살표 애니메이션

    [SerializeField]
    public Ease ease;

    // 버튼 애니메이션
    public void Btn_ClickAni(float target)
    {
        // 나갓다가 돌아왔다가 함
        transform.DOLocalMoveX(target, 0.3f, false).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutCubic);
        transform.DOScaleX(0.8f, 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        transform.DOScaleY(1.2f, 0.3f).SetLoops(2, LoopType.Yoyo).SetEase(ease);
    }
}
