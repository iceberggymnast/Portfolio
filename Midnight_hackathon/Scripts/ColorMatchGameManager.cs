using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ColorMatchGameManager : MonoBehaviour
{
    public AudioManager audioManager;

    [System.Serializable]
    public class ColorResponse
    {
        public string filename;
        public string message;
        public string transcribed_text;
    }

    public enum EGameState
    {
        Ready,
        Question,
        Answered,
        Finished,
    }

    public Text colorMatchProblemText;
    public Text countdownText;
    public Image correctOImage;
    public Image WrongXImage;
    public CountDown countDown;
    public Text scoreText;
    public AudioRecorder recorder;

    private ColorMatchProblem currentColorMatchProblem;
    private EGameState currentState = EGameState.Ready;
    private float elapsedTime;
    private string result;
    private int score = 0;

    private bool switchFinished = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "0<color=black>개</color>";
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (countDown.isTimeUp && !switchFinished)
        {
            switchFinished = true;
            ChangeState(EGameState.Finished);
            return;
        }
        switch (currentState)
        {
            case EGameState.Ready:
                UpdateReady();
                break;
            case EGameState.Question:
                UpdateQuestion();
                break;
            case EGameState.Answered:
                UpdateAnswered();
                break;
            case EGameState.Finished:
                UpdateFinished();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateReady()
    {
        if (elapsedTime > 1)
        {
            ChangeState(EGameState.Question);
        }
    }

    private void UpdateQuestion()
    {
    }

    private void UpdateAnswered()
    {
        if (elapsedTime > 1)
        {
            correctOImage.enabled = false;
            WrongXImage.enabled = false;
            ChangeState(EGameState.Question);
        }
    }

    private void UpdateFinished()
    {
    }

    private void ChangeState(EGameState state)
    {
        elapsedTime = 0f;
        switch (state)
        {
            case EGameState.Ready:
                break;
            case EGameState.Question:
                //create question
                //apply ui
                currentColorMatchProblem = ColorProblemFactory.GetInstance().CreateProblem();
                colorMatchProblemText.color = currentColorMatchProblem.color.color;
                colorMatchProblemText.text = currentColorMatchProblem.text;
                StartCoroutine(CountDownAndAnswer());
                break;
            case EGameState.Answered:
                // check result right or wrong
                if (result == "정답")
                {
                    audioManager.AudioPlay(1);
                    score++;
                    scoreText.text = $"{score}" + "<color=black>개</color>";
                    correctOImage.enabled = true;
                }
                else
                {
                    audioManager.AudioPlay(2);
                    WrongXImage.enabled = true;
                }

                break;
            case EGameState.Finished:
                //결과 출력
                GameResultDataLoader.SaveResult(EGameType.ColorMatch, score, DateTimeOffset.Now.ToUnixTimeMilliseconds());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        currentState = state;
    }

    private IEnumerator CountDownAndAnswer()
    {
        // for (int i = 3; i >= 1; --i)
        // {
        //     countdownText.text = $"{i}";
        //     yield return new WaitForSeconds(1.0f);
        // }

        countdownText.text = "말하세요!";
        recorder.StartRecording();
        yield return new WaitForSeconds(2.0f);
        recorder.StopRecordingAndSave(Application.dataPath + "/test.wav");
        HttpManager.GetInstance().UploadFileByFormDataAsync(
            new HttpRequest<DownloadHandler>
            {
                Url = "http://192.168.1.19:2222/upload",
                RequestBody = Application.dataPath + "/test.wav",
                OnComplete = handler =>
                {
                    var res = JsonUtility.FromJson<ColorResponse>(handler.text);
                    countdownText.text = res.message;
                    print(res.message);
                    result = res.message;
                    ChangeState(EGameState.Answered);
                }
            },
            currentColorMatchProblem.color.text
        );
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