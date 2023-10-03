using Microsoft.Xna.Framework.Input;

namespace Bomberman
{
    public class PlayerKeyboardInterpreter2 : IPlayerKeyboardInterpreter
    {
        private IPlayerKeyboardInterpreter aiPlayer;
        private bool useAiPlayer;
        public PlayerKeyboardInterpreter2(IPlayerKeyboardInterpreter aiPlayer)
        {
            this.aiPlayer = aiPlayer;
            this.useAiPlayer = true;
        }

        public Moves GetMove(KeyboardState keyboardState, Map map, IPlayer player, double gameTime)
        {
            if(keyboardState.IsKeyDown(Keys.O)){this.useAiPlayer = !this.useAiPlayer;};
            if(this.useAiPlayer && this.aiPlayer != null)
            {
                return this.aiPlayer.GetMove(keyboardState, map, player, gameTime);
            }
            var move = Moves.None;
            if(keyboardState.IsKeyDown(Keys.Up)) move |= Moves.Up;
            if(keyboardState.IsKeyDown(Keys.Left)) move |= Moves.Left;
            if(keyboardState.IsKeyDown(Keys.Right)) move |= Moves.Right;
            if(keyboardState.IsKeyDown(Keys.Down)) move |= Moves.Down;
            if(keyboardState.IsKeyDown(Keys.Enter)) move |= Moves.Bomb;

            return move;
        }
    }
}