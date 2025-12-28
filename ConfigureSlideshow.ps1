# Script PowerShell pour configurer le diaporama Windows manuellement
# À utiliser si l'application ne parvient pas à activer le diaporama automatiquement

param(
    [string]$FolderPath = "$env:USERPROFILE\Pictures\GitHubWallpapers",
    [int]$IntervalMinutes = 15,
    [bool]$Shuffle = $false
)

Write-Host "Configuration du diaporama Windows..." -ForegroundColor Cyan
Write-Host "Dossier: $FolderPath" -ForegroundColor Yellow
Write-Host "Intervalle: $IntervalMinutes minutes" -ForegroundColor Yellow

# Vérifier que le dossier existe
if (-not (Test-Path $FolderPath)) {
    Write-Host "ERREUR: Le dossier n'existe pas!" -ForegroundColor Red
    exit 1
}

# Convertir en millisecondes
$IntervalMs = $IntervalMinutes * 60 * 1000
$ShuffleValue = if ($Shuffle) { 1 } else { 0 }

# Configurer le registre
try {
    # Desktop Slideshow
    $slideshowPath = "HKCU:\Control Panel\Personalization\Desktop Slideshow"
    if (-not (Test-Path $slideshowPath)) {
        New-Item -Path $slideshowPath -Force | Out-Null
    }
    Set-ItemProperty -Path $slideshowPath -Name "Interval" -Value $IntervalMs -Type DWord
    Set-ItemProperty -Path $slideshowPath -Name "Shuffle" -Value $ShuffleValue -Type DWord
    
    # Themes
    $themePath = "HKCU:\Software\Microsoft\Windows\CurrentVersion\Themes"
    if (-not (Test-Path $themePath)) {
        New-Item -Path $themePath -Force | Out-Null
    }
    Set-ItemProperty -Path $themePath -Name "WallpaperPath" -Value $FolderPath -Type String
    
    # Slideshow sub-key
    $slideshowThemePath = "$themePath\Slideshow"
    if (-not (Test-Path $slideshowThemePath)) {
        New-Item -Path $slideshowThemePath -Force | Out-Null
    }
    Set-ItemProperty -Path $slideshowThemePath -Name "Interval" -Value $IntervalMs -Type DWord
    Set-ItemProperty -Path $slideshowThemePath -Name "Shuffle" -Value $ShuffleValue -Type DWord
    Set-ItemProperty -Path $slideshowThemePath -Name "ImagesRootPath" -Value $FolderPath -Type String
    
    Write-Host "Configuration terminée avec succès!" -ForegroundColor Green
    Write-Host ""
    Write-Host "IMPORTANT: Pour activer le diaporama, suivez ces étapes manuellement:" -ForegroundColor Yellow
    Write-Host "1. Clic droit sur le Bureau > Personnaliser" -ForegroundColor White
    Write-Host "2. Arrière-plan > Type: Diaporama" -ForegroundColor White
    Write-Host "3. Parcourir > Sélectionnez: $FolderPath" -ForegroundColor White
    Write-Host "4. Définissez l'intervalle et les autres options" -ForegroundColor White
    Write-Host ""
}
catch {
    Write-Host "ERREUR: $_" -ForegroundColor Red
    exit 1
}
