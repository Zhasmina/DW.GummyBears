using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Contracts
{
    public class GroupMessage
    {
        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string Message { get; set; }

        public DateTime SendDate { get; set; }
    }
}
