using System;
using Tetris.Core;

namespace Tetris.DesktopGL
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using var game = new Game();
            game.Run();
        }
    }
}
