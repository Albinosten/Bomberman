using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BombermanExtention;

namespace Bomberman
{
    public class TileWall : PositionalTexture2D, ITile
    {
        public TileWall(IGraphicsDeviceManagerNew graphics
            , Texture2D texture
            ) : base(texture, graphics)
        {
            this.Scale = 1f;
            this.GetTexture()
                .SetData(TextureCreator
                    .CreateTileTextureData((int)this.Width, (int)this.Height, Color.Gray));

        }
        private static (int, int) getSize((int x, int y) mapSize)
        {
            return (mapSize.x * Tile.s_width, mapSize.y * Tile.s_height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawHelper.Draw(spriteBatch, this);
        }
    }
}