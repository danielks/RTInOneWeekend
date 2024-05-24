using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public class Ray
    {
        public Ray(Vector3 origin, Vector3 direction)
        {
            orig = origin;
            dir = direction;
        }

        public Vector3 origin()
        {
            return orig;
        }

        public Vector3 direction()
        {
            return dir;
        }

        public Vector3 at(float t)
        {
            return orig + (t * dir);
        }

        public Vector3 orig;
        public Vector3 dir;
    }
}
