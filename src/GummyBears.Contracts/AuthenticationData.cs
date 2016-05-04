using System.ComponentModel.DataAnnotations;

namespace GummyBears.Contracts
{
    public class AuthenticationData
    {
        public string Username { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }
    }
}
