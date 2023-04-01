using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class PlayerMovementInterpreter2 : IPlayerMovementInterpreter
    {
        public void Move(GameTime gameTime, KeyboardState keyboardState, Player player)
        {
            if(keyboardState.IsKeyDown(Keys.Up))
            {
                player.MoveUp(gameTime.ElapsedGameTime.TotalSeconds);   
            }
            if(keyboardState.IsKeyDown(Keys.Left))
            {
                player.MoveLeft(gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(keyboardState.IsKeyDown(Keys.Down))
            {
                player.MoveDown(gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(keyboardState.IsKeyDown(Keys.Right))
            {
                player.MoveRight(gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    }
}