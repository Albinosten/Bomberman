using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;


namespace Bomberman
{
    public partial class BombRayFactory
    {
        private class Pixel : IPositionalTexture2D
        {
            public Pixel(Vector2 startPos, float rotation)
            {
                this.startPos = startPos;
                this.rotation = rotation;
                this.XPos = startPos.X;
                this.YPos = startPos.Y;
            }
            public float lenght;
            private Vector2 startPos;
            private readonly float rotation;

            public float XPos{get;set;}
            public float YPos {get;set;}
            public float Scale => 1;

            public float Width => 1;

            public float Height => 1;

            public Vector2 Position => new Vector2(this.XPos, this.YPos);

            public void Update(float lenght)
            {
                this.lenght += lenght;
                this.XPos = this.startPos.X + (this.lenght * (float)Math.Cos(this.rotation));
                this.YPos = this.startPos.Y + (this.lenght * (float)Math.Sin(this.rotation));
            }

            public Texture2D GetTexture()
            {
                throw new NotImplementedException();
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                throw new NotImplementedException();
            }

        }
    }
    
}