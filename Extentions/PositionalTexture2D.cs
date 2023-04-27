using Microsoft.Xna.Framework.Graphics;
using Bomberman;
using Microsoft.Xna.Framework;

namespace BombermanExtention
{

    public abstract class PositionalTexture2D : IPositionalTexture2D
    {
        protected Texture2D Texture{get;set;}
        protected IGraphicsDeviceManagerNew graphics;
        public PositionalTexture2D(Texture2D texture, IGraphicsDeviceManagerNew graphics)
        {
            this.Texture = texture;
            this.graphics = graphics;
            this.XSpeed = 1;
            this.YPos = 1;
            this.Scale = 1;
        }
        public Texture2D GetTexture()
        {
            return this.Texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawHelper.Draw(spriteBatch, this);
        }

        public float XPos {get;set;}
        public float Width => this.Texture.Width * this.Scale;
        public float YPos {get;set;}
        public float Height => this.Texture.Height * this.Scale;
        public float Scale {get;set;}
        public int XSpeed {get;set;}
        public int YSpeed {get;set;}
        
        public bool IsColiding {get;set;}
        public Vector2 Position => new Vector2(this.XPos, this.YPos);
        public Vector2 PositionCenter => new Vector2(this.XPos - (this.Width/2), this.YPos- (this.Height/2));
    }
}