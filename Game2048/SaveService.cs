using Game2048.Library;
using System.Text;

namespace Game2048;

internal class SaveService : ISaveService
{
    private enum ValueIndex
    {
        HighScore,
        PreviousGameScore,
        PreviousGameGrid,
        PreviousMoveScore,
        PreviousMoveGrid
    }

#pragma warning disable CS8618
    private static SaveService _saveService;
#pragma warning restore CS8618
    private readonly int _gameSize;
    private readonly string _savefilePath;

    private SaveService(int gameSize, string savefilePath)
    {
        _gameSize = gameSize;
        _savefilePath = savefilePath;
        if (!File.Exists(_savefilePath))
        {
            File.Create(_savefilePath).Close();
        }
    }

    internal static SaveService GetInstance(int gameSize, string savefilePath)
    {
        return _saveService ??= new SaveService(gameSize, savefilePath);
    }

    private string this[ValueIndex i]
    {
        get
        {
            var index = (int)i;
            var lines = File.ReadLines(_savefilePath);
            return (lines.Count() < index) ? string.Empty : lines.ElementAt(index);
        }
        set
        {
            var index = (int)i;
            var lines = File.ReadLines(_savefilePath).ToList();
            while (lines.Count <= index)
            {
                lines.Add(string.Empty);
            }
            lines[(int)i] = value;
            File.WriteAllLines(_savefilePath, lines);
        }
    }

    #region Public methods
    public int FetchHighScore()
    {
        return int.TryParse(this[ValueIndex.HighScore], out int highScore) ? highScore : 0;
    }

    public void SaveHighScore(int score)
    {
        this[ValueIndex.HighScore] = score.ToString();
    }

    public GameSnapshot FetchSnapshot(Save saveType)
    {
        var snapshotScoreIndex = saveType == Save.PreviousGame
                ? ValueIndex.PreviousGameScore
                : ValueIndex.PreviousMoveScore;

        var snapshotGridKey = saveType == Save.PreviousGame
                ? ValueIndex.PreviousGameGrid
                : ValueIndex.PreviousMoveGrid;

        var score = int.TryParse(this[snapshotScoreIndex], out int parsedScore) ? parsedScore : 0;
        var gridAsText = this[snapshotGridKey];
        int[,]? grid = null;

        if (gridAsText != string.Empty)
        {
            grid = new int[_gameSize, _gameSize];
            var cellStringValues = gridAsText.Split(';');
            var i = 0;
            for (var row = 0; row < _gameSize; row++)
            {
                for (var col = 0; col < _gameSize; col++)
                {
                    grid[row, col] = int.TryParse(cellStringValues[i], out int parsed)
                        ? parsed
                        : 0;
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
                this[ValueIndex.PreviousGameGrid] = grid;
                this[ValueIndex.PreviousGameScore] = snapshot.Score.ToString();
                break;
            case Save.PreviousMove:
                this[ValueIndex.PreviousMoveGrid] = grid;
                this[ValueIndex.PreviousMoveScore] = snapshot.Score.ToString();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(saveType));
        }
    }

    public void ClearPreviousMove()
    {
        this[ValueIndex.PreviousMoveGrid] = string.Empty;
        this[ValueIndex.PreviousGameScore] = string.Empty;
    }

    public void ClearAll()
    {
        File.WriteAllText(_savefilePath, string.Empty);
    }
    #endregion
}
