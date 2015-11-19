using System;
using System.IO;
using System.Security.Cryptography;

namespace GummyBears.CreationRightsManager
{
    public class Creation : CreationRightsData
    {
        public MemoryStream Data { get; set; }

        public string DataFootprint
        {
            get
            {
                RIPEMD160 myRIPEMD160 = RIPEMD160.Create();
                var hash = myRIPEMD160.ComputeHash(Data);
                return Convert.ToBase64String(hash);
            }
        }

        public string ToMetaString()
        {
            return string.Format("{0}/{1}/{2}/{3}", Author, Owner, TimeOfCreation.Ticks, DataFootprint);
        }
    }
}
