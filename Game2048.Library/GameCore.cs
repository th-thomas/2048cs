namespace Game2048.Library;

public class GameCore : IScoreManager, IGameCore
{
    #region Fields
    private List<IObserver<IGameCore>> _observers;
    private readonly Grid _grid;
    // private readonly SaveService _saveService;
    #endregion

    #region Properties
    public int Size { get; }

    public GameState GameState { get; private set; }

    public int Score { get; private set; }

    public int HighScore { get; } // _saveService.FetchHighScore(); }
    #endregion

    public GameCore(int size) //, SaveService saveService)
    {
        _observers = new List<IObserver<IGameCore>>();
        Size = size;
        _grid = new Grid(size, this);
        //_saveService = saveService ?? throw new ArgumentNullException(nameof(saveService));

        //if (_saveService.FetchSnapshot(Save.PreviousGame).Grid is null)
        //    Init(false);
        //else
        //    LoadSavedGame(Save.PreviousGame);
    }

    #region Publics methods
    public void Init(bool clear)
    {
        GameState = GameState.Ongoing;

        if (clear)
        {
            _grid.ClearCells();
            // _saveService.ClearPreviousMove();
        }

        _grid.SpawnInitCells();
        _observers.ForEach(x => x.OnNext(this));
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
        //SaveSnapshot(Save.PreviousMove);
        if (!_grid.CanAnyCellMove(direction))
        {
            return;
        }
        _grid.MoveEachCell(direction);
        _grid.SpawnNewCell();
        GameState = _grid.IsScoreGoalReached ? GameState.Win : _grid.CanAnyCellMove() ? GameState.Ongoing : GameState.Loss;
        //SaveSnapshot(Save.PreviousGame);
        _observers.ForEach(x => x.OnNext(this));
    }

    public IDisposable Subscribe(IObserver<IGameCore> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
        return new Unsubscriber(_observers, observer);
    }

    //public void LoadSavedGame(Save saveType)
    //{

    //    var snapshot = _saveService.FetchSnapshot(saveType);
    //    if (snapshot.Grid is null)
    //    {
    //        return;
    //    }
    //    Score = snapshot.Score;
    //    for (var row = 0; row < Size; row++)
    //    {
    //        for (var col = 0; col < Size; col++)
    //        {
    //            int cellValue = snapshot.Grid[row, col];
    //            _grid.SetCell(row, col, cellValue);
    //        }
    //    }
    //    // TODO
    //    // setChanged();
    //    // notifyObservers();
    //}
    #endregion

    #region Private methods
    //private void SaveSnapshot(Save saveType)
    //{
    //    if (Score > _saveService.FetchHighScore())
    //    {
    //        _saveService.SaveHighScore(Score);
    //    }

    //    var saveGrid = new int[Size, Size];
    //    for (var row = 0; row < Size; row++)
    //    {
    //        for (var col = 0; col < Size; col++)
    //        {
    //            Cell? cell = _grid.GetCell(row, col);
    //            saveGrid[row, col] = cell is null ? 0 : cell.Value;
    //        }
    //    }

    //    var snapshot = new GameSnapshot(saveGrid, Score);
    //    _saveService.SaveSnapshot(snapshot, saveType);
    //}
    #endregion
}
