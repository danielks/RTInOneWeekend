using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public static class Util
    {
        public static float infinity = float.PositiveInfinity;
        public static float pi = 3.1415926535897932385f;
        private static Random random = new Random();



        public static float degrees_to_radians(float degrees)
        {
            return degrees * pi / 180.0f;
        }

        public static float random_float()
        {            
            return random.NextSingle();
        }

        public static float random_float(float min, float max)
        {
            //returns a random real in [min, max).
            return min + (max - min) * random_float();
        }

        public static float clamp(float x, float min, float max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }
    }
}
