using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Helpers;

internal static class Extensions
{
    internal static void DrawColoredRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, Texture2D emptyTexture)
    {
        spriteBatch.Draw(emptyTexture, rectangle, color);
    }

    public enum AlignX { LeftAligned, CenterAligned, RightAligned }
    public enum AlignY { TopAligned, CenterAligned, BottomAligned }

    internal static void DrawString(this SpriteBatch spriteBatch, SpriteFont font, string text, Color color, Rectangle bounds,
        AlignX ax = AlignX.LeftAligned, AlignY ay = AlignY.TopAligned)
    {
        var location = new Vector2(bounds.X, bounds.Y);
        var size = font.MeasureString(text);

        location.X += ax switch
        {
            AlignX.CenterAligned => (bounds.Width / 2) - (size.X / 2),
            AlignX.RightAligned => bounds.Width - size.X,
            _ => 0
        };

        location.Y += ay switch
        {
            AlignY.CenterAligned => (bounds.Height / 2) - (size.Y / 2),
            AlignY.BottomAligned => bounds.Height - size.Y,
            _ => 0
        };

        spriteBatch.DrawString(font, text, location, color);
    }
}
