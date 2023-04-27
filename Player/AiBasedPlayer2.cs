using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
namespace Bomberman
{
    public class AiBasedPlayers : IPlayerKeyboardInterpreter
    {
        private readonly ICollitionController collitionController;
        private readonly IGraphicsDeviceManagerNew graphics;

        public AiBasedPlayers(ICollitionController collitionController
            , IGraphicsDeviceManagerNew graphics
        )
        {
            this.collitionController = collitionController;
            this.graphics = graphics;
        }
        private Moves? lastMove;
        double distanceSinceLastMove = 0;

        public Moves GetMove(KeyboardState keyboardState, Map map, IPlayer me, double gameTime)
        {
            // return Moves.None;
            //override ai controller
            var move = new PlayerKeyboardInterpreter2().GetMove(keyboardState,map,me,gameTime); 
            if(move != (Moves)0)
            {
                return move;
            }

            //0.3 Seconds best so far
            if(this.distanceSinceLastMove > 0.01)//try make distance based liek one tile or something.
            {
                distanceSinceLastMove = 0;
                lastMove = null;
            }
                // lastMove = null;

            if(!lastMove.HasValue)
            {
                var result = this.Solver(me, map.Players[0], map, gameTime);
                lastMove = result.Item2;
            }

            this.distanceSinceLastMove+=gameTime;
            return this.lastMove.Value;
        }
        IList<(int, int, Moves)> GetAdjacent((int x, int y, Moves firstMove) cord)
        {
            var result = new List<(int,int,Moves)>(4);
            foreach(var move in this.GetAdjacent((cord.x,cord.y)))
            {
                result.Add((move.Item1, move.Item2, cord.firstMove));
            }
            return result;
        }
        (int, int, Moves)[] GetAdjacent((int x, int y) cord)
        {
            return new []
            {
                (cord.x+1, cord.y, Moves.Right),
                (cord.x-1, cord.y, Moves.Left),
                (cord.x, cord.y+1, Moves.Down),
                (cord.x, cord.y-1, Moves.Up),
            };
        }
        (int, int, Moves) GetAdjacent((int x, int y) cord, Moves move) => move switch
        {
            Moves.Right => (cord.x+1, cord.y, Moves.Right),
            Moves.Left =>(cord.x-1, cord.y, Moves.Left),
            Moves.Down =>(cord.x, cord.y+1, Moves.Down),
            Moves.Up =>(cord.x, cord.y-1, Moves.Up),
        };
        private (int, int) GetPos(IPlayer player)
        {
            return ((int)(player.XPos/Tile.s_width), (int)(player.YPos/Tile.s_height));
        }
        private (int, Moves) Solver(IPlayer me, IPlayer opponent, Map map, double gameTime)
        {
            var bitmap = map.GetBitMap(false);
            var max = (bitmap[0].Count()-1, bitmap.Count-1);
            var visited = new HashSet<(int,int)>();

            var next = new HashSet<(int,int, Moves)>();
            var pos = GetPos(me);
            foreach(var move in this.GetPossibleMoves(map, me, map.GraphicMax, gameTime).OrderBy(x => x == this.lastMove))
            {
                var a = GetAdjacent(pos, move);
                next.Add(a);
                visited.Add((a.Item1, a.Item2));
            }
            Console.WriteLine("possible move:");
            foreach(var a in next)
            {
                Console.WriteLine(a);
            }

            return (0, Moves.None);

        }
        IEnumerable<(int, int, Moves)> FilterOutOfBound(IList<(int x, int y, Moves _)> cords, (int x, int y) max)
        {
            for(int i = 0; i < cords.Count(); i++)
            {
                var cord = cords[i];
                if(cord.x <= max.x && cord.y <= max.y && cord.x >= 0 && cord.y >= 0)
                {
                    yield return cord;
                }
            }
        }
        
        private IList<Moves> GetPossibleMoves(Map map, IPlayer player,(int maxHeight, int maxWidth) max, double gameTime)
        {
            var moves = new List<Moves>();
            var dist = gameTime *2;
            if(player.Clone((Player)player).Move(Moves.Up)(map,dist))moves.Add(Moves.Up);
            if(player.Clone((Player)player).Move(Moves.Left)(map,dist))moves.Add(Moves.Left);
            if(player.Clone((Player)player).Move(Moves.Right)(map,dist))moves.Add(Moves.Right);
            if(player.Clone((Player)player).Move(Moves.Down)(map,dist))moves.Add(Moves.Down);

            return moves;
        }
    }
}