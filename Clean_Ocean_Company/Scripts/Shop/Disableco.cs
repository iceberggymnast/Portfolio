using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disableco : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(disalbe());
    }

    IEnumerator disalbe()
    {
        yield return new WaitForSeconds(2.0f);

        this.gameObject.SetActive(false);
    }
}
