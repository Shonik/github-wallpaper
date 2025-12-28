![Build Status](https://github.com/Shonik/github-wallpaper/workflows/Build%20and%20Release/badge.svg)
![Downloads](https://img.shields.io/github/downloads/Shonik/github-wallpaper/total)
![Release](https://img.shields.io/github/v/release/Shonik/github-wallpaper)
![License](https://img.shields.io/github/license/Shonik/github-wallpaper)
![Stars](https://img.shields.io/github/stars/Shonik/github-wallpaper)

# 🖼️ GitHub Wallpaper

**Automatic wallpaper rotation from GitHub repositories for Windows**

A lightweight Windows system tray application that synchronizes and displays wallpapers from any GitHub repository with automatic rotation using Windows native slideshow.

![Platform](https://img.shields.io/badge/platform-Windows%2010%2B-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

## ✨ Features

- 🎨 **System Tray Application** - Runs discreetly in the background
- 🔄 **GitHub Sync** - Pull wallpapers from any public or private repository
- 🌳 **Visual Selection** - TreeView interface to select specific images
- 🎬 **Native Windows Slideshow** - Uses Windows slideshow with smooth transitions
- 🧠 **Smart Buffering** - Maintains 2-3 images locally to optimize downloads
- ⚙️ **Configurable** - Rotation interval, sequential/random order, fit modes
- 💾 **Persistent Settings** - Your preferences are automatically saved
- 🪶 **Lightweight** - Only ~20-35 MB RAM usage

## 🚀 Quick Start

### Download & Run

1. Download the latest release from [Releases](../../releases)
2. Extract `GitHubWallpaper.exe`
3. Double-click to launch
4. An icon appears in the system tray (taskbar)

### First Configuration

1. **Right-click** the tray icon → **Settings**
2. **Repository tab**:
   - Enter GitHub URL (e.g., `https://github.com/dharmx/walls`)
   - Add GitHub token if private repo
   - Click **"Load Repository Structure"**
3. **Image Selection tab**:
   - Check/uncheck desired images
   - Folders can be toggled to select all children
4. **Slideshow Settings tab**:
   - Set rotation interval (default: 15 minutes)
   - Choose sequential or random order
   - Select fit mode (recommended: "Fit" for ultra-wide screens)
5. Click **Save**
6. **Right-click** tray icon → **Start**

### ⚠️ Important: Manual Activation Required

Windows requires manual slideshow activation the first time:

1. **Right-click Desktop** → **Personalize**
2. **Background** → Type: **"Slideshow"**
3. **Browse** → Select: `%USERPROFILE%\Pictures\GitHubWallpapers`
4. Other settings are already configured ✅

After this one-time setup, the app manages everything automatically.

## 📋 Requirements

- **OS**: Windows 10 (1803+) or Windows 11
- **.NET**: 8.0 Runtime (or use self-contained build)
- **Disk**: 100 MB free space (for cache)
- **Network**: Internet connection

## 🛠️ Build from Source

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (optional)

### Quick Build

```bash
# Clone repository
git clone https://github.com/yourusername/github-wallpaper.git
cd github-wallpaper

# Build (Windows)
build.bat

# OR manual build
dotnet build -c Release

# OR self-contained build (no .NET required)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Executable will be in: `bin/Release/net8.0-windows/win-x64/publish/`

## 📁 Project Structure

```
GitHubWallpaper/
├── Core/                    # Business logic
│   ├── ConfigManager.cs     # JSON configuration
│   ├── GitHubService.cs     # GitHub API integration
│   ├── WallpaperManager.cs  # Windows registry manipulation
│   └── ImageRotator.cs      # Rotation orchestration
├── UI/                      # User interface
│   ├── SettingsWindow.xaml  # WPF settings window
│   └── SettingsWindow.xaml.cs
├── Models/                  # Data models
│   └── AppConfig.cs
└── App.xaml.cs             # System tray app
```

## ⚙️ Configuration

### Settings Location

- **Configuration**: `%APPDATA%\GitHubWallpaper\config.json`
- **Image Cache**: `%USERPROFILE%\Pictures\GitHubWallpapers\`

### Configuration File (JSON)

```json
{
  "RepoUrl": "https://github.com/dharmx/walls",
  "GitHubToken": null,
  "RotationIntervalMinutes": 15,
  "IsRandomOrder": false,
  "FitMode": "Fit",
  "SelectedImages": ["nature/image1.jpg", "..."],
  "CurrentIndex": 0,
  "BufferSize": 3,
  "LocalCacheFolder": "C:\\Users\\...\\Pictures\\GitHubWallpapers"
}
```

### Fit Modes

- **Fit** (Recommended) - Preserves aspect ratio, no distortion
- **Fill** - Fills screen, may crop slightly
- **Stretch** - Stretches to fill (distorts images)
- **Center** - Centers without resizing
- **Span** - Extends across multiple monitors

## 🔧 Troubleshooting

### Slideshow doesn't rotate

1. Wait for configured interval (default: 15 minutes)
2. Verify images are in cache folder
3. Manually activate slideshow via Windows Settings
4. See [TROUBLESHOOTING.md](TROUBLESHOOTING.md) for details

### Images not downloading

1. Check internet connection
2. Verify GitHub token (for private repos)
3. Check tray icon notifications for errors

### API rate limit exceeded

- Add a GitHub token (Settings > Repository tab)
- Tokens increase limit from 60/hour to 5000/hour
- Create token at: https://github.com/settings/tokens

See [TROUBLESHOOTING.md](TROUBLESHOOTING.md) for complete guide.

## 📚 Documentation

- [Installation Guide](INSTALLATION.md)
- [Manual Slideshow Setup](MANUAL_SLIDESHOW_SETUP.md)
- [Troubleshooting](TROUBLESHOOTING.md)
- [Project Overview](PROJECT_OVERVIEW.md)
- [Changelog](CHANGELOG.md)

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Development Setup

1. Fork the repository
2. Clone your fork
3. Open `GitHubWallpaper.sln` in Visual Studio 2022
4. Make your changes
5. Test thoroughly
6. Submit a PR

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [Octokit.NET](https://github.com/octokit/octokit.net) - GitHub API library
- [Newtonsoft.Json](https://www.newtonsoft.com/json) - JSON serialization
- [dharmx/walls](https://github.com/dharmx/walls) - Example wallpaper repository

## 📧 Support

- **Issues**: [GitHub Issues](../../issues)
- **Discussions**: [GitHub Discussions](../../discussions)

---

**Made with ❤️ for the Windows community**
