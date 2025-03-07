using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Inventory_UI;

public class Inventory_UI : MonoBehaviour
{
    public ItemManager itemManager;
    public GameObject inventoryContent;

    public List<ItemUI> itemUIs = new List<ItemUI>();
    public class ItemUI
    {
        public Sprite spriteIcon;
        public int haveNum;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        InventoryRefresh();
    }

    public void InventoryRefresh()
    {
        // 리스트 초기화
        itemUIs.Clear();

        // 현재 인벤토리에 있는 아이템 갯수를 체크하고 추가적으로 목록을 더 만들어야 하는지 확인


        // 활성화 될떄 인벤토리 리스트 체크 
        for (int i = 0; i < itemManager.itemInventory.Count; i++)
        {
            ItemUI itemui = new ItemUI();
            itemui.spriteIcon = itemManager.itemInventory[i].itemSprite;
            itemui.haveNum = itemManager.itemInventory[i].itemHaveNum;
            itemUIs.Add(itemui);
        }

        // 작성한 리스트에 따라 UI에 적용
        for (int i = 0; i < itemManager.itemInventory.Count; i++)
        {
            GameObject reference = inventoryContent.transform.GetChild(i).gameObject;
            Image img = reference.transform.GetChild(1).GetComponent<Image>();
            TMP_Text num = reference.transform.GetChild(0).GetComponent<TMP_Text>();

            img.sprite = itemUIs[i].spriteIcon;
            num.text = itemUIs[i].haveNum.ToString();
        }
    }

}
