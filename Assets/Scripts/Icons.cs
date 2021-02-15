using System.Collections.Generic;
using UnityEngine;

public static class Icons
{
    public static Dictionary<KeyCode, int> KeyIconMappings = new Dictionary<KeyCode, int>
    { { KeyCode.UpArrow, 0 },
        { KeyCode.DownArrow, 1 },
        { KeyCode.LeftArrow, 2 },
        { KeyCode.RightArrow, 3 },
        { KeyCode.Z, 4 },
        { KeyCode.X, 5 },
        { KeyCode.C, 6 },
        { KeyCode.W, 7 },
        { KeyCode.S, 8 },
        { KeyCode.A, 9 },
        { KeyCode.D, 10 },
        { KeyCode.Comma, 11 },
        { KeyCode.Period, 12 },
        { KeyCode.Slash, 13 },
    };

    public static string IconText(KeyCode key)
    {
        int index = KeyIconMappings[key];
        return $"<sprite index={index}>";
    }

    public static string HorizontalAxis(InputScheme scheme)
    {
        return $"{IconText(scheme.left)}{IconText(scheme.right)}";
    }

    public static string VerticalAxis(InputScheme scheme)
    {
        return $"{IconText(scheme.up)}{IconText(scheme.down)}";
    }
}
