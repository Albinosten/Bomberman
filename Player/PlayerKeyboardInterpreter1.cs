using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Bomberman
{
    public class PlayerKeyboardInterpreter1 : IPlayerKeyboardInterpreter
    {
        public Moves GetMove(KeyboardState keyboardState) 
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