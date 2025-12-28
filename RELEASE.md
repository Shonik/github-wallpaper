# Release Guide

This guide is for maintainers creating new releases.

## Version Numbering

Follow [Semantic Versioning](https://semver.org/):
- **MAJOR**: Breaking changes
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes

Format: `vMAJOR.MINOR.PATCH` (e.g., `v1.2.3`)

## Release Checklist

### 1. Pre-Release

- [ ] Update CHANGELOG.md with all changes
- [ ] Update version in AssemblyInfo (if applicable)
- [ ] Test on clean Windows installation
- [ ] Verify all features work
- [ ] Check for memory leaks
- [ ] Review open issues and PRs

### 2. Build

```bash
# Clean previous builds
dotnet clean -c Release

# Build self-contained
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

# Test the published executable
.\bin\Release\net8.0-windows\win-x64\publish\GitHubWallpaper.exe
```

### 3. Create Release

```bash
# Create and push tag
git tag -a v1.0.0 -m "Release version 1.0.0"
git push origin v1.0.0
```

GitHub Actions will automatically:
- Build the project
- Create a release
- Attach the executable

### 4. Post-Release

- [ ] Verify release on GitHub
- [ ] Test download link
- [ ] Update documentation if needed
- [ ] Announce on discussions/social media
- [ ] Close related issues

## Release Notes Template

```markdown
## What's New in v1.0.0

### ✨ Features
- New feature description

### 🐛 Bug Fixes
- Bug fix description

### 📚 Documentation
- Documentation updates

### 🔧 Improvements
- Performance improvements
- UI/UX enhancements

## Installation

Download `GitHubWallpaper.exe` from the assets below and run it.

## Full Changelog
[v0.9.0...v1.0.0](https://github.com/user/repo/compare/v0.9.0...v1.0.0)
```

## Hotfix Releases

For critical bugs:

1. Create hotfix branch from main
2. Fix the bug
3. Test thoroughly
4. Merge to main
5. Tag as PATCH version (e.g., v1.0.1)

## Beta Releases

For testing new features:

1. Tag as pre-release: `v1.1.0-beta.1`
2. Mark as "Pre-release" on GitHub
3. Gather feedback
4. Fix issues
5. Release stable version
