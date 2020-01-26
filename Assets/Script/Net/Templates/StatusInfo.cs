using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Net.Templates
{
    public class StatusInfo
    {
        public Version version;
        public Players players;
        public Description description;
        public class Version
        {
            public string name;
            public int protocol;
        }
        public class Players
        {
            public int max;
            public int online;
        }
        public class Description
        {

        }
    }
}
