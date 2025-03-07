using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSelect : MonoBehaviour
{
    public List<RectTransform> bottons = new List<RectTransform>();
    public List<Vector3> buttonPos = new List<Vector3>();
    public float speed = 20.0f;
    public int activatedButton = 0;

    void Update()
    {
        if (bottons.Count == 2)
        {
            if (activatedButton == 0)
            {
                bottons[0].anchoredPosition = Vector3.Lerp(bottons[0].anchoredPosition, buttonPos[0], Time.unscaledDeltaTime * speed);
                bottons[0].transform.GetChild(1).gameObject.SetActive(true);
                bottons[1].anchoredPosition = Vector3.Lerp(bottons[1].anchoredPosition, buttonPos[1] + new Vector3(0, -31.2f, 0), Time.unscaledDeltaTime * speed);
                bottons[1].transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (activatedButton == 1)
            {
                bottons[0].anchoredPosition = Vector3.Lerp(bottons[0].anchoredPosition, buttonPos[0], Time.unscaledDeltaTime * speed);
                bottons[0].transform.GetChild(1).gameObject.SetActive(false);
                bottons[1].anchoredPosition = Vector3.Lerp(bottons[1].anchoredPosition, buttonPos[1], Time.unscaledDeltaTime * speed);
                bottons[1].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        else if (bottons.Count == 3)
        {
            if (activatedButton == 0)
            {
                bottons[0].anchoredPosition = Vector3.Lerp(bottons[0].anchoredPosition, buttonPos[0], Time.unscaledDeltaTime * speed);
                bottons[0].transform.GetChild(1).gameObject.SetActive(true);
                bottons[1].anchoredPosition = Vector3.Lerp(bottons[1].anchoredPosition, buttonPos[1] + new Vector3(0, -31.2f, 0), Time.unscaledDeltaTime * speed);
                bottons[1].transform.GetChild(1).gameObject.SetActive(false);
                bottons[2].anchoredPosition = Vector3.Lerp(bottons[2].anchoredPosition, buttonPos[2] + new Vector3(0, -31.2f, 0), Time.unscaledDeltaTime * speed);
                bottons[2].transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (activatedButton == 1)
            {
                bottons[0].anchoredPosition = Vector3.Lerp(bottons[0].anchoredPosition, buttonPos[0], Time.unscaledDeltaTime * speed);
                bottons[0].transform.GetChild(1).gameObject.SetActive(false);
                bottons[1].anchoredPosition = Vector3.Lerp(bottons[1].anchoredPosition, buttonPos[1], Time.unscaledDeltaTime * speed);
                bottons[1].transform.GetChild(1).gameObject.SetActive(true);
                bottons[2].anchoredPosition = Vector3.Lerp(bottons[2].anchoredPosition, buttonPos[2] + new Vector3(0, -31.2f, 0), Time.unscaledDeltaTime * speed);
                bottons[2].transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (activatedButton == 2)
            {
                bottons[0].anchoredPosition = Vector3.Lerp(bottons[0].anchoredPosition, buttonPos[0], Time.unscaledDeltaTime * speed);
                bottons[0].transform.GetChild(1).gameObject.SetActive(false);
                bottons[1].anchoredPosition = Vector3.Lerp(bottons[1].anchoredPosition, buttonPos[1], Time.unscaledDeltaTime * speed);
                bottons[1].transform.GetChild(1).gameObject.SetActive(false);
                bottons[2].anchoredPosition = Vector3.Lerp(bottons[2].anchoredPosition, buttonPos[2], Time.unscaledDeltaTime * speed);
                bottons[2].transform.GetChild(1).gameObject.SetActive(true);
            }

        }

    }

    // 버튼 호버시 호버된 값 전달
    public void ButtonHilghted (int buttonNumber)
    {
        activatedButton = buttonNumber;
    }

    public AudioSource sound;
    public void hoversoundplay()
    {
        sound.Play();
    }

    public void Loadscene (int senceNumber)
    {
        SceneManager.LoadScene (senceNumber);
    }

    public void GameQuit ()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit(); // 어플리케이션 종료
        #endif
    }

    public void timescale()
    {
        Time.timeScale = 1.0f;
    }
}
