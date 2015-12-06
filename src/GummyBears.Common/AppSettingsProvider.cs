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
                return string.Format("Server={0};Database={1};User Id={2};Password={3};", GetSetting("db.host"), GetSetting("db.name"), GetSetting("db.user"), GetSetting("db.password"));
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
