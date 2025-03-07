using Models;

public class ClockProblemFactory
{
    private static ClockProblemFactory _instance;
    
    public static ClockProblemFactory GetInstance()
    {
        if (_instance == null)
        {
            _instance = new ClockProblemFactory();
        }

        return _instance;
    }
    
    public ClockProblem CreateProblem()
    {
        var hour = UnityEngine.Random.Range(1, 5) * 3;
        var min = UnityEngine.Random.Range(0, 4) * 15;
        return new ClockProblem()
        {
            hour = hour,
            minute = min
        };
    }
}