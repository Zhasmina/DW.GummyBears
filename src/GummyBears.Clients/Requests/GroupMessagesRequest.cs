using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public class GroupMessagesRequest: IRequest
    {
        public string AuthenticationToken { get; set; }

        public string CorrelationToken { get; set; }

        public int GroupId { get; set; }

        public int UserId { get; set; }
    }
}
