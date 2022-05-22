using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;
using static Game2048.Helpers.DisplayConstants;
using Game2048.Library;
using Game2048.Drawables;

namespace Game2048;

public class Game2048 : Game
{
    private ViewportAdapter _viewportAdapter;
    private SpriteBatch _spriteBatch;
    private IGameCore _gameCore;
    private GameContent _gameContent;

    private Texture2D _emptyTexture;
    private SpriteFont _font;

    private readonly GraphicsDeviceManager _graphics;

    #region Drawables
    private Button _previousMoveButton;
    private Button _newGameButton;
    private Title _title;
    #endregion

    public Game2048()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        _gameCore = new GameCore(4);
        _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, RESOLUTION_X, RESOLUTION_Y);

        _graphics.PreferredBackBufferWidth = RESOLUTION_X;
        _graphics.PreferredBackBufferHeight = RESOLUTION_Y;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _gameContent = new GameContent(Content, GraphicsDevice);

        _emptyTexture = _gameContent.BackgroundTexture;
        _font = _gameContent.Font;

        _previousMoveButton = new Button(_spriteBatch, _gameContent.PreviousMoveButtonTexture, new Rectangle(550, 0, 125, 100), null);
        _newGameButton = new Button(_spriteBatch, _gameContent.NewGameButtonTexture, new Rectangle(675, 0, 125, 100), null);
        _title = new Title(_spriteBatch, _gameContent, new Rectangle(0, 0, 200, 100));
    }

    protected override void Update(GameTime gameTime)
    {
        var gamepadState = GamePad.GetState(PlayerIndex.One);
        var keyboardState = Keyboard.GetState();

        if (gamepadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            Exit();
        else if (gamepadState.DPad.Left == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Left))
            _gameCore.Action(Direction.Left);
        else if (gamepadState.DPad.Right == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Right))
            _gameCore.Action(Direction.Right);
        else if (gamepadState.DPad.Up == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Up))
            _gameCore.Action(Direction.Up);
        else if (gamepadState.DPad.Down == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Down))
            _gameCore.Action(Direction.Down);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(transformMatrix: _viewportAdapter.GetScaleMatrix());

        FillViewportWithBackgroundColor();
        DrawTopPanel();

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void FillViewportWithBackgroundColor()
    {
        _spriteBatch.Draw(_emptyTexture, _viewportAdapter.BoundingRectangle, COLOR_BG_MAIN);
    }

    private void DrawTopPanel()
    {
        _title.Draw();
        _previousMoveButton.Draw();
        _newGameButton.Draw();
    }
}
