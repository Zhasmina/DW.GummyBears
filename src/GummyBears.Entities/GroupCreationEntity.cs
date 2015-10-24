using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapperer;

namespace GummyBears.Entities
{
    [Table("GroupCreations")]
    public class GroupCreationEntity : BaseEntity
    {
        [Column("GroupId")]
        public int GroupId { get; set; }

        [Column("CreationId")]
        public int CreationId { get; set; }
    }
}
