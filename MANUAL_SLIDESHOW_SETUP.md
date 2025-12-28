# Configuration Manuelle du Diaporama Windows

Si le diaporama ne se lance pas automatiquement après avoir cliqué "Start", suivez ces étapes pour le configurer manuellement.

## ⚠️ IMPORTANT : Configuration Manuelle Requise

**Windows nécessite souvent une activation manuelle du diaporama via l'interface utilisateur.**  
L'application configure automatiquement tous les paramètres dans le registre, mais Windows ne détecte pas toujours ces changements immédiatement.

## Méthode Recommandée : Activation Manuelle (2 minutes)

### Étapes :

1. **Clic droit sur le Bureau** → **Personnaliser**

2. Dans **Arrière-plan** :
   - **Type** : Sélectionnez **"Diaporama"** dans la liste déroulante
   
3. Cliquez sur **"Parcourir"**
   - Naviguez vers : `C:\Users\VotreNom\Pictures\GitHubWallpapers`
   - **OU** copiez-collez ce chemin dans la barre d'adresse de l'explorateur :
     ```
     %USERPROFILE%\Pictures\GitHubWallpapers
     ```
   
4. **Modifier l'image toutes les** : 
   - L'intervalle devrait déjà être configuré (ex: 15 minutes)
   - Sinon, sélectionnez votre intervalle préféré
   
5. **Ordre aléatoire** :
   - Devrait déjà être configuré selon vos paramètres
   - Sinon, activez ou désactivez selon votre préférence
   
6. **Ajuster** :
   - Devrait déjà être configuré ("Ajuster" recommandé)
   - Choisissez "Remplir", "Ajuster" ou "Étirer" si nécessaire

7. ✅ **Le diaporama devrait maintenant démarrer automatiquement !**

**Note** : Une fois cette configuration manuelle faite une première fois, l'application pourra modifier les paramètres automatiquement par la suite.

---

## Méthode 2 : Via PowerShell (Automatique)

Si vous préférez utiliser un script :

1. **Ouvrez PowerShell en tant qu'administrateur** :
   - Recherchez "PowerShell"
   - Clic droit > "Exécuter en tant qu'administrateur"

2. **Naviguez vers le dossier du projet** :
   ```powershell
   cd "C:\Path\To\GitHubWallpaper"
   ```

3. **Exécutez le script** :
   ```powershell
   .\ConfigureSlideshow.ps1 -FolderPath "$env:USERPROFILE\Pictures\GitHubWallpapers" -IntervalMinutes 15 -Shuffle $false
   ```

4. **Suivez les instructions affichées** pour finaliser la configuration manuellement

---

## Méthode 3 : Vérification et Dépannage

### Vérifier que les images sont bien téléchargées :

1. Ouvrez l'Explorateur de fichiers
2. Allez dans : `C:\Users\VotreNom\Pictures\GitHubWallpapers`
3. Vous devriez voir 2-3 images (JPG, PNG, etc.)

**Si le dossier est vide** :
- L'application n'a pas encore téléchargé les images
- Vérifiez que le service est démarré (clic droit sur l'icône > "Stop" devrait être actif)
- Attendez quelques secondes pour que les images se téléchargent

### Vérifier la configuration du diaporama :

1. **Paramètres Windows** :
   - Paramètres > Personnalisation > Arrière-plan
   - Vérifiez que "Diaporama" est sélectionné
   - Vérifiez le chemin du dossier

2. **Registre Windows** (Avancé) :
   - Appuyez sur `Win + R`
   - Tapez `regedit` et appuyez sur Entrée
   - Naviguez vers : `HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes`
   - Vérifiez la valeur de `WallpaperPath`

---

## Problèmes Courants

### Le diaporama ne change pas les images

**Solution 1** : Redémarrer Windows
- Parfois Windows nécessite un redémarrage pour appliquer les changements de registre

**Solution 2** : Réinitialiser le diaporama
1. Paramètres > Personnalisation > Arrière-plan
2. Changez temporairement le type vers "Image"
3. Attendez 5 secondes
4. Rechangez vers "Diaporama"
5. Reconfigurer le dossier

**Solution 3** : Vérifier les permissions
- Le dossier `GitHubWallpapers` doit être accessible en lecture
- Vérifiez qu'il n'est pas sur un lecteur réseau ou amovible

### Les images ne se téléchargent pas

1. Vérifiez votre connexion Internet
2. Vérifiez que vous avez ajouté un token GitHub (si nécessaire)
3. Consultez les notifications de l'application (icône system tray)

### Le registre n'est pas modifié

- Exécutez l'application en tant qu'administrateur (temporairement pour diagnostiquer)
- Vérifiez les permissions sur les clés de registre

---

## Configuration Optimale Recommandée

Pour la meilleure expérience :

- **Intervalle** : 15-30 minutes (équilibre entre variété et consommation réseau)
- **Nombre d'images sélectionnées** : 20-100 images
- **Buffer** : 3 images (défaut)
- **Ajustement** : "Remplir" (pour couvrir tout l'écran)
- **Ordre** : Séquentiel pour découvrir toutes les images, Aléatoire pour plus de surprise

---

## Si Rien Ne Fonctionne

**Plan B : Configuration 100% manuelle**

1. Téléchargez manuellement quelques images du dépôt GitHub
2. Placez-les dans un dossier (ex: `C:\Wallpapers`)
3. Configurez le diaporama Windows pour pointer vers ce dossier
4. L'application GitHubWallpaper peut rester arrêtée

**Contacter le support** :
- Créez une issue sur GitHub avec :
  - Version de Windows (Win + R > `winver`)
  - Message d'erreur exact
  - Capture d'écran des paramètres

---

## Astuce : Démarrage Automatique

Pour que l'application démarre automatiquement au login :

1. Appuyez sur `Win + R`
2. Tapez `shell:startup` et appuyez sur Entrée
3. Créez un raccourci de `GitHubWallpaper.exe` dans ce dossier
4. L'application démarrera à chaque connexion Windows
