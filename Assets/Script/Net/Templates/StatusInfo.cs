using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Net.Templates
{
    public class StatusInfo
    {
        public Version version { get; set; }
        public Players players { get; set; }
        public Description description { get; set; }
        public class Version
        {
            public string name { get; set; }
            public int protocol { get; set; }
        }
        public class Players
        {
            public int max { get; set; }
            public int online { get; set; }
        }
        public class Description
        {

        }
    }
}
