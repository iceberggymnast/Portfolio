using DG.Tweening;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviourPun
{
    //         SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    //         붙여서 쓰면 댐

    public CanvasGroup cg;
    float timer;
    bool tick;

    private void OnEnable()
    {
        DontDestroyOnLoad(this);

        // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
        DOTween.To(
            () => cg.alpha,       // getter: 현재 padding.top 값을 가져옴
        x => {
                cg.alpha = x; // setter: padding.top 값을 설정
            },
            1,                 // 목표 값
            1f                             // 애니메이션 시간(초)
        );
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3.0f)
        {
            if (!(SceneManager.GetActiveScene().name.Contains("Lobby")))
            {
                if (tick) return;
                //StartCoroutine(PlayerInfo.instance.iC());
                StartCoroutine(Ocean());
                tick = true;
            }
            else if (SceneManager.GetActiveScene().name.Contains("Lobby"))
            {
                DOTween.To(
                () => cg.alpha,       // getter: 현재 alpha 값을 가져옴
                x => {
                    cg.alpha = x; // setter: alpha 값을 설정
                },
                    0,                 // 목표 값
                    1f                 // 애니메이션 시간(초)
                ).OnComplete(() => {
                    // 트윈이 끝났을 때 실행할 함수
                    StartCoroutine(Dead());
                });
            }
        }
    }

    IEnumerator Ocean()
    {
        yield return new WaitForSeconds(3.1f);
        DOTween.To(
        () => cg.alpha,       // getter: 현재 alpha 값을 가져옴
        x => {
            cg.alpha = x; // setter: alpha 값을 설정
        },
            0,                 // 목표 값
            1f                 // 애니메이션 시간(초)
        ).OnComplete(() => {
            // 트윈이 끝났을 때 실행할 함수
            Dead();
            //BriefingUI.instance.SetText("이곳은 안전한 지대입니다.\n여기서 산소를 공급할 수 있고, 쓰레기도 비울 수 있습니다.@혹시 도움이 필요한 분이 있을 지도 모르니\n살펴보는 것도 좋겠습니다.", 3f);
        });
    }

    IEnumerator Dead()
    {
        yield return null;
        Destroy(this.gameObject);
    }
}
