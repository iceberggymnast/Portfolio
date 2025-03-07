using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotweeenImg : MonoBehaviour
{
    void Start()
    {
        transform.DOLocalMoveY(-907, 5, false).SetLoops(-1, LoopType.Restart);
    }

}
