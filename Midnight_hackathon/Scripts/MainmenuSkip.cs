using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainmenuSkip : MonoBehaviour
{
    public bool skip;
    public static MainmenuSkip Instance;
    public GameRecord game1;
    public GameRecord game2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        game1 = GameResultDataLoader.GetLastGameRecord(gameType: 0);
        game2 = GameResultDataLoader.GetLastGameRecord(gameType: EGameType.ColorMatch);
    }


    void Update()
    {
        if (!skip)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                skip = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    game1 = GameResultDataLoader.GetLastGameRecord(gameType: 0);
                    game2 = GameResultDataLoader.GetLastGameRecord(gameType: EGameType.ColorMatch);
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
