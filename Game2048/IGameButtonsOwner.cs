using Game2048.Buttons;

namespace Game2048;

internal interface IGameButtonsOwner
{
    IButton NewGameButton { get; set; }
    IButton PreviousMoveButton { get; set; }
}
