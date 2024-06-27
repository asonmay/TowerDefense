using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Enemy : Sprite
    {
        public int health { get; set; }
        public Point gridPos { get; set; }
        public int speed;
        private TimeSpan enemyTimer;

        public Enemy(int speed, int health, Point gridPos, Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Vector2 origin, Texture2D texture)
            : base(position, color, scale, rotation, sourceRectangle, origin, texture)
        {
            this.speed = speed;
            this.health = health;
            this.gridPos = gridPos;
            enemyTimer = TimeSpan.Zero;
        }

        private void ResetPathTiles(Tile[,] tiles)
        {
            for(int x = 0; x < tiles.GetLength(0); x++)
            {
                for(int y = 0; y < tiles.GetLength(1); y++)
                {
                    if (tiles[x,y] is PathTile)
                    {
                        ((PathTile)tiles[x, y]).cumulativeDistance = float.MaxValue;
                        ((PathTile)tiles[x, y]).hasBeenVisited = false;
                        ((PathTile)tiles[x, y]).Founder = null;
                    }
                }
            }
        }

        private PathTile[] GetShortestRougt(Tile[,] tiles, Point startingPoint, Point endingPoint)
        {
            ResetPathTiles(tiles);

            PriorityQueue<PathTile, float> priorityQueue = new PriorityQueue<PathTile, float>();
            ((PathTile)tiles[startingPoint.X, startingPoint.Y]).cumulativeDistance = 0;
            priorityQueue.Enqueue((PathTile)tiles[startingPoint.X, startingPoint.Y], ((PathTile)tiles[startingPoint.X, startingPoint.Y]).cumulativeDistance);

            PathTile currentVertex;
            while (!((PathTile)tiles[endingPoint.X, endingPoint.Y]).hasBeenVisited)
            {
                currentVertex = priorityQueue.Dequeue();
                for(int i = 0; i < currentVertex.Neighbors.Length; i++)
                {
                    float tenetiveDistance = currentVertex.cumulativeDistance + 1;
                    if (tenetiveDistance < currentVertex.Neighbors[i].cumulativeDistance)
                    {
                        currentVertex.Neighbors[i].cumulativeDistance = tenetiveDistance;
                        currentVertex.Neighbors[i].Founder = currentVertex;
                    }
                    priorityQueue.Enqueue(currentVertex.Neighbors[i], currentVertex.Neighbors[i].cumulativeDistance);
                }
                currentVertex.hasBeenVisited = true;
            }

            List<PathTile> shortestRought = new List<PathTile>();

            currentVertex = ((PathTile)tiles[endingPoint.X, endingPoint.Y]);
            while (currentVertex != ((PathTile)tiles[startingPoint.X, startingPoint.Y]))
            {
                shortestRought.Add(currentVertex);
                currentVertex = currentVertex.Founder;
            }

            shortestRought.Reverse();

            return shortestRought.ToArray();
        }

        public void Move(Tile[,] tiles, Point startingPoint, Point endingPoint)
        {
            gridPos = GetShortestRougt(tiles, startingPoint, endingPoint)[0].GridPos;
        }

        public void Update(GameTime gameTime, Tile[,] tiles, Point startingPoint, Point endingPoint)
        {
            enemyTimer += gameTime.ElapsedGameTime;
            if(enemyTimer.TotalMilliseconds >= speed)
            {
                Move(tiles, startingPoint, endingPoint);
            }
        }

        public void Draw()
        {

        }
    }
}
