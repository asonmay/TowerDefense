using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public class Timer
    {
        private TimeSpan timer;
        private TimeSpan rate;

        public Timer(TimeSpan rate)
        {
            timer = TimeSpan.Zero;
            this.rate = rate;
        }

        public bool Update(GameTime gametime)
        {
            timer += gametime.ElapsedGameTime;
            if(timer >= rate)
            {
                timer = TimeSpan.Zero;
                return true;
            }
            return false;
        }
    }
}
