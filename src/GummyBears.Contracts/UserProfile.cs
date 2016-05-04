using System;
using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{4,15}$")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string TelephoneNumber { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
