using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class PlayerMovementInterpreter1 : IPlayerMovementInterpreter
    {
        public void Move(GameTime gameTime, KeyboardState keyboardState, Player player)
        {
            if(keyboardState.IsKeyDown(Keys.W))
            {
                player.MoveUp(gameTime.ElapsedGameTime.TotalSeconds);   
            }
            if(keyboardState.IsKeyDown(Keys.A))
            {
                player.MoveLeft(gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(keyboardState.IsKeyDown(Keys.S))
            {
                player.MoveDown(gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(keyboardState.IsKeyDown(Keys.D))
            {
                player.MoveRight(gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}