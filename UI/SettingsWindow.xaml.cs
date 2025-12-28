using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using GitHubWallpaper.Core;
using GitHubWallpaper.Models;

namespace GitHubWallpaper.UI
{
    public partial class SettingsWindow : Window
    {
        private AppConfig _config;
        private ImageNode? _rootNode;

        public AppConfig Config => _config;
        public bool WasSaved { get; private set; }

        public SettingsWindow(AppConfig config)
        {
            InitializeComponent();
            _config = config;
            LoadSettings();
        }

        private void LoadSettings()
        {
            RepoUrlTextBox.Text = _config.RepoUrl;
            
            // Load token if exists (but we can't display it in PasswordBox for security)
            // User will need to re-enter if they want to change it
            if (!string.IsNullOrEmpty(_config.GitHubToken))
            {
                TokenPasswordBox.Password = _config.GitHubToken;
            }
            
            IntervalSlider.Value = _config.RotationIntervalMinutes;
            CacheFolderTextBox.Text = _config.LocalCacheFolder;
            
            if (_config.IsRandomOrder)
                RandomRadio.IsChecked = true;
            else
                SequentialRadio.IsChecked = true;

            // Set FitMode
            SetFitModeComboBox(_config.FitMode);

            UpdateIntervalDisplay();
            
            // Auto-load repository structure if we have a URL
            if (!string.IsNullOrWhiteSpace(_config.RepoUrl) && _config.SelectedImages.Count > 0)
            {
                // Load in background
                Dispatcher.BeginInvoke(new Action(async () => 
                {
                    await LoadRepositoryInBackground();
                }));
            }
        }

        private void SetFitModeComboBox(WallpaperFit fitMode)
        {
            var tagToFind = fitMode.ToString();
            for (int i = 0; i < FitModeComboBox.Items.Count; i++)
            {
                if (FitModeComboBox.Items[i] is ComboBoxItem item && item.Tag?.ToString() == tagToFind)
                {
                    FitModeComboBox.SelectedIndex = i;
                    return;
                }
            }
            FitModeComboBox.SelectedIndex = 1; // Default to Fit
        }

        private WallpaperFit GetSelectedFitMode()
        {
            if (FitModeComboBox.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                if (Enum.TryParse<WallpaperFit>(item.Tag.ToString(), out var fitMode))
                {
                    return fitMode;
                }
            }
            return WallpaperFit.Fit; // Default
        }

        private async System.Threading.Tasks.Task LoadRepositoryInBackground()
        {
            try
            {
                LoadRepoButton.IsEnabled = false;
                StatusTextBlock.Text = "Loading saved configuration...";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.DarkBlue;

                var token = string.IsNullOrEmpty(_config.GitHubToken) ? null : _config.GitHubToken;
                var github = new GitHubService(token);
                
                _rootNode = await github.LoadRepositoryStructure(_config.RepoUrl);

                // Restore previous selections
                RestoreSelections(_rootNode, _config.SelectedImages);

                // Set ItemTemplate and bind data
                ImageTreeView.ItemTemplate = (HierarchicalDataTemplate)ImageTreeView.Resources["TreeViewItemTemplate"];
                ImageTreeView.ItemsSource = new[] { _rootNode };
                
                // Force refresh to show correct checkbox states
                ImageTreeView.Items.Refresh();
                
                UpdateSelectionCount();

                StatusTextBlock.Text = "Configuration loaded successfully!";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.DarkGreen;
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Could not load repository. You can reload it manually. Error: {ex.Message}";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Orange;
            }
            finally
            {
                LoadRepoButton.IsEnabled = true;
            }
        }

        private async void LoadRepoButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RepoUrlTextBox.Text))
            {
                MessageBox.Show("Please enter a GitHub repository URL.", "Missing URL", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoadRepoButton.IsEnabled = false;
            StatusTextBlock.Text = "Loading repository structure...";
            StatusTextBlock.Foreground = System.Windows.Media.Brushes.DarkBlue;

            try
            {
                var token = string.IsNullOrEmpty(TokenPasswordBox.Password) ? null : TokenPasswordBox.Password;
                var github = new GitHubService(token);
                
                _rootNode = await github.LoadRepositoryStructure(RepoUrlTextBox.Text);

                // Restore previous selections
                RestoreSelections(_rootNode, _config.SelectedImages);

                // Set ItemTemplate and bind data
                ImageTreeView.ItemTemplate = (HierarchicalDataTemplate)ImageTreeView.Resources["TreeViewItemTemplate"];
                ImageTreeView.ItemsSource = new[] { _rootNode };
                
                // Force refresh to show correct checkbox states
                ImageTreeView.Items.Refresh();
                
                UpdateSelectionCount();

                StatusTextBlock.Text = "Repository loaded successfully!";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.DarkGreen;
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error: {ex.Message}";
                StatusTextBlock.Foreground = System.Windows.Media.Brushes.Red;
                MessageBox.Show($"Failed to load repository:\n{ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadRepoButton.IsEnabled = true;
            }
        }

        private void RestoreSelections(ImageNode node, System.Collections.Generic.List<string> selectedPaths)
        {
            if (!node.IsDirectory)
            {
                // For images: set selection based on whether path is in selected list
                node.IsSelected = selectedPaths.Contains(node.Path);
            }
            else
            {
                // For directories: first restore children, then update directory state
                foreach (var child in node.Children)
                {
                    RestoreSelections(child, selectedPaths);
                }
                
                // Set directory as selected only if ALL children are selected
                var hasChildren = node.Children.Count > 0;
                if (hasChildren)
                {
                    node.IsSelected = node.Children.All(c => c.IsSelected);
                }
            }
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (_rootNode != null)
            {
                SetAllSelections(_rootNode, true);
                ImageTreeView.Items.Refresh();
                UpdateSelectionCount();
            }
        }

        private void DeselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (_rootNode != null)
            {
                SetAllSelections(_rootNode, false);
                ImageTreeView.Items.Refresh();
                UpdateSelectionCount();
            }
        }

        private void SetAllSelections(ImageNode node, bool isSelected)
        {
            node.IsSelected = isSelected;
            foreach (var child in node.Children)
            {
                SetAllSelections(child, isSelected);
            }
        }

        private void CheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.DataContext is ImageNode node)
            {
                // If it's a directory, update all children
                if (node.IsDirectory)
                {
                    SetAllSelections(node, node.IsSelected);
                }
                
                UpdateSelectionCount();
            }
        }

        private void UpdateSelectionCount()
        {
            if (_rootNode != null)
            {
                var count = CountSelectedImages(_rootNode);
                SelectionCountTextBlock.Text = $"{count} image(s) selected";
            }
        }

        private int CountSelectedImages(ImageNode node)
        {
            int count = 0;
            if (!node.IsDirectory && node.IsSelected)
            {
                count = 1;
            }

            foreach (var child in node.Children)
            {
                count += CountSelectedImages(child);
            }

            return count;
        }

        private void IntervalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateIntervalDisplay();
        }

        private void UpdateIntervalDisplay()
        {
            if (IntervalValueTextBlock != null)
            {
                var minutes = (int)IntervalSlider.Value;
                if (minutes < 60)
                {
                    IntervalValueTextBlock.Text = $"{minutes} min";
                }
                else
                {
                    var hours = minutes / 60;
                    var remainingMinutes = minutes % 60;
                    IntervalValueTextBlock.Text = remainingMinutes > 0 
                        ? $"{hours}h {remainingMinutes}min" 
                        : $"{hours}h";
                }
            }
        }

        private void BrowseCacheFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select folder for wallpaper cache",
                SelectedPath = CacheFolderTextBox.Text
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CacheFolderTextBox.Text = dialog.SelectedPath;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RepoUrlTextBox.Text))
            {
                MessageBox.Show("Please enter a repository URL.", "Missing Information", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CacheFolderTextBox.Text))
            {
                MessageBox.Show("Please select a cache folder.", "Missing Information", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Save settings
            _config.RepoUrl = RepoUrlTextBox.Text;
            
            // Only update token if user entered something (don't overwrite with empty)
            if (!string.IsNullOrEmpty(TokenPasswordBox.Password))
            {
                _config.GitHubToken = TokenPasswordBox.Password;
            }
            
            _config.RotationIntervalMinutes = (int)IntervalSlider.Value;
            _config.LocalCacheFolder = CacheFolderTextBox.Text;
            _config.IsRandomOrder = RandomRadio.IsChecked == true;
            _config.FitMode = GetSelectedFitMode();

            // Save selected images
            if (_rootNode != null)
            {
                _config.SelectedImages.Clear();
                CollectSelectedImages(_rootNode, _config.SelectedImages);
                
                if (_config.SelectedImages.Count == 0)
                {
                    var result = MessageBox.Show(
                        "No images are selected. The slideshow won't work without images. Continue anyway?",
                        "No Images Selected",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);
                    
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }

            WasSaved = true;
            ConfigManager.Save(_config);

            MessageBox.Show("Settings saved successfully!", "Success", 
                MessageBoxButton.OK, MessageBoxImage.Information);
            
            Close();
        }

        private void CollectSelectedImages(ImageNode node, System.Collections.Generic.List<string> selectedImages)
        {
            if (!node.IsDirectory && node.IsSelected)
            {
                selectedImages.Add(node.Path);
            }

            foreach (var child in node.Children)
            {
                CollectSelectedImages(child, selectedImages);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WasSaved = false;
            Close();
        }
    }
}
