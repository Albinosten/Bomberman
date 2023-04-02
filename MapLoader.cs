using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace Bomberman
{
    public class Map
    {
        public int TileWidthCount {get;set;}
        public int TileHeightCount {get;set;}
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
    public class MapLoader : IMapLoader
    {
        public Map Load(IGraphicsDeviceManagerNew graphics
        , GraphicsDevice graphicsDevice
        , ContentManager ContentManager
        )
        {
            var tiles = new List<ITile>();
            var players = new List<IPlayer>();
            var tileHeightCount = 0;
            var tileWidthCount = 0;
            var filePath = "Maps/map1.txt";
            if(File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath).ToList();
                tileHeightCount = lines.Count;
                tileWidthCount = lines.Max(x => x.Count());
                for (int y = 0; y < lines.Count; y++)
                {
                    for(int x = 0; x < lines[y].Length; x++)
                    {
                        if((int)Char.GetNumericValue(lines[y][x]) == 1)
                        {
                            var tile = new Tile(graphics, graphicsDevice)
                            {
                                XPos = Tile.s_width * x,
                                YPos = Tile.s_height * y
                            };
                            tiles.Add(tile);
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
                                    XPos = Tile.s_width * x,
                                    YPos = Tile.s_height * y
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
                                    XPos = Tile.s_width * x,
                                    YPos = Tile.s_height * y
                                });
                        }
                    }
                }
            }
            return new Map
            {
                Tiles = tiles,
                Players = players,
                TileHeightCount = tileHeightCount,
                TileWidthCount = tileWidthCount,
            };
        }
    }
}
