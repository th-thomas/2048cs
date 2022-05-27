using Game2048.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;
using static IButton;

namespace Game2048.Buttons;

internal class Button : IButton
{
    #region Private fields
    private readonly ButtonsSharedInfo _buttonsSharedInfo;
    private readonly ViewportAdapter _viewportAdapter;
    private readonly SpriteBatch _spriteBatch;
    private readonly Texture2D _texture;
    private readonly Rectangle _bounds;

    private static readonly Dictionary<GameButtonState, Color> _buttonColor = new()
    {
        { GameButtonState.None, Color.White },
        { GameButtonState.Released, Color.White },
        { GameButtonState.Pressed, DisplayConstants.COLOR_DARK },
        { GameButtonState.Hovered, new Color(DisplayConstants.COLOR_DARK, 127) }
    };
    #endregion

    #region Properties
    public GameButtonState State { get; set; } = GameButtonState.None;
    #endregion

    internal Button(ButtonsSharedInfo buttonsSharedInfo, ViewportAdapter viewportAdapter, SpriteBatch spriteBatch, Texture2D texture, Rectangle bounds)
    {
        _buttonsSharedInfo = buttonsSharedInfo;
        _viewportAdapter = viewportAdapter;
        _spriteBatch = spriteBatch;
        _texture = texture;

        var originX = bounds.X + bounds.Width / 2 - 45;
        var originY = bounds.Y + bounds.Height / 2 - 45;
        _bounds = new Rectangle(originX, originY, 90, 90);
    }

    internal void Draw()
    {
        _spriteBatch.Draw(_texture, _bounds, _buttonColor[State]);
    }

    public void Update(MouseStateExtended mouseState)
    {
        if (_bounds.Contains(_viewportAdapter.PointToScreen(mouseState.Position)))
        {
            if (mouseState.WasButtonJustDown(MouseButton.Left))
            {
                _buttonsSharedInfo.FirstPressed = this;
            }
            if (mouseState.IsButtonDown(MouseButton.Left))
            {
                State = GameButtonState.Pressed;
                
            }
            else
            {
                if (mouseState.WasButtonJustUp(MouseButton.Left)
                    && _buttonsSharedInfo.LastEntered == this
                    && _buttonsSharedInfo.FirstPressed == this)
                {
                    State = GameButtonState.Released;
                }
                else
                {
                    State = GameButtonState.Hovered;
                }
            }
            _buttonsSharedInfo.LastEntered = this;
        }
        else
        {
            State = GameButtonState.None;
        }
    }
}
