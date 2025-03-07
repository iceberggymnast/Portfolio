using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointTextResfresh : MonoBehaviour
{
    public TMP_Text pointValue;

    private void Start()
    {
        PointRefresh();
    }

    [Button]
    public void PointRefresh()
    {
        if (PlayerInfo.instance != null)
        {
            pointValue.text = PlayerInfo.instance.point.ToString();
        }
        else
        {
            pointValue.text = "playerinfo 없음";
        }
    }

}
