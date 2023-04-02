using Microsoft.Xna.Framework.Graphics;
using BombermanExtention;
using System;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Collections.Generic;


namespace Bomberman
{

    public interface IBomb : IPositionalTexture2D
    {
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime, Map map);
    }
    public class Bomb : PositionalTexture2D, IBomb
    {
        public static int s_width => 34;
        private float? creationtime;
        private static int s_numberOfRays => 120;
        // private static int s_numberOfRays => 1;

        private IList<IBombRay> bombRays;
        private readonly ICollitionController collitionController;
        private GraphicsDevice graphicsDevice;
        private readonly ITextureWidthAndHeight parent;

        public Bomb(IGraphicsDeviceManagerNew graphics
            , ICollitionController collitionController
            , GraphicsDevice graphicsDevice
            , ITextureWidthAndHeight parent
            ) : base(new Texture2D(graphicsDevice, s_width, s_width), graphics)
        {
            this.scale = 1f;
            this.bombRays = new List<IBombRay>();
            this.collitionController = collitionController;
            this.graphicsDevice = graphicsDevice;
            this.parent = parent;
            this.GetTexture().SetData(AddCircleWithColor(s_width,s_width/2, Color.Red));
        }
        public static Color[] AddCircleWithColor(int width
            , int circleRadius
            , Color color
            )
        {

            Color[] data = new Color[width*width];
            var y = 0;
            for(var i = 0; i< width*width;i++)
            {
                var x =  i%width;
                if(x == width-1)
                {
                    y++;
                }
                var valueWithOffset = GetValue(x, y, width);
                var valueWithoutOffset = GetValue(x,y, width);

                if(valueWithOffset > circleRadius*circleRadius || valueWithoutOffset > ((width/2)*(width/2)))
                {
                    //utanför cirkeln
                 //   data[i] = Color.Transparent;
                }
                else
                {
                    //här är i cirkeln
                    data[i] = color;

                }
                
            }
            return data;
        }
         private static int GetValue(int x,int y, int width)
        {
            x = Math.Max(0, x);
            y = Math.Max(0, y);
            var deltax = Math.Abs(x-(width/2));
            var deltay = Math.Abs(y-(width/2));

            return deltax * deltax + deltay * deltay;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var pos = new Vector2(this.XPos,this.YPos);
            // spriteBatch.Draw(this.GetTexture()
            //     , position: pos
            //     , sourceRectangle: null
            //     , color: Color.White
            //     , rotation: 0f
            //     , origin: Vector2.Zero
            //     , scale: this.scale
            //     , effects: SpriteEffects.None
            //     , layerDepth: 0f
            //     );
            for(int i = 0; i < this.bombRays.Count; i++)
            {
                this.bombRays[i].Draw(spriteBatch);
            }
        }

        private bool isExploded = false;
        public void Update(GameTime gameTime, Map map)
        {
            if(isExploded)
            {
                return;
            }
            if(!this.creationtime.HasValue)
            {
                this.creationtime = (float)gameTime.TotalGameTime.TotalSeconds;
            }
            if(gameTime.TotalGameTime.TotalSeconds - (this.creationtime ?? 0f) > 3 
                && this.bombRays.Count == decimal.Zero)
            {
                float rotation = 0;
                float rotationStep = (float)s_numberOfRays / 6;
                
                var texture = new Texture2D(graphicsDevice, BombRay.s_width, BombRay.s_height);
                texture.SetData(GetColorData(BombRay.s_width, BombRay.s_height));

                for(int i = 0; i < s_numberOfRays; i++)
                {
                    this.bombRays.Add(new BombRay(this.graphics
                        , this.collitionController
                        , this.graphicsDevice
                        , texture
                        , rotation
                        )
                        {
                            XPos = this.XPos + (this.parent.Width / 2),
                            YPos = this.YPos + (this.parent.Height / 2),
                        });
                    rotation+=rotationStep;
                }
            }
            for(int i = 0; i < this.bombRays.Count; i++)
            {
                this.bombRays[i].Update(gameTime, map);
            }
            if(gameTime.TotalGameTime.TotalSeconds - (this.creationtime ?? 0f) > 10)
            {
                this.bombRays.Clear();
                this.isExploded = true;
            }
        }

        private static Color[] GetColorData(int width
            , int height
            )
        {
            Color[] data = new Color[width*width];
            for(var i = 0; i< width*width;i++)
            {
                data[i] = Color.Purple;
            }
            return data;
        }
    }
}