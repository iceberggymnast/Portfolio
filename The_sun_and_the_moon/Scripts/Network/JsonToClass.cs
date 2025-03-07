using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate string JsonReturn();

public class JsonToClass : MonoBehaviour
{
    public bool isString;

    [Serializable]
    public class ResultInt
    {
        public int result;
    }

    [Serializable]
    public class ResultString
    {
        public string result;
    }

    public ResultString resultString;
    public ResultInt resultInt;

    string jsonReturn;
    public JsonReturn jsonReturnDel;

    void Start()
    {
        if (isString)
        {
            resultString = new ResultString();
        }
        else
        {
            resultInt = new ResultInt();
        }
    }

    void Update()
    {
        CreateFromJSON();
    }

    public void CreateFromJSON()
    {
        jsonReturn = jsonReturnDel();

        if (isString)
        {
            resultString = JsonUtility.FromJson<ResultString>(jsonReturn);
        }
        else
        {
            resultInt = JsonUtility.FromJson<ResultInt>(jsonReturn);
        }

    }

}
