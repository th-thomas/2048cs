using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Drawables;

internal class Button
{
    private readonly Texture2D _texture;
    private readonly SpriteBatch _spriteBatch;
    private readonly Rectangle _rectangle;

    internal Button(SpriteBatch spriteBatch, Texture2D texture, Rectangle bounds, Action action)
    {
        _spriteBatch = spriteBatch;
        _texture = texture;

        var originX = bounds.X + bounds.Width / 2 - 45;
        var originY = bounds.Y + bounds.Height / 2 - 45;
        _rectangle = new Rectangle(originX, originY, 90, 90);
    }

    internal void Draw()
    {
        _spriteBatch.Draw(_texture, _rectangle, Color.White);
    }
}
