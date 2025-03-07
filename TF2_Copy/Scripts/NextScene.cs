using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public float timer;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 12)
        {
            SceneManager.LoadScene(1);
        }
    }
}
