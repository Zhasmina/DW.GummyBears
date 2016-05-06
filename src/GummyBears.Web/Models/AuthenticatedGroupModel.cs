﻿using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GummyBears.Web.Models
{
    public class AuthenticatedGroupModel : Group
    {
        public string AuthenticationToken { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }
    }
}