using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerOutline : MonoBehaviour
{
    public Outline[] outline;
    PhotonView pv;
    bool active;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {

        if (!pv .IsMine)
        {
            OutLineSet(0, false);
            OutLineSet(1, false);
            OutLineSet(2, false);
            this.enabled = false;
        }

        RaycastHit[] hit = Physics.SphereCastAll(transform.position, 5, Vector3.up, 0.1f, 1 << 11);

        if (hit.Length > 1)
        {
            if (!active)
            {
                active = true;
                OutLineSet(0, true);
                OutLineSet(1, true);
                OutLineSet(2, true);
                OutLineAnimation(0, 10, true);
                OutLineAnimation(1, 10, true);
                OutLineAnimation(2, 10, true);
            }
        }
        else
        {
            if (active)
            {
                active = false;
                OutLineAnimation(0, 0, false);
                OutLineAnimation(1, 0, false);
                OutLineAnimation(2, 0, false);
            }
        }
    }

    public void OutLineSet (int index, bool enabled)
    {
            outline[index].enabled = enabled;
    }

    public void OutLineAnimation(int index, float width, bool enabled)
    {
        // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
        DOTween.To(
            () => outline[index].OutlineWidth,       // getter: 현재 padding.top 값을 가져옴
        x => {
            outline[index].OutlineWidth = x; // setter: padding.top 값을 설정
        },
            width,                 // 목표 값
            1f                             // 애니메이션 시간(초)
        ).OnComplete(() => {
            // 트윈이 끝났을 때 실행할 함수
            OutLineSet(index, enabled);
        });
    }
}
