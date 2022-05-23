using Game2048.Drawables;
using Game2048.Helpers;
using Game2048.Library;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Panels
{
    internal class InfoPanel
    {
        private readonly SpriteBatch _spriteBatch;
        private readonly ScorePanel _currentScorePanel;
        private readonly ScorePanel _highScorePanel;
        private readonly Button _previousMoveButton;
        private readonly Button _newGameButton;
        private readonly SpriteFont _titleFont;
        private readonly Rectangle _titleBounds;

        internal InfoPanel(SpriteBatch spriteBatch, GameContent gameContent, IGameCore gameCore, Rectangle bounds)
        {
            _spriteBatch = spriteBatch;

            var titleWidth = bounds.Width / 4;
            _titleBounds = new Rectangle(bounds.X, bounds.Y, titleWidth, bounds.Height);
            _titleFont = gameContent.FontBig;

            var currScoreX = bounds.X + titleWidth;
            var scoreWidth = bounds.Width * 7 / 32;
            _currentScorePanel = new ScorePanel(_spriteBatch, gameContent, ScorePanel.ScoreType.CurrentScore, new Rectangle(currScoreX, bounds.Y, scoreWidth, bounds.Height));

            var highScoreX = currScoreX + scoreWidth;
            _highScorePanel = new ScorePanel(_spriteBatch, gameContent, ScorePanel.ScoreType.HighScore, new Rectangle(highScoreX, bounds.Y, scoreWidth, bounds.Height));

            var prevMoveX = highScoreX + scoreWidth;
            var buttonWidth = bounds.Width * 5 / 32;
            _previousMoveButton = new Button(_spriteBatch, gameContent.PreviousMoveButtonTexture, new Rectangle(prevMoveX, bounds.Y, buttonWidth, bounds.Height), null);

            var newGameX = prevMoveX + buttonWidth;
            _newGameButton = new Button(_spriteBatch, gameContent.NewGameButtonTexture, new Rectangle(newGameX, bounds.Y, buttonWidth, bounds.Height), null);
        }

        internal void Draw()
        {
            _spriteBatch.DrawString(_titleFont, "2048", DisplayConstants.COLOR_DARK, _titleBounds, Extensions.AlignX.CenterAligned, Extensions.AlignY.CenterAligned);
            _currentScorePanel.Draw();
            _highScorePanel.Draw();
            _previousMoveButton.Draw();
            _newGameButton.Draw();
        }
    }
}
