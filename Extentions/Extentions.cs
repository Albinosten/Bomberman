using Microsoft.Xna.Framework;

namespace BombermanExtention
{
    public static class Extentions
    {
        public static Vector2 AsVector(this (int,int) input)
        {
            return new Vector2(input.Item1, input.Item2);
        }
    }
}