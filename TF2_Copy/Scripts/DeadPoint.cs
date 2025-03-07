using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadPoint : MonoBehaviour
{

    public Image Image;
    float time;
    bool deadzone;
    GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (deadzone)
        {
            Color color = Image.color;
            color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 20.0f);
            Image.color = color;
            time += Time.deltaTime;

            if (time > 1)
            {
                player.transform.position = new Vector3(146.9013f, 47.9541f, -132.8459f);
                time = 0;
                color.a = 0;
                Image.color = color;
                deadzone = false;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            deadzone = true;
        }
    }
}
