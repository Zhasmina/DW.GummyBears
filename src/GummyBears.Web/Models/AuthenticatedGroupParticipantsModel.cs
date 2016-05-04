using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedGroupParticipantsModel
    {
        public string AuthenticationToken { get; set; }

        public int UserId { get; set; }

        public int GroupId { get; set; }

        public IEnumerable<int> ParticipantIds { get; set; }
    }
}