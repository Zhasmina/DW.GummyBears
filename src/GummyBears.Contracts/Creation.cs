
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Contracts
{
    public class Creation
    {
        public int UserId { get; set; }

        public int CreationId { get; set; }

        public string CreationName { get; set; }

        public string CreationPath { get; set; }

        public string Footprint { get; set; }

        public string Signature { get; set; }
    }
}
