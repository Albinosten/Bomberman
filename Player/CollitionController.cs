using System.Collections.Generic;

namespace Bomberman
{
    public interface  ICollitionController
    {
        public bool CanMoveLeft(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player);
        bool CanMoveRight(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player, int maxWidth);
        bool CanMoveUp(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player);
        bool CanMoveDown(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player, int maxHeight);
        bool HasColition(IList<IPositionalTexture2D> tile, IPositionalTexture2D player);
        bool HasColition(IPositionalTexture2D tile, IPositionalTexture2D player);
    }
    public class CollitionController : ICollitionController
    {
        public bool CanMoveLeft(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player)
        {
            foreach (var tile in tiles)
            {
                if(this.CanMoveHorizontal(tile,player)
                    && this.CanMoveVertical(player, tile)
                    )
                {
                    return false;
                }
            }
            return (player.XPos > 0 ); //wall collision
        }
        public bool CanMoveRight(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player, int maxWidth)
        {
            foreach (var tile in tiles)
            {
                if(this.CanMoveHorizontal(player,tile)
                    && this.CanMoveVertical(player, tile)
                    )
                {
                    return false;
                }
            }
            return (player.XPos+player.Width <  maxWidth); //wall collision
        }
        public bool CanMoveUp(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player)
        {
            foreach (var tile in tiles)
            {
                if(this.CanMove(tile,player))
                {
                    return false;
                }
            }
            return (player.YPos > 0); //wall collision
        }
        public bool CanMoveDown(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player, int maxHeight)
        {
            foreach (var tile in tiles)
            {
                if(this.CanMove(tile,player))
                {
                    return false;
                }
            }
            return (player.YPos + player.Height < maxHeight); //wall collision
        }
        private bool CanMove(IPositionalTexture2D tile, IPositionalTexture2D player)
        {
            if(this.CanMoveHorizontal(player,tile)
                && this.CanMoveVertical(player, tile)
                )
            {
                return true;
            }
            return false;
        }
        private bool CanMoveVertical(IPositionalTexture2D object1, IPositionalTexture2D object2)
        {
            return object1.YPos < object2.YPos + object2.Height
                && object1.YPos + object1.Height > object2.YPos;
        }
        private bool CanMoveHorizontal(IPositionalTexture2D object1, IPositionalTexture2D object2)
        {
                return (int)object2.XPos < (int)object1.XPos + (int)object1.Width
                    && (int)object2.XPos + (int)object2.Width > (int)object1.XPos;
        }

        public bool HasColition(IPositionalTexture2D tile, IPositionalTexture2D player)
        {
            return this.CanMove(player, tile);
        }

        public bool HasColition(IList<IPositionalTexture2D> tiles, IPositionalTexture2D player)
        {
            foreach (var tile in tiles)
            {
                if(this.HasColition(tile, player))
                {
                    return true;
                }
            }
            return false;
        }
    }
}