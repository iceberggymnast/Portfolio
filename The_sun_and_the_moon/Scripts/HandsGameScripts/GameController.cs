using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int score;

    [field: SerializeField]
    public float MaxTime { private set; get; }
    public float CurrentTime { private set; get; }

    public int Score
    {
        set => score = Mathf.Max(0, value);
        get => score;
    }

    public int NormalHandCount { set; get;}
    public int RedHandCount { set; get; }

    void Start()
    {
        GameStart();
    }

    void GameStart()
    {
        StartCoroutine("OnTimeCount");
    }

    IEnumerator OnTimeCount()
    {
        CurrentTime = MaxTime;

        while (CurrentTime > 0)
        {
            CurrentTime -= Time.deltaTime;

            yield return null;
        }
        // currentTime = 0이 되면 GameOver Method를 호출해 GameOver 처리함.
        GameOver();
    }
    
    void GameOver()
    {
        // 현재 획득한 여러 정보 저장
        PlayerPrefs.SetInt("CurrentScore", Score);
        PlayerPrefs.SetInt("CurrentNormalHandCount", NormalHandCount);
        PlayerPrefs.SetInt("CurrentRedHandCount", RedHandCount);

        // GameOver Scene으로 이동
        SceneManager.LoadScene("GameOverScene");
    }
}