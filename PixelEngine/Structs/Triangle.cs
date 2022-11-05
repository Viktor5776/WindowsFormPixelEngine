using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    internal class Triangle<T>
    {
        public Triangle(T v0, T v1, T v2)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
        }

        public T v0;
        public T v1;
        public T v2;
    }
}
