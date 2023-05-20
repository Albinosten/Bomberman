using System;
using System.Collections.Generic;

namespace Bomberman
{
    public static class Program
    {
        [STAThread]
        static void Main(string[] arg)
        {
            // 2. powerups. Extra bomb, Extra long bomb, Bomb that explodes through walls
            // 3. block powerup to build walls
            
            //bara tre bomber i taget
            //sido moves
            
            // prestanda. just nu så har alla bombRay's kollitioncontroll med tilsen för att få coolaefekter.
                    //och jag spawnar ca 100 rays per bomb. hade ju gått att spawna större eldar istället i enbart x eller y
                    //och inte som nu i alla riktningar.

            //bombrays skulle kunna updatera bitmapen med vilka tiles som blir dödliga
                //då kan botten söka efter säkra rutor att hamna i.
            using (var game = new Bomberman(new MapLoader()))
            {
                game.Run();
            }
        }
    }
}
