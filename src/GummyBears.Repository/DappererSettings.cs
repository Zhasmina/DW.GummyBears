using Dapperer;
using GummyBears.Common;

namespace GummyBears.Repository
{
    public class DappererSettings : IDappererSettings
    {
        public string ConnectionString
        {
            get { return AppSettingsProvider.DatabaseConnectionString; }
        }
    }
}
