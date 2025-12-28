# Notes de Version

## Version 1.0.0 (Initiale)

### Fonctionnalités
- ✅ Application System Tray pour Windows
- ✅ Synchronisation avec dépôts GitHub (publics et privés)
- ✅ Interface graphique avec TreeView pour sélection d'images
- ✅ Configuration du diaporama Windows natif
- ✅ Rotation séquentielle ou aléatoire
- ✅ Gestion intelligente du cache (buffer de 2-3 images)
- ✅ Intervalle de rotation configurable (5 min - 24h)
- ✅ Sauvegarde automatique de la configuration
- ✅ Notifications system tray pour les événements importants

### Caractéristiques techniques
- Consommation mémoire : ~20-35 MB en idle
- Formats supportés : JPG, JPEG, PNG, BMP, GIF, WEBP
- Utilise l'API GitHub (Octokit) pour la récupération des images
- Configuration persistante en JSON
- Build self-contained disponible (aucune dépendance .NET requise)

### Dépôt pré-configuré
- https://github.com/dharmx/walls

### Configuration requise
- Windows 10 ou supérieur
- .NET 8.0 Runtime (ou version self-contained)

### Fichiers de configuration
- Configuration : `%APPDATA%\GitHubWallpaper\config.json`
- Cache images : `%USERPROFILE%\Pictures\GitHubWallpapers\`

### Limitations connues
- Pas de support pour les dépôts Git LFS (Large File Storage)
- Les transitions sont gérées par Windows (pas de contrôle direct)
- Maximum recommandé : ~1000 images dans le dépôt

### Améliorations futures possibles
- Support de multiples dépôts
- Planification avancée (différents intervalles selon l'heure)
- Filtres par tags/catégories
- Aperçu des images dans l'interface
- Mode "favorites" avec notation d'images
- Export/import de configurations
- Support pour d'autres sources (Google Drive, Dropbox, etc.)
