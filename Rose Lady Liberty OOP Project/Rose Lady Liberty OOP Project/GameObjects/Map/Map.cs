﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RoseLadyLibertyOOPProject.GameObjects.Map
{
    public class Map : Interfaces.IDrawable
    {
        private int tileWidth;
        private int tileHeight;
        private int mapRowCells;
        private int mapColumnCells;
        private Texture2D grassTexture;
        private Texture2D pathTexture;
        private Texture2D dirtTexture;
        private Tile[,] map;
        private List<PathGenerator.Direction> mobDirections;

        public Map(TheGame game, int tileWidth, int tileHeight, int rowCells = 16, int columnCells = 16)
        {
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            this.MapRowCells = rowCells;
            this.MapColumnCells = columnCells;
            grassTexture = game.Content.Load<Texture2D>("Terrain/grass");
            pathTexture = game.Content.Load<Texture2D>("Terrain/path");
            dirtTexture = game.Content.Load<Texture2D>("Terrain/dirt");
            map = new Tile[this.MapRowCells, this.MapColumnCells];
            this.CreateMap(grassTexture);
        }

        public Tile[,] MapCells { get { return this.map; } }
        
        public int TileWidth
        {
            get { return this.tileWidth; }
            set
            {
                if (value > 64 || value < 1)
                {
                    throw new ArgumentOutOfRangeException("Tile width must be less then 64");
                }
                this.tileWidth = value;
            }
        }
        public int TileHeight
        {
            get { return this.tileHeight; }
            set
            {
                if (value > 64 || value < 1)
                {
                    throw new ArgumentOutOfRangeException("Tile height must be less then 64");
                }
                this.tileHeight = value;
            }
        }
        public int MapRowCells
        {
            get { return this.mapRowCells; }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Map width can not be negative or zero!");
                }
                this.mapRowCells = value;
            }
        }
        public int MapColumnCells
        {
            get { return this.mapColumnCells; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("Map height can not be negative or zero!");
                }
                this.mapColumnCells = value;
            }
        }

        public int MapWidth 
        {
            get { return this.MapRowCells * this.TileWidth; } 
        }

        public int MapHeight
        {
            get { return this.MapColumnCells * this.TileHeight; }
        }

        public void CreateMap(Texture2D texture)
        {
            for (int row = 0; row < this.MapRowCells; row++)
            {
                for (int column = 0; column < this.MapColumnCells; column++)
                {
                    this.map[row, column] = new Tile("grass_tile", row * this.TileWidth, column * this.TileHeight, TileWidth, TileHeight, texture);
                }
            }
            this.GenerateBoards();
            this.GeneratePath();
        }

        public Tile[] PathTiles
        { get; private set; }

        public void GeneratePath()
        {

            List<Tile> pathTiles = new List<Tile>();

            var path = PathGenerator.GeneratePath(this.MapRowCells, this.MapColumnCells);

            for (int i = 0; i < path.Count; i++)
            {
                this.map[path[i].Item1, path[i].Item2].TileTexture = pathTexture;
                this.map[path[i].Item1, path[i].Item2].TileType = Enumerations.TileType.Path;
                pathTiles.Add(this.map[path[i].Item1, path[i].Item2]);
            }
            this.PathTiles = pathTiles.ToArray();
        }

        public void GenerateBoards()
        {
            for (int row = 0; row < this.MapRowCells; row++)
            {
                for (int column = 0; column < this.MapColumnCells; column++)
                {
                    if (row == 0 || row == this.MapRowCells - 1 || column == 0 || column == this.MapColumnCells - 1)
                    {
                        this.map[row, column].TileTexture = dirtTexture;
                    }
                }
            }
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int row = 0; row < this.MapRowCells; row++)
            {
                for (int column = 0; column < this.MapColumnCells; column++)
                {
                    this.map[row, column].Draw(spriteBatch);
                }
            }
            spriteBatch.End();
        }
    }
}
