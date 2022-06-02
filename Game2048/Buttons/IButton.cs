using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

internal interface IButton
{
    internal enum GameButtonState
    {
        Enabled,
        Disabled,
        Pressed,
        Hovered,
        Released
    }
    GameButtonState State { get; }
    void UpdateDisabled();
    void Update(GamePadState gamePadState, KeyboardStateExtended keyboardState, MouseStateExtended mouseState, bool invokedByOtherController = false);
}
