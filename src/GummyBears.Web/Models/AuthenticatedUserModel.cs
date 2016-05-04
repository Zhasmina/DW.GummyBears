using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedUserModel
    {
        public string Token { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }
    }
}