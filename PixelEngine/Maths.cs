using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    internal class Maths<T>
    {
        public static T wrap_angle(T theta_in)
        {
            dynamic theta = theta_in;
            dynamic modded = (T)(theta % (2.0f * Math.PI));
            return (modded > Math.PI) ?
                (modded - 2.0f * (float)Math.PI) :
                modded;
        }

        public static T interpolate(T src, T dst, float alpha)
        {
            dynamic src_d = src;
            return src + (dst - src_d) * alpha;
        }
    }
}
