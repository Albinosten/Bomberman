using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BombermanExtention;
namespace Bomberman
{
    public class Player : PositionalTexture2D
    {
        public bool DebugOutput {get;set;}
        private const float maxJumpHeight = 40;
        private const float maxJumpTime = 2;
        private const float jumpingLockTime = 1.2f;
        private const int jumpSpeed = 3;
        private float jumpingStatingPosition;
        IPlayerMovementInterpreter playerMovementInterpreter;
        public Player(Texture2D texture
            , IGraphicsDeviceManagerNew graphics
            , IPlayerMovementInterpreter playerMovementInterpreter
            ): base(texture, graphics)
        {
            this.jumpingStatingPosition = graphics.PreferredBackBufferHeight/2;
            this.XPos = this.graphics.PreferredBackBufferWidth/2;
            this.XSpeed = 150;
            this.YSpeed = 90;
            this.scale = 0.3f;
            this.playerMovementInterpreter = playerMovementInterpreter;
        }

        public void Move(GameTime gameTime, KeyboardState keyboardState)
        {
            this.playerMovementInterpreter.Move(gameTime, keyboardState, this);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            var myBallVector = new Vector2(this.XPos,this.YPos);
            spriteBatch.Draw(this.GetTexture()
                , position: myBallVector
                , sourceRectangle: null
                , color: Color.White
                , rotation: 0f
                , origin: Vector2.Zero
                , scale: this.scale
                , effects: SpriteEffects.None
                , layerDepth: 0f
                );
        }
        public void MoveRight(double x)
        {
            if(this.CanMoveRight(this.graphics.PreferredBackBufferWidth))
            {
                this.XPos+=this.XSpeed*(float)x;
            }
        }
        public void MoveLeft(double x)
        {
            if(this.CanMoveLeft())
            {
                this.XPos-=this.XSpeed*(float)x;
            }
        }
        public void MoveUp(double x)
        {
            if(this.CanMoveUp())
            {
                this.YPos-=this.YSpeed*(float)x;
            }
        }
        public void MoveDown(double x)
        {
            if(this.CanMoveDown(this.graphics.PreferredBackBufferHeight))
            {
                this.YPos+=this.YSpeed*(float)x;
            }
        }


        private bool CanMoveLeft()
        {
            return (this.XPos > 0 ); //wall collision
        }
        private bool CanMoveRight(int maxWidth)
        {
            return (this.XPos+this.Width <  maxWidth); //wall collision
        }
        private bool CanMoveUp()
        {
            return (this.YPos > 0); //wall collision
        }
        private bool CanMoveDown(int maxHeight)
        {
            return (this.YPos + this.Height < maxHeight); //wall collision
        }
    }
}