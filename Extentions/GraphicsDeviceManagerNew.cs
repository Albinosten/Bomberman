using Microsoft.Xna.Framework;

namespace Bomberman
{
    public class GraphicsDeviceManagerNew : GraphicsDeviceManager , IGraphicsDeviceManagerNew
    {
        public GraphicsDeviceManagerNew(Game game)
            :base(game)
        {
            
        }
    }
}