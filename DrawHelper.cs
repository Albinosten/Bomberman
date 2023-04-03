using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman
{
    public static class DrawHelper
    {
        public static void Draw(SpriteBatch spriteBatch, IPositionalTexture2D texture)
        {
            var pos = new Vector2(texture.XPos,texture.YPos);
            spriteBatch.Draw(texture.GetTexture()
                , position: pos
                , sourceRectangle: null
                , color: Color.White
                , rotation: 0f
                , origin: Vector2.Zero
                , scale: texture.Scale
                , effects: SpriteEffects.None
                , layerDepth: 0f
                );
        }
    }
}