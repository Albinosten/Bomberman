using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class PlayerKeyboardInterpreter1 : IPlayerKeyboardInterpreter
    {
        public Moves GetMove(KeyboardState keyboardState, Map _, IPlayer __, double gameTime)
        {
            var move = Moves.None;

            if(keyboardState.IsKeyDown(Keys.W)) move |= Moves.Up;
            if(keyboardState.IsKeyDown(Keys.A)) move |= Moves.Left;
            if(keyboardState.IsKeyDown(Keys.S)) move |= Moves.Down;
            if(keyboardState.IsKeyDown(Keys.D)) move |= Moves.Right;
            if(keyboardState.IsKeyDown(Keys.Space)) move |= Moves.Bomb;

            return move;
        }
    }
}