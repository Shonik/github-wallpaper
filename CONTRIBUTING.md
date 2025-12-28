# Contributing to GitHub Wallpaper

Thank you for your interest in contributing! This document provides guidelines and instructions for contributing to this project.

## 🤝 How to Contribute

### Reporting Bugs

If you find a bug, please create an issue with:

1. **Clear title** - Describe the issue concisely
2. **Steps to reproduce** - How to trigger the bug
3. **Expected behavior** - What should happen
4. **Actual behavior** - What actually happens
5. **Environment**:
   - Windows version (Win + R → `winver`)
   - .NET version
   - Application version
6. **Screenshots** - If applicable

### Suggesting Features

Feature requests are welcome! Please:

1. Check existing issues to avoid duplicates
2. Clearly describe the feature and its benefits
3. Explain your use case
4. Be open to discussion

### Pull Requests

1. **Fork** the repository
2. **Create a branch** for your feature:
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes**
4. **Test thoroughly**
5. **Commit** with clear messages:
   ```bash
   git commit -m "Add: feature description"
   ```
6. **Push** to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```
7. **Create a Pull Request** with:
   - Description of changes
   - Related issue (if applicable)
   - Screenshots/demos (if UI changes)

## 🛠️ Development Setup

### Prerequisites

- Windows 10 or 11
- Visual Studio 2022 (Community Edition or higher)
- .NET 8.0 SDK
- Git

### Getting Started

```bash
# Clone your fork
git clone https://github.com/your-username/github-wallpaper.git
cd github-wallpaper

# Open in Visual Studio
start GitHubWallpaper.sln

# Or build from command line
dotnet restore
dotnet build
```

### Project Structure

```
GitHubWallpaper/
├── Core/           # Business logic (no UI dependencies)
├── UI/             # WPF user interface
├── Models/         # Data models and DTOs
└── App.xaml.cs     # Application entry point
```

### Code Style

- **C# conventions**: Follow Microsoft's C# coding conventions
- **Naming**: PascalCase for public members, camelCase for private
- **Comments**: XML documentation for public APIs
- **Line length**: Keep under 120 characters
- **Indentation**: 4 spaces (no tabs)

### Testing

Before submitting a PR:

1. **Build** in Release mode
2. **Test** on a clean Windows installation (VM recommended)
3. **Verify**:
   - Application starts correctly
   - Settings save and load properly
   - GitHub integration works
   - Slideshow activates
   - No memory leaks (check Task Manager after 1 hour)

### Commit Messages

Format: `Type: Short description`

**Types**:
- `Add:` New feature
- `Fix:` Bug fix
- `Update:` Modify existing feature
- `Remove:` Remove feature
- `Refactor:` Code restructuring
- `Docs:` Documentation changes
- `Style:` Formatting changes
- `Test:` Testing changes

**Examples**:
```
Add: Support for WebP image format
Fix: TreeView not restoring selections
Update: Improve slideshow activation reliability
Docs: Add troubleshooting for API rate limits
```

## 🎯 Areas for Contribution

### Good First Issues

- Documentation improvements
- UI/UX enhancements
- Additional image format support
- Localization (translations)
- Bug fixes

### Advanced Features

- Multi-monitor independent wallpapers
- Cloud storage integration (Google Drive, OneDrive)
- Custom transition effects
- Wallpaper scheduling
- Image filtering/effects

## 📝 Code Review Process

1. Maintainer reviews PR within 7 days
2. Feedback provided via PR comments
3. Requested changes must be addressed
4. Once approved, PR is merged
5. Contributor is credited in CHANGELOG.md

## 🐛 Debugging Tips

### Enable Verbose Logging

Launch from command line to see console output:
```bash
GitHubWallpaper.exe > debug.log 2>&1
```

### Common Issues

**Application won't start**:
- Check .NET 8.0 is installed
- Run as administrator (temporarily)
- Check antivirus isn't blocking

**Slideshow not activating**:
- Verify registry keys are being written
- Use `regedit` to inspect registry manually
- Check Windows Event Viewer for errors

## 📚 Resources

- [.NET 8.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [WPF Guide](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
- [Octokit.NET Docs](https://octokitnet.readthedocs.io/)
- [Windows Registry Reference](https://docs.microsoft.com/en-us/windows/win32/sysinfo/registry)

## ⚖️ License

By contributing, you agree that your contributions will be licensed under the MIT License.

## 🙏 Recognition

Contributors will be:
- Listed in CHANGELOG.md for their contributions
- Credited in release notes
- Added to GitHub contributors page

Thank you for contributing to GitHub Wallpaper! 🎉
