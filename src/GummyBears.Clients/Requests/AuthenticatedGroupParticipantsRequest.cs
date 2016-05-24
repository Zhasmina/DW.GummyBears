using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public class AuthenticatedGroupParticipantsRequest : RequestBase<int>
    {
        public string AuthenticationToken { get; set; }
        public int GroupId { get; set; }
    }
}
