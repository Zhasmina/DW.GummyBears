﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Clients.Requests
{
    public interface IRequest
    {
        string CorrelationToken { get; set; }
    }
}
