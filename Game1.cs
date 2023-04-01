using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class Game1 : Game
    {
        private GraphicsDeviceManagerNew graphics;
        private SpriteBatch spriteBatch;
        private Ball myBall {get;set;}
        private Ball opponentBall {get;set;}
        public Game1()
        {
            this.graphics = new GraphicsDeviceManagerNew(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
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
            this.myBall = new Ball(this.Content.Load<Texture2D>("ball"), this.graphics);
            this.opponentBall = new Ball(this.Content.Load<Texture2D>("ball"), this.graphics);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            var keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.W))
            {
                this.myBall.MoveUp(gameTime.ElapsedGameTime.TotalSeconds);   
            }
            if(keyboardState.IsKeyDown(Keys.A))
            {
                this.myBall.MoveLeft(gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(keyboardState.IsKeyDown(Keys.S))
            {
                this.myBall.MoveDown(gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(keyboardState.IsKeyDown(Keys.D))
            {
                this.myBall.MoveRight(gameTime.ElapsedGameTime.TotalSeconds);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // TODO: Add your drawing code here
            this.spriteBatch.Begin();

            var myBallVector = new Vector2(this.myBall.XPos,this.myBall.YPos);
            spriteBatch.Draw(this.myBall.GetTexture()
                , position: myBallVector
                , sourceRectangle: null
                , color: Color.White
                , rotation: 0f
                , origin: Vector2.Zero
                , scale: this.myBall.scale
                , effects: SpriteEffects.None
                , layerDepth: 0f
                );


            var opponentBallVector = new Vector2(this.opponentBall.XPos,this.opponentBall.YPos);
            spriteBatch.Draw(this.opponentBall.GetTexture()
                , position: opponentBallVector
                , sourceRectangle: null
                , color: Color.Black
                , rotation: 0f
                , origin: Vector2.Zero
                , scale: this.opponentBall.scale
                , effects: SpriteEffects.None
                , layerDepth: 0f
                );

            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
