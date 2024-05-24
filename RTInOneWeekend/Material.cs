using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public abstract class Material
    {
        public abstract bool scatter(Ray r_in, hit_record rec, ref Vector3 attenuation, ref Ray scattered);
    }
}
