using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Contracts
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string TelephoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
