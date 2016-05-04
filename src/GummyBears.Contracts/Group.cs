using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class Group
    {
        public int GroupId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z][A-Za-z0-9._]{5,50}$")]
        public string GroupName { get; set; }

        public int AuthorId { get; set; }
    }
}
