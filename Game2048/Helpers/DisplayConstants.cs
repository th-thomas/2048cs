using Microsoft.Xna.Framework;

namespace Game2048.Helpers;

public static class DisplayConstants
{
    public record CellColor(Color BackgroundColor, Color ForegroundColor);

    public static readonly (int X, int Y) RESOLUTION = (800, 900);
    public static readonly (int X, int Y) RESOLUTION_MIN = (400, 450);

    public static readonly Color COLOR_BG_MAIN = new(187, 173, 160);
    public static readonly Color COLOR_LIGHT_ALT = new(238, 228, 218);
    public static readonly Color COLOR_DARK = new(119, 110, 101);
    public static readonly Color COLOR_LIGHT = new(249, 246, 242);
    public static readonly float FONT_SIZE_BIG = 46.0f;
    public static readonly float FONT_SIZE_MEDIUM = 23.0f;

    public static readonly Dictionary<int, CellColor> CELL_COLORS = new()
    {
        { 0, new CellColor(new Color(205, 193, 180), COLOR_DARK) },
        { 2, new CellColor(COLOR_LIGHT_ALT, COLOR_DARK) },
        { 4, new CellColor(new Color(237, 224, 200), COLOR_DARK) },
        { 8, new CellColor(new Color(242, 177, 121), COLOR_LIGHT) },
        { 16, new CellColor(new Color(245, 149, 99), COLOR_LIGHT) },
        { 32, new CellColor(new Color(246, 124, 95), COLOR_LIGHT) },
        { 64, new CellColor(new Color(246, 94, 59), COLOR_LIGHT) },
        { 128, new CellColor(new Color(237, 207, 114), COLOR_LIGHT) },
        { 256, new CellColor(new Color(237, 204, 97), COLOR_LIGHT) },
        { 512, new CellColor(new Color(237, 200, 80), COLOR_LIGHT) },
        { 1024, new CellColor(new Color(237, 197, 63), COLOR_LIGHT) },
        { 2048, new CellColor(new Color(237, 194, 46), COLOR_LIGHT) }
    };
}
