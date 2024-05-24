using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public class Sphere : Hittable
    {
        public Vector3 center;
        public float radius;
        public Material mat_ptr;

        public Sphere(Vector3 cen, float r, Material m)
        {
            center = cen;
            radius = r;
            mat_ptr = m;
        }

        public override bool hit(Ray r, float t_min, float t_max, ref hit_record rec)
        {
            Vector3 oc = r.origin() - center;
            float a = r.direction().LengthSquared();
            float half_b = Vector3.Dot(oc, r.direction());
            float c = oc.LengthSquared() - radius * radius;

            double discriminant = half_b * half_b - a * c;
            if (discriminant < 0) return false;
            float sqrtd = (float)Math.Sqrt(discriminant);

            // Find the nearest root that lies in the acceptable range.
            float root = (-half_b - sqrtd) / a;

            if (root < t_min || t_max < root)
            {
                root = (-half_b + sqrtd) / a;

                if (root < t_min || t_max < root)
                    return false;
            }

            rec.t = root;
            rec.p = r.at(rec.t);
            Vector3 outward_normal = (rec.p - center) / radius;
            rec.set_face_normal(r, outward_normal);
            rec.mat_ptr = mat_ptr;

            return true;
        }
    }
}
