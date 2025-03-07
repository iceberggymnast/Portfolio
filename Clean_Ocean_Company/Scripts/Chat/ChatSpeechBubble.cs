using Photon.Pun;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;


public class ChatSpeechBubble : MonoBehaviourPun
{
    // 스트링 값을 받아서 대화 말풍선을 띄워주는 기능을 합니다.
    public GameObject bubbleOBJ;

    // 자녀 오브젝트들
    public Image bubble;
    public TMP_Text tmp_text_conversation;

    public Transform myTransform;
    Transform playerTransform;


    private void Start()
    {
        this.gameObject.SetActive(false);
    }


    public IEnumerator PlayOwn(string conversation, Coroutine speechCoroutine)
    {
        // 말풍선을 활성화 해주고 값을 초기화
        bubbleOBJ.SetActive(true);
        tmp_text_conversation.text = conversation;
        // 텍스트 사이즈 체크 후 약간의 애니메이션
        float size = tmp_text_conversation.preferredHeight;
        tmp_text_conversation.text = "";


        // 말풍선은 작게 잡아주기
        RectTransform img = bubble.GetComponent<RectTransform>();
        float tagetSize = size + 200;
        img.sizeDelta = new Vector3(img.sizeDelta.x, 0);

        // 약간의 애니메이션
        Vector3 targetsizeV3 = new Vector3(img.sizeDelta.x, tagetSize);
        while (img.sizeDelta.y < targetsizeV3.y - 0.1f)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, targetsizeV3, Time.deltaTime * 14.0f);
            yield return null;
        }

        // 텍스트 설정
        tmp_text_conversation.text = conversation;

        yield return new WaitForSeconds(3);

        tmp_text_conversation.text = "";

        // 애니메이션 효과 나오면서 닫힘
        while (img.sizeDelta.y > 130)
        {
            img.sizeDelta = Vector3.Lerp(img.sizeDelta, new Vector3(img.sizeDelta.x, 120), Time.deltaTime * 20.0f);
            yield return null;
        }

        bubbleOBJ.SetActive(false);

        speechCoroutine = null;
    }

}
