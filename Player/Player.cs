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
        int XSpeed { get; }
        bool IsDead{ get; }
        void Kill();
        void Move(GameTime gameTime, KeyboardState keyboardState, Map map);
        Func<Map, double, bool> Move(Moves move);
        IPlayer Clone(Player player);
        IPlayer GetPlayerInNextPosition(double x, Moves move, bool checkCollition);
    }
    public enum Players
    {
        One,
        Two,
    }
    public class Player : PositionalTexture2D, IPlayer
    {
        private readonly IPlayerKeyboardInterpreter playerMovementInterpreter;
        public readonly ICollitionController collitionController;
        private readonly GraphicsDevice graphicsDevice;
        private readonly IBombRayFactory bombRayFactory;
        private bool checkCollition;
        public static int s_width => 24;
        private int maxNumberOfBombs => 3;

        public bool IsDead { get; private set; }
        public string Name {get;set;}
        public Player(Color[] texture
            , IGraphicsDeviceManagerNew graphics
            , IPlayerKeyboardInterpreter playerMovementInterpreter
            , ICollitionController collitionController
            , GraphicsDevice graphicsDevice
            , IBombRayFactory bombRayFactory
            ): base(new Texture2D(graphicsDevice, s_width, s_width), graphics)
        {
            this.XPos = this.graphics.PreferredBackBufferWidth/2;
            this.XSpeed = 150;
            this.YSpeed = 150;
            this.playerMovementInterpreter = playerMovementInterpreter;
            this.collitionController = collitionController;
            this.graphicsDevice = graphicsDevice;
            this.bombRayFactory = bombRayFactory;
            checkCollition = true;
            if(texture != null)
            {
                this.GetTexture().SetData(texture);
            }
        }

        public void Move(GameTime gameTime, KeyboardState keyboardState, Map map)
        {
            if(this.IsDead)return;
            var moves = this.playerMovementInterpreter.GetMove(keyboardState, map, this, gameTime.ElapsedGameTime.TotalSeconds);
            foreach (Moves value in Enum.GetValues<Moves>().Where(x => moves.HasFlag(x)))
            {
                var move = this.Move(value);
                var result = move(map, gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
        public Func<Map, double, bool> Move(Moves move) => move switch
        {
            Moves.Up => (t,x) => this.MoveUp(t.Tiles,x),
            Moves.Down => (t,x) => this.MoveDown(t.Tiles,x),
            Moves.Left => (t,x) => this.MoveLeft(t.Tiles,x),
            Moves.Right => (t,x) => this.MoveRight(t.Tiles,x),

            Moves.Up | Moves.Right => (t,x) => this.MoveRight(t.Tiles,x) && this.MoveUp(t.Tiles, x),
            Moves.Up | Moves.Left => (t,x) => this.MoveLeft(t.Tiles,x) && this.MoveUp(t.Tiles, x),
            Moves.Down | Moves.Right => (t,x) => this.MoveRight(t.Tiles,x) && this.MoveDown(t.Tiles, x),
            Moves.Down | Moves.Left => (t,x) => this.MoveLeft(t.Tiles,x) && this.MoveDown(t.Tiles, x),

            Moves.Bomb => (t,x) => this.PlaceBomb(t),
            Moves.None => (t,x) => false,
            _ => (t,x) => false,
        };
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this.IsDead)return;
            DrawHelper.Draw(spriteBatch, this);
        }
        private bool PlaceBomb(Map map)
        {
            map.Bombs.Add(new Bomb(this.graphics
                , this.collitionController
                , this.graphicsDevice
                , this
                , this.bombRayFactory
                )
            {
                XPos = this.XPos + this.Width/2 - Bomb.s_width / 2,
                YPos = this.YPos + this.Height/2 - Bomb.s_width / 2,
            });
            return true;
        }
        private bool MoveRight(IList<IPositionalTexture2D> tiles, double x)
        {
            if(!this.checkCollition 
                || this.collitionController.CanMoveRight(tiles.ToList<IPositionalTexture2D>()
                    , this.GetPlayerInNextPosition(x, Moves.Right, false)
                    , this.graphics.PreferredBackBufferWidth
                    ))
            {
                this.XPos+=this.XSpeed*(float)x;
                return true;
            }
            return false;
        }
        private bool MoveLeft(IList<IPositionalTexture2D> tiles, double x)
        {
            if(!this.checkCollition 
                || this.collitionController.CanMoveLeft(tiles.ToList<IPositionalTexture2D>()
                    , this.GetPlayerInNextPosition(x, Moves.Left, false)
                    ))
            {
                this.XPos-=this.XSpeed*(float)x;
                return true;
            }
            return false;
        }
        public bool MoveUp(IList<IPositionalTexture2D> tiles, double x)
        {
            if(!this.checkCollition 
                || this.collitionController.CanMoveUp(tiles.ToList<IPositionalTexture2D>()
                    , this.GetPlayerInNextPosition(x, Moves.Up, false)
                    ))
            {
                this.YPos-=this.YSpeed*(float)x;
                return true;
            }
            return false;
        }
        public bool MoveDown(IList<IPositionalTexture2D> tiles, double x)
        {
            if(!this.checkCollition 
                || this.collitionController.CanMoveDown(tiles.ToList<IPositionalTexture2D>()
                    , this.GetPlayerInNextPosition(x, Moves.Down, false)
                    , this.graphics.PreferredBackBufferHeight
                    ))
            {
                this.YPos+=this.YSpeed*(float)x;
                return true;
            }
            return false;
        }
        public Player(Player player, IGraphicsDeviceManagerNew graphics, bool checkCollition)
            : this(null, graphics, null, null, player.graphicsDevice, null) 
            //Used for collition handling by creating a clone of player and apply movement
            //before checking collitions
            //Maybe its called raycasting? or maybe not.. because im not checking every position inbetweed. only last
        {
            this.checkCollition = checkCollition;
            this.XPos = player.XPos;
            this.YPos = player.YPos;
            this.XSpeed = (int)(player.XSpeed * 1);
            this.collitionController = player.collitionController;
        }
        public IPlayer GetPlayerInNextPosition(double x, Moves move, bool checkCollition)
        {
            var player = new Player(this, this.graphics, checkCollition);
            
            player.Move(move)(new Map(), x);
            
            return player;
        }
        public IPlayer Clone(Player player)
        {
            var newPlayer = new Player(this, this.graphics, checkCollition){checkCollition = true, Name = player.Name + " - Clone"};
            
            return newPlayer;
        }
        

        public void Kill()
        {
            this.IsDead = true;
        }
    }
}