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
using System;

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