using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    internal class TexVertex
    {
        public TexVertex( Vec3<float> pos_in, Vec2<float> tc_in)
        {
            pos = pos_in;
            tc = tc_in;
        }

        public TexVertex InterpolateTo(TexVertex dest, float alpha)
        {
            return new TexVertex(
                pos.InterpolateTo(dest.pos,alpha), 
                tc.InterpolateTo(dest.tc,alpha)
            );
        }

        public static TexVertex operator+(TexVertex lhs, TexVertex rhs)
        {
            return new TexVertex(lhs.pos + rhs.pos, lhs.tc + rhs.tc);
        }

        public static TexVertex operator -(TexVertex lhs, TexVertex rhs)
        {
            return new TexVertex(lhs.pos - rhs.pos, lhs.tc - rhs.tc);
        }

        public static TexVertex operator *(TexVertex lhs, float rhs)
        {
            return new TexVertex(lhs.pos * rhs, lhs.tc * rhs);
        }

        public static TexVertex operator /(TexVertex lhs, float rhs)
        {
            return new TexVertex(lhs.pos / rhs, lhs.tc / rhs);
        }

        public Vec3<float> pos;
        public Vec2<float> tc;
    }
}
