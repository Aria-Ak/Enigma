using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class Config
    {
        public string LastUsername { get; set; }
        public string LastPassword { get; set; }
        public bool RememberMe { get; set; }
        public bool StayLoggedIn { get; set; }

        public Config()
        {
            RememberMe = false;
            StayLoggedIn = false;
        }
    }
}
