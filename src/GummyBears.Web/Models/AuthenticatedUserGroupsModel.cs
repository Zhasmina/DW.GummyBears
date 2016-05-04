using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedUserGroupsModel
    {
        public string AuthenticationToken { get; set; }

        public List<Group> Groups { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }
    }
}