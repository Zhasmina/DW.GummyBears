using Dapperer;

namespace GummyBears.Entities
{
    [Table("Groups")]
    public class GroupEntity : BaseEntity
    {
        [Column("Name")]
        public string Name { get; set; }

        [Column("AuthorId")]
        public int AuthorId { get; set; }
    }
}
