using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class Creation
    {
        public int UserId { get; set; }

        public int CreationId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{4,15}$")]
        public string CreationName { get; set; }

        [Required(ErrorMessage ="Missing creation path")]
        public string CreationPath { get; set; }

        public string Footprint { get; set; }

        public string Signature { get; set; }
    }
}
