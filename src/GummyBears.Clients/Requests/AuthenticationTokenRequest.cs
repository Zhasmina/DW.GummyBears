using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public class AuthenticationTokenRequest : IRequest
    {
        public string AuthenticationToken { get; set; }

        public string CorrelationToken { get; set; }
    }
}
