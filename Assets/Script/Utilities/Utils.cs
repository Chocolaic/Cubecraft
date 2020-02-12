using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubecraft.Utilities
{
    class Utils
    {
        public static int Round(float num)
        {
            return num > 0 ? (int)(num + 0.5) : (int)(num - 0.5);
        }
    }
}
