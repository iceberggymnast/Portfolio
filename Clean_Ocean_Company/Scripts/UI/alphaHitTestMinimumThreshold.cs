using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class alphaHitTestMinimumThreshold : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image imgButton = GetComponent<Image>();
        imgButton.alphaHitTestMinimumThreshold = 0.1f;
    }

}
