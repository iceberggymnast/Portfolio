using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory_Mousehover : MonoBehaviourPunCallbacks, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // 선택한 현재 인벤obj의 이름 번호
    int tagetitem;

    // 인벤 컴포넌트
    public ItemManager inventoryManager;

    // 설명창 아이콘 및 텍스트
    public Image itmeIcon;
    public TMP_Text itemName;
    public TMP_Text itemDescription;

    // 호버 여부 판단
    public RectTransform hoverUI;
    bool isHover;
    public RectTransform canvas;
    Vector2 mousepos;


    public void OnPointerEnter(PointerEventData eventData)
    {
        // 선택한 UI의 이름을 int로 저장
            tagetitem = int.Parse(eventData.hovered[0].name);

        // 해당 리스트가 존재한다면 해당 값에 맞는 아이템 정보를 출력해준다.
        if (tagetitem < inventoryManager.itemInventory.Count)
        {
            itmeIcon.sprite = inventoryManager.itemInventory[tagetitem].itemSprite;
            itemName.text = inventoryManager.itemInventory[tagetitem].itemName;
            itemDescription.text = inventoryManager.itemInventory[tagetitem].itemDescription;
            isHover = true;
        }

    }

    

    private void Update()
    {
        if (isHover)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out mousepos);
            hoverUI.localPosition = mousepos;
        }
        else
        {
            hoverUI.localPosition = new Vector2(-2000, -2000);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tagetitem = 0;
        isHover = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //tagetitem = int.Parse(eventData.hovered[0].name);
        if (tagetitem == 5)
        {
            print("기름병임");
        }
            photonView.RPC(nameof(UseRicecake), RpcTarget.All);

        //inventoryManager.itemInventory[tagetitem].ItemEffect();
    }

    [PunRPC]
    void UseRicecake()
    {
        if (QuestManager.questManager.questList[7].questState == QuestData.QuestState.progress)
        {
            inventoryManager.itemList[5].itemHaveNum--;
            QuestManager.questManager.QuestAddProgress(7, 0, 1);
            NpcClick npcClick = GameObject.Find("NPC_tree").GetComponent<NpcClick>();
            npcClick.StartConversation();
        }
    }
}
