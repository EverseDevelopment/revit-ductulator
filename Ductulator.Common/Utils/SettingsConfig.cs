using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;

namespace Ductulator.Common.Utils
{
    public static class SettingsConfig
    {
        private static readonly string _configFile =
            Path.Combine(Links.configDir, "nancy.config");

        private static readonly object _locker = new object();

        static SettingsConfig()
        {
            // Ensure folder exists
            if (!Directory.Exists(Links.configDir))
                Directory.CreateDirectory(Links.configDir);

            // Ensure file exists with defaults
            if (!File.Exists(_configFile))
                CreateDefaultConfig();
        }

        public static string GetValue(string key)
        {
            if (!File.Exists(_configFile))
                CreateDefaultConfig();

            lock (_locker)
            {
                try
                {
                    Configuration config = OpenConfig();
                    KeyValueConfigurationElement setting = config.AppSettings.Settings[key];
                    return setting != null ? setting.Value : null;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Error retrieving value for key '{key}'.", ex);
                }
            }
        }

        public static void SetValue(string key, string value)
        {
            if (!File.Exists(_configFile))
                CreateDefaultConfig();

            lock (_locker)
            {
                try
                {
                    Configuration config = OpenConfig();
                    KeyValueConfigurationCollection settings = config.AppSettings.Settings;

                    if (settings[key] == null)
                        settings.Add(key, value);
                    else
                        settings[key].Value = value;

                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Error setting value for key '{key}'.", ex);
                }
            }
        }
        private static Configuration OpenConfig()
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = _configFile };
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        /// <summary>
        /// Creates leia.config with the requested default keys/values.
        /// </summary>
        private static void CreateDefaultConfig()
        {
            // Your requested defaults (removed the duplicated "runs" key to avoid errors).
            var defaults = new Dictionary<string, string>
            {
                {"selectedunit", "Inches (decimal)"}
            };

            var doc = new XmlDocument();
            var decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(decl);

            XmlElement configuration = doc.CreateElement("configuration");
            doc.AppendChild(configuration);

            XmlElement appSettings = doc.CreateElement("appSettings");
            configuration.AppendChild(appSettings);

            foreach (var kvp in defaults)
            {
                XmlElement add = doc.CreateElement("add");
                add.SetAttribute("key", kvp.Key);
                add.SetAttribute("value", kvp.Value);
                appSettings.AppendChild(add);
            }

            doc.Save(_configFile);
        }
    }
}
