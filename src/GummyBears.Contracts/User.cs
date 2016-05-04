using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class User : UserProfile
    {
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$")] // between 6 and 20 chars, at least one small letter, one capital letter and one digital char
        public string Password { get; set; }
    }
}
