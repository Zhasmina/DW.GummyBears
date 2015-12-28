using GummyBears.Clients.Requests;
using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients
{
    public class AuthenticatedFeedRequest : RequestBase<Feed>
    {
        public string AuthenticationToken { get; set; }
    }
}
