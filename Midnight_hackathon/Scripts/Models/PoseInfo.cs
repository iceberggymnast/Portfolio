using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PoseInfo
{
    public float lhX;
    public float lhY;
    public float rhX;
    public float rhY;

    public int hour;
    public int minute;

    public static PoseInfo FromUdpResponse(string s)
    {
        var arr = s.Split(',');
        // Debug.Log("arr.Length: " + arr.Length);
        if (arr.Length < 8) return null;

        var time = arr[7];
        var t = time.Split(":");
        return new PoseInfo()
        {
            lhX = Convert.ToInt32(arr[1]),
            lhY = Convert.ToInt32(arr[2]),
            rhX = Convert.ToInt32(arr[4]),
            rhY = Convert.ToInt32(arr[5]),
            
            hour = Convert.ToInt32(t[0]),
            minute = Convert.ToInt32(t[1])
        };
    }
}