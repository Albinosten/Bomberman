using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    [Flags]
    public enum Moves
    {
        None = 0,
        Up = 1,
        Down = 2,
        Left = 4,
        Right = 8,
        Bomb = 16,
    }
    public static class MovesExtentions
    {
        public static bool IsHorizontal(this Moves? move)
        {
            return move.HasValue ?  (move.Value.HasFlag(Moves.Left) || move.Value.HasFlag(Moves.Right)) : false;
        }
    }
    public interface IPlayerKeyboardInterpreter
    {
        Moves GetMove(KeyboardState keyboardState, Map map, IPlayer player, double gameTime);
    }
}