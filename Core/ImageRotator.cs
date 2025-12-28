using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GitHubWallpaper.Models;

namespace GitHubWallpaper.Core
{
    public class ImageRotator
    {
        private readonly AppConfig _config;
        private readonly GitHubService _gitHub;
        private Timer? _rotationTimer;
        private bool _isRunning;
        private List<string> _imageQueue = new();
        private readonly Random _random = new();

        public event EventHandler<string>? StatusChanged;
        public event EventHandler<string>? ErrorOccurred;

        public bool IsRunning => _isRunning;

        public ImageRotator(AppConfig config)
        {
            _config = config;
            _gitHub = new GitHubService(_config.GitHubToken);
        }

        public async Task Start()
        {
            if (_isRunning) return;

            try
            {
                _isRunning = true;
                StatusChanged?.Invoke(this, "Starting wallpaper rotation...");

                // Ensure cache folder exists
                if (!Directory.Exists(_config.LocalCacheFolder))
                {
                    Directory.CreateDirectory(_config.LocalCacheFolder);
                }

                // Load the repository structure and get selected images
                StatusChanged?.Invoke(this, "Loading repository structure...");
                var rootNode = await _gitHub.LoadRepositoryStructure(_config.RepoUrl);
                _imageQueue = _gitHub.GetAllImagePaths(rootNode, _config.SelectedImages);

                if (_imageQueue.Count == 0)
                {
                    ErrorOccurred?.Invoke(this, "No images selected. Please select images in settings.");
                    _isRunning = false;
                    return;
                }

                // Shuffle if random order
                if (_config.IsRandomOrder)
                {
                    _imageQueue = _imageQueue.OrderBy(x => _random.Next()).ToList();
                }

                // Download initial buffer of images
                await DownloadNextBatch();

                // Configure Windows slideshow
                WallpaperManager.ConfigureSlideshow(
                    _config.LocalCacheFolder,
                    _config.RotationIntervalMinutes,
                    _config.IsRandomOrder,
                    _config.FitMode
                );
                WallpaperManager.EnableSlideshow();

                StatusChanged?.Invoke(this, $"Slideshow configured! Rotating every {_config.RotationIntervalMinutes} minutes.");
                StatusChanged?.Invoke(this, "IMPORTANT: Open Windows Settings > Personalization > Background and set type to 'Slideshow' if not already done.");

                // Start timer for downloading next images and cleaning up old ones
                var timerInterval = TimeSpan.FromMinutes(_config.RotationIntervalMinutes);
                _rotationTimer = new Timer(OnTimerTick, null, timerInterval, timerInterval);
            }
            catch (Exception ex)
            {
                _isRunning = false;
                ErrorOccurred?.Invoke(this, $"Failed to start: {ex.Message}");
            }
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _rotationTimer?.Dispose();
            _rotationTimer = null;
            _isRunning = false;

            StatusChanged?.Invoke(this, "Wallpaper rotation stopped.");
        }

        private async void OnTimerTick(object? state)
        {
            try
            {
                await DownloadNextBatch();
                CleanupOldImages();
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Error during rotation: {ex.Message}");
            }
        }

        private async Task DownloadNextBatch()
        {
            try
            {
                // Calculate which images to download
                var currentFiles = Directory.GetFiles(_config.LocalCacheFolder)
                    .Select(Path.GetFileName)
                    .Where(f => f != null)
                    .ToHashSet();

                var imagesToDownload = new List<string>();
                
                for (int i = 0; i < _config.BufferSize; i++)
                {
                    var index = (_config.CurrentIndex + i) % _imageQueue.Count;
                    var imagePath = _imageQueue[index];
                    var fileName = Path.GetFileName(imagePath);

                    if (!currentFiles.Contains(fileName))
                    {
                        imagesToDownload.Add(imagePath);
                    }
                }

                // Download missing images
                foreach (var imagePath in imagesToDownload)
                {
                    StatusChanged?.Invoke(this, $"Downloading: {Path.GetFileName(imagePath)}");
                    await _gitHub.DownloadImage(_config.RepoUrl, imagePath, _config.LocalCacheFolder);
                }

                // Move to next image
                _config.CurrentIndex = (_config.CurrentIndex + 1) % _imageQueue.Count;
                ConfigManager.Save(_config);
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Download error: {ex.Message}");
            }
        }

        private void CleanupOldImages()
        {
            try
            {
                var currentFiles = Directory.GetFiles(_config.LocalCacheFolder).ToList();
                
                // Determine which images should be kept (current buffer)
                var imagesToKeep = new HashSet<string>();
                for (int i = -1; i < _config.BufferSize; i++)
                {
                    var index = (_config.CurrentIndex + i + _imageQueue.Count) % _imageQueue.Count;
                    var imagePath = _imageQueue[index];
                    imagesToKeep.Add(Path.GetFileName(imagePath));
                }

                // Delete files not in the keep list
                foreach (var file in currentFiles)
                {
                    var fileName = Path.GetFileName(file);
                    if (!imagesToKeep.Contains(fileName))
                    {
                        try
                        {
                            File.Delete(file);
                            StatusChanged?.Invoke(this, $"Cleaned up: {fileName}");
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, $"Cleanup error: {ex.Message}");
            }
        }
    }
}
