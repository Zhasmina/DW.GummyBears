using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.CreationRightsManager
{
    public class CreationCertificateData : CreationRightsData
    {
        public string CreationFootprint { get; set; }

        public string Signature { get; set; }
    }
}
