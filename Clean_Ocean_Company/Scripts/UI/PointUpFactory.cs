using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointUpFactory : MonoBehaviour
{
    public GameObject pointObj;
    public GameObject pointObjminus;

    [Button]
    public void AddPoint(Vector3 startpos, int value)
    {
        if (value > 0)
        {
            GameObject go = Instantiate(pointObj);
            go.transform.SetParent(this.transform);
            PointUpScripts cs = go.GetComponent<PointUpScripts>();
            cs.Pointcoroutine(startpos, new Vector3 (700, 430, 0), value);
        }
        else if (value < 0)
        {
            GameObject go = Instantiate(pointObjminus);
            go.transform.SetParent(this.transform);
            PointDownScripts cs = go.GetComponent<PointDownScripts>();
            cs.SetValue(value);
            PointTextResfresh resfresh = GameObject.FindFirstObjectByType<PointTextResfresh>();
            resfresh.PointRefresh();
        }
            SoundManger soundManger = GameObject.FindObjectOfType<SoundManger>();
            if (soundManger != null)
            {
                soundManger.UISFXPlayRandom(4, 7);
            }
    }
}
