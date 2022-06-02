namespace Game2048.Library;

public class GameCore : IScoreManager, IGameCore
{
    #region Fields
    private readonly List<IObserver<IGameCore>> _observers;
    private readonly Grid _grid;
    private readonly ISaveService _saveService;
    #endregion

    #region Properties
    public int Size { get; }
    public GameState GameState { get; private set; } = GameState.Ongoing;
    public int Score { get; private set; }
    public int HighScore => _saveService.FetchHighScore();
    public bool IsPreviousMovePossible { get; private set; }
    #endregion

    public GameCore(int size, ISaveService saveService)
    {
        _observers = new List<IObserver<IGameCore>>();
        Size = size;
        _grid = new Grid(size, this);
        _saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));
    }

    #region Publics methods
    public void Reset()
    {
        IsPreviousMovePossible = false;
        GameState = GameState.Ongoing;
        _grid.ClearCells();
        Score = 0;
        _saveService.ClearPreviousMove();
        _grid.SpawnInitCells();
        NotifyObservers();
    }

    public void AddToScore(int points)
    {
        Score += points;
    }

    public ICell? GetCell(int row, int col)
    {
        return _grid.GetCell(row, col);
    }

    public void Action(Direction direction)
    {
        if (!_grid.CanAnyCellMove() || !_grid.CanAnyCellMove(direction))
        {
            return;
        }
        SaveSnapshot(Save.PreviousMove);
        IsPreviousMovePossible = true;
        _grid.MoveEachCell(direction);
        _grid.SpawnNewCell();
        GameState = _grid.IsScoreGoalReached ? GameState.Win : _grid.CanAnyCellMove() ? GameState.Ongoing : GameState.Loss;
        if (GameState == GameState.Win)
        {
            IsPreviousMovePossible = false;
        }
        SaveSnapshot(Save.PreviousGame);
        NotifyObservers();
    }

    public IDisposable Subscribe(IObserver<IGameCore> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
        return new Unsubscriber(_observers, observer);
    }

    public bool LoadSavedGame(Save saveType)
    {
        var snapshot = _saveService.FetchSnapshot(saveType);
        if (!TrySetGameState(snapshot, saveType))
        {
            return false;
        }
        NotifyObservers();
        return true;
    }
    #endregion

    #region Private methods
    private void NotifyObservers()
    {
        _observers.ForEach(x => x.OnNext(this));
    }

    private void SaveSnapshot(Save saveType)
    {
        if (Score > _saveService.FetchHighScore())
        {
            _saveService.SaveHighScore(Score);
        }

        var saveGrid = new int[Size, Size];
        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                Cell? cell = _grid.GetCell(row, col);
                saveGrid[row, col] = cell is null ? 0 : cell.Value;
            }
        }

        var snapshot = new GameSnapshot(saveGrid, Score);
        _saveService.SaveSnapshot(snapshot, saveType);
    }

    private bool TrySetGameState(GameSnapshot snapshot, Save saveType)
    {
        if (snapshot.Grid is null)
        {
            IsPreviousMovePossible = false;
            return false;
        }

        Score = snapshot.Score;

        for (var row = 0; row < Size; row++)
        {
            for (var col = 0; col < Size; col++)
            {
                int cellValue = snapshot.Grid[row, col];
                _grid.SetCell(row, col, cellValue);
            }
        }

        GameState = _grid.IsScoreGoalReached ? GameState.Win : _grid.CanAnyCellMove() ? GameState.Ongoing : GameState.Loss;
        
        if (saveType == Save.PreviousMove)
        {
            IsPreviousMovePossible = false;
            SaveSnapshot(Save.PreviousGame);
        }
        else
        {
            var previousMoveGrid = _saveService.FetchSnapshot(Save.PreviousMove).Grid;
            IsPreviousMovePossible = previousMoveGrid is not null && !_grid.IsGridLikeMe(previousMoveGrid);
        }

        return true;
    }
    #endregion
}
