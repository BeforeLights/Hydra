using Hydra.API.Services;
using HYDRA.BLL.Services;
using HYDRA.DAL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HYDRA.GUI
{
    public partial class GameEditorWindow : Window
    {
        private readonly ApiClient _api = ApiClient.Default;
        private dynamic? _rawgDetail;

        private readonly GameService _gameService;
        private readonly Game? _editingGame; // null => Add mode

       
        public GameEditorWindow()
        {
            InitializeComponent();
            _gameService = new GameService();

                          
            Title = "Add New Game";

          
            RawgPanel.Visibility = Visibility.Visible;
        }

        
        public GameEditorWindow(Game gameToEdit)
        {
            InitializeComponent();
            _gameService = new GameService();
            Title = "Edit Game";

            _editingGame = gameToEdit;
            PopulateForm();

           
            RawgPanel.Visibility = Visibility.Collapsed;
        }

        private void PopulateForm()
        {
            if (_editingGame is null) return;
            TitleTextBox.Text = _editingGame.Title;
            DescriptionTextBox.Text = _editingGame.Description;
            PriceTextBox.Text = _editingGame.Price.ToString(CultureInfo.InvariantCulture);
            ReleaseDatePicker.SelectedDate = _editingGame.ReleaseDate.HasValue
                ? _editingGame.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue)
                : null;
            CoverArtPathTextBox.Text = _editingGame.CoverArtPath;
            IsForSaleCheckBox.IsChecked = _editingGame.IsForSale;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidatePrice()) return;

            try
            {
                if (_editingGame == null) // Add mode
                {
                    var newGame = new Game();
                    UpdateGameObject(newGame);
                    _gameService.AddGame(newGame);
                }
                else // Edit mode
                {
                    UpdateGameObject(_editingGame);
                    _gameService.UpdateGame(_editingGame);
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving game: {ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidatePrice()
        {
            string priceText = PriceTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(priceText))
            {
                MessageBox.Show("Price cannot be empty. Please enter a valid price.",
                               "Invalid Price", MessageBoxButton.OK, MessageBoxImage.Warning);
                PriceTextBox.Focus();
                return false;
            }

            if (!decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
            {
                MessageBox.Show("Price must be a valid number. (e.g., 19.99)",
                               "Invalid Price", MessageBoxButton.OK, MessageBoxImage.Warning);
                PriceTextBox.Focus();
                PriceTextBox.SelectAll();
                return false;
            }

            if (price < 0)
            {
                MessageBox.Show("Price cannot be negative.",
                               "Invalid Price", MessageBoxButton.OK, MessageBoxImage.Warning);
                PriceTextBox.Focus();
                PriceTextBox.SelectAll();
                return false;
            }

            if (price > 999.99m)
            {
                var result = MessageBox.Show($"The price {price.ToString("C", CultureInfo.CurrentCulture)} seems unusually high. Continue?",
                                           "High Price Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    PriceTextBox.Focus();
                    PriceTextBox.SelectAll();
                    return false;
                }
            }

            return true;
        }

        private void UpdateGameObject(Game game)
        {
            game.Title = TitleTextBox.Text;
            game.Description = DescriptionTextBox.Text;
            game.Price = decimal.TryParse(PriceTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var price)
                ? price : 0;
            game.CoverArtPath = CoverArtPathTextBox.Text;
            game.IsForSale = IsForSaleCheckBox.IsChecked ?? false;

            if (ReleaseDatePicker.SelectedDate.HasValue)
                game.ReleaseDate = DateOnly.FromDateTime(ReleaseDatePicker.SelectedDate.Value);
            else
                game.ReleaseDate = null;
        }

        // ===================== RAWG (CHỈ hoạt động ở Add mode) =====================

        private class RawgSearchItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public int? Year { get; set; }
            public string[] Platforms { get; set; } = Array.Empty<string>();
            public string Display =>
                $"{Name}" +
                (Year != null ? $" ({Year})" : "") +
                (Platforms.Length > 0 ? $" — {string.Join(", ", Platforms.Take(3))}" : "");
        }

        private async void BtnSearchRawg_Click(object sender, RoutedEventArgs e)
        {
            if (_editingGame != null) return; 

            var q = RawgNameBox.Text?.Trim();
            if (string.IsNullOrEmpty(q))
            {
                MessageBox.Show("Nhập tên game để tìm.", "RAWG", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            BtnSearchRawg.IsEnabled = false;
            RawgResults.ItemsSource = null;

            try
            {
                
                var res = await _api.SearchRawgAsync(q, take: 20);

                var list = res.Select(x => new RawgSearchItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Year = x.ReleaseDate?.Year,
                    Platforms = x.Platforms ?? Array.Empty<string>()
                }).ToList();

                if (list.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy game phù hợp.", "RAWG", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (list.Count == 1)
                {
                    await LoadRawgAndFillAsync(list[0].Id);
                    return;
                }

                RawgResults.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm RAWG: " + ex.Message, "RAWG", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                BtnSearchRawg.IsEnabled = true;
            }
        }

        private async void RawgResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_editingGame != null) return;
            if (RawgResults.SelectedItem is RawgSearchItem it)
                await LoadRawgAndFillAsync(it.Id, it.Name);
        }

        private async Task LoadRawgAndFillAsync(int rawgId, string? fallbackName = null)
        {
            RawgDetail? d = null;

            // Thử theo ID trước
            if (rawgId > 0)
            {
                try { d = await _api.GetRawgDetailAsync(rawgId); }
                catch (ApiClient.ApiError ex) when (ex.StatusCode == 404) { d = null; }
            }

            // Fallback theo tên nếu ID không dùng được
            if (d == null && !string.IsNullOrWhiteSpace(fallbackName))
            {
                try { d = await _api.GetRawgDetailByNameAsync(fallbackName); }
                catch { d = null; }
            }

            if (d == null)
            {
                MessageBox.Show("Không lấy được chi tiết từ RAWG cho mục đã chọn.", "RAWG",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ApplyRawgDetailToForm(d);
        }
        private void ApplyRawgDetailToForm(RawgDetail d)
        {
            _rawgDetail = d;

            TitleTextBox.Text = d.Name ?? "";
            DescriptionTextBox.Text = d.Description ?? "";
            CoverArtPathTextBox.Text = d.BackgroundImage ?? "";

            if (d.ReleaseDate != null)
                ReleaseDatePicker.SelectedDate =
                    new DateTime(d.ReleaseDate.Value.Year, d.ReleaseDate.Value.Month, d.ReleaseDate.Value.Day);
            else
                ReleaseDatePicker.SelectedDate = null;

            if (string.IsNullOrWhiteSpace(PriceTextBox.Text))
                PriceTextBox.Text = "0";

            if (IsForSaleCheckBox.IsChecked == null)
                IsForSaleCheckBox.IsChecked = true;

           

            MessageBox.Show("Đã nạp dữ liệu từ RAWG. Kiểm tra lại và bấm Save.", "RAWG",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
