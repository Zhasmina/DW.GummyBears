using GummyBears.Clients.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class TokenResponse<T> : Response<T>
          where T : class
    {
        public string Token { get; set; }
    }
}