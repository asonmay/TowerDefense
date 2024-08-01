using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Wave
    {
        public Enemy[] enemies { get; set; }
        public int moneyEarned { get; private set; }
        public TimeSpan enemySpawnRate { get; private set; }
        private int enemyIndex;

        public Wave(Enemy[] enemies, int moneyEarned, TimeSpan enemySpawnRate)
        {
            this.enemies = enemies;
            this.moneyEarned = moneyEarned;
            this.enemySpawnRate = enemySpawnRate;
            enemyIndex = 0;
        }

        public Enemy GetNextEnemy()
        {
            Enemy enemy = enemies[enemyIndex];
            enemyIndex++;
            return enemy;
        }

        public bool IsWaveDone()
        {
            return enemyIndex >= enemies.Length;
        }
    }
}
