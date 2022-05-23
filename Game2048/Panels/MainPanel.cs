using Game2048.Helpers;
using Game2048.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Panels;

internal class MainPanel
{
    private readonly SpriteBatch _spriteBatch;
    private readonly IGameCore _gameCore;
    private readonly Texture2D _emptyTexture;

    internal MainPanel(SpriteBatch spriteBatch, GameContent gameContent, IGameCore gameCore, Rectangle bounds)
    {
        _spriteBatch = spriteBatch;
        _emptyTexture = gameContent.EmptyTexture;
        _gameCore = gameCore;
    }

    internal void Draw()
    {
        var cellRectangle = new Rectangle();
        int cellX = 0;
        int cellY = 100;

        for (var row = 0; row < _gameCore.Size; row++)
        {
            for (var col = 0; col < _gameCore.Size; col++)
            {
                cellRectangle.X = cellX;
                cellRectangle.Y = cellY;
                cellRectangle.Width = 200;
                cellRectangle.Height = 200;

                var cell = _gameCore.GetCell(row, col);
                if (cell is null)
                {
                    _spriteBatch.DrawColoredRectangle(cellRectangle, Color.AliceBlue, _emptyTexture);
                }
                else
                {
                    _spriteBatch.DrawColoredRectangle(cellRectangle, Color.AliceBlue, _emptyTexture);
                }
                cellX += 200;
                if (col == _gameCore.Size - 1)
                {
                    cellX = 0;
                }
            }
            cellY += 200;
        }
    }
}
