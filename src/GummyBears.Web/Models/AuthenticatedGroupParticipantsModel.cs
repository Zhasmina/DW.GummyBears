using System.Collections.Generic;
using System.Web.Mvc;

namespace GummyBears.Web.Models
{
    public class AuthenticatedGroupParticipantsModel
    {
        public string AuthenticationToken { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public List<SelectListItem> ParticipantSelectListItems { get; set; }

        public List<Participant> Participants { get; set; }

        public int ParticipantId { get; set; }
    }
}