using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    internal class PubeScreenTransformer
    {
        public PubeScreenTransformer()
        {
            xFactor = ((float)(640) / 2.0f);
            yFactor = ((float)(640) / 2.0f);
        }
        public Vec3<float> Transform(Vec3<float> v)
        {
            float zInv = 1.0f / v.z;
            v.x = (v.x * zInv + 1.0f) * xFactor;
            v.y = (-v.y * zInv + 1.0f) * yFactor;
            return v;
        }

        public Vec3<float> GetTransformed( Vec3<float> v )
	    {
            return Transform( new Vec3<float>(v) );
        }
	    
        float xFactor;
        float yFactor;
    }
}
