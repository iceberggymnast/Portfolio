using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissonSelectScrollPaddingAnimation : MonoBehaviour
{
    public VerticalLayoutGroup group;
    public RectTransform rectTf;
    public GameObject prefab;
    public float prefabWidth;
    public float startPadding;
    public float targetPadding;

    public Interection_RoomMake roominfo;

    void Update()
    {
        int count = transform.childCount;
        float height = (group.spacing * count) + group.padding.top + (prefabWidth * count);
        rectTf.sizeDelta = new Vector2(rectTf.sizeDelta.x, height);
    }

    private void OnEnable()
    {
        rectTf.localPosition = new Vector3(rectTf.localPosition.x, 0, rectTf.localPosition.z);
        int list = roominfo.currentLocation.questData.Count;
        for (int i = 0; i < list; i++)
        {
            GameObject go = Instantiate(prefab, transform);
            TMP_Text header = go.transform.GetChild(0).GetComponent<TMP_Text>();
            TMP_Text body = go.transform.GetChild(1).GetComponent<TMP_Text>();
            Button button = go.GetComponentInChildren<Button>();
            
            if (!roominfo.currentLocation.questData[i].usable)
            {
                button.interactable = false;
            }

            header.text = roominfo.currentLocation.questData[i].questName;
            body.text = roominfo.currentLocation.questData[i].questShortDescription;
            go.name = i.ToString();
        }

        group.padding.top = Mathf.RoundToInt(startPadding);
        group.spacing = Mathf.RoundToInt(startPadding);

        // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
        DOTween.To(
            () => group.padding.top,       // getter: 현재 padding.top 값을 가져옴
            x => {
                group.padding.top = Mathf.RoundToInt(x); // setter: padding.top 값을 설정
            },
            targetPadding,                 // 목표 값
            1f                             // 애니메이션 시간(초)
        );

        // DOTween.To를 사용하여 startPadding에서 targetPadding으로 애니메이션
        DOTween.To(
            () => group.spacing,       // getter: 현재 padding.top 값을 가져옴
            x => {
                group.spacing = Mathf.RoundToInt(x); // setter: padding.top 값을 설정
            },
            targetPadding,                 // 목표 값
            1f                             // 애니메이션 시간(초)
        );
    }

    private void OnDisable()
    {
        int count = transform.childCount;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
