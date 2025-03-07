using Models;
using UnityEngine;

public class ColorProblemFactory : MonoBehaviour
{
    public ColorText[] colors = new[]
    {
        new ColorText { color = Color.red, text = "빨간색" },
        new ColorText { color = Color.green, text = "초록색" },
        new ColorText { color = Color.blue, text = "파란색" },
        new ColorText { color = Color.yellow, text = "노란색" },
    };

    public string[] texts = new[]
    {
        "빨간색",
        "초록색",
        "파란색",
        "노란색",
    };

    private static ColorProblemFactory _instance;

    public static ColorProblemFactory GetInstance()
    {
        if (_instance == null)
        {
            var go = new GameObject();
            go.name = "ColorProblemFactory";
            var t = go.AddComponent<ColorProblemFactory>();
            _instance = t;
        }

        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public ColorMatchProblem CreateProblem()
    {
        var idx = Random.Range(0, colors.Length);
        var tIdx = Random.Range(0, texts.Length);
        return new ColorMatchProblem()
        {
            text = texts[tIdx],
            color = colors[idx]
        };
    }
}