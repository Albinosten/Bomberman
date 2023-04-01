using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public interface IPlayerMovementInterpreter
    {
        void Move(GameTime gameTime, KeyboardState keyboardState, Player player);
    }
}