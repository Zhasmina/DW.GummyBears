using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace GummyBears.WebApi
{
    public class SimpleIdentity : IIdentity
    {
        public SimpleIdentity(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public string AuthenticationType
        {
            get
            {
                return "Authorization token";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name { get; private set; }

        public int Id { get; private set; }
    }
}