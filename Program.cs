using System;

namespace Bomberman
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] arg)
        {
            using (var game = new Bomberman())
            {
                game.Run();
            }
        }
    }
}
