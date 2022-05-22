namespace Game2048;

public static class Program
{
    [STAThread]
    static void Main()
    {
        using var game = new Game2048();
        game.Run();
    }
}
