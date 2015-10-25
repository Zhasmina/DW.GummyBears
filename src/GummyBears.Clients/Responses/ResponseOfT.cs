using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Responses
{
    public class Response<T> : Response
    {
        public T Payload { get; set; }
    }
}
