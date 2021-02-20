using UnityEngine;

public static class Format
{
    public static string Duration(float seconds)
    {
        var isNegative = seconds < 0f;
        seconds = Mathf.Abs(seconds);

        int displayMinutes = Mathf.FloorToInt(seconds / 60);
        int displaySeconds = Mathf.RoundToInt(seconds % 60);
        return $"{(isNegative ? "-" : "")}{displayMinutes}:{displaySeconds.ToString("D2")}";
    }

    public static string Distance(float distance)
    {
        if (distance < 1000)
        {
            return $"{distance.ToString("0")}m";
        }
        else
        {
            return $"{(distance / 1000f).ToString("0.00")}km";
        }
    }
}
