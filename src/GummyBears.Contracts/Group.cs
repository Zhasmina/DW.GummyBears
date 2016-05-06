using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class Group
    {
        public int GroupId { get; set; }
        
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{5,50}$",
            ErrorMessage = "Group name can contain only letters, digits, dots and down slashes. Length of the name must be between 5 and 50 characters")]
        public string GroupName { get; set; }

        public int AuthorId { get; set; }
    }
}
