# Guide de Dépannage - GitHub Wallpaper

## Problèmes de démarrage

### L'application ne se lance pas du tout

**Symptôme** : Double-clic sur .exe ne fait rien, aucune icône n'apparaît

**Solutions** :
1. Vérifiez que vous utilisez Windows 10 ou supérieur
2. Si vous utilisez la version nécessitant .NET :
   - Téléchargez [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0/runtime)
   - Installez le package "Desktop Runtime" (pas SDK)
3. Essayez de lancer depuis un terminal pour voir les erreurs :
   ```cmd
   GitHubWallpaper.exe
   ```
4. Vérifiez l'antivirus - ajoutez une exception si nécessaire

### L'icône du system tray n'apparaît pas

**Solutions** :
1. Vérifiez les paramètres Windows :
   - Paramètres > Personnalisation > Barre des tâches
   - Cliquez sur "Sélectionner les icônes à afficher dans la barre des tâches"
   - Activez "GitHubWallpaper"
2. Cliquez sur la flèche ^ dans la barre des tâches pour voir les icônes cachées
3. Redémarrez l'application

## Problèmes de configuration

### Impossible de charger le dépôt

**Symptôme** : "Failed to load repository" ou timeout

**Solutions** :
1. Vérifiez l'URL du dépôt :
   - Format correct : `https://github.com/username/repo`
   - Sans `.git` à la fin
2. Pour les dépôts privés :
   - Créez un Personal Access Token sur GitHub
   - Settings > Developer settings > Personal access tokens > Tokens (classic)
   - Scope requis : `repo` (Full control of private repositories)
   - Collez le token dans le champ approprié
3. Vérifiez votre connexion internet
4. Si derrière un proxy d'entreprise, configurez les paramètres proxy Windows

### L'arborescence des images est vide

**Solutions** :
1. Vérifiez que le dépôt contient bien des images (JPG, PNG, BMP, GIF, WEBP)
2. Les images doivent être dans des fichiers (pas dans des releases)
3. Rechargez la structure après avoir corrigé l'URL

### Impossible de sauvegarder les paramètres

**Solutions** :
1. Vérifiez les permissions :
   - L'application doit pouvoir écrire dans `%APPDATA%`
   - Vérifiez que le dossier n'est pas en lecture seule
2. Exécutez l'application en tant qu'administrateur (temporairement pour diagnostiquer)
3. Vérifiez l'espace disque disponible

## Problèmes de rotation

### Les images ne changent pas

**Symptôme** : Le fond d'écran reste fixe malgré le service actif

**Solutions** :
1. Vérifiez que le service est bien démarré :
   - Clic droit sur l'icône > devrait afficher "Stop" (pas "Start")
2. Vérifiez le diaporama Windows :
   - Paramètres > Personnalisation > Arrière-plan
   - Devrait afficher "Diaporama" comme type
3. Attendez le délai configuré (par défaut 15 minutes)
4. Redémarrez le service :
   - Clic droit > Stop
   - Clic droit > Start
5. Vérifiez le dossier cache :
   - `%USERPROFILE%\Pictures\GitHubWallpapers\`
   - Devrait contenir 2-3 images

### Les images sont téléchargées mais pas affichées

**Solutions** :
1. Vérifiez le format des images (Windows supporte JPG, PNG, BMP nativement)
2. Vérifiez que le chemin du dossier cache ne contient pas de caractères spéciaux
3. Définissez manuellement le fond d'écran sur une image du cache pour tester
4. Redémarrez Windows (le registre peut nécessiter un redémarrage)

### Le téléchargement des images échoue

**Symptôme** : Messages d'erreur dans les notifications

**Solutions** :
1. Vérifiez votre connexion internet
2. Vérifiez que le token GitHub est toujours valide (s'il est utilisé)
3. Le dépôt peut avoir changé de structure - rechargez l'arborescence
4. Vérifiez l'espace disque disponible dans le dossier cache
5. Pour les gros fichiers (>10 MB), le téléchargement peut prendre du temps

## Problèmes de performance

### Consommation mémoire élevée

**Symptôme** : Plus de 100 MB de RAM utilisée

**Solutions** :
1. Vérifiez le nombre d'images dans le cache :
   - Ne devrait pas dépasser 3-5 images
2. Réduisez le BufferSize dans config.json :
   ```json
   "BufferSize": 2
   ```
3. Supprimez manuellement les anciennes images du cache
4. Redémarrez l'application

### L'application ralentit Windows

**Solutions** :
1. Augmentez l'intervalle de rotation (ex: 60 minutes au lieu de 15)
2. Réduisez le nombre d'images sélectionnées
3. Vérifiez que vous n'avez pas d'autres applications de fond d'écran actives
4. Désactivez temporairement pour diagnostiquer

## Problèmes de fichiers

### Les fichiers de configuration sont corrompus

**Solutions** :
1. Supprimez `%APPDATA%\GitHubWallpaper\config.json`
2. Relancez l'application (créera une config par défaut)
3. Reconfigurez manuellement

### Le cache prend trop de place

**Solutions** :
1. Le cache ne devrait contenir que 2-3 images (quelques Mo)
2. Si le dossier est volumineux :
   - Arrêtez le service
   - Supprimez tout le contenu de `%USERPROFILE%\Pictures\GitHubWallpapers\`
   - Redémarrez le service
3. Changez l'emplacement du cache vers un disque avec plus d'espace

## Messages d'erreur courants

### "Failed to download image"
- Vérifiez la connexion internet
- L'image peut avoir été supprimée du dépôt
- Rechargez la structure du dépôt

### "No images selected"
- Ouvrez Settings > Image Selection
- Cochez au moins une image
- Sauvegardez

### "Repository not found"
- Vérifiez l'URL
- Pour les dépôts privés, ajoutez un token
- Le dépôt peut avoir été supprimé ou renommé

### "Access denied"
- Token GitHub invalide ou expiré
- Permissions insuffisantes sur le dépôt
- Créez un nouveau token

## Réinitialisation complète

Si rien ne fonctionne, réinitialisez complètement :

1. Arrêtez l'application
2. Supprimez :
   - `%APPDATA%\GitHubWallpaper\` (configuration)
   - `%USERPROFILE%\Pictures\GitHubWallpapers\` (cache)
3. Relancez l'application
4. Reconfigurez depuis zéro

## Logs et diagnostics

### Activer les logs détaillés

Lancez l'application depuis un terminal pour voir les logs :
```cmd
cd C:\Path\To\Application
GitHubWallpaper.exe > logs.txt 2>&1
```

Les messages de statut et d'erreur s'afficheront dans le terminal.

## Contacter le support

Si le problème persiste :
1. Notez le message d'erreur exact
2. Vérifiez la version de Windows (Win + R > `winver`)
3. Vérifiez la version .NET installée (si applicable)
4. Créez une issue sur GitHub avec :
   - Description du problème
   - Étapes pour reproduire
   - Messages d'erreur
   - Configuration système
