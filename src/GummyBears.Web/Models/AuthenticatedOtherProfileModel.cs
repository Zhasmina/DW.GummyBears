using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedOtherProfileModel
    {
        public string AuthenticationToken { get; set; }
        public int MyUserId { get; set; }
        public string MyUsername { get; set; }
        public int TargetUserId { get; set; }
        public string TargerFirstName { get; set; }
        public string TargetLastName { get; set; }
        public string TargetProfilePath { get; set; }
        public string TargetDescription { get; set; }
        public DateTime TargetDateOfBirth { get; set; }
        public string TargetCountry { get; set; }
    }
}