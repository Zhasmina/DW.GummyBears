using System;
using System.Configuration;

namespace GummyBears.Common
{
    public static class AppSettingsProvider
    {
        public static string DatabaseConnectionString
        {
            get
            {
                return string.Format("Server={0},{1};Database={2};User Id={3};Password={4};", GetSetting("db.host"), GetSetting("db.port"), GetSetting("db.name"), GetSetting("db.user"), GetSetting("db.password"));
            }
        }

        private static string GetSetting(string settingName)
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[settingName]))
            {
                return ConfigurationManager.AppSettings[settingName];
            }

            throw new ArgumentNullException(settingName, string.Format("Missing Application Setting for {0}. Check AppSettings", settingName));
        }
    }
}
