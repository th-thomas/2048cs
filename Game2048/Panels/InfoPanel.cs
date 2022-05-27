using Game2048.Buttons;
using Game2048.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace Game2048.Panels
{
    internal class InfoPanel
    {
        #region Fields
        private readonly SpriteBatch _spriteBatch;
        private readonly ScorePanel _currentScorePanel;
        private readonly ScorePanel _highScorePanel;
        private readonly Button _previousMoveButton;
        private readonly Button _newGameButton;
        private readonly SpriteFont _titleFont;
        private readonly Rectangle _titleBounds;
        #endregion

        #region Properties
        internal int Score { get; set; }
        internal int HighScore { get; set; }
        #endregion

        internal InfoPanel(ButtonsManager buttonsManager, ViewportAdapter viewportAdapter, SpriteBatch spriteBatch, GameContent gameContent, Rectangle bounds)
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

            var buttonsSharedInfo = new ButtonsSharedInfo();

            var buttonWidth = bounds.Width * 5 / 32;
            var prevMoveX = highScoreX + scoreWidth;
            var prevMoveBounds = new Rectangle(prevMoveX, bounds.Y, buttonWidth, bounds.Height);
            _previousMoveButton = new Button(buttonsSharedInfo, viewportAdapter, _spriteBatch, gameContent.PreviousMoveButtonTexture, prevMoveBounds);
            buttonsManager.PreviousMoveButton = _previousMoveButton;

            var newGameX = prevMoveX + buttonWidth;
            var newGameBounds = new Rectangle(newGameX, bounds.Y, buttonWidth, bounds.Height);
            _newGameButton = new Button(buttonsSharedInfo, viewportAdapter, _spriteBatch, gameContent.NewGameButtonTexture, newGameBounds);
            buttonsManager.NewGameButton = _newGameButton;
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
