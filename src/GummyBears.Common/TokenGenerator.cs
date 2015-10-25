using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Common
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
