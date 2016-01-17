using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedGroupMessagesModel
    {
        public string AuthenticationToken { get; set; }

        public int UserId { get; set; }

        public int GroupId { get; set; }

        public IEnumerable<GroupMessage> GroupMessages { get; set; }
    }
}