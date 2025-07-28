using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HYDRA.BLL.Services;
using HYDRA.DAL.Models;

namespace HYDRA.GUI
{
    public partial class GameDetailView : UserControl
    {
        private readonly GameService _gameService;
        private readonly CartService _cartService;
        private readonly LibraryService _libraryService;
        private readonly User _currentUser;
        private readonly Game _currentGame;

        public GameDetailView(int gameId, User currentUser)
        {
            InitializeComponent();
            _gameService = new GameService();
            _cartService = new CartService();
            _libraryService = new LibraryService();
            _currentUser = currentUser;

            _currentGame = _gameService.GetGameById(gameId);
            
            // Set the DataContext to the current game for data binding
            this.DataContext = _currentGame;
            
            LoadGameDetails();
        }

        private void LoadGameDetails()
        {
            if (_currentGame == null) return;

            // Load basic game information
            TitleTextBlock.Text = _currentGame.Title;
            DescriptionTextBlock.Text = _currentGame.Description ?? "No description available.";
            PriceTextBlock.Text = _currentGame.Price.ToString("C");
            ReleaseDateTextBlock.Text = _currentGame.ReleaseDate.HasValue ? _currentGame.ReleaseDate.Value.ToShortDateString() : "N/A";
            PublisherTextBlock.Text = _currentGame.Publisher?.Name ?? "N/A";
            
            // Load cover art with improved error handling
            LoadCoverArt();

            // Load screenshots if available
            LoadScreenshots();

            // Check if the game is in the user's library
            bool userOwnsGame = _libraryService.IsGameInLibrary(_currentUser.UserId, _currentGame.GameId);

            if (userOwnsGame)
            {
                AddToCartButton.Content = "🎮 Play Game";
                AddToCartButton.IsEnabled = true;
                AddToCartButton.Style = (Style)FindResource("ModernButton");
            }
            else
            {
                AddToCartButton.Content = "🛒 Add to Cart";
                AddToCartButton.IsEnabled = true;
                AddToCartButton.Style = (Style)FindResource("ModernButton");
            }
        }

        private void LoadCoverArt()
        {
            // Debug output to see what we're working with
            System.Diagnostics.Debug.WriteLine($"Loading cover art for {_currentGame.Title}");
            System.Diagnostics.Debug.WriteLine($"CoverArtPath: '{_currentGame.CoverArtPath ?? "NULL"}'");
            
            if (string.IsNullOrEmpty(_currentGame.CoverArtPath))
            {
                // No cover art path available - show fallback
                System.Diagnostics.Debug.WriteLine("No cover art path available, showing fallback");
                CoverArtImage.Source = null;
                return;
            }

            try
            {
                // Create bitmap image with better error handling
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                
                // Set cache option to load the image into memory
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                
                // Handle both local and web URLs
                if (_currentGame.CoverArtPath.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    System.Diagnostics.Debug.WriteLine($"Loading web image: {_currentGame.CoverArtPath}");
                    bitmap.UriSource = new Uri(_currentGame.CoverArtPath, UriKind.Absolute);
                }
                else
                {
                    // For local paths, try to construct the full path
                    string fullPath = _currentGame.CoverArtPath;
                    if (!System.IO.Path.IsPathRooted(fullPath))
                    {
                        // If it's a relative path, make it relative to the application
                        fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fullPath);
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Loading local image: {fullPath}");
                    
                    // Check if file exists
                    if (System.IO.File.Exists(fullPath))
                    {
                        bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Image file not found: {fullPath}");
                        CoverArtImage.Source = null;
                        return;
                    }
                }
                
                // Add error handling for download failure
                bitmap.DownloadFailed += (s, e) => 
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to download image: {e.ErrorException?.Message}");
                    Dispatcher.Invoke(() => CoverArtImage.Source = null);
                };
                
                bitmap.EndInit();
                CoverArtImage.Source = bitmap;
                System.Diagnostics.Debug.WriteLine("Image loaded successfully");
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                System.Diagnostics.Debug.WriteLine($"Failed to load cover art for {_currentGame.Title}: {ex.Message}");
                CoverArtImage.Source = null;
            }
        }

        private void LoadScreenshots()
        {
            // Use the ScreenshotsItemsControl from XAML
            if (_currentGame.GameImages == null || !_currentGame.GameImages.Any())
            {
                // No screenshots available - set the ItemsSource to null or empty list
                ScreenshotsItemsControl.ItemsSource = null;
                return;
            }

            // Set the ItemsSource to the game images for data binding
            ScreenshotsItemsControl.ItemsSource = _currentGame.GameImages.Take(5); // Limit to 5 screenshots
        }

        private void ShowFullSizeImage(string imagePath)
        {
            try
            {
                // Create a simple window to show the full-size image
                var imageWindow = new Window
                {
                    Title = "Screenshot",
                    SizeToContent = SizeToContent.WidthAndHeight,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Background = new SolidColorBrush(Colors.Black)
                };

                var fullImage = new Image
                {
                    MaxWidth = 800,
                    MaxHeight = 600,
                    Stretch = Stretch.Uniform
                };

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();
                
                fullImage.Source = bitmap;
                imageWindow.Content = fullImage;
                imageWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load full-size image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            bool userOwnsGame = _libraryService.IsGameInLibrary(_currentUser.UserId, _currentGame.GameId);

            if (userOwnsGame)
            {
                MessageBox.Show("This is a prototype, ain't no keys for you :)", 
                               "Game Launch", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Information);
            }
            else
            {
                _cartService.AddGameToCart(_currentUser.UserId, _currentGame.GameId);
                MessageBox.Show($"{_currentGame.Title} has been added to your cart!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}