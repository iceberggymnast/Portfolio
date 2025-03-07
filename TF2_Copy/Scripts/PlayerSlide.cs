using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSlide : MonoBehaviour
{
    public float mouseX;
    public float mouseY;
    public float rotSpeed;
    public PlayerFire playerFIre;
    public PlayerMove PlayerMove;

    private void Update()
    {
        RotateType();
        Recoil();
        Runing();
        Hurt();
    }

    void RotateType()
    {
        if (Time.timeScale != 0)
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            transform.localEulerAngles += new Vector3(mouseY * 0.5f, -mouseX * 0.5f, 0) + recoilcom; 
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(Vector3.zero), rotSpeed * Time.deltaTime);
        }
    }

    public bool recoil;
    public float recoiltime;
    public float xrecoil;
    public float yrecoil;
    public float zrecoil;
    public float zrecoillerp;
    public Vector3 recoilcom;

    public GameObject bulletshell;
    public Transform bulletShellPos;
    void Recoil()
    {
        if (playerFIre.currentBullet > 0 && Time.timeScale != 0 && Input.GetKeyDown(KeyCode.Mouse0))
        { 
            recoil = true;
            recoiltime = 0;
            xrecoil = Random.Range(-0.08f, - 0.1f);
            yrecoil = Random.Range(-0.1f, 0.1f);
            zrecoil = Random.Range(-3f, 3f);

            GameObject bulletShell = Instantiate(bulletshell);
            bulletShell.transform.position = bulletShellPos.position;
            bulletShell.transform.rotation = Quaternion.LookRotation(bulletShellPos.forward);
        }


        if (recoil && recoiltime < 0.01f)
        {
            recoiltime += Time.deltaTime;
            recoilcom = Vector3.Lerp(Vector3.zero, new Vector3(recoilcom.x + xrecoil, recoilcom.y + yrecoil, 0), recoiltime * 100.0f);
            zrecoillerp = Mathf.Lerp(0, zrecoil, recoiltime * 100.0f);
        }
        else if (recoiltime >= 0.01f && recoil)
        {
            recoil = false;
            recoiltime = 0;
        }
        else if (!recoil)
        {
            recoiltime += Time.deltaTime;
            recoilcom = Vector3.Lerp(recoilcom, Vector3.zero, recoiltime);
            zrecoillerp = Mathf.Lerp(zrecoil, 0, recoiltime * 100.0f);
        }
    }

    public bool hurt;
    public bool hurtoutput;
    public float hurttime;
    public float xhurt;
    public float yhurt;
    public float zhurt;
    public float zhurtLerp;
 
    public Vector3 hurtdir;
    void Hurt()
    {
        if (hurt)
        {
            hurt = false;
            hurtoutput = true;
            hurttime = 0;
            xhurt = Random.Range(-0.1f, -0.2f);
            yhurt = Random.Range(-0.1f, 0.1f);
            zhurt = Random.Range(-5f, 5f);
        }

        if (hurtoutput && hurttime < 0.01f)
        {
            hurttime += Time.deltaTime;
            hurtdir = Vector3.Lerp(Vector3.zero, new Vector3(hurtdir.x + xhurt, hurtdir.y + yhurt, 0), hurttime * 100.0f);
            zhurtLerp = Mathf.Lerp(0, zhurt, hurttime * 100.0f);

        }
        else if (hurttime >= 0.01f && hurtoutput)
        {
            hurtoutput = false;
            hurttime = 0;
        }
        else if (!hurtoutput)
        {
            hurttime += Time.deltaTime;
            hurtdir = Vector3.Lerp(hurtdir, Vector3.zero, hurttime);
            zhurtLerp = Mathf.Lerp(zhurt, 0, hurttime * 100.0f);

        }
    }

    public float runLerp;
    float runtimer;
    void Runing()
    {
        if (PlayerMove.isRun && PlayerMove.isGround)
        {
            runtimer += Time.deltaTime;
            if(runtimer < 0.4f)
            {
                runLerp = Mathf.Lerp(runLerp, 3, Time.deltaTime * 5);
            }
            else if (runtimer < 0.6f)
            {
                runLerp = Mathf.Lerp(runLerp, 0, Time.deltaTime * 5);
            }
            else if (runtimer < 0.8f)
            {
                runLerp = Mathf.Lerp(runLerp, -3, Time.deltaTime * 5);
            }
            else if (runtimer < 1)
            {
                runtimer = 0;
                runLerp = Mathf.Lerp(runLerp, 0, Time.deltaTime * 5);
            }
        }
        else
        {
            runtimer = 0;
            runLerp = Mathf.Lerp(runLerp, 0, Time.deltaTime * 5);
        }
    }
}
