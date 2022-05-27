namespace Game2048.Buttons;

internal class ButtonsSharedInfo
{
    internal IButton? LastEntered { get; set; } = null;
    internal IButton? FirstPressed { get; set; } = null;
}
