using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class Feed
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public int AuthorId { get; set; }

        public string AurhorName { get; set; }
    }
}
