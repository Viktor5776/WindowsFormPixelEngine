using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    class IndexedTriangleList<T>
    {
        public IndexedTriangleList()
        {
            vertices = new List<T>();
            indices = new List<int>();
        }

        public IndexedTriangleList(IndexedTriangleList<T> l)
        {
            vertices = l.vertices;
            indices = l.indices;
        }

        public IndexedTriangleList<T> DeepCopy()
        {
            IndexedTriangleList<T> other = new IndexedTriangleList<T>();
            other.vertices = new List<T>();
            foreach (var v in this.vertices)
            {
                other.vertices.Add(v);
            }
            other.indices = new List<int>();
            foreach(var i in this.indices)
            {
                other.indices.Add(i);
            }
            
            return other;
        }

        public List<T> vertices;
        public List<int> indices;
    };
}
