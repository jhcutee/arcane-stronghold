
using UnityEngine;

public static class ColorUtils
{
    public static Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        return Color.white; // Default to white if parsing fails
    }
}
