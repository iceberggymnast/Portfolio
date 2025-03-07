using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueData
{
    public string type;
    public int dialogue_group_id;
    public int sequence_id;
    public int npc_id;
    public string npc_name;
    public string text;
    public int event_code;
    public int next_sequence_id;
    public string condition;


    public void DataAdd(string data1, int data2, int data3, int data4, string data5, string data6, int data7, int data8, string data9)
    {
    type = data1;
     dialogue_group_id = data2;
     sequence_id = data3;
     npc_id = data4;
     npc_name = data5;
     data6 = data6.Replace("@", ",");
     data6 = data6.Replace("<br/>", "<br>");
     text = data6;
     event_code = data7;
     next_sequence_id = data8;
     condition = data9;
    }

}
