using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueInfo
{
    public string dialogueID;
    public string name;
    public string conversation;
    public bool isFix;
    public string eventType;
    public int csvDataIndex;
}

public class CSVManager : MonoBehaviourPunCallbacks
{
    public static CSVManager instance;
    public GameObject ownSpeechBubble;

    public SoundManger soundManger;

    public static CSVManager Get()
    {
        if (instance == null)
        {
            GameObject go = new GameObject();
            go.name = nameof(CSVManager);
            go.AddComponent<CSVManager>();
        }
        return instance;
    }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        ownSpeechBubble = GameObject.FindWithTag("OwnSpeehBubble");


        soundManger.BGMSetting(0);
    }

    public Dictionary<string, List<DialogueInfo>> Parse(string fileName)
    {
        string path = Application.streamingAssetsPath + "/CSV/" + fileName + ".csv";
        string stringData = File.ReadAllText(path);

        // 개행 문자로 줄을 나눔
        string[] lines = stringData.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
        Dictionary<string, List<DialogueInfo>> dialogueDictionary = new Dictionary<string, List<DialogueInfo>>();

        // 첫 번째 줄은 헤더이므로 건너뜀, 따라서 i = 1 부터 시작
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;  // 빈 줄은 건너뜀
            }

            // ','를 기준으로 값을 분할
            string[] value = lines[i].Split(',');

            // 값이 기대한 수(3개)가 아닐 경우 건너뜀
            if (value.Length < 5)
            {
                Debug.LogWarning($"Invalid line format at line {i}: {lines[i]}");
                continue;
            }

            // DialogueInfo 객체 생성 및 값 할당
            DialogueInfo info = new DialogueInfo();
            info.dialogueID = value[0].Trim();
            info.name = value[1].Trim();

            if (value[2].Contains("@"))
            {
                info.conversation = value[2].Replace("@", ",").Trim();  // '@'를 ','로 변환
            }
            else info.conversation = value[2].Trim();

            info.isFix = value[3] == "1";
            info.eventType = value[4].Trim();

            // dialogueID에 따라 Dictionary에 추가
            if (!dialogueDictionary.ContainsKey(info.dialogueID))
            {
                dialogueDictionary[info.dialogueID] = new List<DialogueInfo>();
            }

            dialogueDictionary[info.dialogueID].Add(info);
        }

        return dialogueDictionary;
    }

}
