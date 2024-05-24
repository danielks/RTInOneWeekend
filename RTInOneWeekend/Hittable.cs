using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public struct hit_record
    {
        public Vector3 p;
        public Vector3 normal;
        public Material mat_ptr;
        public float t;
        public bool front_face;

        public void set_face_normal(Ray r, Vector3 outward_normal)
        {
            this.front_face = Vector3.Dot(r.direction(), outward_normal) < 0;
            this.normal = front_face ? outward_normal : -outward_normal;
        }
    }

    public abstract class Hittable
    {
        public abstract bool hit(Ray r, float t_min, float t_max, ref hit_record rec);
    }
}
