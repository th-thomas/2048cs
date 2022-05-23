using Microsoft.Extensions.Configuration;
using System.Text;

namespace Game2048.Library;

public class SaveService
{
    private readonly IConfiguration _config;
    private readonly int _gameSize;

    #region Constants
    private const string HIGHSCORE_KEY = "highScore";
    private const string PREVIOUSGAME_SCORE_KEY = "previousGameScore";
    private const string PREVIOUSGAME_GRID_KEY = "previousGameGrid";
    private const string PREVIOUSMOVE_SCORE_KEY = "previousMoveScore";
    private const string PREVIOUSMOVE_GRID_KEY = "previousMoveGrid";
    #endregion

    public SaveService(GameCore game, IConfiguration configuration)
    {
        _config = configuration;
        _gameSize = game.Size;
    }

    #region Public methods
    public int FetchHighScore()
    {
        return int.TryParse(_config[HIGHSCORE_KEY], out int highScore) ? highScore : 0;
    }

    public void SaveHighScore(int score)
    {
        _config[HIGHSCORE_KEY] = score.ToString();
    }

    public GameSnapshot FetchSnapshot(Save saveType)
    {
        var snapshotScoreKey = saveType == Save.PreviousGame
                ? PREVIOUSGAME_SCORE_KEY
                : PREVIOUSMOVE_SCORE_KEY;

        var snapshotGridKey = saveType == Save.PreviousGame
                ? PREVIOUSGAME_GRID_KEY
                : PREVIOUSMOVE_GRID_KEY;

        var score = int.TryParse(_config[snapshotScoreKey], out int parsedScore) ? parsedScore : 0;
        var gridAsText = _config[snapshotGridKey];
        int[,]? grid = null;

        if (gridAsText is not null)
        {
            grid = new int[_gameSize, _gameSize];
            var cellStringValues = gridAsText.Split(";");
            var i = 0;
            for (var row = 0; row < _gameSize; row++)
            {
                for (var col = 0; col < _gameSize; col++)
                {
                    grid[row, col] = int.TryParse(cellStringValues[i], out int parsed)
                        ? parsed
                        : throw new ArithmeticException("Token could not be parsed to Integer type");
                    i++;
                }
            }
        }

        return new GameSnapshot(grid, score);
    }

    public void SaveSnapshot(GameSnapshot snapshot, Save saveType)
    {
        var sb = new StringBuilder();

        if (snapshot.Grid is not null)
        {
            for (var row = 0; row < _gameSize; row++)
            {
                for (var col = 0; col < _gameSize; col++)
                {
                    var cellValue = snapshot.Grid[row, col];
                    sb.Append(cellValue);
                    sb.Append(';');
                }
            }
        }

        var grid = sb.ToString();

        switch (saveType)
        {
            case Save.PreviousGame:
                _config[PREVIOUSGAME_GRID_KEY] = grid;
                _config[PREVIOUSGAME_SCORE_KEY] = snapshot.Score.ToString();
                break;
            case Save.PreviousMove:
                _config[PREVIOUSMOVE_GRID_KEY] = grid;
                _config[PREVIOUSMOVE_SCORE_KEY] = snapshot.Score.ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(saveType));
        }
    }

    public void ClearPreviousMove()
    {
        _config[PREVIOUSMOVE_GRID_KEY] = null;
        _config[PREVIOUSGAME_SCORE_KEY] = null;
    }
    #endregion
}
