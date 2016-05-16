using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public class CreateGroupMessageRequest : RequestBase<GroupMessage>
    {
        public string AuthenticationToken { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }
    }
}
