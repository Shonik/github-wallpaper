using System;
using System.IO;
using Newtonsoft.Json;
using GitHubWallpaper.Models;

namespace GitHubWallpaper.Core
{
    public class ConfigManager
    {
        private static readonly string ConfigPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GitHubWallpaper",
            "config.json"
        );

        public static AppConfig Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    var json = File.ReadAllText(ConfigPath);
                    var config = JsonConvert.DeserializeObject<AppConfig>(json);
                    if (config != null)
                    {
                        // Ensure cache folder is set
                        if (string.IsNullOrEmpty(config.LocalCacheFolder))
                        {
                            config.LocalCacheFolder = GetDefaultCacheFolder();
                        }
                        return config;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading config: {ex.Message}");
            }

            // Return default config
            var defaultConfig = new AppConfig
            {
                LocalCacheFolder = GetDefaultCacheFolder()
            };
            return defaultConfig;
        }

        public static void Save(AppConfig config)
        {
            try
            {
                var directory = Path.GetDirectoryName(ConfigPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving config: {ex.Message}");
            }
        }

        private static string GetDefaultCacheFolder()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "GitHubWallpapers"
            );
        }
    }
}
