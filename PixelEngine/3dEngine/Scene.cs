using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelEngine
{
    internal class Scene
    {
        public virtual void Update(Keyboard kbd, float dt) { }
	    public virtual void Draw() { }

    }
}
