using System.Configuration;

namespace GummyBears.Clients
{
    public class DefaultGummyBearsClientSettings : IGummyBearClientSettings
    {
        public string BaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["Bede.PlayerMessenger.Url"];
            }
        }
    }
}
