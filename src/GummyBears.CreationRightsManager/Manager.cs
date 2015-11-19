using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography;
using System.IO;

namespace GummyBears.CreationRightsManager
{
    public class Manager
    {
        private string _cryptoKey;

        public Manager()
        {
            _cryptoKey = ConfigurationManager.AppSettings["CreationRightsManager.CryptoKey"];
        }

        public CreationCertificateData Register(Creation creation)
        {
            string metaString = creation.ToMetaString();
            CreationCertificateData result = new CreationCertificateData
            {
                Author = creation.Author,
                Owner = creation.Owner,
                TimeOfCreation = creation.TimeOfCreation,
                CreationFootprint = creation.DataFootprint,
                Signature = GetSignature(metaString, _cryptoKey)
            };

            return result;
        }

        public string GetSignature(string input, string key)
        {
            HMACSHA1 myhmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            byte[] byteArray = Encoding.UTF8.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            return Convert.ToBase64String(myhmacsha1.ComputeHash(stream));
        }
    }
}
