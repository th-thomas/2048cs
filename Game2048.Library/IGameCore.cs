namespace Game2048.Library;

public interface IGameCore : IObservable<IGameCore>
{
    int Size { get; }
    int Score { get; }
    int HighScore { get; }
    GameState GameState { get; }
    ICell? GetCell(int row, int col);
    void Action(Direction direction);
    void Init(bool clear);
}
