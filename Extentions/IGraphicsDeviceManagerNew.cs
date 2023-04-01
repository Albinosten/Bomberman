using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman
{
    public interface IGraphicsDeviceManagerNew : IGraphicsDeviceService, IGraphicsDeviceManager
    {
        int PreferredBackBufferWidth {get;}
        int PreferredBackBufferHeight {get;}
        bool IsFullScreen {get;set;}

        void ApplyChanges();
    }
}