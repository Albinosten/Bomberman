using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using BombermanExtention;

namespace Bomberman
{
    public static class MapExtention
    {
        public static Map Clone(this Map map, IPlayer player)
        {
            return new Map
            {
                Tiles = map.Tiles,
                Bombs = map.Bombs,
                Players = map.Players.Where(x => x != player).ToList(),
                TileCount = map.TileCount,
                GraphicMax = map.GraphicMax,
            };
        }
    }

    [Flags]
    public enum BitMapValue
    {
        None = 0,
        Tile = 1,
        Player = 2,
        Bomb = 4,
        BombRay = 8,
    }
    public class Map
    {
        private static int s_bombRayLenght = (int)(BombRay.s_maxLenght / Tile.s_height);
        public (int Hight, int Width) TileCount {get;set;}
        public (int Hight, int Width) GraphicMax {get;set;}
        public Map()
        {
            this.Tiles = new List<IPositionalTexture2D>();
            this.Players = new List<IPlayer>();
            this.Bombs = new List<IBomb>();
        }
        public IList<IPositionalTexture2D> Tiles{get;set;}
        public IList<IPlayer> Players {get;set;}
        public IList<IBomb> Bombs {get;set;}
        public void PrintBitmap(IList<IList<BitMapValue>> map)
        {
            Console.WriteLine("*****************");

            for(int y = 0; y < map.Count; y++)
            {
                Console.WriteLine();
                for(int x = 0; x < map[y].Count; x++)
                {
                    Console.Write((int)map[y][x]);
                }
            }
        }

        public IList<IList<T>> SubDivide<T>(IList<IList<T>> input, int factor)
        {
            var result = new List<IList<T>>();
            for(int y = 0; y < input.Count * factor; y++)
            {
                result.Add(new List<T>());
                for(int x = 0; x < input[0].Count * factor; x++)
                {
                    int xvalue = (int)x/factor;
                    var yvalue = (int)y/factor;
                    var value = input[yvalue][xvalue];
                    result[y].Add(value);
                }
            }
            return result;
        }
        public IList<IList<BitMapValue>> GetBitMap(bool withPlayers)
        {
            var result = this.CreateBitmap();
            result = this.AddBombUsingDistance(result);
            result = this.AddBoarderTiles(result);
            foreach(var tile in this.Tiles)
            {
                var y = (int)tile.YPos/Tile.s_height;
                var x = (int)tile.XPos/Tile.s_width;
                result[y][x] = BitMapValue.Tile;
            }
            if(withPlayers)
            {
                foreach(var player in this.Players)
                {
                    var x = (int)player.XPos/Tile.s_width;
                    var y = (int)player.YPos/Tile.s_height;
                    result[y][x] = BitMapValue.Player;
                }
            }
            return result;
        }
        
        private IList<IList<BitMapValue>> CreateBitmap()
        {
            var result = new List<IList<BitMapValue>>();

            for(int y = 0; y < this.TileCount.Hight; y++)
            {
                result.Add(new List<BitMapValue>());   
                for(int x = 0; x < this.TileCount.Width; x++)
                {
                    result[y].Add(BitMapValue.None);
                }
            }
            return result;
        }
        private IList<IList<BitMapValue>> AddBoarderTiles(IList<IList<BitMapValue>> result)
        {
            for(int y = 0; y < result.Count; y++)
            {
                for(int x = 0; x < result[y].Count; x++)
                {
                    if(y == 0 
                        || y == this.TileCount.Hight - 1
                        || x == 0
                        || x == this.TileCount.Width -1
                        ) 
                    {
                        result[y][x] = BitMapValue.Tile;
                    }
                }
            }
            return result;
        }
        private IList<IList<BitMapValue>> AddBombUsingDistance(IList<IList<BitMapValue>> result)
        {
            foreach(var tile in this.Bombs)
            {
                var yStart = (int)tile.YPos/Tile.s_height;
                var xStart = (int)tile.XPos/Tile.s_width;

                for(int y = 0; y < result.Count; y++)
                {
                    for(int x = 0; x < result[y].Count; x++)
                    {
                        var distance = Vector2.Distance(new Vector2(yStart,xStart), new Vector2(y,x));
                        if(distance <= s_bombRayLenght)
                        {
                            result[y][x] = BitMapValue.BombRay;
                        }
                    }
                    result[yStart][xStart] = BitMapValue.Bomb;

                }
            }
            return result;
        }
        private IList<IList<BitMapValue>> AddBombXandYOnly(IList<IList<BitMapValue>> result)
        {
            foreach(var tile in this.Bombs)
            {
                var xStart = (int)tile.XPos/Tile.s_width;
                var yStart = (int)tile.YPos/Tile.s_height;
                // result[x][y] = BitMapValue.Bomb;
                var bombRayLength = 4;
                for(int i = 1; i<bombRayLength;i++) // X +
                {
                    var nextX = Math.Min(xStart + i, result.Count -1);
                    if(result[nextX][yStart] == BitMapValue.Tile)
                    {
                        i = bombRayLength;
                    }
                    else
                    {
                        result[nextX][yStart] = BitMapValue.BombRay;
                    }
                }
                for(int i = 1; i<bombRayLength;i++)// X - 
                {
                    var nextX = Math.Max(xStart - i, 0);
                    if(result[nextX][yStart] == BitMapValue.Tile)
                    {
                        i = bombRayLength;
                    }
                    else
                    {
                        result[nextX][yStart] = BitMapValue.BombRay;
                    }
                }
                for(int i = 1; i<bombRayLength;i++) // Y +
                {
                    var newY = Math.Min(yStart + i, result.Count -1);
                    if(result[xStart][newY] == BitMapValue.Tile)
                    {
                        i = bombRayLength;
                    }
                    else
                    {
                        result[xStart][newY] = BitMapValue.BombRay;
                    }
                }
                for(int i = 1; i<bombRayLength;i++) // Y -
                {
                    var newY = Math.Max(yStart - i, 0);
                    if(result[xStart][newY] == BitMapValue.Tile)
                    {
                        i = bombRayLength;
                    }
                    else
                    {
                        result[xStart][newY] = BitMapValue.BombRay;
                    }
                }
                result[xStart][yStart] = BitMapValue.Bomb;

            }
            return result;
        }
    }
    public interface IMapLoader
    {
        Map Load(IGraphicsDeviceManagerNew graphics
            , GraphicsDevice graphicsDevice
            , ContentManager contentManager
            , ICollitionController collitionController
            , IBombRayFactory bombRayFactory
            , int mapNumber
            );
    }
    public class TileCreator
    {
        Texture2D tileTexture;
        public TileCreator(GraphicsDevice graphicsDevice)
        {
            this.tileTexture = new Texture2D(graphicsDevice, Tile.s_height, Tile.s_width);
            this.tileTexture
                .SetData(TextureCreator.CreateTileTextureData(Tile.s_height, Tile.s_width, Color.Gray));
        }
        public ITile Create(IGraphicsDeviceManagerNew graphics
            , GraphicsDevice graphicsDevice
            , (int x, int y) pos
            )
        {

            return new Tile(graphics, graphicsDevice, this.tileTexture)
            {
                XPos = Tile.s_width * (pos.x + 1),
                YPos = Tile.s_height * (pos.y + 1),
            };
        }
        public IList<ITile> CreateBoarder(IGraphicsDeviceManagerNew graphics
            , GraphicsDevice graphicsDevice
            , (int x, int y) mapSize
            )
        {
            var textureHorizontal = new Texture2D(graphicsDevice, mapSize.x * Tile.s_width, Tile.s_height);
            var topWall = new TileWall(graphics, textureHorizontal)
            {
                XPos = 0,
                YPos = 0,
            };
            var bottomWall = new TileWall(graphics, textureHorizontal)
            {
                XPos = 0, 
                YPos = Tile.s_height * (mapSize.y - 1),
            };

            var textureVertical = new Texture2D(graphicsDevice, Tile.s_width, mapSize.y * Tile.s_height);
            var leftWall =  new TileWall(graphics, textureVertical)
            {
                XPos = 0,
                YPos = 0,
            };
            var rightWall =  new TileWall(graphics, textureVertical)
            {
                XPos = Tile.s_width * (mapSize.x - 1),
                YPos = 0,
            };
            return new []
            {
                topWall,
                bottomWall,
                leftWall,
                rightWall,
            };
        }
    }
    public class MapLoader : IMapLoader
    {
        public Map Load(IGraphicsDeviceManagerNew graphics
        , GraphicsDevice graphicsDevice
        , ContentManager ContentManager
        , ICollitionController collitionController
        , IBombRayFactory bombRayFactory
        , int mapnumber
        )
        {
            var tileCreator = new TileCreator(graphicsDevice);

            var tiles = new List<IPositionalTexture2D>();
            var players = new List<IPlayer>();
            var playerOuterData = TextureCreator.AddCircleWithColor(Player.s_width, Player.s_width/2, null, Color.Black);
            (int x, int y) tileCount = (0,0);

            var numberOfMaps = Directory.GetFiles("Maps/").Count();
            var currentMap = mapnumber % numberOfMaps;
            var filePath = $"Maps/map{currentMap}.txt";
            if(File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).ToList();
                tileCount = (lines.Max(x => x.Count()) + 2,lines.Count + 2);
                for (int y = 0; y < lines.Count; y++)
                {
                    for(int x = 0; x < lines[y].Length; x++)
                    {
                        if((int)Char.GetNumericValue(lines[y][x]) == 1)
                        {
                            tiles.Add(tileCreator.Create(graphics, graphicsDevice, (x,y)));
                        }
                        if(lines[y][x] == 'a')
                        {
                            players.Add(new Player(TextureCreator.AddCircleWithColor(Player.s_width, (Player.s_width/2) -2, playerOuterData, Color.Blue)
                                , graphics
                                , new PlayerKeyboardInterpreter1()
                                , collitionController
                                , graphicsDevice
                                , bombRayFactory
                                )
                                {
                                    XPos = Tile.s_width * (x + 1),
                                    YPos = Tile.s_height * (y + 1),
                                    Name = "My name",
                                });
                        }
                        if(lines[y][x] == 'b')
                        {
                            players.Add(new Player(TextureCreator.AddCircleWithColor(Player.s_width, (Player.s_width/2) -2, playerOuterData, Color.Red)
                                , graphics
                                , new PlayerKeyboardInterpreter2(new AiBasedPlayer(collitionController, graphics))
                                // , 
                                , collitionController
                                , graphicsDevice
                                , bombRayFactory
                                )
                                {
                                    XPos = Tile.s_width * (x + 1),
                                    YPos = Tile.s_height * (y + 1),
                                    Name = "bot",
                                });
                        }
                    }
                }
                tiles.AddRange(tileCreator.CreateBoarder(graphics, graphicsDevice, tileCount));
            }
            return new Map
            {
                Tiles = tiles,
                Players = players,
                TileCount = (tileCount.y, tileCount.x),
                GraphicMax = (graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth),
            };
        }
    }
}
