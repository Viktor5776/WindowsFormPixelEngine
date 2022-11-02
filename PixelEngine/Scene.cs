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
	    public virtual void Draw(PixelGraphics gfx) { }


        public float wrap_angle(float theta)
        {
            float modded = (float)(theta % (2.0f * Math.PI));
            return (modded > Math.PI) ?
                (modded - 2.0f * (float)Math.PI) :
                modded;
        }
    }
}
