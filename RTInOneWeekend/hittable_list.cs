using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    class hittable_list : Hittable
    {
        public List<Hittable> objects;

        public hittable_list()
        {
            objects = new List<Hittable>();
        }

        public void clear()
        {
            objects.Clear();
        }

        public void add(Hittable hobject)
        {
            objects.Add(hobject);
        }

        public override bool hit(Ray r, float t_min, float t_max, ref hit_record rec)
        {
            hit_record temp_rec = new hit_record();
            bool hit_anything = false;
            float closest_so_far = t_max;

            foreach (var hobject in objects)
            {
                if (hobject.hit(r, t_min, closest_so_far, ref temp_rec))
                {
                    hit_anything = true;
                    closest_so_far = temp_rec.t;
                    rec = temp_rec;
                }
            }

            return hit_anything;
        }
    }
}
