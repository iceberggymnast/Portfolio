using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startblackfade : MonoBehaviour
{
    //�����Ҷ� ������ ȭ�鿡�� �ڿ������� ���̵� �ƿ� �Ǵ� ȿ��
    Image blackimg;
    float alpha = 1;
    float timer;
    float speed = 0.8f;
    public bool fading;

    void Start()
    {
        blackimg = gameObject.GetComponent<Image>();
    }


    void Update()
    {
        fading = PlayerManager.pm.blackon;
        print(alpha);

        if (PlayerManager.pm.hp <= 0)
        {
            if (PlayerManager.pm.life != 0)
            {
                timer += Time.deltaTime;
                alpha = Mathf.Lerp(0, 1, (speed * timer) - 0.5f);
                blackimg.color = new Color(0, 0, 0, alpha);
            }
        }
        else
        {
            if (!fading)
            {
                timer = 0;
                alpha -= Time.deltaTime;
                blackimg.color = new Color(0, 0, 0, alpha + 0.5f);
            }
            else
            {
                    timer += Time.deltaTime;
                    alpha = Mathf.Lerp(0, 1, (speed * timer * 10));
                    blackimg.color = new Color(0, 0, 0, alpha);
            }
        }
    }
}
