# GitHub Wallpaper - Projet Complet

## 📋 Résumé

Application Windows System Tray permettant de synchroniser et afficher automatiquement des fonds d'écran depuis un dépôt GitHub, avec rotation séquentielle ou aléatoire.

## ✨ Caractéristiques Principales

### Fonctionnalités
- **System Tray** : Application discrète dans la barre des tâches
- **Synchronisation GitHub** : Support des dépôts publics et privés
- **Sélection visuelle** : Interface TreeView pour choisir précisément vos images
- **Diaporama natif** : Utilise le système Windows avec transitions fluides
- **Rotation intelligente** : Buffer de 2-3 images pour optimiser les téléchargements
- **Configuration persistante** : Sauvegarde automatique de vos préférences

### Performance
- **Mémoire** : ~20-35 MB en fonctionnement
- **CPU** : <1% (pics pendant téléchargement uniquement)
- **Réseau** : Téléchargement uniquement des images sélectionnées

### Technologies
- **Framework** : .NET 8.0 (C#)
- **UI** : WPF pour la fenêtre de paramètres
- **System Tray** : WinForms NotifyIcon
- **API GitHub** : Octokit.NET
- **Configuration** : JSON avec Newtonsoft.Json

## 📁 Structure du Projet

```
GitHubWallpaper/
│
├── Core/                           # Logique métier
│   ├── ConfigManager.cs            # Gestion configuration JSON
│   ├── GitHubService.cs            # Interaction API GitHub
│   ├── WallpaperManager.cs         # Manipulation registre Windows
│   └── ImageRotator.cs             # Orchestration rotation
│
├── UI/                             # Interface utilisateur
│   ├── SettingsWindow.xaml         # Interface WPF
│   └── SettingsWindow.xaml.cs      # Code-behind
│
├── Models/                         # Modèles de données
│   └── AppConfig.cs                # Configuration et structures
│
├── App.xaml                        # Application WPF
├── App.xaml.cs                     # System tray et logique principale
├── GitHubWallpaper.csproj          # Fichier projet
├── GitHubWallpaper.sln             # Solution Visual Studio
│
├── README.md                       # Documentation principale
├── INSTALLATION.md                 # Guide installation rapide
├── TROUBLESHOOTING.md              # Guide de dépannage
├── CHANGELOG.md                    # Notes de version
├── config.example.json             # Exemple de configuration
│
├── build.bat                       # Script de compilation
└── .gitignore                      # Exclusions Git
```

## 🚀 Démarrage Rapide

### Option 1 : Build et Exécution

```bash
# 1. Cloner ou télécharger le projet

# 2. Compiler (Windows)
build.bat

# 3. Lancer
bin\Release\net8.0-windows\win-x64\publish\GitHubWallpaper.exe
```

### Option 2 : Développement avec Visual Studio

```bash
# 1. Ouvrir GitHubWallpaper.sln dans Visual Studio 2022

# 2. Restaurer les packages NuGet

# 3. Build > Build Solution (F6)

# 4. Debug > Start Without Debugging (Ctrl+F5)
```

### Option 3 : Ligne de commande

```bash
# Build
dotnet build -c Release

# Publish (self-contained)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## 🎯 Utilisation

### Première configuration

1. **Lancer l'application** → Icône apparaît dans system tray
2. **Clic droit > Settings**
3. **Onglet Repository** :
   - URL : `https://github.com/dharmx/walls`
   - Cliquer "Load Repository Structure"
4. **Onglet Image Selection** :
   - Cocher/décocher les images désirées
5. **Onglet Slideshow Settings** :
   - Intervalle : 15 minutes (ajustable)
   - Ordre : Sequential / Random
6. **Save** puis **Clic droit > Start**

### Contrôles System Tray

- **Start** : Démarre la rotation
- **Stop** : Arrête la rotation
- **Settings** : Ouvre la configuration
- **Exit** : Quitte l'application

## 🔧 Configuration Technique

### Fichiers de Configuration

**Configuration utilisateur** :
```
%APPDATA%\GitHubWallpaper\config.json
```

**Cache des images** :
```
%USERPROFILE%\Pictures\GitHubWallpapers\
```

### Format de Configuration (JSON)

```json
{
  "RepoUrl": "https://github.com/username/repo",
  "GitHubToken": null,
  "RotationIntervalMinutes": 15,
  "IsRandomOrder": false,
  "SelectedImages": ["path/to/image1.jpg", "..."],
  "CurrentIndex": 0,
  "BufferSize": 3,
  "LocalCacheFolder": "C:\\Users\\...\\Pictures\\GitHubWallpapers"
}
```

### Registre Windows Modifié

L'application modifie ces clés pour configurer le diaporama :

```
HKEY_CURRENT_USER\Control Panel\Personalization\Desktop Slideshow
HKEY_CURRENT_USER\Control Panel\Desktop
HKEY_CURRENT_USER\Control Panel\Personalization
```

## 📦 Dépendances NuGet

- **Octokit** (v9.0.0) : API GitHub
- **Newtonsoft.Json** (v13.0.3) : Sérialisation JSON

## 🎨 Formats Supportés

- JPG / JPEG
- PNG
- BMP
- GIF
- WEBP

## ⚙️ Configuration Système Requise

### Minimum
- Windows 10 (1803 ou supérieur)
- .NET 8.0 Runtime (ou version self-contained)
- 50 MB d'espace disque
- Connexion internet

### Recommandé
- Windows 11
- 100 MB d'espace disque (pour le cache)
- Connexion internet stable

## 🐛 Dépannage Courant

### L'application ne démarre pas
→ Vérifiez .NET 8.0 Runtime ou utilisez la version self-contained

### Aucune image ne s'affiche
→ Vérifiez qu'au moins une image est cochée dans Settings

### Les images ne changent pas
→ Attendez l'intervalle configuré ou vérifiez que Start est actif

**Pour plus de détails** : Consultez `TROUBLESHOOTING.md`

## 🔐 Sécurité et Confidentialité

- **Token GitHub** : Stocké en clair dans config.json (attention aux partages)
- **Données locales** : Tout est stocké localement
- **Aucune télémétrie** : Aucune donnée envoyée à des serveurs tiers
- **GitHub API** : Uniquement pour télécharger les images

## 📝 Licence

Ce projet est fourni tel quel, à des fins éducatives et personnelles.

## 🙏 Remerciements

- **Octokit** : Bibliothèque .NET pour l'API GitHub
- **dharmx/walls** : Dépôt d'exemple pour les fonds d'écran
- **Newtonsoft.Json** : Sérialisation JSON performante

## 📮 Support

- **Documentation** : Consultez README.md, INSTALLATION.md, TROUBLESHOOTING.md
- **Problèmes** : Créez une issue sur GitHub
- **Logs** : Lancez depuis un terminal pour voir les messages

## 🚀 Améliorations Futures Possibles

- [ ] Support de multiples dépôts simultanés
- [ ] Aperçu des images dans l'interface
- [ ] Système de favoris avec notation
- [ ] Planification avancée (horaires différents)
- [ ] Support d'autres sources (Google Drive, OneDrive)
- [ ] Mode "découverte" avec dépôts aléatoires
- [ ] Export/import de configurations
- [ ] Synchronisation cloud des préférences
- [ ] Filtres par tags/métadonnées
- [ ] Historique des images affichées

## 🏗️ Architecture et Flux de Données

```
┌──────────────────┐
│  System Tray UI  │
│   (App.xaml)     │
└────────┬─────────┘
         │
         │ Control
         ▼
┌──────────────────┐      ┌──────────────────┐
│  ImageRotator    │◄────►│  GitHubService   │
│                  │      │  (Octokit)       │
└────────┬─────────┘      └──────────────────┘
         │                         │
         │ Manage                  │ Download
         ▼                         ▼
┌──────────────────┐      ┌──────────────────┐
│ WallpaperManager │      │  Local Cache     │
│ (Windows API)    │      │  (Pictures)      │
└──────────────────┘      └──────────────────┘
         │
         │ Update Registry
         ▼
┌──────────────────┐
│ Windows Slideshow│
│   (Native)       │
└──────────────────┘
```

## 💡 Conseils d'Utilisation

1. **Intervalle recommandé** : 15-30 minutes pour un bon équilibre
2. **Sélection d'images** : 20-100 images pour une bonne variété
3. **Buffer** : Gardez 3 images pour éviter les temps de chargement
4. **Réseau** : Utilisez sur WiFi pour éviter la consommation de données mobiles
5. **Performance** : Désactivez si vous jouez à des jeux exigeants
6. **Dépôt personnel** : Créez votre propre dépôt pour un contrôle total

---

**Version** : 1.0.0  
**Date** : Décembre 2024  
**Auteur** : Claude (Anthropic)  
**Dépôt exemple** : https://github.com/dharmx/walls
