using System.Collections.Generic;

namespace GitHubWallpaper.Models
{
    public class AppConfig
    {
        public string RepoUrl { get; set; } = "https://github.com/dharmx/walls";
        public string? GitHubToken { get; set; }
        public int RotationIntervalMinutes { get; set; } = 15;
        public bool IsRandomOrder { get; set; } = false;
        public List<string> SelectedImages { get; set; } = new();
        public int CurrentIndex { get; set; } = 0;
        public int BufferSize { get; set; } = 3;
        public string LocalCacheFolder { get; set; } = "";
        public WallpaperFit FitMode { get; set; } = WallpaperFit.Fit;
    }

    public class ImageNode
    {
        public string Path { get; set; } = "";
        public string Name { get; set; } = "";
        public bool IsDirectory { get; set; }
        public bool IsSelected { get; set; } = true;
        public List<ImageNode> Children { get; set; } = new();
    }

    public enum WallpaperFit
    {
        Fill = 10,      // Remplit l'écran (peut déformer)
        Fit = 6,        // Ajuste sans déformer (recommandé pour ratios différents)
        Stretch = 2,    // Étire pour remplir (déforme)
        Tile = 0,       // Mosaïque
        Center = 0,     // Centre sans redimensionner
        Span = 22       // Étend sur plusieurs écrans
    }
}
