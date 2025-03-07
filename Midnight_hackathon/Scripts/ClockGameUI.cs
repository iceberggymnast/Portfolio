using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClockGameUI : MonoBehaviour
{
    public AudioManager audioManager;
    public ClockGameManager manager;
    public ColorMatchGameManager colorMatchGameManager;
    public CountDown countDown;
    public int gamenumber = 0;
    void Start()
    {
        if (gamenumber == 0)
        {
            manager = GameObject.Find("GM").GetComponent<ClockGameManager>();
        }
        if (gamenumber == 1)
        {
            colorMatchGameManager = GameObject.Find("ColorMatchGM").GetComponent<ColorMatchGameManager>();
        }
        StartCoroutine(Standby(0));
    }

    void Update()
    {
        if (gamenumber == 0)
        {
            if (manager.CurrentState == ClockGameManager.EGameState.End)
            {
                if (!Canvas_Result.activeSelf)
                {
                    Canvas_Result.SetActive(true);
                }
            }
        }
        else if (gamenumber == 1)
        {
            if (colorMatchGameManager.CurrentState == ColorMatchGameManager.EGameState.Finished && gamenumber == 1)
            {
                if (!Canvas_Result.activeSelf)
                {
                    Canvas_Result.SetActive(true);
                }
            }
        }
    }

    // 씬 진입 시 준비화면
    public GameObject readyBackground;
    public GameObject readyText;
    public GameObject goText;
    public Image blurimg;
    IEnumerator Standby(float time)
    {
        RectTransform rectTransformBackground = readyBackground.GetComponent<RectTransform>();
        RectTransform rectTransformReadyText = readyText.GetComponent<RectTransform>();
        RectTransform rectTransformGoText = goText.GetComponent<RectTransform>();
        // Ready..
        audioManager.AudioPlay(0);
        while (time < 3)
        {
            time += Time.deltaTime;
            rectTransformBackground.localScale = Vector3.Lerp(rectTransformBackground.localScale, Vector3.one, Time.deltaTime * 10.0f);
            rectTransformReadyText.localPosition = Vector3.Lerp(rectTransformReadyText.localPosition, new Vector3(0, -53, 0), Time.deltaTime * 10.0f);
            yield return null;
        }

        // Go..!
        while (time < 4)
        {
            time += Time.deltaTime;
            rectTransformReadyText.localPosition = Vector3.Lerp(rectTransformReadyText.localPosition, new Vector3(-1258, -53, 0), Time.deltaTime * 10.0f);
            rectTransformGoText.localPosition = Vector3.Lerp(rectTransformGoText.localPosition, new Vector3(0, -53, 0), Time.deltaTime * 10.0f);
            yield return null;
        }

        // gone..
        while (time < 5)
        {
            time += Time.deltaTime;
            rectTransformBackground.localScale = Vector3.Lerp(rectTransformBackground.localScale, new Vector3(1, 0, 1), Time.deltaTime * 10.0f);
            rectTransformGoText.localPosition = Vector3.Lerp(rectTransformGoText.localPosition, new Vector3(-1258, -53, 0), Time.deltaTime * 10.0f);
            Color blurimgColor = blurimg.color;
            blurimgColor.a = Mathf.Lerp(blurimg.color.a, 0, Time.deltaTime * 100f);
            blurimg.color = blurimgColor;
            yield return null;
        }

        if (gamenumber == 0)
        {
            manager.enabled = true;
        }
        else if (gamenumber == 1)
        {
            colorMatchGameManager.enabled = true;
            countDown.enabled = true;
        }
    }

    public GameObject Canvas_Result;
    public Image resultblurimg;
    public GameObject resultbackground;
    public GameObject doneText;

    public GameObject scoreResultText;
    public GameObject button_CognitiveAbility_Result;

    public Text scoreText;
    public BrainSocreDisplay brainSocreDisplay;

    public GameObject exitbutton;


    // 결과창 팝업
    public IEnumerator Result(float time)
    {
        int score = 0;
        if (gamenumber == 0)
        {
             score = manager.Score;
        }
        else if (gamenumber == 1)
        {
            score = colorMatchGameManager.Score;
        }
        int percent = score * 9 + 10;

        RectTransform resultbackgroundRect = resultbackground.GetComponent<RectTransform>();
        RectTransform scoreResultTextRect = scoreResultText.GetComponent<RectTransform>();
        RectTransform button_CognitiveAbility_ResultRect = button_CognitiveAbility_Result.GetComponent<RectTransform>();
        RectTransform doneTextRect = doneText.GetComponent<RectTransform>();
        RectTransform exitbuttonRect = exitbutton.GetComponent<RectTransform>();

        while (time < 4)
        {
            time += Time.deltaTime;
            Color blurimgColor = resultblurimg.color;
            blurimgColor.a = Mathf.Lerp(blurimg.color.a, 1, Time.deltaTime * 10.0f);
            resultblurimg.color = blurimgColor;
            resultbackgroundRect.localScale = Vector3.Lerp(resultbackgroundRect.localScale, Vector3.one, Time.deltaTime * 10.0f);
            doneTextRect.localPosition = Vector3.Lerp(doneTextRect.localPosition, Vector3.zero, Time.deltaTime * 10.0f);
            yield return null;
        }

        while(time < 4.5f)
        {
            time += Time.deltaTime;
            doneTextRect.localPosition = Vector3.Lerp(doneTextRect.localPosition, new Vector3(-1783, 0,0), Time.deltaTime * 10.0f);
            yield return null;
        }

        while (time < 5)
        {
            time += Time.deltaTime;

            scoreResultTextRect.localPosition = Vector3.Lerp(scoreResultTextRect.localPosition, new Vector3(-628.5f, -58.75001f, 0), Time.deltaTime * 10.0f);
            button_CognitiveAbility_ResultRect.localPosition = Vector3.Lerp(button_CognitiveAbility_ResultRect.localPosition, new Vector3(99, 197, 0), Time.deltaTime * 10.0f);
            exitbuttonRect.localPosition = Vector3.Lerp(exitbuttonRect.localPosition, new Vector3(-784, 435, 0), Time.deltaTime * 10.0f);
            yield return null;
        }

        while (time < 10)
        {
            time += Time.deltaTime;
            for (int i = 0; i < score + 1; i++)
            {
                if(gamenumber == 1)
                { 
                scoreText.text = i.ToString() + "<color=black>개</color>";
                }
                else if (gamenumber == 0)
                {
                scoreText.text = i.ToString() + "<color=black>/10</color>";
                }
                brainSocreDisplay.socoreValue = percent * i / score;
                yield return new WaitForSeconds(0.1f);
            }
            break;
        }


    }

    public void MapLoad(int mapnumber)
    {
        MainmenuSkip.Instance.game1 = GameResultDataLoader.GetLastGameRecord(gameType: 0);
        MainmenuSkip.Instance.game2 = GameResultDataLoader.GetLastGameRecord(gameType: EGameType.ColorMatch);
        SceneManager.LoadScene(mapnumber);
    }
}
