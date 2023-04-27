using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BombermanExtention;

namespace Bomberman
{
    public interface ITile : IPositionalTexture2D
    {
    }
    public class Tile : PositionalTexture2D, ITile
    {
        public static int s_height => 35;
        public static int s_width => 35;
        public Tile(IGraphicsDeviceManagerNew graphics
            , GraphicsDevice graphicsDevice
            , Texture2D texture
            ) : base(texture, graphics)
        {
            this.Scale = 1f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var myBallVector = new Vector2(this.XPos,this.YPos);
            spriteBatch.Draw(this.GetTexture()
                , position: myBallVector
                , sourceRectangle: null
                , color: Color.White
                , rotation: 0f
                , origin: Vector2.Zero
                , scale: this.Scale
                , effects: SpriteEffects.None
                , layerDepth: 0f
                );
        }
    }
}