using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstItemGet : MonoBehaviour
{
    // 아이템 정보를 가져올 아이템 매니져 컴포넌트
    public ItemManager itemManager;

    // 처음 먹었을때 기준 게임오브젝트
    public GameObject firstGet;
    // 아이템 아이콘을 띄울 이미지
    public Image _Img_itemIcon;
    // 아이템 이름을 띄울 텍스트
    public TMP_Text _Text_ItemName;
    public TMP_Text _Text_itemDescriptionLong;

    // 이후에 먹고나서 뜰 게임 오브젝트
    public GameObject atferGet;
    //아이템 아이콘
    public Image _Img_itemIcon2;
    // 아이템 이름을 띄울 텍스트
    public TMP_Text _Text_ItemName2;


    // 클릭을 받을 BOOL 값
    bool isClick;
    bool canClick;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && canClick)
        {
            isClick = true;
        }
    }

    public IEnumerator GetPopup(int id)
    {
        // 해당 아이템을 처음 먹은건지 확인 한다.
        if (!itemManager.itemList[id].firstGet)
        {
            // 먹었던 적이 있다고 바꿔줌
            itemManager.itemList[id].firstGet = true;

            // UI의 값들을 변경 시켜줌
            _Img_itemIcon.sprite = itemManager.itemList[id].itemSprite;
            _Text_ItemName.text = itemManager.itemList[id].itemName;
            _Text_itemDescriptionLong.text = itemManager.itemList[id].itemDescriptionLong;

            // 대화중이면 안뜨게 함
            yield return null;
            yield return new WaitUntil(() => QuestManager.questManager.dialogSystem.isConversation == false);

            // UI 팝업!
            firstGet.SetActive(true);

            PlayerState playerState = QuestManager.questManager.myPlayer.GetComponent<PlayerState>();
            playerState.isOpenUI = true;

            // 0.5초간 클릭 못하게 대기
            yield return new WaitForSeconds(0.5f);

            // 입력받을 준비
            canClick = true;

            // 클릭시 UI Close
            yield return new WaitUntil(() => isClick == true);

            // 다시 false로 만들어 준다.
            isClick = false;
            canClick = false;

            // UI 사라짐~ 뿅
            playerState.isOpenUI = false;
            firstGet.SetActive(false);
        }
        else
        {
            // UI의 값들을 변경 시켜줌
            _Img_itemIcon2.sprite = itemManager.itemList[id].itemSprite;
            _Text_ItemName2.text = itemManager.itemList[id].itemName;

            // 대화중이면 안뜨게 함
            yield return null;
            yield return new WaitUntil(() => QuestManager.questManager.dialogSystem.isConversation == false);

            // 안먹었으니 구석에 popup
            atferGet.SetActive(true);

            yield return new WaitForSeconds(1);

            // UI 사라짐~ 뿅
            atferGet.SetActive(false);
        }

        yield return null;
    }

}
