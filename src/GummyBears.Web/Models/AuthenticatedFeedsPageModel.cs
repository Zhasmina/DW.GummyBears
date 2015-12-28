using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedFeedsPageModel : FeedsPage
    {
        public string AuthenticationToken { get; set; }

        public int UserId { get; set; }
    }
}