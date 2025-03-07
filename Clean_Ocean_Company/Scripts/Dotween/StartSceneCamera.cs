using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneCamera : MonoBehaviour
{
    void Start()
    {
        transform.DOLocalMoveX(0.005f, 2, false).SetEase(Ease.OutQuart);
    }

}
