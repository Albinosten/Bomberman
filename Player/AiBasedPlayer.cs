using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Xna.Framework;
using BombermanExtention;

namespace Bomberman
{
    public class AiBasedPlayer : IPlayerKeyboardInterpreter
    {
        private readonly ICollitionController collitionController;
        private readonly IGraphicsDeviceManagerNew graphics;

        public AiBasedPlayer(ICollitionController collitionController
            , IGraphicsDeviceManagerNew graphics
        )
        {
            this.collitionController = collitionController;
            this.graphics = graphics;
        }
        private Moves? lastMove = Moves.None;
        double distanceSinceLastMove = 0;
        double timeBetweenBombs = 8000;
        double timeSinceLastBomb = 0;

        public Moves GetMove(KeyboardState keyboardState, Map map, IPlayer me, double gameTime)
        {
            //override ai controller
            var move = new PlayerKeyboardInterpreter2(null).GetMove(keyboardState,map,me,gameTime); 
            if(move != Moves.None)
            {
                return move;
            }

            this.timeSinceLastBomb += gameTime * 1000;
            var value = new Random().Next(0, (int)(timeBetweenBombs - timeSinceLastBomb));
            var opponent = this.GetPos(map.Players[0]);

            var distance = Vector2.Distance(GetPos(me).AsVector(),opponent.AsVector());
            if((value < 10 || distance <= 2) && this.timeSinceLastBomb > 2000)
            {
                this.timeSinceLastBomb = 0;
                return Moves.Bomb;
            }


            if(( lastMove.IsHorizontal() && (me.XPos % Tile.s_width)  >= (Tile.s_width - me.Width))
                || !lastMove.IsHorizontal() && (me.YPos % Tile.s_height) >= (Tile.s_height - me.Height)
                )
            {
                return lastMove.Value;
            }
            if(this.distanceSinceLastMove > 0.01)//try make distance based liek one tile or something.
            {
                distanceSinceLastMove = 0;
                lastMove = null;
            }
            if(!lastMove.HasValue)
            {
                var bitMap = map.GetBitMap(false);
                var result = this.Solver(me
                    , opponent
                    , map
                    , bitMap
                    , gameTime
                    , BitMapValue.None
                    );
                lastMove = result.Item2;
                if(result.Item2 == Moves.None)
                {
                    lastMove = this.Solver(me
                        , this.GetFirstSafeTile(bitMap, me)
                        , map
                        , bitMap
                        , gameTime
                        , BitMapValue.BombRay 
                            | BitMapValue.Bomb 
                            | BitMapValue.Player 
                            | BitMapValue.None
                        ).Item2;
                }
            }

            this.distanceSinceLastMove+=gameTime;
            return this.lastMove.Value;
        }
        (int y, int x) GetFirstSafeTile(IList<IList<BitMapValue>> bitmap, IPlayer tile)
        {
            var yStart = (int)tile.YPos/Tile.s_height;
            var xStart = (int)tile.XPos/Tile.s_width;
            float minDistance = int.MaxValue;
            (int y, int x) result = (0,0);
            for(int y = 0; y < bitmap.Count; y++)
            {
                for(int x = 0; x < bitmap[y].Count; x++)
                {
                    var distance = Vector2.Distance(new Vector2(yStart,xStart), new Vector2(y,x));
                    if(bitmap[y][x] == BitMapValue.None && distance < minDistance)
                    {
                        result = (y,x);
                        minDistance = distance;
                    }
                }
            }
            Console.WriteLine("Safest spot: Y: " + result.y + " X: " + result.x);
            Console.WriteLine("Distance: " + minDistance);

            return result;
        }
        IList<(int y, int x, Moves)> GetAdjacent((int y, int x, Moves firstMove) cord)
        {
            var result = new List<(int y,int x, Moves)>(4);
            foreach(var move in this.GetAdjacent((cord.y,cord.x)))
            {
                result.Add((move.y, move.x, cord.firstMove));
            }
            return result;
        }
        (int y, int x, Moves)[] GetAdjacent((int y, int x) cord)
        {
            return new []
            {
                (cord.y, cord.x+1,  Moves.Right),
                (cord.y, cord.x-1, Moves.Left),
                (cord.y+1, cord.x, Moves.Down),
                (cord.y-1, cord.x,  Moves.Up),
            };
        }
        (int y, int x, Moves) GetAdjacent((int y, int x) cord, Moves move) => move switch
        {
            Moves.Right => (cord.y, cord.x+1 , Moves.Right),
            Moves.Left => (cord.y, cord.x-1, Moves.Left),
            Moves.Down => (cord.y+1, cord.x, Moves.Down),
            Moves.Up => (cord.y-1, cord.x, Moves.Up),
            _ => (0,0,Moves.None),
        };
        private (int y, int x) GetPos(IPositionalTexture2D player)
        {
            return ((int)(player.YPos/Tile.s_height), (int)(player.XPos/Tile.s_width));
        }
        private (int, Moves) Solver(IPlayer me
            , (int y, int x) opponentPos
            , Map map
            , IList<IList<BitMapValue>> bitmap
            , double gameTime
            , BitMapValue validBitMapValue
            )
        {

            var visited = new HashSet<(int y,int x)>();

            //Feed with first possible moves
            var next = new HashSet<(int y, int x, Moves)>();
            foreach(var move in this.GetPossibleMoves(map, me, map.GraphicMax, gameTime) 
                .OrderByDescending(x => x == this.lastMove)
                )
            {
                var neighbourTiles = GetAdjacent(GetPos(me), move);
                if(CanMove(bitmap,neighbourTiles.y,neighbourTiles.x, validBitMapValue))
                {
                    next.Add(neighbourTiles);
                    visited.Add((neighbourTiles.y, neighbourTiles.x));
                }
            }
            var stepps = 0;
            while(next.Count > 0)
            {
                var nextBatch = new HashSet<(int y, int x, Moves)>();
                foreach (var current in next)
                {
                    if((current.y, current.x) == opponentPos)
                    {
                        return (stepps, current.Item3);
                    }
                
                    visited.Add((current.y, current.x));
                    foreach (var item in GetAdjacent(current)
                        .Where(x => CanMove(bitmap, x.y, x.x, validBitMapValue))
                        .Where(x => !visited.Contains((x.y, x.x))))
                    {
                        nextBatch.Add(item);
                    }

                }
                next = nextBatch;
                stepps++;
            }

            return (0, Moves.None);
        }
        bool CanMove(IList<IList<BitMapValue>> bitMap, int y, int x, BitMapValue validBitMapValue)
        {
            if(y < 0 || x < 0 || y >= bitMap.Count || x >= bitMap[0].Count){return false;}

            if(bitMap[y][x] == BitMapValue.None)
            {
                return true;
            }
            return validBitMapValue.HasFlag(bitMap[y][x]);

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