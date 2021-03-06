namespace Game2048.Library;

public interface IGameCore : IObservable<IGameCore>
{
    int Size { get; }
    int Score { get; }
    int HighScore { get; }
    GameState GameState { get; }
    bool IsPreviousMovePossible { get; }
    ICell? GetCell(int row, int col);
    void Action(Direction direction);
    void Reset();
    bool LoadSavedGame(Save saveType);
}
