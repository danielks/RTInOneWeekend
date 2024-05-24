using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public class Camera
    {
        private Vector3 origin;
        private Vector3 lower_left_corner;
        private Vector3 horizontal;
        private Vector3 vertical;
        private Vector3 u, v, w;
        private float lens_radius;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vfov">vertical field-of-view in degrees</param>
        /// <param name="aspect_ratio"></param>
        public Camera(Vector3 lookfrom, Vector3 lookat, Vector3 vup, float vfov, float aspect_ratio, float aperture, float focus_dist)
        {
            float theta = Util.degrees_to_radians(vfov);
            float h = (float)Math.Tan(theta / 2);
            float viewport_height = 2.0f * h;
            float viewport_width = aspect_ratio * viewport_height;

            w = Vec3Utils.unit_vector(lookfrom - lookat);
            u = Vec3Utils.unit_vector(Vector3.Cross(vup, w));
            v = Vector3.Cross(w, u);

            origin = lookfrom;
            horizontal = focus_dist * viewport_width * u;
            vertical = focus_dist * viewport_height * v;
            lower_left_corner = origin - horizontal / 2 - vertical / 2 - focus_dist * w;

            lens_radius = aperture / 2;
        }

        public Ray get_ray(float s, float t)
        {
            Vector3 rd = lens_radius * Vec3Utils.random_in_unit_disk();
            Vector3 offset = u * rd.X + v * rd.Y;

            return new Ray(
                origin + offset,
                lower_left_corner + s * horizontal + t * vertical - origin - offset
            );
        }
    }
}
