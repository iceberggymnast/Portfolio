using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipBubble : MonoBehaviour
{
    public bool isFirstAction = false;

    public string objectNameText;
    public string introductionText;

    public TextMeshProUGUI objectName;
    public TextMeshProUGUI introduction;

    void Start()
    {
        objectName.text = objectNameText;
        introduction.text = introductionText;
    }

    private void Update()
    {
        isActionCheck();
    }

    public void isActionCheck()
    {
        if (isFirstAction)
        {
            //print("상호작용 되어 말풍선 삭제 됨");
            gameObject.SetActive(false);
        }
        else
        {
            //print("아직 상호작용 안됐으니까 계속 존재하기");
        }
    }
}
