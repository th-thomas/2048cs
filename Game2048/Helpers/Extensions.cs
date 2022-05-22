using Microsoft.Xna.Framework;

namespace Game2048.Helpers;

internal static class Extensions
{
    public static Vector2 GetCenter(this Rectangle rectangle)
    {
        return new Vector2(rectangle.Width / 2, rectangle.Height / 2);
    }
}
