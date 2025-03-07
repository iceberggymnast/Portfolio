using System;
using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class ClockGameManager : MonoBehaviour
{
    public AudioManager audioManager;
    public enum EGameState
    {
        Ready,
        Running,
        End,
    }

    [SerializeField] private UDPServer udpServer;
    [SerializeField] private Text timeText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text problemText;
    [SerializeField] private Text countdownText;

    [SerializeField] private float timeLimit;

    private float currentTimer;
    private int score = 0;
    private EGameState currentState = EGameState.Ready;
    private float elapsedTime = 0f;
    

    private void Awake()
    {
     
    }

    private void Update()
    {
        UpdateUI();
        switch (currentState)
        {
            case EGameState.Ready:
                UpdateReady();
                break;
            case EGameState.Running:
                UpdateRunning();
                break;
            case EGameState.End:
                UpdateEnd();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateUI()
    {
        timeText.text = $"{currentTimer}";
        scoreText.text = $"{score}" + "<color=black>개</color>";
    }

    private void UpdateEnd()
    {
    }

    private void UpdateRunning()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer < 0) currentTimer = 0;
        var x = Math.Floor(currentTimer);
        
        timeText.text = x.ToString("00");
        if (currentTimer <= 0f)
        {
            ChangeState(EGameState.End);
        }
    }

    private void UpdateReady()
    {
        elapsedTime += 0f;
        if (elapsedTime > 3.0f)
        {
            ChangeState(EGameState.Running);
        }
    }

    private void Start()
    {
        currentTimer = timeLimit;
        ChangeState(EGameState.Running);
    }

    private void ChangeState(EGameState state)
    {
        elapsedTime = 0f;
        //on changed
        switch (state)
        {
            case EGameState.Ready:
                break;
            case EGameState.Running:
                StartCoroutine(ProgressRound());
                break;
            case EGameState.End:
                StopAllCoroutines();
                GameResultDataLoader.SaveResult(EGameType.ClockProblem, score, DateTimeOffset.Now.ToUnixTimeMilliseconds());
                print("Finished!");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentState = state;
    }

    private IEnumerator ProgressRound()
    {
        //ui - 문제
        //ui - 3초
        print(3);
        countdownText.text = "3";
        var problem = ClockProblemFactory.GetInstance().CreateProblem();

        if(problem.hour <= 9)
        {
            problemText.text = "0" + $"{problem.hour}:";
        }
        else
        {
            problemText.text = $"{problem.hour}:";
        }

        if(problem.minute == 0)
        {
            problemText.text += "0" + $"{problem.minute}";
        }
        else
        {
            problemText.text += $"{problem.minute}";
        }

        for (int i = 5; i >= 1; i--)
        {
            countdownText.text = $"{i}";
            yield return new WaitForSeconds(1.0f);
        }
        
        if (udpServer != null)
        {
            var curPose = udpServer.CurrentPoseInfo;
            if (IsRightAnswer(problem, curPose.hour, curPose.minute))
            {
                score += 1;
                countdownText.text = "정답!";
                audioManager.AudioPlay(1);
            }
            else
            {
                countdownText.text = "땡!";
                audioManager.AudioPlay(2);
            }
        }

        print("score: " + score);
        scoreText.text = $"{score}";
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ProgressRound());
    }

    private bool IsRightAnswer(ClockProblem problem, int hour, int minute)
    {
        int h2 = MinuteToHour(minute);
        if (!CheckHour(hour, problem.hour) && !CheckHour(h2, problem.hour))
        {
            return false;
        }

        int ansH2 = MinuteToHour(problem.minute);
        if (!CheckHour(hour, ansH2) && !CheckHour(h2, ansH2))
        {
            return false;
        }

        return true;
    }

    private int MinuteToHour(int min)
    {
        if (min <= 2 || min > 58)
        {
            return 12;
        }
        else if (min <= 7)
        {
            return 1;
        }
        else if (min <= 12)
        {
            return 2;
        }
        else if (min <= 17)
        {
            return 3;
        }
        else if (min <= 22)
        {
            return 4;
        }
        else if (min <= 27)
        {
            return 5;
        }

        else if (min <= 32)
        {
            return 6;
        }

        else if (min <= 37)
        {
            return 7;
        }

        else if (min <= 42)
        {
            return 8;
        }
        else if (min <= 47)
        {
            return 9;
        }

        else if (min <= 52)
        {
            return 10;
        }
        else
        {
            return 11;
        }
    }

    private bool CheckHour(int input, int answer)
    {
        if (input >= 11 || input < 1)
        {
            input = 12;
        }
        else if (input >= 7)
        {
            input = 9;
        }
        else if (input >= 4)
        {
            input = 6;
        }

        else
        {
            input = 3;
        }

        return answer == input;
    }



    public EGameState CurrentState
    {
        get { return currentState; }
    }

    public int Score
    {
        get { return score; }
    }
}