using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.ViewportAdapters;
using static Game2048.Helpers.DisplayConstants;
using Game2048.Library;
using Game2048.Panels;
using Game2048.Helpers;
using Game2048.Buttons;
using static IButton;

namespace Game2048;

internal class Game2048 : Game, IObserver<IGameCore>
{
    #region Fields
    private ViewportAdapter _viewportAdapter;
    private SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _graphics;
    private readonly IGameCore _gameCore;
    private GameContent _gameContent;
    private readonly ICell?[,] _cells;
    private readonly ButtonsManager _buttonsManager = new();
    #endregion

    #region Drawables
    private InfoPanel _infoPanel;
    private MainPanel _mainPanel;
    #endregion

    internal Game2048(IGameCore gameCore)
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += Window_ClientSizeChanged;
        _gameCore = gameCore;
        _cells = new ICell[_gameCore.Size, _gameCore.Size];
    }

    protected override void Initialize()
    {
        _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, RESOLUTION.X, RESOLUTION.Y);
        _graphics.PreferredBackBufferWidth = RESOLUTION.X;
        _graphics.PreferredBackBufferHeight = RESOLUTION.Y;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    private void Window_ClientSizeChanged(object? sender, EventArgs e)
    {
        (_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight) = Window.ClientBounds switch
        {
            { Width: var width, Height: var height } when width < RESOLUTION_MIN.X || height < RESOLUTION_MIN.Y => (RESOLUTION_MIN.X, RESOLUTION_MIN.Y),
            _ => (Window.ClientBounds.Width, Window.ClientBounds.Height)
        };

        _graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _gameContent = new GameContent(Content, GraphicsDevice);
        _infoPanel = new InfoPanel(_buttonsManager, _viewportAdapter, _spriteBatch, _gameContent, new Rectangle(0, 0, RESOLUTION.X, 100));
        _mainPanel = new MainPanel(_spriteBatch, _gameContent, new Rectangle(0, 100, RESOLUTION.X, RESOLUTION.Y - 100));
        _gameCore.Init(true);
    }

    protected override void Update(GameTime gameTime)
    {
        var gamepadState = GamePad.GetState(PlayerIndex.One);
        var keyboardState = KeyboardExtended.GetState();
        var mouseState = MouseExtended.GetState();

        if (gamepadState.Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
            Exit();
        else if (gamepadState.Buttons.LeftShoulder == ButtonState.Pressed || keyboardState.WasKeyJustUp(Keys.F2) || _buttonsManager.PreviousMoveButton?.State == GameButtonState.Released)
        {
            _buttonsManager.PreviousMoveButton?.InvokedByKeyboardOrGamepad();
            _gameCore.LoadSavedGame(Save.PreviousMove);
        }
        else if (gamepadState.Buttons.RightShoulder == ButtonState.Pressed || keyboardState.WasKeyJustUp(Keys.F3) || _buttonsManager.NewGameButton?.State == GameButtonState.Released)
        {
            _buttonsManager.NewGameButton?.InvokedByKeyboardOrGamepad();
            _gameCore.Init(true);
        }
        else if (gamepadState.DPad.Left == ButtonState.Pressed || keyboardState.WasKeyJustUp(Keys.Left))
            _gameCore.Action(Direction.Left);
        else if (gamepadState.DPad.Right == ButtonState.Pressed || keyboardState.WasKeyJustUp(Keys.Right))
            _gameCore.Action(Direction.Right);
        else if (gamepadState.DPad.Up == ButtonState.Pressed || keyboardState.WasKeyJustUp(Keys.Up))
            _gameCore.Action(Direction.Up);
        else if (gamepadState.DPad.Down == ButtonState.Pressed || keyboardState.WasKeyJustUp(Keys.Down))
            _gameCore.Action(Direction.Down);

        _buttonsManager.PreviousMoveButton?.Update(gamepadState, keyboardState, mouseState);
        _buttonsManager.NewGameButton?.Update(gamepadState, keyboardState, mouseState);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(transformMatrix: _viewportAdapter.GetScaleMatrix());

        _spriteBatch.DrawColoredRectangle(_viewportAdapter.BoundingRectangle, COLOR_BG_MAIN, _gameContent.EmptyTexture);
        _infoPanel.Draw();
        _mainPanel.Draw();

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    #region Observer implementation
    private IDisposable _cancellation;

    public void OnNext(IGameCore gameCore)
    {
        _infoPanel.UpdateScore(gameCore.Score);
        _infoPanel.UpdateHighScore(gameCore.HighScore);
        for (var row = 0; row < gameCore.Size; row++)
        {
            for (var col = 0; col < gameCore.Size; col++)
            {
                _cells[row, col] = gameCore.GetCell(row, col);
            }
        }
        _mainPanel.Cells = _cells;
        _mainPanel.GameState = gameCore.GameState;
    }

    public void OnCompleted()
    {
        throw new NotImplementedException();
    }

    public void OnError(Exception error)
    {
        throw new NotImplementedException();
    }

    public virtual void Subscribe(IGameCore provider)
    {
        _cancellation = provider.Subscribe(this);
    }

    public virtual void Unsubscribe()
    {
        _cancellation.Dispose();
    }
    #endregion
}
