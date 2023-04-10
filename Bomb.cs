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
        bool IsExploded {get;}
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime, Map map);
    }
    public class Bomb : PositionalTexture2D, IBomb
    {
        public static int s_width => 18;
        private float? creationtime;
        public static int s_numberOfRays => 100;

        private IList<IBombRay> bombRays;
        private readonly ICollitionController collitionController;
        private GraphicsDevice graphicsDevice;
        private readonly ITextureWidthAndHeight parent;
        private readonly IBombRayFactory bombRayFactory;

        public Bomb(IGraphicsDeviceManagerNew graphics
            , ICollitionController collitionController
            , GraphicsDevice graphicsDevice
            , ITextureWidthAndHeight parent
            , IBombRayFactory bombRayFactory
            ) : base(new Texture2D(graphicsDevice, s_width, s_width), graphics)
        {
            this.Scale = 1f;
            this.bombRays = new List<IBombRay>();
            this.collitionController = collitionController;
            this.graphicsDevice = graphicsDevice;
            this.parent = parent;
            this.bombRayFactory = bombRayFactory;
            this.GetTexture().SetData(AddCircleWithColor(s_width,s_width/2, Color.Black));
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
            DrawHelper.Draw(spriteBatch, this);
            for(int i = 0; i < this.bombRays.Count; i++)
            {
                this.bombRays[i].Draw(spriteBatch);
            }
        }

        public bool IsExploded {get;private set;}
        public void Update(GameTime gameTime, Map map)
        {
            if(IsExploded)
            {
                return;
            }
            var totalSeconds = (float)gameTime.TotalGameTime.TotalSeconds;
            this.creationtime = this.creationtime 
                ?? totalSeconds;
            
            if(totalSeconds - (this.creationtime ?? 0f) > 3 
                && this.bombRays.Count == decimal.Zero)
            {
                this.bombRays = this.bombRayFactory
                    .Create(this.graphicsDevice
                    , this
                    , this.graphics
                    , this.collitionController
                    , map.Tiles.ToList<IPositionalTexture2D>()
                    );
            }
            for(int i = 0; i < this.bombRays.Count; i++)
            {
                this.bombRays[i].Update(gameTime, map);
            }
            if(gameTime.TotalGameTime.TotalSeconds - (this.creationtime ?? 0f) > 5)
            {
                this.bombRays.Clear();
                this.IsExploded = true;
            }
        }        
    }
    public interface IBombRayFactory
    {
        List<IBombRay> Create(GraphicsDevice graphicsDevice
            , IPositionalTexture2D parent
            , IGraphicsDeviceManagerNew graphics
            , ICollitionController collitionController
            , IList<IPositionalTexture2D> tiles
            );
    }
    public class BombRayFactory : IBombRayFactory
    {
        private readonly ICollitionController collitionController;

        public BombRayFactory(ICollitionController collitionController)
        {
            this.collitionController = collitionController;
        }
        public List<IBombRay> Create(GraphicsDevice graphicsDevice
            , IPositionalTexture2D parent
            , IGraphicsDeviceManagerNew graphics
            , ICollitionController collitionController
            , IList<IPositionalTexture2D> tiles
            )
        {
            var bombRays = new List<IBombRay>();
            float rotation = 0;
            float rotationStep = (float)Bomb.s_numberOfRays / 6;
            
            var texture =  TextureCreator
                .CreateBombrayTexture(BombRay.s_height
                    , BombRay.s_width
                    , graphicsDevice
                    );
            for(int i = 0; i < Bomb.s_numberOfRays; i++)
            {
                var pixel = new Pixel(new Vector2(parent.XPos + (parent.Width / 2),  parent.YPos + (parent.Height / 2))
                    , rotation
                    );

                while(!collitionController.HasColition(tiles,pixel))
                {
                    pixel.Update(10f);
                }
                bombRays.Add(new BombRay(graphics
                    , this.collitionController
                    , graphicsDevice
                    , texture
                    , rotation
                    , pixel.lenght
                    )
                    {
                        XPos = parent.XPos + (parent.Width / 2),
                        YPos = parent.YPos + (parent.Height / 2),
                    });
                rotation += rotationStep;
            }
            return bombRays;
        }
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
        }
    }
    
}