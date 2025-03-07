using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Btn_interactionTurnOn : MonoBehaviour
{
    public float time = 1.0f;
    void Start()
    {
        StartCoroutine(Interaction());
    }

    IEnumerator Interaction()
    {
        yield return new WaitForSeconds(time);
        Button btn = GetComponent<Button>();
        btn.interactable = true;
    }
}
