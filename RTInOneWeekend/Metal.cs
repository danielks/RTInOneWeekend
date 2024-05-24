using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public class Metal : Material
    {
        public Vector3 albedo;
        public float fuzz;

        public Metal(Vector3 a, float f)
        {
            albedo = a;
            fuzz = f < 1f ? f : 1f;
        }

        public override bool scatter(Ray r_in, hit_record rec, ref Vector3 attenuation, ref Ray scattered)
        {
            Vector3 reflected = Vec3Utils.reflect(Vec3Utils.unit_vector(r_in.direction()), rec.normal);
            scattered = new Ray(rec.p, reflected + fuzz * Vec3Utils.random_in_unit_sphere());
            attenuation = albedo;
            return (Vector3.Dot(scattered.direction(), rec.normal) > 0);
        }
    }
}
