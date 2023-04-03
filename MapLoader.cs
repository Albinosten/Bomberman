using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Bomberman
{
    public class Map
    {
        public (int Hight, int Width) TileCount {get;set;}
        public Map()
        {
            this.Tiles = new List<ITile>();
            this.Players = new List<IPlayer>();
            this.Bombs = new List<IBomb>();
        }
        public IList<ITile> Tiles{get;set;}
        public IList<IPlayer> Players {get;set;}
        public IList<IBomb> Bombs {get;set;}
    }
    public interface IMapLoader
    {
        Map Load(IGraphicsDeviceManagerNew graphics
            , GraphicsDevice graphicsDevice
            , ContentManager contentManager
            );
    }
    public class TileCreator
    {
        Texture2D tileTexture;
        public TileCreator(GraphicsDevice graphicsDevice)
        {
            this.tileTexture = new Texture2D(graphicsDevice, Tile.s_height, Tile.s_width);
            this.tileTexture
                .SetData(TextureCreator.CreateTileTexture(Tile.s_height, Tile.s_width, Color.Gray));
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
        )
        {
            var tileCreator = new TileCreator(graphicsDevice);

            var tiles = new List<ITile>();
            var players = new List<IPlayer>();
            (int x, int y) tileCount = (0,0);
            var filePath = "Maps/map1.txt";
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
                                , new CollitionController()
                                , graphicsDevice
                                )
                                {
                                    XPos = Tile.s_width * (x + 1),
                                    YPos = Tile.s_height * (y + 1),
                                });
                        }
                        if(lines[y][x] == 'b')
                        {
                            players.Add( new Player(ContentManager.Load<Texture2D>("ball")
                                , graphics
                                , new PlayerKeyboardInterpreter2()
                                , new CollitionController()
                                , graphicsDevice
                                )
                                {
                                    XPos = Tile.s_width * (x + 1),
                                    YPos = Tile.s_height * (y + 1),
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
            };
        }
    }
}
