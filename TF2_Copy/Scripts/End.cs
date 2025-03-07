using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public CharacterController cc;
    public Image img;
    public float time;

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() != null)
        {
            cc = other.gameObject.GetComponent<CharacterController>();
            GetComponent<AudioSource>().Play();
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (cc != null)
        {
            Color color = img.color;
            color.a = Mathf.Lerp(color.a, 1, Time.deltaTime * 20.0f);
            img.color = color;
            time += Time.deltaTime;

            if (time > 5)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(1);
            }
        }
    }


}
