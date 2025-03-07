using DG.Tweening;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatOtherSpeechBubble : MonoBehaviourPun
{
    // 스트링 값을 받아서 대화 말풍선을 띄워주는 기능을 합니다.
    public GameObject bubbleOBJ;
    // 자녀 오브젝트들
    public Image bubble;
    public TMP_Text tmp_text_conversation;
    public TMP_Text tmp_text_npc;
    public GameObject uiCamera;
    public CanvasGroup cg;
    bool isStart = false;
    public bool isChat = false;

    private void Start()
    {
        if (photonView.IsMine)
        {
            cg.alpha = 0;
        }

        photonView.RPC(nameof(SetBusy), RpcTarget.AllBuffered, false);

        StartCoroutine(SetUICamera());
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (isStart && uiCamera != null)
            {
                // LookAt 방향 계산 (Y축만)
                Vector3 targetPosition = new Vector3(uiCamera.transform.position.x, bubbleOBJ.transform.position.y, uiCamera.transform.position.z);

                // Y축만 회전 적용
                bubbleOBJ.transform.LookAt(targetPosition);
            }
        }
    }

    IEnumerator SetUICamera()
    {
        uiCamera = GameObject.FindWithTag("UICamera");
        yield return new WaitUntil(() => uiCamera != null);
        isStart = true;
    }

    [PunRPC]
    public void SetBusy(bool isbusy)
    {
        isChat = isbusy; // 상태를 동기화
        bubbleOBJ.SetActive(isbusy); // bubbleOBJ 활성화/비활성

        GameObject uiCamera = GameObject.FindWithTag("UICamera");

        if (isbusy)
        {
            bubbleOBJ.transform.localScale = Vector3.zero;
            bubbleOBJ.transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.OutBounce);

            bubbleOBJ.transform.LookAt(uiCamera.transform.position);
            bubbleOBJ.transform.Rotate(0, 180, 0);
        }
    }


    [PunRPC]
    void SendText(float size, float targetSize, string name, string conversation)
    {
        tmp_text_conversation.text = conversation;
        tmp_text_npc.text = name;

        RectTransform img = bubble.GetComponent<RectTransform>();

        // 애니메이션 초기화
        img.sizeDelta = new Vector2(img.sizeDelta.x, 0);

        // 목표 사이즈를 애니메이션으로 설정
        StartCoroutine(AnimateBubble(img, size, targetSize));
    }

    IEnumerator AnimateBubble(RectTransform img, float size, float targetSize)
    {
        Vector2 targetSizeV2 = new Vector2(img.sizeDelta.x, targetSize);

        // 확장 애니메이션
        while (img.sizeDelta.y < targetSize - 0.1f)
        {
            img.sizeDelta = Vector2.Lerp(img.sizeDelta, targetSizeV2, Time.deltaTime * 14.0f);
            yield return null;
        }

        yield return new WaitForSeconds(3); // 말풍선 유지 시간

        // 축소 애니메이션
        while (img.sizeDelta.y > 130)
        {
            img.sizeDelta = Vector2.Lerp(img.sizeDelta, new Vector2(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
            yield return null;
        }

        tmp_text_conversation.text = "";
        tmp_text_npc.text = "";

        photonView.RPC(nameof(SetBusy), RpcTarget.All, false);
    }




    public IEnumerator PlayOwn(string name, string conversation, Coroutine speechCoroutine)
    {
        photonView.RPC(nameof(SetBusy), RpcTarget.All, true);

        // 텍스트 높이 계산
        tmp_text_conversation.text = conversation;
        float size = tmp_text_conversation.preferredHeight;

        float targetSize = size < 1.1f ? 300 : (size - 1) * 6.6f + 300;

        //Debug.LogError("targetSize = size < 1.1f ? 300 : (size - 1) * 6.6f + 300");
        //Debug.LogError($"{targetSize} = {size} < 1.1f ? 300 : {(size - 1) * 6.6f} + 300");

        // RPC 호출로 데이터를 모든 클라이언트에 전달
        photonView.RPC(nameof(SendText), RpcTarget.All, size, targetSize, name, conversation);

        

        yield return null; // 코루틴 종료
    }
}
