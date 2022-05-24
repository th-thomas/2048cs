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
    private readonly SpriteFont _cellFont;

    internal ICell?[,]? Cells { get; set; }

    internal MainPanel(SpriteBatch spriteBatch, GameContent gameContent, Rectangle bounds)
    {
        _spriteBatch = spriteBatch;
        _bounds = bounds;
        _emptyTexture = gameContent.EmptyTexture;
        _cellFont = gameContent.FontBig;
    }

    internal void Draw()
    {
        if (Cells is null)
        {
            return;
        }

        var totalRectangleSize = _bounds.Width / Cells.GetLength(0);
        var cellRectangle = new Rectangle();
        cellRectangle.Width = cellRectangle.Height = totalRectangleSize - (2 * CELL_PADDING);

        var cellX = _bounds.X;
        var cellY = _bounds.Y;
        Color cellBg;
        Color cellFg;

        for (var row = 0; row < Cells.GetLength(0); row++)
        {
            for (var col = 0; col < Cells.GetLength(1); col++)
            {
                cellRectangle.X = cellX + CELL_PADDING;
                cellRectangle.Y = cellY + CELL_PADDING;

                var cell = Cells[row, col];
                (cellBg, cellFg) = DisplayConstants.CELL_COLORS[cell is null ? 0 : cell.Value];
                _spriteBatch.DrawColoredRectangle(cellRectangle, cellBg, _emptyTexture);
                _spriteBatch.DrawString(_cellFont, cell is null ? string.Empty : cell.Value.ToString(), cellFg, cellRectangle, Extensions.AlignX.CenterAligned, Extensions.AlignY.CenterAligned);
                
                cellX += totalRectangleSize;
                if (col == Cells.GetLength(1) - 1)
                {
                    cellX = 0;
                }
            }
            cellY += totalRectangleSize;
        }
    }
}
