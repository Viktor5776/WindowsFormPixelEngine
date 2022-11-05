using PixelEngine;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PixelEngine
{   
    internal class Vec2<T>
    {
        public Vec2()
        {
        }

        public Vec2(T x_in, T y_in)
        {
            x = x_in;
            y = y_in;
        }

        public Vec2(Vec2<T> vect)
        {
            x = vect.x;
            y = vect.y;
        }

        public T LenSq() 
	    {
            dynamic x_in = x;
            dynamic y_in = y;

            return (T)(x_in*x_in+y_in*y_in);
        }

        public T Len()
        {
            dynamic lenSq_in = LenSq();
            return (T)Math.Sqrt(lenSq_in);
        }

        public Vec2<T> Normalize()
        {
            dynamic length = Len();
            x /= length;
            y /= length;
            return this;
        }

        public Vec2<T> GetNormalized()
        {
            Vec2<T> norm = new Vec2<T>(this);
            norm.Normalize();
            return norm;
        }

        public static Vec2<T> operator-(Vec2<T> rhs)
        {
            dynamic x_in = rhs.x;
            dynamic y_in = rhs.y;
            return new Vec2<T>(-x_in, -y_in);
        }

        public static Vec2<T> operator +(Vec2<T> lhs, Vec2<T> rhs)
        {
            dynamic d_lhs = lhs;
            dynamic d_rhs = rhs;
            return new Vec2<T>(d_lhs.x + d_rhs.x, d_lhs.y + d_rhs.y);
        }

        public static Vec2<T> operator -(Vec2<T> lhs, Vec2<T> rhs)
        {
            dynamic d_lhs = lhs;
            dynamic d_rhs = rhs;
            return new Vec2<T>(d_lhs.x - d_rhs.x, d_lhs.y - d_rhs.y);
        }
 
        public static Vec2<T> operator*(Vec2<T> lhs, T rhs)
        {
            dynamic d_lhs = lhs;

            return new Vec2<T>(d_lhs.x * rhs, d_lhs.y * rhs);
        }

        public static Vec2<T> operator/(Vec2<T> lhs, T rhs)
        {
            dynamic d_lhs = lhs;

            return new Vec2<T>(d_lhs.x / rhs, d_lhs.y / rhs);
        }

        public static bool operator==(Vec2<T> lhs, Vec2<T> rhs)
        {
            dynamic d_lhs = lhs;
            dynamic d_rhs = rhs;

            return d_lhs.x == d_rhs.x && d_lhs.y == d_rhs.y;
        }

        public static bool operator !=(Vec2<T> lhs, Vec2<T> rhs)
        {
            return !(lhs == rhs);
        }

    public T x;
        public T y;
        
    }
}
