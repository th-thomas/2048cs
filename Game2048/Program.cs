using Game2048.Library;
using System.Reflection;

namespace Game2048;

internal static class Program
{
    private const int GAME_SIZE = 4;

    [STAThread]
    internal static void Main()
    {
        var dirSep = Path.DirectorySeparatorChar;
        var saveFile = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}{dirSep}save.txt";
        var saveService = SaveService.GetInstance(GAME_SIZE, saveFile);
        var gameCore = new GameCore(GAME_SIZE, saveService);
        using var game = new Game2048(gameCore);
        game.Subscribe(gameCore);
        game.Run();
    }
}
