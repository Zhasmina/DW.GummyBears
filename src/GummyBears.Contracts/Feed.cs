using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Contracts
{
    public class Feed
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int AuthorId { get; set; }

        public string AurhorName { get; set; }
    }
}
