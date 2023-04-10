using Microsoft.Xna.Framework.Graphics;
using BombermanExtention;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Bomberman
{

    public interface IBombRay : IPositionalTexture2D
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime, Map map);
    }
    public class BombRay : PositionalTexture2D , IBombRay
    {
        private float lenght;
        public static int s_width => 1;
        public static int s_height => 1;
        public static int s_speed => 90;

        private readonly ICollitionController collitionController;
        private float rotation;
        private readonly float maxLenght;

        public BombRay(IGraphicsDeviceManagerNew graphics
            , ICollitionController collitionController
            , GraphicsDevice graphicsDevice
            , Texture2D texture
            , float rotation
            , float maxLenght
            ) : base(texture, graphics)
        {
            this.collitionController = collitionController;
            this.rotation = rotation;
            this.maxLenght = maxLenght;
        }
        private Vector2? startPos;

        public void Draw(SpriteBatch spriteBatch)
        {
            var pos = new Vector2(this.XPos,this.YPos);
            Line.DrawLine(spriteBatch
                , this.startPos ?? pos
                , pos
                , Color.Red
                , 10
                );
        }
        
        private bool hasColided;
        public void Update(GameTime gameTime, Map map)
        {
            foreach(var player in map.Players.Where(x => this.collitionController.HasColition(this, x)))
            {
                player.Kill();
            }

            this.startPos = this.startPos ?? new Vector2(this.XPos, this.YPos);
            // if(!this.hasColided && !this.collitionController.HasColition(map.Tiles.ToList<IPositionalTexture2D>(), this))
            // {
            //     this.lenght += (float)gameTime.ElapsedGameTime.TotalSeconds * s_speed;
            //     this.XPos = this.startPos.Value.X + (this.lenght * (float)Math.Cos(this.rotation));
            //     this.YPos = this.startPos.Value.Y + (this.lenght * (float)Math.Sin(this.rotation));
            // }
            // else
            // {
            //     this.hasColided = true;
            // }
            if(this.lenght < this.maxLenght)
            {
                this.lenght += (float)gameTime.ElapsedGameTime.TotalSeconds * s_speed;
                this.XPos = this.startPos.Value.X + (this.lenght * (float)Math.Cos(this.rotation));
                this.YPos = this.startPos.Value.Y + (this.lenght * (float)Math.Sin(this.rotation));
            }
        }
    }
}