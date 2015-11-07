using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Contracts
{
    public class GroupParticipants
    {
        public int GroupId { get; set; }

        public List<int> ParticipantIds { get; set; }
    }
}
