using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;
using static Game2048.Helpers.DisplayConstants;
using Game2048.Library;
using Game2048.Panels;
using Game2048.Helpers;

namespace Game2048;

internal class Game2048 : Game
{
    #region Fields
    private ViewportAdapter _viewportAdapter;
    private SpriteBatch _spriteBatch;
    private readonly GraphicsDeviceManager _graphics;
    private IGameCore _gameCore;
    private GameContent _gameContent;
    #endregion

    #region Drawables
    private InfoPanel _infoPanel;
    private MainPanel _mainPanel;
    #endregion

    internal Game2048()
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
        _infoPanel = new InfoPanel(_viewportAdapter, _spriteBatch, _gameContent, _gameCore, new Rectangle(0, 0, RESOLUTION_X, 100));
        _mainPanel = new MainPanel(_viewportAdapter, _spriteBatch, _gameContent, _gameCore, new Rectangle(0, 100, RESOLUTION_X, RESOLUTION_X));
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
        
        _infoPanel.Update();

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
}
