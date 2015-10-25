using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace GummyBears.WebApi
{
    public class SimplePrincipal : IPrincipal
    {
        private string _role;

        public IIdentity Identity { get; private set; }

        public SimplePrincipal(string username, string role)
        {
            Identity = new SimpleIdentity(username);
            _role = role;
        }

        public bool IsInRole(string role)
        {
            return Identity != null &&
                Identity.IsAuthenticated && 
                !string.IsNullOrWhiteSpace(role) && 
                _role.Equals(role, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}