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
            this.GetTexture().SetData(TextureCreator.AddCircleWithColor(s_width,s_width/2, null, Color.Black));
        }
         
        public override void Draw(SpriteBatch spriteBatch)
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
            
            if(totalSeconds - (this.creationtime ?? 0f) > 3 //3 
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
            if(gameTime.TotalGameTime.TotalSeconds - (this.creationtime ?? 0f) > 5) //5
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
    public partial class BombRayFactory : IBombRayFactory
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
                    pixel.Update(5f);
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
    }
}