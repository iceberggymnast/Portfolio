using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeDirUI : MonoBehaviour
{
    public GameObject grenade;
    GameObject player;
    public GameObject uiOBJ;
    Vector3 playerPos;
    Vector3 grenadePos;
    float howLeth;

    void Start()
    {
        player = GameObject.Find("Player");
        uiOBJ = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        print(howLeth);
        if (grenade != null)
        {
            howLeth = (grenadePos - playerPos).magnitude;
        }

        if (grenade != null && howLeth <= 10)
        {
            uiOBJ.SetActive(true);
            grenadePos = grenade.transform.position;
            playerPos = player.transform.position;
            Vector3 dir = (grenadePos - playerPos).normalized;
            dir = Quaternion.Inverse(player.transform.rotation) * dir;
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else if (grenade != null && howLeth > 10)
        {
            uiOBJ.SetActive(false);
            grenadePos = grenade.transform.position;
            playerPos = player.transform.position;
            Vector3 dir = (grenadePos - playerPos).normalized;
            dir = Quaternion.Inverse(player.transform.rotation) * dir;
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else if (grenade == null || grenade.GetComponent<GrenadeScript>().explosion)
        {
            Destroy(gameObject);
        }
    }

    public void InputGrenade(GameObject grenadetaget)
    {
        grenade = grenadetaget;
    }
}
