using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Responses
{
    public class Response
    {
        public Status Status { get; set; }

        public IList<string> Errors { get; set; }
    }
}
