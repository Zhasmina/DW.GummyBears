using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class Creation
    {
        public int UserId { get; set; }

        public int CreationId { get; set; }

        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{3,100}$",
            ErrorMessage = "Creation name can contain only letters, digits, dots and down slashes. Length of the name must be between 3 and 100 characters")]
        public string CreationName { get; set; }

        [Required(ErrorMessage ="Missing creation path")]
        public string CreationPath { get; set; }

        public string Footprint { get; set; }

        public string Signature { get; set; }
    }
}
