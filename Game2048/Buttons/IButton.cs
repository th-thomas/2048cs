using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

internal interface IButton
{
    internal enum GameButtonState
    {
        None,
        Pressed,
        Hovered,
        Released
    }
    GameButtonState State { get; }
    void Update(GamePadState gamePadState, KeyboardStateExtended keyboardState, MouseStateExtended mouseState);
    void InvokedByKeyboardOrGamepad();
}
