using System;

namespace Bomberman
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] arg)
        {
            bool isServer = false;
            if(arg.Length>0 && arg[0] == "1")
            {
                isServer = true;

            }
            Console.WriteLine("is server: " + isServer);

            using (var game = new Game1())
                game.Run();
        }
    }
}
