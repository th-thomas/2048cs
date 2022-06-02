using Game2048.Helpers;
using Game2048.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Panels;

internal class MainPanel
{
    private const int CELL_PADDING = 10;

    private readonly SpriteBatch _spriteBatch;
    private readonly Rectangle _bounds;
    private readonly Texture2D _emptyTexture;
    private readonly SpriteFont _font;
    private Rectangle _cellRectangle;
    private readonly Dictionary<GameState, string> _endgameText = new() { { GameState.Loss, "PERDU" }, { GameState.Win, "GAGNE" } };
    private readonly Color _endgameDimColor = new(127, 127, 127, 127);

    internal ICell?[,]? Cells { get; set; }
    internal GameState GameState { get; set; }

    internal MainPanel(SpriteBatch spriteBatch, GameContent gameContent, Rectangle bounds)
    {
        _spriteBatch = spriteBatch;
        _bounds = bounds;
        _emptyTexture = gameContent.EmptyTexture;
        _font = gameContent.FontBig;
        GameState = GameState.Ongoing;
        _cellRectangle = new Rectangle();
    }

    internal void Draw()
    {
        if (Cells is null)
        {
            return;
        }

        var totalRectangleSize = _bounds.Width / Cells.GetLength(0);
        _cellRectangle.Width = _cellRectangle.Height = totalRectangleSize - (2 * CELL_PADDING);

        var cellX = _bounds.X;
        var cellY = _bounds.Y;
        Color cellBg;
        Color cellFg;

        for (var row = 0; row < Cells.GetLength(0); row++)
        {
            for (var col = 0; col < Cells.GetLength(1); col++)
            {
                _cellRectangle.X = cellX + CELL_PADDING;
                _cellRectangle.Y = cellY + CELL_PADDING;

                var cell = Cells[row, col];
                (cellBg, cellFg) = DisplayConstants.CELL_COLORS[cell is null ? 0 : cell.Value];
                _spriteBatch.DrawColoredRectangle(_cellRectangle, cellBg, _emptyTexture);
                _spriteBatch.DrawString(_font, cell is null ? string.Empty : cell.Value.ToString(), cellFg, _cellRectangle, Extensions.AlignX.CenterAligned, Extensions.AlignY.CenterAligned);
                
                cellX += totalRectangleSize;
                if (col == Cells.GetLength(1) - 1)
                {
                    cellX = 0;
                }
            }
            cellY += totalRectangleSize;
        }

        if (GameState != GameState.Ongoing)
        {
            DrawEndgame();
        }
    }

    private void DrawEndgame()
    {
        _spriteBatch.DrawColoredRectangle(_bounds, _endgameDimColor, _emptyTexture);
        _spriteBatch.DrawString(_font, _endgameText[GameState], DisplayConstants.COLOR_DARK, _bounds, Extensions.AlignX.CenterAligned, Extensions.AlignY.CenterAligned);
    }
}
