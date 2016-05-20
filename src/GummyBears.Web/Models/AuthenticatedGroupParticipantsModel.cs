﻿using System.Collections.Generic;

namespace GummyBears.Web.Models
{
    public class AuthenticatedGroupParticipantsModel
    {
        public string AuthenticationToken { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public List<Participant> Participants { get; set; }
    }
}