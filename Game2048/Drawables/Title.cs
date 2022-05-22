using Game2048.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Drawables;

internal class Title
{
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private Vector2 _pos;
    private Vector2 _origin;

    internal Title(SpriteBatch spriteBatch, GameContent gameContent, Rectangle bounds)
    {
        _spriteBatch = spriteBatch;
        _font = gameContent.Font;

        var size = _font.MeasureString("2048");
        _pos = bounds.GetCenter();
        _origin = size * 0.5f;
    }

    internal void Draw()
    {
        _spriteBatch.DrawString(_font, "2048", _pos, Color.White, 0, _origin, .8f, SpriteEffects.None, 0);
    }
}
