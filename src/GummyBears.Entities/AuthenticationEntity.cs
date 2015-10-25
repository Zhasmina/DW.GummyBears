using Dapperer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GummyBears.Entities
{
    [Table("Authentications")]
    public class AuthenticationEntity : IIdentifier<string>
    {
        [Column("Token", IsPrimary = true, AutoIncrement = false)]
        public string Token { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("LastSeen")]
        public DateTime LastSeen { get; set; }

        public string GetIdentity()
        {
            return Token;
        }

        public void SetIdentity(string identity)
        {
            Token = identity;
        }
    }
}
