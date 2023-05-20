using Microsoft.Xna.Framework.Graphics;
using BombermanExtention;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Bomberman
{

    public interface IBombRay : IPositionalTexture2D
    {
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
        public static float s_maxLenght = 5 * Tile.s_height;
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
            this.maxLenght = Math.Min(maxLenght, s_maxLenght);
        }
        private Vector2? startPos;

        public override void Draw(SpriteBatch spriteBatch)
        {
            var pos = new Vector2(this.XPos,this.YPos);
            Line.DrawLine(spriteBatch
                , this.startPos ?? pos
                , pos
                , Color.Red
                , 10
                );
        }
        
        public void Update(GameTime gameTime, Map map)
        {
            foreach(var player in map.Players.Where(x => this.collitionController.HasColition(this, x)))
            {
                player.Kill();
            }

            this.startPos = this.startPos ?? new Vector2(this.XPos, this.YPos);
            
            if(this.lenght < this.maxLenght)
            {
                this.lenght += (float)gameTime.ElapsedGameTime.TotalSeconds * s_speed;
                this.XPos = this.startPos.Value.X + (this.lenght * (float)Math.Cos(this.rotation));
                this.YPos = this.startPos.Value.Y + (this.lenght * (float)Math.Sin(this.rotation));
            }
        }
    }
}