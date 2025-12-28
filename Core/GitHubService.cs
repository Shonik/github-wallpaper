using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Octokit;
using GitHubWallpaper.Models;

namespace GitHubWallpaper.Core
{
    public class GitHubService
    {
        private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp" };
        private readonly GitHubClient _client;
        private readonly HttpClient _httpClient;

        public GitHubService(string? token = null)
        {
            _client = new GitHubClient(new ProductHeaderValue("GitHubWallpaper"));
            if (!string.IsNullOrEmpty(token))
            {
                _client.Credentials = new Credentials(token);
            }
            _httpClient = new HttpClient();
        }

        public async Task<ImageNode> LoadRepositoryStructure(string repoUrl)
        {
            var (owner, repo) = ParseRepoUrl(repoUrl);
            var rootNode = new ImageNode
            {
                Name = repo,
                Path = "",
                IsDirectory = true
            };

            try
            {
                var contents = await _client.Repository.Content.GetAllContents(owner, repo);
                await BuildTreeRecursive(owner, repo, "", rootNode, contents);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load repository: {ex.Message}", ex);
            }

            return rootNode;
        }

        private async Task BuildTreeRecursive(string owner, string repo, string path, ImageNode parentNode, IReadOnlyList<RepositoryContent> contents)
        {
            // Sort: directories first, then by name
            var sortedContents = contents
                .OrderBy(c => c.Type == ContentType.Dir ? 0 : 1)
                .ThenBy(c => c.Name ?? string.Empty)
                .ToList();

            foreach (var item in sortedContents)
            {
                if (item.Type == ContentType.Dir)
                {
                    var dirNode = new ImageNode
                    {
                        Name = item.Name,
                        Path = item.Path,
                        IsDirectory = true
                    };
                    parentNode.Children.Add(dirNode);

                    try
                    {
                        var subContents = await _client.Repository.Content.GetAllContents(owner, repo, item.Path);
                        await BuildTreeRecursive(owner, repo, item.Path, dirNode, subContents);
                    }
                    catch { }
                }
                else if (item.Type == ContentType.File && IsImageFile(item.Name))
                {
                    var fileNode = new ImageNode
                    {
                        Name = item.Name,
                        Path = item.Path,
                        IsDirectory = false
                    };
                    parentNode.Children.Add(fileNode);
                }
            }
        }

        public async Task<string> DownloadImage(string repoUrl, string imagePath, string localFolder)
        {
            var (owner, repo) = ParseRepoUrl(repoUrl);
            
            try
            {
                var content = await _client.Repository.Content.GetAllContents(owner, repo, imagePath);
                if (content.Count > 0)
                {
                    var fileContent = content[0];
                    var localPath = Path.Combine(localFolder, Path.GetFileName(imagePath));

                    // Download using download URL
                    var imageData = await _httpClient.GetByteArrayAsync(fileContent.DownloadUrl);
                    
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath)!);
                    await File.WriteAllBytesAsync(localPath, imageData);
                    
                    return localPath;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to download image {imagePath}: {ex.Message}", ex);
            }

            throw new Exception($"Image not found: {imagePath}");
        }

        public List<string> GetAllImagePaths(ImageNode root, List<string> selectedPaths)
        {
            var allImages = new List<string>();
            CollectImagePaths(root, allImages);
            
            // Filter only selected images
            return allImages.Where(img => selectedPaths.Contains(img)).ToList();
        }

        private void CollectImagePaths(ImageNode node, List<string> images)
        {
            if (!node.IsDirectory)
            {
                images.Add(node.Path);
                return;
            }

            foreach (var child in node.Children)
            {
                CollectImagePaths(child, images);
            }
        }

        private (string owner, string repo) ParseRepoUrl(string url)
        {
            // Parse URLs like: https://github.com/dharmx/walls or https://github.com/dharmx/walls.git
            var uri = new Uri(url.TrimEnd('/').Replace(".git", ""));
            var parts = uri.AbsolutePath.Trim('/').Split('/');
            
            if (parts.Length < 2)
                throw new ArgumentException("Invalid GitHub repository URL");
            
            return (parts[0], parts[1]);
        }

        private bool IsImageFile(string filename)
        {
            var extension = Path.GetExtension(filename).ToLowerInvariant();
            return ImageExtensions.Contains(extension);
        }
    }
}
