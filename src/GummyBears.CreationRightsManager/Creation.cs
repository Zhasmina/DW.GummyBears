using System;
using System.IO;
using System.Security.Cryptography;

namespace GummyBears.CreationRightsManager
{
    public class Creation
    {
        public string Author { get; set; }

        public string Owner { get; set; }

        public DateTime TimeOfCreation { get; set; }

        public MemoryStream Data { get; set; }

        public string ToMetaString()
        {
            RIPEMD160 myRIPEMD160 = RIPEMD160.Create();
            // Compute the hash of the fileStream.
            var hashValue = myRIPEMD160.ComputeHash(Data);
            return string.Format("{0}/{1}/{2}/{3}", Author, Owner, TimeOfCreation.Ticks, Convert.ToBase64String(hashValue));
        }
    }
}
