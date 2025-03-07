using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameControl : MonoBehaviour
{
    private void Start()
    {
        // 씬이 로딩될때마다 PlayerSetActive 함수를 호출시킨다.
        SceneManager.sceneLoaded += PlayerSetActive;

    }

    // 호출되면 플레이어의 셋액티브를 끄고 킨다.
    void PlayerSetActive(Scene scene, LoadSceneMode mode)
    {
        // 미니게임 맵에 간거라면 false 해준다.
        if (scene.name == "Minigame_massage" || scene.name == "Minigame_tether")
        {
            QuestManager.questManager.myPlayer.SetActive(false); 
        }

        // 집 맵에 간거라면 true 해준다.
        if(scene.name == "home")
        {
            QuestManager.questManager.myPlayer.SetActive(true); 
        }
    }

}
