using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using GitHubWallpaper.Models;

namespace GitHubWallpaper.Core
{
    public class WallpaperManager
    {
        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public static void ConfigureSlideshow(string folderPath, int intervalMinutes, bool shuffle, WallpaperFit fitMode = WallpaperFit.Fit)
        {
            try
            {
                // Ensure folder exists
                if (!Directory.Exists(folderPath))
                {
                    throw new DirectoryNotFoundException($"Folder not found: {folderPath}");
                }

                // Convert minutes to milliseconds
                var intervalMs = intervalMinutes * 60 * 1000;

                // CRITICAL: First set the theme to slideshow mode
                using (var themeKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Slideshow"))
                {
                    if (themeKey != null)
                    {
                        themeKey.SetValue("Interval", intervalMs, RegistryValueKind.DWord);
                        themeKey.SetValue("Shuffle", shuffle ? 1 : 0, RegistryValueKind.DWord);
                        themeKey.SetValue("ImagesRootPIDL", new byte[0], RegistryValueKind.Binary);
                        themeKey.SetValue("RssFeed", "", RegistryValueKind.String);
                    }
                }

                // Set slideshow items (the actual folder path)
                using (var itemsKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers\Slideshow"))
                {
                    if (itemsKey != null)
                    {
                        itemsKey.SetValue("Interval", intervalMs, RegistryValueKind.DWord);
                        itemsKey.SetValue("Shuffle", shuffle ? 1 : 0, RegistryValueKind.DWord);
                    }
                }

                // Desktop settings - CRITICAL for fit mode
                using (var desktopKey = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop"))
                {
                    if (desktopKey != null)
                    {
                        // Set wallpaper style based on fit mode
                        desktopKey.SetValue("WallpaperStyle", ((int)fitMode).ToString(), RegistryValueKind.String);
                        desktopKey.SetValue("TileWallpaper", fitMode == WallpaperFit.Tile ? "1" : "0", RegistryValueKind.String);
                    }
                }

                // Personalization - set background type to slideshow
                using (var personalizeKey = Registry.CurrentUser.CreateSubKey(@"Control Panel\Personalization\Desktop Slideshow"))
                {
                    if (personalizeKey != null)
                    {
                        personalizeKey.SetValue("Interval", intervalMs, RegistryValueKind.DWord);
                        personalizeKey.SetValue("Shuffle", shuffle ? 1 : 0, RegistryValueKind.DWord);
                    }
                }

                // MOST IMPORTANT: Set the Background type in Personalization settings
                using (var backgroundKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers"))
                {
                    if (backgroundKey != null)
                    {
                        // Set background style to slideshow (2 = slideshow)
                        backgroundKey.SetValue("BackgroundType", 2, RegistryValueKind.DWord);
                    }
                }

                // Create a multi-string value with the folder path for the slideshow
                using (var currentThemeKey = Registry.CurrentUser.CreateSubKey(@"Control Panel\Personalization\Desktop Slideshow"))
                {
                    if (currentThemeKey != null)
                    {
                        // Windows expects the folder path without trailing slash
                        var cleanPath = folderPath.TrimEnd('\\');
                        currentThemeKey.SetValue("SlideshowDirectory", cleanPath, RegistryValueKind.String);
                    }
                }

                RefreshDesktop();
                System.Threading.Thread.Sleep(500);
                RefreshDesktop();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to configure slideshow: {ex.Message}", ex);
            }
        }

        private static bool IsImageFile(string path)
        {
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp" || ext == ".gif" || ext == ".webp";
        }

        public static void SetWallpaper(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("Wallpaper image not found", imagePath);

            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        public static void EnableSlideshow()
        {
            try
            {
                // Set registry to slideshow mode
                using (var backgroundKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Wallpapers"))
                {
                    if (backgroundKey != null)
                    {
                        // BackgroundType: 0=Image, 1=SolidColor, 2=Slideshow
                        backgroundKey.SetValue("BackgroundType", 2, RegistryValueKind.DWord);
                    }
                }

                using (var key = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop"))
                {
                    if (key != null)
                    {
                        key.SetValue("Slideshow", "1", RegistryValueKind.String);
                    }
                }
                
                RefreshDesktop();
                
                // Try to trigger slideshow start via PowerShell
                try
                {
                    var psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = @"-NoProfile -WindowStyle Hidden -Command ""& {
                            $code = @'
using System;
using System.Runtime.InteropServices;
public class Wallpaper {
    [DllImport(""user32.dll"", CharSet=CharSet.Auto)]
    public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
}
'@
                            Add-Type -TypeDefinition $code
                            [Wallpaper]::SystemParametersInfo(0x0014, 0, $null, 0x0002)
                        }""",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    var proc = System.Diagnostics.Process.Start(psi);
                    proc?.WaitForExit(3000);
                }
                catch { }
                
                System.Threading.Thread.Sleep(500);
                RefreshDesktop();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to enable slideshow: {ex.Message}", ex);
            }
        }

        public static void DisableSlideshow()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                if (key != null)
                {
                    key.SetValue("Slideshow", "0", RegistryValueKind.String);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to disable slideshow: {ex.Message}", ex);
            }
        }

        private static void RefreshDesktop()
        {
            // Force Windows to refresh desktop
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, null, SPIF_SENDCHANGE);
        }
    }
}
