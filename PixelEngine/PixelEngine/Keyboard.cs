using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelEngine
{
    internal class Keyboard
    {
        readonly SortedDictionary<Keys, bool> keys = new SortedDictionary<Keys, bool>();
        
        public bool GetKeyPressed(Keys ch)
        {
            if (keys.TryGetValue(ch, out bool result))
            {
                return result;
            }
            else
            {
                keys.Add(ch, false);
                return false;
            }
        }

        public void SetKey(Keys ch, bool val)
        {
            if (keys.TryGetValue(ch, out bool b))
            {
                keys[ch] = val;
            }
            else
            {
                keys.Add(ch, val);
            }
        }
    }
}
