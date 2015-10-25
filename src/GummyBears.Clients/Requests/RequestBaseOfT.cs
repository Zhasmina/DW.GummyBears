using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public abstract class RequestBase<T> : IRequest
    {
        public string CorrelationToken { get; set; }

        public T Payload { get; set; }
    }
}
