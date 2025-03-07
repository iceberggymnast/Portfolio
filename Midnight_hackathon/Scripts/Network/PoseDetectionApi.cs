using System;
using System.Collections;
using System.Collections.Generic;

public class PoseDetectionApi
{
    private static string url = "http://192.168.1.19:7777/";
    
    public static void GetCurrentPose(Action<List<PoseInfo>> onComplete)
    {
        var info = new HttpRequest<CurrentPoseList>()
        {
            Url = url+"media",
            OnComplete = (response) =>
            {
                onComplete(response.data);
            }
        };
        
        HttpManager.GetInstance().GetJsonAsync(info);
    }

    public static void GetCurrentPoseDirectingTime(Action<PoseInfo> onComplete)
    {
        var info = new HttpRequest<PoseInfo>()
        {
            Url = url + "/time",
            OnComplete = onComplete
        };
        
        HttpManager.GetInstance().GetJsonAsync(info);
    }
    
}