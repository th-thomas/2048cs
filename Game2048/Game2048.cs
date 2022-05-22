using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.ViewportAdapters;
using Game2048.Library;

namespace Game2048;

public class Game2048 : Game
{
    private const int RESOLUTION_X = 800;
    private const int RESOLUTION_Y = 900;

    private readonly GraphicsDeviceManager _graphics;
    
    private ViewportAdapter? _viewportAdapter;
    private SpriteBatch? _spriteBatch;
    private IGameCore? _gameCore;

    public Game2048()
    {
        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = RESOLUTION_X,
            PreferredBackBufferHeight = RESOLUTION_Y
        };

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
    }

    protected override void Initialize()
    {
        _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, RESOLUTION_X, RESOLUTION_Y);
        _gameCore = new GameCore(4);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here
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

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
