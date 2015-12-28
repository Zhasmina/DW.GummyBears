using System;
using System.IO;
using System.Security.Cryptography;

namespace GummyBears.CreationRightsManager
{
    public class Creation : CreationRightsData
    {
        private string _dataFootPrint;
        private MemoryStream _data;

        public MemoryStream Data
        {
            private get
            {
                return _data;
            }
            set
            {

                if (_dataFootPrint != null)
                {
                    throw new Exception("Data should not be set after data footprint.");
                }

                _data = value;
            }
        }

        public string DataFootprint
        {
            get
            {
                if (_dataFootPrint == null)
                {
                    RIPEMD160 myRIPEMD160 = RIPEMD160.Create();
                    var hash = myRIPEMD160.ComputeHash(Data);
                    _dataFootPrint = Convert.ToBase64String(hash);
                }

                return _dataFootPrint;
            }

            set
            {
                if (Data != null)
                {
                    throw new Exception("If Data is not null, footprint should be calculated, not set");
                }

                _dataFootPrint = value;
            }
        }

        public string ToMetaString()
        {
            return string.Format("{0}/{1}/{2}/{3}", Author, Owner, TimeOfCreation.Ticks, DataFootprint);
        }
    }
}
