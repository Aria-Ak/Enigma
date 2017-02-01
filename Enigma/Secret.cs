using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma
{
    class Secret
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Remarks { get; set; }

        public override string ToString()
        {
            if (!String.IsNullOrEmpty(Name))
                return Name;

            return base.ToString();
        }
    }
}
