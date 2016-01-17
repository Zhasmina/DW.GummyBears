using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public class AuthenticatedGroupRequest : RequestBase<Group>
    {
        public string AuthenticationToken { get; set; }
    }
}
