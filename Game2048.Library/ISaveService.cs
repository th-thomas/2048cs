namespace Game2048.Library;

public interface ISaveService
{
    int FetchHighScore();
    void SaveHighScore(int score);
    GameSnapshot FetchSnapshot(Save saveType);
    void SaveSnapshot(GameSnapshot snapshot, Save saveType);
    void ClearPreviousMove();
    void ClearAll();
}
