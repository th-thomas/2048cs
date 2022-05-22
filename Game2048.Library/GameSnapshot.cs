namespace Game2048.Library;

public record GameSnapshot
{
    public GameSnapshot(int[,]? grid, int score)
    {
        Grid = grid;
        Score = score;
    }

    public int[,]? Grid { get; }
    public int Score { get; }
}
