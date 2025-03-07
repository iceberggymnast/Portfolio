using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIOnOffManager : MonoBehaviour
{
    public List<GameObject> popupCanvas; // 팝업 창의 캔버스

    // 팝업이 열릴 때 호출됩니다.
    public void Open_Close()
    {
        bool currentActiveState = popupCanvas[0].activeSelf; // 첫 번째 부모 캔버스의 활성 상태를 확인합니다.

        // 모든 부모 캔버스와 자식 캔버스 오브젝트들을 활성화 또는 비활성화합니다.
        ToggleCanvasAndChildren(!currentActiveState);
    }

    public void Open()
    {
        if (popupCanvas != null)
        {
            // 팝업 캔버스를 활성화합니다.
            ToggleCanvasAndChildren(true);
        }
    }

    public void Close()
    {
        if (popupCanvas != null)
        {
            ToggleCanvasAndChildren(false);
        }
    }

    private void ToggleCanvasAndChildren(bool active)
    {
        foreach (GameObject canvas in popupCanvas)
        {
            canvas.SetActive(active);
        }
    }

    // 게임 종료 버튼 클릭 시 호출될 함수
    public void QuitGame()
    {
        Debug.Log("게임 종료 버튼이 클릭되었습니다."); // 이 줄은 필요하지 않지만 디버깅에 도움이 됩니다.
        Application.Quit(); // 게임 종료
    }

}
