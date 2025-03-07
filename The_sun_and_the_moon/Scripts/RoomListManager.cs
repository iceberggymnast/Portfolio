using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomListManager : MonoBehaviour
{
    public Button[] CellBtn; // 4개의 버튼
    public Button PreviousBtn, NextBtn;
    public List<string> myList = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18" };

    int currentPage = 1, maxPage, itemsPerPage = 4;

    void Start()
    {
        // 최대 페이지 계산
        maxPage = (myList.Count % itemsPerPage == 0) ? myList.Count / itemsPerPage : myList.Count / itemsPerPage + 1;

        // 이전, 다음 버튼 상태 설정
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        int startIndex = (currentPage - 1) * itemsPerPage;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            if (startIndex + i < myList.Count)
            {
                CellBtn[i].interactable = true;
                CellBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = myList[startIndex + i];
            }
            else
            {
                CellBtn[i].interactable = false;
                CellBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }

    // ◀버튼 -2, ▶버튼 -1, 셀 숫자
    public void BtnClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else if (num >= 0 && num < CellBtn.Length)
        {
            int index = (currentPage - 1) * itemsPerPage + num;
            if (index < myList.Count)
                print(myList[index]);
        }

        Start(); // 페이지 업데이트
    }

    [ContextMenu("리스트추가")]
    void ListAdd() { myList.Add("새 항목"); Start(); }

    [ContextMenu("리스트제거")]
    void ListRemove() { if (myList.Count > 0) { myList.RemoveAt(0); Start(); } }
}
