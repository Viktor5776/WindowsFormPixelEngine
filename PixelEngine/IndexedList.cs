using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
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

    class IndexedTriangleList
    {
        public IndexedTriangleList()
        {
            vertices = new List<Vec3<float>>();
            indices = new List<int>();
        }

        public IndexedTriangleList(IndexedTriangleList l)
        {
            vertices = l.vertices;
            indices = l.indices;
        }

        public IndexedTriangleList DeepCopy()
        {
            IndexedTriangleList other = (IndexedTriangleList)this.MemberwiseClone();
            other.vertices = new List<Vec3<float>>(vertices);
            other.indices = new List<int>(indices);
            return other;
        }

        public List<Vec3<float>> vertices;
        public List<int> indices;
    };
}
