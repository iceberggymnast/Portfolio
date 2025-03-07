using System;
using UnityEngine;

public class TigerPunchList : MonoBehaviour
{
    [Serializable]
    public class TigerPunch
    {
        public string result;
    }

    private TigerPunch tigerPunch;

    private void Start()
    {
        tigerPunch = new TigerPunch();
        tigerPunch.result = "test";
    }

    public void CreateFromJSON(string jsonString)
    {
        tigerPunch = JsonUtility.FromJson<TigerPunch>(jsonString);
        if (tigerPunch.result != "")
        {
            print(tigerPunch.result);
            if (tigerPunch.result == "LEFT")
            {
                print("Left Punch");
            }
            else if (tigerPunch.result == "RIGHT")
            {
                print("Right Punch");
            }
        }
    }

    // tigerPunch.result를 반환하는 공개 메서드 추가
    public string GetTigerPunchResult()
    {
        return tigerPunch.result;
    }
}
