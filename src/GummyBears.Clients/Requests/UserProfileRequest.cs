using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public class UserProfileRequest : AuthenticationTokenRequest
    {
        public int UserId { get; set; }
        public string Username { get; set; }
    }
}