using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeDetectionUI : MonoBehaviour
{
    public GameObject player;
    public GameObject grenadeUI;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 10.0f);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.layer == 12)
                {
                    if (colliders[i].gameObject.GetComponent<GrenadeScript>().playerFind == false)
                    {
                        colliders[i].gameObject.GetComponent<GrenadeScript>().playerFind = true;
                        GameObject go = Instantiate(grenadeUI, transform);
                        go.GetComponent<GrenadeDirUI>().InputGrenade(colliders[i].gameObject);
                    }
                }
            }
        }
    }

    public GameObject hitdir;
    public IEnumerator Hit(Vector3 hitPos, float timerDuration)
    {
        GameObject go = Instantiate(hitdir, transform);
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        Image image = go.GetComponent<Image>();
        float timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime * 20.0f;
            Vector3 dir = hitPos - player.transform.position;
            dir = Quaternion.Euler(0, 0 ,Camera.main.transform.eulerAngles.y) * dir;
            //float zRotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            //zRotation = (zRotation + 360) % 360;

            float zRotation = Quaternion.FromToRotation(Vector3.up, dir).eulerAngles.z;
            zRotation = (zRotation + 360) % 360;
            rectTransform.eulerAngles = new Vector3(0, 0, zRotation);
            print(zRotation);
            yield return null;
        }
    }
    


}
