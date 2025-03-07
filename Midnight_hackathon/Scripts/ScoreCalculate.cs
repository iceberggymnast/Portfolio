  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCalculate : MonoBehaviour
{
    public Text averageScore;
    public Text averageScore2;
    public BrainSocreDisplay gameresult1;
    public BrainSocreDisplay gameresult2;
    void Start()
    {

    }

    void Update()
    {
        gameresult1.socoreValue = MainmenuSkip.Instance.game1.score * 9 + 10;
        gameresult2.socoreValue = MainmenuSkip.Instance.game2.score * 9 + 10;
        int averageScoreV = ((gameresult1.socoreValue + gameresult2.socoreValue) / 2);
        averageScore.text = "  ³ú °Ç°­µµ" + System.Environment.NewLine + "  " + averageScoreV.ToString() + "%";
        averageScore2.text = "  ³ú °Ç°­µµ" + System.Environment.NewLine + "  " + averageScoreV.ToString() + "%";
    }
}
