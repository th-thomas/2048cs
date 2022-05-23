using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Helpers;

internal static class Extensions
{
    internal static Vector2 GetCenter(this Rectangle bounds)
    {
        return new Vector2(bounds.Width / 2, bounds.Height / 2);
    }

    internal static void DrawColoredRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, Texture2D emptyTexture)
    {
        spriteBatch.Draw(emptyTexture, rectangle, color);
    }

    public enum AlignX { LeftAligned, CenterAligned, RightAligned }
    public enum AlignY { TopAligned, CenterAligned, BottomAligned }

    internal static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Color color, Rectangle renderArea,
        AlignX ax = AlignX.LeftAligned, AlignY ay = AlignY.TopAligned)
    {

        var location = new Vector2(renderArea.X, renderArea.Y);

        var size = font.MeasureString(text);

        switch (ax)
        {
            case AlignX.CenterAligned:
                location.X += (renderArea.Width - size.X) / 2;
                break;
            case AlignX.RightAligned:
                location.X += renderArea.Width - size.X;
                break;
        }

        switch (ay)
        {
            case AlignY.CenterAligned:
                location.Y += (renderArea.Height - size.Y) / 2;
                break;
            case AlignY.BottomAligned:
                location.Y += renderArea.Height - size.Y;
                break;
        }

        spriteBatch.DrawString(font, text, location, color);
    }
}
