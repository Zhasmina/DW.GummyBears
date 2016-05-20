using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedGroupMessageModel
    {
        public string AuthenticationToken { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public string MessageText { get; set; }
    }
}