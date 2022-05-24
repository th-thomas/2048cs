using Game2048.Library;

namespace Game2048;

public static class Program
{
    [STAThread]
    static void Main()
    {
        var gameCore = new GameCore(4);
        using var game = new Game2048(gameCore);
        game.Subscribe(gameCore);
        game.Run();
    }
}
