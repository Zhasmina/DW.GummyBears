using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
