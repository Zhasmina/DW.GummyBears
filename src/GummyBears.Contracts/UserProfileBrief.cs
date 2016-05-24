using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class UserProfileBrief
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "First name must be specified")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "First name should be between 1 and 20 characters long")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name must be specified")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "Last name should be between 1 and 20 characters long")]
        public string LastName { get; set; }
    }
}
