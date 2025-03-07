using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    MediapipeMoveTrigger mediapipeMoveTrigger;


    public enum SideType
    {
        left,
        right
    }

    public SideType side;

    private void Awake()
    {
        mediapipeMoveTrigger = GameObject.Find("MoveParent").GetComponent<MediapipeMoveTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HandImage"))
        {
            mediapipeMoveTrigger.SetCheckPos(System.Convert.ToInt32(side));
        }
    }
}
