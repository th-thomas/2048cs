using Game2048.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;

namespace Game2048.Drawables;

internal class Button
{
    private readonly ViewportAdapter _viewportAdapter;
    private readonly SpriteBatch _spriteBatch;
    private readonly Rectangle _rectangle;
    private readonly Texture2D _texture;

    internal Button(ViewportAdapter viewportAdapter, SpriteBatch spriteBatch, Texture2D texture, Rectangle bounds, Action action)
    {
        _viewportAdapter = viewportAdapter;
        _spriteBatch = spriteBatch;
        _texture = texture;

        var originX = bounds.X + bounds.Width / 2 - 45;
        var originY = bounds.Y + bounds.Height / 2 - 45;
        _rectangle = new Rectangle(originX, originY, 90, 90);
    }

    private bool _isPressed = false;
    private Color ButtonColor { get => _isPressed ? DisplayConstants.COLOR_DARK : Color.White; }

    internal void Draw()
    {
        _spriteBatch.Draw(_texture, _rectangle, ButtonColor);
    }

    internal void Update()
    {
        var gamepadState = GamePad.GetState(PlayerIndex.One);
        var keyboardState = Keyboard.GetState();
        var mouseState = Mouse.GetState();

        _isPressed = gamepadState.Buttons.LeftShoulder == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.F2)
            || (mouseState.LeftButton.HasFlag(ButtonState.Pressed) && _rectangle.Contains(_viewportAdapter.PointToScreen(mouseState.X, mouseState.Y)));
    }
}
