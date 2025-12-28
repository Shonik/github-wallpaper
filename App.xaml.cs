using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using GitHubWallpaper.Core;
using GitHubWallpaper.Models;
using GitHubWallpaper.UI;
using Application = System.Windows.Application;

namespace GitHubWallpaper
{
    public partial class App : Application
    {
        private NotifyIcon? _notifyIcon;
        private ImageRotator? _rotator;
        private AppConfig _config = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Load configuration
            _config = ConfigManager.Load();

            // Create system tray icon
            CreateTrayIcon();

            // Auto-start if configured
            if (_config.SelectedImages.Count > 0)
            {
                StartRotation();
            }
            else
            {
                ShowSettingsWindow();
            }
        }

        private void CreateTrayIcon()
        {
            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Application, // You can replace with custom icon
                Visible = true,
                Text = "GitHub Wallpaper"
            };

            var contextMenu = new ContextMenuStrip();
            
            var startItem = new ToolStripMenuItem("Start", null, OnStart)
            {
                Name = "StartItem"
            };
            var stopItem = new ToolStripMenuItem("Stop", null, OnStop)
            {
                Name = "StopItem",
                Enabled = false
            };
            var settingsItem = new ToolStripMenuItem("Settings...", null, OnSettings);
            var separator = new ToolStripSeparator();
            var exitItem = new ToolStripMenuItem("Exit", null, OnExit);

            contextMenu.Items.AddRange(new ToolStripItem[] 
            { 
                startItem, 
                stopItem, 
                new ToolStripSeparator(),
                settingsItem, 
                separator, 
                exitItem 
            });

            _notifyIcon.ContextMenuStrip = contextMenu;
            _notifyIcon.DoubleClick += (s, e) => ShowSettingsWindow();
        }

        private void OnStart(object? sender, EventArgs e)
        {
            StartRotation();
        }

        private void OnStop(object? sender, EventArgs e)
        {
            StopRotation();
        }

        private void OnSettings(object? sender, EventArgs e)
        {
            ShowSettingsWindow();
        }

        private void OnExit(object? sender, EventArgs e)
        {
            StopRotation();
            _notifyIcon?.Dispose();
            Shutdown();
        }

        private async void StartRotation()
        {
            if (_rotator?.IsRunning == true) return;

            try
            {
                _rotator = new ImageRotator(_config);
                _rotator.StatusChanged += OnStatusChanged;
                _rotator.ErrorOccurred += OnErrorOccurred;

                await _rotator.Start();

                UpdateMenuState(true);
                
                // Show detailed notification with instructions
                ShowBalloonTip(
                    "Wallpaper Rotation Started", 
                    "Images are being downloaded. If the slideshow doesn't start automatically, open Windows Settings > Personalization > Background and set type to 'Slideshow'.", 
                    ToolTipIcon.Info,
                    5000);
            }
            catch (Exception ex)
            {
                ShowBalloonTip("Error", $"Failed to start: {ex.Message}", ToolTipIcon.Error);
            }
        }

        private void StopRotation()
        {
            if (_rotator?.IsRunning != true) return;

            _rotator.Stop();
            UpdateMenuState(false);
            ShowBalloonTip("Rotation Stopped", "Wallpaper rotation has been stopped.", ToolTipIcon.Info);
        }

        private void ShowSettingsWindow()
        {
            var wasRunning = _rotator?.IsRunning == true;
            if (wasRunning)
            {
                StopRotation();
            }

            var settingsWindow = new SettingsWindow(_config);
            settingsWindow.ShowDialog();

            if (settingsWindow.WasSaved)
            {
                _config = ConfigManager.Load();
                
                if (wasRunning || _config.SelectedImages.Count > 0)
                {
                    var result = System.Windows.MessageBox.Show(
                        "Would you like to start the wallpaper rotation now?",
                        "Start Rotation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        StartRotation();
                    }
                }
            }
            else if (wasRunning)
            {
                StartRotation();
            }
        }

        private void OnStatusChanged(object? sender, string message)
        {
            Console.WriteLine($"Status: {message}");
        }

        private void OnErrorOccurred(object? sender, string error)
        {
            Console.WriteLine($"Error: {error}");
            ShowBalloonTip("Error", error, ToolTipIcon.Error);
        }

        private void UpdateMenuState(bool isRunning)
        {
            if (_notifyIcon?.ContextMenuStrip == null) return;

            var startItem = _notifyIcon.ContextMenuStrip.Items["StartItem"] as ToolStripMenuItem;
            var stopItem = _notifyIcon.ContextMenuStrip.Items["StopItem"] as ToolStripMenuItem;

            if (startItem != null) startItem.Enabled = !isRunning;
            if (stopItem != null) stopItem.Enabled = isRunning;
        }

        private void ShowBalloonTip(string title, string text, ToolTipIcon icon, int timeout = 3000)
        {
            _notifyIcon?.ShowBalloonTip(timeout, title, text, icon);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon?.Dispose();
            base.OnExit(e);
        }
    }
}
