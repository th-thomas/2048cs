namespace Game2048.Library;

internal class Cell : ICell
{
    private readonly Grid _grid;
    private readonly IScoreManager _scoreManager;
    private bool _hasFusionned;

    public Cell(int valeur, Grid grid, IScoreManager game)
    {
        Value = valeur;
        _grid = grid;
        _scoreManager = game;
        _hasFusionned = false;
    }

    #region Properties
    public int Value { get; private set; }
    #endregion

    #region Public methods
    public bool CanMove(Direction direction)
    {
        if (_grid.NeighborCellExists(direction, this))
        {
            var neighborCell = _grid.GetNeighborCell(direction, this);
            return neighborCell is null || Value == neighborCell.Value;
        }
        return false;
    }

    public void Move(Direction direction)
    {
        _hasFusionned = false;
        while (_grid.NeighborCellExists(direction, this))
        {
            var neighborCell = _grid.GetNeighborCell(direction, this);
            if (neighborCell is null)
            {
                _hasFusionned = false;
                _grid.MoveCell(direction, this);
            }
            else if (neighborCell.Value == Value && !neighborCell._hasFusionned && !_hasFusionned)
            {
                FusionCells();
                _hasFusionned = true;
                _grid.MoveCell(direction, this);
            }
            else
            {
                return;
            }
        }
    }
    #endregion

    #region Private methods
    private void FusionCells()
    {
        Value *= 2;
        _scoreManager.AddToScore(Value);
    }
    #endregion
}
