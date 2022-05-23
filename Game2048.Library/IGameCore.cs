namespace Game2048.Library;

public interface IGameCore
{
    void Action(Direction direction);
    int Size { get; }
    Cell? GetCell(int row, int col);
}
