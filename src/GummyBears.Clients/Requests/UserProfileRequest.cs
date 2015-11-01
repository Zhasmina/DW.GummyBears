using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public class UserProfileRequest : IRequest
    {
        public int UserId { get; set; }

        public string CorrelationToken { get; set; }
    }
}