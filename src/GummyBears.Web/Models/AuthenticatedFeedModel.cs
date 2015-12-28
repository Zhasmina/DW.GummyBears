using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedFeedModel
    {
        public string AuthenticationToken { get; set; }

        public int UserId { get; set; }

        public string MessageText { get; set; }
    }
}