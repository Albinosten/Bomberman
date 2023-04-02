using Microsoft.Xna.Framework.Graphics;

namespace Bomberman
{
    public interface ITextureWidthAndHeight
    {
        float Width {get;}
        float Height{get;}

    }
    public interface IPositionalTexture2D : ITextureWidthAndHeight
    {
        float XPos {get;set;}
        float YPos {get;set;}
        Texture2D GetTexture();
    }
}