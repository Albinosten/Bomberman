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
    }
}