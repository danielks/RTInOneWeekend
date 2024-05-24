using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public class Dieletric : Material
    {
        public float ir; //Index of Refraction

        public Dieletric(float index_of_refraction)
        {
            ir = index_of_refraction;
        }

        public override bool scatter(Ray r_in, hit_record rec, ref Vector3 attenuation, ref Ray scattered)
        {
            attenuation = new Vector3(1.0f, 1.0f, 1.0f);
            float refraction_ratio = rec.front_face ? (1.0f / ir) : ir;

            Vector3 unit_direction = Vec3Utils.unit_vector(r_in.direction());
            float cos_theta = (float)Math.Min(Vector3.Dot(-unit_direction, rec.normal), 1.0);
            float sin_theta = (float)Math.Sqrt(1.0f - cos_theta * cos_theta);

            bool cannot_refract = refraction_ratio * sin_theta > 1.0;
            Vector3 direction;

            if (cannot_refract || reflectance(cos_theta, refraction_ratio) > Util.random_float())
                direction = Vec3Utils.reflect(unit_direction, rec.normal);
            else
                direction = Vec3Utils.refract(unit_direction, rec.normal, refraction_ratio);

            scattered = new Ray(rec.p, direction);
            return true;
        }

        private double reflectance(double cosine, double ref_idx)
        {
            // Use Schlick's approximation for reflectance.
            double r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
        }
    }
}
