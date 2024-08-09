using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TowerDefense
{
    public enum EnemyType
    {
        NormalZanlin,
        MadZanlin,
        HappyZanlin,
    }

    public class Enemy : Sprite
    {
        public int Health { get; set; }
        public Point GridPos { get; set; }
        public int Speed;
        private TimeSpan enemyTimer;
        public bool HasReachedEnd;
        public Tilemap Map;
        private PathTile[] rought;
        private int currentPathIndex;
        public int Reward;
        public string Name;

        public Enemy(int speed, int health, Vector2 position, Color color, float scale, int rotation, Rectangle sourceRectangle, Texture2D texture, int reward, string name)
            : base(position, color, scale, rotation, sourceRectangle, new Vector2(texture.Width * scale / 2, texture.Height * scale / 2), texture)
        {
            Speed = speed;
            Health = health;
            GridPos = Point.Zero;
            enemyTimer = TimeSpan.Zero;
            Reward = reward;
            Name = name;
        }

        public Enemy(int speed, int health, float scale, Rectangle sourceRectangle, Texture2D texture, int reward, string name)
            :this(speed, health, new Vector2(0,0), Color.White, scale, 0, sourceRectangle, texture, reward, name)
        {

        }

        static NodeWrapper DijstrasSelection(List<NodeWrapper> nodes)
        {
            NodeWrapper node = nodes[0];

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].cumulativeDistance < node.cumulativeDistance)
                {
                    node = nodes[i];
                }
            }
            nodes.Remove(node);

            return node;
        }

        private bool Contains(List<NodeWrapper> nodes, NodeWrapper node)
        {
            for(int i = 0; i < nodes.Count; i++)
            {
                if(node.WrappedNode.GridPos == nodes[i].WrappedNode.GridPos)
                {
                    return true;
                }
            }
            return false;
        }

        public void GenerateRougt()
        {
            List<NodeWrapper> visitedNodes = new List<NodeWrapper>();
            NodeWrapper currentNode = new NodeWrapper((PathTile)Map.Tiles[Map.StartingPoint.X, Map.StartingPoint.Y], 0, null);
            List<NodeWrapper> frontier = new List<NodeWrapper>();
            while (currentNode.WrappedNode.GridPos != ((PathTile)Map.Tiles[Map.EndingPoint.X, Map.EndingPoint.Y]).GridPos)
            {
                currentNode.WrappedNode.Neighbors.Shuffle(new Random());
                for (int i = 0; i < currentNode.WrappedNode.Neighbors.Length; i++)
                {
                    float distanceFromStart = currentNode.cumulativeDistance + 1;
                    NodeWrapper neighbor = new NodeWrapper(currentNode.WrappedNode.Neighbors[i], distanceFromStart, currentNode);
                    if (!Contains(visitedNodes,neighbor) && !Contains(frontier, neighbor))
                    {
                        frontier.Add(neighbor);
                    }
                }

                visitedNodes.Add(currentNode);
                currentNode = DijstrasSelection(frontier);
            }

            List<PathTile> path = new List<PathTile>();
            while (currentNode.WrappedNode.GridPos != ((PathTile)Map.Tiles[Map.StartingPoint.X, Map.StartingPoint.Y]).GridPos)
            {
                path.Add(currentNode.WrappedNode);
                currentNode = currentNode.Founder;
            }
            path.Add(currentNode.WrappedNode);
            path.Reverse();

            rought = path.ToArray();
            currentPathIndex = 0;
        }

        public void Move()
        {
            GridPos = rought[currentPathIndex].GridPos;
        }

        public void Update(GameTime gameTime)
        {
            enemyTimer += gameTime.ElapsedGameTime;
            if(enemyTimer.TotalMilliseconds >= Speed)
            {
                Move();
                enemyTimer = TimeSpan.Zero;
                currentPathIndex++;
            }
            Position = new Vector2(Map.mapPosition.X + GridPos.X * Map.Specs.TileSize.X, Map.mapPosition.Y + GridPos.Y * Map.Specs.TileSize.Y);

            if (GridPos == Map.EndingPoint)
            {
                HasReachedEnd = true;
            }
        }

        public Enemy Clone()
        {
            return new Enemy(Speed, Health, Scale, SourceRectangle, Texture, Reward, Name);
        }
    }
}
