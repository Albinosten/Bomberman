using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BombermanExtention
{
    public static class TextureCreator
    {
        public static Texture2D CreateBombrayTexture(int width, int height,GraphicsDevice graphicsDevice)
        {
            var texture = new Texture2D(graphicsDevice, width, height);
            texture.SetData(GetColorData(width, height, Color.Purple));
            return texture;
        }
        public static  Color[] CreateTileTextureData((int width, int height) size, Color color)
        {
            return CreateTileTextureData(size.width, size.height, color);
        }
        public static  Color[] CreateTileTextureData(int width, int height, Color color)
        {
            Color[] data = new Color[width*height];
            for(int i=0; i < data.Length; ++i) 
            {
                var mod = i%width;
                var mody = (i/height) % height;
                if(mod < 2 
                    || mod > width - 3 
                     || i < width*2 
                     || i > data.Length - (width*2)
                    )
                {
                    data[i] = Color.Black;
                }
                else if(i % 10 < 3)
                {
                    data[i] = Color.Red;
                }
                else
                {
                    data[i] = color;

                }
            }
            return data;
        }
        private static Color[] GetColorData(int width
            , int height
            , Color color
            )
        {
            Color[] data = new Color[width*width];
            for(var i = 0; i< width*width;i++)
            {
                data[i] = color;
            }
            return data;
        }
        

        
        public static Color[] AddCircleWithColor(int width
            , int circleRadius
            , Color[] data
            , Color color
            )
        {
            data = data == null 
                ? new Color[width*width] 
                : data;
            var y = 0;
            for(var i = 0; i< width*width;i++)
            {
                var x =  i%width;
                if(x == width-1)
                {
                    y++;
                }
                var valueWithOffset = GetValue(x, y, width);
                var valueWithoutOffset = GetValue(x, y, width);

                if(valueWithOffset > circleRadius*circleRadius || valueWithoutOffset > ((width/2)*(width/2)))
                {
                    //utanför cirkeln
                 //   data[i] = Color.Transparent;
                }
                else
                {
                    //här är i cirkeln
                    data[i] = color;

                }
                
            }
            return data;
        }
        private static int GetValue(int x,int y, int width)
        {
            x = Math.Max(0, x);
            y = Math.Max(0, y);
            var deltax = Math.Abs(x-(width/2));
            var deltay = Math.Abs(y-(width/2));

            return deltax * deltax + deltay * deltay;
        }
    }
}