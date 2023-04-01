using Microsoft.Xna.Framework.Graphics;

namespace Bomberman
{
    public interface IPositionalTexture2D
    {
        float XPos {get;set;}
        float Width {get;}
        float YPos {get;set;}
        float Height{get;}
        Texture2D GetTexture();
    }
}