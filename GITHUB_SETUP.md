# 📦 Guide de Publication sur GitHub

## Étapes pour publier ce projet sur GitHub

### 1. Créer le dépôt GitHub

1. Allez sur [GitHub](https://github.com)
2. Cliquez sur **"New repository"**
3. Configurez :
   - **Nom** : `github-wallpaper` (ou autre nom)
   - **Description** : `Automatic wallpaper rotation from GitHub repositories for Windows`
   - **Visibilité** : Public ou Private
   - **NE PAS** initialiser avec README, .gitignore, ou license (on les a déjà)
4. Cliquez **"Create repository"**

### 2. Initialiser Git localement

```bash
# Dans le dossier GitHubWallpaper
cd GitHubWallpaper

# Initialiser Git
git init

# Ajouter tous les fichiers
git add .

# Premier commit
git commit -m "Initial commit: GitHub Wallpaper v1.0.0"

# Ajouter le remote (remplacez YOUR_USERNAME)
git remote add origin https://github.com/YOUR_USERNAME/github-wallpaper.git

# Pousser vers GitHub
git branch -M main
git push -u origin main
```

### 3. Configurer GitHub Actions

Les workflows sont déjà configurés dans `.github/workflows/build.yml`.

Pour activer :
1. GitHub détecte automatiquement le workflow
2. À chaque push, le build se lancera automatiquement
3. Vérifiez dans l'onglet **Actions**

### 4. Créer la première release

```bash
# Tag de version
git tag -a v1.0.0 -m "Release v1.0.0"
git push origin v1.0.0
```

GitHub Actions va automatiquement :
- Builder le projet
- Créer une release
- Attacher `GitHubWallpaper.exe`

Ou créez manuellement :
1. Allez dans **Releases** → **"Create a new release"**
2. Tag : `v1.0.0`
3. Title : `v1.0.0 - Initial Release`
4. Description : Utilisez le template de CHANGELOG.md
5. Uploadez `GitHubWallpaper.exe` compilé
6. Cliquez **"Publish release"**

### 5. Configurer GitHub Pages (optionnel)

Pour héberger la documentation :

1. Settings → Pages
2. Source : Deploy from branch
3. Branch : `main` / `docs` (si vous créez un dossier docs)
4. Save

### 6. Ajouter des Topics

Dans la page principale du repo :
1. Cliquez sur ⚙️ à côté de "About"
2. Ajoutez des topics :
   - `windows`
   - `wallpaper`
   - `github`
   - `csharp`
   - `wpf`
   - `dotnet`
   - `slideshow`
   - `system-tray`

### 7. Activer les fonctionnalités

Settings → General → Features :
- ✅ Issues
- ✅ Discussions (pour le support communautaire)
- ✅ Projects (si vous voulez un roadmap)
- ✅ Wiki (optionnel)

### 8. Configurer la branche par défaut

Settings → Branches :
- Default branch : `main`
- Branch protection rules (optionnel) :
  - Require pull request reviews
  - Require status checks to pass

### 9. Ajouter des collaborateurs (optionnel)

Settings → Collaborators :
- Invitez des contributeurs si besoin

### 10. Promouvoir votre projet

- Partagez sur Reddit : r/opensource, r/Windows10, r/software
- Postez sur Twitter/X avec #GitHub #Windows
- Ajoutez sur awesome lists pertinentes
- Créez un post sur dev.to ou Medium

## Structure finale sur GitHub

```
github-wallpaper/
├── .github/
│   ├── workflows/
│   │   └── build.yml
│   └── ISSUE_TEMPLATE/
│       ├── bug_report.md
│       └── feature_request.md
├── Core/
├── UI/
├── Models/
├── .gitignore
├── .gitattributes
├── LICENSE
├── README.md
├── CONTRIBUTING.md
├── SECURITY.md
├── CHANGELOG.md
├── TROUBLESHOOTING.md
└── ...
```

## Commandes Git utiles

```bash
# Vérifier le statut
git status

# Ajouter des changements
git add .
git commit -m "Add: nouvelle fonctionnalité"
git push

# Créer une branche
git checkout -b feature/ma-feature

# Fusionner une branche
git checkout main
git merge feature/ma-feature

# Voir l'historique
git log --oneline

# Annuler le dernier commit (garde les changements)
git reset --soft HEAD~1

# Mettre à jour depuis GitHub
git pull origin main
```

## Badges pour README

Ajoutez ces badges en haut du README.md :

```markdown
![Build Status](https://github.com/YOUR_USERNAME/github-wallpaper/workflows/Build%20and%20Release/badge.svg)
![Downloads](https://img.shields.io/github/downloads/YOUR_USERNAME/github-wallpaper/total)
![Release](https://img.shields.io/github/v/release/YOUR_USERNAME/github-wallpaper)
![License](https://img.shields.io/github/license/YOUR_USERNAME/github-wallpaper)
![Stars](https://img.shields.io/github/stars/YOUR_USERNAME/github-wallpaper)
```

## Checklist finale

- [ ] Dépôt créé sur GitHub
- [ ] Code poussé
- [ ] README.md bien formaté
- [ ] LICENSE présent
- [ ] .gitignore configuré
- [ ] GitHub Actions fonctionnel
- [ ] Première release publiée
- [ ] Topics ajoutés
- [ ] Issues activées
- [ ] CONTRIBUTING.md présent
- [ ] SECURITY.md présent

## 🎉 Votre projet est prêt !

Votre projet est maintenant public et professionnel. Bonne chance ! 🚀
