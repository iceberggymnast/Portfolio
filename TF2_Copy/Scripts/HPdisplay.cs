using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPdisplay : MonoBehaviour
{
    public TMP_Text hp;
    public PlayerState state;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = state.playerHP.ToString();
    }
}
