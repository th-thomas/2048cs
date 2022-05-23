using Game2048.Helpers;
using Game2048.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Panels;

internal class MainPanel
{
    private const int CELL_PADDING = 10;

    private readonly SpriteBatch _spriteBatch;
    private readonly IGameCore _gameCore;
    private readonly Rectangle _bounds;
    private readonly Texture2D _emptyTexture;
    private readonly SpriteFont _cellFont;

    internal MainPanel(SpriteBatch spriteBatch, GameContent gameContent, IGameCore gameCore, Rectangle bounds)
    {
        _spriteBatch = spriteBatch;
        _gameCore = gameCore;
        _bounds = bounds;
        _emptyTexture = gameContent.EmptyTexture;
        _cellFont = gameContent.FontBig;
    }

    internal void Draw()
    {
        var totalRectangleSize = _bounds.Width / _gameCore.Size;
        var cellRectangle = new Rectangle();
        cellRectangle.Width = cellRectangle.Height = totalRectangleSize - (2 * CELL_PADDING);

        var cellX = _bounds.X;
        var cellY = _bounds.Y;
        Color cellBg;
        Color cellFg;

        for (var row = 0; row < _gameCore.Size; row++)
        {
            for (var col = 0; col < _gameCore.Size; col++)
            {
                cellRectangle.X = cellX + CELL_PADDING;
                cellRectangle.Y = cellY + CELL_PADDING;

                var cell = _gameCore.GetCell(row, col);
                (cellBg, cellFg) = DisplayConstants.CELL_COLORS[cell is null ? 0 : cell.Value];
                _spriteBatch.DrawColoredRectangle(cellRectangle, cellBg, _emptyTexture);
                _spriteBatch.DrawString(_cellFont, cell is null ? string.Empty : cell.Value.ToString(), cellFg, cellRectangle, Extensions.AlignX.CenterAligned, Extensions.AlignY.CenterAligned);
                
                cellX += totalRectangleSize;
                if (col == _gameCore.Size - 1)
                {
                    cellX = 0;
                }
            }
            cellY += totalRectangleSize;
        }
    }
}
