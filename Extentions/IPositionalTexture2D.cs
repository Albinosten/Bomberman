using Microsoft.Xna.Framework;
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
        float Scale {get;}
        Texture2D GetTexture();
        Vector2 Position {get;}
        void Draw(SpriteBatch spriteBatch);
    }
}