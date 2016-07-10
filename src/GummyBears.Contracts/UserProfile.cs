using System;
using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class UserProfile : UserProfileBrief
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Username must be specified")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{3,14}$",
            ErrorMessage = "Username can contain only letters, digits, dots and down slashes. Length of the name must be between 4 and 15 characters")]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email must be specified")]
        [EmailAddress(ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        public string TelephoneNumber { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }

        public string ProfilePicturePath { get; set; }
    }
}
