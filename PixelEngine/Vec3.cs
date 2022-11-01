using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    internal class Vec3<T> : Vec2<T>
    {
        public Vec3()
        { }

        public Vec3(T x_in, T y_in, T z_in)
        {
            x = x_in;
            y = y_in;
            z = z_in;
        }

        public Vec3(Vec3<T> vect)
        {
            x = vect.x;
            y = vect.y;
            z = vect.z;
        }

        new public T LenSq()
        {
            dynamic x_in = x;
            dynamic y_in = y;
            dynamic z_in = z;

            return (T)(x_in * x_in + y_in * y_in + z_in * z_in);
        }

        new public T Len()
        {
            dynamic lenSq_in = LenSq();
            return (T)Math.Sqrt(lenSq_in);
        }

        new public Vec3<T> Normalize()
        {
            dynamic length = Len();
            x /= length;
            y /= length;
            z /= length;
            return this;
        }

        new public Vec3<T> GetNormalized()
        {
            Vec3<T> norm = new Vec3<T>(this);
            norm.Normalize();
            return norm;
        }

        public static Vec3<T> operator -(Vec3<T> rhs)
        {
            dynamic x_in = rhs.x;
            dynamic y_in = rhs.y;
            dynamic z_in = rhs.z;
            return new Vec3<T>(-x_in, -y_in, -z_in);
        }

        public static Vec3<T> operator +(Vec3<T> lhs, Vec3<T> rhs)
        {
            dynamic d_lhs = lhs;
            dynamic d_rhs = rhs;
            return new Vec3<T>(d_lhs.x + d_rhs.x, d_lhs.y + d_rhs.y, d_lhs.z + d_rhs.z);
        }

        public static Vec3<T> operator -(Vec3<T> lhs, Vec3<T> rhs)
        {
            dynamic d_lhs = lhs;
            dynamic d_rhs = rhs;
            return new Vec3<T>(d_lhs.x - d_rhs.x, d_lhs.y - d_lhs.y, d_lhs.z - d_lhs.z);
        }

        public static Vec3<T> operator *(Vec3<T> lhs, T rhs)
        {
            dynamic d_lhs = lhs;

            return new Vec3<T>(d_lhs.x * rhs, d_lhs.y * rhs, d_lhs * rhs);
        }

        public static Vec3<T> operator /(Vec3<T> lhs, T rhs)
        {
            dynamic d_lhs = lhs;

            return new Vec3<T>(d_lhs.x / rhs, d_lhs.y / rhs, d_lhs.z / rhs);
        }

        public static bool operator ==(Vec3<T> lhs, Vec3<T> rhs)
        {
            dynamic d_lhs = lhs;
            dynamic d_rhs = rhs;

            return d_lhs.x == d_rhs.x && d_lhs.y == d_rhs.y && d_lhs.z == d_rhs.z;
        }

        public static bool operator !=(Vec3<T> lhs, Vec3<T> rhs)
        {
            return !(lhs == rhs);
        }

        public T z;
    }
}
