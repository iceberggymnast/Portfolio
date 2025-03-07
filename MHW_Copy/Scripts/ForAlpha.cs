using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForAlpha : MonoBehaviour
{
    public GameObject popupui;
    public int mapnumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeUI()
    {
        popupui.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Changemap()
    {
        gameObject.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(mapnumber);
    }

}
