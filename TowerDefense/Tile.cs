﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Tile : Sprite
    {
        public Point GridPos;
        public TileTypes Type;

        public Tile(Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture, Point gridPos, TileTypes type)
            : base(position, color, scale, rotation, sourceRectangle, origin, texture)
        {
            GridPos = gridPos;
            Type = type;
        }
    }
}
