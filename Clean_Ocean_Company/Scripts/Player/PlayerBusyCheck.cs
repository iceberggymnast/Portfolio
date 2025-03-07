using DG.Tweening;
using Febucci.UI.Core;
using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerBusyCheck : MonoBehaviour
{
    // 활성 비활성 여부
    public bool isBusy;

    // 내 포톤 뷰
    public PhotonView pv;

    // 해당 스크립트 자녀 오브젝트
    public CanvasGroup cg;
    public TypewriterCore anim;
    public GameObject busyUI;
    Coroutine co;
    public GameObject uiCamera;
    public GameObject otherSpeechBubble;
    bool isStart = false;

    public TMP_Text playerName;

    void Start()
    {
        // 서버 연결 안되어있으면 오프라인 모드로
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom("OfflineRoom");
        }

        // 내 말풍선이면 알파 0
        if (pv.IsMine)
        {
            cg.alpha = 0;
            //playerName.text = PhotonNetwork.LocalPlayer.NickName;
            pv.RPC(nameof(NickNameSetting), RpcTarget.AllBuffered, PhotonNetwork.NickName);
            CanvasGroup textCg = playerName.GetComponent<CanvasGroup>();
            textCg.alpha = 0;
            //pv.RPC(nameof(NickNameSetting), RpcTarget.AllBuffered ,playerName.text);
        }


        StartCoroutine(SetUICamera());
    }

    IEnumerator SetUICamera()
    {
        yield return new WaitUntil(() => PlayerInfo.instance.player != null);
        uiCamera = PlayerInfo.instance.player.transform.Find("CamPos/UICamera").gameObject;
        yield return new WaitUntil(() => uiCamera != null);
        isStart = true;
    }

    void Update()
    {
        if (pv.IsMine) 
        { 
            // 조건 작성
            if (PlayerInfo.instance.isCusor || Input.GetKey(KeyCode.Space))
            {
                isBusy = true;
            }
            else
            {
                isBusy = false;
            }


            if (!otherSpeechBubble.activeInHierarchy)
            {
                // 활성화 여부
                if (!busyUI.activeInHierarchy && isBusy)
                {
                    print("busy set");
                    pv.RPC(nameof(SetBusy), RpcTarget.All, true);
                }
                else if (busyUI.activeInHierarchy && !isBusy)
                {
                    print("Not busy set");
                    pv.RPC(nameof(SetBusy), RpcTarget.All, false);
                }
            }
            else
            {
                // 활성화 여부
                if (!busyUI.activeInHierarchy)
                {
                    return;
                }
                else if (busyUI.activeInHierarchy)
                {
                    pv.RPC(nameof(SetBusy), RpcTarget.All, false);
                }
            }
        }

        if (isStart)
        {
            // LookAt 방향 계산 (Y축만)
            Vector3 targetPosition = new Vector3(uiCamera.transform.position.x, transform.position.y, uiCamera.transform.position.z);

            // Y축만 회전 적용
            transform.LookAt(targetPosition);
        }
    }

    public void Replay()
    {
        co = StartCoroutine(ReplayCo());
    }

    IEnumerator ReplayCo()
    {
        yield return new WaitForSeconds(1.0f);
        anim.ShowText("..........");
    }

    [PunRPC]
    public void SetBusy(bool isbusy)
    {
        if (!isbusy && co != null)
        {
            StopCoroutine(co);
        }

        busyUI.SetActive(isbusy);

        GameObject uiCamera = GameObject.FindWithTag("UICamera");

        if (isbusy)
        {
            busyUI.transform.localScale = Vector3.zero;
            busyUI.transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.OutBounce);

            busyUI.transform.LookAt(uiCamera.transform.position);
            busyUI.transform.Rotate(0, 180, 0);
        }
    }

    [PunRPC]
    public void NickNameSetting(string nickname)
    {
        playerName.text = nickname;
    }
}
