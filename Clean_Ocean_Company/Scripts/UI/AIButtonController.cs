using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AIButtonController : MonoBehaviour
{
    public List<Button> aiButtons;

    ChatbotVerticalLayout chatbot;


    // Start is called before the first frame update
    void Awake()
    {
        chatbot = GameObject.Find("Canvas_Chatbot").GetComponent<ChatbotVerticalLayout>();

        foreach (var button in aiButtons)
        {
            TMP_Text buttonText = button.transform.GetChild(0).GetComponent<TMP_Text>();
            button.onClick.AddListener(() => {
                if (button.gameObject.name == "DailyAskButton")
                {
                    chatbot.OnSubmitButton("오늘의 예상 퀴즈는?");
                }
                else
                {
                    chatbot.OnSubmitButton(buttonText.text);
                }
            });
        }
    }

    void SetString(string value)
    {
        value = "";
    }

    void ChatbotIsStart(bool value)
    {
        chatbot.isStartChat = value;
    }
}
