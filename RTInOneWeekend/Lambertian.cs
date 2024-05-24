using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public class Lambertian : Material
    {
        public Vector3 albedo;

        public Lambertian(Vector3 a)
        {
            albedo = a;
        }

        public override bool scatter(Ray r_in, hit_record rec, ref Vector3 attenuation, ref Ray scattered)
        {
            Vector3 scatter_direction = rec.normal + Vec3Utils.random_unit_vector();

            // Catch degenerate scatter direction
            if (Vec3Utils.near_zero(scatter_direction))
                scatter_direction = rec.normal;

            scattered = new Ray(rec.p, scatter_direction);
            attenuation = albedo;
            return true;
        }
    }
}
