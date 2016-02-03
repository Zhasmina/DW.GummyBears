using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public class AuthenticatedGroupParticipantsRequest : RequestBase<List<GroupParticipants>>
    {
        public string AuthenticationToken { get; set; }
    }
}
