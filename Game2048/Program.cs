using Game2048.Library;
using System.Reflection;

namespace Game2048;

public static class Program
{
    private const int GAME_SIZE = 4;

    [STAThread]
    static void Main()
    {
        var dirSep = Path.DirectorySeparatorChar;
        var saveFile = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}{dirSep}save.txt";
        var saveService = SaveService.GetSaveService(GAME_SIZE, saveFile);
        var gameCore = new GameCore(GAME_SIZE, saveService);
        using var game = new Game2048(gameCore);
        game.Subscribe(gameCore);
        game.Run();
    }
}
