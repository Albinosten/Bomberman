using Microsoft.Xna.Framework;

namespace Bomberman
{
    public static class TextureCreator
    {
        public static  Color[] CreateTileTexture((int width, int height) size, Color color)
        {
            return CreateTileTexture(size.width, size.height, color);
        }
        public static  Color[] CreateTileTexture(int width, int height, Color color)
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
    }
}