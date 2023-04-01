using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Bomberman
{
    public class Bomberman : Game
    {
        private GraphicsDeviceManagerNew graphics;
        private SpriteBatch spriteBatch;
        private IList<Player> players {get;set;}

        public Bomberman()
        {
            this.graphics = new GraphicsDeviceManagerNew(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.players = new List<Player>();
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
            this.players.Add(new Player(this.Content.Load<Texture2D>("ball"), this.graphics, new PlayerMovementInterpreter1()));
            this.players.Add(new Player(this.Content.Load<Texture2D>("ball"), this.graphics, new PlayerMovementInterpreter2()));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            
            var keyboardState = Keyboard.GetState();
            foreach(var player in players)
            {
                player.Move(gameTime, keyboardState);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            foreach(var player in players)
            {
                player.Draw(spriteBatch);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
