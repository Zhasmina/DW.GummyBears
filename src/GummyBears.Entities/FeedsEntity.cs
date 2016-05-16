using Dapperer;
using System;

namespace GummyBears.Entities
{
    [Table("Feeds")]
    public class FeedsEntity : BaseEntity
    {
        [Column("Text")]
        public string Text { get; set; }

        [Column("AuthorId")]
        public int AuthorId { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
