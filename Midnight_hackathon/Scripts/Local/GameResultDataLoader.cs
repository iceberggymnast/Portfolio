using System.IO;
using System.Linq;
using UnityEngine;

public enum EGameType
{
    ClockProblem = 0,
    ColorMatch = 1,
}

public class GameRecord
{
    public EGameType type;
    public int score;
    public long timestamp;

    public static GameRecord FromString(string str)
    {
        var data = str.Split(',');
        if (data.Length != 3)
        {
            return null;
        }

        var type = (EGameType)int.Parse(data[0]);
        var score = int.Parse(data[1]);
        var timestamp = long.Parse(data[2]);

        return new GameRecord
        {
            type = type,
            score = score,
            timestamp = timestamp,
        };
    }
}

public struct GameResultDataLoader
{
    public static void SaveResult(EGameType gameType, int score, long timestamp)
    {
        var path = Application.dataPath + "/result.csv";

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            var result = $"{(int)gameType},{score},{timestamp}";
            writer.WriteLine(result);
        }
    }

    public static GameRecord GetLastGameRecord(EGameType gameType)
    {
        var path = Application.dataPath + "/result.csv";
        if (!File.Exists(path))
        {
            return null;
        }

        var records = File.ReadAllLines(path)
            .Select(GameRecord.FromString)
            .Where(record => record != null && record.type == gameType)
            .OrderBy(record => record.timestamp)
            .ToArray();

        return records.Length == 0 ? null : records.Last();
    }
}