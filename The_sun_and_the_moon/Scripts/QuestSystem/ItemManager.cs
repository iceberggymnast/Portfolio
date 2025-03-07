using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ItemManager : MonoBehaviourPunCallbacks
{
    // 아이템 데이터를 관리할 리스트
    public List<ItemData.ItemInfo> itemList = new List<ItemData.ItemInfo>();

    // 아이템 인벤토리를 관리함
    public List<ItemData.ItemInfo> itemInventory = new List<ItemData.ItemInfo>();

    // 아이템 데이터를 저장할 CSV 파일
    public TextAsset csvFile;

    // 퀘스트 매니져 컴포넌트
    QuestManager questManager;

    // 인벤토리 UI 컴포넌트
    public Inventory_UI inventory_UI;

    // 처음먹나
    public FirstItemGet firstItemGet;

    void Start()
    {
        ReadCSV();
        questManager = GetComponent<QuestManager>();
    }

    // 아이템을 추가할때 쓸 함수 (호출해서 쓰세요)
    [PunRPC]
    public void ItemAdd(int id, int number)
    {
        // 갯수가 1개 이상으로 지정되어있는지 체크
        if (number <= 0)
        {
            print("지급할 아이템의 갯수가 지정되지 않았거나 음수입니다.");
            return;
        }
        
        // 아이템을 이미 가지고 있는지 체크한다
        for (int i = 0; i < itemInventory.Count; i++)
        {
            // 넣으려는 아이템이 인벤토리에 이미 있는지 체크한다.
            if(itemInventory[i].itemId == itemList[id].itemId)
            {
                // 있으면 해당 인벤토리의 아이템 갯수를 증감시킨다.
                itemInventory[i].itemHaveNum += number;
                // 아이템 갯수와 퀘스트 진행도를 동기화 해준다.
                questManager.QuestProgressItemCheck();
                invenCheck();

                // 해당 아이템이 두번째 먹었으니 띄워준다.
                firstItemGet.StartCoroutine(firstItemGet.GetPopup(id));

                print(itemList[id].itemName + " 아이템이 " + number.ToString() + "개 지급되었습니다");
                return;
            }
        }
        // for문을 다 돌았으면 없다는 의미이니 해당 아이템을 리스트에 추가한다.
        itemInventory.Add(itemList[id]);
        // 아이템을 추가하면 마지막으로 가있으니 마지막에 있는 항목의 가지고 있는 갯수를 증감시킨다.
        itemInventory[itemInventory.Count - 1].itemHaveNum += number;
        // 해당 아이템이 처음 먹은 아이템이라면 팝업을 띄워준다.
        firstItemGet.StartCoroutine(firstItemGet.GetPopup(id));
        print(itemList[id].itemName + " 아이템이 " + number.ToString() + "개 지급되었습니다");
        // 아이템 갯수와 퀘스트 진행도를 동기화 해준다.
        questManager.QuestProgressItemCheck();
        // 인벤토리 새로고침
        invenCheck();
    }


    // 아이템을 제거할때 쓸 함수 (호출해서 쓰세요)
    [PunRPC]
    public void ItemRemove(int id, int number)
    {
        // 해당 아이템이 리스트에 있는지 찾는다.
        for (int i = 0;i < itemInventory.Count; i++)
        {
            if (itemInventory[i].itemId == id)
            {
                // 찾았다면 해당 아이템의 갯수를 입력한 값만큼 빼준다.
                itemInventory[i].itemHaveNum -= number;

                print(itemInventory[i].itemName + " 아이템이 " + number + "개 제거되었습니다");

                // 해당 아이템의 가지고 있는 갯수가 0보다 작아지면 리스트에서 제거한다.
                if (itemInventory[i].itemHaveNum <= 0)
                {
                    itemInventory.RemoveAt(i);
                }


                // 아이템 갯수와 퀘스트 진행도를 동기화 해준다.
                invenCheck();
                questManager.QuestProgressItemCheck();
                break;
            }
        }
        // 아이템 갯수와 퀘스트 진행도를 동기화 해준다.
        questManager.QuestProgressItemCheck();
        invenCheck();
    }

    // 인벤이 열려있으면 아이템이 추가되고 없어질때 마다 체크해준다.
    public void invenCheck()
    {
        if (inventory_UI.gameObject.activeSelf)
        {
            inventory_UI.InventoryRefresh();
        }
    }

    // 아이템 정보를 csv파일을 읽어 데이터 리스트에 저장해놓는다.
    void ReadCSV()
    {
        StringReader reader = new StringReader(csvFile.text);
        bool isFirstLine = true;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            // 첫 줄이 헤더인 경우 넘어감
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            // 쉼표로 구분된 데이터를 분리
            string[] values = line.Split(',');

            if (values.Length == 5) // CSV의 필드 수에 맞추어 수정
            {
                int itemId = int.Parse(values[0]);
                string itemName = values[1];
                string itemDescription = values[2];
                string sprite = values[3];
                string longDescription = values[4];


                // ItemInfo 객체 생성 및 값 설정
                ItemData.ItemInfo itemInfo = new ItemData.ItemInfo();
                itemInfo.ItemDataAdd(itemId, itemName, itemDescription, sprite, longDescription);
                // 리스트에 추가
                itemList.Add(itemInfo);
            }
        }
    }
}
