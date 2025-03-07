using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBrushTrigger : MonoBehaviour
{
    public GameObject brushPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HandImage") && !MediapipeManager.Instance.createBrushTrue)
        {
            MediapipeManager.Instance.createBrushTrue = true;
            GameObject hand = collision.gameObject;
            GameObject go = Instantiate(brushPrefab, hand.transform);
        }
    }
}
