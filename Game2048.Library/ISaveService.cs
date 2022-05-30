namespace Game2048.Library;

internal interface ISaveService
{
    int FetchHighScore();
    void SaveHighScore(int score);
    GameSnapshot FetchSnapshot(Save saveType);
    void SaveSnapshot(GameSnapshot snapshot, Save saveType);
    void ClearPreviousMove();
}
