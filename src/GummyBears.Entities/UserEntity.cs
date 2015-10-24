using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapperer;

namespace GummyBears.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        [Column("Username")]
        public string UserName { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("Password")]
        public string Password { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("TelephoneNumber")]
        public string TelephoneNumber { get; set; }

        [Column("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Country")]
        public string Country { get; set; }

        [Column("ProfilePicturePath")]
        public string ProfilePicturePath { get; set; }
    }
}
