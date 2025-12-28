@echo off
echo ========================================
echo GitHub Wallpaper - Build Script
echo ========================================
echo.

REM Check if dotnet is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: .NET SDK not found!
    echo Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo [1/3] Cleaning previous builds...
dotnet clean -c Release
if exist "bin\Release" rmdir /s /q "bin\Release"
if exist "obj" rmdir /s /q "obj"

echo.
echo [2/3] Building application (self-contained)...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: Build failed!
    pause
    exit /b 1
)

echo.
echo [3/3] Build successful!
echo.
echo Executable location:
echo bin\Release\net8.0-windows\win-x64\publish\GitHubWallpaper.exe
echo.
echo File size:
dir /s "bin\Release\net8.0-windows\win-x64\publish\GitHubWallpaper.exe" | find "GitHubWallpaper.exe"

echo.
echo ========================================
echo Build completed successfully!
echo ========================================
pause
