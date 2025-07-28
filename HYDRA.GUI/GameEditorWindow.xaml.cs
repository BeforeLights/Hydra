using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using HYDRA.BLL.Services;
using HYDRA.DAL.Models;

namespace HYDRA.GUI
{
    public partial class GameEditorWindow : Window
    {
        private readonly GameService _gameService;
        private readonly Game _editingGame; // Will be null if we are adding a new game

        // --- CONSTRUCTOR FOR ADDING ---
        public GameEditorWindow()
        {
            InitializeComponent();
            _gameService = new GameService();
            Title = "Add New Game"; // Set window title for adding
        }

        // --- NEW CONSTRUCTOR FOR EDITING ---
        public GameEditorWindow(Game gameToEdit)
        {
            InitializeComponent();
            _gameService = new GameService();
            Title = "Edit Game"; // Set window title for editing

            _editingGame = gameToEdit;
            PopulateForm();
        }

        private void PopulateForm()
        {
            TitleTextBox.Text = _editingGame.Title;
            DescriptionTextBox.Text = _editingGame.Description;
            PriceTextBox.Text = _editingGame.Price.ToString();
            ReleaseDatePicker.SelectedDate = _editingGame.ReleaseDate.HasValue ? _editingGame.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue) : null;
            CoverArtPathTextBox.Text = _editingGame.CoverArtPath;
            IsForSaleCheckBox.IsChecked = _editingGame.IsForSale;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate the price first
            if (!ValidatePrice())
            {
                return; // Don't save if validation fails
            }

            try
            {
                if (_editingGame == null) // We are in "Add" mode
                {
                    var newGame = new Game();
                    UpdateGameObject(newGame);
                    _gameService.AddGame(newGame);
                }
                else // We are in "Edit" mode
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

            // Check if price is empty
            if (string.IsNullOrWhiteSpace(priceText))
            {
                MessageBox.Show("Price cannot be empty. Please enter a valid price.", 
                               "Invalid Price", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                PriceTextBox.Focus();
                return false;
            }

            // Try to parse the price
            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Price must be a valid number. Please enter a valid price (e.g., 19.99).", 
                               "Invalid Price", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                PriceTextBox.Focus();
                PriceTextBox.SelectAll();
                return false;
            }

            // Check if price is negative
            if (price < 0)
            {
                MessageBox.Show("Price cannot be negative. Please enter a valid positive price.", 
                               "Invalid Price", 
                               MessageBoxButton.OK, 
                               MessageBoxImage.Warning);
                PriceTextBox.Focus();
                PriceTextBox.SelectAll();
                return false;
            }

            // Check if price is unreasonably high (optional validation)
            if (price > 999.99m)
            {
                var result = MessageBox.Show("The price seems unusually high. Are you sure you want to set the price to " + price.ToString("C") + "?", 
                                           "High Price Warning", 
                                           MessageBoxButton.YesNo, 
                                           MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    PriceTextBox.Focus();
                    PriceTextBox.SelectAll();
                    return false;
                }
            }

            return true;
        }

        // Helper method to update a game object from the form
        private void UpdateGameObject(Game game)
        {
            game.Title = TitleTextBox.Text;
            game.Description = DescriptionTextBox.Text;
            game.Price = decimal.TryParse(PriceTextBox.Text, out var price) ? price : 0;
            game.CoverArtPath = CoverArtPathTextBox.Text;
            game.IsForSale = IsForSaleCheckBox.IsChecked ?? false;

            if (ReleaseDatePicker.SelectedDate.HasValue)
            {
                game.ReleaseDate = DateOnly.FromDateTime(ReleaseDatePicker.SelectedDate.Value);
            }
            else
            {
                game.ReleaseDate = null;
            }
        }
    }
}