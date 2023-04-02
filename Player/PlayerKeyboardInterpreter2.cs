using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class PlayerKeyboardInterpreter2 : IPlayerKeyboardInterpreter
    {
        public Moves GetMove(KeyboardState keyboardState) 
        {
            var move = Moves.None;
            if(keyboardState.IsKeyDown(Keys.Up)) move |= Moves.Up;
            if(keyboardState.IsKeyDown(Keys.Left)) move |= Moves.Left;
            if(keyboardState.IsKeyDown(Keys.Right)) move |= Moves.Right;
            if(keyboardState.IsKeyDown(Keys.Down)) move |= Moves.Down;
            if(keyboardState.IsKeyDown(Keys.Enter)) move |= Moves.Bomb;

            return move;
        }
    }
}