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
            v.x = (v.x + 1.0f) * xFactor;
            v.y = (-v.y + 1.0f) * yFactor;
            return v;
        }

        public Vec3<float> GetTransformed( Vec3<float> v )
	    {
            return Transform( new Vec3<float>(v) );
        }
	    
        float xFactor;
        float yFactor;
    }

    class IndexedLineList
    {
        public IndexedLineList()
        {
            vertices = new List<Vec3<float>>();
            indices = new List<int>();
        }

        public IndexedLineList(IndexedLineList l)
        {
            vertices = l.vertices;
            indices = l.indices;
        }

        public IndexedLineList DeepCopy()
        {
            IndexedLineList other = (IndexedLineList)this.MemberwiseClone();
            other.vertices = new List<Vec3<float>>(vertices);
            other.indices = new List<int>(indices);
            return other;
        }

        public List<Vec3<float>> vertices;
        public List<int> indices;
    };
}
