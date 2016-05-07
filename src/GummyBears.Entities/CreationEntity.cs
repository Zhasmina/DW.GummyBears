using System;
using Dapperer;

namespace GummyBears.Entities
{
    [Table("Creations")]
    public class CreationEntity : BaseEntity
    {
        [Column("UserId")]
        public int UserId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("FilePath")]
        public string FilePath { get; set; }

        [Column("Signiture")]
        public string Signiture { get; set; }

        [Column("CreationFootprint")]
        public string CreationFootprint { get; set; }

        [Column("Author")]
        public string Author { get; set; }

        [Column("Owner")]
        public string Owner { get; set; }

        [Column("TimeOfCreation")]
        public DateTime TimeOfCreation { get; set; }
    }
}
