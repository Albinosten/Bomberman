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
    public class Map
    {
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
        public void PrintBitmap(IList<IList<int>> map)
        {
            Console.WriteLine("*****************");

            for(int y = 0; y < map[0].Count; y++)
            {
                Console.WriteLine();
                for(int x = 0; x < map.Count; x++)
                {
                    Console.Write(map[x][y]);
                }
            }
        }
        public IList<IList<int>> GetBitMap(bool withPlayers)
        {
            var result = new List<IList<int>>();
            for(int x = 0; x < this.TileCount.Width; x++)
            {
                result.Add(new List<int>());   
                for(int y = 0; y < this.TileCount.Hight; y++)
                {
                    var value = (y == 0 
                        || y == this.TileCount.Hight - 1
                        || x == 0
                        || x == this.TileCount.Width -1
                        ) 
                        ? 1 
                        : 0;
                    result[x].Add(value);
                }
            }
            foreach(var tile in this.Tiles)
            {
                var x = (int)tile.XPos/Tile.s_width;
                var y = (int)tile.YPos/Tile.s_height;
                result[x][y] = 1;
            }
            if(withPlayers)
            {
                foreach(var player in this.Players)
                {
                    var x = (int)player.XPos/Tile.s_width;
                    var y = (int)player.YPos/Tile.s_height;
                    result[x][y] = 2;
                }
            }
            // result[(int)p.XPos/Tile.s_width][(int)p.YPos/Tile.s_height] = 9;
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
                            players.Add(new Player(ContentManager.Load<Texture2D>("ball")
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
                            players.Add( new Player(ContentManager.Load<Texture2D>("ball")
                                , graphics
                                , new AiBasedPlayer(collitionController, graphics)
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
