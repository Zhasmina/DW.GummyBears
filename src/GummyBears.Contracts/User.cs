using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class User : UserProfile
    {
        [Required(ErrorMessage = "The password should be between 6 and 20 charactest and should contain at least one small letter, one capital letter and one digit")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$", 
            ErrorMessage = "The password should be between 6 and 20 charactest and should contain at least one small letter, one capital letter and one digit")]
        public string Password { get; set; }
    }
}
