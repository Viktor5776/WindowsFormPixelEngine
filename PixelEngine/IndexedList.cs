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

    class IndexedTriangleList<T>
    {
        public IndexedTriangleList()
        {
            vertices = new List<T>();
            indices = new List<int>();
            cullFlags = new List<bool>();
        }

        public IndexedTriangleList(IndexedTriangleList<T> l)
        {
            vertices = l.vertices;
            indices = l.indices;
            cullFlags = l.cullFlags;
        }

        public IndexedTriangleList<T> DeepCopy()
        {
            IndexedTriangleList<T> other = (IndexedTriangleList<T>)this.MemberwiseClone();
            other.vertices = new List<T>(vertices);
            other.indices = new List<int>(indices);
            other.cullFlags = new List<bool>(cullFlags);
            return other;
        }

        public List<T> vertices;
        public List<int> indices;
        public List<bool> cullFlags;
    };
}
