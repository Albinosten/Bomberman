using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class Ball : PositionalTexture2D
    {
        public bool DebugOutput {get;set;}
        private const float maxJumpHeight = 40;
        private const float maxJumpTime = 2;
        private const float jumpingLockTime = 1.2f;
        private const int jumpSpeed = 3;
        private float jumpingStatingPosition;
        public Ball(Texture2D texture,IGraphicsDeviceManagerNew graphics): base(texture, graphics)
        {
            this.jumpingStatingPosition = graphics.PreferredBackBufferHeight/2;
            this.XPos = this.graphics.PreferredBackBufferWidth/2;
            this.XSpeed = 150;
            this.YSpeed = 90;
            this.scale = 0.3f;
        }


    }
     public interface IGraphicsDeviceManagerNew : IGraphicsDeviceService, IGraphicsDeviceManager
    {
        int PreferredBackBufferWidth {get;}
        int PreferredBackBufferHeight {get;}
        bool IsFullScreen {get;set;}

        void ApplyChanges();
    }
    public class GraphicsDeviceManagerNew : GraphicsDeviceManager , IGraphicsDeviceManagerNew
    {
        public GraphicsDeviceManagerNew(Game game)
            :base(game)
        {
            
        }
    }
    public interface IPositionalTexture2D
    {
        float XPos {get;set;}
        float Width {get;}
        float YPos {get;set;}
        float Height{get;}
        Texture2D GetTexture();
    }

    public class PositionalTexture2D : IPositionalTexture2D
    {
        protected Texture2D Texture{get;set;}
        protected IGraphicsDeviceManagerNew graphics;
        public PositionalTexture2D(Texture2D texture, IGraphicsDeviceManagerNew graphics)
        {
            this.Texture = texture;
            this.graphics = graphics;
            this.XSpeed = 1;
            this.YPos = 1;
            this.scale = 1;
        }
        public Texture2D GetTexture()
        {
            return this.Texture;
        }
        public float XPos {get;set;}
        public float Width => this.Texture.Width * this.scale;
        public float YPos {get;set;}
        public float Height => this.Texture.Height * this.scale;
        public float scale {get;set;}
        public int XSpeed {get;set;}
        public int YSpeed {get;set;}
        

        
        public bool IsColiding {get;set;}
        
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


        public bool CanMoveLeft()
        {
            if(this.XPos > 0 ) //wall collision
            {
                return true;
            }
            return false;
        }
        public bool CanMoveRight(int maxWidth)
        {
            if(this.XPos+this.Width <  maxWidth) //wall collision
            {
                return true;
            }
            return false;
        }
        public bool CanMoveUp()
        {
            if(this.YPos > 0) //wall collision
            {
                return true;
            }
            return false;
        }
        public bool CanMoveDown(int maxHeight)
        {
            if(this.YPos + this.Height < maxHeight) //wall collision
            {
                return true;
            }
            return false;
        }

    }
}