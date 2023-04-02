using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BombermanExtention;

namespace Bomberman
{
    public interface ITile : IPositionalTexture2D
    {
        void Draw(SpriteBatch spriteBatch);
    }
    public class Tile : PositionalTexture2D, ITile
    {
        public static int s_height => 34;
        public static int s_width => 34;
        public Tile(IGraphicsDeviceManagerNew graphics
            , GraphicsDevice graphicsDevice
            ) : base(new Texture2D(graphicsDevice, s_height, s_width), graphics)
        {
            this.scale = 1f;
            this.GetTexture().SetData(this.GetTextureData(s_height,s_width, Color.Gray));
        }

        private Color[] GetTextureData(int width, int height, Color color)
        {
            Color[] data = new Color[width*height];
            for(int i=0; i < data.Length; ++i) 
            {
                var mod = i%width;
                var mody = (i/height) % height;
                if(mod < 2 
                    || mod > width - 3 
                     || i < width*2 
                     || i > data.Length - (width*2)
                    )
                {
                    data[i] = Color.Black;
                }
                /*
11001100
11001100
00110011
00110011

11001100 11001100 00110011 00110011
*/
                else if(i % 10 < 3
                )
                {
                    data[i] = Color.Red;
                }
                else
                {
                    data[i] = color;

                }
            }
            return data;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var myBallVector = new Vector2(this.XPos,this.YPos);
            spriteBatch.Draw(this.GetTexture()
                , position: myBallVector
                , sourceRectangle: null
                , color: Color.White
                , rotation: 0f
                , origin: Vector2.Zero
                , scale: this.scale
                , effects: SpriteEffects.None
                , layerDepth: 0f
                );
        }
    }
}