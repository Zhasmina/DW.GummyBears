using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Contracts
{
    public class AuthenticationData
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }
    }
}
