using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NpcData : MonoBehaviour
{

    public int npcId;
    public string npcName;
    public int dialogue_group_id;

    // 퀘스트 데이터를 저장할 CSV 파일
    public TextAsset csvFile;

    // 대화 관련 데이터 저장
    public List<DialogueData> dialogueDatas = new List<DialogueData>();

    private void Start()
    {
        ReadCSV();
    }



    // 아이템 정보를 csv파일을 읽어 데이터 리스트에 저장해놓는다.
    void ReadCSV()
    {
        StringReader reader = new StringReader(csvFile.text);
        bool isFirstLine = true;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();

            // 첫 줄이 헤더인 경우 넘어감
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }

            // 쉼표로 구분된 데이터를 분리
            string[] values = line.Split(',');

            if (values.Length == 9) // CSV의 필드 수에 맞추어 수정
            {

                string type = values[0];
                int dialogue_group = int.Parse(values[1]);
                int sequence_id = int.Parse(values[2]);
                int npc_id = int.Parse(values[3]);
                string npc_name = values[4];
                string text = values[5];
                int event_code = int.Parse(values[6]);
                int next_sequence_id = int.Parse(values[7]);
                string condition = values[8];


                // ItemInfo 객체 생성 및 값 설정
                DialogueData dialogedata = new DialogueData();
                dialogedata.DataAdd(type, dialogue_group, sequence_id, npc_id, npc_name, text, event_code, next_sequence_id, condition);
                // 리스트에 추가
                dialogueDatas.Add(dialogedata);
            }
        }
    }

}
