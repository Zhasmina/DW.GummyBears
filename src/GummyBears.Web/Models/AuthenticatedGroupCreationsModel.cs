using GummyBears.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GummyBears.Web.Models
{
    public class AuthenticatedGroupCreationsModel
    {
        public string AuthenticationToken { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string Username { get; set; }
        public string GroupName { get; set; }
        public List<SelectListItem> CreationSelectedList { get; set; }
        public List<Creation> Creations { get; set; }
        public int CreationId { get; set; }
    }
}