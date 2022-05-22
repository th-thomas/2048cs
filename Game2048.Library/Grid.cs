using System.Collections;

namespace Game2048.Library;

public class Grid
{
    private readonly int _size;
    private readonly IScoreManager _game;
    private readonly Cell?[,] _cellsArray;
    private readonly Dictionary<Cell, Point> _cellsMap;
    private readonly Random _rand;

    public Grid(int size, IScoreManager game)
    {
        _size = size;
        _game = game ?? throw new ArgumentNullException(nameof(game));
        _cellsArray = new Cell[size, size];
        _cellsMap = new();
        _rand = new Random();
    }

    #region Properties
    public bool IsScoreGoalReached
    {
        // TODO: refactor
        get => _cellsMap.Keys.Any(cell => cell.Value == 2048);
    }
    #endregion

    #region Public methods
    public Cell? GetCell(int row, int col)
    {
        return _cellsArray[row, col];
    }

    public void SetCell(int row, int col, int value)
    {
        var cell = _cellsArray[row, col];
        if (value == 0)
        {
            cell = null;
        }
        else
        {
            cell = new Cell(value, this, _game);
            _cellsMap.Add(cell, new Point(col, row));
        }
    }

    public void ClearCells()
    {
        ((IList)_cellsArray).Clear();
        _cellsMap.Clear();
    }

    public bool NeighborCellExists(Direction direction, Cell c)
    {
        Point coord = _cellsMap[c];
        return direction switch
        {
            Direction.Up => coord.Y != 0,
            Direction.Down => coord.Y != _size - 1,
            Direction.Left => coord.X != 0,
            Direction.Right => coord.X != _size - 1,
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    public Cell? GetNeighborCell(Direction direction, Cell c)
    {
        Point coord = _cellsMap[c];
        return direction switch
        {
            Direction.Up => _cellsArray[coord.Y - 1, coord.X],
            Direction.Down => _cellsArray[coord.Y + 1, coord.X],
            Direction.Left => _cellsArray[coord.Y, coord.X - 1],
            Direction.Right => _cellsArray[coord.Y, coord.X + 1],
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    public bool CanAnyCellMove()
    {
        foreach (var dir in (Direction[])Enum.GetValues(typeof(Direction)))
        {
            if (CanAnyCellMove(dir))
            {
                return true;
            }
        }
        return false;
    }

    public bool CanAnyCellMove(Direction direction)
    {
        return _cellsMap.Keys.Any(cell => cell.CanMove(direction));
    }

    public void MoveEachCell(Direction direction)
    {
        if (direction == Direction.Down || direction == Direction.Right)
        {
            for (var row = _size - 1; row >= 0; row--)
            {
                for (var col = _size - 1; col >= 0; col--)
                {
                    var cellToMove = _cellsArray[row, col];
                    if (cellToMove is not null)
                    {
                        cellToMove.Move(direction);
                    }
                }
            }
        }
        else
        {
            for (var row = 0; row < _size; row++)
            {
                for (var col = 0; col < _size; col++)
                {
                    var cellToMove = _cellsArray[row, col];
                    if (cellToMove is not null)
                    {
                        cellToMove.Move(direction);
                    }
                }
            }
        }
    }

    public void MoveCell(Direction direction, Cell c)
    {
        Point coord = _cellsMap[c];

        Point newCoord = direction switch
        {
            Direction.Up => new Point(coord.X, coord.Y - 1),
            Direction.Down => new Point(coord.X, coord.Y + 1),
            Direction.Left => new Point(coord.X - 1, coord.Y),
            Direction.Right => new Point(coord.X + 1, coord.Y),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

        var neighborCell = GetNeighborCell(direction, c);

        if (neighborCell is not null)
        {
            _cellsMap.Remove(neighborCell);
        }

        _cellsArray[newCoord.Y, newCoord.X] = c;
        _cellsArray[coord.Y, coord.X] = null;

        _cellsMap[c] = newCoord;
    }

    public void SpawnInitCells()
    {
        var cell1Value = RandCellValue();
        var cell1Pos = RandCellPosition();

        _cellsArray[cell1Pos.Y, cell1Pos.X] = new Cell(cell1Value, this, _game);
        _cellsMap.Add(_cellsArray[cell1Pos.Y, cell1Pos.X], cell1Pos);

        var cell2Value = RandCellValue();
        Point cell2Pos;
        do
        {
            cell2Pos = RandCellPosition();
        } while (cell2Pos == cell1Pos);
        _cellsArray[cell2Pos.Y, cell2Pos.X] = new Cell(cell2Value, this, _game);
        _cellsMap.Add(_cellsArray[cell2Pos.Y, cell2Pos.X], cell2Pos);
    }

    public void SpawnNewCell()
    {
        Point spawnPoint;
        do
        {
            spawnPoint = RandCellPosition();
        } while (_cellsArray[spawnPoint.Y, spawnPoint.X] is not null);

        _cellsArray[spawnPoint.Y, spawnPoint.X] = new Cell(RandCellValue(), this, _game);
        _cellsMap.Add(_cellsArray[spawnPoint.Y, spawnPoint.X], spawnPoint);
    }
    #endregion

    #region Private methods
    private Point RandCellPosition()
    {
        int x = _rand.Next(_size);
        int y = _rand.Next(_size);
        return new Point(x, y);
    }

    private int RandCellValue()
    {
        return _rand.Next(1, 3) * 2;
    }
    #endregion
}
