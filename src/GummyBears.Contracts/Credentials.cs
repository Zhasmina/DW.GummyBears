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
        [Required]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{4,15}$")]
        public string Username { get; set; }
   
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,20}$")] // between 6 and 20 chars, at least one small letter, one capital letter and one digital char
        public string Password { get; set; }
    }
}
