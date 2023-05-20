using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Bomberman
{

    public class Bomberman : Game
    {
        private GraphicsDeviceManagerNew graphics;
        private SpriteBatch spriteBatch;
        private Map map;
        private IMapLoader mapLoader;
        private IBombRayFactory bombRayFactory;
        private CollitionController collitionController;
        public Bomberman(IMapLoader mapLoader)
        {
            this.graphics = new GraphicsDeviceManagerNew(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.mapLoader = mapLoader;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.collitionController = new CollitionController();
            this.bombRayFactory = new BombRayFactory(this.collitionController);

            base.Initialize();
        }

        private int mapNumber = 0;
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.map =  this.mapLoader
                .Load(this.graphics
                    , this.GraphicsDevice
                    , this.Content
                    , this.collitionController
                    , this.bombRayFactory
                    , this.mapNumber
                    );

            this.graphics.PreferredBackBufferHeight = this.map.TileCount.Hight * Tile.s_height;
            this.graphics.PreferredBackBufferWidth = this.map.TileCount.Width * Tile.s_width;
            this.graphics.ApplyChanges();
        }

        double timeout = 1;
        protected override void Update(GameTime gameTime)
        {
            timeout -= gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            var keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.R))
            {
                this.Initialize();
            }
            if(keyboardState.IsKeyDown(Keys.L))
            {
                
            }
            if(keyboardState.IsKeyDown(Keys.P))
            {
                map.PrintBitmap(map.GetBitMap(true));
            }
            if(keyboardState.IsKeyDown(Keys.N) && this.timeout < 0)
            {
                this.timeout = 1;
                this.mapNumber++;
                this.Initialize();
            }
            
            foreach(var player in map.Players)
            {
                player.Move(gameTime, keyboardState, map.Clone(player));
            }
            
            var explodedBombs = new List<IBomb>();
            foreach(var bomb in map.Bombs)
            {
                bomb.Update(gameTime, map);
                if(bomb.IsExploded)
                {
                    explodedBombs.Add(bomb);
                }
            }
            foreach (var exploded in explodedBombs)
            {
                map.Bombs.Remove(exploded);
            }
            var indexes = new List<int>(map.Bombs.Count);
            for(int i = 0; i < map.Bombs.Count; i++)
            {
                indexes.Add(i);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            foreach(var player in this.map.Players)
            {
                player.Draw(this.spriteBatch);
            }

            foreach(var tile in this.map.Tiles)
            {
                tile.Draw(this.spriteBatch);
            }

            foreach(var bomb in this.map.Bombs)
            {
                bomb.Draw(this.spriteBatch);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
