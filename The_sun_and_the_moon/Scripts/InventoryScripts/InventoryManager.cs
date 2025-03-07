using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    private bool menuActivated;

    void Start()
    {
        
    }

    // 키보드 'i'키를 누르면 인벤토리 창이 활성화 되게 하기
    void Update()
    {
        // 처음에는 인벤토리를 비활성화 시켰기 때문에 화면에 보이지 않음.
        if(Input.GetButtonDown("Inventory") && menuActivated)
        {
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
        // 인벤토리가 비 활성화 된 상태에서 키보드 "i" 버튼을 누를 경우 진행화면이 정지되면서...
        // 인벤토리 창이 생성됨.
        else if (Input.GetButtonDown("Inventory") && !menuActivated)
        {
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }
    }
}
