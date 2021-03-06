using Game2048.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2048.Panels;

internal class ScorePanel
{
    internal enum ScoreType
    {
        CurrentScore,
        HighScore
    }

    private readonly SpriteBatch _spriteBatch;
    private readonly SpriteFont _smallFont;
    private readonly SpriteFont _mediumFont;
    private readonly Rectangle _upperRectangle;
    private readonly Rectangle _bottomRectangle;
    private readonly string _title;


    internal int Score { get; set; }

    internal ScorePanel(SpriteBatch spriteBatch, GameContent gameContent, ScoreType scoreType, Rectangle bounds)
    {
        _spriteBatch = spriteBatch;

        _smallFont = gameContent.FontSmall;
        _mediumFont = gameContent.FontMedium;

        _upperRectangle = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height / 2);
        _bottomRectangle = new Rectangle(bounds.X, bounds.Height / 2, bounds.Width, bounds.Height / 2);

        _title = scoreType switch
        {
            ScoreType.CurrentScore => "SCORE",
            ScoreType.HighScore => "MEILLEUR",
            _ => throw new ArgumentOutOfRangeException(nameof(scoreType))
        };
    }

    internal void Draw()
    { 
        _spriteBatch.DrawString(_smallFont, _title, DisplayConstants.COLOR_LIGHT_ALT, _upperRectangle, Extensions.AlignX.CenterAligned, Extensions.AlignY.BottomAligned);
        _spriteBatch.DrawString(_mediumFont, Score.ToString(), Color.White, _bottomRectangle, Extensions.AlignX.CenterAligned, Extensions.AlignY.TopAligned);
    }
}
