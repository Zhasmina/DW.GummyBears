using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapperer;

namespace GummyBears.Entities
{
    [Table("GroupMessages")]
    public class GroupMessageEntity : BaseEntity
    {
        [Column("GroupId")]
        public int GroupId { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("Message")]
        public string Message { get; set; }

        [Column("SendDate")]
        public DateTime SendDate { get; set; }
    }
}
