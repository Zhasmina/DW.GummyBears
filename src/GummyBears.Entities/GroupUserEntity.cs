using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapperer;

namespace GummyBears.Entities
{
    [Table("UserGroups")]
    public class GroupUserEntity : BaseEntity
    {
        [Column("GroupId")]
        public int GroupId { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("IsAdmin")]
        public bool IsAdmin { get; set; }
    }
}
