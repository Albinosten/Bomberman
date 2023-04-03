using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Bomberman;

namespace BombermanExtention
{

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
            this.Scale = 1;
        }
        public Texture2D GetTexture()
        {
            return this.Texture;
        }
        public float XPos {get;set;}
        public float Width => this.Texture.Width * this.Scale;
        public float YPos {get;set;}
        public float Height => this.Texture.Height * this.Scale;
        public float Scale {get;set;}
        public int XSpeed {get;set;}
        public int YSpeed {get;set;}
        

        
        public bool IsColiding {get;set;}
        
    }
}