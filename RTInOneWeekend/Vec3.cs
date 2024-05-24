using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RTInOneWeekend
{
    public static class Vec3Utils
    {



        public static Vector3 random()
        {
            return new Vector3(Util.random_float(), Util.random_float(), Util.random_float());
        }

        public static Vector3 random(float min, float max)
        {
            return new Vector3(Util.random_float(min, max), Util.random_float(min, max), Util.random_float(min, max));
        }

        public static Vector3 random_in_unit_sphere()
        {
            while (true)
            {
                Vector3 p = Vec3Utils.random(-1, 1);
                if (p.LengthSquared() >= 1f) continue;
                return p;
            }
        }

        public static Vector3 random_unit_vector()
        {




            return Vec3Utils.unit_vector(Vec3Utils.random_in_unit_sphere());
        }

        public static Vector3 random_in_hemisphere(Vector3 normal)
        {
            Vector3 in_unit_sphere = Vec3Utils.random_in_unit_sphere();

            if (Vector3.Dot(in_unit_sphere, normal) > 0.0) // In the same hemisphere as the normal
                return in_unit_sphere;
            else
                return -in_unit_sphere;
        }

        public static Vector3 random_in_unit_disk()
        {
            while (true)
            {
                Vector3 p = new Vector3(Util.random_float(-1, 1), Util.random_float(-1, 1), 0);
                if (p.LengthSquared() >= 1) continue;
                return p;
            }
        }


        public static bool near_zero(Vector3 v)
        {
            float s = 1e-8f;
            return (Math.Abs(v.X) < s) && (Math.Abs(v.Y) < s) && (Math.Abs(v.Z) < s);
        }

        public static string ToString(Vector3 v)
        {
            return string.Format("{0} {1} {2}", v.X, v.Y, v.Z);
        }


        public static Vector3 unit_vector(Vector3 v)
        {
            return v / v.Length();
        }



        public static Vector3 reflect(Vector3 v, Vector3 n)
        {
            return v - 2 * Vector3.Dot(v, n) * n;
        }

        public static Vector3 refract(Vector3 uv, Vector3 n, float etai_over_etat)
        {
            float cos_theta = Math.Min(Vector3.Dot(-uv, n), 1.0f);

            Vector3 r_out_perp = etai_over_etat * (uv + cos_theta * n);


            Vector3 r_out_parallel = -(MathF.Sqrt(Math.Abs(1.0f - r_out_perp.LengthSquared()))) * n;
            return r_out_perp + r_out_parallel;
        }





        //a partir daqui o que no artigo fala "vec3 utility functions"
        /*
         * // vec3 Utility Functions        

        inline vec3 operator*(const vec3 &u, const vec3 &v) {
        return vec3(u.e[0] * v.e[0], u.e[1] * v.e[1], u.e[2] * v.e[2]);
        }

        inline vec3 operator*(const vec3 &v, double t) {
        return t * v;
        }

        inline vec3 operator/(vec3 v, double t) {
        return (1/t) * v;
        }

        inline double dot(const vec3 &u, const vec3 &v) {
        return u.e[0] * v.e[0]
         + u.e[1] * v.e[1]
         + u.e[2] * v.e[2];
        }

        inline vec3 cross(const vec3 &u, const vec3 &v) {
        return vec3(u.e[1] * v.e[2] - u.e[2] * v.e[1],
                u.e[2] * v.e[0] - u.e[0] * v.e[2],
                u.e[0] * v.e[1] - u.e[1] * v.e[0]);
        }

        */


    }
}
