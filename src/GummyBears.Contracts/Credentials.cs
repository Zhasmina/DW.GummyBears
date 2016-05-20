using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Contracts
{
    public class Credentials
    { 
        [Required(AllowEmptyStrings =false, ErrorMessage ="Username must be specified")]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{4,15}$",
        ErrorMessage = "Username can contain only letters, digits, dots and down slashes. Length of the name must be between 4 and 15 characters")]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "The password should be between 6 and 20 charactest and should contain at least one small letter, one capital letter and one digit")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$",
            ErrorMessage = "The password should be between 6 and 20 charactest and should contain at least one small letter, one capital letter and one digit")]
        public string Password { get; set; }
    }
}
