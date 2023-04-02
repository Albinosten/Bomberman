using System;

namespace Bomberman
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] arg)
        {
            // 1. Bombs that shoots out a laser in every direction and slowly increases in lenght untill it hits something.
            // 2. powerups. Extra bomb, Extra long bomb, Bomb that explodes through walls
            // 3. block powerup to build walls

            // prestanda. just nu så har alla bombRay's kollitioncontroll med tilsen för att få coolaefekter.
                    //och jag spawnar ca 100 rays per bomb. hade ju gått att spawna större eldar istället i enbart x eller y
                    //och inte som nu i alla riktningar.
                    //Det kanske också går att ta bort ytter tile väggarna för att mindska antalet kontroller.

            // bot that plays
            using (var game = new Bomberman(new MapLoader()))
            {
                game.Run();
            }
        }
    }
}
