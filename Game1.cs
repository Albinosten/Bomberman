﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Bomberman
{
    public class Bomberman : Game
    {
        private GraphicsDeviceManagerNew graphics;
        private SpriteBatch spriteBatch;
        private Map map;
        private IMapLoader mapLoader;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            this.map =  this.mapLoader.Load(this.graphics, this.GraphicsDevice, this.Content);

            this.graphics.PreferredBackBufferHeight = this.map.TileCount.Hight * Tile.s_height;
            this.graphics.PreferredBackBufferWidth = this.map.TileCount.Width * Tile.s_width;
            this.graphics.ApplyChanges();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            
            var keyboardState = Keyboard.GetState();
            foreach(var player in map.Players)
            {
                player.Move(gameTime, keyboardState, map);
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            foreach(var player in this.map.Players )
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
