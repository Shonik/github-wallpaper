# Installation Rapide - GitHub Wallpaper

## Option 1 : Télécharger l'exécutable pré-compilé (Recommandé)

1. Téléchargez `GitHubWallpaper.exe` depuis les releases
2. Double-cliquez pour lancer
3. C'est tout ! L'application est autonome (self-contained)

## Option 2 : Compiler depuis les sources

### Prérequis
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10 ou supérieur

### Compilation

**Méthode simple (avec script)**
1. Ouvrez un terminal dans le dossier du projet
2. Exécutez `build.bat`
3. L'exécutable sera dans `bin\Release\net8.0-windows\win-x64\publish\`

**Méthode manuelle**
```bash
# Build self-contained (aucune dépendance requise)
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true

# OU Build avec .NET Runtime requis (fichier plus petit)
dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

## Première utilisation

1. **Lancez l'application** (double-clic sur .exe)
2. Une **icône apparaît dans le system tray** (barre des tâches)
3. **Clic droit > Settings** pour ouvrir la configuration
4. Configurez votre dépôt GitHub et vos préférences
5. Cliquez **Save** puis **Clic droit > Start**

## Configuration recommandée pour le dépôt dharmx/walls

```
Repository URL: https://github.com/dharmx/walls
GitHub Token: (laisser vide - dépôt public)
Rotation Interval: 15 minutes
Order: Sequential ou Random selon préférence
```

## Démarrage automatique au login Windows (optionnel)

1. Appuyez sur `Win + R`
2. Tapez `shell:startup` et appuyez sur Entrée
3. Créez un raccourci de `GitHubWallpaper.exe` dans ce dossier
4. L'application démarrera automatiquement à chaque connexion

## Notes importantes

- **Première synchronisation** : Le téléchargement initial des images peut prendre quelques minutes selon votre connexion
- **Consommation** : ~20-35 MB de RAM en fonctionnement normal
- **Cache** : Les images sont stockées dans `%USERPROFILE%\Pictures\GitHubWallpapers\`
- **Configuration** : Sauvegardée dans `%APPDATA%\GitHubWallpaper\config.json`

## Dépannage rapide

**L'icône du system tray n'apparaît pas**
- Vérifiez les paramètres Windows : Paramètres > Personnalisation > Barre des tâches > Sélectionner les icônes à afficher

**Erreur ".NET Runtime not found"**
- Téléchargez la version self-contained OU installez [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0/runtime)

**Les images ne changent pas**
- Vérifiez que le service est démarré (clic droit > devrait afficher "Stop" en gras)
- Patientez le délai configuré (par défaut 15 minutes)

## Support

Pour toute question ou problème :
1. Vérifiez le README.md pour plus de détails
2. Consultez les logs dans la console (si lancé depuis terminal)
3. Créez une issue sur GitHub
