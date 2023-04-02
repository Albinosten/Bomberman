using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BombermanExtention;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Bomberman
{
    public interface IPlayer : IPositionalTexture2D
    {
        int XSpeed {get;}
        void Move(GameTime gameTime, KeyboardState keyboardState, Map map);
        void Draw(SpriteBatch spriteBatch);
    }
    public class Player : PositionalTexture2D, IPlayer
    {
        private readonly IPlayerKeyboardInterpreter playerMovementInterpreter;
        private readonly ICollitionController collitionController;
        private readonly GraphicsDevice graphicsDevice;
        private bool checkCollition;
        private int maxNumberOfBombs => 3;

        public Player(Texture2D texture
            , IGraphicsDeviceManagerNew graphics
            , IPlayerKeyboardInterpreter playerMovementInterpreter
            , ICollitionController collitionController
            , GraphicsDevice graphicsDevice
            ): base(texture, graphics)
        {
            this.XPos = this.graphics.PreferredBackBufferWidth/2;
            this.XSpeed = 150;
            this.YSpeed = 150;
            this.scale = 0.3f;
            this.playerMovementInterpreter = playerMovementInterpreter;
            this.collitionController = collitionController;
            this.graphicsDevice = graphicsDevice;
            checkCollition = true;
        }

        public void Move(GameTime gameTime, KeyboardState keyboardState, Map map)
        {
            var moves = this.playerMovementInterpreter.GetMove(keyboardState);
            foreach (Moves value in Enum.GetValues<Moves>().Where(x => moves.HasFlag(x)))
            {
                var move = this.Move(value);
                move(map, gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        private Action<Map, double> Move(Moves move) => move switch
        {
            Moves.Up => (t,x) => this.MoveUp(t.Tiles,x),
            Moves.Down => (t,x) => this.MoveDown(t.Tiles,x),
            Moves.Left => (t,x) => this.MoveLeft(t.Tiles,x),
            Moves.Right => (t,x) => this.MoveRight(t.Tiles,x),
            Moves.Bomb => (t,x) => this.PlaceBomb(t),
            Moves.None => (t,x) => {},
            _ => (t,x) => {},
        };
        public void Draw(SpriteBatch spriteBatch)
        {
            var pos = new Vector2(this.XPos,this.YPos);
            spriteBatch.Draw(this.GetTexture()
                , position: pos
                , sourceRectangle: null
                , color: Color.White
                , rotation: 0f
                , origin: Vector2.Zero
                , scale: this.scale
                , effects: SpriteEffects.None
                , layerDepth: 0f
                );
        }
        private void PlaceBomb(Map map)
        {
            map.Bombs.Add(new Bomb(this.graphics
                , this.collitionController
                , this.graphicsDevice
                , this
                )
            {
                XPos = this.XPos,
                YPos = this.YPos,
            });
        }
        private void MoveRight(IList<ITile> tiles, double x)
        {
            if(!this.checkCollition 
                || this.collitionController.CanMoveRight(tiles.ToList<IPositionalTexture2D>()
                    , this.GetPlayerInNextPosition(x, Moves.Right)
                    , this.graphics.PreferredBackBufferWidth
                    ))
            {
                this.XPos+=this.XSpeed*(float)x;
            }
        }
        private void MoveLeft(IList<ITile> tiles, double x)
        {
            if(!this.checkCollition 
                || this.collitionController.CanMoveLeft(tiles.ToList<IPositionalTexture2D>()
                    , this.GetPlayerInNextPosition(x, Moves.Left)
                    ))
            {
                this.XPos-=this.XSpeed*(float)x;
            }
        }
        public void MoveUp(IList<ITile> tiles, double x)
        {
            if(!this.checkCollition 
                || this.collitionController.CanMoveUp(tiles.ToList<IPositionalTexture2D>()
                    , this.GetPlayerInNextPosition(x, Moves.Up)
                    ))
            {
                this.YPos-=this.YSpeed*(float)x;
            }
        }
        public void MoveDown(IList<ITile> tiles, double x)
        {
            if(!this.checkCollition 
                || this.collitionController.CanMoveDown(tiles.ToList<IPositionalTexture2D>()
                    , this.GetPlayerInNextPosition(x, Moves.Down)
                    , this.graphics.PreferredBackBufferHeight
                    ))
            {
                this.YPos+=this.YSpeed*(float)x;
            }
        }
        public Player(IPlayer player, IGraphicsDeviceManagerNew graphics) 
            : this(player.GetTexture(), graphics, null, null, null) 
            //Used for collition handling by creating a clone of player and apply movement
            //before checking collitions
            //Maybe its called raycasting? or maybe not.. because im not checking every position inbetweed. only last
        {
            this.checkCollition = false;
            this.XPos = player.XPos;
            this.YPos = player.YPos;
            this.XSpeed = (int)(player.XSpeed * 1);
        }
        private IPlayer GetPlayerInNextPosition(double x, Moves move)
        {
            var player = new Player(this, this.graphics);
            
            player.Move(move)(new Map(), x);
            
            return player;
        }
    }
}