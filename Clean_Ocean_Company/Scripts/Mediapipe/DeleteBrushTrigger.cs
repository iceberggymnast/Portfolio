using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteBrushTrigger : MonoBehaviour
{
    public GameObject brushPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("LeftBrush") && MediapipeManager.Instance.createBrushTrue)
        {
            MediapipeManager.Instance.createBrushTrue = false;
            Destroy(collision.gameObject);
        }
    }
}
